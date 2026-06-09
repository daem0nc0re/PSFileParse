using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct note_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     char     data_owner[16];
    //     uint64_t offset;
    //     uint64_t size;
    // };
    // 
    public sealed class NoteCommand
    {
        public String Owner { get; }
        public UInt64 Offset { get; }
        public UInt64 Size { get; }
        public byte[] Data { get; }


        internal NoteCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            var data = new byte[16];
            Array.Copy(filebytes, offset, data, 0, 16);
            Owner = BinaryHelper.GetUTF8String(data, 0);

            if (is_bigendian)
            {
                Offset = BinaryHelper.ToUInt64Big(filebytes, offset);
                Size = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
            }
            else
            {
                Offset = BinaryHelper.ToUInt64(filebytes, offset);
                Size = BinaryHelper.ToUInt64(filebytes, offset + 8);
            }

            Data = new byte[Size];

            for (UInt64 i = 0; i < Size; i++)
                Data[i] = filebytes[Offset + i];
        }


        public override String ToString()
        {
            return String.Format("@{{Owner={0}; Offset={1}; Size={2}; Data={3}}}",
                Owner,
                Offset,
                Size,
                Data);
        }
    }
}
