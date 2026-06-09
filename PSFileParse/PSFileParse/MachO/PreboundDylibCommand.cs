using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // union lc_str {
    //     uint32_t offset;
    // #ifndef __LP64__
    //     char     *ptr;
    // #endif 
    // };
    // 
    // struct prebound_dylib_command {
    //     uint32_t     cmd;
    //     uint32_t     cmdsize;
    //     union lc_str name;
    //     uint32_t     nmodules;
    //     union lc_str linked_modules;
    // };
    // 
    public sealed class PreboundDylibCommand
    {
        public String Name { get; }
        public UInt32 NumberOfModules { get; }
        public byte[] LinkedModules{ get; }


        internal PreboundDylibCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 name_offset;
            UInt32 mod_offset;

            if (is_bigendian)
            {
                name_offset = BinaryHelper.ToUInt32Big(filebytes, offset);
                NumberOfModules = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                mod_offset = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            }
            else
            {
                name_offset = BinaryHelper.ToUInt32(filebytes, offset);
                NumberOfModules = BinaryHelper.ToUInt32(filebytes, offset + 4);
                mod_offset = BinaryHelper.ToUInt32(filebytes, offset + 8);
            }

            name_offset = offset + name_offset - 8;
            mod_offset = offset + mod_offset - 8;
            Name = BinaryHelper.GetUTF8String(filebytes, name_offset);
            LinkedModules = new byte[NumberOfModules];

            for (UInt32 i = 0; i < (UInt32)LinkedModules.Length; i++)
                LinkedModules[i] = (byte)((filebytes[mod_offset + (i / 8)] >> (byte)(i % 8)) & 1);
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; NumberOfModules={1}; LinkedModules={2}}}",
                Name,
                NumberOfModules,
                LinkedModules);
        }
    }
}