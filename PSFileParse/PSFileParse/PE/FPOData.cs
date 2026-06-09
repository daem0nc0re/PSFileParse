using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // #define FRAME_FPO       0
    // #define FRAME_TRAP      1
    // #define FRAME_TSS       2
    // #define FRAME_NONFPO    3
    // 
    // struct FPO_DATA {
    //     int   ulOffStart;   // offset 1st byte of function code
    //     int   cbProcSize;   // # bytes in function
    //     int   cdwLocals;    // # bytes in locals/4
    //     short cdwParams;    // # bytes in params/4
    //     short cbProlog : 8; // # bytes in prolog
    //     short cbRegs   : 3; // # regs saved
    //     short fHasSEH  : 1; // TRUE if SEH in func
    //     short fUseBP   : 1; // TRUE if EBP has been allocated
    //     short reserved : 1; // reserved for future use
    //     short cbFrame  : 2; // frame type
    // };
    // 
    public sealed class FPOData
    {
        public UInt32 StartOffset { get; }
        public UInt32 FunctionSize { get; }
        public UInt32 NumberOfLocals { get; }
        public UInt16 NumberOfParams { get; }
        public UInt16 NumberOfPrologBytes { get; }
        public UInt16 NumberOfRegisters { get; }
        public bool HasSEH { get; }
        public bool UseEBP { get; }
        public FPOFrameType FrameType { get; }
        internal static readonly UInt32 SizeOfStruct = 0x10;


        internal FPOData(byte[] filebytes, UInt32 offset)
        {
            StartOffset = BinaryHelper.ToUInt32(filebytes, offset);
            FunctionSize = BinaryHelper.ToUInt32(filebytes, offset + 4);
            NumberOfLocals = BinaryHelper.ToUInt32(filebytes, offset + 8);
            NumberOfParams = BinaryHelper.ToUInt16(filebytes, offset + 12);
            NumberOfPrologBytes = (UInt16)filebytes[offset + 14];
            NumberOfRegisters = (UInt16)(filebytes[offset + 15] & 7);
            HasSEH = ((filebytes[offset + 15] >> 3) & 1) == 1;
            UseEBP = ((filebytes[offset + 15] >> 4) & 1) == 1;
            FrameType = (FPOFrameType)((filebytes[offset + 15] >> 6) & 3);
        }


        public override String ToString()
        {
            return String.Format("@{{StartOffset={0}; FunctionSize={1}; NumberOfLocals={2}; NumberOfParams={3}; NumberOfPrologBytes={4}; NumberOfRegisters={5}; HasSEH={6}; UseEBP={7}; FrameType={8}}}",
                StartOffset,
                FunctionSize,
                NumberOfLocals,
                NumberOfParams,
                NumberOfPrologBytes,
                NumberOfRegisters,
                HasSEH,
                UseEBP,
                FrameType);
        }
    }
}
