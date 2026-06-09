using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_DATA_DIRECTORY
    // {
    //     int VirtualAddress;
    //     int Size;
    // };
    //
    public sealed class ImageDataDirectory
    {
        public Int32 VirtualAddress { get; }
        public Int32 Size { get; }
        internal static readonly UInt32 SizeOfStruct = 0x8;


        internal ImageDataDirectory(byte[] filebytes, UInt32 offset)
        {
            VirtualAddress = BinaryHelper.ToInt32(filebytes, offset);
            Size = BinaryHelper.ToInt32(filebytes, offset + 4);
        }


        public override String ToString()
        {
            return String.Format("@{{VirtualAddress={0}; Size={1}}}",
                VirtualAddress,
                Size);
        }
    }
}
