using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class FunctionStartsEntry
    {
        public UInt32 Index { get; }
        public UInt64 Offset { get; }


        internal FunctionStartsEntry(
            byte[] data,
            ref UInt32 offset,
            UInt64 prev_offset,
            UInt32 index)
        {
            Index = index;
            Offset = prev_offset + LEB128.ToUInt64(data, ref offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}}}", Offset);
        }
    }
}
