using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class MethodDefEntry
    {
        public UInt32 Index { get; }
        public UInt32 RVA { get; }
        public MethodImplFlags ImplFlags { get; }
        public MethodFlags Flags { get; }
        public String Name { get; }
        public byte[] Signature { get; }
        public TableIndex ParamList { get; }


        internal MethodDefEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset,
            UInt32 blob_table_offset)
        {
            Index = index;
            RVA = BinaryHelper.ToUInt32(filebytes, offset);
            offset += 4u;
            ImplFlags = new MethodImplFlags(filebytes, ref offset);
            Flags = new MethodFlags(filebytes, ref offset);

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

            ParamList = new TableIndex(filebytes, ref offset, MetadataTableIdentifier.Param);
        }


        public override String ToString()
        {
            return String.Format("@{{RVA={0}; ImplFlags={1}; Flags={2}; Name={3}; Signature={4}; ParamList={5}}}",
                RVA,
                ImplFlags,
                Flags,
                Name,
                Signature,
                ParamList);
        }
    }
}
