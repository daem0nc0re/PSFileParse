using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public class GenericParamAttributeFlags
    {
        public GenericParamVarianceAttributes Variance { get; }
        public GenericParamConstraintFlags SpecialConstraint { get; }


        internal GenericParamAttributeFlags(byte[] filebytes, ref UInt32 offset)
        {
            var attributes = BinaryHelper.ToUInt16(filebytes, offset);
            offset += 2u;
            Variance = (GenericParamVarianceAttributes)(attributes & 0x3);
            SpecialConstraint = (GenericParamConstraintFlags)(attributes & 0x1C);
        }


        public override String ToString()
        {
            return String.Format("@{{Variance={0}; SpecialConstraint={1}}}",
                Variance,
                SpecialConstraint);
        }
    }
}
