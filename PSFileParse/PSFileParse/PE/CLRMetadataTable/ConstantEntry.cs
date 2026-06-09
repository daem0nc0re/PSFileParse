using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class ConstantEntry
    {
        public UInt32 Index { get; }
        public byte Type { get; }
        public byte Padding { get; }
        public CodedIndex Parent { get; }
        public byte[] Value { get; }


        internal ConstantEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 blob_table_offset)
        {
            Index = index;
            Type = filebytes[offset++];
            Padding = filebytes[offset++];
            Parent = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.HasConstant);

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
            return String.Format("@{{Type={0}; Padding={1}; Parent={2}; Value={3}}}",
                Type,
                Padding,
                Parent,
                Value);
        }
    }
}
