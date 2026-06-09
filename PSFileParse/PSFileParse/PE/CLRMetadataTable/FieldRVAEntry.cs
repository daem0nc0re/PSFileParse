using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class FieldRVAEntry
    {
        public UInt32 Index { get; }
        public UInt32 RVA { get; }
        public TableIndex Field { get; }


        internal FieldRVAEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            RVA = BinaryHelper.ToUInt32(filebytes, offset);
            offset += 4u;
            Field = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.Field);
        }


        public override String ToString()
        {
            return String.Format("@{{RVA={0}; Field={1}}}", RVA, Field);
        }
    }
}
