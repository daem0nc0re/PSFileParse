using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_COFF_SYMBOLS_HEADER
    // {
    //     int NumberOfSymbols;
    //     int LvaToFirstSymbol;
    //     int NumberOfLinenumbers;
    //     int LvaToFirstLinenumber;
    //     int RvaToFirstByteOfCode;
    //     int RvaToLastByteOfCode;
    //     int RvaToFirstByteOfData;
    //     int RvaToLastByteOfData;
    // };
    // 
    public sealed class ImageCOFFSymbolsHeader
    {
        public UInt32 NumberOfSymbols { get; }
        public UInt32 LVAToFirstSymbol { get; }
        public UInt32 NumberOfLinenumbers { get; }
        public UInt32 LVAToFirstLinenumber { get; }
        public UInt32 RVAToFirstByteOfCode { get; }
        public UInt32 RVAToLastByteOfCode { get; }
        public UInt32 RVAToFirstByteOfData { get; }
        public UInt32 RVAToLastByteOfData { get; }
        internal static readonly UInt32 SizeOfStruct = 0x20;


        internal ImageCOFFSymbolsHeader(byte[] filebytes, UInt32 offset)
        {
            NumberOfSymbols = BinaryHelper.ToUInt32(filebytes, offset);
            LVAToFirstSymbol = BinaryHelper.ToUInt32(filebytes, offset + 4);
            NumberOfLinenumbers = BinaryHelper.ToUInt32(filebytes, offset + 8);
            LVAToFirstLinenumber = BinaryHelper.ToUInt32(filebytes, offset + 12);
            RVAToFirstByteOfCode = BinaryHelper.ToUInt32(filebytes, offset + 16);
            RVAToLastByteOfCode = BinaryHelper.ToUInt32(filebytes, offset + 20);
            RVAToFirstByteOfData = BinaryHelper.ToUInt32(filebytes, offset + 24);
            RVAToLastByteOfData = BinaryHelper.ToUInt32(filebytes, offset + 28);
        }


        public override String ToString()
        {
            return String.Format("@{{NumberOfSymbols={0}; LvaToFirstSymbol={1}; NumberOfLinenumbers={2};...}}",
                NumberOfSymbols,
                LVAToFirstSymbol,
                NumberOfLinenumbers);
        }
    }
}
