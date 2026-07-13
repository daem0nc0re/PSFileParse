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
        public CSBlobIndex[] Blobs { get; }


        public CSBlob(
            byte[] filebytes,
            UInt32 offset)
        {
            Magic = BinaryHelper.ToUInt32Big(filebytes, offset);
            Length = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            Count = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            Blobs = new CSBlobIndex[Count];
            offset += 12u;

            for (UInt32 i = 0u; i < Count; i++)
                Blobs[i] = new CSBlobIndex(filebytes, ref offset, i);
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; Length={1}; Count={2}; Blobs={3}}}",
                Magic,
                Length,
                Count,
                Blobs);
        }
    }
}
