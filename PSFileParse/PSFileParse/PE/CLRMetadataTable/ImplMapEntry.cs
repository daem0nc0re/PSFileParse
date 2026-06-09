using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ImplMapEntry
    {
        public UInt32 Index { get; }
        public PInvokeAttributeFlags MappingFlags { get; }
        public CodedIndex MemberForwarded { get; }
        public String ImportName { get; }
        public TableIndex ImportScope { get; }


        internal ImplMapEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            MappingFlags = new PInvokeAttributeFlags(filebytes, ref offset);
            MemberForwarded = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.MemberForwarded);

            if (Globals.UseWideStringIndex)
            {
                var str_index = BinaryHelper.ToUInt32(filebytes, offset);
                ImportName = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 4u;
            }
            else
            {
                var str_index = BinaryHelper.ToUInt16(filebytes, offset);
                ImportName = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 2u;
            }

            ImportScope = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.ModuleRef);
        }


        public override String ToString()
        {
            return String.Format("@{{MappingFlags={0}; MemberForwarded={1}; ImportName={2}; ImportScope={3}}}",
                MappingFlags,
                MemberForwarded,
                ImportName,
                ImportScope);
        }
    }
}
