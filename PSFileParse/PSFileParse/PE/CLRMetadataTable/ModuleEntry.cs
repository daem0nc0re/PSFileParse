using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ModuleEntry
    {
        public UInt32 Index { get; }
        public UInt16 Generation { get; }
        public String Name { get; }
        public Object MVID { get; }
        public Object EncId { get; }
        public Object EncBaseId { get; }


        internal ModuleEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset,
            GuidStreamEntry[] guids)
        {
            UInt32 name_index;
            UInt32 mvid_index;
            UInt32 encid_index;
            UInt32 encbaseid_index;
            Index = index;
            Generation = BinaryHelper.ToUInt16(filebytes, offset);

            if (Globals.UseWideStringIndex)
            {
                name_index = BinaryHelper.ToUInt32(filebytes, offset + 2);
                offset += 6u;
            }
            else
            {
                name_index = BinaryHelper.ToUInt16(filebytes, offset + 2);
                offset += 4u;
            }

            if (Globals.UseWideGuidIndex)
            {
                mvid_index = BinaryHelper.ToUInt32(filebytes, offset);
                encid_index = BinaryHelper.ToUInt32(filebytes, offset + 4);
                encbaseid_index = BinaryHelper.ToUInt32(filebytes, offset + 8);
                offset += 12u;
            }
            else
            {
                mvid_index = BinaryHelper.ToUInt16(filebytes, offset);
                encid_index = BinaryHelper.ToUInt16(filebytes, offset + 2);
                encbaseid_index = BinaryHelper.ToUInt16(filebytes, offset + 4);
                offset += 6u;
            }

            Name = BinaryHelper.GetUTF8String(filebytes, str_table_offset + name_index);
            MVID = guids[mvid_index];
            EncId = guids[encid_index];
            EncBaseId = guids[encbaseid_index];
        }


        public override String ToString()
        {
            return String.Format("@{{Generation={0}; Name={1}; MVID={2}; EncId={3}; EncBaseId={4}}}",
                Generation,
                Name,
                MVID,
                EncId,
                EncBaseId);
        }
    }
}
