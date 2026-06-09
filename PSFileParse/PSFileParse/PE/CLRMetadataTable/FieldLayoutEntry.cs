using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class FieldLayoutEntry
    {
        public UInt32 Index { get; }
        public UInt32 Offset { get; }
        public TableIndex Field { get; }


        internal FieldLayoutEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Offset = BinaryHelper.ToUInt32(filebytes, offset);
            offset += 4u;
            Field = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.Field);
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; Field={1}}}",
                Offset,
                Field);
        }
    }
}
