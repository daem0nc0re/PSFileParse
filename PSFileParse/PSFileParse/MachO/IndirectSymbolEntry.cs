using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class IndirectSymbolEntry
    {
        public UInt32 Index { get; }
        public IndirectSymbolIndex SymbolIndex { get; }
        public UInt64 Address { get; }
        public String Section { get; }
        public String Segment { get; }
        public SectionTypes Type { get; }
        public String Name { get; }


        internal IndirectSymbolEntry(
            byte[] filebytes,
            UInt32 offset,
            UInt32 index,
            SymbolTableEntry[] symbols,
            MachOSection section,
            bool is_bigendian,
            bool is64bit)
        {
            var sym_mask = (UInt32)(IndirectSymbolIndex.Local | IndirectSymbolIndex.Absolute);
            var stride = (section.Flags.Type == SectionTypes.SymbolStubs) ?
                section.Reserved[1] :
                (is64bit ? 8u : 4u);
            var base_index = index - section.Reserved[0];
            Index = index;
            Address = section.Address + (base_index * stride);
            Section = section.SectionName;
            Segment = section.SegmentName;
            Type = section.Flags.Type;

            if (is_bigendian)
            {
                SymbolIndex = (IndirectSymbolIndex)BinaryHelper.ToUInt32Big(
                    filebytes,
                    offset + (index * 4u));
            }
            else
            {
                SymbolIndex = (IndirectSymbolIndex)BinaryHelper.ToUInt32(
                    filebytes,
                    offset + (index * 4u));
            }

            if (((UInt32)SymbolIndex & sym_mask) == 0)
            {
                try
                {
                    Name = symbols[(UInt32)SymbolIndex].Name;
                }
                catch { }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{SymbolIndex={0}; Address={1}; Section={2}; Segment={3}; Type={4}; Name={5}}}",
                SymbolIndex,
                Address,
                Section,
                Segment,
                Type,
                Name);
        }
    }
}
