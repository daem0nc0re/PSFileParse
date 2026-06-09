using System;

namespace PSFileParse.Auxiliary
{
    public sealed class FileSignature
    {
        public byte[] Bytes { get; }


        internal FileSignature(byte[] data, UInt32 offset, UInt32 length)
        {
            Bytes = new byte[length];

            for (UInt32 i = 0u; i < length; i++)
                Bytes[i] = data[offset + i];
        }


        public override String ToString()
        {
            var str = new String[this.Bytes.Length];

            for (Int32 i = 0; i < this.Bytes.Length; i++)
            {
                if (this.Bytes[i] == 0)
                    str[i] = @"\0";
                else if (this.Bytes[i] == 7)
                    str[i] = @"\a";
                else if (this.Bytes[i] == 8)
                    str[i] = @"\b";
                else if (this.Bytes[i] == 9)
                    str[i] = @"\t";
                else if (this.Bytes[i] == 10)
                    str[i] = @"\n";
                else if (this.Bytes[i] == 11)
                    str[i] = @"\v";
                else if (this.Bytes[i] == 12)
                    str[i] = @"\f";
                else if (this.Bytes[i] == 13)
                    str[i] = @"\r";
                else if (this.Bytes[i] == 27)
                    str[i] = @"\e";
                else if (Char.IsControl((Char)this.Bytes[i]))
                    str[i] = String.Format(@"\x{0}", this.Bytes[i].ToString("X2"));
                else
                    str[i] = ((Char)this.Bytes[i]).ToString();
            }

            return String.Join(String.Empty, str);
        }
    }
}
