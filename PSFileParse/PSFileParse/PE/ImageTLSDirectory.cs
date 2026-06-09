using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_TLS_DIRECTORY64
    // {
    //     unsigned long long StartAddressOfRawData;
    //     unsigned long long EndAddressOfRawData;
    //     unsigned long long AddressOfIndex;         // PDWORD
    //     unsigned long long AddressOfCallBacks;     // PIMAGE_TLS_CALLBACK *;
    //     int                SizeOfZeroFill;
    //     union {
    //         int Characteristics;
    //         struct {
    //             int Reserved0 : 20;
    //             int Alignment : 4;
    //             int Reserved1 : 8;
    //         } DUMMYSTRUCTNAME;
    //     } DUMMYUNIONNAME;
    // };
    // 
    // struct IMAGE_TLS_DIRECTORY32
    // {
    //     int StartAddressOfRawData;
    //     int EndAddressOfRawData;
    //     int AddressOfIndex;             // PDWORD
    //     int AddressOfCallBacks;         // PIMAGE_TLS_CALLBACK *
    //     int SizeOfZeroFill;
    //     union {
    //         DWORD Characteristics;
    //         struct {
    //             int Reserved0 : 20;
    //             int Alignment : 4;
    //             int Reserved1 : 8;
    //         } DUMMYSTRUCTNAME;
    //     } DUMMYUNIONNAME;
    // };
    // 
    public sealed class ImageTLSDirectory
    {
        public IntPtr StartAddressOfRawData { get; }
        public IntPtr EndAddressOfRawData { get; }
        public IntPtr AddressOfIndex { get; }
        public IntPtr AddressOfCallBacks { get; }
        public Int32 SizeOfZeroFill { get; }
        public ImageSectionAlignment Characteristics { get; }


        internal ImageTLSDirectory(byte[] filebytes, UInt32 offset, bool is_64bit)
        {
            if (is_64bit)
            {
                StartAddressOfRawData = new IntPtr(BinaryHelper.ToInt64(filebytes, offset));
                EndAddressOfRawData = new IntPtr(BinaryHelper.ToInt64(filebytes, offset + 8));
                AddressOfIndex = new IntPtr(BinaryHelper.ToInt64(filebytes, offset + 16));
                AddressOfCallBacks = new IntPtr(BinaryHelper.ToInt64(filebytes, offset + 24));
                SizeOfZeroFill = BinaryHelper.ToInt32(filebytes, offset + 32);
                Characteristics = (ImageSectionAlignment)BinaryHelper.ToInt32(filebytes, offset + 36);
            }
            else
            {
                StartAddressOfRawData = new IntPtr(BinaryHelper.ToInt64(filebytes, offset));
                EndAddressOfRawData = new IntPtr(BinaryHelper.ToInt64(filebytes, offset + 4));
                AddressOfIndex = new IntPtr(BinaryHelper.ToInt64(filebytes, offset + 8));
                AddressOfCallBacks = new IntPtr(BinaryHelper.ToInt64(filebytes, offset + 12));
                SizeOfZeroFill = BinaryHelper.ToInt32(filebytes, offset + 16);
                Characteristics = (ImageSectionAlignment)BinaryHelper.ToInt32(filebytes, offset + 24);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{StartAddressOfRawData={0}; EndAddressOfRawData={1}; AddressOfIndex={2}; AddressOfCallBacks={3}; SizeOfZeroFill={4}; Characteristics={5}}}",
                StartAddressOfRawData,
                EndAddressOfRawData,
                AddressOfIndex,
                AddressOfCallBacks,
                SizeOfZeroFill,
                Characteristics);
        }
    }
}
