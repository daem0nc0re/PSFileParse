using System;
using System.Diagnostics.Contracts;

namespace PSFileParse.MachO
{
    public enum ARMThreadFlavors : UInt32
    {
        ThreadState = 1,
        VFPState = 2,
        ExceptionState = 3,
        DebugState = 4,
        ThreadStateNone = 5,
        ThreadState64 = 6,
        ExceptionState64 = 7,
        Last = 8,
        SavedState32 = 9,
        SavedState64 = 10,
        NeonSavedState32 = 11,
        NeonSavedState64 = 12,
        VFPState64 = 13,
        DebugState32 = 14,
        DebugState64 = 15,
        NeonState64 = 17
    }


    public enum BindDylibOrdinal : Int16
    {
        Self = 0,
        MainExecutable = -1,
        FlatLookup = -2,
        WeakLookup = -3
    }


    public enum BindOpcode : byte
    {
        Done = 0x00,
        SetDylibOrdinalImm = 0x10,
        SetDylibOrdinalULEB = 0x20,
        SetDylibSpecialImm = 0x30,
        SetSymbolTrailingFlagsImm = 0x40,
        SetTypeImm = 0x50,
        SetAddendSLEB = 0x60,
        SetSegmentAndOffsetULEB = 0x70,
        AddAddrULEB = 0x80,
        DoBind = 0x90,
        DoBindAddAddrULEB = 0xA0,
        DoBindAddAddrImmScaled = 0xB0,
        DoBindULEBTimesSkippingULEB = 0xC0,
        Threaded = 0xD0,
        OpcodeMask = 0xF0,
        ImmediateMask = 0x0F
    }


    public enum BindSpecialDylib
    {
        Self = 0,
        MainExecutable = -1,
        FlatLookup = -2,
        WeakLookup = -3
    }


    public enum BindSymbolFlags : byte
    {
        None = 0x00,
        WeakImport = 0x01,
        NonWeakDefinition = 0x8
    }


    public enum BindType : byte
    {
        None = 0,
        Pointer = 1,
        TextAbsolute32 = 2,
        TextPCRel32 = 3
    }


    public enum  CSMagic : UInt32
    {
        Requirement = 0xFADE0C00,
        Requirements = 0xFADE0C01,
        CodeDirectory = 0xFADE0C02,
        EmbeddedSignature = 0xFADE0CC0,
        EmbeddedSignatureOld = 0xFADE0B02,
        EmbeddedEntitlements = 0xFADE7171,
        EmbeddedDEREntitlements = 0xfade7172,
        DetachedSignature = 0xFADE0CC1,
        BlobWrapper = 0xFADE0B01,
        EmbeddedLaunchConstraint = 0xFADE8181
    }


    public enum CSSlotType : UInt32
    {
        CodeDirectory = 0,
        InfoSlot = 1,
        Requirements = 2,
        ResourceDir = 3,
        Application = 4,
        Entitlements = 5,
        DEREntitlements = 7,
        LaunchConstraintSelf = 8,
        LaunchConstraintParent = 9,
        LaunchConstraintResponsible = 10,
        LibraryConstraint = 11,
        AlternateCodeDirectories = 0x1000,
        // CSSLOT_ALTERNATE_CODEDIRECTORY_MAX = 5, /* max number of alternate CD slots */
        SignatureSlot = 0x10000,
        IdentificationSlot = 0x10001,
        TicketSlot = 0x10002
    }


    public enum DataInCodeEntryKinds : UInt16
    {
        None = 0x00000000,
        Data = 0x00000001,
        JumpTable8 = 0x00000002,
        JumpTable16 = 0x00000003,
        JumpTable32 = 0x00000004,
        AbsoluteJumpTable32 = 0x00000005
    }


    public enum DyldChainedImportsFormat : UInt32
    {
        Import = 1,
        ImportAddend,
        ImportAddend64
    }


    public enum DyldChainedPointerFormat : UInt16
    {
        PointerARM64E = 1,
        Pointer64 = 2,
        Pointer32 = 3,
        Pointer32Cache = 4,
        Pointer32Firmware = 5,
        Pointer64Offset = 6,
        PointerARM64EOffset = 7,
        PointerARM64EKernel = 7,
        Pointer64KernelCache = 8,
        PointerARM64EUserland = 9,
        PointerARM64EFirmware = 10,
        PointerX64KernelCache = 11,
        PointerARM64EUserland24 = 12,
        PointerARM64ESharedCache = 13
    }


