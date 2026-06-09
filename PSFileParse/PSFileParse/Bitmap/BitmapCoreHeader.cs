using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    //
    // struct BITMAPCOREHEADER {
    //     int   bcSize;
    //     short bcWidth;
    //     short bcHeight;
    //     short bcPlanes;
    //     short bcBitCount;
    // };
    //
    public sealed class BitmapCoreHeader
    {
        public Int32 Size { get; }
        public Int16 Width { get; }
        public Int16 Height { get; }
        public Int16 Planes { get; }
        public Int16 BitCount { get; }


        internal BitmapCoreHeader(byte[] filebytes, UInt32 offset)
        {
            if (filebytes.Length < offset + 0xC)
                throw new Exception("Input file is too small.");

            Size = BinaryHelper.ToInt32(filebytes, offset);
            Width = BinaryHelper.ToInt16(filebytes, offset + 4);
            Height = BinaryHelper.ToInt16(filebytes, offset + 6);
            Planes = BinaryHelper.ToInt16(filebytes, offset + 8);
            BitCount = BinaryHelper.ToInt16(filebytes, offset + 10);

            if (Size != 0xC)
                throw new Exception("Invalid header size.");
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; Width={1}; Height={2}; Planes={3}; BitCount={4}}}",
                this.Size,
                this.Width,
                this.Height,
                this.Planes,
                this.BitCount);
        }


        public byte[] GetBytes()
        {
            var header = new byte[0xC];
            Buffer.BlockCopy(BitConverter.GetBytes(this.Size), 0, header, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Width), 0, header, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Height), 0, header, 6, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Planes), 0, header, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(this.BitCount), 0, header, 10, 2);

            return header;
        }
    }
}
