using System;

namespace PSFileParse.PE
{
    [Flags]
    public enum AssemblyFlags : UInt32
    {
        None = 0x0000,
        PublicKey = 0x0001,
        Retargetable = 0x0100,
        DisableJITcompileOptimizer = 0x4000,
        EnableJITcompileTracking = 0x8000
    }


    public enum AssemblyHashAlgorithm : UInt32
    {
        None = 0x0000,
        MD5 = 0x8003,
        SHA1 = 0x8004
    }


    public enum ClassInitialization : UInt32
    {
        None = 0x00000000,
        BeforeFieldInit = 0x00100000
    }


    [Flags]
    public enum ClassAdditionalFlags : UInt32
    {
        None = 0x00000000,
        RTSpecialName = 0x00000800,
        HasSecurity = 0x00040000,
        IsTypeForwarder = 0x00200000
    }


    [Flags]
    public enum ClassLayoutFlags : UInt32
    {
        Auto = 0x00000000,
        Sequential = 0x00000008,
        Explicit = 0x00000010,
        // LayoutMask = 0x00000018
    }


    public enum ClassSemantics : UInt32
    {
        Class = 0x00000000,
        Interface = 0x00000020,
        // SemanticMask = 0x00000020,
    }


    [Flags]
    public enum ClassSemanticsFlags : UInt32
    {
        None = 0x00000000,
        Abstract = 0x00000080,
        Sealed = 0x00000100,
        SpecialName = 0x00000400
    }


    [Flags]
    public enum CLRHeapSizeFlags : byte
    {
        None = 0x00,
        UseWideStringIndex = 0x01,
        UseWideGuidIndex = 0x02,
        UseWideBlobIndex = 0x04
    }


    [Flags]
    public enum COMImageFlags : UInt32
    {
        None = 0x00000000,
        ILOnly = 0x00000001,
        Required32Bit = 0x00000002,
        ILLibrary = 0x00000004,
        StrongNameSigned = 0x00000008,
        NativeEntryPoint = 0x00000010,
        TrackDebugData = 0x00010000,
        Preferred32Bit = 0x00020000
    }


    public enum DebugType
    {
        Unknown = 0,
        COFF = 1,
        CodeView = 2,
        FPO = 3,
        Misc = 4,
        Exception = 5,
        Fixup = 6,
        OmapToSrc = 7,
        OmapFromSrc = 8,
        Borland = 9,
        BBT = 10,
        CLSID = 11,
        VCFeature = 12,
        POGO = 13,
        ILTCG = 14,
        MPX = 15,
        Repro = 16,
        Reserved17 = 17,
        SPGO = 18,
        Reserved19 = 19,
        ExDllCharacteristics = 20
    }


    public enum ElementTypes : byte
    {
        End = 0x00,
        Void = 0x01,
        Boolean = 0x02,
        Char = 0x03,
        I1 = 0x04,
        U1 = 0x05,
        I2 = 0x06,
        U2 = 0x07,
        I4 = 0x08,
        U4 = 0x09,
        I8 = 0x0A,
        U8 = 0x0B,
        R4 = 0x0C,
        R8 = 0x0D,
        String = 0x0E,
        Ptr = 0x0F,
        ByRef = 0x10,
        ValueType = 0x11,
        Class = 0x12,
        Var = 0x13,
        Array = 0x14,
        GenericInst = 0x15,
        TypedByRef = 0x16,
        IntPtr = 0x18,
        UIntPtr = 0x19,
        FnPtr = 0x1B,
        Object = 0x1C,
        SzArray = 0x1D,
        MVar = 0x1E,
        CModReqd = 0x1F,
        CModOpt = 0x20,
        Internal = 0x21,
        Modifier = 0x40,
        Sentinel = 0x41,
        Pinned = 0x45,
        Type = 0x50,
        Boxed = 0x51,
        Reserved = 0x52,
        Field = 0x53,
        Property = 0x54,
        Enum = 0x55
    }


    [Flags]
    public enum EventAttributeFlags : UInt16
    {
        None = 0x0000,
        SpecialName = 0x0200,
        RTSpecialName = 0x0400
    }


    public enum FieldAccessAttributes : UInt16
    {
        CompilerControlled = 0x0000,
        Private = 0x0001,
        FamANDAssem = 0x0002,
        Assembly = 0x0003,
        Family = 0x0004,
        FamORAssem = 0x0005,
        Public = 0x0006,
        // FieldAccessMask = 0x0007,
    }


