using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct dylib_module {
    //     uint32_t module_name;
    //     uint32_t iextdefsym;
    //     uint32_t nextdefsym;
    //     uint32_t irefsym;
    //     uint32_t nrefsym;
    //     uint32_t ilocalsym;
    //     uint32_t nlocalsym;
    //     uint32_t iextrel;
    //     uint32_t nextrel;
    //     uint32_t iinit_iterm;
    //     uint32_t ninit_nterm;
    //     uint32_t objc_module_info_addr;
    //     uint32_t objc_module_info_size;
    // };
    // 
    // struct dylib_module_64 {
    //     uint32_t module_name;
    //     uint32_t iextdefsym;
    //     uint32_t nextdefsym;
    //     uint32_t irefsym;
    //     uint32_t nrefsym;
    //     uint32_t ilocalsym;
    //     uint32_t nlocalsym;
    //     uint32_t iextrel;
    //     uint32_t nextrel;
    //     uint32_t iinit_iterm;
    //     uint32_t ninit_nterm;
    //     uint32_t objc_module_info_size;
    //     uint64_t objc_module_info_addr;
    // };
    // 
    public sealed class DylibModule
    {
        public UInt32 Index { get; }
        public String ModuleName { get; }
        public UInt32 IndexOfExtDefSymbols { get; }
        public UInt32 NumberOfExtDefSymbols { get; }
        public UInt32 IndexOfReferencedSymbols { get; }
        public UInt32 NumberOfReferencedSymbols { get; }
        public UInt32 IndexOfLocalSymbols { get; }
        public UInt32 NumberOfLocalSymbols { get; }
        public UInt32 IndexOfExtRelocs { get; }
        public UInt32 NumberOfExtRelocs { get; }
        public UInt16 IndexOfInitSection { get; }
        public UInt16 IndexOfTermSection { get; }
        public UInt16 NumberOfInitSectionEntries { get; }
        public UInt16 NumberOfTermSectionEntries { get; }
        public UInt64 ObjCModuleInfoAddress { get; }
        public UInt32 ObjCModuleInfoSize { get; }


        internal DylibModule(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 strtab_offset,
            UInt32 index,
            bool is_bigendian,
            bool is64bit)
        {
            UInt32 str_index;
            Index = index;

            if (is_bigendian)
            {
                str_index = BinaryHelper.ToUInt32Big(filebytes, offset);
                IndexOfExtDefSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                NumberOfExtDefSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                IndexOfReferencedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                NumberOfReferencedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                IndexOfLocalSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                NumberOfLocalSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 24);
                IndexOfExtRelocs = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
                NumberOfExtRelocs = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                IndexOfInitSection = BinaryHelper.ToUInt16Big(filebytes, offset + 36);
                IndexOfTermSection = BinaryHelper.ToUInt16Big(filebytes, offset + 38);
                NumberOfInitSectionEntries = BinaryHelper.ToUInt16Big(filebytes, offset + 40);
                NumberOfTermSectionEntries = BinaryHelper.ToUInt16Big(filebytes, offset + 42);

                if (is64bit)
                {
                    ObjCModuleInfoSize = BinaryHelper.ToUInt32Big(filebytes, offset + 44);
                    ObjCModuleInfoAddress = BinaryHelper.ToUInt64Big(filebytes, offset + 48);
                }
                else
                {
                    ObjCModuleInfoAddress = BinaryHelper.ToUInt32Big(filebytes, offset + 44);
                    ObjCModuleInfoSize = BinaryHelper.ToUInt32Big(filebytes, offset + 48);
                }
            }
            else
            {
                str_index = BinaryHelper.ToUInt32(filebytes, offset);
                IndexOfExtDefSymbols = BinaryHelper.ToUInt32(filebytes, offset + 4);
                NumberOfExtDefSymbols = BinaryHelper.ToUInt32(filebytes, offset + 8);
                IndexOfReferencedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 12);
                NumberOfReferencedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 16);
                IndexOfLocalSymbols = BinaryHelper.ToUInt32(filebytes, offset + 20);
                NumberOfLocalSymbols = BinaryHelper.ToUInt32(filebytes, offset + 24);
                IndexOfExtRelocs = BinaryHelper.ToUInt32(filebytes, offset + 28);
                NumberOfExtRelocs = BinaryHelper.ToUInt32(filebytes, offset + 32);
                IndexOfInitSection = BinaryHelper.ToUInt16(filebytes, offset + 36);
                IndexOfTermSection = BinaryHelper.ToUInt16(filebytes, offset + 38);
                NumberOfInitSectionEntries = BinaryHelper.ToUInt16(filebytes, offset + 40);
                NumberOfTermSectionEntries = BinaryHelper.ToUInt16(filebytes, offset + 42);

                if (is64bit)
                {
                    ObjCModuleInfoSize = BinaryHelper.ToUInt32(filebytes, offset + 44);
                    ObjCModuleInfoAddress = BinaryHelper.ToUInt64(filebytes, offset + 48);
                }
                else
                {
                    ObjCModuleInfoAddress = BinaryHelper.ToUInt32(filebytes, offset + 44);
                    ObjCModuleInfoSize = BinaryHelper.ToUInt32(filebytes, offset + 48);
                }
            }

            ModuleName = BinaryHelper.GetUTF8String(filebytes, strtab_offset + str_index);
            offset += is64bit ? 56u : 52u;
        }


        public override String ToString()
        {
            return String.Format("@{{ModuleName={0}; IndexOfExtDefSymbols={1}; NumberOfExtDefSymbols={2}; IndexOfReferencedSymbols={3}; NumberOfReferencedSymbols={4}; IndexOfLocalSymbols={5}; NumberOfLocalSymbols={6}; IndexOfExtRelocs={7}; NumberOfExtRelocs={8}; IndexOfInitSection={9}; IndexOfTermSection={10}; NumberOfInitSectionEntries={11}; NumberOfTermSectionEntries={12}; ObjCModuleInfoAddress={13}; ObjCModuleInfoSize={14}}}",
                ModuleName,
                IndexOfExtDefSymbols,
                NumberOfExtDefSymbols,
                IndexOfReferencedSymbols,
                NumberOfReferencedSymbols,
                IndexOfLocalSymbols,
                NumberOfLocalSymbols,
                IndexOfExtRelocs,
                NumberOfExtRelocs,
                IndexOfInitSection,
                IndexOfTermSection,
                NumberOfInitSectionEntries,
                NumberOfTermSectionEntries,
                ObjCModuleInfoAddress,
                ObjCModuleInfoSize);
        }
    }
}
