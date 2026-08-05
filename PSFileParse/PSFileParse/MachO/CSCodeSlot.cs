using System;
using System.Text;

namespace PSFileParse.MachO
{
    public sealed class CSCodeSlot
    {
        public UInt32 Index { get; }
        public CSHashType HashType { get; }
        public byte[] HashBytes { get; }
        public String HashString { get; }


        internal CSCodeSlot(
            byte[] filebytes,
            UInt32 offset,
            UInt32 index,
            CSHashType hashType,
            byte hashSize)
        {
            var hexStringBuilder = new StringBuilder();
            Index = index;
            HashType = hashType;
            HashBytes = new byte[hashSize];
            Array.Copy(filebytes, offset, HashBytes, 0, HashBytes.Length);

            foreach (byte b in HashBytes)
                hexStringBuilder.AppendFormat("{0}", b.ToString("X2"));

            HashString = hexStringBuilder.ToString();
        }


        public override String ToString()
        {
            return String.Format("@{{HashType={0}; HashBytes={1}; HashString={2}}}",
                HashType,
                HashBytes,
                HashString);
        }
    }
}
