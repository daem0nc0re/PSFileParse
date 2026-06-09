using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    public sealed class RichHeaderEntry
    {
        public UInt16 Version { get; }
        public UInt16 ID { get; }
        public UInt32 Count { get; }
        internal static readonly UInt32 SizeOfStruct = 0x8;


        internal RichHeaderEntry(byte[] filebytes, UInt32 offset, UInt32 key)
        {
            Version = (UInt16)(BinaryHelper.ToUInt16(filebytes, offset) ^ (key & 0xFFFF));
            ID = (UInt16)(BinaryHelper.ToUInt16(filebytes, offset + 2) ^ ((key >> 16) & 0xFFFF));
            Count = (UInt32)(BinaryHelper.ToUInt32(filebytes, offset + 4) ^ key);
        }


        public override String ToString()
        {
            return String.Format("{0}.{1}.{2}", Version, ID, Count);
        }
    }
}
