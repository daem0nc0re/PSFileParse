using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class CustomAttributeEntry
    {
        public UInt32 Index { get; }
        public CodedIndex Parent { get; }
        public CodedIndex Type { get; }
        public byte[] Value { get; }


        internal CustomAttributeEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 blob_table_offset)
        {
            Index = index;
            Parent = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.HasCustomAttribute);
            Type = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.CustomAttributeType);

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                Value = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                Value = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 2u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Parent={0}; Type={1}; Value={2}}}",
                Parent,
                Type,
                Value);
        }
    }
}
