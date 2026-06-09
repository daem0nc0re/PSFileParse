using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_BASE_RELOCATION
    // {
    //     unsigned int VirtualAddress;
    //     unsigned int SizeOfBlock;
    // };
    //
    public sealed class ImageBaseRelocation
    {
        public UInt32 Index { get; }
        public UInt32 VirtualAddress { get; }
        public UInt32 SizeOfBlock { get; }
        public BaseRelocTypeOffsetEntry[] Entries { get; }


        internal ImageBaseRelocation(
            byte[] filebytes,
            UInt32 index,
            UInt32 offset,
            ImageFileMachine machine)
        {
            Index = index;
            VirtualAddress = BinaryHelper.ToUInt32(filebytes, offset);
            SizeOfBlock = BinaryHelper.ToUInt32(filebytes, offset + 4);
            Entries = new BaseRelocTypeOffsetEntry[(SizeOfBlock - 8) / 2];
            offset += 8;

            for (UInt32 i = 0; i < Entries.Length; i++)
            {
                Entries[i] = new BaseRelocTypeOffsetEntry(
                    filebytes,
                    i,
                    offset,
                    machine);
                offset += BaseRelocTypeOffsetEntry.SizeOfStruct;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{VirtualAddress={0}; SizeOfBlock={1}; Entries={2}}}",
                VirtualAddress,
                SizeOfBlock,
                Entries);
        }
    }
}
