using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class PropertyMapEntry
    {
        public UInt32 Index { get; }
        public TableIndex Parent { get; }
        public TableIndex PropertyList { get; }


        internal PropertyMapEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Parent = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
            PropertyList = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.Property);
        }


        public override String ToString()
        {
            return String.Format("@{{Parent={0}; PropertyList={1}}}",
                Parent,
                PropertyList);
        }
    }
}
