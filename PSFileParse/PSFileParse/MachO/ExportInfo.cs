using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class ExportInfo
    {
        public ExportSymbolFlags Flags { get; }
        public Object Offset { get; }
        public Object Ordinal { get; }
        public String OtherName { get; }
        public Object StubOffset { get; }
        public Object ResolverOffset { get; }


        internal ExportInfo(byte[] value)
        {
            var offset = 0u;
            Flags = (ExportSymbolFlags)LEB128.ToUInt64(value, ref offset);

            if (Flags == ExportSymbolFlags.Reexport)
            {
                Ordinal = LEB128.ToUInt64(value, ref offset);
                OtherName = BinaryHelper.GetUTF8String(value, offset);
            }
            else if (Flags == ExportSymbolFlags.StubAndResolver)
            {
                StubOffset = LEB128.ToUInt64(value, ref offset);
                ResolverOffset = LEB128.ToUInt64(value, ref offset);
            }
            else
            {
                Offset = LEB128.ToUInt64(value, ref offset);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Flags={0}; Offset={1}; Ordinal={2}; OtherName={3}; StubOffset={4}; ResolverOffset={5}}}",
                Flags,
                Offset,
                Ordinal,
                OtherName,
                StubOffset,
                ResolverOffset);
        }
    }
}
