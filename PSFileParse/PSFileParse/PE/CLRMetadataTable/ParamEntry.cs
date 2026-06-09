using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ParamEntry
    {
        public UInt32 Index { get; }
        public ParamAttributes Flags { get; }
        public UInt16 Sequence { get; }
        public String Name { get; }


        internal ParamEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            Flags = (ParamAttributes)BinaryHelper.ToUInt16(filebytes, offset);
            Sequence = BinaryHelper.ToUInt16(filebytes, offset + 2);
            offset += 4u;

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
            return String.Format("@{{Flags={0}; Sequence={1}; Name={2}}}",
                Flags,
                Sequence,
                Name);
        }
    }
}
