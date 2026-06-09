using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    public sealed class ImageNtHeaders
    {
        public FileSignature Signature { get; }
        public ImageFileHeader FileHeader { get; }
        public Object OptionalHeader { get; }


        internal ImageNtHeaders(
            byte[] filebytes,
            UInt32 offset,
            out ImageHeaderMagic magic)
        {
            Signature = new FileSignature(filebytes, offset, 4);
            FileHeader = new ImageFileHeader(filebytes, offset + 4);
            magic = (ImageHeaderMagic)BinaryHelper.ToUInt16(filebytes, offset + 24);

            if (magic == ImageHeaderMagic.ROM)
                OptionalHeader = new ImageRomOptionalHeader(filebytes, offset + 24);
            else if (magic == ImageHeaderMagic.NT32)
                OptionalHeader = new ImageOptionalHeader32(filebytes, offset + 24);
            else if (magic == ImageHeaderMagic.NT64)
                OptionalHeader = new ImageOptionalHeader64(filebytes, offset + 24);
            else
                throw new ArgumentException("Invalid optinal header magic.");
        }


        public override String ToString()
        {
            return String.Format("@{{Signature={0}; FileHeader={1}; OptionalHeader={2}}}",
                Signature,
                FileHeader,
                OptionalHeader);
        }
    }
}
