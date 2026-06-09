using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // #if defined(__arm64__)
    // _STRUCT_ARM_NEON_STATE64
    // {
    //     __uint128_t       __v[32];
    //     __uint32_t        __fpsr;
    //     __uint32_t        __fpcr;
    // };
    // 
    // _STRUCT_ARM_NEON_STATE
    // {
    //     __uint128_t       __v[16];
    //     __uint32_t        __fpsr;
    //     __uint32_t        __fpcr;
    // };
    // 
    public sealed class Arm64NeonState
    {
        public UInt128[] V { get; }
        public UInt32 FPSR { get; }
        public UInt32 FPCR { get; }


        internal Arm64NeonState(
            byte[] filebytes,
            ref UInt32 offset,
            bool is_bigendian,
            bool is64bit)
        {
            if (is64bit)
                V = new UInt128[32];
            else
                V = new UInt128[16];

            if (is_bigendian)
            {
                for (UInt32 i = 0; i < (UInt32)V.Length; i++)
                {
                    var high = BinaryHelper.ToUInt64Big(filebytes, offset);
                    var low = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                    V[i] = new UInt128(low, high);
                    offset += 16u;
                }

                FPSR = BinaryHelper.ToUInt32Big(filebytes, offset);
                FPCR = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                offset += 8u;
            }
            else
            {
                for (UInt32 i = 0; i < (UInt32)V.Length; i++)
                {
                    var low = BinaryHelper.ToUInt64(filebytes, offset);
                    var high = BinaryHelper.ToUInt64(filebytes, offset + 8);
                    V[i] = new UInt128(low, high);
                    offset += 16u;
                }

                FPSR = BinaryHelper.ToUInt32(filebytes, offset);
                FPCR = BinaryHelper.ToUInt32(filebytes, offset + 4);
                offset += 8u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{V={0}; FPSR={1}; FPCR={2}}}",
                V,
                FPSR,
                FPCR);
        }
    }
}
