using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class InterfaceImplEntry
    {
        public UInt32 Index { get; }
        public TableIndex Class { get; }
        public CodedIndex Interface { get; }


        internal InterfaceImplEntry(byte[] filebytes, ref UInt32 offset, UInt32 index)
        {
            Index = index;
            Class = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
            Interface = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.TypeDefOrRef);
        }


        public override String ToString()
        {
            return String.Format("@{{Class={0}; Interface={1}}}",
                Class,
                Interface);
        }
    }
}
