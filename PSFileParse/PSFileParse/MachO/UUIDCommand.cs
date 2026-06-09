using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct uuid_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint8_t  uuid[16];
    // };
    // 
    public sealed class UUIDCommand
    {
        public Guid UUID { get; }

        internal UUIDCommand(byte[] filebytes, UInt32 offset, bool is_bigendian)
        {
            Int32 a;
            Int16 b;
            Int16 c;
            var d = new byte[8];

            if (is_bigendian)
            {
                a = (Int32)BinaryHelper.ToUInt32(filebytes, offset);
                b = (Int16)BinaryHelper.ToUInt16(filebytes, offset + 4);
                c = (Int16)BinaryHelper.ToUInt16(filebytes, offset + 6);
            }
            else
            {
                a = (Int32)BinaryHelper.ToUInt32Big(filebytes, offset);
                b = (Int16)BinaryHelper.ToUInt16Big(filebytes, offset + 4);
                c = (Int16)BinaryHelper.ToUInt16Big(filebytes, offset + 6);
            }

            Array.Copy(filebytes, offset + 8, d, 0, 8);
            UUID = new Guid(a, b, c, d);
        }


        public override String ToString()
        {
            return String.Format("@{{UUID={0}}}", UUID);
        }
    }
}
