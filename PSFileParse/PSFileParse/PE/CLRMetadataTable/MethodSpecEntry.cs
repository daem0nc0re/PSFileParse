using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class MethodSpecEntry
    {
        public UInt32 Index { get; }
        public CodedIndex Method { get; }
        public byte[] Instantiation { get; }


        internal MethodSpecEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 blob_table_offset)
        {
            Index = index;
            Method = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.MethodDefOrRef);

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                Instantiation = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                Instantiation = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 2u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Method={0}; Instantiation={1}}}",
                Method,
                Instantiation);
        }
    }
}
