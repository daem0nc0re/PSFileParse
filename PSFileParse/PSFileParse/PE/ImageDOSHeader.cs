using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_DOS_HEADER
    // {
    //     short e_magic;    // Magic number
    //     short e_cblp;     // Bytes on last page of file
    //     short e_cp;       // Pages in file
    //     short e_crlc;     // Relocations
    //     short e_cparhdr;  // Size of header in paragraphs
    //     short e_minalloc; // Minimum extra paragraphs needed
    //     short e_maxalloc; // Maximum extra paragraphs needed
    //     short e_ss;       // Initial (relative) SS value
    //     short e_sp;       // Initial SP value
    //     short e_csum;     // Checksum
    //     short e_ip;       // Initial IP value
    //     short e_cs;       // Initial (relative) CS value
    //     short e_lfarlc;   // File address of relocation table
    //     short e_ovno;     // Overlay number
    //     short e_res[4];   // Reserved words
    //     short e_oemid;    // OEM identifier (for e_oeminfo)
    //     short e_oeminfo;  // OEM information; e_oemid specific
    //     short e_res2[10]; // Reserved words
    //     int   e_lfanew;   // File address of new exe header
    // };
    // 
    public sealed class ImageDOSHeader
    {
        public FileSignature Magic { get; }
        public UInt16 BytesOnLastPageOfFile { get; }
        public UInt16 PageInFile { get; }
        public UInt16 Relocations { get; }
        public UInt16 SizeOfHeaderInParagraphs { get; }
        public UInt16 MinimumExtraParagraphsNeeded { get; }
        public UInt16 MaximumExtraParagraphsNeeded { get; }
        public UInt16 InitialStackSegment { get; }
        public UInt16 InitialStackPointer { get; }
        public UInt16 Checksum { get; }
        public UInt16 InitialInstructionPointer { get; }
        public UInt16 InitialCodeSegment { get; }
        public UInt16 FileAddressOfRelocationTable { get; }
        public UInt16 OverlayNumber { get; }
        public UInt16[] Reserved { get; }
        public UInt16 OemIdentifire { get; }
        public UInt16 OemInformation { get; }
        public UInt16[] Reserved2 { get; }
        public UInt32 FileAddressOfNewHeader { get; }
        internal static readonly UInt32 SizeOfStruct = 0x40;


        internal ImageDOSHeader(byte[] filebytes, UInt32 offset)
        {
            if ((filebytes == null) || (filebytes.Length < 64))
                throw new ArgumentException("Input file is null or too small.");

            Magic = new FileSignature(filebytes, offset, 2);

            if (Magic.ToString() != "MZ")
                throw new ArgumentException("Invalid DOS magic value.");

            BytesOnLastPageOfFile = BinaryHelper.ToUInt16(filebytes, offset + 2);
            PageInFile = BinaryHelper.ToUInt16(filebytes, offset + 4);
            Relocations = BinaryHelper.ToUInt16(filebytes, offset + 6);
            SizeOfHeaderInParagraphs = BinaryHelper.ToUInt16(filebytes, offset + 8);
            MinimumExtraParagraphsNeeded = BinaryHelper.ToUInt16(filebytes, offset + 10);
            MaximumExtraParagraphsNeeded = BinaryHelper.ToUInt16(filebytes, offset + 12);
            InitialStackSegment = BinaryHelper.ToUInt16(filebytes, offset + 14);
            InitialStackPointer = BinaryHelper.ToUInt16(filebytes, offset + 16);
            Checksum = BinaryHelper.ToUInt16(filebytes, offset + 18);
            InitialInstructionPointer = BinaryHelper.ToUInt16(filebytes, offset + 20);
            InitialCodeSegment = BinaryHelper.ToUInt16(filebytes, offset + 22);
            FileAddressOfRelocationTable = BinaryHelper.ToUInt16(filebytes, offset + 24);
            OverlayNumber = BinaryHelper.ToUInt16(filebytes, offset + 26);
            Reserved = new UInt16[4];

            for (UInt32 i = 0; i < 4; i++)
                Reserved[i] = BinaryHelper.ToUInt16(filebytes, offset + 28 + (i * 2));

            OemIdentifire = BinaryHelper.ToUInt16(filebytes, offset + 36);
            OemInformation = BinaryHelper.ToUInt16(filebytes, offset + 38);
            Reserved2 = new UInt16[10];

            for (UInt32 i = 0; i < 10; i++)
                Reserved2[i] = BinaryHelper.ToUInt16(filebytes, offset + 40 + (i * 2));

            FileAddressOfNewHeader = BinaryHelper.ToUInt32(filebytes, offset + 60);

            if (filebytes.Length < FileAddressOfNewHeader + 0x18)
                throw new ArgumentException("Input file is too small.");
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; BytesOnLastPageOfFile={1}; PageInFile={2}; Relocations={3}; SizeOfHeaderInParagraphs={4}; MinimumExtraParagraphsNeeded={5}; MaximumExtraParagraphsNeeded={6}; InitialStackSegment={7}; InitialStackPointer={8}; Checksum={9}; InitialInstructionPointer={10}; InitialCodeSegment={11}; FileAddressOfRelocationTable={12}; OverlayNumber={13}; Reserved={14}; OemIdentifire={15}; OemInformation={16}; Reserved2={17}}}",
                Magic,
                BytesOnLastPageOfFile,
                PageInFile,
                Relocations,
                SizeOfHeaderInParagraphs,
                MinimumExtraParagraphsNeeded,
                MaximumExtraParagraphsNeeded,
                InitialStackSegment,
                InitialStackPointer,
                Checksum,
                InitialInstructionPointer,
                InitialCodeSegment,
                FileAddressOfRelocationTable,
                OverlayNumber,
                Reserved,
                OemIdentifire,
                OemInformation,
                Reserved2);
        }
    }
}
