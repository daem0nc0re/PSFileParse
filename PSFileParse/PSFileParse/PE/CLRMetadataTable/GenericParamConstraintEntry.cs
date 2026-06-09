using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class GenericParamConstraintEntry
    {
        public UInt32 Index { get; }
        public TableIndex Owner { get; }
        public CodedIndex Constraint { get; }


        internal GenericParamConstraintEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Owner = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.GenericParam);
            Constraint = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.TypeDefOrRef);
        }


        public override String ToString()
        {
            return String.Format("@{{Owner={0}; Constraint={1}}}",
                Owner,
                Constraint);
        }
    }
}
