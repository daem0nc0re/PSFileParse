using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    public sealed class DelayImportFunction
    {
        public IntPtr Address { get; }
        public Object Ordinal { get; }
        public Object Hint { get; }
        public String Name { get; }


        internal DelayImportFunction(
            byte[] filebytes,
            UInt32 raw_addr_offset,
            UInt32 int_offset,
            ImageSectionHeader[] sections,
            bool is_64bit)
        {
            if (is_64bit)
            {
                var addr_of_data = BinaryHelper.ToUInt64(filebytes, int_offset);
                Address = new IntPtr(BinaryHelper.ToInt64(filebytes, raw_addr_offset));

                if ((addr_of_data & 0x8000000000000000UL) != 0)
                {
                    Ordinal = (UInt16)(addr_of_data & 0xFFFF);
                }
                else
                {
                    var hint_name_addr = (UInt32)(addr_of_data & 0x7FFFFFFF);
                    var hint_name = new ImageImportByName(
                        filebytes,
                        PEFile.VirtToPhys(sections, hint_name_addr, out String _));
                    Hint = hint_name.Hint;
                    Name = hint_name.Name;
                }
            }
            else
            {
                var addr_of_data = BinaryHelper.ToUInt32(filebytes, int_offset);
                Address = new IntPtr(BinaryHelper.ToInt32(filebytes, raw_addr_offset));

                if ((addr_of_data & 0x80000000) != 0)
                {
                    Ordinal = (UInt16)(addr_of_data & 0xFFFF);
                }
                else
                {
                    var hint_name_addr = addr_of_data & 0x7FFFFFFF;
                    var hint_name = new ImageImportByName(
                        filebytes,
                        PEFile.VirtToPhys(sections, hint_name_addr, out String _));
                    Hint = hint_name.Hint;
                    Name = hint_name.Name;
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Address={0}; Ordinal={1}; Hint={2}; Name={3}}}",
                Address,
                Ordinal,
                Hint,
                Name);
        }
    }
}
