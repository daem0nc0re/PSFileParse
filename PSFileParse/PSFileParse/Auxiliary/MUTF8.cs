using System;

namespace PSFileParse.Auxiliary
{
    internal class MUTF8
    {
        public static byte[] ConvertToUnicodeBytes(
            byte[] mutf8bytes,
            UInt32 offset,
            UInt32 length,
            out bool is_printable)
        {
            var cursor = offset;
            var capacity = (UInt32)mutf8bytes.Length;
            var unicodes = new byte[length * 2];
            is_printable = true;

            if ((length == 0) || (mutf8bytes.Length == 0))
            {
                is_printable = false;
                return new byte[0];
            }

            for (UInt32 i = 0; (i < length * 2) && (cursor < capacity); i += 2)
            {
                UInt16 code1;
                UInt16 code2;
                UInt16 code0 = (UInt16)mutf8bytes[cursor++];

                if ((code0 & 0xE0) == 0xC0)
                {
                    if (cursor + 1 > capacity)
                        break;

                    code1 = (UInt16)mutf8bytes[cursor++];

                    if ((code1 & 0x80) != 0x80)
                        break;

                    code0 = (UInt16)(((code0 & 0x1F) << 6) | (code1 & 0x3F));
                }
                else if ((code0 & 0xF0) == 0xE0)
                {
                    if (cursor + 2 > capacity)
                        break;
                    
                    code1 = (UInt16)mutf8bytes[cursor++];
                    code2 = (UInt16)mutf8bytes[cursor++];

                    if (((code1 & 0x80) != 0x80) || ((code2 & 0x80) != 0x80))
                        break;

                    code0 = (UInt16)(((code0 & 0x0F) << 12) | ((code1 & 0x3F) << 6) | (code2 & 0x3F));
                }
                else if (code0 > 0x7F)
                {
                    break;
                }

                if (is_printable && Char.IsControl((Char)code0))
                    is_printable = false;

                unicodes[i] = (byte)(code0 & 0xFF);
                unicodes[i + 1] = (byte)((code0 >> 8) & 0xFF);
            }

            return unicodes;
        }
    }
}
