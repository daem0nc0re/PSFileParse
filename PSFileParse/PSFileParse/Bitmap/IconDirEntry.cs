using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    // 
    // struct ICONDIRENTRY
    // {
    //     unsigned char bWidth;          // Width, in pixels, of the image
    //     unsigned char bHeight;         // Height, in pixels, of the image
    //     unsigned char bColorCount;     // Number of colors in image (0 if >=8bpp)
    //     unsigned char bReserved;       // Reserved ( must be 0)
    //     short         wPlanes;         // Color Planes
    //     short         wBitCount;       // Bits per pixel
    //     int           dwBytesInRes;    // How many bytes in this resource?
    //     int           dwImageOffset;   // Where in the file is this image?
    // };
    // 
    public sealed class IconDirEntry
    {
        public byte Width { get; }
        public byte Height { get; }
        public byte ColorCount { get; }
        public byte Reserved { get; }
        public UInt16 Planes { get; }
        public UInt16 BitCount { get; }
        public UInt32 BytesInRes { get; }
        public UInt32 ImageOffset { get; }


        internal IconDirEntry(byte[] filebytes, UInt32 offset)
        {
            Width = filebytes[offset];
            Height = filebytes[offset + 1];
            ColorCount = filebytes[offset + 2];
            Reserved = filebytes[offset + 3];
            Planes = BinaryHelper.ToUInt16(filebytes, offset + 4);
            BitCount = BinaryHelper.ToUInt16(filebytes, offset + 6);
            BytesInRes = BinaryHelper.ToUInt32(filebytes, offset + 8);
            ImageOffset = BinaryHelper.ToUInt32(filebytes, offset + 12);
        }


        public override String ToString()
        {
            return String.Format("@{{Width={0}; Height={1}; ColorCount={2}; Reserved={3}; Planes={4}; BitCount={5}; BytesInRes={6}; ImageOffset={7}}}",
                this.Width,
                this.Height,
                this.ColorCount,
                this.Reserved,
                this.Planes,
                this.BitCount,
                this.BytesInRes,
                this.ImageOffset);
        }
    }
}
