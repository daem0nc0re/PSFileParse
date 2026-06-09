using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct twolevel_hint {
    //     uint32_t 
    //     isub_image:8,
    //     itoc:24;
    // };
    // 
    public sealed class TwoLevelHint
    {
        public UInt32 Index { get; }
        public UInt32 SubImageIndex { get; }
        public UInt32 TableOfContentsIndex { get; }


        internal TwoLevelHint(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            bool is_bigendian)
        {
            UInt32 value;
            Index = index;

            if (is_bigendian)
                value = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                value = BinaryHelper.ToUInt32(filebytes, offset);

            SubImageIndex = value & 0xFF;
            TableOfContentsIndex = (value >> 8) & 0x00FFFFFF;
            offset += 4;
        }


        public override String ToString()
        {
            return String.Format("@{{SubImageIndex={0}; TableOfContentsIndex={1}}}",
                SubImageIndex,
                TableOfContentsIndex);
        }
    }
}