    [Flags]
    public enum FieldAdditionalFlags : UInt16
    {
        None = 0x0000,
        HasFieldRVA = 0x0100,
        RTSpecialName = 0x0400,
        HasFieldMarshal = 0x1000,
        HasDefault = 0x8000
    }


    [Flags]
    public enum FieldAttributeFlags : UInt16
    {
        None = 0x0000,
        Static = 0x0010,
        InitOnly = 0x0020,
        Literal = 0x0040,
        NotSerialized = 0x0080,
        SpecialName = 0x0200
    }


    [Flags]
    public enum FieldInteropAttributes : UInt16
    {
        None = 0x0000,
        PInvokeImpl = 0x2000
    }


    [Flags]
    public enum FileAttributes : UInt16
    {
        ContainsMetaData = 0x0000,
        ContainsNoMetaData = 0x0001
    }


    public enum FPOFrameType
    {
        FPO = 0,
        Trap,
        TSS,
        NonFPO
    }


    [Flags]
    public enum GenericParamConstraintFlags : UInt16
    {
        None = 0x0000,
        ReferenceTypeConstraint = 0x0004,
        NotNullableValueTypeConstraint = 0x0008,
        DefaultConstructorConstraint = 0x0010,
        // SpecialConstraintMask = 0x001C
    }


    public enum GenericParamVarianceAttributes : UInt16
    {
        None = 0x0000,
        Covariant = 0x0001,
        Contravariant = 0x0002,
        // VarianceMask = 0x0003,
    }


    [Flags]
    public enum GuardFlags : UInt32
    {
        None = 0x00000000,
        FIDSuppressed = 0x00000001,
        ExportSuppressed = 0x00000002,
        FIDLangExcptHandler = 0x00000004,
        FIDXFG = 0x00000008,
        CFInstrumented = 0x00000100,
        CFWInstrumented = 0x00000200,
        CFFunctionTablePresent = 0x00000400,
        SecurityCookieUnused = 0x00000800,
        ProtectDelayloadIAT = 0x00001000,
        DelayloadIatInItsOwnSection = 0x00002000,
        CFExportSuppressionInfoPresent = 0x00004000,
        CFEnableExportSuppression = 0x00008000,
        CFLongjumpTablePresent = 0x00010000,
        RFInstrumented = 0x00020000,
        RFEnable = 0x00040000,
        RFStrict = 0x00080000,
        RetpolinePresent = 0x00100000,
        EHContinuationTablePresent = 0x00400000,
        XfgEnabled = 0x00800000,
        CastguardPresent = 0x01000000,
        MemoryPresent = 0x02000000,
        CFFunctionTableSize1 = 0x10000000,
        CFFunctionTableSize2 = 0x20000000,
        CFFunctionTableSize4 = 0x40000000,
        CFFunctionTableSize8 = 0x80000000,
        CFFunctionTableSizeMask = 0xF0000000
    }


    [Flags]
    public enum HeapFlags : UInt32
    {
        None = 0x00000000,
        HeapNoSerialize = 0x00000001,
        HeapGenerateExceptions = 0x00000004,
        HeapCreateEnableExecute = 0x00040000
    }


    [Flags]
    public enum ImageCharacteristics : UInt16
    {
        None = 0x0000,
        Reserved0 = 0x0001,
        Reserved1 = 0x0002,
        Reserved2 = 0x0004,
        Reserved3 = 0x0008,
        HighEntropyVA = 0x0020,
        DynamicBase = 0x0040,
        ForceIntegrity = 0x0080,
        NXCompat = 0x0100,
        NoIsolation = 0x0200,
        NoSEH = 0x0400,
        NoBind = 0x0800,
        AppContainer = 0x1000,
        WDMDriver = 0x2000,
        GuardCF = 0x4000,
        TerminalServerAware = 0x8000
    }


    public enum ImageDebugMiscType
    {
        None = 0,
        ExeName = 1
    }


    [Flags]
    public enum ImageDllCharacteristicsEx : UInt32
    {
        None = 0x0000,
        CETCompat = 0x0001,
        CETCompatStrictMode = 0x0002,
        CETSetContextIPValidationRelaxedMode = 0x0004,
        CETDynamicApisAllowInProc = 0x0008,
        CETReserved1 = 0x0010,
        CETReserved2 = 0x0020,
        ForwardCFICompat = 0x0040,
        HotPatchCompatible = 0x0080
    }


