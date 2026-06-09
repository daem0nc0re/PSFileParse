using PSFileParse.Auxiliary;
using PSFileParse.PE;
using System;

namespace PSFileParse.Bitmap
{
    // 
    // struct ICONDIR // Alignment is 2 bytes
    // {
    //     short        idReserved;   // Reserved (must be 0)
    //     short        idType;       // Resource Type (1 for icons)
    //     short        idCount;      // How many images?
    //     ICONDIRENTRY idEntries[1]; // An entry for each image (idCount of 'em)
    // };
    // 
    public sealed class IconDir
    {
        public UInt16 Reserved { get; }
        public ResourceType Type { get; }
        public UInt16 Count { get; }
        public IconDirEntry[] Entries { get; }


        internal IconDir(byte[] filebytes, UInt32 offset)
        {
            if ((filebytes == null) || (filebytes.Length < 22))
                throw new Exception("File size is too small.");

            Reserved = BinaryHelper.ToUInt16(filebytes, offset);
            Type = (ResourceType)BinaryHelper.ToUInt16(filebytes, offset + 2);
            Count = BinaryHelper.ToUInt16(filebytes, offset + 4);
            Entries = new IconDirEntry[Count];

            if ((Reserved != 0) ||
                ((Type != ResourceType.Icon) && (Type != ResourceType.Cursor)) ||
                (Count == 0) ||
                (filebytes.Length < offset + 6 + (Count * 16)))
            {
                throw new Exception("Invalid file format.");
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Reserved={0}; Type={1}; Count={2}; Entries={3}}}",
                this.Reserved,
                this.Type.ToString(),
                this.Count,
                this.Entries.ToString());
        }


        internal static byte[] GetBytesFromDibBytes(
            byte[] dib_bytes,
            UInt32 offset,
            UInt32 dib_size)
        {
            if ((dib_bytes == null) || (dib_bytes.Length < 0xC + offset))
                return null;

            var magic = BinaryHelper.ToUInt32(dib_bytes, offset);
            var header_bytes = new byte[]
            {
                0x00, 0x00,             // 00: IconDir.Reserved must be 0
                0x01, 0x00,             // 02: IconDir.Type = 1 for icon
                0x01, 0x00,             // 04: IconDir.Count = 1
                0x00,                   // 06: IconDir.Entries[1].Width
                0x00,                   // 07: IconDir.Entries[1].Height
                0x00,                   // 08: IconDir.Entries[1].ColorCount
                0x00,                   // 09: IconDir.Entries[1].Reserved
                0x00, 0x00,             // 0A: IconDir.Entries[1].Planes
                0x00, 0x00,             // 0C: IconDir.Entries[1].BitCount
                0x00, 0x00, 0x00, 0x00, // 0E: IconDir.Entries[1].BytesInRes
                0x16, 0x00, 0x00, 0x00, // 12: IconDir.Entries[1].ImageOffset
            };

            if ((magic == 0x28) && (dib_bytes.Length < 0x28 + offset))
                return null;

            if (magic == 0x28)
            {
                var info_header = new BitmapInfoHeader(dib_bytes, offset);
                header_bytes[0x6] = (byte)(info_header.Width & 0xFF);
                header_bytes[0x7] = (byte)(info_header.Height & 0xFF);
                Buffer.BlockCopy(BitConverter.GetBytes(info_header.Planes), 0, header_bytes, 0xA, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(info_header.BitCount), 0, header_bytes, 0xC, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(dib_size), 0, header_bytes, 0xE, 4);

                if (info_header.BitCount < 8)
                {
                    int colors_num;

                    if (info_header.Compression == BitmapCompressionType.Rgb)
                    {
                        if (info_header.ClrUsed == 0)
                            colors_num = 1 << info_header.BitCount;
                        else
                            colors_num = 1 << (Int32)info_header.ClrUsed;
                    }
                    else
                    {
                        colors_num = (info_header.Compression == BitmapCompressionType.BitFields) ? 3 : 0;
                    }

                    header_bytes[0x8] = (byte)(colors_num & 0xFF);
                }
            }
            else
            {
                var core_header = new BitmapCoreHeader(dib_bytes, offset);
                header_bytes[0x6] = (byte)(core_header.Width & 0xFF);
                header_bytes[0x7] = (byte)(core_header.Height & 0xFF);
                Buffer.BlockCopy(BitConverter.GetBytes(core_header.Planes), 0, header_bytes, 0xA, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(core_header.BitCount), 0, header_bytes, 0xC, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(dib_size), 0, header_bytes, 0xE, 4);

                if (core_header.BitCount < 8)
                {
                    int colors_num;

                    if (core_header.BitCount == 1)
                        colors_num = 2;
                    else if (core_header.BitCount == 4)
                        colors_num = 16;
                    else if (core_header.BitCount == 8)
                        colors_num = 256;
                    else if (core_header.BitCount == 24)
                        colors_num = 0;
                    else
                        colors_num = 0;

                    header_bytes[0x8] = (byte)(colors_num & 0xFF);
                }
            }

            return header_bytes;
        }
    }
}
