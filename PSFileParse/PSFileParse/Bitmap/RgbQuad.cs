using System;

namespace PSFileParse.Bitmap
{
    // 
    // struct RGBQUAD
    // {
    //     unsigned char rgbBlue;
    //     unsigned char rgbGreen;
    //     unsigned char rgbRed;
    //     unsigned char rgbReserved;
    // };
    // 
    public sealed class RgbQuad
    {
        public byte Blue { get; }
        public byte Green { get; }
        public byte Red { get; }
        public byte Reserved { get; }


        internal RgbQuad(byte[] filebytes, UInt32 offset)
        {
            Blue = filebytes[offset];
            Green = filebytes[offset + 1];
            Red = filebytes[offset + 2];
            Reserved = filebytes[offset + 3];
        }


        public override String ToString()
        {
            return String.Format("@{{Blue={0}; Green={1}; Red={2}; Reserved={3}}}",
                this.Blue,
                this.Green,
                this.Red,
                this.Reserved);
        }


        public byte[] GetBytes()
        {
            return new byte[] { Blue, Green, Red, Reserved };
        }
    }
}
