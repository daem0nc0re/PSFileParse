using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_LOAD_CONFIG_DIRECTORY64 {
    //     int                              Size;
    //     int                              TimeDateStamp;
    //     short                            MajorVersion;
    //     short                            MinorVersion;
    //     int                              GlobalFlagsClear;
    //     int                              GlobalFlagsSet;
    //     int                              CriticalSectionDefaultTimeout;
    //     unsigned long long               DeCommitFreeBlockThreshold;
    //     unsigned long long               DeCommitTotalFreeThreshold;
    //     unsigned long long               LockPrefixTable;                // VA
    //     unsigned long long               MaximumAllocationSize;
    //     unsigned long long               VirtualMemoryThreshold;
    //     unsigned long long               ProcessAffinityMask;
    //     int                              ProcessHeapFlags;
    //     short                            CSDVersion;
    //     short                            DependentLoadFlags;
    //     unsigned long long               EditList;                       // VA
    //     unsigned long long               SecurityCookie;                 // VA
    //     unsigned long long               SEHandlerTable;                 // VA
    //     unsigned long long               SEHandlerCount;
    //     unsigned long long               GuardCFCheckFunctionPointer;    // VA
    //     unsigned long long               GuardCFDispatchFunctionPointer; // VA
    //     unsigned long long               GuardCFFunctionTable;           // VA
    //     unsigned long long               GuardCFFunctionCount;
    //     int                              GuardFlags;
    //     IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;
    //     unsigned long long               GuardAddressTakenIatEntryTable; // VA
    //     unsigned long long               GuardAddressTakenIatEntryCount;
    //     unsigned long long               GuardLongJumpTargetTable;       // VA
    //     unsigned long long               GuardLongJumpTargetCount;
    //     unsigned long long               DynamicValueRelocTable;         // VA
    //     unsigned long long               CHPEMetadataPointer;            // VA
    //     unsigned long long               GuardRFFailureRoutine;          // VA
    //     unsigned long long               GuardRFFailureRoutineFunctionPointer; // VA
    //     int                              DynamicValueRelocTableOffset;
    //     short                            DynamicValueRelocTableSection;
    //     short                            Reserved2;
    //     unsigned long long               GuardRFVerifyStackPointerFunctionPointer; // VA
    //     int                              HotPatchTableOffset;
    //     int                              Reserved3;
    //     unsigned long long               EnclaveConfigurationPointer;    // VA
    //     unsigned long long               VolatileMetadataPointer;        // VA
    //     unsigned long long               GuardEHContinuationTable;       // VA
    //     unsigned long long               GuardEHContinuationCount;
    //     unsigned long long               GuardXFGCheckFunctionPointer;   // VA
    //     unsigned long long               GuardXFGDispatchFunctionPointer; // VA
    //     unsigned long long               GuardXFGTableDispatchFunctionPointer; // VA
    //     unsigned long long               CastGuardOsDeterminedFailureMode; // VA
    //     unsigned long long               GuardMemcpyFunctionPointer;     // VA
    // };
    //
    public sealed class ImageLoadConfigDirectory64
    {
        public UInt32 Size { get; }
        public UnixTime TimeDateStamp { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public UInt32 GlobalFlagsClear { get; }
        public UInt32 GlobalFlagsSet { get; }
        public UInt32 CriticalSectionDefaultTimeout { get; }
        public UInt64 DeCommitFreeBlockThreshold { get; }
        public UInt64 DeCommitTotalFreeThreshold { get; }
        public UInt64 LockPrefixTable { get; }
        public UInt64 MaximumAllocationSize { get; }
        public UInt64 VirtualMemoryThreshold { get; }
        public UInt64 ProcessAffinityMask { get; }
        public HeapFlags ProcessHeapFlags { get; }
        public UInt16 CSDVersion { get; }
        public LoadLibraryFlags DependentLoadFlags { get; }
        public UInt64 EditList { get; }
        public UInt64 SecurityCookie { get; }
        public UInt64 SEHandlerTable { get; }
        public UInt64 SEHandlerCount { get; }
        // Following properties depend on "Size" property
        public Object /* UInt64 */ GuardCFCheckFunctionPointer { get; }
        public Object /* UInt64 */ GuardCFDispatchFunctionPointer { get; }
        public Object /* UInt64 */ GuardCFFunctionTable { get; }
        public Object /* UInt64 */ GuardCFFunctionCount { get; }
        public Object /* UInt32 */ GuardFlags { get; }
        public Object CodeIntegrity { get; }
        public Object /* UInt64 */ GuardAddressTakenIATEntryTable { get; }
        public Object /* UInt64 */ GuardAddressTakenIATEntryCount { get; }
        public Object /* UInt64 */ GuardLongJumpTargetTable { get; }
        public Object /* UInt64 */ GuardLongJumpTargetCount { get; }
        public Object /* UInt64 */ DynamicValueRelocTable { get; }
        public Object /* UInt64 */ CHPEMetadataPointer { get; }
        public Object /* UInt64 */ GuardRFFailureRoutine { get; }
        public Object /* UInt64 */ GuardRFFailureRoutineFunctionPointer { get; }
        public Object /* UInt32 */ DynamicValueRelocTableOffset { get; }
        public Object /* UInt16 */ DynamicValueRelocTableSection { get; }
        public Object /* UInt16 */ Reserved2 { get; }
        public Object /* UInt64 */ GuardRFVerifyStackPointerFunctionPointer { get; }
        public Object /* UInt32 */ HotPatchTableOffset { get; }
        public Object /* UInt32 */ Reserved3 { get; }
        public Object /* UInt64 */ EnclaveConfigurationPointer { get; }
        public Object /* UInt64 */ VolatileMetadataPointer { get; }
        public Object /* UInt64 */ GuardEHContinuationTable { get; }
        public Object /* UInt64 */ GuardEHContinuationCount { get; }
        public Object /* UInt64 */ GuardXFGCheckFunctionPointer { get; }
        public Object /* UInt64 */ GuardXFGDispatchFunctionPointer { get; }
        public Object /* UInt64 */ GuardXFGTableDispatchFunctionPointer { get; }
        public Object /* UInt64 */ CastGuardOSDeterminedFailureMode { get; }
        public Object /* UInt64 */ GuardMemcpyFunctionPointer { get; }


        internal ImageLoadConfigDirectory64(byte[] filebytes, UInt32 offset)
        {
            Size = BinaryHelper.ToUInt32(filebytes, offset);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, offset + 4));
            MajorVersion = BinaryHelper.ToUInt16(filebytes, offset + 8);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, offset + 10);
            GlobalFlagsClear = BinaryHelper.ToUInt32(filebytes, offset + 12);
            GlobalFlagsSet = BinaryHelper.ToUInt32(filebytes, offset + 16);
            CriticalSectionDefaultTimeout = BinaryHelper.ToUInt32(filebytes, offset + 20);
            DeCommitFreeBlockThreshold = BinaryHelper.ToUInt64(filebytes, offset + 24);
            DeCommitTotalFreeThreshold = BinaryHelper.ToUInt64(filebytes, offset + 32);
            LockPrefixTable = BinaryHelper.ToUInt64(filebytes, offset + 40);
            MaximumAllocationSize = BinaryHelper.ToUInt64(filebytes, offset + 48);
            VirtualMemoryThreshold = BinaryHelper.ToUInt64(filebytes, offset + 56);
            ProcessAffinityMask = BinaryHelper.ToUInt64(filebytes, offset + 64);
            ProcessHeapFlags = (HeapFlags)BinaryHelper.ToUInt32(filebytes, offset + 72);
            CSDVersion = BinaryHelper.ToUInt16(filebytes, offset + 76);
            DependentLoadFlags = (LoadLibraryFlags)BinaryHelper.ToUInt16(filebytes, offset + 78);
            EditList = BinaryHelper.ToUInt64(filebytes, offset + 80);
            SecurityCookie = BinaryHelper.ToUInt64(filebytes, offset + 88);
            SEHandlerTable = BinaryHelper.ToUInt64(filebytes, offset + 96);
            SEHandlerCount = BinaryHelper.ToUInt64(filebytes, offset + 104);

            if (Size >= 120)
                GuardCFCheckFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 112);

            if (Size >= 128)
                GuardCFDispatchFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 120);

            if (Size >= 136)
                GuardCFFunctionTable = BinaryHelper.ToUInt64(filebytes, offset + 128);

            if (Size >= 144)
                GuardCFFunctionCount = BinaryHelper.ToUInt64(filebytes, offset + 136);

            if (Size >= 148)
                GuardFlags = (GuardFlags)BinaryHelper.ToUInt32(filebytes, offset + 144);

            if (Size >= 160)
                CodeIntegrity = new ImageLoadConfigCodeIntegrity(filebytes, offset + 148);

            if (Size >= 168)
                GuardAddressTakenIATEntryTable = BinaryHelper.ToUInt64(filebytes, offset + 160);

            if (Size >= 176)
                GuardAddressTakenIATEntryCount = BinaryHelper.ToUInt64(filebytes, offset + 168);

            if (Size >= 184)
                GuardLongJumpTargetTable = BinaryHelper.ToUInt64(filebytes, offset + 176);

            if (Size >= 192)
                GuardLongJumpTargetCount = BinaryHelper.ToUInt64(filebytes, offset + 184);

            if (Size >= 200)
                DynamicValueRelocTable = BinaryHelper.ToUInt64(filebytes, offset + 192);

            if (Size >= 208)
                CHPEMetadataPointer = BinaryHelper.ToUInt64(filebytes, offset + 200);

            if (Size >= 216)
                GuardRFFailureRoutine = BinaryHelper.ToUInt64(filebytes, offset + 208);

            if (Size >= 224)
                GuardRFFailureRoutineFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 216);

            if (Size >= 228)
                DynamicValueRelocTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 224);

            if (Size >= 230)
                DynamicValueRelocTableSection = BinaryHelper.ToUInt16(filebytes, offset + 228);

            if (Size >= 232)
                Reserved2 = BinaryHelper.ToUInt16(filebytes, offset + 230);

            if (Size >= 240)
                GuardRFVerifyStackPointerFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 232);

            if (Size >= 244)
                HotPatchTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 240);

            if (Size >= 248)
                Reserved3 = BinaryHelper.ToUInt32(filebytes, offset + 244);

            if (Size >= 256)
                EnclaveConfigurationPointer = BinaryHelper.ToUInt64(filebytes, offset + 248);

            if (Size >= 264)
                VolatileMetadataPointer = BinaryHelper.ToUInt64(filebytes, offset + 256);

            if (Size >= 272)
                GuardEHContinuationTable = BinaryHelper.ToUInt64(filebytes, offset + 264);

            if (Size >= 280)
                GuardEHContinuationCount = BinaryHelper.ToUInt64(filebytes, offset + 272);

            if (Size >= 288)
                GuardXFGCheckFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 280);

            if (Size >= 296)
                GuardXFGDispatchFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 288);

            if (Size >= 304)
                GuardXFGTableDispatchFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 296);

            if (Size >= 312)
                CastGuardOSDeterminedFailureMode = BinaryHelper.ToUInt64(filebytes, offset + 304);

            if (Size >= 320)
                GuardMemcpyFunctionPointer = BinaryHelper.ToUInt64(filebytes, offset + 312);
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; TimeDateStamp={1}; MajorVersion={2}; MinorVersion={3}; GlobalFlagsClear={4}; GlobalFlagsSet={5};...}}",
                Size,
                TimeDateStamp,
                MajorVersion,
                MinorVersion,
                GlobalFlagsClear,
                GlobalFlagsSet);
        }
    }
}
