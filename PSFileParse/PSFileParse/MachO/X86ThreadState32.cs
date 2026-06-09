using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_X86_THREAD_STATE32
    // {
    //     unsigned int eax;
    //     unsigned int ebx;
    //     unsigned int ecx;
    //     unsigned int edx;
    //     unsigned int edi;
    //     unsigned int esi;
    //     unsigned int ebp;
    //     unsigned int esp;
    //     unsigned int ss;
    //     unsigned int eflags;
    //     unsigned int eip;
    //     unsigned int cs;
    //     unsigned int ds;
    //     unsigned int es;
    //     unsigned int fs;
    //     unsigned int gs;
    // };
    // 
    public sealed class X86ThreadState32
    {
        public UInt32 EAX { get; }
        public UInt32 EBX { get; }
        public UInt32 ECX { get; }
        public UInt32 EDX { get; }
        public UInt32 EDI { get; }
        public UInt32 ESI { get; }
        public UInt32 EBP { get; }
        public UInt32 ESP { get; }
        public UInt32 SS { get; }
        public UInt32 EFLAGS { get; }
        public UInt32 EIP { get; }
        public UInt32 CS { get; }
        public UInt32 DS { get; }
        public UInt32 ES { get; }
        public UInt32 FS { get; }
        public UInt32 GS { get; }


        internal X86ThreadState32(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                EAX = BinaryHelper.ToUInt32Big(filebytes, offset);
                EBX = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                ECX = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                EDX = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                EDI = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                ESI = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                EBP = BinaryHelper.ToUInt32Big(filebytes, offset + 24);
                ESP = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
                SS = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                EFLAGS = BinaryHelper.ToUInt32Big(filebytes, offset + 36);
                EIP = BinaryHelper.ToUInt32Big(filebytes, offset + 40);
                CS = BinaryHelper.ToUInt32Big(filebytes, offset + 44);
                DS = BinaryHelper.ToUInt32Big(filebytes, offset + 48);
                ES = BinaryHelper.ToUInt32Big(filebytes, offset + 52);
                FS = BinaryHelper.ToUInt32Big(filebytes, offset + 56);
                GS = BinaryHelper.ToUInt32Big(filebytes, offset + 60);
            }
            else
            {
                EAX = BinaryHelper.ToUInt32(filebytes, offset);
                EBX = BinaryHelper.ToUInt32(filebytes, offset + 4);
                ECX = BinaryHelper.ToUInt32(filebytes, offset + 8);
                EDX = BinaryHelper.ToUInt32(filebytes, offset + 12);
                EDI = BinaryHelper.ToUInt32(filebytes, offset + 16);
                ESI = BinaryHelper.ToUInt32(filebytes, offset + 20);
                EBP = BinaryHelper.ToUInt32(filebytes, offset + 24);
                ESP = BinaryHelper.ToUInt32(filebytes, offset + 28);
                SS = BinaryHelper.ToUInt32(filebytes, offset + 32);
                EFLAGS = BinaryHelper.ToUInt32(filebytes, offset + 36);
                EIP = BinaryHelper.ToUInt32(filebytes, offset + 40);
                CS = BinaryHelper.ToUInt32(filebytes, offset + 44);
                DS = BinaryHelper.ToUInt32(filebytes, offset + 48);
                ES = BinaryHelper.ToUInt32(filebytes, offset + 52);
                FS = BinaryHelper.ToUInt32(filebytes, offset + 56);
                GS = BinaryHelper.ToUInt32(filebytes, offset + 60);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{EAX={0}; EBX={1}; ECX={2}; EDX={3}; EDI={4}; ESI={5}; EBP={6}; ESP={7}; SS={8}; EFLAGS={9}; EIP={10}; CS={11}; DS={12}; ES={13}; FS={14}; GS={15}}}",
                EAX,
                EBX,
                ECX,
                EDX,
                EDI,
                ESI,
                EBP,
                ESP,
                SS,
                EFLAGS,
                EIP,
                CS,
                DS,
                ES,
                FS,
                GS);
        }
    }
}
