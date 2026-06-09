using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_ARM_THREAD_STATE
    // {
    //     __uint32_t r[13];
    //     __uint32_t sp;
    //     __uint32_t lr;
    //     __uint32_t pc;
    //     __uint32_t cpsr;
    // };
    // 
    public sealed class ArmThreadState
    {
        public UInt32[] R { get; }
        public UInt32 SP { get; }
        public UInt32 LR { get; }
        public UInt32 PC { get; }
        public UInt32 CPSR { get; }


        internal ArmThreadState(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            R = new UInt32[13];

            if (is_bigendian)
            {
                for (UInt32 i = 0u; i < 13u; i++)
                {
                    R[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                    offset += 4u;
                }

                SP = BinaryHelper.ToUInt32Big(filebytes, offset);
                LR = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                PC = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                CPSR = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            }
            else
            {
                for (UInt32 i = 0u; i < 13u; i++)
                {
                    R[i] = BinaryHelper.ToUInt32(filebytes, offset);
                    offset += 4u;
                }

                SP = BinaryHelper.ToUInt32(filebytes, offset);
                LR = BinaryHelper.ToUInt32(filebytes, offset + 4);
                PC = BinaryHelper.ToUInt32(filebytes, offset + 8);
                CPSR = BinaryHelper.ToUInt32(filebytes, offset + 12);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{R={0}; SP={1}; LR={2}; PC={3}; CPSR={4}}}",
                R,
                SP,
                LR,
                PC,
                CPSR);
        }
    }
}
