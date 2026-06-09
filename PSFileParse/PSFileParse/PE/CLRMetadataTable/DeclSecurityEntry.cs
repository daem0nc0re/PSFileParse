using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class DeclSecurityEntry
    {
        public UInt32 Index { get; }
        public UInt16 Action { get; }
        public CodedIndex Parent { get; }
        public byte[] PermissionSet { get; }


        internal DeclSecurityEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 blob_table_offset)
        {
            Index = index;
            Action = BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;
            Parent = new CodedIndex(filebytes, ref offset, CodedIndex.TypeId.HasDeclSecurity);

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                PermissionSet = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                PermissionSet = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 2u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Action={0}; Parent={1}; PermissionSet={2}}}",
                Action,
                Parent,
                PermissionSet);
        }
    }
}
