using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_ROM_OPTIONAL_HEADER
    //{
    //     short         Magic;
    //     unsigned char MajorLinkerVersion;
    //     unsigned char MinorLinkerVersion;
    //     int           SizeOfCode;
    //     int           SizeOfInitializedData;
    //     int           SizeOfUninitializedData;
    //     int           AddressOfEntryPoint;
    //     int           BaseOfCode;
    //     int           BaseOfData;
    //     int           BaseOfBss;
    //     int           GprMask;
    //     int           CprMask[4];
    //     int           GpValue;
    // };
    //
    public sealed class ImageRomOptionalHeader
    {
        public ImageHeaderMagic Magic { get; }
        public byte MajorLinkerVersion { get; }
        public byte MinorLinkerVersion { get; }
        public Int32 SizeOfCode { get; }
        public Int32 SizeOfInitializedData { get; }
        public Int32 SizeOfUninitializedData { get; }
        public Int32 AddressOfEntryPoint { get; }
        public Int32 BaseOfCode { get; }
        public Int32 BaseOfData { get; }
        public Int32 BaseOfBss { get; }
        public Int32 GprMask { get; }
        public Int32[] CprMask { get; }
        public Int32 GpValue { get; }
        internal static readonly UInt32 SizeOfStruct = 0x38;


        internal ImageRomOptionalHeader(byte[] filebytes, UInt32 offset)
        {
            Magic = (ImageHeaderMagic)BinaryHelper.ToUInt16(filebytes, offset);
            MajorLinkerVersion = filebytes[offset + 2];
            MinorLinkerVersion = filebytes[offset + 3];
            SizeOfCode = BinaryHelper.ToInt32(filebytes, offset + 4);
            SizeOfInitializedData = BinaryHelper.ToInt32(filebytes, offset + 8);
            SizeOfUninitializedData = BinaryHelper.ToInt32(filebytes, offset + 12);
            AddressOfEntryPoint = BinaryHelper.ToInt32(filebytes, offset + 16);
            BaseOfCode = BinaryHelper.ToInt32(filebytes, offset + 20);
            BaseOfData = BinaryHelper.ToInt32(filebytes, offset + 24);
            BaseOfBss = BinaryHelper.ToInt32(filebytes, offset + 28);
            GprMask = BinaryHelper.ToInt32(filebytes, offset + 32);
            CprMask = new Int32[]
            {
                BinaryHelper.ToInt32(filebytes, offset + 36),
                BinaryHelper.ToInt32(filebytes, offset + 40),
                BinaryHelper.ToInt32(filebytes, offset + 44),
                BinaryHelper.ToInt32(filebytes, offset + 48)
            };
            GpValue = BinaryHelper.ToInt32(filebytes, offset + 52);
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; MajorLinkerVersion={1}; MinorLinkerVersion={2}; SizeOfCode={3}; SizeOfInitializedData={4}; SizeOfUninitializedData={5}; AddressOfEntryPoint={6}; BaseOfCode={7}; BaseOfData={8}; BaseOfBss={9}; GprMask={10}; CprMask={11}; GpValue={12}}}",
                Magic,
                MajorLinkerVersion,
                MinorLinkerVersion,
                SizeOfCode,
                SizeOfInitializedData,
                SizeOfUninitializedData,
                AddressOfEntryPoint,
                BaseOfCode,
                BaseOfData,
                BaseOfBss,
                GprMask,
                CprMask,
                GpValue);
        }
    }
}
