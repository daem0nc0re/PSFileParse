using System;

namespace PSFileParse.PE
{
    public sealed class GuidStreamEntry
    {
        public UInt32 Index { get; }
        public Guid Identifier { get; }


        public GuidStreamEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            var guid_bytes = new byte[16];
            Index = index;

            for (UInt32 i = 0; i < 16u; i++)
                guid_bytes[i] = filebytes[offset++];

            Identifier = new Guid(guid_bytes);
        }


        public override String ToString()
        {
            return String.Format("@{{Identifier={0}}}", Identifier.ToString());
        }
    }
}
