using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_PPC_THREAD_STATE
    // {
    //     uint32_t srr0;
    //     uint32_t srr1;
    //     uint32_t r0;
    //     uint32_t r1;
    //     uint32_t r2;
    //     uint32_t r3;
    //     uint32_t r4;
    //     uint32_t r5;
    //     uint32_t r6;
    //     uint32_t r7;
    //     uint32_t r8;
    //     uint32_t r9;
    //     uint32_t r10;
    //     uint32_t r11;
    //     uint32_t r12;
    //     uint32_t r13;
    //     uint32_t r14;
    //     uint32_t r15;
    //     uint32_t r16;
    //     uint32_t r17;
    //     uint32_t r18;
    //     uint32_t r19;
    //     uint32_t r20;
    //     uint32_t r21;
    //     uint32_t r22;
    //     uint32_t r23;
    //     uint32_t r24;
    //     uint32_t r25;
    //     uint32_t r26;
    //     uint32_t r27;
    //     uint32_t r28;
    //     uint32_t r29;
    //     uint32_t r30;
    //     uint32_t r31;
    //     uint32_t ct;
    //     uint32_t xer;
    //     uint32_t lr;
    //     uint32_t ctr;
    //     uint32_t mq;
    //     uint32_t vrsave;
    // };
    // 
    public sealed class PPCThreadState
    {
        public UInt32[] SRR { get; }
        public UInt32[] R { get; }
        public UInt32 CT;
        public UInt32 XER;
        public UInt32 LR;
        public UInt32 CTR;
        public UInt32 MQ;
        public UInt32 VRSAVE;


        internal PPCThreadState(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            SRR = new UInt32[2];
            R = new UInt32[32];

            if (is_bigendian)
            {
                for (UInt32 i = 0u; i < (UInt32)SRR.Length; i++)
                {
                    SRR[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                    offset += 4u;
                }

                for (UInt32 i = 0u; i < (UInt32)R.Length; i++)
                {
                    R[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                    offset += 4u;
                }

                CT = BinaryHelper.ToUInt32Big(filebytes, offset);
                XER = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                LR = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                CTR = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                MQ = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                VRSAVE = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
            }
            else
            {
                for (UInt32 i = 0u; i < (UInt32)SRR.Length; i++)
                {
                    SRR[i] = BinaryHelper.ToUInt32(filebytes, offset);
                    offset += 4u;
                }

                for (UInt32 i = 0u; i < (UInt32)R.Length; i++)
                {
                    R[i] = BinaryHelper.ToUInt32(filebytes, offset);
                    offset += 4u;
                }

                CT = BinaryHelper.ToUInt32(filebytes, offset);
                XER = BinaryHelper.ToUInt32(filebytes, offset + 4);
                LR = BinaryHelper.ToUInt32(filebytes, offset + 8);
                CTR = BinaryHelper.ToUInt32(filebytes, offset + 12);
                MQ = BinaryHelper.ToUInt32(filebytes, offset + 16);
                VRSAVE = BinaryHelper.ToUInt32(filebytes, offset + 20);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{SRR={0}; R={1}; CT={2}; XER={3}; LR={4}; CTR={5}; MQ={6}; VRSAVE={7}}}",
                SRR,
                R,
                CT,
                XER,
                LR,
                CTR,
                MQ,
                VRSAVE);
        }
    }
}
