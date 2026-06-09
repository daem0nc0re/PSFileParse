using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    public class ImageLoadConfigCodeIntegrity
    {
        public UInt16 Flags { get; }
        public UInt16 Catalog { get; }
        public UInt32 CatalogOffset { get; }
        public UInt32 Reserved { get; }


        internal ImageLoadConfigCodeIntegrity(byte[] filebytes, UInt32 offset)
        {
            Flags = BinaryHelper.ToUInt16(filebytes, offset);
            Catalog = BinaryHelper.ToUInt16(filebytes, offset + 2);
            CatalogOffset = BinaryHelper.ToUInt32(filebytes, offset + 4);
            Reserved = BinaryHelper.ToUInt32(filebytes, offset + 8);
        }


        public override String ToString()
        {
            return String.Format("@{{Flags={0}; Catalog={1}; CatalogOffet={2}; Reserved={3}}}",
                Flags,
                Catalog,
                CatalogOffset,
                Reserved);
        }
    }
}
