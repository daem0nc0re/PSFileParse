using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE
{
    public sealed class PEFile
    {
        public String FileName { get; }
        public UInt32 FileSize { get; }
        public CryptoHash ImpHash { get; }
        public FileHash FileHash { get; }
        public byte[] FileBytes { get; }
        public ImageDOSHeader DOSHeader { get; }
        public ImageRichHeader RichHeader { get; }
        public ImageNtHeaders NtHeaders { get; }
        public ImageSectionHeader[] SectionHeaders { get; }
        public Dictionary<String, Object> DataDirectories { get; }


        internal PEFile(String filename, byte[] filebytes, out List<String> warn_msgs)
        {
            ImageRichHeader rich_header;
            UInt32 section_offset;
            UInt32 required_size = 0;
            FileName = filename;
            FileSize = (UInt32)filebytes.Length;
            FileHash = new FileHash(filebytes);
            FileBytes = filebytes;
            DOSHeader = new ImageDOSHeader(filebytes, 0);
            NtHeaders = new ImageNtHeaders(
                filebytes,
                DOSHeader.FileAddressOfNewHeader,
                out ImageHeaderMagic magic);
            SectionHeaders = new ImageSectionHeader[NtHeaders.FileHeader.NumberOfSections];
            DataDirectories = new Dictionary<String, Object>();
            rich_header = new ImageRichHeader(filebytes, DOSHeader.FileAddressOfNewHeader);
            section_offset = DOSHeader.FileAddressOfNewHeader + 0x18 + NtHeaders.FileHeader.SizeOfOptionalHeader;
            warn_msgs = new List<String>();

            if (rich_header.Magic != null)
                RichHeader = rich_header;

            try
            {
                for (UInt32 i = 0; i < (UInt32)SectionHeaders.Length; i++)
                {
                    SectionHeaders[i] = new ImageSectionHeader(filebytes, i, section_offset);
                    section_offset += ImageSectionHeader.SizeOfStruct;

                    if (required_size < SectionHeaders[i].PointerToRawData + SectionHeaders[i].SizeOfRawData)
                        required_size = SectionHeaders[i].PointerToRawData + SectionHeaders[i].SizeOfRawData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to parse section headers: {0}", ex.Message));
            }

            if (required_size > (UInt32)filebytes.Length)
                throw new Exception("Input file size is smaller than expected based on section headers.");

            if ((magic == ImageHeaderMagic.NT64) || (magic == ImageHeaderMagic.NT32))
            {
                ImageDataDirectories dirs;
                ImageFileMachine arch = NtHeaders.FileHeader.Machine;
                bool is_64bit = (magic == ImageHeaderMagic.NT64);

                if (is_64bit)
                    dirs = ((ImageOptionalHeader64)NtHeaders.OptionalHeader).DataDirectory;
                else
                    dirs = ((ImageOptionalHeader32)NtHeaders.OptionalHeader).DataDirectory;

                if ((dirs.Export.VirtualAddress > 0) && (dirs.Export.Size > 0))
                {
                    try
                    {
                        var exports = new ImageExportDirectory(
                            filebytes,
                            dirs.Export,
                            SectionHeaders);
                        DataDirectories.Add("Export", exports);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse Export directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.Import.VirtualAddress > 0) && (dirs.Import.Size > 0))
                {
                    try
                    {
                        var index = 0u;
                        var imports = new List<ImageImportDescriptor>();
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.Import.VirtualAddress,
                            out String _);

                        while (true)
                        {
                            var desc = new ImageImportDescriptor(
                                filebytes,
                                index++,
                                dir_base,
                                SectionHeaders,
                                is_64bit);
                            dir_base += ImageImportDescriptor.SizeOfStruct;

                            if (desc.IsNull)
                                break;

                            imports.Add(desc);
                        }

                        if (imports.Count > 0)
                        {
                            var imports_array = imports.ToArray();
                            DataDirectories.Add("Import", imports_array);
                            ImpHash = ImpHashGen.GetImpHash(imports_array);
                        }
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse Import directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.Resource.VirtualAddress > 0) && (dirs.Resource.Size > 0))
                {
                    try
                    {
                        var dir_base = VirtToPhys(
                        SectionHeaders,
                        (UInt32)dirs.Resource.VirtualAddress,
                        out String _);
                        var delta = (UInt32)dirs.Resource.VirtualAddress - dir_base;
                        var rsrc = new ImageResourceDirectory(
                            filebytes,
                            dir_base,
                            0u,
                            delta,
                            null,
                            null,
                            null);
                        DataDirectories.Add("Resource", rsrc);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse Resource directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.Exception.VirtualAddress > 0) && (dirs.Exception.Size > 0))
                {
                    try
                    {
                        Object exceptions = null;
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.Exception.VirtualAddress,
                            out String _);
                        var boundary = dir_base + dirs.Exception.Size;

                        if ((arch == ImageFileMachine.AMD64) || (arch == ImageFileMachine.IA64))
                        {
                            var entries = new List<ImageRuntimeFunctionEntryX64>();

                            for (var oft = dir_base; oft < boundary; oft += 12u)
                                entries.Add(new ImageRuntimeFunctionEntryX64(filebytes, oft));

                            if (entries.Count > 0)
                                exceptions = entries.ToArray();
                        }
                        else if ((arch == ImageFileMachine.ARM64) ||
                            (arch == ImageFileMachine.ARM64EC) ||
                            (arch == ImageFileMachine.ARM64X))
                        {
                            var entries = new List<ImageRuntimeFunctionEntryArm64>();

                            for (var oft = dir_base; oft < boundary; oft += 8u)
                                entries.Add(new ImageRuntimeFunctionEntryArm64(filebytes, oft));

                            if (entries.Count > 0)
                                exceptions = entries.ToArray();
                        }
                        else if ((arch == ImageFileMachine.ARM) ||
                            (arch == ImageFileMachine.ARMNT) ||
                            (arch == ImageFileMachine.POWERPC) ||
                            (arch == ImageFileMachine.SH3) ||
                            (arch == ImageFileMachine.SH3E) ||
                            (arch == ImageFileMachine.SH3DSP) ||
                            (arch == ImageFileMachine.SH4))
                        {
                            var entries = new List<ImageRuntimeFunctionEntryArm>();

                            for (var oft = dir_base; oft < boundary; oft += 8u)
                                entries.Add(new ImageRuntimeFunctionEntryArm(filebytes, oft));

                            if (entries.Count > 0)
                                exceptions = entries.ToArray();
                        }
                        else if (((arch == ImageFileMachine.MIPSFPU) && !is_64bit) ||
                            (arch == ImageFileMachine.R3000) ||
                            (arch == ImageFileMachine.R3000BE) ||
                            (arch == ImageFileMachine.WCEMIPSV2))
                        {
                            var entries = new List<ImageRuntimeFunctionEntryMips32>();

                            for (var oft = dir_base; oft < boundary; oft += 20u)
                                entries.Add(new ImageRuntimeFunctionEntryMips32(filebytes, oft));

                            if (entries.Count > 0)
                                exceptions = entries.ToArray();
                        }

                        if (exceptions != null)
                            DataDirectories.Add("Exception", exceptions);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse Exception directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.Security.VirtualAddress > 0) && (dirs.Security.Size > 0))
                {
                    try
                    {
                        // For Security Directory, VirtualAddress field indicate raw offset.
                        var cert = new WinCertificate(
                            filebytes,
                            (UInt32)dirs.Security.VirtualAddress);
                        DataDirectories.Add("Security", cert);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse Security directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.BaseReloc.VirtualAddress > 0) && (dirs.BaseReloc.Size > 0))
                {
                    try
                    {
                        var size = 0u;
                        var index = 0u;
                        var max_size = (UInt32)dirs.BaseReloc.Size;
                        var relocs = new List<ImageBaseRelocation>();
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.BaseReloc.VirtualAddress,
                            out String _);

                        while (size < max_size)
                        {
                            var reloc = new ImageBaseRelocation(
                                filebytes,
                                index++,
                                dir_base,
                                arch);
                            size += reloc.SizeOfBlock;
                            dir_base += reloc.SizeOfBlock;
                            relocs.Add(reloc);
                        }

                        if (relocs.Count > 0)
                            DataDirectories.Add("BaseReloc", relocs.ToArray());
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse BaseReloc directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.Debug.VirtualAddress > 0) && (dirs.Debug.Size > 0))
                {
                    try
                    {
                        var count = dirs.Debug.Size / 28;
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.Debug.VirtualAddress,
                            out String _);
                        var debugs = new ImageDebugDirectory[count];

                        for (UInt32 i = 0; i < count; i++)
                        {
                            debugs[i] = new ImageDebugDirectory(
                                filebytes,
                                i,
                                dir_base);
                            dir_base += ImageDebugDirectory.SizeOfStruct;
                        }

                        if (count > 0)
                            DataDirectories.Add("Debug", debugs);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse Debug directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.TLS.VirtualAddress > 0) && (dirs.TLS.Size > 0))
                {
                    try
                    {
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.TLS.VirtualAddress,
                            out String _);
                        var tls = new ImageTLSDirectory(
                            filebytes,
                            dir_base,
                            is_64bit);
                        DataDirectories.Add("TLS", tls);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse TLS directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.LoadConfig.VirtualAddress > 0) && (dirs.LoadConfig.Size > 0))
                {
                    try
                    {
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.LoadConfig.VirtualAddress,
                            out String _);

                        if (is_64bit)
                        {
                            var config64 = new ImageLoadConfigDirectory64(
                                filebytes,
                                dir_base);
                            DataDirectories.Add("LoadConfig", config64);
                        }
                        else
                        {
                            var config32 = new ImageLoadConfigDirectory32(
                                filebytes,
                                dir_base);
                            DataDirectories.Add("LoadConfig", config32);
                        }
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse LoadConfig directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.DelayImport.VirtualAddress > 0) && (dirs.DelayImport.Size > 0))
                {
                    try
                    {
                        var delay_imports = new List<ImageDelayLoadDescriptor>();
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.DelayImport.VirtualAddress,
                            out String _);

                        while (true)
                        {
                            if (BinaryHelper.ToInt32(filebytes, dir_base + 4) == 0)
                                break;

                            var desc = new ImageDelayLoadDescriptor(
                                filebytes,
                                dir_base,
                                SectionHeaders,
                                is_64bit);
                            dir_base += ImageDelayLoadDescriptor.SizeOfStruct;
                            delay_imports.Add(desc);
                        }

                        if (delay_imports.Count > 0)
                            DataDirectories.Add("DelayImport", delay_imports.ToArray());
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse DelayImport directory (packed?): {0}",
                            ex.Message));
                    }
                }

                if ((dirs.CLR.VirtualAddress > 0) && (dirs.CLR.Size > 0))
                {
                    try
                    {
                        var dir_base = VirtToPhys(
                            SectionHeaders,
                            (UInt32)dirs.CLR.VirtualAddress,
                            out String _);
                        var clr = new ImageCLRDirectory(
                            filebytes,
                            dir_base,
                            SectionHeaders);
                        DataDirectories.Add("CLR", clr);
                    }
                    catch (Exception ex)
                    {
                        warn_msgs.Add(String.Format("Failed to parse CLR directory (packed?): {0}",
                            ex.Message));
                    }
                }
            }
        }


        internal static UInt32 VirtToPhys(
            ImageSectionHeader[] sections,
            UInt32 virt_offset,
            out String sec_name)
        {
            var raw_offset = virt_offset;
            sec_name = null;

            if ((sections is null) || (sections.Length == 0))
                return raw_offset;

            if (virt_offset >= (UInt32)sections[0].VirtualAddress)
            {
                foreach (var sec in sections)
                {
                    var virt_addr = sec.VirtualAddress;
                    var virt_size = (sec.VirtualSize + 0xFFF) & 0xFFFFF000;

                    if ((virt_offset >= virt_addr) && (virt_offset < (virt_addr + virt_size)))
                    {
                        raw_offset = virt_offset - virt_addr + sec.PointerToRawData;
                        sec_name = sec.Name;
                        break;
                    }
                }
            }

            return raw_offset;
        }
    }
}
