using System;

namespace PSFileParse.Bitmap
{
    public class BmpFile
    {
        public String FileName { get; }
        public UInt32 FileSize { get; }
        public BitmapFileHeader FileHeader { get; }
        public Object InfoHeader { get; }
        public Object[] Colors { get; }
        public byte[] Content { get; }
        private UInt32 ContentOffset { get; }
        private bool IsCoreHeader { get; }


        internal BmpFile(String filename, byte[] filebytes)
        {
            if (filebytes.Length < 0x1A)
                throw new Exception("File size is too small");

            UInt32 content_offset;
            var magic = BitConverter.ToInt32(filebytes, 14);
            FileName = filename;
            FileSize = (UInt32)filebytes.Length;
            FileHeader = new BitmapFileHeader(filebytes, 0);
            IsCoreHeader = false;

            if (magic == 0x28)
            {
                Int32 colors_num;
                var info_header = new BitmapInfoHeader(filebytes, 14);

                if ((info_header.BitCount <= 8) &&
                    (info_header.Compression == BitmapCompressionType.Rgb))
                {
                    if (info_header.ClrUsed == 0)
                        colors_num = 1 << info_header.BitCount;
                    else
                        colors_num = 1 << (Int32)info_header.ClrUsed;
                }
                else
                {
                    colors_num = (info_header.Compression == BitmapCompressionType.BitFields) ? 3 : 0;  
                }

                content_offset = 0x36u + (UInt32)(4 * colors_num);
                InfoHeader = info_header;

                if ((UInt32)filebytes.Length < content_offset)
                    throw new Exception("File size is too small");

                if (colors_num > 0)
                {
                    var colors_offset = 0x36u;
                    Colors = new RgbQuad[colors_num];

                    for (UInt32 i = 0; i < colors_num; i++)
                    {
                        Colors[i] = new RgbQuad(filebytes, colors_offset);
                        colors_offset += 4;
                    }
                }
            }
            else if (magic == 0xC)
            {
                Int32 colors_num;
                var core_header = new BitmapCoreHeader(filebytes, 14);
                IsCoreHeader = true;

                if (core_header.BitCount == 1)
                    colors_num = 2;
                else if (core_header.BitCount == 4)
                    colors_num = 16;
                else if (core_header.BitCount == 8)
                    colors_num = 256;
                else if (core_header.BitCount == 24)
                    colors_num = 0;
                else
                    throw new Exception("Invalid BitCount.");

                content_offset = 0x1Au + (UInt32)(3 * colors_num);
                InfoHeader = core_header;

                if ((UInt32)filebytes.Length < content_offset)
                    throw new Exception("File size is too small");

                if (colors_num > 0)
                {
                    var colors_offset = 0xCu;
                    Colors = new RgbTriple[colors_num];

                    for (UInt32 i = 0; i < colors_num; i++)
                    {
                        Colors[i] = new RgbTriple(filebytes, colors_offset);
                        colors_offset += 3;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid file magic.");
            }

            Content = new byte[FileSize - content_offset];
            ContentOffset = content_offset;

            for (UInt32 i = 0u; i < (FileSize - content_offset); i++)
                Content[i] = filebytes[content_offset + i];
        }


        public byte[] ToDibBytes()
        {
            var total_size = this.FileSize - 14;
            var offset = 0;
            var filebytes = new byte[total_size];

            if (this.IsCoreHeader)
            {
                var info_header = ((BitmapCoreHeader)this.InfoHeader).GetBytes();
                Buffer.BlockCopy(info_header, 0, filebytes, offset, info_header.Length);
                offset += info_header.Length;

                if (this.Colors != null)
                {
                    for (Int32 i = 0; i < this.Colors.Length; i++)
                    {
                        var color = ((RgbTriple)this.Colors[i]).GetBytes();
                        Buffer.BlockCopy(color, 0, filebytes, offset, color.Length);
                        offset += color.Length;
                    }
                }

                Buffer.BlockCopy(this.Content, 0, filebytes, offset, this.Content.Length);
            }
            else
            {
                var info_header = ((BitmapInfoHeader)this.InfoHeader).GetBytes();
                Buffer.BlockCopy(BitConverter.GetBytes((Int32)0), 0, info_header, 20, 4);
                Buffer.BlockCopy(info_header, 0, filebytes, offset, info_header.Length);
                offset += info_header.Length;

                if (this.Colors != null)
                {
                    for (Int32 i = 0; i < this.Colors.Length; i++)
                    {
                        var color = ((RgbQuad)this.Colors[i]).GetBytes();
                        Buffer.BlockCopy(color, 0, filebytes, offset, color.Length);
                        offset += color.Length;
                    }
                }

                Buffer.BlockCopy(this.Content, 0, filebytes, offset, this.Content.Length);
            }

            return filebytes;
        }
    }
}
