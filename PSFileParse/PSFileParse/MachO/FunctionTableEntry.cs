using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    public sealed class FunctionTableEntry
    {
        public UInt32 Index { get; }
        public UInt64 Offset { get; }
        public UInt64 Address { get; }
        public String Name { get; }


        internal FunctionTableEntry(
            UInt32 index,
            UInt64 offset,
            UInt64 text_base,
            Dictionary<String, ExportInfo> exports)
        {
            Index = index;
            Offset = offset;
            Address = text_base + offset;

            if (exports != null)
            {
                foreach (var info in exports)
                {
                    if (info.Value.Offset == null)
                        continue;

                    if ((UInt64)info.Value.Offset == Offset)
                    {
                        Name = info.Key;
                        break;
                    }
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; Address={1}; Name={2}}}",
                Offset,
                Address,
                Name);
        }
    }
}
