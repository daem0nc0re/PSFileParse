using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct segment_command {
    //     uint32_t  cmd;
    //     uint32_t  cmdsize;
    //     char      segname[16];
    //     uint32_t  vmaddr;
    //     uint32_t  vmsize;
    //     uint32_t  fileoff;
    //     uint32_t  filesize;
    //     vm_prot_t maxprot;
    //     vm_prot_t initprot;
    //     uint32_t  nsects;
    //     uint32_t  flags;
    // };
    // 
    // struct segment_command_64 {
    //     uint32_t  cmd;
    //     uint32_t  cmdsize;
    //     char      segname[16];
    //     uint64_t  vmaddr;
    //     uint64_t  vmsize;
    //     uint64_t  fileoff;
    //     uint64_t  filesize;
    //     vm_prot_t maxprot;
    //     vm_prot_t initprot;
    //     uint32_t  nsects;
    //     uint32_t  flags;
    // };
    // 
    public sealed class SegmentCommand
    {
        public String Name { get; }
        public UInt64 VMAddress { get; }
        public UInt64 VMSize { get; }
        public UInt64 FileOffset { get; }
        public UInt64 FileSize { get; }
        public VMProtectionFlags MaxProtection { get; }
        public VMProtectionFlags InitProtection { get; }
        public UInt32 NumberOfSections { get; }
        public SegmentFlags Flags { get; }
        public MachOSection[] Sections { get; }


        internal SegmentCommand(
            byte[] filebytes,
            UInt32 offset,
            MachCpuType cputype,
            bool is_bigendian,
            bool is64bit)
        {
            var namebytes = new byte[16];
            Array.Copy(filebytes, offset, namebytes, 0, 16);
            Name = BinaryHelper.GetUTF8String(namebytes, 0);

            if (is64bit)
            {
                if (is_bigendian)
                {
                    VMAddress = BinaryHelper.ToUInt64Big(filebytes, offset + 16);
                    VMSize = BinaryHelper.ToUInt64Big(filebytes, offset + 24);
                    FileOffset = BinaryHelper.ToUInt64Big(filebytes, offset + 32);
                    FileSize = BinaryHelper.ToUInt64Big(filebytes, offset + 40);
                    MaxProtection = (VMProtectionFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 48);
                    InitProtection = (VMProtectionFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 52);
                    NumberOfSections = BinaryHelper.ToUInt32Big(filebytes, offset + 56);
                    Flags = (SegmentFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 60);
                }
                else
                {
                    VMAddress = BinaryHelper.ToUInt64(filebytes, offset + 16);
                    VMSize = BinaryHelper.ToUInt64(filebytes, offset + 24);
                    FileOffset = BinaryHelper.ToUInt64(filebytes, offset + 32);
                    FileSize = BinaryHelper.ToUInt64(filebytes, offset + 40);
                    MaxProtection = (VMProtectionFlags)BinaryHelper.ToUInt32(filebytes, offset + 48);
                    InitProtection = (VMProtectionFlags)BinaryHelper.ToUInt32(filebytes, offset + 52);
                    NumberOfSections = BinaryHelper.ToUInt32(filebytes, offset + 56);
                    Flags = (SegmentFlags)BinaryHelper.ToUInt32(filebytes, offset + 60);
                }
            }
            else
            {
                if (is_bigendian)
                {
                    VMAddress = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                    VMSize = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                    FileOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 24);
                    FileSize = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
                    MaxProtection = (VMProtectionFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                    InitProtection = (VMProtectionFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 36);
                    NumberOfSections = BinaryHelper.ToUInt32Big(filebytes, offset + 40);
                    Flags = (SegmentFlags)BinaryHelper.ToUInt32Big(filebytes, offset + 44);
                }
                else
                {
                    VMAddress = BinaryHelper.ToUInt32(filebytes, offset + 16);
                    VMSize = BinaryHelper.ToUInt32(filebytes, offset + 20);
                    FileOffset = BinaryHelper.ToUInt32(filebytes, offset + 24);
                    FileSize = BinaryHelper.ToUInt32(filebytes, offset + 28);
                    MaxProtection = (VMProtectionFlags)BinaryHelper.ToUInt32(filebytes, offset + 32);
                    InitProtection = (VMProtectionFlags)BinaryHelper.ToUInt32(filebytes, offset + 36);
                    NumberOfSections = BinaryHelper.ToUInt32(filebytes, offset + 40);
                    Flags = (SegmentFlags)BinaryHelper.ToUInt32(filebytes, offset + 44);
                }
            }

            Sections = new MachOSection[NumberOfSections];
            offset += is64bit ? 64u : 48u;

            for (UInt32 i = 0u; i < (UInt32)Sections.Length; i++)
            {
                Sections[i] = new MachOSection(
                    filebytes,
                    ref offset,
                    i,
                    cputype,
                    is_bigendian,
                    is64bit);
            }

            MachOFile.NumberOfSegments++;
            MachOFile.NumberOfSections += NumberOfSections;
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; VMAddress={1}; VMSize={2}; FileOffset={3}; FileSize={4}; MaxProtection={5}; InitProtection={6}; NumberOfSections={7}; Flags={8}; Sections={9}}}",
                Name,
                VMAddress,
                VMSize,
                FileOffset,
                FileSize,
                MaxProtection,
                InitProtection,
                NumberOfSections,
                Flags,
                Sections);
        }
    }
}