using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct fat_arch {
    //     cpu_type_t    cputype;
    //     cpu_subtype_t cpusubtype;
    //     uint32_t      offset;
    //     uint32_t      size;
    //     uint32_t      align;
    // };
    // 
    public sealed class FatArch
    {
        public MachCpuType CPUType { get; }
        public Object CPUSubType { get; }
        public Object Capabilities { get; }
        public UInt32 Offset { get; }
        public UInt32 Size { get; }
        public UInt32 Align { get; }


        internal FatArch(byte[] filebytes, FatMagic magic, ref UInt32 offset)
        {
            UInt32 subtype;
            UInt32 caps;

            if (magic == FatMagic.LittleEndian)
            {
                CPUType = (MachCpuType)BinaryHelper.ToUInt32(filebytes, offset);
                subtype = BinaryHelper.ToUInt32(filebytes, offset + 4);
                Offset = BinaryHelper.ToUInt32(filebytes, offset + 8);
                Size = BinaryHelper.ToUInt32(filebytes, offset + 12);
                Align = BinaryHelper.ToUInt32(filebytes, offset + 16);
            }
            else if (magic == FatMagic.BigEndian)
            {
                CPUType = (MachCpuType)BinaryHelper.ToUInt32Big(filebytes, offset);
                subtype = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                Offset = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                Size = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                Align = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
            }
            else
            {
                throw new Exception("Invalid Mach-O magic.");
            }

            caps = subtype & 0xFF000000;
            subtype &= 0x00FFFFFF;

            if (CPUType == MachCpuType.VAX)
                CPUSubType = (MachCpuSubTypeVAX)subtype;
            else if (CPUType == MachCpuType.MC680x0)
                CPUSubType = (MachCpuSubTypeMC680x0)subtype;
            else if (CPUType == MachCpuType.I386)
                CPUSubType =(MachCpuSubTypeI386)subtype;
            else if (CPUType == MachCpuType.X86_64)
                CPUSubType =(MachCpuSubTypeX86)subtype;
            else if (CPUType == MachCpuType.MC98000)
                CPUSubType=(MachCpuSubTypeMC98000)subtype;
            else if (CPUType == MachCpuType.HPPA)
                CPUSubType = (MachCpuSubTypeHPPA)subtype;
            else if (CPUType == MachCpuType.ARM)
                CPUSubType= (MachCpuSubTypeARM)subtype;
            else if (CPUType == MachCpuType.ARM64)
                CPUSubType = (MachCpuSubTypeARM64)subtype;
            else if (CPUType == MachCpuType.ARM64_32)
                CPUSubType = (MachCpuSubTypeARM64_32)subtype;
            else if (CPUType == MachCpuType.SPARC)
                CPUSubType = (MachCpuSubTypeSPARC)subtype;
            else if (CPUType == MachCpuType.MC88000)
                CPUSubType = (MachCpuSubTypeMC88000)subtype;
            else if ((CPUType == MachCpuType.POWERPC) || (CPUType == MachCpuType.POWERPC64))
                CPUSubType = (MachCpuSubTypePOWERPC)subtype;
            else
                CPUSubType = (Int32)subtype;

            if ((CPUType == MachCpuType.X86_64) || (CPUType == MachCpuType.POWERPC64))
            {
                Capabilities = ((MachCpuCapabilitiesX64)caps).ToString();
            }
            else if (CPUType == MachCpuType.ARM64)
            {
                if ((caps & (UInt32)MachCpuCapabilitiesArm64.PtrAuthVersion) != 0)
                {
                    bool is_kernel = ((caps & (UInt32)MachCpuCapabilitiesArm64.Kernel) != 0);
                    Capabilities = String.Format("PtrAuthVersion {0} {1}",
                        is_kernel ? "Kernel" : "Userspace",
                        subtype & ((UInt32)MachCpuCapabilitiesMask.Arm64eKernelAbiMask >> 24));
                }
                else
                {
                    if (caps == 0)
                        Capabilities = "None";
                    else
                        Capabilities = (caps >> 24) & 0xFF;
                }
            }
            else
            {
                Capabilities = (caps >> 24) & 0xFF;
            }

            offset += 20u;
        }


        public override String ToString()
        {
            return String.Format("@{{CPUType={0}; CPUSubType={1}; Capabilities={2}; Offset={3}; Size={4}; Align={5}}}",
                CPUType,
                CPUSubType,
                Capabilities,
                Offset,
                Size,
                Align);
        }
    }
}
