using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.Bitmap
{
    //
    // struct GRADIENT_TRIANGLE
    // {
    //     unsigned int Vertex1;
    //     unsigned int Vertex2;
    //     unsigned int Vertex3;
    // };
    //
    public sealed class GradientTriangle
    {
        public UInt32 Vertex1 { get; }
        public UInt32 Vertex2 { get; }
        public UInt32 Vertex3 { get; }


        internal GradientTriangle(byte[] filebytes, UInt32 offset)
        {
            Vertex1 = BinaryHelper.ToUInt32(filebytes, offset);
            Vertex2 = BinaryHelper.ToUInt32(filebytes, offset + 4);
            Vertex3 = BinaryHelper.ToUInt32(filebytes, offset + 8);
        }


        public override String ToString()
        {
            return String.Format("@{{Vertex1={0}; Vertex2={1}; Vertex3={2}}}",
                this.Vertex1,
                this.Vertex2,
                this.Vertex3);
        }
    }
}
