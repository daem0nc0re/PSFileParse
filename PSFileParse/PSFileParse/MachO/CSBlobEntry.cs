using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // typedef struct __BlobIndex {
    //     uint32_t type;
    //     uint32_t offset;
    // } CS_BlobIndex
    // 
    public sealed class CSBlobEntry
    {
        public UInt32 Index { get; }
        public CSSlotType Type { get; }
        public UInt32 Offset { get; }
        public Object Content { get; }


        public CSBlobEntry(
            byte[] filebytes,
            UInt32 superblob_base,
            UInt32 index)
        {
            var index_base = superblob_base + 12u + (8u * index);
            Index = index;
            Type = (CSSlotType)BinaryHelper.ToUInt32Big(filebytes, index_base);
            Offset = BinaryHelper.ToUInt32Big(filebytes, index_base + 4);

            if (Type == CSSlotType.CodeDirectory)
                Content = new CSCodeDirectory(filebytes, superblob_base + Offset);
            else if (Type == CSSlotType.Requirements)
                Content = new CSRequirementsBlob(filebytes, superblob_base + Offset);
            else
                Content = new CSGenericBlobs(filebytes, superblob_base + Offset);
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Offset={1}; Content={2}}}",
                Type,
                Offset,
                Content);
        }
    }
}
