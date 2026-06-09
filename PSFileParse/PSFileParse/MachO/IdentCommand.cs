using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    // 
    // struct ident_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    // };
    // 
    public sealed class IdentCommand
    {
        public StringEntry[] Strings { get; }


        internal IdentCommand(byte[] filebytes, UInt32 offset, UInt32 size)
        {
            var index = 0u;
            var str_list = new List<StringEntry>();
            var end = offset + size;

            while (offset < end)
            {
                var str = new StringEntry(filebytes, ref offset, index);

                if (String.IsNullOrEmpty(str.String))
                    continue;

                str_list.Add(str);
                index++;
            }

            Strings = str_list.ToArray();
        }


        public override String ToString()
        {
            return String.Format("@{{Strings={0}}}", Strings);
        }
    }
}