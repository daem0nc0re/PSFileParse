using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class DyldChaindPointerEntry
    {
        public UInt32 Index { get; }
        public DyldChainedPointerType Type { get; }
        public String Segment { get; }
        public String Section { get; }
        public UInt64 Address { get; }
        public Object /* bool */ Auth { get; }
        public Object /* bool */ AddrDiv { get; }
        public Object /* bool */ KeyIsData { get; }
        public Object /* byte */ High8 { get; }
        public Object /* byte */ Key { get; }
        public Object /* byte */ CacheLevel { get; }
        public Object /* UInt32 */ Ordinal { get; }
        public Object /* UInt32 */ Addend { get; }
        public Object /* UInt16 */ Diversity { get; }
        public Object /* UInt16 */ Zero { get; }
        public UInt32 Next { get; }
        public Object /* UInt16 */ Unused { get; }
        public Object /* UInt32 */ Reserved { get; }
        public Object /* UInt64 */ RuntimeOffset { get; }
        public Object Target { get; }


        internal DyldChaindPointerEntry(
            byte[] filebytes,
            ref UInt64 offset,
            UInt32 index,
            DyldChainedPointerFormat format,
            UInt32 max_valid_pointer,
            MachOSection[] sections,
            DyldChainedImport[] imports,
            bool is_bigendian)
        {
            UInt64 value;
            UInt64 delta = 0;
            Index = index;
            Address = MachOFile.ImageBase + offset;

            foreach (var sec in sections)
            {
                if ((sec.Offset > 0) &&
                    (offset >= sec.Address - MachOFile.ImageBase) &&
                    (offset < sec.Address - MachOFile.ImageBase + sec.Size))
                {
                    Segment = sec.SegmentName;
                    Section = sec.SectionName;
                    delta = (sec.Address - MachOFile.ImageBase - sec.Offset);
                    break;
                }
            }

            if (format == DyldChainedPointerFormat.Pointer32)
            {
                // 
                // // DYLD_CHAINED_PTR_32
                // struct dyld_chained_ptr_32_rebase
                // {
                //     uint32_t    target    : 26,
                //                 next      :  5,
                //                 bind      :  1;
                // };
                // 
                // // DYLD_CHAINED_PTR_32
                // struct dyld_chained_ptr_32_bind
                // {
                //     uint32_t    ordinal   : 20,
                //                 addend    :  6,
                //                 next      :  5,
                //                 bind      :  1;
                // };
                // 
                if (is_bigendian)
                    value = BinaryHelper.ToUInt32Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt32(filebytes, offset - delta);

                Type = (DyldChainedPointerType)((value >> 31) & 1);
                Next = (UInt32)((value >> 26) & 0x1F);

                if (Type == DyldChainedPointerType.Rebase)
                {
                    var target = (UInt32)(value & 0x3FFFFFF);

                    if (target > max_valid_pointer)
                    {
                        var bias = (0x04000000 + max_valid_pointer) / 2;
                        target -= bias;
                    }

                    Target = MachOFile.ImageBase + target;
                }
                else if (Type == DyldChainedPointerType.Bind)
                {
                    Ordinal = (UInt32)(value & 0xFFFFF);
                    Addend = (UInt32)((value >> 20) & 0x3F);

                    if ((UInt32)Ordinal < imports.Length)
                    {
                        var imp = imports[(UInt32)Ordinal];
                        Target = new BindTarget(imp.DylibName, imp.Name);
                    }
                }

                offset += (Next * 4u);
            }
            else if (format == DyldChainedPointerFormat.Pointer32Cache)
            {
                // 
                // // DYLD_CHAINED_PTR_32_CACHE
                // struct dyld_chained_ptr_32_cache_rebase
                // {
                //     uint32_t    target    : 30,
                //                 next      :  2;
                // };
                // 
                if (is_bigendian)
                    value = BinaryHelper.ToUInt32Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt32(filebytes, offset - delta);

                Type = DyldChainedPointerType.Rebase;
                Target = MachOFile.ImageBase + (UInt64)(value & 0x3FFFFFFF);
                Next = (UInt32)((value >> 30) & 0x3);
                offset += (Next * 4u);
            }
            else if (format == DyldChainedPointerFormat.Pointer32Firmware)
            {
                // 
                // // DYLD_CHAINED_PTR_32_FIRMWARE
                // struct dyld_chained_ptr_32_firmware_rebase
                // {
                //     uint32_t    target   : 26,
                //                 next     :  6;
                // };
                // 
                if (is_bigendian)
                    value = BinaryHelper.ToUInt32Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt32(filebytes, offset - delta);

                Type = DyldChainedPointerType.Rebase;
                Target = MachOFile.ImageBase + (UInt64)(value & 0x3FFFFFF);
                Next = (UInt32)((value >> 26) & 0x3F);
                offset += (Next * 4u);
            }
            else if ((format == DyldChainedPointerFormat.Pointer64) ||
                (format == DyldChainedPointerFormat.Pointer64Offset))
            {
                // 
                // // DYLD_CHAINED_PTR_64/DYLD_CHAINED_PTR_64_OFFSET
                // struct dyld_chained_ptr_64_rebase
                // {
                //     uint64_t    target    : 36,
                //                 high8     :  8,
                //                 reserved  :  7,
                //                 next      : 12,
                //                 bind      :  1; // = 0
                // };
                // 
                // // DYLD_CHAINED_PTR_64
                // struct dyld_chained_ptr_64_bind
                // {
                //     uint64_t    ordinal   : 24,
                //                 addend    :  8,
                //                 reserved  : 19,
                //                 next      : 12,
                //                 bind      :  1; // = 1
                // };
                // 
                if (is_bigendian)
                    value = BinaryHelper.ToUInt64Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt64(filebytes, offset - delta);

                Type = (DyldChainedPointerType)((value >> 63) & 1);
                Next = (UInt32)((value >> 51) & 0xFFF);

                if (Type == DyldChainedPointerType.Rebase)
                {
                    Target = MachOFile.ImageBase + (UInt64)(value & 0xFFFFFFFFF);
                    High8 = (UInt32)((value >> 36) & 0xFF);
                    Reserved = (UInt32)((value >> 44) & 0x7F);
                }
                else if (Type == DyldChainedPointerType.Bind)
                {
                    Ordinal = (UInt32)(value & 0xFFFFFF);
                    Addend = (UInt32)((value >> 24) & 0xFF);
                    Reserved = (UInt32)((value >> 32) & 0x7FFFF);

                    if ((UInt32)Ordinal < imports.Length)
                    {
                        var imp = imports[(UInt32)Ordinal];
                        Target = new BindTarget(imp.DylibName, imp.Name);
                    }
                }

                offset += (Next * 4u);
            }
            else if ((format == DyldChainedPointerFormat.Pointer64KernelCache) ||
                (format == DyldChainedPointerFormat.PointerX64KernelCache))
            {
                // 
                // // DYLD_CHAINED_PTR_64_KERNEL_CACHE, DYLD_CHAINED_PTR_X86_64_KERNEL_CACHE
                // struct dyld_chained_ptr_64_kernel_cache_rebase
                // {
                //     uint64_t    target     : 30,
                //                 cacheLevel :  2,
                //                 diversity  : 16,
                //                 addrDiv    :  1,
                //                 key        :  2,
                //                 next       : 12,
                //                 isAuth     :  1;
                // };
                // 
                if (is_bigendian)
                    value = BinaryHelper.ToUInt64Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt64(filebytes, offset - delta);

                Type = DyldChainedPointerType.Rebase;
                Target = MachOFile.ImageBase + (UInt64)(value & 0x3FFFFFFF);
                CacheLevel = (byte)((value >> 30) & 0x3);
                Diversity = (UInt16)((value >> 32) & 0xFFFF);
                AddrDiv = ((value & 0x1000000000000) != 0);
                Key = (byte)((value >> 49) & 0x3);
                Next = (UInt32)((value >> 51) & 0xFFF);
                Auth = ((value & 0x8000000000000000) != 0);
                offset += (format == DyldChainedPointerFormat.Pointer64KernelCache) ? (Next * 4u) : Next;
            }
            else if ((format == DyldChainedPointerFormat.PointerARM64E) ||
                (format == DyldChainedPointerFormat.PointerARM64EKernel) ||
                (format == DyldChainedPointerFormat.PointerARM64EUserland) ||
                (format == DyldChainedPointerFormat.PointerARM64EFirmware) ||
                (format == DyldChainedPointerFormat.PointerARM64EUserland24))
            {
                bool is8stride = (format == DyldChainedPointerFormat.PointerARM64E) ||
                    (format == DyldChainedPointerFormat.PointerARM64EUserland) ||
                    (format == DyldChainedPointerFormat.PointerARM64EUserland24);

                if (is_bigendian)
                    value = BinaryHelper.ToUInt64Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt64(filebytes, offset - delta);

                Type = (DyldChainedPointerType)((value >> 62) & 3);
                Next = (UInt32)((value >> 51) & 0x7FF);

                if (Type == DyldChainedPointerType.Rebase)
                {
                    // 
                    // // DYLD_CHAINED_PTR_ARM64E
                    // struct dyld_chained_ptr_arm64e_rebase
                    // {
                    //     uint64_t    target   : 43,
                    //                 high8    :  8,
                    //                 next     : 11,
                    //                 bind     :  1, // = 0
                    //                 auth     :  1; // = 0
                    // };
                    // 
                    Target = MachOFile.ImageBase + (UInt64)(value & 0x7FFFFFFFFFF);
                    High8 = (UInt32)((value >> 43) & 0xFF);
                }
                else if (Type == DyldChainedPointerType.Bind)
                {
                    if (format == DyldChainedPointerFormat.PointerARM64EUserland24)
                    {
                        // 
                        // // DYLD_CHAINED_PTR_ARM64E_USERLAND24
                        // struct dyld_chained_ptr_arm64e_bind24
                        // {
                        //     uint64_t    ordinal   : 24,
                        //                 zero      :  8,
                        //                 addend    : 19,
                        //                 next      : 11,
                        //                 bind      :  1, // = 1
                        //                 auth      :  1; // = 0
                        // };
                        // 
                        Ordinal = (UInt32)(value & 0xFFFFFF);
                        Zero = (byte)((value >> 24) & 0xFF);
                        Addend = (UInt32)((value >> 32) & 0x7FFFF);
                    }
                    else
                    {
                        // 
                        // // DYLD_CHAINED_PTR_ARM64E
                        // struct dyld_chained_ptr_arm64e_bind
                        // {
                        //     uint64_t    ordinal   : 16,
                        //                 zero      : 16,
                        //                 addend    : 19,
                        //                 next      : 11,
                        //                 bind      :  1, // = 1
                        //                 auth      :  1; // = 0
                        // };
                        // 
                        Ordinal = (UInt32)(value & 0xFFFF);
                        Zero = (UInt32)((value >> 16) & 0xFFFF);
                        Addend = (UInt32)((value >> 32) & 0x7FFFF);
                    }

                    if ((UInt32)Ordinal < imports.Length)
                    {
                        var imp = imports[(UInt32)Ordinal];
                        Target = new BindTarget(imp.DylibName, imp.Name);
                    }
                }
                else if (Type == DyldChainedPointerType.AuthRebase)
                {
                    // 
                    // // DYLD_CHAINED_PTR_ARM64E
                    // struct dyld_chained_ptr_arm64e_auth_rebase
                    // {
                    //     uint64_t    target    : 32,
                    //                 diversity : 16,
                    //                 addrDiv   :  1,
                    //                 key       :  2,
                    //                 next      : 11,
                    //                 bind      :  1, // = 0
                    //                 auth      :  1; // = 1
                    // };
                    // 
                    Target = MachOFile.ImageBase + (UInt64)(value & 0xFFFFFFFF);
                    Diversity = (UInt16)((value >> 32) & 0xFFFF);
                    AddrDiv = ((value & 0x1000000000000) != 0);
                    Key = (byte)((value >> 49) & 0x3);
                }
                else if (Type == DyldChainedPointerType.AuthBind)
                {
                    if (format == DyldChainedPointerFormat.PointerARM64EUserland24)
                    {
                        // 
                        // // DYLD_CHAINED_PTR_ARM64E_USERLAND24
                        // struct dyld_chained_ptr_arm64e_auth_bind24
                        // {
                        //     uint64_t    ordinal   : 24,
                        //                 zero      :  8,
                        //                 diversity : 16,
                        //                 addrDiv   :  1,
                        //                 key       :  2,
                        //                 next      : 11,
                        //                 bind      :  1, // = 1
                        //                 auth      :  1; // = 1
                        // };
                        // 
                        Ordinal = (UInt32)(value & 0xFFFFFF);
                        Zero = (byte)((value >> 24) & 0xFF);
                        Diversity = (UInt16)((value >> 32) & 0xFFFF);
                        AddrDiv = ((value & 0x1000000000000) != 0);
                        Key = (byte)((value >> 49) & 0x3);
                    }
                    else
                    {
                        // 
                        // // DYLD_CHAINED_PTR_ARM64E
                        // struct dyld_chained_ptr_arm64e_auth_bind
                        // {
                        //     uint64_t    ordinal   : 16,
                        //                 zero      : 16,
                        //                 diversity : 16,
                        //                 addrDiv   :  1,
                        //                 key       :  2,
                        //                 next      : 11,
                        //                 bind      :  1, // = 1
                        //                 auth      :  1; // = 1
                        // };
                        // 
                        Ordinal = (UInt32)(value & 0xFFFF);
                        Diversity = (UInt16)((value >> 32) & 0xFFFF);
                        AddrDiv = ((value & 0x1000000000000) != 0);
                        Key = (byte)((value >> 49) & 0x3);
                        Zero = (UInt32)((value >> 16) & 0xFFFF);
                    }

                    if ((UInt32)Ordinal < imports.Length)
                    {
                        var imp = imports[(UInt32)Ordinal];
                        Target = new BindTarget(imp.DylibName, imp.Name);
                    }
                }

                offset += is8stride ? (Next * 8u) : (Next * 4u);
            }
            else if (format == DyldChainedPointerFormat.PointerARM64ESharedCache)
            {
                // 
                // // DYLD_CHAINED_PTR_ARM64E_SHARED_CACHE
                // struct dyld_chained_ptr_arm64e_shared_cache_auth_rebase
                // {
                //     uint64_t    runtimeOffset   : 34,
                //                 diversity       : 16,
                //                 addrDiv         :  1,
                //                 keyIsData       :  1,
                //                 next            : 11,
                //                 auth            :  1; // = 1
                // };
                // 
                // // DYLD_CHAINED_PTR_ARM64E_SHARED_CACHE
                // struct dyld_chained_ptr_arm64e_shared_cache_rebase
                // {
                //     uint64_t    runtimeOffset   : 34,
                //                 high8           :  8,
                //                 unused          : 10,
                //                 next            : 11,
                //                 auth            :  1; // = 0
                // };
                // 
                if (is_bigendian)
                    value = BinaryHelper.ToUInt64Big(filebytes, offset - delta);
                else
                    value = BinaryHelper.ToUInt64(filebytes, offset - delta);

                Auth = (value & 0x8000000000000000) != 0;
                RuntimeOffset = (UInt64)(value & 0x3FFFFFFFF);
                Next = (UInt32)((value >> 52) & 0x7FF);

                if ((bool)Auth)
                {
                    Type = DyldChainedPointerType.AuthRebase;
                    Diversity = (UInt16)((value >> 34) & 0xFFFF);
                    AddrDiv = ((value & 0x4000000000000) != 0);
                    KeyIsData = ((value & 0x8000000000000) != 0);
                }
                else
                {
                    Type = DyldChainedPointerType.Rebase;
                    High8 = (byte)((value >> 34) & 0xFF);
                    Unused = (UInt16)((value >> 42) & 0x3FF);
                }

                offset += (Next * 8u);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Type={0}; Segment={1}; Section={2}; Address={3}; Auth={4}; AddrDiv={5}; KeyIsData={6}; High8={7}; Key={8}; CacheLevel={9}; Ordinal={10}; Addend={11}; Diversity={12}; Zero={13}; Next={14}; Unused={15}; Reserved={16}; RuntimeOffset={17}; Target={18}}}",
                Type,
                Segment,
                Section,
                Address,
                Auth,
                AddrDiv,
                KeyIsData,
                High8,
                Key,
                CacheLevel,
                Ordinal,
                Addend,
                Diversity,
                Zero,
                Next,
                Unused,
                Reserved,
                RuntimeOffset,
                Target);
        }
    }
}
