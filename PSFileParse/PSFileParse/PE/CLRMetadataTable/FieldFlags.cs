using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class FieldFlags
    {
        public FieldAccessAttributes Access { get; }
        public FieldAttributeFlags FieldAttributes { get; }
        public FieldInteropAttributes InteropAttributes { get; }
        public FieldAdditionalFlags AdditionalFlags { get; }


        internal FieldFlags(byte[] filebytes, ref UInt32 offset)
        {
            var attributes = BinaryHelper.ToInt16(filebytes, offset);
            offset += 2;

            Access = (FieldAccessAttributes)(attributes & 0x7);
            FieldAttributes = (FieldAttributeFlags)(attributes & 0x2F0);
            InteropAttributes = (FieldInteropAttributes)(attributes & 0x2000);
            AdditionalFlags = (FieldAdditionalFlags)(attributes & 0x9500);
        }


        public override String ToString()
        {
            return String.Format("@{{Access={0}; FieldAttributes={1}; InteropAttributes={2}; AdditionalFlags={3}}}",
                Access,
                FieldAttributes,
                InteropAttributes,
                AdditionalFlags);
        }
    }
}
