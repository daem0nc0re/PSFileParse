using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class AssemblyEntry
    {
        public UInt32 Index { get; }
        public AssemblyHashAlgorithm HashAlgId { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public UInt16 BuildNumber { get; }
        public UInt16 RevisionNumber { get; }
        public AssemblyFlags Flags { get; }
        public byte[] PublicKey { get; }
        public String Name { get; }
        public String Culture { get; }


        internal AssemblyEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset,
            UInt32 blob_table_offset)
        {
            Index = index;
            HashAlgId = (AssemblyHashAlgorithm)BinaryHelper.ToUInt32(filebytes, offset);
            MajorVersion = BinaryHelper.ToUInt16(filebytes, offset + 4);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, offset + 6);
            BuildNumber = BinaryHelper.ToUInt16(filebytes, offset + 8);
            RevisionNumber = BinaryHelper.ToUInt16(filebytes, offset + 10);
            Flags = (AssemblyFlags)BinaryHelper.ToUInt32(filebytes, offset + 12);
            offset += 16u;

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                PublicKey = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                PublicKey = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 2u;
            }

            if (Globals.UseWideStringIndex)
            {
                var str_index = BinaryHelper.ToUInt32(filebytes, offset);
                Name = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                str_index = BinaryHelper.ToUInt32(filebytes, offset + 4);
                Culture = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 8u;
            }
            else
            {
                var str_index = BinaryHelper.ToUInt16(filebytes, offset);
                Name = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                str_index = BinaryHelper.ToUInt16(filebytes, offset + 2);
                Culture = BinaryHelper.GetUTF8String(filebytes, str_table_offset + str_index);
                offset += 4u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{HashAlgId={0}; MajorVersion={1}; MinorVersion={2}; BuildNumber={3}; RevisionNumber={4}; Flags={5}; PublicKey={6}; Name={7}; Culture={8}}}",
                HashAlgId,
                MajorVersion,
                MinorVersion,
                BuildNumber,
                RevisionNumber,
                Flags,
                PublicKey,
                Name,
                Culture);
        }
    }
}
