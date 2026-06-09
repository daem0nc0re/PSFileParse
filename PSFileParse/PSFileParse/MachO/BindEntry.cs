using System;

namespace PSFileParse.MachO
{
    public sealed class BindEntry
    {
        public UInt32 Index { get; }
        public String Segment { get; }
        public String Section { get; }
        public UInt64 Address { get; }
        public BindType Type { get; }
        public bool WeakImport { get; }
        public String Dylib { get; }
        public String Symbol { get; }


        internal BindEntry(
            SegmentTableEntry segment,
            MachOSection[] sections,
            UInt64 seg_offset,
            BindType type,
            bool is_weak,
            String dylib,
            String symbol)
        {
            Index = (UInt32)BindFSA.Stack.Count;
            Segment = segment.Name;
            Address = segment.VMAddress + seg_offset;
            Type = type;
            WeakImport = is_weak;
            Dylib = dylib;
            Symbol = symbol;

            foreach (var sec in sections)
            {
                if ((sec.Address <= Address) && (sec.Address + sec.Size > Address))
                {
                    Section = sec.SectionName;
                    break;
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Segment={0}; Section={1}; Address={2}; Type={3}; WeakImport={4}; Dylib={5}; Symbol={6}}}",
                Segment,
                Section,
                Address,
                Type,
                WeakImport,
                Dylib,
                Symbol);
        }
    }
}
