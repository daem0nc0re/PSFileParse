using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    // 
    // struct dyld_chained_starts_in_image
    // {
    //     uint32_t    seg_count;
    //     uint32_t    seg_info_offset[1];
    //     // followed by pool of dyld_chain_starts_in_segment data
    // };
    // 
    public sealed class DyldChainedStartsInImage
    {
        public UInt32 NumberOfSegments { get; }
        public UInt32[] SegmentOffsets { get; }
        public Dictionary<UInt32, DyldChainedStartsInSegment> StartsInSegment { get; } = new Dictionary<UInt32, DyldChainedStartsInSegment>();


        internal DyldChainedStartsInImage(
            byte[] filebytes,
            UInt32 offset,
            MachOSection[] sections,
            DyldChainedImport[] imports,
            bool is_bigendian)
        {
            UInt32 starts_oft = offset + 4u;

            if (is_bigendian)
                NumberOfSegments = BinaryHelper.ToUInt32Big(filebytes, offset);
            else
                NumberOfSegments = BinaryHelper.ToUInt32(filebytes, offset);

            SegmentOffsets = new UInt32[NumberOfSegments];

            for (UInt32 i = 0; i < NumberOfSegments; i++)
            {
                if (is_bigendian)
                    SegmentOffsets[i] = BinaryHelper.ToUInt32Big(filebytes, starts_oft);
                else
                    SegmentOffsets[i] = BinaryHelper.ToUInt32(filebytes, starts_oft);

                if ((SegmentOffsets[i] != 0u) && (!StartsInSegment.ContainsKey(SegmentOffsets[i])))
                {
                    var starts_in_seg = new DyldChainedStartsInSegment(
                        filebytes,
                        offset + SegmentOffsets[i],
                        sections,
                        imports,
                        is_bigendian);
                    StartsInSegment.Add(SegmentOffsets[i], starts_in_seg);
                }

                starts_oft += 4u;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{NumberOfSegments={0}; SegmentOffsets={1}; StartsInSegment={2}}}",
                NumberOfSegments,
                SegmentOffsets,
                StartsInSegment);
        }
    }
}
