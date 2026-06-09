using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class GenericParamEntry
    {
        public UInt32 Index { get; }
        public UInt16 Number { get; }
        public GenericParamAttributeFlags Flags { get; }
        public CodedIndex Owner { get; }
        public String Name { get; }


        internal GenericParamEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            Number = BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;
            Flags = new GenericParamAttributeFlags(filebytes, ref offset);
            Owner = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.TypeOrMethodDef);

            if (Globals.UseWideStringIndex)
            {
                var str_index = BinaryHelper.ToUInt32(filebytes, offset);
                Name = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 4u;
            }
            else
            {
                var str_index = BinaryHelper.ToUInt16(filebytes, offset);
                Name = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 2u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Number={0}; Flags={1}; Owner={2}; Name={3}}}",
                Number,
                Flags,
                Owner,
                Name);
        }
    }
}
