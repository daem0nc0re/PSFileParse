using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class RebaseEntry
    {
        public UInt32 Index { get; }
        public String Segment { get; }
        public String Section { get; }
        public UInt64 Address { get; }
        public RebaseType Type { get; }
        public UInt64 Target { get; }


        internal RebaseEntry(
            byte[] filebytes,
            SegmentTableEntry segment,
            MachOSection[] sections,
            UInt64 seg_offset,
            RebaseType type,
            bool is64bit)
        {
            Index = (UInt32)RebaseFSA.Stack.Count;
            Segment = segment.Name;
            Address = segment.VMAddress + seg_offset;
            Type = type;

            foreach (var sec in sections)
            {
                if ((sec.Address <= Address) && (sec.Address + sec.Size > Address))
                {
                    Section = sec.SectionName;
                    break;
                }
            }

            Target = is64bit ?
                BinaryHelper.ToUInt64(filebytes, segment.FileOffset + seg_offset) :
                BinaryHelper.ToUInt32(filebytes, segment.FileOffset + seg_offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Segment={0}; Section={1}; Address={2}; Type={3}; Target={4}}}",
                Segment,
                Section,
                Address,
                Type,
                Target);
        }
    }
}
