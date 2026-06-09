using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class FieldEntry
    {
        public UInt32 Index { get; }
        public FieldFlags Flags { get; }
        public String Name { get; }
        public byte[] Signature { get; }


        internal FieldEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset,
            UInt32 blob_table_offset)
        {
            Index = index;
            Flags = new FieldFlags(filebytes, ref offset);

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

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                Signature = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                Signature = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 2u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Flags={0}; Name={1}; Signature={2}}}",
                Flags,
                Name,
                Signature);
        }
    }
}
