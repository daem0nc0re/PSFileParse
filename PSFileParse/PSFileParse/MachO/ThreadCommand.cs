using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct thread_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t flavor;
    //     uint32_t count;
    //     // struct XXX_thread_state state;
    // };
    // 
    public sealed class ThreadCommand
    {
        public Object Flavor { get; }
        public UInt32 Count { get; }
        public Object State { get; }


        internal ThreadCommand(
            byte[] filebytes,
            UInt32 offset,
            UInt32 size,
            MachCpuType cputype,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                Flavor = BinaryHelper.ToUInt32Big(filebytes, offset);
                Count = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                Flavor = BinaryHelper.ToUInt32(filebytes, offset);
                Count = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            if ((cputype == MachCpuType.ARM) || (cputype == MachCpuType.ARM64))
            {
                var flavor = (ARMThreadFlavors)Flavor;
                Flavor = flavor;

                if (flavor == ARMThreadFlavors.ThreadState)
                {
                    State = new ArmThreadState(filebytes, offset + 8u, is_bigendian);
                }
                else if (flavor == ARMThreadFlavors.ThreadState64)
                {
                    State = new ArmThreadState64(filebytes, offset + 8u, is_bigendian);
                }
                else
                {
                    var arr = new byte[size - 8u];
                    Array.Copy(filebytes, offset + 8u, arr, 0, arr.Length);
                    State = arr;
                }
            }
            else if ((cputype == MachCpuType.I386) || (cputype == MachCpuType.X86_64))
            {
                var flavor = (X86ThreadFlavors)Flavor;
                Flavor = flavor;

                if ((flavor == X86ThreadFlavors.ThreadState) || (flavor == X86ThreadFlavors.ThreadState32))
                {
                    State = new X86ThreadState32(filebytes, offset + 8u, is_bigendian);
                }
                else if (flavor == X86ThreadFlavors.ThreadState64)
                {
                    State = new X86ThreadState64(filebytes, offset + 8u, is_bigendian);
                }
                else
                {
                    var arr = new byte[size - 8u];
                    Array.Copy(filebytes, offset + 8u, arr, 0, arr.Length);
                    State = arr;
                }
            }
            else if ((cputype == MachCpuType.POWERPC) || (cputype == MachCpuType.POWERPC64))
            {
                var flavor = (PPCThreadFlavors)Flavor;
                Flavor = flavor;

                if (flavor == PPCThreadFlavors.ThreadState)
                {
                    State = new PPCThreadState(filebytes, offset + 8, is_bigendian);
                }
                else if (flavor == PPCThreadFlavors.ThreadState64)
                {
                    State = new PPCThreadState64(filebytes, offset + 8, is_bigendian);
                }
                else
                {
                    var arr = new byte[size - 8u];
                    Array.Copy(filebytes, offset + 8u, arr, 0, arr.Length);
                    State = arr;
                }
            }
            else
            {
                var arr = new byte[size - 8u];
                Array.Copy(filebytes, offset + 8u, arr, 0, arr.Length);
                State = arr;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Flavor={0}; Count={1}; State={2}}}",
                Flavor,
                Count,
                State);
        }
    }
}
