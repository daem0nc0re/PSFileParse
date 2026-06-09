using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class MethodSemanticsEntry
    {
        public UInt32 Index { get; }
        public MethodSemanticsFlags Semantics { get; }
        public TableIndex Method { get; }
        public CodedIndex Association { get; }


        internal MethodSemanticsEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Semantics = (MethodSemanticsFlags)BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;
            Method = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.MethodDef);
            Association = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.HasSemantics);
        }


        public override String ToString()
        {
            return String.Format("@{{Semantics={0}; Method={1}; Association={2}}}",
                Semantics,
                Method,
                Association);
        }
    }
}
