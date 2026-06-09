using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class TypeDefEntry
    {
        public UInt32 Index { get; }
        public TypeFlags Flags { get; }
        public String TypeName { get; }
        public String TypeNamespace { get; }
        public CodedIndex Extends { get; }
        public TableIndex FieldList { get; }
        public TableIndex MethodList { get; }


        internal TypeDefEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            Flags = new TypeFlags(filebytes, ref offset);

            if (Globals.UseWideStringIndex)
            {
                var str_index = BinaryHelper.ToUInt32(filebytes, offset);
                TypeName = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                str_index = BinaryHelper.ToUInt32(filebytes, offset + 4);
                TypeNamespace = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 8u;
            }
            else
            {
                var str_index = BinaryHelper.ToUInt16(filebytes, offset);
                TypeName = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                str_index = BinaryHelper.ToUInt16(filebytes, offset + 2);
                TypeNamespace = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 4u;
            }

            Extends = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.TypeDefOrRef);
            FieldList = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.Field);
            MethodList = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.MethodDef);
        }


        public override String ToString()
        {
            return String.Format("@{{Flags={0}; TypeName={1}; TypeNamespace={2}; Extends={3}; FieldList={4}; MethodList={5}}}",
                Flags,
                TypeName,
                TypeNamespace,
                Extends,
                FieldList,
                MethodList);
        }
    }
}
