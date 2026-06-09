using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class MethodImplFlags
    {
        public MethodCodeTypes CodeType { get; }
        public MethodManageAttributes ManageType { get; }
        public MethodImplementationFlags Implementation { get; }


        internal MethodImplFlags(byte[] filebytes, ref UInt32 offset)
        {
            var attributes = BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;

            CodeType = (MethodCodeTypes)(attributes & 0x3);
            ManageType = (MethodManageAttributes)(attributes & 0x4);
            Implementation = (MethodImplementationFlags)attributes;
        }


        public override String ToString()
        {
            return String.Format("@{{CodeType={0}; ManageType={1}; Implementation={2}}}",
                CodeType,
                ManageType,
                Implementation);
        }
    }
}
