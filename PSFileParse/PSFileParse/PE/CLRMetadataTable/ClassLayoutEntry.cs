using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ClassLayoutEntry
    {
        public UInt32 Index { get; }
        public UInt16 PackingSize { get; }
        public UInt32 ClassSize { get; }
        public TableIndex Parent { get; }


        internal ClassLayoutEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            PackingSize = BinaryHelper.ToUInt16(filebytes, offset);
            ClassSize = BinaryHelper.ToUInt32(filebytes, offset + 2);
            offset += 6u;
            Parent = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
        }


        public override String ToString()
        {
            return String.Format("@{{PackingSize={0}; ClassSize={1}; Parent={2}}}",
                PackingSize,
                ClassSize,
                Parent);
        }
    }
}
