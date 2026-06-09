using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct version_min_command {
    //     uint32_t    cmd;
    //     uint32_t    cmdsize;
    //     uint32_t    version;
    //     uint32_t    sdk;
    // };
    // 
    public sealed class VersionMinCommand
    {
        public String Version { get; }
        public String SDK { get; }


        internal VersionMinCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 version;
            UInt32 sdk;

            if (is_bigendian)
            {
                version = BinaryHelper.ToUInt32Big(filebytes, offset);
                sdk = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                version = BinaryHelper.ToUInt32(filebytes, offset);
                sdk = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            Version = String.Format("{0}.{1}.{2}",
                (version >> 16) & 0xFFFF,
                (version >> 8) & 0xFF,
                version & 0xFF);
            SDK = String.Format("{0}.{1}.{2}",
                (sdk >> 16) & 0xFFFF,
                (sdk >> 8) & 0xFF,
                sdk & 0xFF);
        }


        public override String ToString()
        {
            return String.Format("@{{Version={0}; SDK={1}}}", Version, SDK);
        }
    }
}