    [Flags]
    public enum ImageFileCharacteristics : UInt16
    {
        None = 0x0000,
        RelocsStripped = 0x0001,
        ExecutableImage = 0x0002,
        LineNumsStripped = 0x0004,
        LocalSymsStripped = 0x0008,
        AggresiveWsTrim = 0x0010,
        LargeAddressAware = 0x0020,
        BytesReservedLo = 0x0080,
        Machine32Bit = 0x0100,
        DebugStripped = 0x0200,
        RemovableRunFromSwap = 0x0400,
        NetRunFromSwap = 0x0800,
        System = 0x1000,
        Dll = 0x2000,
        UpSystemOnly = 0x4000,
        BytesReservedHi = 0x8000
    }


    public enum ImageFileMachine : UInt16
    {
        UNKNOWN = 0,
        I386 = 0x014C,
        R3000BE = 0x0160,
        R3000 = 0x0162,
        R4000 = 0x0166,
        R10000 = 0x0168,
        WCEMIPSV2 = 0x0169,
        ALPHA = 0x0184,
        SH3 = 0x01A2,
        SH3DSP = 0x01A3,
        SH3E = 0x01A4,
        SH4 = 0x01A6,
        SH5 = 0x01A8,
        ARM = 0x01C0,
        THUMB = 0x01C2,
        ARMNT = 0x01C4,
        AM33 = 0x01D3,
        POWERPC = 0x01F0,
        POWERPCFP = 0x01F1,
        IA64 = 0x0200,
        MIPS16 = 0x0266,
        ALPHA64 = 0x0284,
        MIPSFPU = 0x0366,
        MIPSFPU16 = 0x0466,
        AXP64 = 0x0284,
        TRICORE = 0x0520,
        CEF = 0x0CEF,
        EBC = 0x0EBC,
        RISCV32 = 0x5032,
        RISCV64 = 0x5064,
        RISCV128 = 0x5128,
        LOONGARCH32 = 0x6232,
        LOONGARCH64 = 0x6264,
        AMD64 = 0x8664,
        M32R = 0x9041,
        ARM64EC = 0xA641,
        ARM64X = 0xA64E,
        ARM64 = 0xAA64,
        CEE = 0xC0EE
    }


    public enum ImageHeaderMagic : UInt16
    {
        NT32 = 0x10B,
        NT64 = 0x20B,
        ROM = 0x107
    }


    public enum ImageRelBasedType : UInt16
    {
        Absolute = 0,
        High = 1,
        Low = 2,
        HighLow = 3,
        HighAdj = 4,
        MachineSpecific5 = 5,
        MachineSpecific7 = 7,
        MachineSpecific8 = 8,
        MachineSpecific9 = 9,
        Dir64 = 10
    }


    public enum ImageRelBasedTypeArm : UInt16
    {
        Absolute = 0,
        High = 1,
        Low = 2,
        HighLow = 3,
        HighAdj = 4,
        ArmMov32 = 5,
        ThumbMov32 = 7,
        MachineSpecific8 = 8,
        MachineSpecific9 = 9,
        Dir64 = 10
    }


    public enum ImageRelBasedLoongArch32 : UInt16
    {
        Absolute = 0,
        High = 1,
        Low = 2,
        HighLow = 3,
        HighAdj = 4,
        MachineSpecific5 = 5,
        MachineSpecific7 = 7,
        LoongArchArch32MarkLA = 8,
        MachineSpecific9 = 9,
        Dir64 = 10
    }


    public enum ImageRelBasedLoongArch64 : UInt16
    {
        Absolute = 0,
        High = 1,
        Low = 2,
        HighLow = 3,
        HighAdj = 4,
        MachineSpecific5 = 5,
        MachineSpecific7 = 7,
        LoongArchArch64MarkLA = 8,
        MachineSpecific9 = 9,
        Dir64 = 10
    }


    public enum ImageRelBasedTypeMips : UInt16
    {
        Absolute = 0,
        High = 1,
        Low = 2,
        HighLow = 3,
        HighAdj = 4,
        MipsJmpAddr = 5,
        MachineSpecific7 = 7,
        MachineSpecific8 = 8,
        MipsJmpAddr19 = 9,
        Dir64 = 10
    }


    public enum ImageRelBasedTypeRISCV : UInt16
    {
        Absolute = 0,
        High = 1,
        Low = 2,
        HighLow = 3,
        HighAdj = 4,
        RISCVHigh20 = 5,
        RISCVLow12i = 7,
        RISCVLow12s = 8,
        MachineSpecific0 = 9,
        Dir64 = 10
    }


