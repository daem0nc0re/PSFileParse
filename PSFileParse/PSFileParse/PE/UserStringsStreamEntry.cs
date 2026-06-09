using System;
using System.Text;

namespace PSFileParse.PE
{
    public sealed class UserStringsStreamEntry
    {
        public UInt32 Index { get; }
        public bool IsPrintable { get; }
        public bool HasUnorthodoxBytes { get; }
        public String String { get; }
        public byte[] UnicodeBytes { get; }


        internal UserStringsStreamEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            UInt32 bytes_len;
            Index = index;
            IsPrintable = true;

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
                if (bytes_len > 1)
                {
                    UnicodeBytes = new byte[bytes_len - 1];

                    for (UInt32 i = 0; i < (UInt32)UnicodeBytes.Length; i++)
                        UnicodeBytes[i] = filebytes[offset++];

                    String = Encoding.Unicode.GetString(UnicodeBytes);

                    foreach (Char c in String)
                    {
                        if (Char.IsControl(c))
                        {
                            IsPrintable = false;
                            String = null;
                            break;
                        }
                    }
                }
                else
                {
                    IsPrintable = false;
                }

                HasUnorthodoxBytes = (filebytes[offset++] != 0);
            }
        }


        public override String ToString()
        {
            var str = this.String ?? String.Empty;

            if (!this.IsPrintable && (this.UnicodeBytes != null))
            {
                var string_builder = new StringBuilder();

                for (var i = 0; i < this.UnicodeBytes.Length; i += 2)
                {
                    var c = BitConverter.ToUInt16(this.UnicodeBytes, i);

                    if (Char.IsControl((Char)c))
                    {
                        if (c == 0)
                            string_builder.Append(@"\0");
                        else if (c == 7)
                            string_builder.Append(@"\a");
                        else if (c == 8)
                            string_builder.Append(@"\b");
                        else if (c == 9)
                            string_builder.Append(@"\t");
                        else if (c == 10)
                            string_builder.Append(@"\n");
                        else if (c == 11)
                            string_builder.Append(@"\v");
                        else if (c == 12)
                            string_builder.Append(@"\f");
                        else if (c == 13)
                            string_builder.Append(@"\r");
                        else if (c == 27)
                            string_builder.Append(@"\e");
                        else
                            string_builder.AppendFormat(@"\u{0}", c.ToString("X4"));
                    }
                    else
                    {
                        string_builder.Append((Char)c);
                    }
                }

                str = string_builder.ToString();
            }

            return str;
        }
    }
}
