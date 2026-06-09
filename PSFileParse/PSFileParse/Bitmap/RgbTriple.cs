using System;

namespace PSFileParse.Bitmap
{
    // 
    // struct RGBTRIPLE
    // {
    //     unsigned char rgbBlue;
    //     unsigned char rgbGreen;
    //     unsigned char rgbRed;
    // };
    // 
    public class RgbTriple
    {
        public byte Blue { get; }
        public byte Green { get; }
        public byte Red { get; }


        internal RgbTriple(byte[] filebytes, UInt32 offset)
        {
            Blue = filebytes[offset];
            Green = filebytes[offset + 1];
            Red = filebytes[offset + 2];
        }


        public override String ToString()
        {
            return String.Format("@{{Blue={0}; Green={1}; Red={2}}}",
                this.Blue,
                this.Green,
                this.Red);
        }


        public byte[] GetBytes()
        {
            return new byte[] { Blue, Green, Red };
        }
    }
}