    public enum ImageSectionAlignment
    {
        Align1Byte = 0x00100000,
        Align2Bytes = 0x00200000,
        Align4Bytes = 0x00300000,
        Align8Bytes = 0x00400000,
        Align16Bytes = 0x00500000,
        Align32Bytes = 0x00600000,
        Align64Bytes = 0x00700000,
        Align128Bytes = 0x00800000,
        Align256Bytes = 0x00900000,
        Align512Bytes = 0x00A00000,
        Align1024Bytes = 0x00B00000,
        Align2048Bytes = 0x00C00000,
        Align4096Bytes = 0x00D00000,
        Align8192Bytes = 0x00E00000,
        AlignMask = 0x00F00000
    }


    public enum ImageSybsystemType : UInt16
    {
        Unknown = 0,
        Native = 1,
        WindowsGUI = 2,
        WindowsCUI = 3,
        OS2CUI = 5,
        PosixCUI = 7,
        WindowsCEGUI = 9,
        EFIApplication = 10,
        EFIBootServiceDriver = 11,
        EFIRuntimeDriver = 12,
        EFIRom = 13,
        Xbox = 14,
        WindowsBootApplication = 16
    }


    [Flags]
    public enum ImplementationAttributes : UInt32
    {
        None = 0x00000000,
        Import = 0x00001000,
        Serializable = 0x00002000
    }


    [Flags]
    public enum InteropFlags : UInt16
    {
        None = 0x0000,
        UnmanagedExport = 0x0008,
        PInvokeImpl = 0x2000
    }


    [Flags]
    public enum LoadLibraryFlags : UInt16
    {
        None = 0x0000,
        DontResolveDllReferences = 0x0001,
        LoadLibraryAsDatafile = 0x0002,
        LoadWithAlteredSearchPath = 0x0008,
        LoadIgnoreCodeAuthzLevel = 0x0010,
        LoadLibraryAsImageResource = 0x0020,
        LoadLibraryAsDatafileExclusive = 0x0040,
        LoadLibraryRequireSignedTarget = 0x0080,
        LoadLibrarySearchDllLoadDir = 0x0100,
        LoadLibrarySearchApplicationDir = 0x0200,
        LoadLibrarySearchUserDirs = 0x0400,
        LoadLibrarySearchSystem32 = 0x0800,
        LoadLibrarySearchDefaultDirs = 0x1000,
        LoadLibrarySafeCurrentDirs = 0x2000
    }


    public enum ManifestResourceAttributes : UInt16
    {
        None = 0x0000,
        Public = 0x0001,
        Private = 0x0002,
        // VisibilityMask = 0x0007
    }


    public enum MemberAccess : UInt16
    {
        CompilerControlled = 0x0000,
        Private = 0x0001,
        FamANDAssem = 0x0002,
        Assembly = 0x0003,
        Family = 0x0004,
        FamORAssem = 0x0005,
        Public = 0x0006,
        // MemberAccessMask = 0x0007,
    }


    [Flags]
    public enum MetadataTableFlags : UInt64
    {
        Module = 0x0000000000000001UL,
        TypeRef = 0x0000000000000002UL,
        TypeDef = 0x0000000000000004UL,
        Field = 0x0000000000000010UL,
        MethodDef = 0x0000000000000040UL,
        Param = 0x0000000000000100UL,
        InterfaceImpl = 0x0000000000000200UL,
        MemberRef = 0x0000000000000400UL,
        Constant = 0x0000000000000800UL,
        CustomAttribute = 0x0000000000001000UL,
        FieldMarshal = 0x0000000000002000UL,
        DeclSecurity = 0x0000000000004000UL,
        ClassLayout = 0x0000000000008000UL,
        FieldLayout = 0x0000000000010000UL,
        StandAloneSig = 0x0000000000020000UL,
        EventMap = 0x0000000000040000UL,
        Event = 0x0000000000100000UL,
        PropertyMap = 0x0000000000200000UL,
        Property = 0x0000000000800000UL,
        MethodSemantics = 0x0000000001000000UL,
        MethodImpl = 0x0000000002000000UL,
        ModuleRef = 0x0000000004000000UL,
        TypeSpec = 0x0000000008000000UL,
        ImplMap = 0x0000000010000000UL,
        FieldRVA = 0x0000000020000000UL,
        Assembly = 0x0000000100000000UL,
        AssemblyProcessor = 0x0000000200000000UL,
        AssemblyOS = 0x0000000400000000UL,
        AssemblyRef = 0x0000000800000000UL,
        AssemblyRefProcessor = 0x0000001000000000UL,
        AssemblyRefOS = 0x0000002000000000UL,
        File = 0x0000004000000000UL,
        ExportedType = 0x0000008000000000UL,
        ManifestResource = 0x0000010000000000UL,
        NestedClass = 0x0000020000000000UL,
        GenericParam = 0x0000040000000000UL,
        MethodSpec = 0x0000080000000000UL,
        GenericParamConstraint = 0x0000100000000000UL
    }


