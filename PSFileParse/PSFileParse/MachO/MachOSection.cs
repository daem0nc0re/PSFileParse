using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct section {
    //     char        sectname[16];
    //     char        segname[16];
    //     uint32_t    addr;
    //     uint32_t    size;
    //     uint32_t    offset;
    //     uint32_t    align;
    //     uint32_t    reloff;
    //     uint32_t    nreloc;
    //     uint32_t    flags;
    //     uint32_t    reserved1;
    //     uint32_t    reserved2;
    // };
    // 
    // struct section_64 {
    //     char        sectname[16];
    //     char        segname[16];
    //     uint64_t    addr;
    //     uint64_t    size;
    //     uint32_t    offset;
    //     uint32_t    align;
    //     uint32_t    reloff;
    //     uint32_t    nreloc;
    //     uint32_t    flags;
    //     uint32_t    reserved1;
    //     uint32_t    reserved2;
    //     uint32_t    reserved3;
    // };
    // 
    public sealed class MachOSection
    {
        public UInt32 Index { get; }
        public String SectionName { get; }
        public String SegmentName { get; }
        public UInt64 Address { get; }
        public UInt64 Size { get; }
        public UInt32 Offset { get; }
        public UInt32 Align { get; }
        public UInt32 RelocOffset { get; }
        public UInt32 NumberOfRelocs { get; }
        public SectionFlags Flags { get; }
        public UInt32[] Reserved { get; }
        public RelocationInfo[] Relocs { get; }


        internal MachOSection(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            MachCpuType cputype,
            bool is_bigendian,
            bool is64bit)
        {
            UInt32 relocs_off;
            var secbytes = new byte[16];
            var segbytes = new byte[16];
            Array.Copy(filebytes, offset, secbytes, 0, 16);
            Array.Copy(filebytes, offset + 16, segbytes, 0, 16);
            Index = index;
            SectionName = BinaryHelper.GetUTF8String(secbytes, 0);
            SegmentName = BinaryHelper.GetUTF8String(segbytes, 0);
            Reserved = is64bit ? new UInt32[3] : new UInt32[2];

            if (is_bigendian)
            {
                if (is64bit)
                {
                    Address = BinaryHelper.ToUInt64Big(filebytes, offset + 32);
                    Size = BinaryHelper.ToUInt64Big(filebytes, offset + 40);
                    Offset = BinaryHelper.ToUInt32Big(filebytes, offset + 48);
                    Align = BinaryHelper.ToUInt32Big(filebytes, offset + 52);
                    RelocOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 56);
                    NumberOfRelocs = BinaryHelper.ToUInt32Big(filebytes, offset + 60);
                    Flags = new SectionFlags(BinaryHelper.ToUInt32Big(filebytes, offset + 64));
                    Reserved[0] = BinaryHelper.ToUInt32Big(filebytes, offset + 68);
                    Reserved[1] = BinaryHelper.ToUInt32Big(filebytes, offset + 72);
                    Reserved[2] = BinaryHelper.ToUInt32Big(filebytes, offset + 76);
                }
                else
                {
                    Address = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                    Size = BinaryHelper.ToUInt32Big(filebytes, offset + 36);
                    Offset = BinaryHelper.ToUInt32Big(filebytes, offset + 40);
                    Align = BinaryHelper.ToUInt32Big(filebytes, offset + 44);
                    RelocOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 48);
                    NumberOfRelocs = BinaryHelper.ToUInt32Big(filebytes, offset + 52);
                    Flags = new SectionFlags(BinaryHelper.ToUInt32Big(filebytes, offset + 56));
                    Reserved[0] = BinaryHelper.ToUInt32Big(filebytes, offset + 60);
                    Reserved[1] = BinaryHelper.ToUInt32Big(filebytes, offset + 64);
                }
            }
            else
            {
                if (is64bit)
                {
                    Address = BinaryHelper.ToUInt64(filebytes, offset + 32);
                    Size = BinaryHelper.ToUInt64(filebytes, offset + 40);
                    Offset = BinaryHelper.ToUInt32(filebytes, offset + 48);
                    Align = BinaryHelper.ToUInt32(filebytes, offset + 52);
                    RelocOffset = BinaryHelper.ToUInt32(filebytes, offset + 56);
                    NumberOfRelocs = BinaryHelper.ToUInt32(filebytes, offset + 60);
                    Flags = new SectionFlags(BinaryHelper.ToUInt32(filebytes, offset + 64));
                    Reserved[0] = BinaryHelper.ToUInt32(filebytes, offset + 68);
                    Reserved[1] = BinaryHelper.ToUInt32(filebytes, offset + 72);
                    Reserved[2] = BinaryHelper.ToUInt32(filebytes, offset + 76);
                }
                else
                {
                    Address = BinaryHelper.ToUInt32(filebytes, offset + 32);
                    Size = BinaryHelper.ToUInt32(filebytes, offset + 36);
                    Offset = BinaryHelper.ToUInt32(filebytes, offset + 40);
                    Align = BinaryHelper.ToUInt32(filebytes, offset + 44);
                    RelocOffset = BinaryHelper.ToUInt32(filebytes, offset + 48);
                    NumberOfRelocs = BinaryHelper.ToUInt32(filebytes, offset + 52);
                    Flags = new SectionFlags(BinaryHelper.ToUInt32(filebytes, offset + 56));
                    Reserved[0] = BinaryHelper.ToUInt32(filebytes, offset + 60);
                    Reserved[1] = BinaryHelper.ToUInt32(filebytes, offset + 64);
                }
            }

            Relocs = new RelocationInfo[NumberOfRelocs];
            relocs_off = RelocOffset;

            for (UInt32 i = 0u; i < NumberOfRelocs; i++)
            {
                Relocs[i] = new RelocationInfo(
                    filebytes,
                    ref relocs_off,
                    i,
                    cputype,
                    is_bigendian);
            }

            offset += is64bit ? 80u : 68u;
        }


        internal MachOSection(UInt32 index, MachOSection section)
        {
            Index = index;
            SectionName = section.SectionName;
            SegmentName = section.SegmentName;
            Address = section.Address;
            Size = section.Size;
            Offset = section.Offset;
            Align = section.Align;
            RelocOffset = section.RelocOffset;
            NumberOfRelocs = section.NumberOfRelocs;
            Flags = section.Flags;
            Reserved = section.Reserved;
            Relocs = section.Relocs;
        }


        public override String ToString()
        {
            return String.Format("@{{SectionName={0}; SegmentName={1}; Address={2}; Size={3}; Offset={4}; Align={5}; RelocOffset={6}; NumberOfRelocs={7}; Flags={8}; Reserved={9}; Relocs={10}}}",
                SectionName,
                SegmentName,
                Address,
                Size,
                Offset,
                Align,
                RelocOffset,
                NumberOfRelocs,
                Flags,
                Reserved,
                Relocs);
        }
    }
}