using System;
using System.Text;

namespace PSFileParse.MachO
{
    public sealed class CSSpecialSlot
    {
        public UInt32 Index { get; }
        public CSSlotType SlotType { get; }
        public CSHashType HashType { get; }
        public byte[] HashBytes { get; }
        public String HashString { get; }


        internal CSSpecialSlot(
            byte[] filebytes,
            UInt32 offset,
            UInt32 index,
            CSHashType hashType,
            byte hashSize)
        {
            var hexStringBuilder = new StringBuilder();
            Index = index;
            SlotType = (CSSlotType)(index + 1);
            HashType = hashType;
            HashBytes = new byte[hashSize];
            Array.Copy(filebytes, offset, HashBytes, 0, HashBytes.Length);

            foreach (byte b in HashBytes)
                hexStringBuilder.AppendFormat("{0}", b.ToString("X2"));

            HashString = hexStringBuilder.ToString();
        }


        public override String ToString()
        {
            return String.Format("@{{SlotType={0}; HashType={1}; HashBytes={2}; HashString={3}}}",
                SlotType,
                HashType,
                HashBytes,
                HashString);
        }
    }
}
