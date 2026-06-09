using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    //
    // struct TRIVERTEX
    // {
    //     int            x;
    //     int            y;
    //     unsigned short Red;
    //     unsigned short Green;
    //     unsigned short Blue;
    //     unsigned short Alpha;
    // };
    //
    public sealed class Trivertex
    {
        public Int32 X { get; }
        public Int32 Y { get; }
        public UInt16 Red { get; }
        public UInt16 Green { get; }
        public UInt16 Blue { get; }
        public UInt16 Alpha { get; }


        internal Trivertex(byte[] filebytes, UInt32 offset)
        {
            X = BinaryHelper.ToInt32(filebytes, offset);
            Y = BinaryHelper.ToInt32(filebytes, offset + 4);
            Red = BinaryHelper.ToUInt16(filebytes, offset + 8);
            Green = BinaryHelper.ToUInt16(filebytes, offset + 10);
            Blue = BinaryHelper.ToUInt16(filebytes, offset + 12);
            Alpha = BinaryHelper.ToUInt16(filebytes, offset + 14);
        }


        public override String ToString()
        {
            return String.Format("@{{X={0}; Y={1}; Red={2}; Green={3}; Blue={4}; Alpha={5}}}",
                this.X,
                this.Y,
                this.Red,
                this.Green,
                this.Blue,
                this.Alpha);
        }
    }
}
