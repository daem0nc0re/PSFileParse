using System;
using System.Text;

namespace PSFileParse.MachO
{
    public sealed class CSHashBlob
    {
        public CSHashType Type { get; }
        public String HexString { get; }
        public byte[] Bytes { get; }


        internal CSHashBlob(
            byte[] filebytes,
            UInt32 offset,
            CSHashType type)
        {
            var hexStringBuilder = new StringBuilder();
            byte nLength = 0;
            Type = type;

            if (type == CSHashType.SHA1)
                nLength = 20;
            else if (type == CSHashType.SHA256)
                nLength = 32;
            else if (type == CSHashType.SHA256Truncated)
                nLength = 20;
            else if (type == CSHashType.SHA384)
                nLength = 48;

            Bytes = new byte[nLength];
            Array.Copy(filebytes, offset, Bytes, 0, Bytes.Length);

            for (byte i = 0; i < Bytes.Length; i++)
                hexStringBuilder.AppendFormat("{0}", Bytes[i].ToString("X2"));

            if (hexStringBuilder.Length > 0)
                HexString = hexStringBuilder.ToString();
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; HexString={1}; Bytes={2}}}",
                Type,
                HexString,
                Bytes);
        }
    }
}