    public enum DyldChainedPointerType : byte
    {
        Rebase = 0,
        Bind,
        AuthRebase,
        AuthBind
    }


    public enum DyldChainedSymbolsFormat : UInt32
    {
        Uncompressed = 0,
        ZlibCompressed
    }


    public enum DylibReferenceTypes : byte
    {
        UndefinedNonLazy = 0x00000000,
        UndefinedLazy = 0x00000001,
        Defined = 0x00000002,
        PrivateDefined = 0x00000003,
        PrivateUndefinedNonLazy = 0x00000004,
        PrivateUndefinedLazy = 0x00000005,
    }


    [Flags]
    public enum ExportSymbolFlags : UInt64
    {
        Regular = 0x00000000,
        ThreadLocal = 0x00000001,
        Absolute = 0x00000002,
        Weak = 0x00000004,
        Reexport = 0x00000008,
        StubAndResolver = 0x00000010
    }


    public enum FatMagic : UInt32
    {
        LittleEndian = 0xCAFEBABE,
        BigEndian = 0xBEBAFECA
    }


    [Flags]
    public enum IndirectSymbolIndex : UInt32
    {
        Absolute = 0x40000000,
        Local = 0x80000000
    }


    [Flags]
    public enum MachCpuCapabilitiesArm64 : UInt32
    {
        None = 0x00000000, // User = 0x00000000,
        Kernel = 0x40000000,
        // KernelAbiMask = 0x40000000,
        PtrAuthVersion = 0x80000000
    }


    [Flags]
    public enum MachCpuCapabilitiesMask : UInt32
    {
        Arm64eKernelAbiMask = 0x40000000,
        Arm64eVersionedAbiMask = 0x80000000,
        Arm64PtrAuthMask = 0x0F000000,
        Arm64ePtrAuthMask = 0x3F000000
    }


    [Flags]
    public enum MachCpuCapabilitiesX64 : UInt32
    {
        None = 0x00000000,
        Lib64 = 0x80000000
    }


    public enum MachCpuSubTypeARM
    {
        ANY = -1,
        ALL = 0,
        V4T = 5,
        V6 = 6,
        V5TEJ = 7,
        XSCALE = 8,
        V7 = 9,
        V7F = 10,
        V7S = 11,
        V7K = 12,
        V8 = 13,
        V6M = 14,
        V7M = 15,
        V7EM = 16,
        V8M = 17,
        V8M_BASE = 18,
        V8_1M_MAIN = 19
    }


    public enum MachCpuSubTypeARM64
    {
        ANY = -1,
        ALL = 0,
        V8 = 1,
        ARM64E = 2
    }


    public enum MachCpuSubTypeARM64_32
    {
        ANY = -1,
        ALL = 0,
        V8 = 1
    }


    public enum MachCpuSubTypeHPPA
    {
        ANY = -1,
        ALL = 0,
        HPPA_7100LC = 1,
    }


    public enum MachCpuSubTypeI386
    {
        ANY = -1,
        I386 = 3,
        I486 = 4,
        I486SX = 132,
        PENT = 5,
        PENTPRO = 22,
        PENTII_M3 = 54,
        PENTII_M5 = 86,
        CELERON = 103,
        CELERON_MOBILE = 119,
        PENTIUM_3 = 8,
        PENTIUM_3_M = 24,
        PENTIUM_3_XEON = 40,
        PENTIUM_M = 9,
        PENTIUM_4 = 10,
        PENTIUM_4_M = 26,
        ITANIUM = 11,
        ITANIUM_2 = 27,
        XEON = 12,
        XEON_MP = 28,
        INTEL_FAMILY_MAX = 15,
        INTEL_MODEL_ALL = 0
    }


    public enum MachCpuSubTypeI860
    {
        ANY = -1,
        ALL = 0,
        I860_860 = 1
    }


    public enum MachCpuSubTypeMC680x0
    {
        ANY = -1,
        ALL = 1,
        MC68030 = 1,
        MC68040 = 2,
        MC68030_ONLY = 3
    }


    public enum MachCpuSubTypeMC88000
    {
        ANY = -1,
        ALL = 0,
        MC88100 = 1,
        MC88110 = 2
    }


    public enum MachCpuSubTypeMC98000
    {
        ANY = -1,
        ALL = 0,
        MC98601 = 1,
    }


