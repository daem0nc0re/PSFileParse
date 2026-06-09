using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ExportedTypeEntry
    {
        public UInt32 Index { get; }
        public TypeFlags Flags { get; }
        public UInt32 TypeDefId { get; }
        public String TypeName { get; }
        public String TypeNamespace { get; }
        public CodedIndex Implementation { get; }


        internal ExportedTypeEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            Flags = new TypeFlags(filebytes, ref offset);
            TypeDefId = BinaryHelper.ToUInt32(filebytes, offset);
            offset += 4u;

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

            Implementation = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.Implementation);
        }


        public override String ToString()
        {
            return String.Format("@{{Flags={0}; TypeDefId={1}; TypeName={2}; TypeNamespace={3}; Implementation={4}}}",
                Flags,
                TypeDefId,
                TypeName,
                TypeNamespace,
                Implementation);
        }
    }
}
