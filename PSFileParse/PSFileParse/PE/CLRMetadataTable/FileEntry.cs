using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class FileEntry
    {
        public UInt32 Index { get; }
        public FileAttributes Flags { get; }
        public String Name { get; }
        public byte[] HashValue { get; }


        internal FileEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset,
            UInt32 blob_table_offset)
        {
            Index = index;
            Flags = (FileAttributes)BinaryHelper.ToUInt32(filebytes, offset);
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

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                HashValue = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                HashValue = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 2u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Flags={0}; Name={1}; HashValue={2}}}",
                Flags,
                Name,
                HashValue);
        }
    }
}
