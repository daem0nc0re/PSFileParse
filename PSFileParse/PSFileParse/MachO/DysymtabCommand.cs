using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    // 
    // struct dysymtab_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t ilocalsym;
    //     uint32_t nlocalsym;
    //     uint32_t iextdefsym;
    //     uint32_t nextdefsym;
    //     uint32_t iundefsym;
    //     uint32_t nundefsym;
    //     uint32_t tocoff;
    //     uint32_t ntoc;
    //     uint32_t modtaboff;
    //     uint32_t nmodtab;
    //     uint32_t extrefsymoff;
    //     uint32_t nextrefsyms;
    //     uint32_t indirectsymoff; 
    //     uint32_t nindirectsyms;  
    //     uint32_t extreloff;
    //     uint32_t nextrel;
    //     uint32_t locreloff;
    //     uint32_t nlocrel;
    // };
    // 
    public sealed class DysymtabCommand
    {
        public UInt32 IndexToLocalSymbol { get; }
        public UInt32 NumberOfLocalSymbols { get; }
        public UInt32 IndexToExtDefinedSymbols { get; }
        public UInt32 NumberOfExtDefinedSymbols { get; }
        public UInt32 IndexToUndefinedSymbols { get; }
        public UInt32 NumberOfUndefinedSymbols { get; }
        public UInt32 TableOfContentsOffset { get; }
        public UInt32 NumberOfContents { get; }
        public UInt32 ModuleTableOffset { get; }
        public UInt32 NumberOfModules { get; }
        public UInt32 ReferencedSymbolTableOffset { get; }
        public UInt32 NumberOfReferencedSymbols { get; }
        public UInt32 IndirectSymbolTableOffset { get; }
        public UInt32 NumberOfIndirectSymbols { get; }
        public UInt32 ExtRelocOffset { get; }
        public UInt32 NumberOfExtRelocs { get; }
        public UInt32 LocalRelocOffset { get; }
        public UInt32 NumberOfLocalRelocs { get; }


        internal DysymtabCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                IndexToLocalSymbol = BinaryHelper.ToUInt32Big(filebytes, offset);
                NumberOfLocalSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                IndexToExtDefinedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                NumberOfExtDefinedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                IndexToUndefinedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                NumberOfUndefinedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                TableOfContentsOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 24);
                NumberOfContents = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
                ModuleTableOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                NumberOfModules = BinaryHelper.ToUInt32Big(filebytes, offset + 36);
                ReferencedSymbolTableOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 40);
                NumberOfReferencedSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 44);
                IndirectSymbolTableOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 48);
                NumberOfIndirectSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 52);
                ExtRelocOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 56);
                NumberOfExtRelocs = BinaryHelper.ToUInt32Big(filebytes, offset + 60);
                LocalRelocOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 64);
                NumberOfLocalRelocs = BinaryHelper.ToUInt32Big(filebytes, offset + 68);
            }
            else
            {
                IndexToLocalSymbol = BinaryHelper.ToUInt32(filebytes, offset);
                NumberOfLocalSymbols = BinaryHelper.ToUInt32(filebytes, offset + 4);
                IndexToExtDefinedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 8);
                NumberOfExtDefinedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 12);
                IndexToUndefinedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 16);
                NumberOfUndefinedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 20);
                TableOfContentsOffset = BinaryHelper.ToUInt32(filebytes, offset + 24);
                NumberOfContents = BinaryHelper.ToUInt32(filebytes, offset + 28);
                ModuleTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 32);
                NumberOfModules = BinaryHelper.ToUInt32(filebytes, offset + 36);
                ReferencedSymbolTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 40);
                NumberOfReferencedSymbols = BinaryHelper.ToUInt32(filebytes, offset + 44);
                IndirectSymbolTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 48);
                NumberOfIndirectSymbols = BinaryHelper.ToUInt32(filebytes, offset + 52);
                ExtRelocOffset = BinaryHelper.ToUInt32(filebytes, offset + 56);
                NumberOfExtRelocs = BinaryHelper.ToUInt32(filebytes, offset + 60);
                LocalRelocOffset = BinaryHelper.ToUInt32(filebytes, offset + 64);
                NumberOfLocalRelocs = BinaryHelper.ToUInt32(filebytes, offset + 68);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{IndexToLocalSymbol={0}; NumberOfLocalSymbols={1}; IndexToExtDefinedSymbols={2}; NumberOfExtDefinedSymbols={3}; IndexToUndefinedSymbols={4}; NumberOfUndefinedSymbols={5}; TableOfContentsOffset={6}; NumberOfContents={7}; ModuleTableOffset={8}; NumberOfModules={9}; ReferencedSymbolTableOffset={10}; NumberOfReferencedSymbols={11}; IndirectSymbolTableOffset={12}; NumberOfIndirectSymbols={13}; ExtRelocOffset={14}; NumberOfExtRelocs={15}; LocalRelocOffset={16}; NumberOfLocalRelocs={17}}}",
                IndexToLocalSymbol,
                NumberOfLocalSymbols,
                IndexToExtDefinedSymbols,
                NumberOfExtDefinedSymbols,
                IndexToUndefinedSymbols,
                NumberOfUndefinedSymbols,
                TableOfContentsOffset,
                NumberOfContents,
                ModuleTableOffset,
                NumberOfModules,
                ReferencedSymbolTableOffset,
                NumberOfReferencedSymbols,
                IndirectSymbolTableOffset,
                NumberOfIndirectSymbols,
                ExtRelocOffset,
                NumberOfExtRelocs,
                LocalRelocOffset,
                NumberOfLocalRelocs);
        }
    }
}