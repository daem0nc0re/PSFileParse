using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_DEBUG_MISC {
    //     int                         DataType;    // type of misc data, see defines
    //     int                         Length;      // total length of record, rounded to four
    //                                              // byte multiple.
    //     unsigned char /* BOOLEAN */ Unicode;     // TRUE if data is unicode string
    //     unsigned char               Reserved[3];
    //     unsigned char               Data[1];     // Actual data
    // };
    // 
    public sealed class ImageDebugMisc
    {
        public ImageDebugMiscType Type { get; }
        public UInt32 Length { get; }
        public bool IsUnicode { get; }
        public String Data { get; }


        internal ImageDebugMisc(byte[] filebytes, UInt32 offset)
        {
            Type = (ImageDebugMiscType)BinaryHelper.ToInt32(filebytes, offset);
            Length = BinaryHelper.ToUInt32(filebytes, offset + 4);
            IsUnicode = filebytes[offset + 8] != 0;

            if (IsUnicode)
                Data = BinaryHelper.GetUnicodeString(filebytes, offset + 12, Length);
            else
                Data = BinaryHelper.GetAnsiString(filebytes, offset + 12, Length);
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Length={1}; IsUnicode={2}; Data={3}}}",
                Type,
                Length,
                IsUnicode,
                Data);
        }
    }
}
