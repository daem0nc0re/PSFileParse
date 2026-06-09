using System;

namespace PSFileParse.Bitmap
{
    public sealed class DibFile
    {
        public String FileName { get; }
        public UInt32 FileSize { get; }
        public Object Header { get; }
        public Object[] Colors { get; }
        public byte[] Content { get; }
        private UInt32 ContentOffset { get; }
        private bool IsCoreHeader { get; }


        internal DibFile(String filename, byte[] filebytes)
        {
            if (filebytes.Length < 0xC)
                throw new Exception("File size is too small");

            UInt32 content_offset;
            var magic = BitConverter.ToInt32(filebytes, 0);
            FileName = filename;
            FileSize = (UInt32)filebytes.Length;
            IsCoreHeader = false;

            if (magic == 0x28)
            {
                Int32 colors_num;
                var info_header = new BitmapInfoHeader(filebytes, 0);

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

                content_offset = 0x28u + (UInt32)(4 * colors_num);
                Header = info_header;

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
                var core_header = new BitmapCoreHeader(filebytes, 0);
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

                content_offset = 0xCu + (UInt32)(3 * colors_num);
                Header = core_header;

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


        public byte[] ToBmpBytes()
        {
            var total_size = this.FileSize + 14;
            var offset = 14;
            var file_header = BitmapFileHeader.GetHeaderBytes(this.FileSize + 14, this.ContentOffset + 14);
            var filebytes = new byte[total_size];
            Buffer.BlockCopy(file_header, 0, filebytes, 0, 14);

            if (this.IsCoreHeader)
            {
                var info_header = ((BitmapCoreHeader)this.Header).GetBytes();
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
                var info_header = ((BitmapInfoHeader)this.Header).GetBytes();
                Buffer.BlockCopy(BitConverter.GetBytes(total_size - 54), 0, info_header, 20, 4); // SizeImage
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


        public byte[] ToWinIconBytes()
        {
            var filebytes = new byte[22 + this.FileSize];
            var header_bytes = new byte[]
            {
                0x00, 0x00,             // 00: IconDir.Reserved must be 0
                0x01, 0x00,             // 02: IconDir.Type = 1 for icon
                0x01, 0x00,             // 04: IconDir.Count = 1
                0x00,                   // 06: IconDir.Entries[1].Width
                0x00,                   // 07: IconDir.Entries[1].Height
                0x00,                   // 08: IconDir.Entries[1].ColorCount
                0x00,                   // 09: IconDir.Entries[1].Reserved
                0x00, 0x00,             // 0A: IconDir.Entries[1].Planes
                0x00, 0x00,             // 0C: IconDir.Entries[1].BitCount
                0x00, 0x00, 0x00, 0x00, // 0E: IconDir.Entries[1].BytesInRes
                0x16, 0x00, 0x00, 0x00, // 12: IconDir.Entries[1].ImageOffset
            };

            if (this.IsCoreHeader)
            {
                var offset = 0x22;
                var info_header = (BitmapCoreHeader)this.Header;
                var info_bytes = info_header.GetBytes();
                header_bytes[0x6] = (byte)(info_header.Width & 0xFF);
                header_bytes[0x7] = (byte)(info_header.Height & 0xFF);
                Buffer.BlockCopy(BitConverter.GetBytes(info_header.Planes), 0, header_bytes, 0xA, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(info_header.BitCount), 0, header_bytes, 0xC, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(this.FileSize), 0, header_bytes, 0xE, 4);

                if ((info_header.BitCount < 8) && (this.Colors != null))
                    header_bytes[0x8] = (byte)(this.Colors.Length & 0xFF);

                Buffer.BlockCopy(header_bytes, 0, filebytes, 0, header_bytes.Length);
                Buffer.BlockCopy(info_bytes, 0, filebytes, 22, info_bytes.Length);

                if (this.Colors != null)
                {
                    foreach (RgbTriple colors in this.Colors)
                    {
                        Buffer.BlockCopy(colors.GetBytes(), 0, header_bytes, offset, 3);
                        offset += 3;
                    }
                }

                Buffer.BlockCopy(this.Content, 0, filebytes, offset, this.Content.Length);
            }
            else
            {
                var offset = 0x3E;
                var info_header = (BitmapInfoHeader)this.Header;
                var info_bytes = info_header.GetBytes();
                header_bytes[0x6] = (byte)(info_header.Width & 0xFF);
                header_bytes[0x7] = (byte)(info_header.Height & 0xFF);
                Buffer.BlockCopy(BitConverter.GetBytes(info_header.Planes), 0, header_bytes, 0xA, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(info_header.BitCount), 0, header_bytes, 0xC, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(this.FileSize), 0, header_bytes, 0xE, 4);

                if ((info_header.BitCount < 8) && (this.Colors != null))
                    header_bytes[0x8] = (byte)(this.Colors.Length & 0xFF);

                Buffer.BlockCopy(header_bytes, 0, filebytes, 0, header_bytes.Length);
                Buffer.BlockCopy(info_bytes, 0, filebytes, 22, info_bytes.Length);

                if (this.Colors != null)
                {
                    foreach (RgbQuad colors in this.Colors)
                    {
                        Buffer.BlockCopy(colors.GetBytes(), 0, header_bytes, offset, 4);
                        offset += 4;
                    }
                }

                Buffer.BlockCopy(this.Content, 0, filebytes, offset, this.Content.Length);
            }

            return filebytes;
        }
    }
}
