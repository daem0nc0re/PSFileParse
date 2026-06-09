using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_IMPORT_BY_NAME {
    //     short Hint;
    //     char  Name[1];
    // };
    // 
    public sealed class ImageImportByName
    {
        public UInt16 Hint { get; }
        public String Name { get; }


        internal ImageImportByName(byte[] filebytes, UInt32 offset)
        {
            Hint = BinaryHelper.ToUInt16(filebytes, offset);
            Name = BinaryHelper.GetAnsiString(filebytes, offset + 2);

            if (Name == String.Empty)
                Name = null;
        }


        public override String ToString()
        {
            return String.Format("@{{Hint={0}; Name={1}}}", Hint, Name);
        }
    }
}
