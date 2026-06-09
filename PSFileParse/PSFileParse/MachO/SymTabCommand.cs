using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct symtab_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t symoff;
    //     uint32_t nsyms;
    //     uint32_t stroff;
    //     uint32_t strsize;
    // };
    // 
    public sealed class SymTabCommand
    {
        public UInt32 SymbolOffset { get; }
        public UInt32 NumberOfSymbols { get; }
        public UInt32 StringOffset { get; }
        public UInt32 StringSize { get; }


        internal SymTabCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                SymbolOffset = BinaryHelper.ToUInt32Big(filebytes, offset);
                NumberOfSymbols = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                StringOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                StringSize = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            }
            else
            {
                SymbolOffset = BinaryHelper.ToUInt32(filebytes, offset);
                NumberOfSymbols = BinaryHelper.ToUInt32(filebytes, offset + 4);
                StringOffset = BinaryHelper.ToUInt32(filebytes, offset + 8);
                StringSize = BinaryHelper.ToUInt32(filebytes, offset + 12);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{SymbolOffset={0}; NumberOfSymbols={1}; StringOffset={2}; StringSize={3}}}",
                SymbolOffset,
                NumberOfSymbols,
                StringOffset,
                StringSize);
        }
    }
}
