using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFileParse.MachO
{
    // 
    // typedef struct __SC_SuperBlob { // Same for Requirements blob
    //     uint32_t magic;
    //     uint32_t length;
    //     uint32_t count;
    //     CS_BlobIndex index[];
    // } CS_SuperBlob
    // 
    // typedef struct __BlobIndex {
    //     uint32_t type;
    //     uint32_t offset;
    // } CS_BlobIndex
    // 
    public sealed class CSRequirementsBlob
    {
        public CSMagic Magic { get; } // Should be CSMagic.Requirements (0xFADE0C01)
        public UInt32 Length { get; }
        public UInt32 Count { get; }
        public CSRequirementEntry[] Requirements { get; }


        internal CSRequirementsBlob(byte[] filebytes, UInt32 offset)
        {
            var superblob_base = offset;
            Magic = (CSMagic)BinaryHelper.ToUInt32Big(filebytes, offset);
            Length = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            Count = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            Requirements = new CSRequirementEntry[Count];
            offset += 12u;

            for (UInt32 i = 0; i < Count; i++)
            {
                Requirements[i] = new CSRequirementEntry(
                    filebytes,
                    offset,
                    i,
                    superblob_base);
                offset += 8u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; Length={1}; Count={2}; Requirements={3}}}",
                Magic,
                Length,
                Count,
                Requirements);
        }
    }
}
