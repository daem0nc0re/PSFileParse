using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct build_version_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t platform;
    //     uint32_t minos;
    //     uint32_t sdk;
    //     uint32_t ntools;
    // };
    // 
    public sealed class BuildVersionCommand
    {
        public MachOPlatform Platform { get; }
        public String MinimumOS { get; }
        public String SDK { get; }
        public UInt32 NumberOfTools { get; }
        public BuildToolVersion[] ToolVersions { get; }


        internal BuildVersionCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 minos;
            UInt32 sdk;

            if (is_bigendian)
            {
                Platform = (MachOPlatform)BinaryHelper.ToUInt32Big(filebytes, offset);
                minos = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                sdk = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                NumberOfTools = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            }
            else
            {
                Platform = (MachOPlatform)BinaryHelper.ToUInt32(filebytes, offset);
                minos = BinaryHelper.ToUInt32(filebytes, offset + 4);
                sdk = BinaryHelper.ToUInt32(filebytes, offset + 8);
                NumberOfTools = BinaryHelper.ToUInt32(filebytes, offset + 12);
            }

            MinimumOS = String.Format("{0}.{1}.{2}",
                (minos >> 16) & 0xFFFF,
                (minos >> 8) & 0xFF,
                minos & 0xFF);
            SDK = String.Format("{0}.{1}.{2}",
                (sdk >> 16) & 0xFFFF,
                (sdk >> 8) & 0xFF,
                sdk & 0xFF);

            if (NumberOfTools > 0)
            {
                ToolVersions = new BuildToolVersion[NumberOfTools];
                offset += 16u;

                for (UInt32 i = 0; i < NumberOfTools; i++)
                    ToolVersions[i] = new BuildToolVersion(filebytes, ref offset, i, is_bigendian);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Platform={0}; MinimumOS={1}; SDK={2}; NumberOfTools={3}; ToolVersions={4}}}",
                Platform,
                MinimumOS,
                SDK,
                NumberOfTools,
                ToolVersions);
        }
    }
}
