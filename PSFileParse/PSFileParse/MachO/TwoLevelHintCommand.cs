using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct twolevel_hints_command
    // {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t offset;
    //     uint32_t nhints;
    // };
    // 
    public sealed class TwoLevelHintCommand
    {
        public UInt32 Offset { get; }
        public UInt32 NumberOfHints { get; }
        public TwoLevelHint[] Hints { get; }


        internal TwoLevelHintCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 table_offset;

            if (is_bigendian)
            {
                Offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                NumberOfHints = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                Offset = BinaryHelper.ToUInt32(filebytes, offset);
                NumberOfHints = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            table_offset = Offset;
            Hints = new TwoLevelHint[NumberOfHints];

            for (UInt32 i = 0u; i < NumberOfHints; i++)
                Hints[i] = new TwoLevelHint(filebytes, ref table_offset, i, is_bigendian);
        }


        public override String ToString()
        {
            return String.Format("@{{Offset={0}; NumberOfHints={1}; Hints={2}}}",
                Offset,
                NumberOfHints,
                Hints);
        }
    }
}