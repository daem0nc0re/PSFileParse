using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct mach_header {
    //     uint32_t      magic;
    //     cpu_type_t    cputype;
    //     cpu_subtype_t cpusubtype;
    //     uint32_t      filetype;
    //     uint32_t      ncmds;
    //     uint32_t      sizeofcmds;
    //     uint32_t      flags;
    // };
    // 
    // struct mach_header_64 {
    //     uint32_t      magic;
    //     cpu_type_t    cputype;
    //     cpu_subtype_t cpusubtype;
    //     uint32_t      filetype;
    //     uint32_t      ncmds;
    //     uint32_t      sizeofcmds;
    //     uint32_t      flags;
    //     uint32_t      reserved;
    // };
    // 
    public sealed class MachOHeader
    {
        public MachOMagic Magic { get; }
        public MachCpuType CPUType { get; }
        public Object CPUSubType { get; }
        public Object Capabilities { get; }
        public MachOFileType FileType { get; }
        public UInt32 NumberOfLoadCommands { get; }
        public UInt32 SizeOfLoadCommands { get; }
        public MachOFlags Flags { get; }
        public Object Reserved { get; }


        internal MachOHeader(
            byte[] filebytes,
            ref UInt32 offset,
            out bool is_bigendian,
            out bool is64bit)
        {
            UInt32 caps;
            UInt32 subtype;
            is64bit = false;
            Magic = (MachOMagic)BinaryHelper.ToUInt32(filebytes, offset);

            if ((Magic == MachOMagic.LittleEndian64Bit) ||
                (Magic == MachOMagic.LittleEndian32Bit))
            {
                CPUType = (MachCpuType)BinaryHelper.ToUInt32(filebytes, offset + 4);
                subtype = BinaryHelper.ToUInt32(filebytes, offset + 8);
                FileType = (MachOFileType)BinaryHelper.ToUInt32(filebytes, offset + 12);
                NumberOfLoadCommands = BinaryHelper.ToUInt32(filebytes, offset + 16);
                SizeOfLoadCommands = BinaryHelper.ToUInt32(filebytes, offset + 20);
                Flags = (MachOFlags)BinaryHelper.ToUInt32(filebytes, offset + 24);
                is_bigendian = false;

                if (Magic == MachOMagic.LittleEndian64Bit)
                {
                    Reserved = BinaryHelper.ToUInt32(filebytes, offset + 28);
                    is64bit = true;
                }
            }
            else
            {
                CPUType = (MachCpuType)BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                subtype = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                FileType = (MachOFileType)BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                NumberOfLoadCommands = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                SizeOfLoadCommands = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                Flags = (MachOFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 24);
                is_bigendian = true;

                if (Magic == MachOMagic.BigEndian64Bit)
                {
                    Reserved = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
                    is64bit = true;
                }
            }

            caps = subtype & 0xFF000000;
            subtype &= 0x00FFFFFF;

            if (CPUType == MachCpuType.VAX)
                CPUSubType = (MachCpuSubTypeVAX)subtype;
            else if (CPUType == MachCpuType.MC680x0)
                CPUSubType = (MachCpuSubTypeMC680x0)subtype;
            else if (CPUType == MachCpuType.I386)
                CPUSubType = (MachCpuSubTypeI386)subtype;
            else if (CPUType == MachCpuType.X86_64)
                CPUSubType = (MachCpuSubTypeX86)subtype;
            else if (CPUType == MachCpuType.MC98000)
                CPUSubType = (MachCpuSubTypeMC98000)subtype;
            else if (CPUType == MachCpuType.HPPA)
                CPUSubType = (MachCpuSubTypeHPPA)subtype;
            else if (CPUType == MachCpuType.ARM)
                CPUSubType = (MachCpuSubTypeARM)subtype;
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
                if ((MachCpuSubTypeARM64)CPUSubType == MachCpuSubTypeARM64.ARM64E)
                {
                    if ((caps & (UInt32)MachCpuCapabilitiesMask.Arm64eVersionedAbiMask) != 0)
                    {
                        var abi = ((caps & (UInt32)MachCpuCapabilitiesMask.Arm64ePtrAuthMask) >> 24) & 0xFF;

                        if ((caps & (UInt32)MachCpuCapabilitiesMask.Arm64eKernelAbiMask) != 0)
                            Capabilities = String.Format("KER{0}", abi.ToString("D2"));
                        else
                            Capabilities = String.Format("USR{0}", abi.ToString("D2"));
                    }
                    else if (((caps & (UInt32)MachCpuCapabilitiesMask.Arm64PtrAuthMask) != 0) &&
                        ((subtype & (~(UInt32)MachCpuCapabilitiesMask.Arm64PtrAuthMask)) == 0))
                    {
                        var abi = ((caps & (UInt32)MachCpuCapabilitiesMask.Arm64PtrAuthMask) >> 24) & 0xFF;
                        Capabilities = String.Format("PAC{0}", abi.ToString("D2"));
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

            offset += is64bit ? 32u : 28u;
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; CPUType={1}; CPUSubType={2}; Capabilities={3}; FileType={4}; NumberOfLoadCommands={5}; SizeOfLoadCommands={6}; Flags={7}; Reserved={8}}}",
                Magic,
                CPUType,
                CPUSubType,
                Capabilities,
                FileType,
                NumberOfLoadCommands,
                SizeOfLoadCommands,
                Flags,
                Reserved);
        }
    }
}
