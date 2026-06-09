using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    //
    // struct GRADIENT_RECT
    // {
    //     unsigned int UpperLeft;
    //     unsigned int LowerRight;
    // };
    //
    public sealed class GradientRect
    {
        public UInt32 UpperLeft { get; }
        public UInt32 LowerRight { get; }


        internal GradientRect(byte[] filebytes, UInt32 offset)
        {
            UpperLeft = BinaryHelper.ToUInt32(filebytes, offset);
            LowerRight = BinaryHelper.ToUInt32(filebytes, offset + 4);
        }


        public override String ToString()
        {
            return String.Format("@{{UpperLeft={0}; LowerRight={1}}}",
                this.UpperLeft,
                this.LowerRight);
        }
    }
}
