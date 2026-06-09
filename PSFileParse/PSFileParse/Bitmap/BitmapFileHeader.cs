using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    //
    // struct BITMAPFILEHEADER {
    //     short bfType;
    //     int   bfSize;
    //     short bfReserved1;
    //     short bfReserved2;
    //     int bfOffBits;
    // };
    //
    public sealed class BitmapFileHeader
    {
        public FileSignature Type { get; }
        public Int32 Size { get; }
        public Int16 Reserved1 { get; }
        public Int16 Reserved2 { get; }
        public Int32 OffBits { get; }


        internal BitmapFileHeader(byte[] filebytes, UInt32 offset)
        {
            Type = new FileSignature(filebytes, offset, 2);
            Size = BinaryHelper.ToInt32(filebytes, offset + 2);
            Reserved1 = BinaryHelper.ToInt16(filebytes, offset + 6);
            Reserved2 = BinaryHelper.ToInt16(filebytes, offset + 8);
            OffBits = BinaryHelper.ToInt32(filebytes, offset + 10);
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Size={1}; Reserved1={2}; Reserved2={3}; OffBits={4}}}",
                this.Type.ToString(),
                this.Size,
                this.Reserved1,
                this.Reserved2,
                this.OffBits);
        }


        internal static byte[] GetHeaderBytes(UInt32 size, UInt32 offbits)
        {
            var header = new byte[]
            {
                0x42, 0x4D,             // Type
                0x00, 0x00, 0x00, 0x00, // Size
                0x00, 0x00,             // Reserved1
                0x00, 0x00,             // Reserved2
                0x00, 0x00, 0x00, 0x00  // OffBits
            };
            Buffer.BlockCopy(BitConverter.GetBytes(size), 0, header, 2, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(offbits), 0, header, 10, 4);

            return header;
        }
    }
}
