using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct symseg_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t offset;
    //     uint32_t size;
    // };
    // 
    public sealed class SymSegCommand
    {
        public UInt32 Offset { get; }
        public UInt32 Size { get; }


        internal SymSegCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                Offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                Size = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                Offset = BinaryHelper.ToUInt32(filebytes, offset);
                Size = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; Size={1}}}", Offset, Size);
        }
    }
}