using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct
    // {
    //     unsigned short Offset : 12;
    //     unsigned short Type : 4;
    // };
    //
    public sealed class BaseRelocTypeOffsetEntry
    {
        public UInt32 Index { get; }
        public Object Type { get; }
        public UInt16 Offset { get; }
        internal static readonly UInt32 SizeOfStruct = 0x2;


        internal BaseRelocTypeOffsetEntry(
            byte[] filebytes,
            UInt32 index,
            UInt32 offset,
            ImageFileMachine machine)
        {
            var data = BinaryHelper.ToUInt16(filebytes, offset);
            Index = index;
            Offset = (UInt16)(data & 0x0FFF);

            if (machine == ImageFileMachine.ARM)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.ARM64)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.ARM64EC)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.ARM64X)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.ARMNT)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.LOONGARCH32)
                Type = (ImageRelBasedLoongArch32)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.LOONGARCH64)
                Type = (ImageRelBasedLoongArch64)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.MIPS16)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.MIPSFPU)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.MIPSFPU16)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.RISCV32)
                Type = (ImageRelBasedTypeRISCV)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.RISCV64)
                Type = (ImageRelBasedTypeRISCV)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.RISCV128)
                Type = (ImageRelBasedTypeRISCV)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.THUMB)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else if (machine == ImageFileMachine.WCEMIPSV2)
                Type = (ImageRelBasedTypeArm)(data >> 12 & 0x0F);
            else
                Type = (ImageRelBasedType)(data >> 12 & 0x0F);
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Offset={1}}}", Type, Offset);
        }
    }
}
