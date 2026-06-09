using PSFileParse.Auxiliary;
using System;
using System.Text;

namespace PSFileParse.PE
{
    //  
    // struct IMAGE_RESOURCE_DIRECTORY_ENTRY
    // {
    //     union {
    //         struct {
    //             int NameOffset : 31;
    //             int NameIsString : 1;
    //         }
    //         int            Name;
    //         unsigned short Id;
    //     };
    //     union {
    //         struct {
    //             int OffsetToDirectory : 31;
    //             int DataIsDirectory : 1;
    //         }
    //         int OffsetToData;
    //     };
    // };
    // 
    public sealed class ImageResourceDirectoryEntry
    {
        public UInt32 Index { get; }
        public String Identifier { get; }
        public Object Content { get; }
        internal bool IsDirectory { get; }
        internal String TypeId { get; }
        internal String NameId { get; }
        internal String LanguageId { get; }
        internal static readonly UInt32 SizeOfStruct = 0x8;


        internal ImageResourceDirectoryEntry(
            byte[] filebytes,
            UInt32 index,
            UInt32 dir_base,
            UInt32 entry_offset,
            UInt32 delta,
            String type_id,
            String name_id,
            String lang_id)
        {
            var name_union = BinaryHelper.ToUInt32(filebytes, entry_offset);
            var data_union = BinaryHelper.ToUInt32(filebytes, entry_offset + 4);
            Index = index;
            IsDirectory = (data_union & 0x80000000) != 0;
            TypeId = type_id;
            NameId = name_id;
            LanguageId = lang_id;

            if ((name_union & 0x80000000) != 0)
            {
                //
                // struct IMAGE_RESOURCE_DIRECTORY_STRING
                // {
                //     unsigned short Length;
                //     unsigned short NameString[1]; // Unicode bytes
                // };
                //
                var name_offset = dir_base + (UInt32)(name_union & 0x7FFFFFFF);
                var name_length = BinaryHelper.ToUInt16(filebytes, name_offset);
                var name_bytes = new byte[name_length * 2];

                for (UInt32 i = 0; i < (UInt32)name_bytes.Length; i++)
                    name_bytes[i] = filebytes[name_offset + 2 + i];

                Identifier = Encoding.Unicode.GetString(name_bytes);

                if (TypeId == null)
                    TypeId = Identifier;
                else if (NameId == null)
                    NameId = Identifier;
                else
                    LanguageId = Identifier;
            }
            else
            {
                if (TypeId == null)
                {
                    Identifier = ((ResourceType)name_union).ToString();
                    TypeId = Identifier;
                }
                else
                {
                    Identifier = name_union.ToString();

                    if (NameId == null)
                        NameId = Identifier;
                    else
                        LanguageId = Identifier;
                }
            }

            if (IsDirectory)
            {
                Content = new ImageResourceDirectory(
                    filebytes,
                    dir_base,
                    (UInt32)(data_union & 0x7FFFFFFF),
                    delta,
                    TypeId,
                    NameId,
                    LanguageId);
            }
            else
            {
                Content = new ImageResourceDataEntry(
                    filebytes,
                    dir_base,
                    data_union,
                    delta,
                    TypeId,
                    NameId,
                    LanguageId);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Identifier={0}; Content={1}}}",
                Identifier,
                Content);
        }
    }
}