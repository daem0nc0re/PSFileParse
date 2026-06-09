using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct nlist {
    //     union {
    // #ifndef __LP64__
    //         char *n_name;
    // #endif
    //         uint32_t n_strx;
    //     } n_un;
    //     uint8_t n_type;
    //     uint8_t n_sect;
    //     int16_t n_desc;
    //     uint32_t n_value;
    // };
    // 
    // struct nlist_64 {
    //     union {
    //         uint32_t  n_strx;
    //     } n_un;
    //     uint8_t n_type;
    //     uint8_t n_sect;
    //     uint16_t n_desc;
    //     uint64_t n_value;
    // };
    // 
    public sealed class SymbolTableEntry
    {
        public UInt32 Index { get; }
        public byte Stab { get; }
        public bool IsPrivateExternal { get; }
        public SymbolNameType NameType { get; }
        public bool IsExternal { get; }
        public SectionNumber SectionNumber { get; }
        public Object DylibOrdinal { get; }
        public SymbolDescription Description { get; }
        public UInt64 Value { get; }
        public String Name { get; }


        internal SymbolTableEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 strtab_offset,
            UInt32 index,
            MachOFlags mhflags,
            bool is_bigendian,
            bool is64bit)
        {
            UInt32 str_index;
            UInt16 desc;
            var b = filebytes[offset + 4];
            Index = index;
            Stab = (byte)(((b & 0xE0) >> 5) & 7);
            IsPrivateExternal = ((b & 0x10) != 0);
            NameType = (SymbolNameType)(b & 0x0E);
            IsExternal = ((b & 0x01) != 0);
            SectionNumber = (SectionNumber)filebytes[offset + 5];

            if (is_bigendian)
            {
                str_index = BinaryHelper.ToUInt32Big(filebytes, offset);
                desc = BinaryHelper.ToUInt16Big(filebytes, offset + 6);

                if (is64bit)
                    Value = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                else
                    Value = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            }
            else
            {
                str_index = BinaryHelper.ToUInt32(filebytes, offset);
                desc = BinaryHelper.ToUInt16(filebytes, offset + 6);

                if (is64bit)
                    Value = BinaryHelper.ToUInt64(filebytes, offset + 8);
                else
                    Value = BinaryHelper.ToUInt32(filebytes, offset + 8);
            }

            if (((mhflags & MachOFlags.TwoLevel) == MachOFlags.TwoLevel) &&
                (((NameType == SymbolNameType.Undefined) && (Value == 0)) || (NameType == SymbolNameType.Prebound)))
            {
                Description = (SymbolDescription)(desc & 0xFF);
                DylibOrdinal = (SymbolOrdinal)((desc >> 8) & 0xFF);
            }
            else
            {
                Description = (SymbolDescription)desc;
            }

            Name = BinaryHelper.GetUTF8String(filebytes, strtab_offset + str_index);
            offset += is64bit ? 16u : 12u;
        }


        public override String ToString()
        {
            return String.Format("@{{Stab={0}; IsPrivateExternal={1}; NameType={2}; IsExternal={3}; SectionNumber={4}; DylibOrdinal={5}; Description={6}; Value={7}; Name={8}}}",
                Stab,
                IsPrivateExternal,
                NameType,
                IsExternal,
                SectionNumber,
                DylibOrdinal,
                Description,
                Value,
                Name);
        }
    }
}
