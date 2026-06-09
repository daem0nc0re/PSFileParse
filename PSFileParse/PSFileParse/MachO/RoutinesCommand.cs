using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct routines_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t init_address;
    //     uint32_t init_module;
    //     uint32_t reserved1;
    //     uint32_t reserved2;
    //     uint32_t reserved3;
    //     uint32_t reserved4;
    //     uint32_t reserved5;
    //     uint32_t reserved6;
    // };
    // 
    // struct routines_command_64 {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint64_t init_address;
    //     uint64_t init_module;
    //     uint64_t reserved1;
    //     uint64_t reserved2;
    //     uint64_t reserved3;
    //     uint64_t reserved4;
    //     uint64_t reserved5;
    //     uint64_t reserved6;
    // };
    // 
    public sealed class RoutinesCommand
    {
        public UInt64 InitAddress { get; }
        public UInt64 InitModule { get; }
        public UInt64[] Reserved { get; }

        internal RoutinesCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian,
            bool is64bit)
        {
            Reserved = new UInt64[6];

            if (is64bit)
            {
                if (is_bigendian)
                {
                    InitAddress = BinaryHelper.ToUInt64Big(filebytes, offset);
                    InitModule = BinaryHelper.ToUInt64Big(filebytes, offset + 8);

                    for (UInt32 i = 0u; i < 6u; i++)
                        Reserved[i] = BinaryHelper.ToUInt64Big(filebytes, offset + 16 + (i * 8));
                }
                else
                {
                    InitAddress = BinaryHelper.ToUInt64(filebytes, offset);
                    InitModule = BinaryHelper.ToUInt64(filebytes, offset + 8);

                    for (UInt32 i = 0u; i < 6u; i++)
                        Reserved[i] = BinaryHelper.ToUInt64(filebytes, offset + 16 + (i * 8));
                }
            }
            else
            {
                if (is_bigendian)
                {
                    InitAddress = BinaryHelper.ToUInt32Big(filebytes, offset);
                    InitModule = BinaryHelper.ToUInt32Big(filebytes, offset + 4);

                    for (UInt32 i = 0u; i < 6u; i++)
                        Reserved[i] = BinaryHelper.ToUInt32Big(filebytes, offset + 8 + (i * 4));
                }
                else
                {
                    InitAddress = BinaryHelper.ToUInt32(filebytes, offset);
                    InitModule = BinaryHelper.ToUInt32(filebytes, offset + 4);

                    for (UInt32 i = 0u; i < 6u; i++)
                        Reserved[i] = BinaryHelper.ToUInt32(filebytes, offset + 8 + (i * 4));
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{InitAddress={0}; InitModule={1}; Reserved={2}}}",
                InitAddress,
                InitModule,
                Reserved);
        }
    }
}