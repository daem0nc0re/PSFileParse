using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class MethodImplEntry
    {
        public UInt32 Index { get; }
        public TableIndex Class { get; }
        public CodedIndex MethodBody { get; }
        public CodedIndex MethodDeclaration { get; }


        internal MethodImplEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Class = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.TypeDef);
            MethodBody = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.MethodDefOrRef);
            MethodDeclaration = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.MethodDefOrRef);
        }


        public override String ToString()
        {
            return String.Format("@{{Class={0}; MethodBody={1}; MethodDeclaration={2}}}",
                Class,
                MethodBody,
                MethodDeclaration);
        }
    }
}