    public enum MachCpuSubTypeMIPS
    {
        ANY = -1,
        ALL = 0,
        R2300 = 1,
        R2600 = 2,
        R2800 = 3,
        R2000a = 4,
        R2000 = 5,
        R3000a = 6,
        R3000 = 7
    }


    public enum MachCpuSubTypePOWERPC
    {
        ALL = 0,
        POWERPC_601 = 1,
        POWERPC_602 = 2,
        POWERPC_603 = 3,
        POWERPC_603e = 4,
        POWERPC_603ev = 5,
        POWERPC_604 = 6,
        POWERPC_604e = 7,
        POWERPC_620 = 8,
        POWERPC_750 = 9,
        POWERPC_7400 = 10,
        POWERPC_7450 = 11,
        POWERPC_970 = 100
    }


    public enum MachCpuSubTypeSPARC
    {
        ANY = -1,
        ALL = 0
    }


    public enum MachCpuSubTypeX86
    {
        ANY = -1,
        ALL = 3,
        ARCH1 = 4,
        H = 8
    }


    public enum MachCpuSubTypeVAX
    {
        ANY = -1,
        ALL = 0,
        VAX780 = 1,
        VAX785 = 2,
        VAX750 = 3,
        VAX730 = 4,
        UVAXI = 5,
        UVAXII = 6,
        VAX8200 = 7,
        VAX8500 = 8,
        VAX8600 = 9,
        VAX8650 = 10,
        VAX8800 = 11,
        UVAXIII = 12
    }


    public enum MachCpuType
    {
        ANY = -1,
        VAX = 1,
        MC680x0 = 6,
        X86 = 7,
        I386 = X86,
        X86_64 = 0x01000007,
        MC98000 = 10,
        HPPA = 11,
        ARM = 12,
        ARM64 = 0x0100000C,
        ARM64_32 = 0x0200000C,
        MC88000 = 13,
        SPARC = 14,
        I860 = 15,
        POWERPC = 18,
        POWERPC64 = 0x01000012
    }


    public enum MachOFileType
    {
        Object = 1,
        Executable = 2,
        FixedVMSharedLib = 3,
        Core = 4,
        PreloadedExecutable = 5,
        DynamicallyLinkedSharedLib = 6,
        DynamicLinker = 7,
        Bundle = 8,
        DynamicLibraryStub = 9,
        DSYMCompanion = 10,
        KextBundle = 11,
        FileSet = 12
    }


    [Flags]
    public enum MachOFlags : UInt32
    {
        None = 0x00000000,
        NoUndefinedReferences = 0x00000001,
        IncrementalLink = 0x00000002,
        DynamicLinker = 0x00000004,
        BindAtLoad = 0x00000008,
        Prebound = 0x00000010,
        SplitSegments = 0x00000020,
        LazyInit = 0x00000040,
        TwoLevel = 0x00000080,
        ForceFlat = 0x00000100,
        NoMultiDefinitions = 0x00000200,
        NoFixPrebinding = 0x00000400,
        Prebindable = 0x00000800,
        AllModulesBound = 0x00001000,
        SubsectionsViaSymbols = 0x00002000,
        Canonical = 0x00004000,
        WeakDefines = 0x00008000,
        BindsToWeak = 0x00010000,
        AllowStackExecution = 0x00020000,
        RootSafe = 0x00040000,
        SetUIDSafe = 0x00080000,
        NoReexportedDylibs = 0x00100000,
        PIE = 0x00200000,
        DeadStrippableDylib = 0x00400000,
        HasTLVDescriptors = 0x00800000,
        NoHeapExecution = 0x01000000,
        AppExtensionSafe = 0x02000000,
        NlistOutOfSyncWithDyldInfo = 0x04000000,
        SimulatorSupport = 0x08000000,
        DylibInCache = 0x80000000
    }


