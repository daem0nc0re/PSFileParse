using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_PPC_THREAD_STATE64
    // {
    //     unsigned long long srr0;
    //     unsigned long long srr1;
    //     unsigned long long r0;
    //     unsigned long long r1;
    //     unsigned long long r2;
    //     unsigned long long r3;
    //     unsigned long long r4;
    //     unsigned long long r5;
    //     unsigned long long r6;
    //     unsigned long long r7;
    //     unsigned long long r8;
    //     unsigned long long r9;
    //     unsigned long long r10;
    //     unsigned long long r11;
    //     unsigned long long r12;
    //     unsigned long long r13;
    //     unsigned long long r14;
    //     unsigned long long r15;
    //     unsigned long long r16;
    //     unsigned long long r17;
    //     unsigned long long r18;
    //     unsigned long long r19;
    //     unsigned long long r20;
    //     unsigned long long r21;
    //     unsigned long long r22;
    //     unsigned long long r23;
    //     unsigned long long r24;
    //     unsigned long long r25;
    //     unsigned long long r26;
    //     unsigned long long r27;
    //     unsigned long long r28;
    //     unsigned long long r29;
    //     unsigned long long r30;
    //     unsigned long long r31;
    //     unsigned int cr;
    //     unsigned long long xer;
    //     unsigned long long lr;
    //     unsigned long long ctr;
    //     unsigned int vrsave;
    // };
    // 
    public sealed class PPCThreadState64
    {
        public UInt64[] SRR { get; }
        public UInt64[] R { get; }
        public UInt64 CR;
        public UInt64 XER;
        public UInt64 LR;
        public UInt64 CTR;
        public UInt64 MQ;
        public UInt64 VRSAVE;


        internal PPCThreadState64(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            SRR = new UInt64[2];
            R = new UInt64[32];

            if (is_bigendian)
            {
                for (UInt32 i = 0u; i < (UInt32)SRR.Length; i++)
                {
                    SRR[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                    offset += 8u;
                }

                for (UInt32 i = 0u; i < (UInt32)R.Length; i++)
                {
                    R[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                    offset += 8u;
                }

                CR = BinaryHelper.ToUInt64Big(filebytes, offset);
                XER = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                LR = BinaryHelper.ToUInt64Big(filebytes, offset + 16);
                CTR = BinaryHelper.ToUInt64Big(filebytes, offset + 24);
                MQ = BinaryHelper.ToUInt64Big(filebytes, offset + 32);
                VRSAVE = BinaryHelper.ToUInt64Big(filebytes, offset + 40);
            }
            else
            {
                for (UInt32 i = 0u; i < (UInt32)SRR.Length; i++)
                {
                    SRR[i] = BinaryHelper.ToUInt64(filebytes, offset);
                    offset += 8u;
                }

                for (UInt32 i = 0u; i < (UInt32)R.Length; i++)
                {
                    R[i] = BinaryHelper.ToUInt64(filebytes, offset);
                    offset += 8u;
                }

                CR = BinaryHelper.ToUInt64(filebytes, offset);
                XER = BinaryHelper.ToUInt64(filebytes, offset + 8);
                LR = BinaryHelper.ToUInt64(filebytes, offset + 16);
                CTR = BinaryHelper.ToUInt64(filebytes, offset + 24);
                MQ = BinaryHelper.ToUInt64(filebytes, offset + 32);
                VRSAVE = BinaryHelper.ToUInt64(filebytes, offset + 40);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{SRR={0}; R={1}; CR={2}; XER={3}; LR={4}; CTR={5}; MQ={6}; VRSAVE={7}}}",
                SRR,
                R,
                CR,
                XER,
                LR,
                CTR,
                MQ,
                VRSAVE);
        }
    }
}
