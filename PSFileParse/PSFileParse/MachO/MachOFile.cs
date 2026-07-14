using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    public sealed class MachOFile
    {
        public String FileName { get; }
        public UInt32 FileSize { get; }
        public FileHash FileHash { get; }
        public byte[] FileBytes { get; }
        public MachOHeader MachOHeader { get; }
        public LoadCommand[] LoadCommands { get; }
        public SegmentTableEntry[] Segments { get; }
        public MachOSection[] Sections { get; }
        public SymbolTableEntry[] Symbols { get; }
        public Dictionary<String, Object> DynamicSymbols { get; } = new Dictionary<String, Object>();
        public FunctionTableEntry[] Functions { get; }
        public DylibTableEntry[] Dylibs { get; }
        public Dictionary<String, Object> LinkEditData { get; } = new Dictionary<String, Object>();
        public Dictionary<String, Object> DyldInfo { get; } = new Dictionary<String, Object>();
        public TwoLevelHintEntry[] TwoLevelHints { get; }
        public String Entitlements { get; }
        internal static UInt32 SymtabIndex { get; set; }
        internal static UInt32 DysymtabIndex { get; set; }
        internal static UInt32 TwoLevelHintIndex { get; set; }
        internal static UInt32 NumberOfSegments { get; set; }
        internal static UInt32 NumberOfSections { get; set; }
        internal static UInt32 NumberOfModules { get; set; }
        internal static UInt64 ImageBase { get; set; }
        internal static Dictionary<MachOLoadCommands, UInt32> LinkEditIndeces { get; set; }


        internal MachOFile(String filename, byte[] filebytes)
        {
            UInt32 offset = 0u;
            UInt32 dylib_index = 0;
            FileName = filename;
            FileSize = (UInt32)filebytes.Length;
            FileHash = new FileHash(filebytes);
            FileBytes = filebytes;
            SymtabIndex = UInt32.MaxValue;
            DysymtabIndex = UInt32.MaxValue;
            TwoLevelHintIndex = UInt32.MaxValue;
            NumberOfSegments = 0u;
            NumberOfSections = 0u;
            NumberOfModules = 0u;
            ImageBase = 0UL;
            LinkEditIndeces = new Dictionary<MachOLoadCommands, UInt32>();
            MachOHeader = new MachOHeader(filebytes, ref offset, out bool is_bigendian, out bool is64bit);
            LoadCommands = new LoadCommand[MachOHeader.NumberOfLoadCommands];

            for (UInt32 i = 0u; i < (UInt32)LoadCommands.Length; i++)
            {
                LoadCommands[i] = new LoadCommand(
                    filebytes,
                    ref offset,
                    i,
                    MachOHeader.CPUType,
                    is_bigendian);
            }

            //
            // Build Segment / Section Table
            //
            var segment_index = 0u;
            var section_index = 0u;
            Segments = new SegmentTableEntry[NumberOfSegments];
            Sections = new MachOSection[NumberOfSections];

            for (UInt32 i = 0u; i < (UInt32)LoadCommands.Length; i++)
            {
                if (LoadCommands[i].Content.GetType() != typeof(SegmentCommand))
                    continue;

                var segment = (SegmentCommand)LoadCommands[i].Content;
                Segments[segment_index] = new SegmentTableEntry(segment_index, segment);
                segment_index++;

                for (UInt32 j = 0u; j < segment.NumberOfSections; j++)
                {
                    Sections[section_index] = new MachOSection(
                        section_index,
                        segment.Sections[j]);
                    section_index++;
                }
            }

            ImageBase = Sections[0].Address - Sections[0].Offset;

            //
            // Build Dylib Table
            //
            Dylibs = new DylibTableEntry[NumberOfModules];

            for (UInt32 i = 0; i < (UInt32)LoadCommands.Length; i++)
            {
                if (LoadCommands[i].Content.GetType() != typeof(DylibCommand))
                    continue;

                Dylibs[dylib_index] = new DylibTableEntry(
                    dylib_index,
                    (DylibCommand)LoadCommands[i].Content);
                dylib_index++;
            }

            //
            // Build Symbol Table
            //
            if (SymtabIndex != UInt32.MaxValue)
            {
                var symtab = (SymTabCommand)LoadCommands[SymtabIndex].Content;
                var syms = new SymbolTableEntry[symtab.NumberOfSymbols];
                var oft = symtab.SymbolOffset;

                for (UInt32 i = 0; i < symtab.NumberOfSymbols; i++)
                {
                    syms[i] = new SymbolTableEntry(
                        filebytes,
                        ref oft,
                        symtab.StringOffset,
                        i,
                        MachOHeader.Flags,
                        is_bigendian,
                        is64bit);
                }

                Symbols = syms;
            }

            //
            // Build Dynamic Symbol Table
            //
            if (DysymtabIndex != UInt32.MaxValue)
            {
                var symtab = (SymTabCommand)LoadCommands[SymtabIndex].Content;
                var dysymtab = (DysymtabCommand)LoadCommands[DysymtabIndex].Content;

                //
                // Build Indirect Symbol Table
                //
                if ((dysymtab.IndirectSymbolTableOffset != 0) &&
                    (dysymtab.NumberOfIndirectSymbols > 0))
                {
                    var index_offset = dysymtab.IndirectSymbolTableOffset;
                    var indirect_syms = new IndirectSymbolEntry[dysymtab.NumberOfIndirectSymbols];

                    for (UInt32 i = 0u; i < (UInt32)Sections.Length; i++)
                    {
                        if ((Sections[i].Flags.Type != SectionTypes.NonLazySymbolPointers) &&
                            (Sections[i].Flags.Type != SectionTypes.LazySymbolPointers) &&
                            (Sections[i].Flags.Type != SectionTypes.SymbolStubs) &&
                            (Sections[i].Flags.Type != SectionTypes.LazyDylibSymbolPointers) &&
                            (Sections[i].Flags.Type != SectionTypes.ThreadLocalVariablePointers))
                        {
                            continue;
                        }

                        var stride = (Sections[i].Flags.Type == SectionTypes.SymbolStubs) ?
                            Sections[i].Reserved[1] :
                            (is64bit ? 8u : 4u);
                        var last = Sections[i].Reserved[0] + (Sections[i].Size / stride);

                        for (UInt32 j = Sections[i].Reserved[0]; j < last; j++)
                        {
                            indirect_syms[j] = new IndirectSymbolEntry(
                                filebytes,
                                index_offset,
                                j,
                                Symbols,
                                Sections[i],
                                is_bigendian,
                                is64bit);
                        }
                    }

                    DynamicSymbols.Add("Indirects", indirect_syms);
                }

                //
                // Build Reference Symbol Table
                //
                if ((dysymtab.ReferencedSymbolTableOffset != 0) &&
                    (dysymtab.NumberOfReferencedSymbols > 0))
                {
                    var ref_offset = dysymtab.ReferencedSymbolTableOffset;
                    var ref_syms = new DylibReference[dysymtab.NumberOfReferencedSymbols];

                    for (UInt32 i = 0u; i < (UInt32)ref_syms.Length; i++)
                    {
                        ref_syms[i] = new DylibReference(
                            filebytes,
                            ref ref_offset,
                            i,
                            Symbols,
                            is_bigendian);
                    }

                    DynamicSymbols.Add("References", ref_syms);
                }

                //
                // Build Module Table
                //
                if ((dysymtab.ModuleTableOffset != 0) &&
                    (dysymtab.NumberOfModules > 0))
                {
                    var mod_offset = dysymtab.ModuleTableOffset;
                    var modules = new DylibModule[dysymtab.NumberOfModules];

                    for (UInt32 i = 0u; i < (UInt32)modules.Length; i++)
                    {
                        modules[i] = new DylibModule(
                            filebytes,
                            ref mod_offset,
                            symtab.StringOffset,
                            i,
                            is_bigendian,
                            is64bit);
                    }

                    DynamicSymbols.Add("Modules", modules);
                }

                //
                // Build Table Of Contents
                //
                if ((dysymtab.TableOfContentsOffset != 0) &&
                    (dysymtab.NumberOfContents > 0))
                {
                    DylibModule[] modules = null;
                    var toc_offset = dysymtab.TableOfContentsOffset;
                    var tocs = new DylibTableOfContents[dysymtab.NumberOfContents];

                    if (DynamicSymbols.ContainsKey("Modules"))
                        modules = (DylibModule[])DynamicSymbols["Modules"];

                    for (UInt32 i = 0u; i < (UInt32)tocs.Length; i++)
                    {
                        tocs[i] = new DylibTableOfContents(
                            filebytes,
                            ref toc_offset,
                            i,
                            Symbols,
                            modules,
                            is_bigendian);
                    }

                    DynamicSymbols.Add("TableOfContents", tocs);
                }

                //
                // Build Local Relocs
                //
                if ((dysymtab.NumberOfLocalRelocs > 0) &&
                    (dysymtab.LocalRelocOffset > 0))
                {
                    var relocs = new RelocationInfo[dysymtab.NumberOfLocalRelocs];
                    var cursor = dysymtab.LocalRelocOffset;

                    for (UInt32 i = 0u; i < dysymtab.NumberOfLocalRelocs; i++)
                    {
                        relocs[i] = new RelocationInfo(
                            filebytes,
                            ref cursor,
                            i,
                            MachOHeader.CPUType,
                            is_bigendian);
                    }

                    DynamicSymbols.Add("LocalRelocs", relocs);
                }

                //
                // Build External Relocs
                //
                if ((dysymtab.NumberOfExtRelocs > 0) &&
                    (dysymtab.ExtRelocOffset > 0))
                {
                    var relocs = new RelocationInfo[dysymtab.NumberOfExtRelocs];
                    var cursor = dysymtab.ExtRelocOffset;

                    for (UInt32 i = 0u; i < dysymtab.NumberOfExtRelocs; i++)
                    {
                        relocs[i] = new RelocationInfo(
                            filebytes,
                            ref cursor,
                            i,
                            MachOHeader.CPUType,
                            is_bigendian);
                    }

                    DynamicSymbols.Add("ExternalRelocs", relocs);
                }
            }

            //
            // Build link_edit_command data
            //
            foreach (var command in LinkEditIndeces)
            {
                if (command.Key == MachOLoadCommands.FunctionStarts)
                {
                    UInt32 index = 0u;
                    UInt32 cursor = 0u;
                    UInt64 prev_offset = 0u;
                    var info = (LinkEditDataCommand)LoadCommands[command.Value].Content;
                    var funcstarts = new List<FunctionStartsEntry>();

                    while (cursor < info.DataSize)
                    {
                        var entry = new FunctionStartsEntry(info.Data, ref cursor, prev_offset, index++);

                        if (entry.Offset == prev_offset)
                            break;

                        prev_offset = entry.Offset;
                        funcstarts.Add(entry);
                    }

                    LinkEditData.Add(command.Key.ToString(), funcstarts.ToArray());
                }
                else if (command.Key == MachOLoadCommands.DataInCode)
                {
                    UInt32 cursor = 0u;
                    var info = (LinkEditDataCommand)LoadCommands[command.Value].Content;
                    var data_in_codes = new DataInCodeEntry[info.DataSize / 8];

                    for (UInt32 i = 0; i < (UInt32)data_in_codes.Length; i++)
                    {
                        data_in_codes[i] = new DataInCodeEntry(
                            info.Data,
                            ref cursor,
                            i,
                            is_bigendian);
                    }

                    LinkEditData.Add(command.Key.ToString(), data_in_codes);
                }
                else if (command.Key == MachOLoadCommands.DyldExportsTrie)
                {
                    UInt64 trie_base = 0u;
                    var exports_trie = (LinkEditDataCommand)LoadCommands[command.Value].Content;
                    var trie_data = exports_trie.Data;
                    var trie = new ExportsTrieEntry(trie_data, ref trie_base);

                    LinkEditData.Add(
                        command.Key.ToString(),
                        ExportsTrieEntry.Dump(trie));
                }
                else if (command.Key == MachOLoadCommands.DyldChainedFixups)
                {
                    var info = (LinkEditDataCommand)LoadCommands[command.Value].Content;
                    var fixups = new DyldChainedFixups(
                        filebytes,
                        info.DataOffset,
                        Dylibs,
                        Sections,
                        is_bigendian);
                    LinkEditData.Add(command.Key.ToString(), fixups);
                }
                else if (command.Key == MachOLoadCommands.CodeSignature)
                {
                    var info = (LinkEditDataCommand)LoadCommands[command.Value].Content;
                    var signature = new CSBlob(filebytes, info.DataOffset);
                    LinkEditData.Add(command.Key.ToString(), signature);

                    foreach (CSGenericBlobs entry in signature.Blobs)
                    {
                        if (entry.Magic == CSMagic.EmbeddedEntitlements)
                        {
                            Entitlements = System.Text.Encoding.UTF8.GetString(entry.Data);
                            break;
                        }
                    }
                }
            }

            //
            // Build dyld info
            //
            foreach (var cmd in LoadCommands)
            {
                if (cmd.Content.GetType() != typeof(DyldInfoCommand))
                    continue;

                var info = (DyldInfoCommand)cmd.Content;

                if ((info.RebaseOffset != 0) && (info.RebaseSize > 0))
                {
                    var content = new Dictionary<String, Object>();
                    var fsa = new RebaseFSA();
                    var rebases = new List<RebaseFSA>();
                    var rebase_offset = info.RebaseOffset;

                    for (UInt32 i = 0u; i < info.RebaseSize; i++)
                    {
                        var rebase = new RebaseFSA(
                            filebytes,
                            ref rebase_offset,
                            in fsa,
                            Segments,
                            Sections,
                            is64bit);
                        rebases.Add(rebase);

                        if (rebase.Opcode == RebaseOpcode.Done)
                            break;

                        fsa = rebase;
                    }

                    content.Add("Operations", rebases.ToArray());
                    content.Add("Entries", RebaseFSA.Stack.ToArray());
                    DyldInfo.Add("Rebases", content);
                }

                if ((info.BindOffset != 0) && (info.BindSize > 0))
                {
                    var content = new Dictionary<String, Object>();
                    var fsa = new BindFSA();
                    var binds = new List<BindFSA>();
                    var bind_offset = info.BindOffset;
                    var last = info.BindOffset + info.BindSize;

                    for (UInt32 i = 0u; bind_offset < last; i++)
                    {
                        var bind = new BindFSA(
                            filebytes,
                            ref bind_offset,
                            in fsa,
                            Segments,
                            Sections,
                            Dylibs,
                            is64bit);
                        binds.Add(bind);
                        fsa = bind;
                    }

                    content.Add("Operations", binds.ToArray());
                    content.Add("Entries", BindFSA.Stack.ToArray());
                    DyldInfo.Add("Binds", content);
                }

                if ((info.WeakBindOffset != 0) && (info.WeakBindSize > 0))
                {
                    var content = new Dictionary<String, Object>();
                    var fsa = new BindFSA();
                    var binds = new List<BindFSA>();
                    var bind_offset = info.WeakBindOffset;
                    var last = info.WeakBindOffset + info.WeakBindSize;

                    for (UInt32 i = 0u; bind_offset < last; i++)
                    {
                        var bind = new BindFSA(
                            filebytes,
                            ref bind_offset,
                            in fsa,
                            Segments,
                            Sections,
                            Dylibs,
                            is64bit);
                        binds.Add(bind);
                        fsa = bind;
                    }

                    content.Add("Operations", binds.ToArray());
                    content.Add("Entries", BindFSA.Stack.ToArray());
                    DyldInfo.Add("WeakBinds", content);
                }

                if ((info.LazyBindOffset != 0) && (info.LazyBindSize > 0))
                {
                    var content = new Dictionary<String, Object>();
                    var fsa = new BindFSA();
                    var binds = new List<BindFSA>();
                    var bind_offset = info.LazyBindOffset;
                    var last = info.LazyBindOffset + info.LazyBindSize;

                    for (UInt32 i = 0u; bind_offset < last; i++)
                    {
                        var bind = new BindFSA(
                            filebytes,
                            ref bind_offset,
                            in fsa,
                            Segments,
                            Sections,
                            Dylibs,
                            is64bit);
                        binds.Add(bind);
                        fsa = bind;
                    }

                    content.Add("Operations", binds.ToArray());
                    content.Add("Entries", BindFSA.Stack.ToArray());
                    DyldInfo.Add("LazyBinds", content);
                }

                if ((info.ExportOffset != 0) && (info.ExportSize > 0))
                {
                    ExportsTrieEntry trie;
                    UInt64 trie_base = 0u;
                    var trie_data = new byte[info.ExportSize];
                    Array.Copy(filebytes, info.ExportOffset, trie_data, 0, info.ExportSize);
                    trie = new ExportsTrieEntry(trie_data, ref trie_base);
                    DyldInfo.Add("Exports", ExportsTrieEntry.Dump(trie));
                }

                break;
            }

            //
            // Build Function Table
            //
            if (LinkEditData.ContainsKey("FunctionStarts"))
            {
                Dictionary<String, ExportInfo> exports = null;
                var funcstarts = (FunctionStartsEntry[])LinkEditData["FunctionStarts"];
                Functions = new FunctionTableEntry[funcstarts.Length];

                if (LinkEditData.ContainsKey("DyldExportsTrie"))
                    exports = (Dictionary<String, ExportInfo>)LinkEditData["DyldExportsTrie"];
                else if (DyldInfo.ContainsKey("Exports"))
                    exports = (Dictionary<String, ExportInfo>)DyldInfo["Exports"];

                for (UInt32 i = 0u; i < (UInt32)funcstarts.Length; i++)
                    Functions[i] = new FunctionTableEntry(i, funcstarts[i].Offset, ImageBase, exports);
            }

            //
            // Build Two Level Hint Table
            //
            if (TwoLevelHintIndex != UInt32.MaxValue)
            {
                DysymtabCommand dysymtab = null;
                var twolevelhint = (TwoLevelHintCommand)LoadCommands[TwoLevelHintIndex].Content;
                TwoLevelHints = new TwoLevelHintEntry[twolevelhint.NumberOfHints];

                if (DysymtabIndex != UInt32.MaxValue)
                    dysymtab = (DysymtabCommand)LoadCommands[DysymtabIndex].Content;

                foreach (var hint in twolevelhint.Hints)
                {
                    TwoLevelHints[hint.Index] = new TwoLevelHintEntry(
                        hint,
                        Symbols,
                        dysymtab,
                        Dylibs);
                }
            }
        }
    }
}
