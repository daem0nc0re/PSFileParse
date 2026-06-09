using System;

namespace PSFileParse.PE
{
    public sealed class ExportFunction
    {
        public UInt32 Ordinal { get; }
        public Object Hint { get; }
        public String Section { get; }
        public UInt32 VirtualAddress { get; }
        public UInt32 RawOffset { get; }
        public String Name { get; }


        internal ExportFunction(
            UInt32 ordinal,
            Object hint,
            String sec_name,
            UInt32 virt_addr,
            UInt32 raw_offset,
            String name)
        {
            Ordinal = ordinal;
            Hint = hint;
            Section = sec_name;
            VirtualAddress = virt_addr;
            RawOffset = raw_offset;
            Name = name;
        }


        public override String ToString()
        {
            return String.Format("@{{Ordinal={0}; Hint={1}; Section={2}; VirtualAddress={3}; RawOffset={4}; Name={5}}}",
                Ordinal,
                Hint,
                Section,
                VirtualAddress,
                RawOffset,
                Name);
        }
    }
}
