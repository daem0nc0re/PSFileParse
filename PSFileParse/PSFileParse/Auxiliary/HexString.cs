using System;

namespace PSFileParse.Auxiliary
{
    public class HexString
    {
        public String String { get; }
        public byte[] Bytes { get; }


        internal HexString(byte[] data, UInt32 offset, UInt32 length)
        {
            var str = new String[length];
            Bytes = new byte[length];

            for (UInt32 i = 0; i < length; i++)
            {
                Bytes[i] = data[i + offset];
                str[i] = Bytes[i].ToString("X2");
            }

            String = String.Join(String.Empty, str);
        }


        internal HexString(byte[] data, UInt64 offset, UInt32 length)
        {
            var str = new String[length];
            Bytes = new byte[length];

            for (UInt32 i = 0; i < length; i++)
            {
                Bytes[i] = data[i + offset];
                str[i] = Bytes[i].ToString("X2");
            }

            String = String.Join(String.Empty, str);
        }


        public override String ToString()
        {
            return this.String;
        }
    }
}
