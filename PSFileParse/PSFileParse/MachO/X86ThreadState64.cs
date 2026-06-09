using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // _STRUCT_X86_THREAD_STATE64
    // {
    //     __uint64_t rax;
    //     __uint64_t rbx;
    //     __uint64_t rcx;
    //     __uint64_t rdx;
    //     __uint64_t rdi;
    //     __uint64_t rsi;
    //     __uint64_t rbp;
    //     __uint64_t rsp;
    //     __uint64_t r8;
    //     __uint64_t r9;
    //     __uint64_t r10;
    //     __uint64_t r11;
    //     __uint64_t r12;
    //     __uint64_t r13;
    //     __uint64_t r14;
    //     __uint64_t r15;
    //     __uint64_t rip;
    //     __uint64_t rflags;
    //     __uint64_t cs;
    //     __uint64_t fs;
    //     __uint64_t gs;
    // };
    // 
    public sealed class X86ThreadState64
    {
        public UInt64 RAX { get; }
        public UInt64 RBX { get; }
        public UInt64 RCX { get; }
        public UInt64 RDX { get; }
        public UInt64 RDI { get; }
        public UInt64 RSI { get; }
        public UInt64 RBP { get; }
        public UInt64 RSP { get; }
        public UInt64 R8 { get; }
        public UInt64 R9 { get; }
        public UInt64 R10 { get; }
        public UInt64 R11 { get; }
        public UInt64 R12 { get; }
        public UInt64 R13 { get; }
        public UInt64 R14 { get; }
        public UInt64 R15 { get; }
        public UInt64 RIP { get; }
        public UInt64 RFLAGS { get; }
        public UInt64 CS { get; }
        public UInt64 FS { get; }
        public UInt64 GS { get; }


        internal X86ThreadState64(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                RAX = BinaryHelper.ToUInt64Big(filebytes, offset);
                RBX = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                RCX = BinaryHelper.ToUInt64Big(filebytes, offset + 16);
                RDX = BinaryHelper.ToUInt64Big(filebytes, offset + 24);
                RDI = BinaryHelper.ToUInt64Big(filebytes, offset + 32);
                RSI = BinaryHelper.ToUInt64Big(filebytes, offset + 40);
                RBP = BinaryHelper.ToUInt64Big(filebytes, offset + 48);
                RSP = BinaryHelper.ToUInt64Big(filebytes, offset + 56);
                R8 = BinaryHelper.ToUInt64Big(filebytes, offset + 64);
                R9 = BinaryHelper.ToUInt64Big(filebytes, offset + 72);
                R10 = BinaryHelper.ToUInt64Big(filebytes, offset + 80);
                R11 = BinaryHelper.ToUInt64Big(filebytes, offset + 88);
                R12 = BinaryHelper.ToUInt64Big(filebytes, offset + 96);
                R13 = BinaryHelper.ToUInt64Big(filebytes, offset + 104);
                R14 = BinaryHelper.ToUInt64Big(filebytes, offset + 112);
                R15 = BinaryHelper.ToUInt64Big(filebytes, offset + 120);
                RIP = BinaryHelper.ToUInt64Big(filebytes, offset + 128);
                RFLAGS = BinaryHelper.ToUInt64Big(filebytes, offset + 136);
                CS = BinaryHelper.ToUInt64Big(filebytes, offset + 144);
                FS = BinaryHelper.ToUInt64Big(filebytes, offset + 152);
                GS = BinaryHelper.ToUInt64Big(filebytes, offset + 160);
            }
            else
            {
                RAX = BinaryHelper.ToUInt64(filebytes, offset);
                RBX = BinaryHelper.ToUInt64(filebytes, offset + 8);
                RCX = BinaryHelper.ToUInt64(filebytes, offset + 16);
                RDX = BinaryHelper.ToUInt64(filebytes, offset + 24);
                RDI = BinaryHelper.ToUInt64(filebytes, offset + 32);
                RSI = BinaryHelper.ToUInt64(filebytes, offset + 40);
                RBP = BinaryHelper.ToUInt64(filebytes, offset + 48);
                RSP = BinaryHelper.ToUInt64(filebytes, offset + 56);
                R8 = BinaryHelper.ToUInt64(filebytes, offset + 64);
                R9 = BinaryHelper.ToUInt64(filebytes, offset + 72);
                R10 = BinaryHelper.ToUInt64(filebytes, offset + 80);
                R11 = BinaryHelper.ToUInt64(filebytes, offset + 88);
                R12 = BinaryHelper.ToUInt64(filebytes, offset + 96);
                R13 = BinaryHelper.ToUInt64(filebytes, offset + 104);
                R14 = BinaryHelper.ToUInt64(filebytes, offset + 112);
                R15 = BinaryHelper.ToUInt64(filebytes, offset + 120);
                RIP = BinaryHelper.ToUInt64(filebytes, offset + 128);
                RFLAGS = BinaryHelper.ToUInt64(filebytes, offset + 136);
                CS = BinaryHelper.ToUInt64(filebytes, offset + 144);
                FS = BinaryHelper.ToUInt64(filebytes, offset + 152);
                GS = BinaryHelper.ToUInt64(filebytes, offset + 160);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{RAX={0}; RBX={1}; RCX={2}; RDX={3}; RDI={4}; RSI={5}; RBP={6}; RSP={7}; R8={8}; R9={9}; R10={10}; R11={11}; R12={12}; R13={13}; R14={14}; R15={15}; RIP={16}; RFLAGS={17}; CS={18}; FS={19}; GS={20}}}",
                RAX,
                RBX,
                RCX,
                RDX,
                RDI,
                RSI,
                RBP,
                RSP,
                R8,
                R9,
                R10,
                R11,
                R12,
                R13,
                R14,
                R15,
                RIP,
                RFLAGS,
                CS,
                FS,
                GS);
        }
    }
}
