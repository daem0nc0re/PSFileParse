using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class AssemblyRefProcessorEntry
    {
        public UInt32 Index { get; }
        public UInt32 Processor { get; }
        public TableIndex AssemblyRef { get; }


        internal AssemblyRefProcessorEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Processor = BinaryHelper.ToUInt32(filebytes, offset);
            offset += 4u;
            AssemblyRef = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.AssemblyRef);
        }


        public override String ToString()
        {
            return String.Format("@{{Processor={0}; AssemblyRef={1}}}",
                Processor,
                AssemblyRef);
        }
    }
}
