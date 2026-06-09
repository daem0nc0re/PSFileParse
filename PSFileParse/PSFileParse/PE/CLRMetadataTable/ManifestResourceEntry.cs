using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ManifestResourceEntry
    {
        public UInt32 Index { get; }
        public UInt32 Offset { get; }
        public ManifestResourceAttributes Flags { get; }
        public String Name { get; }
        public CodedIndex Implementation { get; }


        internal ManifestResourceEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            Offset = BinaryHelper.ToUInt32(filebytes, offset);
            Flags = (ManifestResourceAttributes)BinaryHelper.ToUInt32(filebytes, offset + 4);
            offset += 8u;

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

            Implementation = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.Implementation);
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; Flags={1}; Name={2}; Implementation={3}}}",
                Offset,
                Flags,
                Name,
                Implementation);
        }
    }
}
