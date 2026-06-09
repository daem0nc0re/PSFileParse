using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_OPTIONAL_HEADER64
    // {
    //     short                Magic;
    //     unsigned char        MajorLinkerVersion;
    //     unsigned char        MinorLinkerVersion;
    //     int                  SizeOfCode;
    //     int                  SizeOfInitializedData;
    //     int                  SizeOfUninitializedData;
    //     int                  AddressOfEntryPoint;
    //     int                  BaseOfCode;
    //     unsigned long long   ImageBase;
    //     int                  SectionAlignment;
    //     int                  FileAlignment;
    //     short                MajorOperatingSystemVersion;
    //     short                MinorOperatingSystemVersion;
    //     short                MajorImageVersion;
    //     short                MinorImageVersion;
    //     short                MajorSubsystemVersion;
    //     short                MinorSubsystemVersion;
    //     int                  Win32VersionValue;
    //     int                  SizeOfImage;
    //     int                  SizeOfHeaders;
    //     int                  CheckSum;
    //     short                Subsystem;
    //     short                DllCharacteristics;
    //     unsigned long long   SizeOfStackReserve;
    //     unsigned long long   SizeOfStackCommit;
    //     unsigned long long   SizeOfHeapReserve;
    //     unsigned long long   SizeOfHeapCommit;
    //     int                  LoaderFlags;
    //     int                  NumberOfRvaAndSizes;
    //     IMAGE_DATA_DIRECTORY DataDirectory[16];
    // }; 
    //
    public sealed class ImageOptionalHeader64
    {
        public ImageHeaderMagic Magic { get; }
        public byte MajorLinkerVersion { get; }
        public byte MinorLinkerVersion { get; }
        public Int32 SizeOfCode { get; }
        public Int32 SizeOfInitializedData { get; }
        public Int32 SizeOfUninitializedData { get; }
        public Int32 AddressOfEntryPoint { get; }
        public Int32 BaseOfCode { get; }
        public UInt64 ImageBase { get; }
        public Int32 SectionAlignment { get; }
        public Int32 FileAlignment { get; }
        public Int16 MajorOperatingSystemVersion { get; }
        public Int16 MinorOperatingSystemVersion { get; }
        public Int16 MajorImageVersion { get; }
        public Int16 MinorImageVersion { get; }
        public Int16 MajorSubsystemVersion { get; }
        public Int16 MinorSubsystemVersion { get; }
        public Int32 Win32VersionValue { get; }
        public Int32 SizeOfImage { get; }
        public Int32 SizeOfHeaders { get; }
        public Int32 CheckSum { get; }
        public ImageSybsystemType Subsystem { get; }
        public ImageCharacteristics DllCharacteristics { get; }
        public UInt64 SizeOfStackReserve { get; }
        public UInt64 SizeOfStackCommit { get; }
        public UInt64 SizeOfHeapReserve { get; }
        public UInt64 SizeOfHeapCommit { get; }
        public Int32 LoaderFlags { get; }
        public Int32 NumberOfRvaAndSizes { get; }
        public ImageDataDirectories DataDirectory { get; }
        internal static readonly UInt32 SizeOfStruct = 0xF0;


        internal ImageOptionalHeader64(byte[] filebytes, UInt32 offset)
        {
            Magic = (ImageHeaderMagic)BinaryHelper.ToUInt16(filebytes, offset);
            MajorLinkerVersion = filebytes[offset + 2];
            MinorLinkerVersion = filebytes[offset + 3];
            SizeOfCode = BinaryHelper.ToInt32(filebytes, offset + 4);
            SizeOfInitializedData = BinaryHelper.ToInt32(filebytes, offset + 8);
            SizeOfUninitializedData = BinaryHelper.ToInt32(filebytes, offset + 12);
            AddressOfEntryPoint = BinaryHelper.ToInt32(filebytes, offset + 16);
            BaseOfCode = BinaryHelper.ToInt32(filebytes, offset + 20);
            ImageBase = BinaryHelper.ToUInt64(filebytes, offset + 24);
            SectionAlignment = BinaryHelper.ToInt32(filebytes, offset + 32);
            FileAlignment = BinaryHelper.ToInt32(filebytes, offset + 36);
            MajorOperatingSystemVersion = BinaryHelper.ToInt16(filebytes, offset + 40);
            MinorOperatingSystemVersion = BinaryHelper.ToInt16(filebytes, offset + 42);
            MajorImageVersion = BinaryHelper.ToInt16(filebytes, offset + 44);
            MinorImageVersion = BinaryHelper.ToInt16(filebytes, offset + 46);
            MajorSubsystemVersion = BinaryHelper.ToInt16(filebytes, offset + 48);
            MinorSubsystemVersion = BinaryHelper.ToInt16(filebytes, offset + 50);
            Win32VersionValue = BinaryHelper.ToInt32(filebytes, offset + 52);
            SizeOfImage = BinaryHelper.ToInt32(filebytes, offset + 56);
            SizeOfHeaders = BinaryHelper.ToInt32(filebytes, offset + 60);
            CheckSum = BinaryHelper.ToInt32(filebytes, offset + 64);
            Subsystem = (ImageSybsystemType)BinaryHelper.ToUInt16(filebytes, offset + 68);
            DllCharacteristics = (ImageCharacteristics)BinaryHelper.ToUInt16(filebytes, offset + 70);
            SizeOfStackReserve = BinaryHelper.ToUInt64(filebytes, offset + 72);
            SizeOfStackCommit = BinaryHelper.ToUInt64(filebytes, offset + 80);
            SizeOfHeapReserve = BinaryHelper.ToUInt64(filebytes, offset + 88);
            SizeOfHeapCommit = BinaryHelper.ToUInt64(filebytes, offset + 96);
            LoaderFlags = BinaryHelper.ToInt32(filebytes, offset + 104);
            NumberOfRvaAndSizes = BinaryHelper.ToInt32(filebytes, offset + 108);
            DataDirectory = new ImageDataDirectories(filebytes, offset + 112);
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; MajorLinkerVersion={1}; MinorLinkerVersion={2};...}}",
                Magic,
                MajorLinkerVersion,
                MinorLinkerVersion);
        }
    }
}
