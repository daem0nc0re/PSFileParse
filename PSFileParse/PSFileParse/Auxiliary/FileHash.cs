using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFileParse.Auxiliary
{
    public class FileHash
    {
        public CryptoHash MD5 { get; }
        public CryptoHash SHA1 { get; }
        public CryptoHash SHA256 { get; }


        internal FileHash(byte[] filebytes)
        {
            MD5 = new CryptoHash(filebytes, CryptoHash.CryptoHashType.MD5);
            SHA1 = new CryptoHash(filebytes, CryptoHash.CryptoHashType.SHA1);
            SHA256 = new CryptoHash(filebytes, CryptoHash.CryptoHashType.SHA256);
        }


        public override String ToString()
        {
            return String.Format("@{{MD5={0}; SHA1={1}; SHA256={2}}}",
                this.MD5,
                this.SHA1,
                this.SHA256);
        }
    }
}
