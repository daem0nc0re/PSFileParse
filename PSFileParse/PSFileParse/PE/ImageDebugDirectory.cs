using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_DEBUG_DIRECTORY {
    //     int Characteristics;
    //     int TimeDateStamp;
    //     short MajorVersion;
    //     short MinorVersion;
    //     int Type;
    //     int SizeOfData;
    //     int AddressOfRawData;
    //     int PointerToRawData;
    // };
    //
    public sealed class ImageDebugDirectory
    {
        public UInt32 Index { get; }
        public UInt32 Characteristics { get; }
        public UnixTime TimeDateStamp { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public DebugType Type { get; }
        public UInt32 SizeOfData { get; }
        public UInt32 AddressOfRawData { get; }
        public UInt32 PointerToRawData { get; }
        public Object DebugData { get; }
        internal static readonly UInt32 SizeOfStruct = 0x1C;


        internal ImageDebugDirectory(
            byte[] filebytes,
            UInt32 index,
            UInt32 offset)
        {
            Index = index;
            Characteristics = BinaryHelper.ToUInt32(filebytes, offset);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, offset + 4));
            MajorVersion = BinaryHelper.ToUInt16(filebytes, offset + 8);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, offset + 10);
            Type = (DebugType)BinaryHelper.ToUInt32(filebytes, offset + 12);
            SizeOfData = BinaryHelper.ToUInt32(filebytes, offset + 16);
            AddressOfRawData = BinaryHelper.ToUInt32(filebytes, offset + 20);
            PointerToRawData = BinaryHelper.ToUInt32(filebytes, offset + 24);

            if ((SizeOfData == 0) || (PointerToRawData == 0))
            {
                return;
            }
            else if (Type == DebugType.COFF)
            {
                DebugData = new ImageCOFFSymbolsHeader(filebytes, PointerToRawData);
            }
            else if (Type == DebugType.FPO)
            {
                DebugData = new FPOData(filebytes, PointerToRawData);
            }
            else if (Type == DebugType.CodeView)
            {
                DebugData = new RSDSI(filebytes, PointerToRawData, SizeOfData);
            }
            else if (Type == DebugType.Misc)
            {
                DebugData = new ImageDebugMisc(filebytes, PointerToRawData);
            }
            else if (Type == DebugType.Repro)
            {
                DebugData = new ReproHash(filebytes, PointerToRawData);
            }
            else if (Type == DebugType.ExDllCharacteristics)
            {
                DebugData = (ImageDllCharacteristicsEx)BinaryHelper.ToUInt32(
                    filebytes,
                    PointerToRawData);
            }
            else
            {
                var data = new byte[SizeOfData];

                for (UInt32 i = 0u; i < SizeOfData; i++)
                    data[i] = filebytes[i + PointerToRawData];

                DebugData = data;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Characteristics={0}; TimeDateStamp={1}; MajorVersion={2}; MinorVersion={3}; Type={4}; SizeOfData={5}; AddressOfRawData={6}; PointerToRawData={7}; DebugData={8}}}",
                Characteristics,
                TimeDateStamp,
                MajorVersion,
                MinorVersion,
                Type,
                SizeOfData,
                AddressOfRawData,
                PointerToRawData,
                DebugData);
        }
    }
}
