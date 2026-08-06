using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // typedef struct __SC_GenericBlob {
    //     uint32_t magic;
    //     uint32_t length;
    //     char data[];
    // } CS_GenericBlob
    // 
    public sealed class CSGenericBlobs
    {
        public CSMagic Magic { get; }
        public UInt32 Length { get; }
        public byte[] Data { get; }


        internal CSGenericBlobs(byte[] filebytes, UInt32 offset)
        {
            Magic = (CSMagic)BinaryHelper.ToUInt32Big(filebytes, offset);
            Length = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            Data = new byte[Length - 8];
            Array.Copy(filebytes, offset + 8, Data, 0, Data.Length);
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; Length={1}; Data={2}}}",
                Magic,
                Length,
                Data);
        }
    }
}
