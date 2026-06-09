using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    // 
    // struct fat_header {
    //     uint32_t    magic;
    //     uint32_t    nfat_arch;
    // };
    // 
    public sealed class FatHeader
    {
        public FatMagic Magic { get; }
        public UInt32 NumberOfArch { get; }
        public FatArch[] Arch { get; }


        internal FatHeader(byte[] filebytes, ref UInt32 offset)
        {
            Magic = (FatMagic)BinaryHelper.ToUInt32(filebytes, offset);

            if (Magic == FatMagic.LittleEndian)
            {
                NumberOfArch = BinaryHelper.ToUInt32(filebytes, offset + 4u);
                Arch = new FatArch[NumberOfArch];
                offset += 8u;

                for (UInt32 i = 0; i < NumberOfArch; i++)
                    Arch[i] = new FatArch(filebytes, Magic, ref offset);
            }
            else
            {
                NumberOfArch = BinaryHelper.ToUInt32Big(filebytes, offset + 4u);
                Arch = new FatArch[NumberOfArch];
                offset += 8u;

                for (UInt32 i = 0; i < NumberOfArch; i++)
                    Arch[i] = new FatArch(filebytes, Magic, ref offset);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; NumberOfArch={1}; Arch={2}}}",
                Magic,
                NumberOfArch,
                Arch);
        }
    }
}
