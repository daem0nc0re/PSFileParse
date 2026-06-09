using System;

namespace PSFileParse.MachO
{
    // 
    // #elif defined(__arm__)
    // /*
    //  * No 128-bit intrinsic for ARM; leave it opaque for now.
    //  */
    // _STRUCT_ARM_NEON_STATE64 
    // {
    //     char opaque[(32 * 16) + (2 * sizeof(__uint32_t))];
    // } __attribute__((aligned(16)));
    // 
    // _STRUCT_ARM_NEON_STATE
    // {
    //     char opaque[(16 * 16) + (2 * sizeof(__uint32_t))];
    // } __attribute__((aligned(16)));
    // 
    public sealed class ArmNeonState
    {
        public byte[] Opaque { get; }


        internal ArmNeonState(
            byte[] filebytes,
            ref UInt32 offset,
            bool is64bit)
        {
            UInt32 size = is64bit ? 520u : 264u;
            Opaque = new byte[size];
            Array.Copy(filebytes, offset, Opaque, 0, size);
            offset += size + 8u;
        }


        public override String ToString()
        {
            return String.Format("@{{Opaque={0}}}", Opaque);
        }
    }
}
