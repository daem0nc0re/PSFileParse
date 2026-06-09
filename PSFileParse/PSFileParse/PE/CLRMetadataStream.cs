using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE
{
    // 
    // struct CLR_METADATA_STREAM_HEADER
    // {
    //     unsigned int  Offset;
    //     unsigned int  Size;
    //     unsigned char Name[1]; // Null-terminated ASCII and padding bytes for 4 bytes alignment
    // };
    // 
    public sealed class CLRMetadataStream
    {
        public UInt32 Index { get; }
        public UInt32 Offset { get; }
        public UInt32 Size { get; }
        public String Name { get; }
        public Object Data { get; }
        internal UInt32 SizeOfHeader { get; }


        internal CLRMetadataStream(
            byte[] filebytes,
            UInt32 offset,
            UInt32 index,
            UInt32 metadata_base,
            UInt32 str_table_offset,
            GuidStreamEntry[] guid_table,
            UInt32 blob_table_offset)
        {
            UInt32 name_len;
            Index = index;
            Offset = BinaryHelper.ToUInt32(filebytes, offset);
            Size = BinaryHelper.ToUInt32(filebytes, offset + 4);
            Name = BinaryHelper.GetAnsiString(filebytes, offset + 8);
            name_len = (UInt32)Name.Length + 1u;

            if ((name_len % 4u) != 0)
                name_len += 4u - (name_len % 4u);

            SizeOfHeader = 8u + name_len;

            if (Size > 0)
            {
                UInt32 cursor = metadata_base + Offset;

                if (Name == "#~")
                {
                    Data = new TildeStreamEntry(
                        filebytes,
                        cursor,
                        str_table_offset,
                        guid_table,
                        blob_table_offset);
                }
                else if (Name == "#Strings")
                {
                    UInt32 i = 0;
                    var boundary = cursor + Size;
                    var str_table = new List<StringsStreamEntry>();

                    while (cursor < boundary)
                    {
                        var str = new StringsStreamEntry(
                            filebytes,
                            ref cursor,
                            i++);
                        str_table.Add(str);
                    }

                    Data = str_table.ToArray();
                }
                else if (Name == "#US")
                {
                    UInt32 i = 0;
                    var boundary = cursor + Size;
                    var us_table = new List<UserStringsStreamEntry>();

                    while (cursor < boundary)
                    {
                        var ustr = new UserStringsStreamEntry(
                            filebytes,
                            ref cursor,
                            i++);
                        us_table.Add(ustr);
                    }

                    Data = us_table.ToArray();
                }
                else if (Name == "#GUID")
                {
                    var count = (UInt32)(Size / 16) + 1u;
                    // .NET metadata uses 1 based index. So insert null entry to index 0.
                    var guid_array = new GuidStreamEntry[count];

                    for (UInt32 i = 1u; i < count; i++)
                    {
                        guid_array[i] = new GuidStreamEntry(
                            filebytes,
                            ref cursor,
                            i);
                    }

                    Data = guid_array;
                }
                else if (Name == "#Blob")
                {
                    UInt32 i = 0;
                    var boundary = cursor + Size;
                    var blob_table = new List<BlobStreamEntry>();

                    while (cursor < boundary)
                    {
                        var blob = new BlobStreamEntry(
                            filebytes,
                            ref cursor,
                            i++);
                        blob_table.Add(blob);
                    }

                    Data = blob_table.ToArray();
                }
                else
                {
                    var data_bytes = new byte[Size];

                    for (UInt32 i = 0; i < Size; i++)
                        data_bytes[i] = filebytes[cursor++];

                    Data = data_bytes;
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; Size={1}; Name={2}; Data={3}}}",
                Offset,
                Size,
                Name,
                Data);
        }
    }
}
