using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct encryption_info_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t cryptoff;
    //     uint32_t cryptsize;
    //     uint32_t cryptid;
    // };
    // 
    // struct encryption_info_command_64 {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t cryptoff;
    //     uint32_t cryptsize;
    //     uint32_t cryptid;
    //     uint32_t pad;
    // };
    // 
    public sealed class EncryptionInfoCommand
    {
        public UInt32 CryptOffset { get; }
        public UInt32 CryptSize { get; }
        public UInt32 CryptId { get; }
        public UInt32 Padding { get; }


        internal EncryptionInfoCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                CryptOffset = BinaryHelper.ToUInt32Big(filebytes, offset);
                CryptSize = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                CryptId = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            }
            else
            {
                CryptOffset = BinaryHelper.ToUInt32(filebytes, offset);
                CryptSize = BinaryHelper.ToUInt32(filebytes, offset + 4);
                CryptId = BinaryHelper.ToUInt32(filebytes, offset + 8);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{CryptOffset={0}; CryptSize={1}; CryptId={2}}}",
                CryptOffset,
                CryptSize,
                CryptId);
        }
    }
}