using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class AssemblyRefEntry
    {
        public UInt32 Index { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public UInt16 BuildNumber { get; }
        public UInt16 RevisionNumber { get; }
        public AssemblyFlags Flags { get; }
        public byte[] PublicKeyOrToken { get; }
        public String Name { get; }
        public String Culture { get; }
        public byte[] HashValue { get; }


        internal AssemblyRefEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            UInt32 str_table_offset,
            UInt32 blob_table_offset)
        {
            Index = index;
            MajorVersion = BinaryHelper.ToUInt16(filebytes, offset);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, offset + 2);
            BuildNumber = BinaryHelper.ToUInt16(filebytes, offset + 4);
            RevisionNumber = BinaryHelper.ToUInt16(filebytes, offset + 6);
            Flags = (AssemblyFlags)BinaryHelper.ToUInt32(filebytes, offset + 8);
            offset += 12u;

            if (Globals.UseWideBlobIndex)
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt32(filebytes, offset);
                PublicKeyOrToken = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
                offset += 4u;
            }
            else
            {
                var blob_base = blob_table_offset + BinaryHelper.ToUInt16(filebytes, offset);
                PublicKeyOrToken = BlobStreamEntry.ReadBlobStream(filebytes, ref blob_base);
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
            return String.Format("@{{MajorVersion={0}; MinorVersion={1}; BuildNumber={2}; RevisionNumber={3}; Flags={4}; PublicKeyOrToken={5}; Name={6}; Culture={7}; HashValue={8}}}",
                MajorVersion,
                MinorVersion,
                BuildNumber,
                RevisionNumber,
                Flags,
                PublicKeyOrToken,
                Name,
                Culture,
                HashValue);
        }
    }
}