    public enum MetadataTableIdentifier
    {
        Module = 0x00,
        TypeRef = 0x01,
        TypeDef = 0x02,
        Field = 0x04,
        MethodDef = 0x06,
        Param = 0x08,
        InterfaceImpl = 0x09,
        MemberRef = 0x0A,
        Constant = 0x0B,
        CustomAttribute = 0x0C,
        FieldMarshal = 0x0D,
        DeclSecurity = 0x0E,
        ClassLayout = 0x0F,
        FieldLayout = 0x10,
        StandAloneSig = 0x11,
        EventMap = 0x12,
        Event = 0x14,
        PropertyMap = 0x15,
        Property = 0x17,
        MethodSemantics = 0x18,
        MethodImpl = 0x19,
        ModuleRef = 0x1A,
        TypeSpec = 0x1B,
        ImplMap = 0x1C,
        FieldRVA = 0x1D,
        Assembly = 0x20,
        AssemblyProcessor = 0x21,
        AssemblyOS = 0x22,
        AssemblyRef = 0x23,
        AssemblyRefProcessor = 0x24,
        AssemblyRefOS = 0x25,
        File = 0x26,
        ExportedType = 0x27,
        ManifestResource = 0x28,
        NestedClass = 0x29,
        GenericParam = 0x2A,
        MethodSpec = 0x2B,
        GenericParamConstraint = 0x2C
    }


    [Flags]
    public enum MethodAdditionalFlags : UInt16
    {
        None = 0x0000,
        RTSpecialName = 0x1000,
        HasSecurity = 0x4000,
        RequireSecObject = 0x8000
    }


    [Flags]
    public enum MethodAttributeFlags : UInt16
    {
        None = 0x0000,
        Static = 0x0010,
        Final = 0x0020,
        Virtual = 0x0040,
        HideBySig = 0x0080
    }


    public enum MethodCodeTypes : UInt16
    {
        IL = 0x0000,
        Native = 0x0001,
        OPTIL = 0x0002,
        Runtime = 0x0003
        // CodeTypeMask = 0x0003,
    }


    [Flags]
    public enum MethodImplementationFlags : UInt16
    {
        None = 0x0000,
        NoInlining = 0x0008,
        ForwardRef = 0x0010,
        Synchronized = 0x0020,
        NoOptimization = 0x0040,
        PreserveSig = 0x0080,
        InternalCall = 0x1000
        // MaxMethodImplVal = 0xFFFF,
    }


    public enum MethodManageAttributes : UInt16
    {
        Managed = 0x0000,
        Unmanaged = 0x0004
        // ManagedMask = 0x0004
    }


    [Flags]
    public enum MethodModifierFlags : UInt16
    {
        None = 0x0000,
        Strict = 0x0200,
        Abstract = 0x0400,
        SpecialName = 0x0800
    }


    [Flags]
    public enum MethodSemanticsFlags : UInt16
    {
        None = 0x0000,
        Setter = 0x0001,
        Getter = 0x0002,
        Other = 0x0004,
        AddOn = 0x0008,
        RemoveOn = 0x0010,
        Fire = 0x0020
    }


    [Flags]
    public enum ParamAttributes : UInt16
    {
        None = 0x0000,
        In = 0x0001,
        Out = 0x0002,
        Optional = 0x0010,
        HasDefault = 0x1000,
        HasFieldMarshal = 0x2000,
        Unused = 0xCFE0
    }


    [Flags]
    public enum PInvokeCallConvFlags : UInt16
    {
        None = 0x0000,
        SupportsLastError = 0x0040,
        CallConvPlatformapi = 0x0100,
        CallConvCdecl = 0x0200,
        CallConvStdcall = 0x0300,
        CallConvThiscall = 0x0400,
        CallConvFastcall = 0x0500,
        // CallConvMask = 0x0700
    }


