using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //  
    // struct IMAGE_RESOURCE_DATA_ENTRY
    // {
    //     unsigned int OffsetToData;
    //     unsigned int Size;
    //     unsigned int CodePage;
    //     unsigned int Reserved;
    // };
    // 
    public sealed class ImageResourceDataEntry
    {
        public UInt32 OffsetToData { get; }
        public UInt32 Size { get; }
        public UInt32 CodePage { get; }
        public UInt32 Reserved { get; }
        public byte[] Data { get; }
        internal String TypeId { get; }
        internal String NameId { get; }
        internal String LanguageId { get; }
        internal static readonly UInt32 SizeOfStruct = 0x10;


        internal ImageResourceDataEntry(
            byte[] filebytes,
            UInt32 dir_base,
            UInt32 offset,
            UInt32 delta,
            String type_id,
            String name_id,
            String lang_id)
        {
            var data_offset = dir_base + offset;
            OffsetToData = BinaryHelper.ToUInt32(filebytes, data_offset);
            Size = BinaryHelper.ToUInt32(filebytes, data_offset + 4);
            CodePage = BinaryHelper.ToUInt32(filebytes, data_offset + 8);
            Reserved = BinaryHelper.ToUInt32(filebytes, data_offset + 12);
            TypeId = type_id;
            NameId = name_id;
            LanguageId = lang_id;

            if (Size > 0)
            {
                Data = new byte[Size];
                data_offset = OffsetToData - delta;

                for (UInt32 i = 0; i < Size; i++)
                    Data[i] = filebytes[data_offset + i];
            }
        }


        public override String ToString()
        {
            return String.Format("@{{OffsetToData={0}; Size={1}; CodePage={2}; Reserved={3}; Data={4}}}",
                OffsetToData,
                Size,
                CodePage,
                Reserved,
                Data);
        }
    }
}