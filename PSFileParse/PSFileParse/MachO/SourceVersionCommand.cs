using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class SourceVersionCommand
    {
        public String Version { get; }


        internal SourceVersionCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt64 value;

            if (is_bigendian)
                value = BinaryHelper.ToUInt64Big(filebytes, offset);
            else
                value = BinaryHelper.ToUInt64(filebytes, offset);

            Version = String.Format("{0}.{1}.{2}.{3}.{4}",
                (value >> 40) & 0x00FFFFFF,
                (value >> 30) & 0x000003FF,
                (value >> 20) & 0x000003FF,
                (value >> 10) & 0x000003FF,
                value & 0x000003FF);
        }


        public override String ToString()
        {
            return String.Format("@{{Version={0}}}", Version);
        }
    }
}
