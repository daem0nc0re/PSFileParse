using System;

namespace PSFileParse.PE
{
    internal class BlobStreamEntry
    {
        public UInt32 Index { get; }
        public byte[] Bytes { get; }
        internal UInt32 SizeOfEntry { get; }


        internal BlobStreamEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            Bytes = ReadBlobStream(filebytes, ref offset);
        }


        internal static byte[] ReadBlobStream(byte[] filebytes, ref UInt32 offset)
        {
            UInt32 bytes_len;
            byte[] data = null;

            if ((filebytes[offset] & 0xC0) == 0x80)
            {
                UInt32 b0 = filebytes[offset++] & 0x3Fu;
                UInt32 b1 = filebytes[offset++];
                bytes_len = (b0 << 8) + b1;
            }
            else if ((filebytes[offset] & 0xE0) == 0xC0)
            {
                UInt32 b0 = filebytes[offset++] & 0x1Fu;
                UInt32 b1 = filebytes[offset++];
                UInt32 b2 = filebytes[offset++];
                UInt32 b3 = filebytes[offset++];
                bytes_len = (b0 << 24) + (b1 << 16) + (b2 << 8) + b3;
            }
            else
            {
                bytes_len = filebytes[offset++];
            }

            if (bytes_len > 0)
            {
                data = new byte[bytes_len];

                for (UInt32 i = 0; i < (UInt32)data.Length; i++)
                    data[i] = filebytes[offset++];
            }

            return data;
        }


        public override String ToString()
        {
            return Bytes?.ToString();
        }
    }
}
