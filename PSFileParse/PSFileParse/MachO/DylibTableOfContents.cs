using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct dylib_table_of_contents {
    //     uint32_t symbol_index;
    //     uint32_t module_index;
    // };
    // 
    public sealed class DylibTableOfContents
    {
        public UInt32 Index { get; }
        public UInt32 SymbolIndex { get; }
        public UInt32 ModuleIndex { get; }
        public String SymbolName { get; }
        public String ModuleName { get; }


        internal DylibTableOfContents(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            SymbolTableEntry[] symbols,
            DylibModule[] modules,
            bool is_bigendian)
        {
            Index = index;

            if (is_bigendian)
            {
                SymbolIndex = BinaryHelper.ToUInt32Big(filebytes, offset);
                ModuleIndex = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                SymbolIndex = BinaryHelper.ToUInt32(filebytes, offset);
                ModuleIndex = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            try
            {
                SymbolName = symbols[(UInt32)SymbolIndex].Name;

                if (modules != null)
                    ModuleName = modules[(UInt32)ModuleIndex].ModuleName;
            }
            catch { }

            offset += 8u;
        }


        public override String ToString()
        {
            return String.Format("@{{SymbolIndex={0}; ModuleIndex={1}; SymbolName={2}; ModuleName={3}}}",
                SymbolIndex,
                ModuleIndex,
                SymbolName,
                ModuleName);
        }
    }
}
