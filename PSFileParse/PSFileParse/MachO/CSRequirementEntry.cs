using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class CSRequirementEntry
    {
        public UInt32 Index { get; }
        public CSSlotType Type { get; }
        public UInt32 Offset { get; }
        public CSMagic Magic { get; }
        public UInt32 Length { get; }
        public byte[] Data { get; }


        public CSRequirementEntry(
            byte[] filebytes,
            UInt32 offset,
            UInt32 index,
            UInt32 superblob_base)
        {
            Index = index;
            Type = (CSSlotType)BinaryHelper.ToUInt32Big(filebytes, offset);
            Offset = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            Magic = (CSMagic)BinaryHelper.ToUInt32Big(filebytes, superblob_base + Offset);
            Length = BinaryHelper.ToUInt32Big(filebytes, superblob_base + Offset + 4);
            Data = new byte[Length - 8];
            Array.Copy(filebytes, superblob_base + Offset + 8, Data, 0, Data.Length);
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Offset={1}; Magic={2}; Length={3}; Data={4}}}",
                Type,
                Offset,
                Magic,
                Length,
                Data);
        }
    }
}
