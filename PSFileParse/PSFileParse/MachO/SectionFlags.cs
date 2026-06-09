using System;

namespace PSFileParse.MachO
{
    public sealed class SectionFlags
    {
        public SectionTypes Type { get; }
        public SectionAttributes Attributes { get; }


        internal SectionFlags(UInt32 value)
        {
            Type = (SectionTypes)(value & 0xFF);
            Attributes = (SectionAttributes)(value & 0xFFFFFF00);
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Attributes={1}}}",
                Type,
                Attributes);
        }
    }
}
