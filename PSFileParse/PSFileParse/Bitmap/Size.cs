using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    //
    // struct SIZE
    // {
    //     int cx;
    //     int cy;
    // };
    //
    public sealed class Size
    {
        public Int32 X { get; }
        public Int32 Y { get; }


        internal Size(byte[] filebytes, UInt32 offset)
        {
            X = BinaryHelper.ToInt32(filebytes, offset);
            Y = BinaryHelper.ToInt32(filebytes, offset + 4);
        }


        public override String ToString()
        {
            return String.Format("@{{X={0}; Y={1}}}", this.X, this.Y);
        }
    }
}
