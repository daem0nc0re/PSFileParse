using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    public sealed class ReproHash
    {
        public UInt32 Size { get; }
        public CryptoHash Hash { get; }


        internal ReproHash(byte[] filebytes, UInt32 offset)
        {
            var size = BinaryHelper.ToUInt32(filebytes, offset);
            var bytes = new byte[size];

            for (UInt32 i = 0; i < size; i++)
                bytes[i] = filebytes[offset + 4 + i];

            Size = size;
            Hash = new CryptoHash(bytes);
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; Hash={1}}}", Size, Hash);
        }
    }
}
