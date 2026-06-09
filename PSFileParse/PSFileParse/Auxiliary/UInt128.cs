using System;

namespace PSFileParse.Auxiliary
{
    public sealed class UInt128
    {
        public UInt64 LowPart { get; }
        public UInt64 HighPart { get; }


        public UInt128(ulong lowPart, ulong highPart)
        {
            LowPart = lowPart;
            HighPart = highPart;
        }


        public override String ToString()
        {
            return String.Format("@{{LowPart={0}; HighPart={1}}}",
                LowPart,
                HighPart);
        }
    }
}
