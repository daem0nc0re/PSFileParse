using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    internal class AssemblyProcessorEntry
    {
        public UInt32 Index { get; }
        public UInt32 Processor { get; }


        internal AssemblyProcessorEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Processor = BinaryHelper.ToUInt32(filebytes, offset);
            offset += 4u;
        }


        public override String ToString()
        {
            return String.Format("@{{Processor={0}}}", Processor);
        }
    }
}
