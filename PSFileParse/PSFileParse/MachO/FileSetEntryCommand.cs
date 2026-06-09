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
    // struct fileset_entry_command {
    //     uint32_t     cmd;
    //     uint32_t     cmdsize;
    //     uint64_t     vmaddr;
    //     uint64_t     fileoff;
    //     union lc_str entry_id;
    //     uint32_t     reserved;
    // };
    // 
    public sealed class FileSetEntryCommand
    {
        public UInt64 VMAddress { get; }
        public UInt64 VMOffset { get; }
        public String EntryId { get; }
        public UInt32 Reserved { get; }


        internal FileSetEntryCommand(
            byte[] filebytes,
            UInt32 offset,
            bool is_bigendian)
        {
            UInt32 str_offset;

            if (is_bigendian)
            {
                VMAddress = BinaryHelper.ToUInt64Big(filebytes, offset);
                VMOffset = BinaryHelper.ToUInt64Big(filebytes, offset + 8);
                str_offset = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
                Reserved = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
            }
            else
            {
                VMAddress = BinaryHelper.ToUInt64(filebytes, offset);
                VMOffset = BinaryHelper.ToUInt64(filebytes, offset + 8);
                str_offset = BinaryHelper.ToUInt32(filebytes, offset + 16);
                Reserved = BinaryHelper.ToUInt32(filebytes, offset + 20);
            }

            str_offset = offset + str_offset - 8;
            EntryId = BinaryHelper.GetUTF8String(filebytes, str_offset);
        }


        public override String ToString()
        {
            return String.Format("@{{VMAddress={0}; VMOffset={1}; EntryId={2}; Reserved={3}}}",
                VMAddress,
                VMOffset,
                EntryId,
                Reserved);
        }
    }
}