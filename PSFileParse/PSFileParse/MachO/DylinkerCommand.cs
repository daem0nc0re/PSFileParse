using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // union lc_str {
    //     uint32_t offset;
    // #ifndef __LP64__
    //     char     *ptr;
    // #endif 
    // };
    // 
    // struct dylinker_command {
    //     uint32_t     cmd;
    //     uint32_t     cmdsize;
    //     union lc_str name;
    // };
    // 
    public sealed class DylinkerCommand
    {
        public String Name { get; }


        internal DylinkerCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 name_offset;

            if (is_bigendian)
                name_offset = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                name_offset = BinaryHelper.ToUInt32(filebytes, offset);

            name_offset = offset + name_offset - 8;
            Name = BinaryHelper.GetUTF8String(filebytes, name_offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}}}", Name);
        }
    }
}
