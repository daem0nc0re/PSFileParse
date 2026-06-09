using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct scattered_relocation_info {
    // #ifdef __BIG_ENDIAN__
    //     uint32_t r_scattered:1,
    //              r_pcrel:1,
    //              r_length:2,
    //              r_type:4,
    //              r_address:24;
    //     int32_t  r_value;
    // #endif
    // #ifdef __LITTLE_ENDIAN__
    //     uint32_t r_address:24,
    //              r_type:4,
    //              r_length:2,
    //              r_pcrel:1,
    //              r_scattered:1;
    //     int32_t  r_value;
    // #endif
    // };
    // 
    // struct relocation_info {
    //     int32_t  r_address;
    //     uint32_t r_symbolnum:24,
    //              r_pcrel:1,
    //              r_length:2,
    //              r_extern:1,
    //              r_type:4;
    // };
    // 
    public sealed class RelocationInfo
    {
        public UInt32 Index { get; }
        public bool Scattered { get; }
        public UInt32 Address { get; }
        public Object SymbolNumber { get; }
        public bool PCRelative { get; }
        public RelocLength Length { get; }
        public Object Extern { get; }
        public Object Type { get; }
        public Object Value { get; }


        internal RelocationInfo(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            MachCpuType cputype,
            bool is_bigendian)
        {
            UInt32 value;
            Index = index;

            if (is_bigendian)
            {
                Address = BinaryHelper.ToUInt32Big(filebytes, offset);
                value = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                Address = BinaryHelper.ToUInt32(filebytes, offset);
                value = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            if (((Address & 0x80000000) != 0) && (cputype != MachCpuType.X86_64))
            {
                Scattered = true;
                PCRelative = (Address & 0x40000000) != 0;
                Length = (RelocLength)((Address >> 28) & 0x3);
                Address &= 0x00FFFFFF;
                Value = value;

                if (cputype == MachCpuType.ARM)
                    Type = (RelocTypeArm)((Address >> 24) & 0xF);
                else if (cputype == MachCpuType.ARM64)
                    Type = (RelocTypeArm64)((Address >> 24) & 0xF);
                else if (cputype == MachCpuType.X86_64)
                    Type = (RelocTypeX64)((Address >> 24) & 0xF);
                else
                    Type = (RelocTypeGeneric)((Address >> 24) & 0xF);
            }
            else
            {
                Scattered = false;
                SymbolNumber = value & 0x00FFFFFF;
                PCRelative = ((value & 0x01000000) != 0);
                Length = (RelocLength)((value >> 25) & 0x3);
                Extern = ((value & 0x08000000) != 0);

                if (cputype == MachCpuType.ARM)
                    Type = (RelocTypeArm)((value >> 28) & 0xF);
                else if (cputype == MachCpuType.ARM64)
                    Type = (RelocTypeArm64)((value >> 28) & 0xF);
                else if (cputype == MachCpuType.X86_64)
                    Type = (RelocTypeX64)((value >> 28) & 0xF);
                else
                    Type = (RelocTypeGeneric)((value >> 28) & 0xF);
            }


            offset += 8u;
        }


        public override String ToString()
        {
            return String.Format("@{{Scattered={0}; Address={1}; SymbolNumber={2}; PCRelative={3}; Length={4}; Extern={5}; Type={6}; Value={7}}}",
                Scattered,
                Address,
                SymbolNumber,
                PCRelative,
                Length,
                Extern,
                Type,
                Value);
        }
    }
}