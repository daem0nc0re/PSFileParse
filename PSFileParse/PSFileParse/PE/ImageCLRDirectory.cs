using System;

namespace PSFileParse.PE
{
    public sealed class ImageCLRDirectory
    {
        public ImageCOR20Header COR20Header { get; }
        public CLRMetadata Metadata { get; }


        internal ImageCLRDirectory(
            byte[] filebytes,
            UInt32 offset,
            ImageSectionHeader[] sections)
        {
            COR20Header = new ImageCOR20Header(filebytes, offset);

            if (((UInt32)COR20Header.MetaData.VirtualAddress > 0) &&
                ((UInt32)COR20Header.MetaData.Size > 0))
            {
                UInt32 raw_offset = PEFile.VirtToPhys(
                    sections,
                    (UInt32)COR20Header.MetaData.VirtualAddress,
                    out String _);
                Metadata = new CLRMetadata(filebytes, raw_offset);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{COR20Header={0}; Metadata={1}}}",
                COR20Header,
                Metadata);
        }
    }
}
