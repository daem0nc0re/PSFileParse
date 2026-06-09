using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct linker_option_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t count;
    //     // null-byte terminated strings follow
    // };
    // 
    public sealed class LinkerOptionCommand
    {
        public UInt32 Count { get; }
        public StringEntry[] Strings { get; }


        internal LinkerOptionCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 str_offset = offset + 4u;

            if (is_bigendian)
                Count = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                Count = BinaryHelper.ToUInt32(filebytes, offset);

            Strings = new StringEntry[Count];

            for (UInt32 i = 0u; i < Count; i++)
                Strings[i] = new StringEntry(filebytes, ref str_offset, i);
        }


        public override String ToString()
        {
            return String.Format("@{{Strings={0}}}", Strings);
        }
    }
}