using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_ARM_THREAD_STATE64
    // {
    //     __uint64_t x[29];
    //     __uint64_t fp;
    //     __uint64_t lr;
    //     __uint64_t sp;
    //     __uint64_t pc;
    //     __uint32_t cpsr;
    //     __uint32_t __pad;
    // };
    // 
    public sealed class ArmThreadState64
    {
        public UInt64[] X { get; }
        public UInt64 FP { get; }
        public UInt64 LR { get; }
        public UInt64 SP { get; }
        public UInt64 PC { get; }
        public UInt32 CPSR { get; }
        public UInt32 Padding { get; }


        internal ArmThreadState64(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            X = new UInt64[29];

            if (is_bigendian)
            {
                for (UInt32 i = 0u; i < 29u; i++)
                {
                    X[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                    offset += 8u;
                }

                FP = BinaryHelper.ToUInt64Big(filebytes, offset);
                LR = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                SP = BinaryHelper.ToUInt64Big(filebytes, offset + 16);
                PC = BinaryHelper.ToUInt64Big(filebytes, offset + 24);
                CPSR = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                Padding = BinaryHelper.ToUInt32Big(filebytes, offset + 40);
            }
            else
            {
                for (UInt32 i = 0u; i < 29u; i++)
                {
                    X[i] = BinaryHelper.ToUInt64(filebytes, offset);
                    offset += 8u;
                }

                FP = BinaryHelper.ToUInt64(filebytes, offset);
                LR = BinaryHelper.ToUInt64(filebytes, offset + 8);
                SP = BinaryHelper.ToUInt64(filebytes, offset + 16);
                PC = BinaryHelper.ToUInt64(filebytes, offset + 24);
                CPSR = BinaryHelper.ToUInt32(filebytes, offset + 32);
                Padding = BinaryHelper.ToUInt32(filebytes, offset + 40);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{X={0}; FP={1}; LR={2}; SP={3}; PC={4}; CPSR={5}; Padding={6}}}",
                X,
                FP,
                LR,
                SP,
                PC,
                CPSR,
                Padding);
        }
    }
}
