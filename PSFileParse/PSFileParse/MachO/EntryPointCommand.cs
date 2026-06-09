using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct entry_point_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint64_t entryoff;
    //     uint64_t stacksize;
    // };
    // 
    public sealed class EntryPointCommand
    {
        public UInt64 EntryOffset { get; }
        public UInt64 StackSize { get; }


        internal EntryPointCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                EntryOffset = BinaryHelper.ToUInt64Big(filebytes, offset);
                StackSize = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
            }
            else
            {
                EntryOffset = BinaryHelper.ToUInt64(filebytes, offset);
                StackSize = BinaryHelper.ToUInt64(filebytes, offset + 8);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{EntryOffset={0}; StackSize={1}}}",
                EntryOffset,
                StackSize);
        }
    }
}
