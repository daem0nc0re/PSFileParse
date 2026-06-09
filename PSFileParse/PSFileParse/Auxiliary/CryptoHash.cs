using System;
using System.Security.Cryptography;
using System.Text;

namespace PSFileParse.Auxiliary
{
    public class CryptoHash
    {
        public enum CryptoHashType
        {
            MD5 = 0,
            SHA1 = 1,
            SHA256 = 2,
            SHA384 = 3,
            SHA512 = 4
        }

        public CryptoHashType Type { get; }
        public byte[] Hash { get; }


        internal CryptoHash(byte[] data)
        {
            if (data.Length == 16)
                Type = CryptoHashType.MD5;
            else if (data.Length == 20)
                Type = CryptoHashType.SHA1;
            else if (data.Length == 32)
                Type = CryptoHashType.SHA256;
            else if (data.Length == 48)
                Type = CryptoHashType.SHA384;
            else if (data.Length == 64)
                Type = CryptoHashType.SHA512;
            else
                throw new ArgumentException("Byte array length is invalid.");

            Hash = data;
        }


        internal CryptoHash(byte[] data, CryptoHashType type)
        {
            Type = type;

            if (data is null)
                data = new byte[0];

            if (type == CryptoHashType.MD5)
            {
                var md5provider = MD5.Create();
                Hash = md5provider.ComputeHash(data);
                md5provider.Dispose();
            }
            else if (type == CryptoHashType.SHA1)
            {
                var sha1provider = SHA1.Create();
                Hash = sha1provider.ComputeHash(data);
                sha1provider.Dispose();
            }
            else if (type == CryptoHashType.SHA256)
            {
                var sha256provider = SHA256.Create();
                Hash = sha256provider.ComputeHash(data);
                sha256provider.Dispose();
            }
            else if (type == CryptoHashType.SHA384)
            {
                var sha384provider = SHA384.Create();
                Hash = sha384provider.ComputeHash(data);
                sha384provider.Dispose();
            }
            else if (type == CryptoHashType.SHA512)
            {
                var sha512provider = SHA512.Create();
                Hash = sha512provider.ComputeHash(data);
                sha512provider.Dispose();
            }
            else
            {
                throw new ArgumentException("Invalid hash type specifier.");
            }
        }


        internal CryptoHash(byte[] data, Int32 offset, Int32 length, CryptoHashType type)
        {
            Type = type;

            if (type == CryptoHashType.MD5)
            {
                var md5provider = MD5.Create();
                Hash = md5provider.ComputeHash(data, offset, length);
                md5provider.Dispose();
            }
            else if (type == CryptoHashType.SHA1)
            {
                var sha1provider = SHA1.Create();
                Hash = sha1provider.ComputeHash(data, offset, length);
                sha1provider.Dispose();
            }
            else if (type == CryptoHashType.SHA256)
            {
                var sha256provider = SHA256.Create();
                Hash = sha256provider.ComputeHash(data, offset, length);
                sha256provider.Dispose();
            }
            else if (type == CryptoHashType.SHA384)
            {
                var sha384provider = SHA256.Create();
                Hash = sha384provider.ComputeHash(data, offset, length);
                sha384provider.Dispose();
            }
            else if (type == CryptoHashType.SHA512)
            {
                var sha512provider = SHA512.Create();
                Hash = sha512provider.ComputeHash(data, offset, length);
                sha512provider.Dispose();
            }
            else
            {
                throw new ArgumentException("Invalid hash type specifier.");
            }
        }


        public override String ToString()
        {
            var builder = new StringBuilder();

            foreach (var b in this.Hash)
                builder.AppendFormat("{0}", b.ToString("X2"));

            return builder.ToString();
        }
    }
}
