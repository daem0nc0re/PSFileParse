using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_IMPORT_DESCRIPTOR
    // {
    //     union {
    //         int Characteristics;    // 0 for terminating null import descriptor
    //         int OriginalFirstThunk; // RVA to original unbound IAT (PIMAGE_THUNK_DATA)
    //     } DUMMYUNIONNAME;
    //     int TimeDateStamp;          // 0 if not bound,
    //                                 // -1 if bound, and real date\time stamp
    //                                 //     in IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT (new BIND)
    //                                 // O.W. date/time stamp of DLL bound to (Old BIND)
    //     int ForwarderChain;         // -1 if no forwarders
    //     int Name;
    //     int FirstThunk;             // RVA to IAT (if bound this IAT has actual addresses)
    // };
    //
    public sealed class ImageImportDescriptor
    {
        public UInt32 Index { get; }
        public UInt32 OriginalFirstThunk { get; }
        public UnixTime TimeDateStamp { get; }
        public UInt32 ForwarderChain { get; }
        public String Name { get; }
        public UInt32 FirstThunk { get; }
        public ImageThunkData[] LookupTable { get; }
        internal bool IsNull { get; }
        internal static readonly UInt32 SizeOfStruct = 0x14;


        public ImageImportDescriptor(
            byte[] filebytes,
            UInt32 index,
            UInt32 offset,
            ImageSectionHeader[] sections,
            bool is_64bit)
        {
            UInt32 thunk_base;
            UInt32 delta;
            var lookup_table = new List<ImageThunkData>();
            var epoch = BinaryHelper.ToUInt32(filebytes, offset + 4);
            var name_rva = BinaryHelper.ToUInt32(filebytes, offset + 12);
            var name_offset = PEFile.VirtToPhys(sections, name_rva, out String _);
            Index = index;
            OriginalFirstThunk = BinaryHelper.ToUInt32(filebytes, offset);
            TimeDateStamp = new UnixTime(epoch);
            ForwarderChain = BinaryHelper.ToUInt32(filebytes, offset + 8);
            FirstThunk = BinaryHelper.ToUInt32(filebytes, offset + 16);
            IsNull = (OriginalFirstThunk | epoch | ForwarderChain | name_rva | FirstThunk) == 0;

            if (IsNull)
                return;

            Name = BinaryHelper.GetAnsiString(filebytes, name_offset);

            foreach (Char c in Name)
            {
                if (Char.IsControl(c))
                {
                    IsNull = false;
                    return;
                }
            }

            if (OriginalFirstThunk != 0)
            {
                thunk_base = PEFile.VirtToPhys(sections, OriginalFirstThunk, out String _);
                delta = OriginalFirstThunk - thunk_base;
            }
            else
            {
                thunk_base = PEFile.VirtToPhys(sections, FirstThunk, out String _);
                delta = FirstThunk - thunk_base;
            }

            if (is_64bit)
            {
                while (true)
                {
                    if (BinaryHelper.ToUInt64(filebytes, thunk_base) == 0)
                        break;

                    lookup_table.Add(new ImageThunkData(
                        filebytes,
                        thunk_base,
                        delta,
                        true));
                    thunk_base += 8;
                }
            }
            else
            {
                while (true)
                {
                    if (BinaryHelper.ToUInt32(filebytes, thunk_base) == 0)
                        break;

                    lookup_table.Add(new ImageThunkData(
                        filebytes,
                        thunk_base,
                        delta,
                        false));
                    thunk_base += 4;
                }
            }

            LookupTable = lookup_table.ToArray();
        }


        public override String ToString()
        {
            return String.Format("@{{OriginalFirstThunk={0}; TimeDateStamp={1}; ForwarderChain={2}; Name={3}; FirstThunk={4}; LookupTable={5}}}",
                OriginalFirstThunk,
                TimeDateStamp,
                ForwarderChain,
                Name,
                FirstThunk,
                LookupTable);
        }
    }
}
