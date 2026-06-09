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
    // struct fvmlib {
    //     union lc_str name;
    //     uint32_t     minor_version;
    //     uint32_t     header_addr;
    // };
    //
    // struct fvmlib_command {
    //     uint32_t      cmd;
    //     uint32_t      cmdsize;
    //     struct fvmlib fvmlib;
    // };
    // 
    public sealed class FvmLibCommand
    {
        public String Name { get; }
        public String MinorVersion { get; }
        public UInt32 HeaderAddress { get; }


        internal FvmLibCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 name_offset;
            UInt32 version;

            if (is_bigendian)
            {
                name_offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                version = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                HeaderAddress = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            }
            else
            {
                name_offset = BinaryHelper.ToUInt32(filebytes, offset);
                version = BinaryHelper.ToUInt32(filebytes, offset + 4);
                HeaderAddress = BinaryHelper.ToUInt32(filebytes, offset + 8);
            }

            name_offset = offset + name_offset - 8;
            Name = BinaryHelper.GetUTF8String(filebytes, name_offset);
            MinorVersion = String.Format("{0}.{1}.{2}",
                (version >> 16) & 0xFFFF,
                (version >> 8) & 0xFF,
                version & 0xFF);
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; MinorVersion={1}; HeaderAddress={2}}}",
                Name,
                MinorVersion,
                HeaderAddress);
        }
    }
}