    public enum MachOLoadCommands : UInt32
    {
        Segment = 0x1,
        SymbolTable = 0x2,
        SymbolSegment = 0x3,
        Thread = 0x4,
        UnixThread = 0x5,
        LoadFixedVMSharedLib = 0x6,
        FixedVMSharedLibIdentification = 0x7,
        Identification = 0x8,
        FixedVMFile = 0x9,
        Prepage = 0xA,
        DynamicSymbolTable = 0xB,
        LoadDylib = 0xC,
        DylibIdentification = 0xD,
        LoadDynamicLinker = 0xE,
        DynamicLinkerIdentification = 0xF,
        PreboundDylib = 0x10,
        Routines = 0x11,
        SubFramework = 0x12,
        SubUmbrella = 0x13,
        SubClient = 0x14,
        SubLibrary = 0x15,
        TwoLevelHints = 0x16,
        PrebindChecksum = 0x17,
        LoadWeakDylib = 0x80000018,
        Segment64 = 0x19,
        Routines64 = 0x1A,
        UUID = 0x1B,
        RuntimePath = 0x8000001C,
        CodeSignature = 0x1D,
        SegmentSplitInfo = 0x1E,
        ReexportedDylib = 0x8000001F,
        LazyLoadDylib = 0x20,
        EncryptionInfo = 0x21,
        DyldInfo = 0x22,
        DyldInfoOnly = 0x80000022,
        LoadUpwardDylib = 0x80000023,
        VersionMinMacOSX = 0x24,
        VersionMinIPhoneOS = 0x25,
        FunctionStarts = 0x26,
        DyldEnvironment = 0x27,
        Main = 0x80000028,
        DataInCode = 0x29,
        SourceVersion = 0x2A,
        DylibCodeSignDRs = 0x2B,
        EncryptionInfo64 = 0x2C,
        LinkerOption = 0x2D,
        LinkerOptimizationHint = 0x2E,
        VersionMinTVOS = 0x2F,
        VersionMinWatchOS = 0x30,
        Note = 0x31,
        BuildVersion = 0x32,
        DyldExportsTrie = 0x80000033,
        DyldChainedFixups = 0x80000034,
        FileSetEntry = 0x80000035,
        AtomInfo = 0x36,
        FunctionVariants = 0x37,
        FunctionVariantFixups = 0x38,
        TargetTriple = 0x39,
        LazyLoadDylibInfo = 0x3A
    }


    public enum MachOMagic : UInt32
    {
        LittleEndian32Bit = 0xFEEDFACE,
        BigEndian32Bit = 0xCEFAEDFE,
        LittleEndian64Bit = 0xFEEDFACF,
        BigEndian64Bit = 0xCFFAEDFE
    }


    public enum MachOPlatform
    {
        MacOS = 1,
        IOS,
        TVOS,
        WatchOS,
        BridgeOS,
        MacCatalyst,
        IOSSimulator,
        TVOSSimulator,
        WatchOSSimulator,
        DriverKit
    }


    public enum MachOTools
    {
        Clang = 1,
        Swift,
        LD
    }


    public enum PPCThreadFlavors : UInt32
    {
        ThreadState = 1,
        FloatState = 2,
        ExceptionState = 3,
        VectorState = 4,
        ThreadState64 = 5,
        ExceptionState64 = 6,
        ThreadStateNone = 7
    }


    public enum RebaseOpcode : byte
    {
        Done = 0x00,
        SetTypeImm = 0x10,
        SetSegmentAndOffsetULEB = 0x20,
        AddAddrULEB = 0x30,
        AddAddrImmScaled = 0x40,
        DoRebaseImmTimes = 0x50,
        DoRebaseULEBTimes = 0x60,
        DoRebaseAddAddrULEB = 0x70,
        DoRebaseULEBTimesSkippingULEB = 0x80,
        ImmediateMask = 0x0F,
        OpcodeMask = 0xF0
    }


    public enum RebaseType : byte
    {
        None = 0,
        Pointer = 1,
        TextAbsolute32 = 2,
        TextPCRel32 = 3
    }


    public enum RelocLength
    {
        Byte = 0,
        Word,
        Long,
        Quad
    }


    public enum RelocTypeArm
    {
        Vanilla = 0,
        Pair,
        SectDiff,
        LocalSectDiff,
        PreboundLazyPointer,
        Branch24,
        ThumbRelocBranch22,
        Thumb32BitBranch,
        Half,
        HalfSectDiff
    }


    public enum RelocTypeArm64
    {
        Unsigned = 0,
        Subtractor,
        Branch26,
        Page21,
        PageOffset12,
        GOTLoadPage21,
        GOTLoadPageOffset12,
        PointerToGOT,
        TLVPLoadPage21,
        TLVPLoadPageOffset12,
        Addend
    }


    public enum RelocTypeGeneric
    {
        Vanilla = 0,
        Pair,
        SectDiff,
        PreboundLazyPointer,
        LocalSectDiff,
        ThreadLocalVariables
    }


