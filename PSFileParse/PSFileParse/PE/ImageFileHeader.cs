using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_FILE_HEADER
    // {
    //     short Machine;
    //     short NumberOfSections;
    //     int   TimeDateStamp;
    //     int   PointerToSymbolTable;
    //     int   NumberOfSymbols;
    //     short SizeOfOptionalHeader;
    //     short Characteristics;
    // };
    //  
    public sealed class ImageFileHeader
    {
        public ImageFileMachine Machine { get; }
        public UInt16 NumberOfSections { get; }
        public UnixTime TimeDateStamp { get; }
        public UInt32 PointerToSymbolTable { get; }
        public UInt32 NumberOfSymbols { get; }
        public UInt16 SizeOfOptionalHeader { get; }
        public ImageFileCharacteristics Characteristics { get; }
        internal static readonly UInt32 SizeOfStruct = 0x14;


        internal ImageFileHeader(byte[] filebytes, UInt32 offset)
        {
            Machine = (ImageFileMachine)BinaryHelper.ToUInt16(filebytes, offset);
            NumberOfSections = BinaryHelper.ToUInt16(filebytes, offset + 2);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, offset + 4));
            PointerToSymbolTable = BinaryHelper.ToUInt32(filebytes, offset + 8);
            NumberOfSymbols = BinaryHelper.ToUInt32(filebytes, offset + 12);
            SizeOfOptionalHeader = BinaryHelper.ToUInt16(filebytes, offset + 16);
            Characteristics = (ImageFileCharacteristics)BinaryHelper.ToUInt16(filebytes, offset + 18);

            if (filebytes.Length < offset + 0x18 + SizeOfOptionalHeader + (NumberOfSections * 0x28))
                throw new ArgumentException("Input file is too small.");
        }


        public override String ToString()
        {
            return String.Format("@{{Machine={0}; NumberOfSections={1}; TimeDateStamp={2}; PointerToSymbolTable={3}; NumberOfSymbols={4}; SizeOfOptionalHeader={5}; Characteristics={6}}}",
                Machine,
                NumberOfSections,
                TimeDateStamp,
                PointerToSymbolTable,
                NumberOfSymbols,
                SizeOfOptionalHeader,
                Characteristics);
        }
    }
}
