using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFileParse.MachO
{
    public sealed class ArmExceptionState64
    {
        // 
        // #define _STRUCT_ARM_EXCEPTION_STATE64    struct __darwin_arm_exception_state64
        // _STRUCT_ARM_EXCEPTION_STATE64
        // {
        //     __uint64_t    __far; /* Virtual Fault Address */
        //     __uint32_t    __esr; /* Exception syndrome */
        //     __uint32_t    __exception; /* number of arm exception taken */
        // };
        // 
        public UInt64 FAR { get; }
        public UInt32 ESR { get; }
        public UInt32 Exception { get; }


        internal ArmExceptionState64(
            byte[] filebytes,
            ref UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                FAR = BinaryHelper.ToUInt32Big(filebytes, offset);
                ESR = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                Exception = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            }
            else
            {
                FAR = BinaryHelper.ToUInt32Big(filebytes, offset);
                ESR = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                Exception = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            }

            offset += 16u;
        }


        public override String ToString()
        {
            return String.Format("@{{FAR={0}; ESR={1}; Exception={2}}}",
                FAR,
                ESR,
                Exception);
        }
    }
}
