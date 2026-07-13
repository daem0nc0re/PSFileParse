using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // typedef struct __BlobIndex {
    //     uint32_t type;
    //     uint32_t offset;
    // } CS_BlobIndex
    // __attribute__ ((aligned(1)));
    // 
    public sealed class CSBlobIndex
    {
        public UInt32 Index { get; }
        public CSSlotType Type { get; }
        public UInt32 Offset { get; }


        public CSBlobIndex(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Type = (CSSlotType)BinaryHelper.ToUInt32Big(filebytes, offset);
            Offset = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            offset += 8u;
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Offset={1}}}",
                Type,
                Offset);
        }
    }
}
