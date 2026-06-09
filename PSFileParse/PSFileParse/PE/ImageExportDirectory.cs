using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    //
    // struct IMAGE_EXPORT_DIRECTORY {
    //     int   Characteristics;
    //     int   TimeDateStamp;
    //     short MajorVersion;
    //     short MinorVersion;
    //     int   Name;
    //     int   Base;
    //     int   NumberOfFunctions;
    //     int   NumberOfNames;
    //     int   AddressOfFunctions;     // RVA from base of image
    //     int   AddressOfNames;         // RVA from base of image
    //     int   AddressOfNameOrdinals;  // RVA from base of image
    // };
    //
    public sealed class ImageExportDirectory
    {
        public Int32 Characteristics { get; }
        public UnixTime TimeDateStamp { get; }
        public Int16 MajorVersion { get; }
        public Int16 MinorVersion { get; }
        public String Name { get; }
        public Int32 Base { get; }
        public Int32 NumberOfFunctions { get; }
        public Int32 NumberOfNames { get; }
        public Int32 AddressOfFunctions { get; }
        public Int32 AddressOfNames { get; }
        public Int32 AddressOfNameOrdinals { get; }
        public ExportFunction[] Items { get; }
        internal static readonly UInt32 SizeOfStruct = 0x28;


        internal ImageExportDirectory(
            byte[] filebytes,
            ImageDataDirectory directory,
            ImageSectionHeader[] sections)
        {
            var hint = 0u;
            var virt_addr = (UInt32)directory.VirtualAddress;
            var raw_offset = PEFile.VirtToPhys(sections, virt_addr, out _);
            var delta = virt_addr - raw_offset;
            var name_offset = (UInt32)BinaryHelper.ToInt32(filebytes, raw_offset + 12);
            Characteristics = BinaryHelper.ToInt32(filebytes, raw_offset);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, raw_offset + 4));
            MajorVersion = BinaryHelper.ToInt16(filebytes, raw_offset + 8);
            MinorVersion = BinaryHelper.ToInt16(filebytes, raw_offset + 10);
            Name = BinaryHelper.GetAnsiString(filebytes, name_offset - delta);
            Base = BinaryHelper.ToInt32(filebytes, raw_offset + 16);
            NumberOfFunctions = BinaryHelper.ToInt32(filebytes, raw_offset + 20);
            NumberOfNames = BinaryHelper.ToInt32(filebytes, raw_offset + 24);
            AddressOfFunctions = BinaryHelper.ToInt32(filebytes, raw_offset + 28);
            AddressOfNames = BinaryHelper.ToInt32(filebytes, raw_offset + 32);
            AddressOfNameOrdinals = BinaryHelper.ToInt32(filebytes, raw_offset + 36);
            Items = new ExportFunction[NumberOfFunctions];

            for (UInt32 i = 0; i < (UInt32)NumberOfNames; i++)
            {
                var ordinal = BinaryHelper.ToUInt16(
                    filebytes,
                    (UInt32)AddressOfNameOrdinals + (i * 2) - delta);
                name_offset = BinaryHelper.ToUInt32(
                    filebytes,
                    (UInt32)AddressOfNames + (i * 4) - delta) - delta;
                virt_addr = (UInt32)BinaryHelper.ToUInt32(
                    filebytes,
                    (UInt32)AddressOfFunctions + ((UInt32)ordinal * 4) - delta);
                raw_offset = PEFile.VirtToPhys(sections, virt_addr, out String sec_name);
                Items[ordinal] = new ExportFunction(
                    (UInt32)(ordinal + Base),
                    hint++,
                    sec_name,
                    virt_addr,
                    raw_offset,
                    BinaryHelper.GetAnsiString(filebytes, name_offset));
            }

            if (NumberOfFunctions > NumberOfNames)
            {
                for (UInt32 i = 0; i < (UInt32)NumberOfFunctions; i++)
                {
                    if (Items[i] != null)
                        continue;

                    virt_addr = (UInt32)BinaryHelper.ToUInt32(
                        filebytes,
                        (UInt32)AddressOfFunctions + (i * 4) - delta);
                    raw_offset = PEFile.VirtToPhys(sections, virt_addr, out String sec_name);
                    Items[i] = new ExportFunction(
                        (UInt32)(i + Base),
                        null,
                        sec_name,
                        virt_addr,
                        raw_offset,
                        null);
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Characteristics={0}; TimeDateStamp={1}; MajorVersion={2}; MinorVersion={3}; Name={4}; Base={5}; NumberOfFunctions={6}; NumberOfNames={7}; AddressOfFunctions={8}; AddressOfNames={9}; AddressOfNameOrdinals={10}; Items={11}}}",
                Characteristics,
                TimeDateStamp,
                MajorVersion,
                MinorVersion,
                Name,
                Base,
                NumberOfFunctions,
                NumberOfNames,
                AddressOfFunctions,
                AddressOfNames,
                AddressOfNameOrdinals,
                Items);
        }
    }
}
