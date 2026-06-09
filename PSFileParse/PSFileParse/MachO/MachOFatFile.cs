using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    public sealed class MachOFatFile
    {
        public String FileName { get; }
        public UInt32 FileSize { get; }
        public FileHash FileHash { get; }
        public FatHeader FatHeader { get; }
        public MachOFile[] MachOFiles { get; }


        internal MachOFatFile(
            String filename,
            byte[] filebytes,
            out List<String> warns)
        {
            UInt32 offset = 0u;
            FileName = filename;
            FileSize = (UInt32)filebytes.Length;
            FileHash = new FileHash(filebytes);
            warns = new List<String>();

            if (Enum.IsDefined(typeof(FatMagic), BinaryHelper.ToUInt32(filebytes, 0)))
            {
                FatHeader = new FatHeader(filebytes, ref offset);
                MachOFiles = new MachOFile[FatHeader.NumberOfArch];

                for (UInt32 i = 0u; i < FatHeader.NumberOfArch; i++)
                {
                    try
                    {
                        var data = new byte[FatHeader.Arch[i].Size];
                        var oft = FatHeader.Arch[i].Offset;
                        Array.Copy(filebytes, oft, data, 0, FatHeader.Arch[i].Size);
                        MachOFiles[i] = new MachOFile(filename, data);
                    }
                    catch (Exception e)
                    {
                        var msg = String.Format("Failed to parse {0} Mach-O part (packed?): {1}",
                            FatHeader.Arch[i].CPUType,
                            e.Message);
                        warns.Add(msg);
                    }
                }
            }
        }
    }
}
