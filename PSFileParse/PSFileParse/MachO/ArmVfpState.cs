using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_ARM_VFP_STATE
    // {
    //     __uint32_t        __r[64];
    //     __uint32_t        __fpscr;
    // };
    // 
    public sealed class ArmVfpState
    {
        public UInt32[] R { get; }
        public UInt32 FPSCR { get; }


        internal ArmVfpState(
            byte[] filebytes,
            ref UInt32 offset,
            bool is_bigendian)
        {
            R = new UInt32[64];

            for (UInt32 i = 0u; i < (UInt32)R.Length; i++)
            {
                if (is_bigendian)
                    R[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                else
                    R[i] = BinaryHelper.ToUInt32(filebytes, offset);

                offset += 4u;
            }

            if (is_bigendian)
                FPSCR = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                FPSCR = BinaryHelper.ToUInt32(filebytes, offset);

            offset += 4u;
        }


        public override String ToString()
        {
            return String.Format("@{{R={0}; FPSCR={1}}}", R, FPSCR);
        }
    }
}
