using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct dyld_info_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    //     uint32_t rebase_off;
    //     uint32_t rebase_size;
    //     uint32_t bind_off;
    //     uint32_t bind_size;
    //     uint32_t weak_bind_off;
    //     uint32_t weak_bind_size;
    //     uint32_t lazy_bind_off;
    //     uint32_t lazy_bind_size;
    //     uint32_t export_off;
    //     uint32_t export_size;
    // };
    // 
    internal class DyldInfoCommand
    {
        public UInt32 RebaseOffset { get; }
        public UInt32 RebaseSize { get; }
        public UInt32 BindOffset { get; }
        public UInt32 BindSize { get; }
        public UInt32 WeakBindOffset { get; }
        public UInt32 WeakBindSize { get; }
        public UInt32 LazyBindOffset { get; }
        public UInt32 LazyBindSize { get; }
        public UInt32 ExportOffset { get; }
        public UInt32 ExportSize { get; }


        internal DyldInfoCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            if (is_bigendian)
            {
                RebaseOffset = BinaryHelper.ToUInt32Big(filebytes, offset);
                RebaseSize = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
                BindOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
                BindSize = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
                WeakBindOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                WeakBindSize = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
                LazyBindOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 24);
                LazyBindSize = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
                ExportOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
                ExportSize = BinaryHelper.ToUInt32Big(filebytes, offset + 36);
            }
            else
            {
                RebaseOffset = BinaryHelper.ToUInt32(filebytes, offset);
                RebaseSize = BinaryHelper.ToUInt32(filebytes, offset + 4);
                BindOffset = BinaryHelper.ToUInt32(filebytes, offset + 8);
                BindSize = BinaryHelper.ToUInt32(filebytes, offset + 12);
                WeakBindOffset = BinaryHelper.ToUInt32(filebytes, offset + 16);
                WeakBindSize = BinaryHelper.ToUInt32(filebytes, offset + 20);
                LazyBindOffset = BinaryHelper.ToUInt32(filebytes, offset + 24);
                LazyBindSize = BinaryHelper.ToUInt32(filebytes, offset + 28);
                ExportOffset = BinaryHelper.ToUInt32(filebytes, offset + 32);
                ExportSize = BinaryHelper.ToUInt32(filebytes, offset + 36);
            }
        }



        public override String ToString()
        {
            return String.Format("@{{RebaseOffset={0}; RebaseSize={1}; BindOffset={2}; BindSize={3}; WeakBindOffset={4}; WeakBindSize={5}; LazyBindOffset={6}; LazyBindSize={7}; ExportOffset={8}; ExportSize={9}}}",
                RebaseOffset,
                RebaseSize,
                BindOffset,
                BindSize,
                WeakBindOffset,
                WeakBindSize,
                LazyBindOffset,
                LazyBindSize,
                ExportOffset,
                ExportSize);
        }
    }
}
