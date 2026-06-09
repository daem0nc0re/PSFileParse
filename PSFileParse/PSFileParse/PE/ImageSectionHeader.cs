using PSFileParse.Auxiliary;
using System;
using System.Text;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_SECTION_HEADER
    // {
    //     unsigned char  Name[8];
    //     union {
    //         int PhysicalAddress; // For COFF format
    //         int VirtualSize; // For PE format
    //     } Misc;
    //     int   VirtualAddress;
    //     int   SizeOfRawData;
    //     int   PointerToRawData;
    //     int   PointerToRelocations;
    //     int   PointerToLinenumbers;
    //     short NumberOfRelocations;
    //     short NumberOfLinenumbers;
    //     int   Characteristics;
    // }
    //
    public sealed class ImageSectionHeader
    {
        public UInt32 Index { get; }
        public String Name { get; }
        public UInt32 VirtualSize { get; }
        public UInt32 VirtualAddress { get; }
        public UInt32 SizeOfRawData { get; }
        public UInt32 PointerToRawData { get; }
        public UInt32 PointerToRelocations { get; }
        public UInt32 PointerToLineNumbers { get; }
        public UInt16 NumberOfRelocations { get; }
        public UInt16 NumberOfLineNumbers { get; }
        public SectionCharacteristics Characteristics { get; }
        internal static readonly UInt32 SizeOfStruct = 0x28;


        internal ImageSectionHeader(byte[] filebytes, UInt32 index, UInt32 offset)
        {
            var name_strs = new byte[]
            {
                filebytes[offset],
                filebytes[offset + 1],
                filebytes[offset + 2],
                filebytes[offset + 3],
                filebytes[offset + 4],
                filebytes[offset + 5],
                filebytes[offset + 6],
                filebytes[offset + 7],
            };
            Index = index;
            Name = Encoding.UTF8.GetString(name_strs).TrimEnd('\0');
            VirtualSize = BinaryHelper.ToUInt32(filebytes, offset + 8);
            VirtualAddress = BinaryHelper.ToUInt32(filebytes, offset + 12);
            SizeOfRawData = BinaryHelper.ToUInt32(filebytes, offset + 16);
            PointerToRawData = BinaryHelper.ToUInt32(filebytes, offset + 20);
            PointerToRelocations = BinaryHelper.ToUInt32(filebytes, offset + 24);
            PointerToLineNumbers = BinaryHelper.ToUInt32(filebytes, offset + 28);
            NumberOfRelocations = BinaryHelper.ToUInt16(filebytes, offset + 32);
            NumberOfLineNumbers = BinaryHelper.ToUInt16(filebytes, offset + 34);
            Characteristics = (SectionCharacteristics)BinaryHelper.ToUInt32(filebytes, offset + 36);
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; VirtualSize={1}; VirtualAddress={2}; SizeOfRawData={3}; PointerToRawData={4}; PointerToRelocations={5}; PointerToLineNumbers={6}; NumberOfRelocations={7}; NumberOfLineNumbers={8}; Characteristics={9}}}",
                Name,
                VirtualSize,
                VirtualAddress,
                SizeOfRawData,
                PointerToRawData,
                PointerToRelocations,
                PointerToLineNumbers,
                NumberOfRelocations,
                NumberOfLineNumbers,
                Characteristics);
        }
    }
}
