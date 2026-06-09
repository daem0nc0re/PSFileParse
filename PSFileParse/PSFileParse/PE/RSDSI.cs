using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // Reference:
    // https://github.com/microsoft/microsoft-pdb/blob/082c5290e5aff028ae84e43affa8be717aa7af73/PDB/dbi/locator.h#L33
    //
    // struct RSDSI
    // {
    //     int     dwSig; // Should be "RSDS"
    //     GUID    guidSig;
    //     int     age;
    //     char    szPdb[_MAX_PATH * 3];
    // };
    //
    public sealed class RSDSI
    {
        public FileSignature Signature { get; }
        public Guid UniqueId { get; }
        public UInt32 Age { get; }
        public String Pdb { get; }


        internal RSDSI(byte[] filebytes, UInt32 offset, UInt32 size)
        {
            var guid_bytes = new byte[16];
            var name_length = size - 24;

            Signature = new FileSignature(filebytes, offset, 4);

            for (UInt32 i = 0; i < guid_bytes.Length; i++)
                guid_bytes[i] = filebytes[offset + 4 + i];

            UniqueId = new Guid(guid_bytes);
            Age = BinaryHelper.ToUInt32(filebytes, offset + 20);
            Pdb = BinaryHelper.GetAnsiString(filebytes, offset + 24, name_length);
        }


        public override String ToString()
        {
            return String.Format("@{{Signature={0}; UniqueId={1}; Age={2}; Pdb={3}}}",
                Signature,
                UniqueId,
                Age,
                Pdb);
        }
    }
}
