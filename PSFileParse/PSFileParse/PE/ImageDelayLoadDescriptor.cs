using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_DELAYLOAD_DESCRIPTOR {
    //     union {
    //         int AllAttributes;
    //         struct {
    //             int RvaBased : 1;            // Delay load version 2
    //             int ReservedAttributes : 31;
    //         } DUMMYSTRUCTNAME;
    //     } Attributes;
    //     int DllNameRVA;                      // RVA to the name of the target library (NULL-terminate ASCII string)
    //     int ModuleHandleRVA;                 // RVA to the HMODULE caching location (PHMODULE)
    //     int ImportAddressTableRVA;           // RVA to the start of the IAT (PIMAGE_THUNK_DATA)
    //     int ImportNameTableRVA;              // RVA to the start of the name table (PIMAGE_THUNK_DATA::AddressOfData)
    //     int BoundImportAddressTableRVA;      // RVA to an optional bound IAT
    //     int UnloadInformationTableRVA;       // RVA to an optional unload info table
    //     int TimeDateStamp;                   // 0 if not bound,
    //                                          // Otherwise, date/time of the target Dll
    // };
    // 
    public sealed class ImageDelayLoadDescriptor
    {
        public Int32 Attributes { get; }
        public String DllName { get; }
        public Int32 ModuleHandleRVA { get; }
        public Int32 ImportAddressTableRVA { get; }
        public Int32 ImportNameTableRVA { get; }
        public Int32 ImportBoundImportAddressTableRVA { get; }
        public Int32 UnloadInformationTableRVA { get; }
        public UnixTime TimeDateStamp { get; }
        public DelayImportFunction[] ImportTable { get; }
        internal static readonly UInt32 SizeOfStruct = 0x20;


        internal ImageDelayLoadDescriptor(
            byte[] filebytes,
            UInt32 offset,
            ImageSectionHeader[] sections,
            bool is_64bit)
        {
            var name_offset = BinaryHelper.ToUInt32(filebytes, offset + 4);
            Attributes = BinaryHelper.ToInt32(filebytes, offset);
            DllName = BinaryHelper.GetAnsiString(
                filebytes,
                PEFile.VirtToPhys(sections, name_offset, out String _));
            ModuleHandleRVA = BinaryHelper.ToInt32(filebytes, offset + 8);
            ImportAddressTableRVA = BinaryHelper.ToInt32(filebytes, offset + 12);
            ImportNameTableRVA = BinaryHelper.ToInt32(filebytes, offset + 16);
            ImportBoundImportAddressTableRVA = BinaryHelper.ToInt32(filebytes, offset + 20);
            UnloadInformationTableRVA = BinaryHelper.ToInt32(filebytes, offset + 24);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, offset + 28));

            if (ImportAddressTableRVA != 0)
            {
                var dit = new List<DelayImportFunction>();
                var iat_base = PEFile.VirtToPhys(sections, (UInt32)ImportAddressTableRVA, out String _);
                var int_base = PEFile.VirtToPhys(sections, (UInt32)ImportNameTableRVA, out String _);

                while (true)
                {

                    if (is_64bit && (BinaryHelper.ToUInt64(filebytes, int_base) == 0))
                        break;
                    else if (!is_64bit && (BinaryHelper.ToUInt32(filebytes, int_base) == 0))
                        break;

                    dit.Add(new DelayImportFunction(
                        filebytes,
                        iat_base,
                        int_base,
                        sections,
                        is_64bit));
                    iat_base += is_64bit ? 8u : 4u;
                    int_base += is_64bit ? 8u : 4u;
                }

                ImportTable = dit.ToArray();
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Attributes={0}; DllName={1}; ModuleHandleRVA={2}; ImportAddressTableRVA={3}; ImportNameTableRVA={4}; ImportBoundImportAddressTableRVA={5}; UnloadInformationTableRVA={6}; TimeDateStamp={7}; ImportTable={8}}}",
                Attributes,
                DllName,
                ModuleHandleRVA,
                ImportAddressTableRVA,
                ImportNameTableRVA,
                ImportBoundImportAddressTableRVA,
                UnloadInformationTableRVA,
                TimeDateStamp,
                ImportTable);
        }
    }
}
