using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct linkedit_data_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t dataoff;
    //     uint32_t datasize;
    // };
    // 
    public sealed class LinkEditDataCommand
    {
        public UInt32 DataOffset { get; }
        public UInt32 DataSize { get; }
        public byte[] Data { get; }


        internal LinkEditDataCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                DataOffset = BinaryHelper.ToUInt32Big(filebytes, offset);
                DataSize = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                DataOffset = BinaryHelper.ToUInt32(filebytes, offset);
                DataSize = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            Data = new byte[DataSize];
            Array.Copy(filebytes, DataOffset, Data, 0, DataSize);
        }


        public override String ToString()
        {
            return String.Format("@{{DataOffset={0}; DataSize={1}; Data={2}}}",
                DataOffset,
                DataSize,
                Data);
        }
    }
}
