using System;
using System.Collections.Generic;
using System.Text;

namespace PSFileParse.PE
{
    public sealed class StringsStreamEntry
    {
        public UInt32 Index { get; }
        public String String { get; }
        public byte[] UTF8Bytes { get; }


        internal StringsStreamEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            var bytes = new List<byte>();
            Index = index;

            while (true)
            {
                var b = filebytes[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            if (bytes.Count > 0)
            {
                UTF8Bytes = bytes.ToArray();
                String = Encoding.UTF8.GetString(UTF8Bytes);
            }
        }


        public override String ToString()
        {
            return String;
        }
    }
}
