using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class EventEntry
    {
        public UInt32 Index { get; }
        public EventAttributeFlags EventFlags { get; }
        public String Name { get; }
        public CodedIndex EventType { get; }


        internal EventEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset)
        {
            Index = index;
            EventFlags = (EventAttributeFlags)BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;

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

            EventType = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.TypeDefOrRef);
        }


        public override String ToString()
        {
            return String.Format("@{{EventFlags={0}; Name={1}; EventType={2}}}",
                EventFlags,
                Name,
                EventType);
        }
    }
}
