using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    // 
    // struct BITMAPINFOHEADER
    // {
    //   int   biSize;
    //   int   biWidth;
    //   int   biHeight;
    //   short biPlanes;
    //   short biBitCount;
    //   int   biCompression;
    //   int   biSizeImage;
    //   int   biXPelsPerMeter;
    //   int   biYPelsPerMeter;
    //   int   biClrUsed;
    //   int   biClrImportant;
    // };
    // 
    public sealed class BitmapInfoHeader
    {
        public UInt32 Size { get; }
        public UInt32 Width { get; }
        public UInt32 Height { get; }
        public UInt16 Planes { get; }
        public UInt16 BitCount { get; }
        public BitmapCompressionType Compression { get; }
        public UInt32 SizeImage { get; }
        public UInt32 XPelsPerMeter { get; }
        public UInt32 YPelsPerMeter { get; }
        public UInt32 ClrUsed { get; }
        public UInt32 ClrImportant { get; }


        public BitmapInfoHeader(byte[] filebytes, UInt32 offset)
        {
            if (filebytes.Length < offset + 0x28)
                throw new Exception("Input file is too small.");

            Size = BinaryHelper.ToUInt32(filebytes, offset);
            Width = BinaryHelper.ToUInt32(filebytes, offset + 4);
            Height = BinaryHelper.ToUInt32(filebytes, offset + 8);
            Planes = BinaryHelper.ToUInt16(filebytes, offset + 12);
            BitCount = BinaryHelper.ToUInt16(filebytes, offset + 14);
            Compression = (BitmapCompressionType)BinaryHelper.ToUInt32(filebytes, offset + 16);
            SizeImage = BinaryHelper.ToUInt32(filebytes, offset + 20);
            XPelsPerMeter = BinaryHelper.ToUInt32(filebytes, offset + 24);
            YPelsPerMeter = BinaryHelper.ToUInt32(filebytes, offset + 28);
            ClrUsed = BinaryHelper.ToUInt32(filebytes, offset + 32);
            ClrImportant = BinaryHelper.ToUInt32(filebytes, offset + 36);

            if (Size != 0x28)
                throw new Exception("Invalid header size.");
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; Width={1}; Height={2}; Planes={3}; BitCount={4}; Compression={5}; SizeImage={6}; XPelsPerMeter={7}; YPelsPerMeter={8}; XClrUsed={9}; ClrImportant={10}}}",
                this.Size,
                this.Width,
                this.Height,
                this.Planes,
                this.BitCount,
                this.Compression,
                this.SizeImage,
                this.XPelsPerMeter,
                this.YPelsPerMeter,
                this.ClrUsed,
                this.ClrImportant);
        }


        public byte[] GetBytes()
        {
            var header = new byte[0x28];
            Buffer.BlockCopy(BitConverter.GetBytes(this.Size), 0, header, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Width), 0, header, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Height), 0, header, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Planes), 0, header, 12, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.BitCount), 0, header, 14, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((Int32)this.Compression), 0, header, 16, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.SizeImage), 0, header, 20, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.XPelsPerMeter), 0, header, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.YPelsPerMeter), 0, header, 28, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.ClrUsed), 0, header, 32, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.ClrImportant), 0, header, 36, 4);

            return header;
        }
    }
}
