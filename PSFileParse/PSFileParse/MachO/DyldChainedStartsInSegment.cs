using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    // 
    // struct dyld_chained_starts_in_segment
    // {
    //     uint32_t    size;
    //     uint16_t    page_size;
    //     uint16_t    pointer_format;
    //     uint64_t    segment_offset;
    //     uint32_t    max_valid_pointer;
    //     uint16_t    page_count;
    //     uint16_t    page_start[1];
    //  // uint16_t    chain_starts[1];
    // };
    // 
    public sealed class DyldChainedStartsInSegment
    {
        public UInt32 Size { get; }
        public UInt16 PageSize { get; }
        public DyldChainedPointerFormat PointerFormat { get; }
        public UInt64 SegmentOffset { get; }
        public UInt32 MaxValidPointer { get; }
        public UInt16 PageCount { get; }
        public UInt16[] PageStart { get; }
        public Object Pointers { get; }


        internal DyldChainedStartsInSegment(
            byte[] filebytes,
            UInt32 offset,
            MachOSection[] sections,
            DyldChainedImport[] imports,
            bool is_bigendian)
        {
            UInt32 ptr_index = 0u;
            var pointers = new List<DyldChaindPointerEntry>();

            if (is_bigendian)
            {
                Size = BinaryHelper.ToUInt32Big(filebytes, offset);
                PageSize = BinaryHelper.ToUInt16Big(filebytes, offset + 4);
                PointerFormat = (DyldChainedPointerFormat)BinaryHelper.ToUInt16Big(filebytes, offset + 6);
                SegmentOffset = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                MaxValidPointer = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                PageCount = BinaryHelper.ToUInt16Big(filebytes, offset + 20);
            }
            else
            {
                Size = BinaryHelper.ToUInt32(filebytes, offset);
                PageSize = BinaryHelper.ToUInt16(filebytes, offset + 4);
                PointerFormat = (DyldChainedPointerFormat)BinaryHelper.ToUInt16(filebytes, offset + 6);
                SegmentOffset = BinaryHelper.ToUInt64(filebytes, offset + 8);
                MaxValidPointer = BinaryHelper.ToUInt32(filebytes, offset + 16);
                PageCount = BinaryHelper.ToUInt16(filebytes, offset + 20);
            }

            PageStart = new UInt16[PageCount];

            for (UInt32 i = 0u; i < PageCount; i++)
            {
                if (is_bigendian)
                    PageStart[i] = BinaryHelper.ToUInt16Big(filebytes, offset + 22 + (i * 2));
                else
                    PageStart[i] = BinaryHelper.ToUInt16(filebytes, offset + 22 + (i * 2));
            }

            for (UInt32 i = 0u; i < PageCount; i++)
            {
                var ptr_offset = SegmentOffset + (PageSize * i) + PageStart[i];

                while (true)
                {
                    var ptr = new DyldChaindPointerEntry(
                        filebytes,
                        ref ptr_offset,
                        ptr_index++,
                        PointerFormat,
                        MaxValidPointer,
                        sections,
                        imports,
                        is_bigendian);
                    pointers.Add(ptr);

                    if (ptr.Next == 0)
                        break;
                }
            }

            Pointers = pointers.ToArray();
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; PageSize={1}; PointerFormat={2}; SegmentOffset={3}; MaxValidPointer={4}; PageCount={5}; PageStart={6}}}",
                Size,
                PageSize,
                PointerFormat,
                SegmentOffset,
                MaxValidPointer,
                PageCount,
                PageStart);
        }
    }
}
