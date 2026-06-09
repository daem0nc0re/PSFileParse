using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_COR20_HEADER
    // {
    //     // Header versioning
    //     int                     cb;
    //     short                   MajorRuntimeVersion;
    //     short                   MinorRuntimeVersion;
    //     // Symbol table and startup information
    //     IMAGE_DATA_DIRECTORY    MetaData;
    //     int                     Flags;
    //     // If COMIMAGE_FLAGS_NATIVE_ENTRYPOINT is not set, EntryPointToken represents a managed entrypoint.
    //     // If COMIMAGE_FLAGS_NATIVE_ENTRYPOINT is set, EntryPointRVA represents an RVA to a native entrypoint.
    //     union {
    //         int                 EntryPointToken;
    //         int                 EntryPointRVA;
    //     } DUMMYUNIONNAME;
    //     // Binding information
    //     IMAGE_DATA_DIRECTORY    Resources;
    //     IMAGE_DATA_DIRECTORY    StrongNameSignature;
    //     // Regular fixup and binding information
    //     IMAGE_DATA_DIRECTORY    CodeManagerTable;
    //     IMAGE_DATA_DIRECTORY    VTableFixups;
    //     IMAGE_DATA_DIRECTORY    ExportAddressTableJumps;
    //     // Precompiled image info (internal use only - set to zero)
    //     IMAGE_DATA_DIRECTORY    ManagedNativeHeader;
    // }
    //
    public sealed class ImageCOR20Header
    {
        public Int32 Size { get; }
        public Int16 MajorRuntimeVersion { get; }
        public Int16 MinorRuntimeVersion { get; }
        public ImageDataDirectory MetaData { get; }
        public COMImageFlags Flags { get; }
        public Int32 EntryPointToken { get; }
        public ImageDataDirectory Resources { get; }
        public ImageDataDirectory StrongNameSignature { get; }
        public ImageDataDirectory CodeManagerTable { get; }
        public ImageDataDirectory VTableFixups { get; }
        public ImageDataDirectory ExportAddressTableJumps { get; }
        public ImageDataDirectory ManagedNativeHeader { get; }
        internal static readonly UInt32 SizeOfStruct = 0x48;


        internal ImageCOR20Header(byte[] filebytes, UInt32 offset)
        {
            Size = BinaryHelper.ToInt32(filebytes, offset);
            MajorRuntimeVersion = BinaryHelper.ToInt16(filebytes, offset + 4);
            MinorRuntimeVersion = BinaryHelper.ToInt16(filebytes, offset + 6);
            MetaData = new ImageDataDirectory(filebytes, offset + 8);
            Flags = (COMImageFlags)BinaryHelper.ToUInt32(filebytes, offset + 16);
            EntryPointToken = BinaryHelper.ToInt32(filebytes, offset + 20);
            Resources = new ImageDataDirectory(filebytes, offset + 24);
            StrongNameSignature = new ImageDataDirectory(filebytes, offset + 32);
            CodeManagerTable = new ImageDataDirectory(filebytes, offset + 40);
            VTableFixups = new ImageDataDirectory(filebytes, offset + 48);
            ExportAddressTableJumps = new ImageDataDirectory(filebytes, offset + 56);
            ManagedNativeHeader = new ImageDataDirectory(filebytes, offset + 64);
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; MajorRuntimeVersion={1}; MinorRuntimeVersion={2}; MetaData={3}; Flags={4}; EntryPointToken={5}; Resources={6}; StrongNameSignature={7}; CodeManagerTable={8}; VTableFixups={9}; ExportAddressTableJumps={10}; ManagedNativeHeader={11}}}",
                Size,
                MajorRuntimeVersion,
                MinorRuntimeVersion,
                MetaData,
                Flags,
                EntryPointToken,
                Resources,
                StrongNameSignature,
                CodeManagerTable,
                VTableFixups,
                ExportAddressTableJumps,
                ManagedNativeHeader);
        }
    }
}
