using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE
{
    public sealed class ImageRichHeader
    {
        public FileSignature Magic { get; }
        public UInt32[] Padding { get; }
        public RichHeaderEntry[] Information { get; }
        public FileSignature Signature { get; }
        public UInt32 Checksum { get; }


        internal ImageRichHeader(byte[] filebytes, UInt32 e_lfanew)
        {
            UInt32 end_addr = 0u;

            for (UInt32 oft = 0x40u; oft < e_lfanew - 4u; oft += 4u)
            {
                var sig = BinaryHelper.ToUInt32(filebytes, oft);

                if (sig != 0x68636952u)
                    continue;

                Signature = new FileSignature(filebytes, oft, 4);
                Checksum = BinaryHelper.ToUInt32(filebytes, oft + 4);
                end_addr = oft;
                break;
            }

            if ((Signature != null) || (end_addr == 0u))
            {
                for (UInt32 i = 0x40u; i < e_lfanew - 4u; i += 4u)
                {
                    var info = new List<RichHeaderEntry>();
                    var magic = BinaryHelper.ToUInt32(filebytes, i) ^ Checksum;

                    if (magic != 0x536E6144u)
                        continue;

                    Magic = new FileSignature(BitConverter.GetBytes(magic), 0, 4);
                    Padding = new UInt32[]
                    {
                        BinaryHelper.ToUInt32(filebytes, i + 4) ^ Checksum,
                        BinaryHelper.ToUInt32(filebytes, i + 8) ^ Checksum,
                        BinaryHelper.ToUInt32(filebytes, i + 12) ^ Checksum
                    };

                    for (UInt32 j = i + 16; j < end_addr; j += 8)
                        info.Add(new RichHeaderEntry(filebytes, j, Checksum));

                    Information = info.ToArray();
                    break;
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; Padding={1}; Information={2}; Signature={3}; Checksum={4}}}",
                Magic,
                Padding,
                Information,
                Signature,
                Checksum);
        }
    }
}
