using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class TypeFlags
    {
        public VisibilityAttributes Visibility { get; }
        public ClassLayoutFlags ClassLayout { get; }
        public ClassSemantics ClassSemantics { get; }
        public ClassSemanticsFlags AdditionalClassSemantics { get; }
        public ImplementationAttributes Implementation { get; }
        public StringFormattingFlags StringFormatting { get; }
        public ClassInitialization ClassInitialization { get; }
        public ClassAdditionalFlags AdditionalFlags { get; }


        internal TypeFlags(byte[] filebytes, ref UInt32 offset)
        {
            var attributes = BinaryHelper.ToInt32(filebytes, offset);
            offset += 4;

            Visibility = (VisibilityAttributes)(attributes & 0x7u);
            ClassLayout = (ClassLayoutFlags)(attributes & 0x18u);
            ClassSemantics = (ClassSemantics)(attributes & 0x20u);
            AdditionalClassSemantics = (ClassSemanticsFlags)(attributes & 0x580u);
            Implementation = (ImplementationAttributes)(attributes & 0x3000u);
            StringFormatting = (StringFormattingFlags)(attributes & 0x00030000u);
            ClassInitialization = (ClassInitialization)(attributes & 0x00100000u);
            AdditionalFlags = (ClassAdditionalFlags)(attributes & 0x00240800u);
        }


        public override String ToString()
        {
            return String.Format("@{{Visibility={0}; ClassLayout={1}; ClassSemantics={2}; AdditionalClassSemantics={3}; Implementation={4}; StringFormatting={5}; ClassInitialization={6}; AdditionalFlags={7}}}",
                Visibility,
                ClassLayout,
                ClassSemantics,
                AdditionalClassSemantics,
                Implementation,
                StringFormatting,
                ClassInitialization,
                AdditionalFlags);
        }
    }
}
