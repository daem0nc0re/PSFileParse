using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public class PInvokeAttributeFlags
    {
        public PInvokeNameFlags Mangle { get; }
        public PInvokeCharSet CharSet { get; }
        public PInvokeLastErrorSupportFlags ErrorSupport { get; }
        public PInvokeCallConvFlags CallingConvention { get; }


        internal PInvokeAttributeFlags(byte[] filebytes, ref UInt32 offset)
        {
            var attributes = BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;
            Mangle = (PInvokeNameFlags)(attributes & 0x1);
            CharSet = (PInvokeCharSet)(attributes & 0x6);
            ErrorSupport = (PInvokeLastErrorSupportFlags)(attributes & 0x40);
            CallingConvention = (PInvokeCallConvFlags)(attributes & 0x0700);
        }


        public override String ToString()
        {
            return String.Format("@{{Mangle={0}; CharSet={1}; ErrorSupport={2}; CallingConvention={3}}}",
                Mangle,
                CharSet,
                ErrorSupport,
                CallingConvention);
        }
    }
}
