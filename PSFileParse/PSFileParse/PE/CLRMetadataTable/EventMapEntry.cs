using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class EventMapEntry
    {
        public UInt32 Index { get; }
        public TableIndex Parent { get; }
        public TableIndex EventList { get; }


        internal EventMapEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Parent = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
            EventList = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.Event);
        }


        public override String ToString()
        {
            return String.Format("@{{Parent={0}; EventList={1}}}",
                Parent,
                EventList);
        }
    }
}
