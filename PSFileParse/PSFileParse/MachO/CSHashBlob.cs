using System;
using System.Security.Cryptography;
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
            byte length = 0;
            Type = type;

            if (type == CSHashType.SHA1)
                length = 20;
            else if (type == CSHashType.SHA256)
                length = 32;
            else if (type == CSHashType.SHA256Truncated)
                length = 20;
            else if (type == CSHashType.SHA384)
                length = 48;

            if (length > 0)
            {
                Bytes = new byte[length];
                Array.Copy(filebytes, offset, Bytes, 0, Bytes.Length);

                for (byte i = 0; i < Bytes.Length; i++)
                    hexStringBuilder.AppendFormat("{0}", Bytes[i].ToString("X2"));

                if (hexStringBuilder.Length > 0)
                    HexString = hexStringBuilder.ToString();
            }
        }


        internal CSHashBlob(
            byte[] filebytes,
            UInt32 offset,
            UInt32 range,
            CSHashType type)
        {
            byte[] hashbytes = null;
            var hexStringBuilder = new StringBuilder();
            var blob = new byte[range];
            byte length = 0;
            Type = type;
            Array.Copy(filebytes, offset, blob, 0, range);

            if (type == CSHashType.SHA1)
            {
                length = 20;

                using (var provider = new SHA1CryptoServiceProvider())
                {
                    hashbytes = provider.ComputeHash(blob);
                }
            }
            else if (type == CSHashType.SHA256)
            {
                length = 32;

                using (var provider = new SHA256CryptoServiceProvider())
                {
                    hashbytes = provider.ComputeHash(blob);
                }
            }
            else if (type == CSHashType.SHA256Truncated)
            {
                length = 20;

                using (var provider = new SHA256CryptoServiceProvider())
                {
                    hashbytes = provider.ComputeHash(blob);
                }
            }
            else if (type == CSHashType.SHA384)
            {
                length = 48;

                using (var provider = new SHA384CryptoServiceProvider())
                {
                    hashbytes = provider.ComputeHash(blob);
                }
            }

            if (hashbytes != null)
            {
                Bytes = new byte[length];
                Array.Copy(hashbytes, 0, Bytes, 0, length);

                for (byte i = 0; i < Bytes.Length; i++)
                    hexStringBuilder.AppendFormat("{0}", Bytes[i].ToString("X2"));

                if (hexStringBuilder.Length > 0)
                    HexString = hexStringBuilder.ToString();
            }
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
