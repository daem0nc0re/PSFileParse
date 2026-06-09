using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // #define _STRUCT_ARM_EXCEPTION_STATE    struct __darwin_arm_exception_state
    // _STRUCT_ARM_EXCEPTION_STATE
    // {
    //     __uint32_t    __exception; /* number of arm exception taken */
    //     __uint32_t    __fsr; /* Fault status */
    //     __uint32_t    __far; /* Virtual Fault Address */
    // };
    // 
    public sealed class ArmExceptionState
    {
        public UInt32 Exception { get; }
        public UInt32 FSR { get; }
        public UInt32 FAR { get; }


        internal ArmExceptionState(
            byte[] filebytes,
            ref UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                Exception = BinaryHelper.ToUInt32Big(filebytes, offset);
                FSR = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                FAR = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            }
            else
            {
                Exception = BinaryHelper.ToUInt32(filebytes, offset);
                FSR = BinaryHelper.ToUInt32(filebytes, offset + 4);
                FAR = BinaryHelper.ToUInt32(filebytes, offset + 8);
            }

            offset += 12u;
        }


        public override String ToString()
        {
            return String.Format("@{{Exception={0}; FSR={1}; FAR={2}}}",
                Exception,
                FSR,
                FAR);
        }
    }
}
