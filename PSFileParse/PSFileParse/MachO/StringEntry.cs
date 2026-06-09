using System;
using System.Collections.Generic;
using System.Text;

namespace PSFileParse.MachO
{
    public sealed class StringEntry
    {
        public UInt32 Index { get; }
        public String String { get; }
        public byte[] UTF8Bytes { get; }


        internal StringEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            var utf8bytes = new List<byte>();
            Index = index;

            while (offset < filebytes.Length)
            {
                var b = filebytes[offset++];

                if (b == 0)
                    break;

                utf8bytes.Add(b);
            }

            UTF8Bytes = utf8bytes.ToArray();
            String = Encoding.UTF8.GetString(UTF8Bytes);
        }


        public override String ToString()
        {
            return String.Format("@{{String={0}; UTF8Bytes={1}}}",
                String,
                UTF8Bytes);
        }
    }
}
