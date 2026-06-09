using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct dylib_reference {
    //     uint32_t isym:24,
    //              flags:8;
    // };
    // 
    public sealed class DylibReference
    {
        public UInt32 Index { get; }
        public UInt32 SymbolIndex { get; }
        public DylibReferenceTypes Flags { get; }
        public String Name { get; }


        internal DylibReference(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            SymbolTableEntry[] symbols,
            bool is_bigendian)
        {
            UInt32 sym_index;
            Index = index;

            if (is_bigendian)
                sym_index = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                sym_index = BinaryHelper.ToUInt32(filebytes, offset);

            Flags = (DylibReferenceTypes)((sym_index >> 24) & 0xFF);
            SymbolIndex = sym_index & 0x00FFFFFF;

            try
            {
                Name = symbols[(UInt32)SymbolIndex].Name;
            }
            catch { }

            offset += 4u;
        }


        public override String ToString()
        {
            return String.Format("@{{SymbolIndex={0}; Flags={1}; Name={2}}}",
                SymbolIndex,
                Flags,
                Name);
        }
    }
}
