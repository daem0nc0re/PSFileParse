using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // // DYLD_CHAINED_IMPORT
    // struct dyld_chained_import
    // {
    //     uint32_t lib_ordinal :  8,
    //              weak_import :  1,
    //              name_offset : 23;
    // };
    // 
    // // DYLD_CHAINED_IMPORT_ADDEND
    // struct dyld_chained_import_addend
    // {
    //     uint32_t lib_ordinal :  8,
    //              weak_import :  1,
    //              name_offset : 23;
    //     int32_t  addend;
    // };
    // 
    // // DYLD_CHAINED_IMPORT_ADDEND64
    // struct dyld_chained_import_addend64
    // {
    //     uint64_t lib_ordinal : 16,
    //              weak_import :  1,
    //              reserved    : 15,
    //              name_offset : 32;
    //     uint64_t addend;
    // };
    // 
    public sealed class DyldChainedImport
    {
        public UInt32 Index { get; }
        public BindDylibOrdinal Ordinal { get; }
        public String DylibName { get; }
        public bool WeakImport { get; }
        public UInt32 NameOffset { get; }
        public Object Addend { get; }
        public String Name { get; }


        internal DyldChainedImport(
            byte[] data,
            ref UInt32 offset,
            UInt32 index,
            DylibTableEntry[] dylibs,
            DyldChainedImportsFormat fmt,
            UInt32 sym_base,
            bool is_bigendian)
        {
            Index = index;

            if (is_bigendian)
            {
                if (fmt == DyldChainedImportsFormat.Import)
                {
                    var value = BinaryHelper.ToUInt32Big(data, offset);
                    Ordinal = (BindDylibOrdinal)((sbyte)(value & 0xFF));
                    WeakImport = ((value & 0x100) != 0);
                    NameOffset = (UInt32)((value >> 9) & 0x7FFFFF);
                    offset += 4u;
                }
                else if (fmt == DyldChainedImportsFormat.ImportAddend)
                {
                    var value = BinaryHelper.ToUInt32Big(data, offset);
                    Ordinal = (BindDylibOrdinal)((sbyte)(value & 0xFF));
                    WeakImport = ((value & 0x100) != 0);
                    NameOffset = (UInt32)((value >> 9) & 0x7FFFFF);
                    Addend = BinaryHelper.ToUInt32Big(data, offset + 4);
                    offset += 8u;
                }
                else if (fmt == DyldChainedImportsFormat.ImportAddend64)
                {
                    var value = BinaryHelper.ToUInt64Big(data, offset);
                    Ordinal = (BindDylibOrdinal)(value & 0xFFFF);
                    WeakImport = ((value & 0x10000) != 0);
                    NameOffset = (UInt32)((value >> 32) & 0xFFFFFFFF);
                    Addend = BinaryHelper.ToUInt64Big(data, offset + 4);
                    offset += 16u;
                }
            }
            else
            {
                if (fmt == DyldChainedImportsFormat.Import)
                {
                    var value = BinaryHelper.ToUInt32(data, offset);
                    Ordinal = (BindDylibOrdinal)((sbyte)(value & 0xFF));
                    WeakImport = ((value & 0x100) != 0);
                    NameOffset = (UInt32)((value >> 9) & 0x7FFFFF);
                    offset += 4u;
                }
                else if (fmt == DyldChainedImportsFormat.ImportAddend)
                {
                    var value = BinaryHelper.ToUInt32(data, offset);
                    Ordinal = (BindDylibOrdinal)((sbyte)(value & 0xFF));
                    WeakImport = ((value & 0x100) != 0);
                    NameOffset = (UInt32)((value >> 9) & 0x7FFFFF);
                    Addend = BinaryHelper.ToUInt32(data, offset + 4);
                    offset += 8u;
                }
                else if (fmt == DyldChainedImportsFormat.ImportAddend64)
                {
                    var value = BinaryHelper.ToUInt64(data, offset);
                    Ordinal = (BindDylibOrdinal)(value & 0xFFFF);
                    WeakImport = ((value & 0x10000) != 0);
                    NameOffset = (UInt32)((value >> 32) & 0xFFFFFFFF);
                    Addend = BinaryHelper.ToUInt64(data, offset + 4);
                    offset += 16u;
                }
            }

            if (Ordinal > BindDylibOrdinal.Self)
            {
                var ord = (UInt32)Ordinal - 1;

                if (ord < dylibs.Length)
                    DylibName = dylibs[ord].Name;
            }

            Name = BinaryHelper.GetUTF8String(data, sym_base + NameOffset);
        }


        public override String ToString()
        {
            return String.Format("@{{Ordinal={0}; DylibName={1}; WeakImport={2}; NameOffset={3}; Addend={4}; Name={5}}}",
                Ordinal,
                DylibName,
                WeakImport,
                NameOffset,
                Addend,
                Name);
        }
    }
}
