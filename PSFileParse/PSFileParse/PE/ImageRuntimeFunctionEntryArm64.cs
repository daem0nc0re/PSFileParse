using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_RUNTIME_FUNCTION_ENTRY_ARM64 {
    //     int StartAddress;
    //     struct {
    //         Reserved : 1;
    //         IsPackedUnwindData : 1;
    //         union {
    //             ExceptionInformationRVA : 30;
    //             FunctionLength : 30;
    //         };
    //     };
    // };
    // 
    public sealed class ImageRuntimeFunctionEntryArm64
    {
        public UInt32 StartAddress { get; }
        public UInt32 Information { get; }
        public bool IsPackedUnwindData { get; }
        internal static readonly UInt32 SizeOfStruct = 0x8;


        internal ImageRuntimeFunctionEntryArm64(byte[] filebytes, UInt32 offset)
        {
            var data = BinaryHelper.ToUInt32(filebytes, offset + 4);
            StartAddress = BinaryHelper.ToUInt32(filebytes, offset);
            Information = (data >> 2) & 0x3FFFFFFFu;
            IsPackedUnwindData = ((data >> 1) & 1) == 1;
        }


        public override String ToString()
        {
            return String.Format("@{{StartAddress={0}; Information={1}; IsPackedUnwindData={2}}}",
                StartAddress,
                Information,
                IsPackedUnwindData);
        }
    }
}
