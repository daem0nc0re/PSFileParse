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
    // struct dylib {
    //     union lc_str name;
    //     uint32_t     timestamp;
    //     uint32_t     current_version;
    //     uint32_t     compatibility_version;
    // };
    // 
    // struct dylib_command {
    //     uint32_t     cmd;
    //     uint32_t     cmdsize;
    //     struct dylib dylib;
    // };
    // 
    public sealed class DylibCommand
    {
        public String Name { get; }
        public UnixTime Timestamp { get; }
        public String CurrentVersion { get; }
        public String CompatibilityVersion { get; }


        internal DylibCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 name_offset;
            UInt32 ellapsed;
            UInt32 current;
            UInt32 compat;

            if (is_bigendian)
            {
                name_offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                ellapsed = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                current = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                compat = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            }
            else
            {
                name_offset = BinaryHelper.ToUInt32(filebytes, offset);
                ellapsed = BinaryHelper.ToUInt32(filebytes, offset + 4);
                current = BinaryHelper.ToUInt32(filebytes, offset + 8);
                compat = BinaryHelper.ToUInt32(filebytes, offset + 12);
            }

            name_offset = offset + name_offset - 8;
            Name = BinaryHelper.GetUTF8String(filebytes, name_offset);
            Timestamp = new UnixTime(ellapsed);
            CurrentVersion = String.Format("{0}.{1}.{2}",
                (current >> 16) & 0xFFFF,
                (current >> 8) & 0xFF,
                current & 0xFF);
            CompatibilityVersion = String.Format("{0}.{1}.{2}",
                (compat >> 16) & 0xFFFF,
                (compat >> 8) & 0xFF,
                compat & 0xFF);
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; Timestamp={1}; CurrentVersion={2}; CompatibilityVersion={3}}}",
                Name,
                Timestamp,
                CurrentVersion,
                CompatibilityVersion);
        }
    }
}
