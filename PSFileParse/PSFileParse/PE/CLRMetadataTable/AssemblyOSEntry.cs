using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class AssemblyOSEntry
    {
        public UInt32 Index { get; }
        public UInt32 OSPlatformID { get; }
        public UInt32 OSMajorVersion { get; }
        public UInt32 OSMinorVersion { get; }


        internal AssemblyOSEntry(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index)
        {
            Index = index;
            OSPlatformID = BinaryHelper.ToUInt32(filebytes, offset);
            OSMajorVersion = BinaryHelper.ToUInt32(filebytes, offset + 4);
            OSMinorVersion = BinaryHelper.ToUInt32(filebytes, offset + 8);
            offset += 12u;
        }


        public override String ToString()
        {
            return String.Format("@{{MajorVersion={0}; MinorVersion={1}; OSMinorVersion={2}}}",
                OSPlatformID,
                OSMajorVersion,
                OSMinorVersion);
        }
    }
}
