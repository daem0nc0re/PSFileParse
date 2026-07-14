using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // typedef struct __SC_SuperBlob {
    //     uint32_t magic;
    //     uint32_t length;
    //     uint32_t count;
    //     CS_BlobIndex index[];
    // } CS_SuperBlob
    // 
    public sealed class CSBlob
    {
        public UInt32 Magic { get; }
        public UInt32 Length { get; }
        public UInt32 Count { get; }
        public CSBlobIndex[] BlobIndexes { get; }
        public CSGenericBlobs[] Blobs { get; }


        public CSBlob(
            byte[] filebytes,
            UInt32 offset)
        {
            var index_base = offset + 12u;
            Magic = BinaryHelper.ToUInt32Big(filebytes, offset);
            Length = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            Count = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            BlobIndexes = new CSBlobIndex[Count];
            Blobs = new CSGenericBlobs[Count];

            for (UInt32 i = 0u; i < Count; i++)
            {
                BlobIndexes[i] = new CSBlobIndex(filebytes, ref index_base, i);
                Blobs[i] = new CSGenericBlobs(
                    filebytes,
                    offset + BlobIndexes[i].Offset,
                    i);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; Length={1}; Count={2}; BlobIndexes={3}; Blobs={4}}}",
                Magic,
                Length,
                Count,
                BlobIndexes,
                Blobs);
        }
    }
}
