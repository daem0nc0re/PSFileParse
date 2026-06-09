using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct prebind_cksum_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t cksum;
    // };
    // 
    public sealed class PrebindCksumCommand
    {
        public UInt32 Checksum { get; }


        internal PrebindCksumCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
                Checksum = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                Checksum = BinaryHelper.ToUInt32(filebytes, offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Checksum={0}}}", Checksum);
        }
    }
}