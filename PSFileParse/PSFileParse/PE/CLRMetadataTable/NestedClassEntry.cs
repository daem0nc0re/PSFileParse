using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class NestedClassEntry
    {
        public UInt32 Index { get; }
        public TableIndex NestedClass { get; }
        public TableIndex EnclosingClass { get; }


        internal NestedClassEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            NestedClass = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
            EnclosingClass = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
        }


        public override String ToString()
        {
            return String.Format("@{{NestedClass={0}; EnclosingClass={1}}}",
                NestedClass,
                EnclosingClass);
        }
    }
}
