using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class MethodFlags
    {
        public MemberAccess MemberAccess { get; }
        public MethodAttributeFlags MethodAttributes { get; }
        public VtableLayout VtableLayout { get; }
        public MethodModifierFlags Modifier { get; }
        public InteropFlags Interop { get; }
        public MethodAdditionalFlags AdditionalFlags { get; }


        internal MethodFlags(byte[] filebytes, ref UInt32 offset)
        {
            var attributes = BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;

            MemberAccess = (MemberAccess)(attributes & 0x7);
            MethodAttributes = (MethodAttributeFlags)(attributes & 0xF0);
            VtableLayout = (VtableLayout)(attributes & 0x100);
            Modifier = (MethodModifierFlags)(attributes & 0xE00);
            Interop = (InteropFlags)(attributes & 0x2008);
            AdditionalFlags = (MethodAdditionalFlags)(attributes & 0xD000);
        }


        public override String ToString()
        {
            return String.Format("@{{MemberAccess={0}; MethodAttributes={1}; VtableLayout={2}; Modifier={3}; Interop={4}; AdditionalFlags={5}}}",
                MemberAccess,
                MethodAttributes,
                VtableLayout,
                Modifier,
                Interop,
                AdditionalFlags);
        }
    }
}
