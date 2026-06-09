using PSFileParse.Auxiliary;
using PSFileParse.Bitmap;
using System;
using System.Collections.Generic;
using System.IO;

namespace PSFileParse.PE
{
    //  
    // struct IMAGE_RESOURCE_DIRECTORY
    // {
    //     unsigned int   Characteristics;
    //     unsigned int   TimeDateStamp;
    //     unsigned short MajorVersion;
    //     unsigned short MinorVersion;
    //     unsigned short NumberOfNamedEntries,;
    //     unsigned short NumberOfIdEntries;
    // };
    // 
    public sealed class ImageResourceDirectory
    {
        public UInt32 Characteristics { get; }
        public UnixTime TimeDateStamp { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public UInt16 NumberOfNamedEntries { get; }
        public UInt16 NumberOfIdEntries { get; }
        public ImageResourceDirectoryEntry[] Entries { get; }
        internal String TypeId { get; }
        internal String NameId { get; }
        internal String LanguageId { get; }
        internal static readonly UInt32 SizeOfStruct = 0x10;


        internal ImageResourceDirectory(
            byte[] filebytes,
            UInt32 dir_base,
            UInt32 offset,
            UInt32 delta,
            String type_id,
            String name_id,
            String lang_id)
        {
            var dir_offset = dir_base + offset;
            Characteristics = BinaryHelper.ToUInt32(filebytes, dir_offset);
            TimeDateStamp = new UnixTime(BinaryHelper.ToUInt32(filebytes, dir_offset + 4));
            MajorVersion = BinaryHelper.ToUInt16(filebytes, dir_offset + 8);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, dir_offset + 10);
            NumberOfNamedEntries = BinaryHelper.ToUInt16(filebytes, dir_offset + 12);
            NumberOfIdEntries = BinaryHelper.ToUInt16(filebytes, dir_offset + 14);
            Entries = new ImageResourceDirectoryEntry[NumberOfNamedEntries + NumberOfIdEntries];
            dir_offset += 16u;

            if (type_id != null)
                TypeId = type_id;

            if (name_id != null)
                NameId = name_id;

            if (lang_id != null)
                LanguageId = lang_id;

            for (UInt32 i = 0; i < (UInt32)Entries.Length; i++)
            {
                Entries[i] = new ImageResourceDirectoryEntry(
                    filebytes,
                    i,
                    dir_base,
                    dir_offset,
                    delta,
                    TypeId,
                    NameId,
                    LanguageId);
                dir_offset += ImageResourceDirectoryEntry.SizeOfStruct;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Characteristics={0}; TimeDateStamp={1}; MajorVersion={2}; MinorVersion={3}; NumberOfNamedEntries={4}; NumberOfIdEntries={5}; Entries={6}}}",
                Characteristics,
                TimeDateStamp,
                MajorVersion,
                MinorVersion,
                NumberOfNamedEntries,
                NumberOfIdEntries,
                Entries);
        }


        public void Export(String destination)
        {
            var base_dir = Path.GetFullPath(destination);
            var entries = new List<ImageResourceDirectoryEntry>(this.Entries);

            if (Directory.Exists(base_dir))
                throw new Exception(String.Format("\"{0}\" already exists.", base_dir));
            else
                Directory.CreateDirectory(base_dir);

            while (entries.Count > 0)
            {
                var next_entries = new List<ImageResourceDirectoryEntry>();

                foreach (var entry in entries)
                {
                    var dir = base_dir;
                    var subdir_names = new String[]
                    {
                        entry.TypeId,
                        entry.NameId
                    };

                    foreach (var name in subdir_names)
                    {
                        if (name == null)
                            break;

                        dir = Path.GetFullPath(String.Format("{0}/{1}", dir, name));

                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }

                    if (entry.IsDirectory)
                    {
                        foreach (var sub_dir in ((ImageResourceDirectory)entry.Content).Entries)
                            next_entries.Add(sub_dir);
                    }
                    else
                    {
                        var bytes = ((ImageResourceDataEntry)entry.Content).Data;
                        var ext = GetFileExtension(bytes);
                        var content_path = Path.GetFullPath(String.Format("{0}/{1}{2}",
                            dir,
                            entry.LanguageId,
                            ext));
                        File.WriteAllBytes(content_path, bytes);
                    }
                }

                entries.Clear();
                entries = next_entries;
            }
        }


        private static String GetFileExtension(byte[] resource_data)
        {
            var ext = ".bin";

            try
            {
                var magic16 = BinaryHelper.ToUInt16(resource_data, 0);
                var magic32 = BinaryHelper.ToUInt32(resource_data, 0);
                var magic64 = BinaryHelper.ToUInt64(resource_data, 0);

                if (magic16 == 0)
                {
                    var ico_type = BinaryHelper.ToUInt16(resource_data, 2);
                    var ico_count = BinaryHelper.ToUInt16(resource_data, 4);
                    var reserved = resource_data[9];

                    if ((ico_type == 1) && (ico_count > 0) && (reserved == 0))
                        ext = ".ico";
                    else if ((ico_type == 2) && (ico_count > 0) && (reserved == 0))
                        ext = ".cur";
                }
                else if (magic16 == 0x4D42)
                {
                    ext = ".bmp";
                }
                else if (magic16 == 0x5A4D)
                {
                    var e_lfanew = BinaryHelper.ToUInt32(resource_data, 0x3C);
                    var signature = BinaryHelper.ToUInt32(resource_data, e_lfanew);

                    if (signature == 0x4550)
                        ext = ".exe.bin";
                }
                else if (magic16 == 0xFEFF)
                {
                    magic64 = BinaryHelper.ToUInt64(resource_data, 2);

                    if ((magic64 & 0x0000FFFFFFFFFFFFUL) == 0x0000206C6D783F3CUL)
                        ext = ".xml";
                    else
                        ext = ".txt";
                }
                else if (magic16 == 0xFFFE)
                {
                    magic64 = BinaryHelper.ToUInt64(resource_data, 2);

                    if ((magic64 & 0x0000FFFFFFFFFFFFUL) == 0x0000206C6D783F3CUL)
                        ext = ".xml";
                    else
                        ext = ".txt";
                }
                else if (magic32 == 0xCu)
                {
                    var bitcount = BinaryHelper.ToInt16(resource_data, 10);

                    if ((bitcount == 1) || (bitcount == 4) || (bitcount == 8) || (bitcount == 24))
                        ext = ".dib";
                }
                else if (magic32 == 0x28u)
                {
                    var compression = BinaryHelper.ToInt32(resource_data, 16);

                    if (Enum.IsDefined(typeof(BitmapCompressionType), compression))
                        ext = ".dib";
                }
                else if (magic32 == 0xFEFFu)
                {
                    magic64 = BinaryHelper.ToUInt64(resource_data, 4);

                    if ((magic64 & 0x0000FFFFFFFFFFFFUL) == 0x0000206C6D783F3CUL)
                        ext = ".xml";
                    else
                        ext = ".txt";
                }
                else if ((magic32 & 0x00FFFFFFu) == 0xBFBBEFu)
                {
                    magic64 = BinaryHelper.ToUInt64(resource_data, 3);

                    if ((magic64 & 0x0000FFFFFFFFFFFFUL) == 0x0000206C6D783F3CUL)
                        ext = ".xml";
                    else
                        ext = ".txt";
                }
                else if (magic32 == 0x474E5089u)
                {
                    ext = ".png";
                }
                else if (magic32 == 0xFFFE0000u)
                {
                    magic64 = BinaryHelper.ToUInt64(resource_data, 4);

                    if ((magic64 & 0x0000FFFFFFFFFFFFUL) == 0x0000206C6D783F3CUL)
                        ext = ".xml";
                    else
                        ext = ".txt";
                }
                else if ((magic64 & 0x0000FFFFFFFFFFFFUL) == 0x0000206C6D783F3CUL)
                {
                    ext = ".xml";
                }
            } catch { }

            return ext;
        }
    }
}