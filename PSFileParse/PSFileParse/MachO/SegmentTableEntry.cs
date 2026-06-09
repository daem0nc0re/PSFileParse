using System;

namespace PSFileParse.MachO
{
    public sealed class SegmentTableEntry
    {
        public UInt32 Index { get; }
        public String Name { get; }
        public UInt64 VMAddress { get; }
        public UInt64 VMSize { get; }
        public UInt64 FileOffset { get; }
        public UInt64 FileSize { get; }
        public VMProtectionFlags MaxProtection { get; }
        public VMProtectionFlags InitProtection { get; }
        public UInt32 NumberOfSections { get; }
        public SegmentFlags Flags { get; }


        internal SegmentTableEntry(UInt32 index, SegmentCommand segment)
        {
            Index = index;
            Name = segment.Name;
            VMAddress = segment.VMAddress;
            VMSize = segment.VMSize;
            FileOffset = segment.FileOffset;
            FileSize = segment.FileSize;
            MaxProtection = segment.MaxProtection;
            InitProtection = segment.InitProtection;
            NumberOfSections = segment.NumberOfSections;
            Flags = segment.Flags;
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; VMAddress={1}; VMSize={2}; FileOffset={3}; FileSize={4}; MaxProtection={5}; InitProtection={6}; NumberOfSections={7}; Flags={8}}}",
                Name,
                VMAddress,
                VMSize,
                FileOffset,
                FileSize,
                MaxProtection,
                InitProtection,
                NumberOfSections,
                Flags);
        }
    }
}