    public enum RelocTypeX64
    {
        Unsigned = 0,
        Signed,
        Branch,
        GOTLoad,
        GOT,
        Subtractor,
        Signed1,
        Signed2,
        Signed4,
        ThreadLocalVariables
    }


    [Flags]
    public enum SectionAttributes : UInt32
    {
        None = 0x00000000,
        UserStable = 0xff000000,
        PureInstructions = 0x80000000,
        NoTableOfContents = 0x40000000,
        StripStaticSyms = 0x20000000,
        NoDeadStrip = 0x10000000,
        LiveSupport = 0x08000000,
        SelfModifyingCode = 0x04000000,
        Debug = 0x02000000,
        SystemStable = 0x00ffff00,
        SomeInstructions = 0x00000400,
        ExtReloc = 0x00000200,
        LocalReloc = 0x00000100
    }


    public enum SectionNumber : byte
    {
        NoSection = 0,
        MaxSection = 255
    }


    public enum SectionTypes
    {
        Regular = 0x0,
        ZeroFill = 0x1,
        LiteralsCString = 0x2,
        Literals4Bytes = 0x3,
        Literals8Bytes = 0x4,
        LiteralPointers = 0x5,
        NonLazySymbolPointers = 0x6,
        LazySymbolPointers = 0x7,
        SymbolStubs = 0x8,
        ModInitFuncPointers = 0x9,
        ModTermFuncPointers = 0xA,
        Coalesced = 0xB,
        GBZeroFill = 0xC,
        Interposing = 0xD,
        Literals16Bytes = 0xE,
        DTraceObjectFormat = 0xF,
        LazyDylibSymbolPointers = 0x10,
        ThreadLocalRegular = 0x11,
        ThreadLocalZeroFill = 0x12,
        ThreadLocalVariables = 0x13,
        ThreadLocalVariablePointers = 0x14,
        ThreadLocalInitFunctionPointers = 0x15,
        InitFuncOffsets = 0x16
    }


    [Flags]
    public enum SegmentFlags : UInt32
    {
        None = 0x00000000,
        HighVM = 0x00000001,
        FixedVMSharedLib = 0x00000002,
        NoReloc = 0x00000004,
        ProtectedVersion1 = 0x00000008,
        ReadOnly = 0x00000010
    }


    [Flags]
    public enum SymbolDescription : UInt16
    {
        None = 0x0000,
        ARMThumbDef = 0x0008,
        ReferencedDynamically = 0x0010,
        NoDeadStrip = 0x0020,
        WeakRef = 0x0040,
        WeakDef = 0x0080,
        SymbolResolver = 0x0100,
        AltEntry = 0x0200,
        ColdFunc = 0x0400
    }


    public enum SymbolNameType : byte
    {
        Undefined = 0,
        Absolute = 2,
        Indirect = 0xA,
        Prebound = 0xC,
        Section = 0xE
    }


    public enum SymbolOrdinal : byte
    {
        SelfLibrary = 0x00,
        MaxLibrary = 0xFD,
        DynamicLookup = 0xFE,
        Executable = 0xFF
    }


    [Flags]
    public enum VMProtectionFlags : UInt32
    {
        None = 0x00000000,
        Read = 0x00000001,
        Write = 0x00000002,
        Execute = 0x00000004,
        UsermodeExecute = 0x00000008,
        // Default = Read | Write,
        // All = Read | Write | Execute,
        NoChangeLegacy = 0x00000008,
        NoChange = 0x01000000,
        Copy = 0x00000010,
        Trusted = 0x00000020,
        IsMask = 0x00000040,
        StripRead = 0x00000080,
        // ExecuteOnly = Execute | StripRead,
        CopyFailIfExecutable = 0x00000100,
        TPRO = 0x00000200
    }


    public enum X86ThreadFlavors : UInt32
    {
        ThreadState32 = 1,
        FloatState32 = 2,
        ExceptionState32 = 3,
        ThreadState64 = 4,
        FloatState64 = 5,
        ExceptionState64 = 6,
        ThreadState = 7,
        FloatState = 8,
        ExceptionState = 9,
        DebugState32 = 10,
        DebugState64 = 11,
        DebugState = 12,
        ThreadStateNone = 13,
        AVXState32 = 16,
        AVXState64 = 17,
        AVXState = 18,
        AVX512State32 = 19,
        AVX512State64 = 20,
        AVX512State = 21
    }
}
