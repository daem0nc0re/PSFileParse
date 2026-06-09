using System;

namespace PSFileParse.MachO
{
    public sealed class DyldChainedFixups
    {
        public DyldChainedFixupsHeader FixupsHeader { get; }
        public DyldChainedStartsInImage StartsInImage { get; }


        internal DyldChainedFixups(
            byte[] filebytes,
            UInt32 offset,
            DylibTableEntry[] dylibs,
            MachOSection[] sections,
            bool is_bigendian)
        {
            FixupsHeader = new DyldChainedFixupsHeader(filebytes, offset, dylibs, is_bigendian);
            StartsInImage = new DyldChainedStartsInImage(
                filebytes,
                offset + FixupsHeader.StartsOffset,
                sections,
                FixupsHeader.Imports,
                is_bigendian);
        }


        public override String ToString()
        {
            return String.Format("@{{FixupsHeader={0}; StartsInImage={1}}}",
                FixupsHeader,
                StartsInImage);
        }
    }
}
