using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class StandAloneSigEntry
    {
        public UInt32 Index { get; }
        public byte[] Signature { get; }


        internal StandAloneSigEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 blob_table_offset)
        {
            Index = index;
            
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
            return String.Format("@{{Signature={0}}}", Signature);
        }
    }
}
