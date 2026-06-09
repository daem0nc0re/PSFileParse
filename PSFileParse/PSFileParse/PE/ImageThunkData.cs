using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_THUNK_DATA64 {
    //     union {
    //         unsigned long long ForwarderString;  // PBYTE 
    //         unsigned long long Function;         // PDWORD
    //         unsigned long long Ordinal;
    //         unsigned long long AddressOfData;    // PIMAGE_IMPORT_BY_NAME
    //     } u1;
    // };
    // 
    // struct IMAGE_THUNK_DATA32 {
    //     union {
    //         unsigned int ForwarderString;  // PBYTE 
    //         unsigned int Function;         // PDWORD
    //         unsigned int Ordinal;
    //         unsigned int AddressOfData;    // PIMAGE_IMPORT_BY_NAME
    //     } u1;
    // };
    // 
    public sealed class ImageThunkData
    {
        public Object Hint { get; }
        public Object Ordinal { get; }
        public String Name { get; }


        internal ImageThunkData(
            byte[] filebytes,
            UInt32 offset,
            UInt32 delta,
            bool is_64bit)
        {
            if (is_64bit)
            {
                var addr_of_data = BinaryHelper.ToUInt64(filebytes, offset);

                if (((addr_of_data >> 63) & 1) == 1)
                {
                    Ordinal = (UInt16)(addr_of_data & 0xFFFF);
                }
                else
                {
                    var hint_name_base = (UInt32)(addr_of_data & 0x7FFFFFFF) - delta;
                    var hint_name = new ImageImportByName(filebytes, hint_name_base);
                    Hint = hint_name.Hint;
                    Name = hint_name.Name;
                }
            }
            else
            {
                var addr_of_data = BinaryHelper.ToUInt32(filebytes, offset);

                if ((addr_of_data & 0x80000000) != 0)
                {
                    Ordinal = (UInt16)(addr_of_data & 0xFFFF);
                }
                else
                {
                    var hint_name_base = (UInt32)(addr_of_data & 0x7FFFFFFF) - delta;
                    var hint_name = new ImageImportByName(filebytes, hint_name_base);
                    Hint = hint_name.Hint;
                    Name = hint_name.Name;
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Hint={0}; Ordinal={1}; Name={2}}}",
                Hint,
                Ordinal,
                Name);
        }
    }
}
