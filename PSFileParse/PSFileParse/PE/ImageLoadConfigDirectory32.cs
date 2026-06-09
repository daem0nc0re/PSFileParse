using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct IMAGE_LOAD_CONFIG_DIRECTORY32
    // {
    //     int                              Size;
    //     int                              TimeDateStamp;
    //     short                            MajorVersion;
    //     short                            MinorVersion;
    //     int                              GlobalFlagsClear;
    //     int                              GlobalFlagsSet;
    //     int                              CriticalSectionDefaultTimeout;
    //     int                              DeCommitFreeBlockThreshold;
    //     int                              DeCommitTotalFreeThreshold;
    //     int                              LockPrefixTable;                // VA
    //     int                              MaximumAllocationSize;
    //     int                              VirtualMemoryThreshold;
    //     int                              ProcessHeapFlags;
    //     int                              ProcessAffinityMask;
    //     short                            CSDVersion;
    //     short                            DependentLoadFlags;
    //     int                              EditList;                       // VA
    //     int                              SecurityCookie;                 // VA
    //     int                              SEHandlerTable;                 // VA
    //     int                              SEHandlerCount;
    //     int                              GuardCFCheckFunctionPointer;    // VA
    //     int                              GuardCFDispatchFunctionPointer; // VA
    //     int                              GuardCFFunctionTable;           // VA
    //     int                              GuardCFFunctionCount;
    //     int                              GuardFlags;
    //     IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;
    //     int                              GuardAddressTakenIatEntryTable; // VA
    //     int                              GuardAddressTakenIatEntryCount;
    //     int                              GuardLongJumpTargetTable;       // VA
    //     int                              GuardLongJumpTargetCount;
    //     int                              DynamicValueRelocTable;         // VA
    //     int                              CHPEMetadataPointer;
    //     int                              GuardRFFailureRoutine;          // VA
    //     int                              GuardRFFailureRoutineFunctionPointer; // VA
    //     int                              DynamicValueRelocTableOffset;
    //     short                            DynamicValueRelocTableSection;
    //     short                            Reserved2;
    //     int                              GuardRFVerifyStackPointerFunctionPointer; // VA
    //     int                              HotPatchTableOffset;
    //     int                              Reserved3;
    //     int                              EnclaveConfigurationPointer;    // VA
    //     int                              VolatileMetadataPointer;        // VA
    //     int                              GuardEHContinuationTable;       // VA
    //     int                              GuardEHContinuationCount;
    //     int                              GuardXFGCheckFunctionPointer;   // VA
    //     int                              GuardXFGDispatchFunctionPointer; // VA
    //     int                              GuardXFGTableDispatchFunctionPointer; // VA
    //     int                              CastGuardOsDeterminedFailureMode; // VA
    //     int                              GuardMemcpyFunctionPointer;     // VA
    // };
    //
    public sealed class ImageLoadConfigDirectory32
    {
        public UInt32 Size { get; }
        public UnixTime TimeDateStamp { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public UInt32 GlobalFlagsClear { get; }
        public UInt32 GlobalFlagsSet { get; }
        public UInt32 CriticalSectionDefaultTimeout { get; }
        public UInt32 DeCommitFreeBlockThreshold { get; }
        public UInt32 DeCommitTotalFreeThreshold { get; }
        public UInt32 LockPrefixTable { get; }
        public UInt32 MaximumAllocationSize { get; }
        public UInt32 VirtualMemoryThreshold { get; }
        public HeapFlags ProcessHeapFlags { get; }
        public UInt32 ProcessAffinityMask { get; }
        public UInt16 CSDVersion { get; }
        public LoadLibraryFlags DependentLoadFlags { get; }
        public UInt32 EditList { get; }
        public UInt32 SecurityCookie { get; }
        public UInt32 SEHandlerTable { get; }
        public UInt32 SEHandlerCount { get; }
        // Following properties depend on "Size" property
        public Object /* UInt32 */ GuardCFCheckFunctionPointer { get; }
        public Object /* UInt32 */ GuardCFDispatchFunctionPointer { get; }
        public Object /* UInt32 */ GuardCFFunctionTable { get; }
        public Object /* UInt32 */ GuardCFFunctionCount { get; }
        public Object /* UInt32 */ GuardFlags { get; }
        public Object CodeIntegrity { get; }
        public Object /* UInt32 */ GuardAddressTakenIATEntryTable { get; }
        public Object /* UInt32 */ GuardAddressTakenIATEntryCount { get; }
        public Object /* UInt32 */ GuardLongJumpTargetTable { get; }
        public Object /* UInt32 */ GuardLongJumpTargetCount { get; }
        public Object /* UInt32 */ DynamicValueRelocTable { get; }
        public Object /* UInt32 */ CHPEMetadataPointer { get; }
        public Object /* UInt32 */ GuardRFFailureRoutine { get; }
        public Object /* UInt32 */ GuardRFFailureRoutineFunctionPointer { get; }
        public Object /* UInt32 */ DynamicValueRelocTableOffset { get; }
        public Object /* UInt16 */ DynamicValueRelocTableSection { get; }
        public Object /* UInt16 */ Reserved2 { get; }
        public Object /* UInt32 */ GuardRFVerifyStackPointerFunctionPointer { get; }
        public Object /* UInt32 */ HotPatchTableOffset { get; }
        public Object /* UInt32 */ Reserved3 { get; }
        public Object /* UInt32 */ EnclaveConfigurationPointer { get; }
        public Object /* UInt32 */ VolatileMetadataPointer { get; }
        public Object /* UInt32 */ GuardEHContinuationTable { get; }
        public Object /* UInt32 */ GuardEHContinuationCount { get; }
        public Object /* UInt32 */ GuardXFGCheckFunctionPointer { get; }
        public Object /* UInt32 */ GuardXFGDispatchFunctionPointer { get; }
        public Object /* UInt32 */ GuardXFGTableDispatchFunctionPointer { get; }
        public Object /* UInt32 */ CastGuardOSDeterminedFailureMode { get; }
        public Object /* UInt32 */ GuardMemcpyFunctionPointer { get; }


        internal ImageLoadConfigDirectory32(byte[] filebytes, UInt32 offset)
        {
            Size = BinaryHelper.ToUInt32(filebytes, offset);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, offset + 4));
            MajorVersion = BinaryHelper.ToUInt16(filebytes, offset + 8);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, offset + 10);
            GlobalFlagsClear = BinaryHelper.ToUInt32(filebytes, offset + 12);
            GlobalFlagsSet = BinaryHelper.ToUInt32(filebytes, offset + 16);
            CriticalSectionDefaultTimeout = BinaryHelper.ToUInt32(filebytes, offset + 20);
            DeCommitFreeBlockThreshold = BinaryHelper.ToUInt32(filebytes, offset + 24);
            DeCommitTotalFreeThreshold = BinaryHelper.ToUInt32(filebytes, offset + 28);
            LockPrefixTable = BinaryHelper.ToUInt32(filebytes, offset + 32);
            MaximumAllocationSize = BinaryHelper.ToUInt32(filebytes, offset + 36);
            VirtualMemoryThreshold = BinaryHelper.ToUInt32(filebytes, offset + 40);
            ProcessHeapFlags = (HeapFlags)BinaryHelper.ToUInt32(filebytes, offset + 44);
            ProcessAffinityMask = BinaryHelper.ToUInt32(filebytes, offset + 48);
            CSDVersion = BinaryHelper.ToUInt16(filebytes, offset + 52);
            DependentLoadFlags = (LoadLibraryFlags)BinaryHelper.ToUInt16(filebytes, offset + 54);
            EditList = BinaryHelper.ToUInt32(filebytes, offset + 56);
            SecurityCookie = BinaryHelper.ToUInt32(filebytes, offset + 60);
            SEHandlerTable = BinaryHelper.ToUInt32(filebytes, offset + 64);
            SEHandlerCount = BinaryHelper.ToUInt32(filebytes, offset + 68);

            if (Size >= 76)
                GuardCFCheckFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 72);

            if (Size >= 80)
                GuardCFDispatchFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 76);

            if (Size >= 84)
                GuardCFFunctionTable = BinaryHelper.ToUInt32(filebytes, offset + 80);

            if (Size >= 88)
                GuardCFFunctionCount = BinaryHelper.ToUInt32(filebytes, offset + 84);

            if (Size >= 92)
                GuardFlags = (GuardFlags)BinaryHelper.ToUInt32(filebytes, offset + 88);

            if (Size >= 104)
                CodeIntegrity = new ImageLoadConfigCodeIntegrity(filebytes, offset + 92);

            if (Size >= 108)
                GuardAddressTakenIATEntryTable = BinaryHelper.ToUInt32(filebytes, offset + 104);

            if (Size >= 112)
                GuardAddressTakenIATEntryCount = BinaryHelper.ToUInt32(filebytes, offset + 108);

            if (Size >= 116)
                GuardLongJumpTargetTable = BinaryHelper.ToUInt32(filebytes, offset + 112);

            if (Size >= 120)
                GuardLongJumpTargetCount = BinaryHelper.ToUInt32(filebytes, offset + 116);

            if (Size >= 124)
                DynamicValueRelocTable = BinaryHelper.ToUInt32(filebytes, offset + 120);

            if (Size >= 128)
                CHPEMetadataPointer = BinaryHelper.ToUInt32(filebytes, offset + 124);

            if (Size >= 132)
                GuardRFFailureRoutine = BinaryHelper.ToUInt32(filebytes, offset + 128);

            if (Size >= 136)
                GuardRFFailureRoutineFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 132);

            if (Size >= 140)
                DynamicValueRelocTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 136);

            if (Size >= 142)
                DynamicValueRelocTableSection = BinaryHelper.ToUInt16(filebytes, offset + 140);

            if (Size >= 144)
                Reserved2 = BinaryHelper.ToUInt16(filebytes, offset + 142);

            if (Size >= 148)
                GuardRFVerifyStackPointerFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 144);

            if (Size >= 152)
                HotPatchTableOffset = BinaryHelper.ToUInt32(filebytes, offset + 148);

            if (Size >= 156)
                Reserved3 = BinaryHelper.ToUInt32(filebytes, offset + 152);

            if (Size >= 160)
                EnclaveConfigurationPointer = BinaryHelper.ToUInt32(filebytes, offset + 156);

            if (Size >= 164)
                VolatileMetadataPointer = BinaryHelper.ToUInt32(filebytes, offset + 160);

            if (Size >= 168)
                GuardEHContinuationTable = BinaryHelper.ToUInt32(filebytes, offset + 164);

            if (Size >= 172)
                GuardEHContinuationCount = BinaryHelper.ToUInt32(filebytes, offset + 168);

            if (Size >= 176)
                GuardXFGCheckFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 172);

            if (Size >= 180)
                GuardXFGDispatchFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 176);

            if (Size >= 184)
                GuardXFGTableDispatchFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 180);

            if (Size >= 188)
                CastGuardOSDeterminedFailureMode = BinaryHelper.ToUInt32(filebytes, offset + 184);

            if (Size >= 192)
                GuardMemcpyFunctionPointer = BinaryHelper.ToUInt32(filebytes, offset + 188);
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
