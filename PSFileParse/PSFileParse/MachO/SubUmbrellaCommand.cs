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
    // struct sub_umbrella_command {
    //     uint32_t     cmd;
    //     uint32_t     cmdsize;
    //     union lc_str sub_umbrella;
    // };
    // 
    public sealed class SubUmbrellaCommand
    {
        public String Umbrella { get; }


        internal SubUmbrellaCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 str_offset;

            if (is_bigendian)
                str_offset = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                str_offset = BinaryHelper.ToUInt32(filebytes, offset);

            str_offset = offset + str_offset - 8;
            Umbrella = BinaryHelper.GetUTF8String(filebytes, str_offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Umbrella={0}}}", Umbrella);
        }
    }
}
