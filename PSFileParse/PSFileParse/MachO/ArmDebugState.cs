using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    //
    // #if defined(__arm__)
    // #define _STRUCT_ARM_DEBUG_STATE    struct __darwin_arm_debug_state
    // _STRUCT_ARM_DEBUG_STATE
    // {
    //     __uint32_t        __bvr[16];
    //     __uint32_t        __bcr[16];
    //     __uint32_t        __wvr[16];
    //     __uint32_t        __wcr[16];
    // };
    // 
    // #elif defined(__arm64__)
    // #define _STRUCT_ARM_LEGACY_DEBUG_STATE    struct arm_legacy_debug_state
    // _STRUCT_ARM_LEGACY_DEBUG_STATE
    // {
    //     __uint32_t        __bvr[16];
    //     __uint32_t        __bcr[16];
    //     __uint32_t        __wvr[16];
    //     __uint32_t        __wcr[16];
    // };
    // 
    // #define _STRUCT_ARM_DEBUG_STATE32    struct __darwin_arm_debug_state32
    // _STRUCT_ARM_DEBUG_STATE32
    // {
    //     __uint32_t        __bvr[16];
    //     __uint32_t        __bcr[16];
    //     __uint32_t        __wvr[16];
    //     __uint32_t        __wcr[16];
    //     __uint64_t      __mdscr_el1; /* Bit 0 is SS (Hardware Single Step) */
    // };
    // 
    // #define _STRUCT_ARM_DEBUG_STATE64    struct __darwin_arm_debug_state64
    // _STRUCT_ARM_DEBUG_STATE64
    // {
    //     __uint64_t        __bvr[16];
    //     __uint64_t        __bcr[16];
    //     __uint64_t        __wvr[16];
    //     __uint64_t        __wcr[16];
    //     __uint64_t      __mdscr_el1; /* Bit 0 is SS (Hardware Single Step) */
    // };
    // 
    // #else
    // /* #error unknown architecture */
    // #endif
    // 
    public sealed class ArmDebugState
    {
        public UInt64[] BVR { get; }
        public UInt64[] BCR { get; }
        public UInt64[] WVR { get; }
        public UInt64[] WCR { get; }
        public Object MDCSR_EL1 { get; }


        internal ArmDebugState(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian,
            bool is64bit,
            bool is_legacy)
        {
            BVR = new UInt64[16];
            BCR = new UInt64[16];
            WVR = new UInt64[16];
            WCR = new UInt64[16];

            if (is_bigendian)
            {
                if (is64bit)
                {
                    for (UInt32 i = 0; i < (UInt32)BVR.Length; i++)
                    {
                        BVR[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                        offset += 8u;
                    }

                    for (UInt32 i = 0; i < (UInt32)BCR.Length; i++)
                    {
                        BCR[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                        offset += 8u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WVR.Length; i++)
                    {
                        WVR[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                        offset += 8u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WCR.Length; i++)
                    {
                        WCR[i] = BinaryHelper.ToUInt64Big(filebytes, offset);
                        offset += 8u;
                    }
                }
                else
                {
                    for (UInt32 i = 0; i < (UInt32)BVR.Length; i++)
                    {
                        BVR[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                        offset += 4u;
                    }

                    for (UInt32 i = 0; i < (UInt32)BCR.Length; i++)
                    {
                        BCR[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                        offset += 4u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WVR.Length; i++)
                    {
                        WVR[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                        offset += 4u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WCR.Length; i++)
                    {
                        WCR[i] = BinaryHelper.ToUInt32Big(filebytes, offset);
                        offset += 4u;
                    }
                }

                if (!is_legacy)
                    MDCSR_EL1 = BinaryHelper.ToUInt64Big(filebytes, offset);
            }
            else
            {
                if (is64bit)
                {
                    for (UInt32 i = 0; i < (UInt32)BVR.Length; i++)
                    {
                        BVR[i] = BinaryHelper.ToUInt64(filebytes, offset);
                        offset += 8u;
                    }

                    for (UInt32 i = 0; i < (UInt32)BCR.Length; i++)
                    {
                        BCR[i] = BinaryHelper.ToUInt64(filebytes, offset);
                        offset += 8u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WVR.Length; i++)
                    {
                        WVR[i] = BinaryHelper.ToUInt64(filebytes, offset);
                        offset += 8u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WCR.Length; i++)
                    {
                        WCR[i] = BinaryHelper.ToUInt64(filebytes, offset);
                        offset += 8u;
                    }
                }
                else
                {
                    for (UInt32 i = 0; i < (UInt32)BVR.Length; i++)
                    {
                        BVR[i] = BinaryHelper.ToUInt32(filebytes, offset);
                        offset += 4u;
                    }

                    for (UInt32 i = 0; i < (UInt32)BCR.Length; i++)
                    {
                        BCR[i] = BinaryHelper.ToUInt32(filebytes, offset);
                        offset += 4u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WVR.Length; i++)
                    {
                        WVR[i] = BinaryHelper.ToUInt32(filebytes, offset);
                        offset += 4u;
                    }

                    for (UInt32 i = 0; i < (UInt32)WCR.Length; i++)
                    {
                        WCR[i] = BinaryHelper.ToUInt32(filebytes, offset);
                        offset += 4u;
                    }
                }

                if (!is_legacy)
                    MDCSR_EL1 = BinaryHelper.ToUInt64(filebytes, offset);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{BVR={0}; BCR={1}; WVR={2}; WCR={3}; MDCSR_EL1={4}}}",
                BVR,
                BCR,
                WVR,
                WCR,
                MDCSR_EL1);
        }
    }
}
