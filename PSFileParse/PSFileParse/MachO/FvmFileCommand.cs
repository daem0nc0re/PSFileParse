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
    // struct fvmfile_command {
    //     uint32_t     cmd;
    //     uint32_t     cmdsize;
    //     union lc_str name;
    //     uint32_t     header_addr;
    // };
    // 
    public sealed class FvmFileCommand
    {
        public String Name { get; }
        public UInt32 HeaderAddress { get; }


        internal FvmFileCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 str_offset;

            if (is_bigendian)
            {
                str_offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                HeaderAddress = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                str_offset = BinaryHelper.ToUInt32(filebytes, offset);
                HeaderAddress = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            str_offset = offset + str_offset - 8;
            Name = BinaryHelper.GetUTF8String(filebytes, str_offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; HeaderAddress={1}}}",
                Name,
                HeaderAddress);
        }
    }
}