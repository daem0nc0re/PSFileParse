using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct build_tool_version {
    //     uint32_t tool;
    //     uint32_t version;
    // };
    // 
    public sealed class BuildToolVersion
    {
        public UInt32 Index { get; }
        public MachOTools Tool { get; }
        public String Version { get; }


        internal BuildToolVersion(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            bool is_bigendian)
        {
            UInt32 version;
            Index = index;

            if (is_bigendian)
            {
                Tool = (MachOTools)BinaryHelper.ToUInt32Big(filebytes, offset);
                version = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                Tool = (MachOTools)BinaryHelper.ToUInt32(filebytes, offset);
                version = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            Version = String.Format("{0}.{1}.{2}",
                (version >> 16) & 0xFFFF,
                (version >> 8) & 0xFF,
                version & 0xFF);
            offset += 8u;
        }


        public override String ToString()
        {
            return String.Format("@{{Tool={0}; Version={1}}}",
                Tool,
                Version);
        }
    }
}