    public enum PInvokeCharSet : UInt16
    {
        CharSetNotSpec = 0x0000,
        CharSetAnsi = 0x0002,
        CharSetUnicode = 0x0004,
        CharSetAuto = 0x0006,
        // CharSetMask = 0x0006,
    }


    public enum PInvokeLastErrorSupportFlags : UInt16
    {
        None = 0x0000,
        SupportsLastError = 0x0040
    }


    [Flags]
    public enum PInvokeNameFlags : UInt16
    {
        None = 0x0000,
        NoMangle = 0x0001
    }


    [Flags]
    public enum PropertyAttributeFlags : UInt16
    {
        None = 0x0000,
        SpecialName = 0x0200,
        RTSpecialName = 0x0400,
        HasDefault = 0x1000,
        Unused = 0xE9FF
    }


    public enum ResourceType
    {
        Cursor = 1,
        Bitmap = 2,
        Icon = 3,
        Menu = 4,
        Dialog = 5,
        String = 6,
        FondDir = 7,
        Font = 8,
        Accelerator = 9,
        ResourceData = 10,
        MessageTable = 11,
        GroupCursor = 12,
        GroupIcon = 14,
        Version = 16,
        DLGInclude = 17,
        PlugPlay = 19,
        VXD = 20,
        AnimationCursor = 21,
        AnimationIcon = 22,
        HTML = 23,
        Manifest = 24
    }


    [Flags]
    public enum SectionCharacteristics : UInt32
    {
        None = 0x00000000,
        NoPad = 0x00000008,
        CntCode = 0x00000020,
        CntInitializedData = 0x00000040,
        CntUninitializedData = 0x00000080,
        LnkInfo = 0x00000200,
        LnkRemove = 0x00000800,
        LnkComdat = 0x00001000,
        NoDeferSpecExc = 0x00004000,
        GPRel = 0x00008000,
        MemFarData = 0x00008000,
        MemPurgeable = 0x00020000,
        Mem16Bit = 0x00020000,
        MemLocked = 0x00040000,
        MemPreload = 0x00080000,
        Align1Byte = 0x00100000,
        Align2Bytes = 0x00200000,
        Align4Bytes = 0x00300000,
        Align8Bytes = 0x00400000,
        Align16Bytes = 0x00500000,
        Align32Bytes = 0x00600000,
        Align64Bytes = 0x00700000,
        Align128Bytes = 0x00800000,
        Align256Bytes = 0x00900000,
        Align512Bytes = 0x00A00000,
        Align1024Bytes = 0x00B00000,
        Align2048Bytes = 0x00C00000,
        Align4096Bytes = 0x00D00000,
        Align8192Bytes = 0x00E00000,
        AlignMask = 0x00F00000,
        LnkNRelocOVFL = 0x01000000,
        MemDiscardable = 0x02000000,
        MemNotCached = 0x04000000,
        MemNotPaged = 0x08000000,
        MemShared = 0x10000000,
        MemExecute = 0x20000000,
        MemRead = 0x40000000,
        MemWrite = 0x80000000
    }


    public enum StringFormattingFlags : UInt32
    {
        ANSI = 0x00000000,
        Unicode = 0x00010000,
        Auto = 0x00020000,
        // StringFormatMask = 0x00030000,
        Custom = 0x00030000,
        CustomStringFormatMask = 0x00C00000,
    }


    public enum VisibilityAttributes : UInt32
    {
        NotPublic = 0x00000000,
        Public = 0x00000001,
        NestedPublic = 0x00000002,
        NestedPrivate = 0x00000003,
        NestedFamily = 0x00000004,
        NestedAssembly = 0x00000005,
        NestedFamANDAssem = 0x00000006,
        NestedFamORAssem = 0x00000007
        // VisibilityMask = 0x00000007
    }


    [Flags]
    public enum VtableLayout : UInt16
    {
        ReuseSlot = 0x0000,
        NewSlot = 0x0100,
        // VtableLayoutMask = 0x0100
    }


    public enum WinCertRevision : UInt16
    {
        Revision_1_0 = 0x0100,
        Revision_2_0 = 0x0200,
    }


    public enum WinCertType : UInt16
    {
        X509 = 1,
        PKCS7,
        Reserved,
        TerminalServerProtocolStack
    }
}
