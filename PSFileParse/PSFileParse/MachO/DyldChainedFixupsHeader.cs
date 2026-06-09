using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct dyld_chained_fixups_header
    // {
    //     uint32_t fixups_version;
    //     uint32_t starts_offset;
    //     uint32_t imports_offset;
    //     uint32_t symbols_offset;
    //     uint32_t imports_count;
    //     uint32_t imports_format;
    //     uint32_t symbols_format;
    // };
    // 
    public sealed class DyldChainedFixupsHeader
    {
        public UInt32 FixupsVersion { get; }
        public UInt32 StartsOffset { get; }
        public UInt32 ImportsOffset { get; }
        public UInt32 SymbolsOffset { get; }
        public UInt32 ImportsCount { get; }
        public DyldChainedImportsFormat ImportsFormat { get; }
        public DyldChainedSymbolsFormat SymbolsFormat { get; }
        public DyldChainedImport[] Imports { get; }


        internal DyldChainedFixupsHeader(
            byte[] filebytes,
            UInt32 offset,
            DylibTableEntry[] dylibs,
            bool is_bigendian)
        {
            UInt32 imports_base;

            if (is_bigendian)
            {
                FixupsVersion = BinaryHelper.ToUInt32Big(filebytes, offset);
                StartsOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                ImportsOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                SymbolsOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                ImportsCount = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                ImportsFormat = (DyldChainedImportsFormat)BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                SymbolsFormat = (DyldChainedSymbolsFormat)BinaryHelper.ToUInt32Big(filebytes, offset + 24);
            }
            else
            {
                FixupsVersion = BinaryHelper.ToUInt32(filebytes, offset);
                StartsOffset = BinaryHelper.ToUInt32(filebytes, offset + 4);
                ImportsOffset = BinaryHelper.ToUInt32(filebytes, offset + 8);
                SymbolsOffset = BinaryHelper.ToUInt32(filebytes, offset + 12);
                ImportsCount = BinaryHelper.ToUInt32(filebytes, offset + 16);
                ImportsFormat = (DyldChainedImportsFormat)BinaryHelper.ToUInt32(filebytes, offset + 20);
                SymbolsFormat = (DyldChainedSymbolsFormat)BinaryHelper.ToUInt32(filebytes, offset + 24);
            }

            imports_base = offset + ImportsOffset;
            Imports = new DyldChainedImport[ImportsCount];

            for (UInt32 i = 0u; i < ImportsCount; i++)
            {
                Imports[i] = new DyldChainedImport(
                    filebytes,
                    ref imports_base,
                    i,
                    dylibs,
                    ImportsFormat,
                    offset + SymbolsOffset,
                    is_bigendian);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{FixupsVersion={0}; StartsOffset={1}; ImportsOffset={2}; SymbolsOffset={3}; ImportsCount={4}; ImportsFormat={5}; SymbolsFormat={6}; Imports={7}}}",
                FixupsVersion,
                StartsOffset,
                ImportsOffset,
                SymbolsOffset,
                ImportsCount,
                ImportsFormat,
                SymbolsFormat,
                Imports);
        }
    }
}
