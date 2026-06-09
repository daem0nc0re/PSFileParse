using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_RUNTIME_FUNCTION_ENTRY_X64 {
    //     int BeginAddress;
    //     struct {
    //         PrologLength : 8;
    //         FunctionLength : 22;
    //         Is32bit : 1;
    //         ExceptionHandlerExists : 1;
    //     };
    // };
    // 
    public sealed class ImageRuntimeFunctionEntryArm
    {
        public UInt32 BeginAddress { get; }
        public UInt32 PrologLength { get; }
        public UInt32 FunctionLength { get; }
        public bool Is32Bit { get; }
        public bool ExceptionHandlerExists { get; }
        internal static readonly UInt32 SizeOfStruct = 0x8;


        internal ImageRuntimeFunctionEntryArm(byte[] filebytes, UInt32 offset)
        {
            var union_val = BinaryHelper.ToUInt32(filebytes, offset + 4);
            BeginAddress = BinaryHelper.ToUInt32(filebytes, offset);
            PrologLength = union_val & 0xF;
            FunctionLength = (union_val >> 8) & 0x003FFFFFu;
            Is32Bit = ((union_val >> 30) & 1) == 1;
            ExceptionHandlerExists = ((union_val >> 31) & 1) == 1;
        }


        public override String ToString()
        {
            return String.Format("@{{BeginAddress={0}; PrologLength={1}; FunctionLength=0x{2}; Is32Bit={3}; ExceptionHandlerExists={4}}}",
                BeginAddress,
                PrologLength,
                FunctionLength,
                Is32Bit,
                ExceptionHandlerExists);
        }
    }
}
