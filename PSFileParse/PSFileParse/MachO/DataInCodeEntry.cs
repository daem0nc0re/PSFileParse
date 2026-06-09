using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct data_in_code_entry {
    //     uint32_t offset;
    //     uint16_t length;
    //     uint16_t kind;
    // };
    // 
    public sealed class DataInCodeEntry
    {
        public UInt32 Index { get; }
        public UInt32 Offset { get; }
        public UInt16 Length { get; }
        public DataInCodeEntryKinds Kind { get; }


        public DataInCodeEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            bool is_bigendian)
        {
            Index = index;

            if (is_bigendian)
            {
                Offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                Length = BinaryHelper.ToUInt16Big(filebytes, offset + 4);
                Kind = (DataInCodeEntryKinds)BinaryHelper.ToUInt16Big(filebytes, offset + 6);
            }
            else
            {
                Offset = BinaryHelper.ToUInt32(filebytes, offset);
                Length = BinaryHelper.ToUInt16(filebytes, offset + 4);
                Kind = (DataInCodeEntryKinds)BinaryHelper.ToUInt16(filebytes, offset + 6);
            }

            offset += 8u;
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; Length={1}; Kind={2}}}",
                Offset,
                Length,
                Kind);
        }
    }
}
