using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_RUNTIME_FUNCTION_ENTRY_MIPS32 {
    //     int BeginAddress;
    //     int EndAddress;
    //     int ExceptionHandler;
    //     int HandlerData;
    //     int PrologEndAddress;
    // };
    // 
    public sealed class ImageRuntimeFunctionEntryMips32
    {
        public UInt32 BeginAddress { get; }
        public UInt32 EndAddress { get; }
        public UInt32 ExceptionHandler { get; }
        public UInt32 HandlerData { get; }
        public UInt32 PrologEndAddress { get; }
        internal static readonly UInt32 SizeOfStruct = 0x14;


        internal ImageRuntimeFunctionEntryMips32(byte[] filebytes, UInt32 offset)
        {
            BeginAddress = BinaryHelper.ToUInt32(filebytes, offset);
            EndAddress = BinaryHelper.ToUInt32(filebytes, offset + 4);
            ExceptionHandler = BinaryHelper.ToUInt32(filebytes, offset + 8);
            HandlerData = BinaryHelper.ToUInt32(filebytes, offset + 12);
            PrologEndAddress = BinaryHelper.ToUInt32(filebytes, offset + 16);
        }


        public override String ToString()
        {
            return String.Format("@{{BeginAddress={0}; EndAddress={1}; ExceptionHandler={2}; HandlerData={3}; PrologEndAddress={4}}}",
                BeginAddress,
                EndAddress,
                ExceptionHandler,
                HandlerData,
                PrologEndAddress);
        }
    }
}
