using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class TypeRefEntry
    {
        public UInt32 Index { get; }
        public CodedIndex ResolutionScope { get; }
        public String TypeName { get; }
        public String TypeNamespace { get; }


        internal TypeRefEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            ResolutionScope = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.ResolutionScope);

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
        }


        public override String ToString()
        {
            return String.Format("@{{ResolutionScope={0}; TypeName={1}; TypeNamespace={2}}}",
                ResolutionScope,
                TypeName,
                TypeNamespace);
        }
    }
}
