using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_RUNTIME_FUNCTION_ENTRY_X64 {
    //     int BeginAddress;
    //     int EndAddress;
    //     int UnwindInformation;
    // };
    // 
    public sealed class ImageRuntimeFunctionEntryX64
    {
        public UInt32 BeginAddress { get; }
        public UInt32 EndAddress { get; }
        public UInt32 UnwindInformation { get; }
        internal static readonly UInt32 SizeOfStruct = 0xC;


        internal ImageRuntimeFunctionEntryX64(byte[] filebytes, UInt32 offset)
        {
            BeginAddress = BinaryHelper.ToUInt32(filebytes, offset);
            EndAddress = BinaryHelper.ToUInt32(filebytes, offset + 4);
            UnwindInformation = BinaryHelper.ToUInt32(filebytes, offset + 8);
        }


        public override String ToString()
        {
            return String.Format("@{{BeginAddress={0}; EndAddress={1}; UnwindInformation={2}}}",
                BeginAddress,
                EndAddress,
                UnwindInformation);
        }
    }
}
