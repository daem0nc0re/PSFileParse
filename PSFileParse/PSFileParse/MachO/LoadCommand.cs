using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    //
    // struct load_command {
    //     uint32_t cmd;
    //     uint32_t cmdsize;
    // };
    //
    public sealed class LoadCommand
    {
        public UInt32 Index { get; }
        public MachOLoadCommands Command { get; }
        public UInt32 Size { get; }
        public Object Content { get; }


        internal LoadCommand(
            byte[] filebytes,
            ref UInt32 offset,
            UInt32 index,
            MachCpuType cputype,
            bool is_bigendian)
        {
            Index = index;

            if (is_bigendian)
            {
                Command = (MachOLoadCommands)BinaryHelper.ToUInt32Big(filebytes, offset);
                Size = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            }
            else
            {
                Command = (MachOLoadCommands)BinaryHelper.ToUInt32(filebytes, offset);
                Size = BinaryHelper.ToUInt32(filebytes, offset + 4);
            }

            if (Command == MachOLoadCommands.Segment)
            {
                Content = new SegmentCommand(filebytes, offset + 8u, cputype, is_bigendian, false);
            }
            else if (Command == MachOLoadCommands.SymbolTable)
            {
                Content = new SymTabCommand(filebytes, offset + 8u, is_bigendian);
                MachOFile.SymtabIndex = Index;
            }
            else if (Command == MachOLoadCommands.SymbolSegment)
            {
                Content = new SymSegCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if ((Command == MachOLoadCommands.Thread) ||
                (Command == MachOLoadCommands.UnixThread))
            {
                Content = new ThreadCommand(filebytes, offset + 8u, Size - 8u, cputype, is_bigendian);
            }
            else if ((Command == MachOLoadCommands.LoadFixedVMSharedLib) ||
                (Command == MachOLoadCommands.FixedVMSharedLibIdentification))
            {
                Content = new FvmLibCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.Identification)
            {
                Content = new IdentCommand(filebytes, offset + 8u, Size - 8u);
            }
            else if (Command == MachOLoadCommands.FixedVMFile)
            {
                Content = new FvmFileCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.DynamicSymbolTable)
            {
                Content = new DysymtabCommand(filebytes, offset + 8u, is_bigendian);
                MachOFile.DysymtabIndex = Index;
            }
            else if ((Command == MachOLoadCommands.LoadDylib) ||
                (Command == MachOLoadCommands.DylibIdentification) ||
                (Command == MachOLoadCommands.LoadWeakDylib) ||
                (Command == MachOLoadCommands.ReexportedDylib) ||
                (Command == MachOLoadCommands.LazyLoadDylib) ||
                (Command == MachOLoadCommands.LoadUpwardDylib))
            {
                Content = new DylibCommand(filebytes, offset + 8u, is_bigendian);
                MachOFile.NumberOfModules++;
            }
            else if (
                (Command == MachOLoadCommands.LoadDynamicLinker) ||
                (Command == MachOLoadCommands.DynamicLinkerIdentification) ||
                (Command == MachOLoadCommands.DyldEnvironment))
            {
                Content = new DylinkerCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.PreboundDylib)
            {
                Content = new PreboundDylibCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.Routines)
            {
                Content = new RoutinesCommand(filebytes, offset + 8u, is_bigendian, false);
            }
            else if (Command == MachOLoadCommands.SubFramework)
            {
                Content = new SubFrameworkCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.SubUmbrella)
            {
                Content = new SubUmbrellaCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.SubClient)
            {
                Content = new SubClientCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.SubLibrary)
            {
                Content = new SubLibraryCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.TwoLevelHints)
            {
                Content = new TwoLevelHintCommand(filebytes, offset + 8u, is_bigendian);
                MachOFile.TwoLevelHintIndex = Index;
            }
            else if (Command == MachOLoadCommands.PrebindChecksum)
            {
                Content = new PrebindCksumCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.Segment64)
            {
                Content = new SegmentCommand(filebytes, offset + 8u, cputype, is_bigendian, true);
            }
            else if (Command == MachOLoadCommands.Routines64)
            {
                Content = new RoutinesCommand(filebytes, offset + 8u, is_bigendian, true);
            }
            else if (Command == MachOLoadCommands.UUID)
            {
                Content = new UUIDCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.RuntimePath)
            {
                Content = new RPathCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if ((Command == MachOLoadCommands.CodeSignature) ||
                (Command == MachOLoadCommands.SegmentSplitInfo) ||
                (Command == MachOLoadCommands.FunctionStarts) ||
                (Command == MachOLoadCommands.DataInCode) ||
                (Command == MachOLoadCommands.DylibCodeSignDRs) ||
                (Command == MachOLoadCommands.LinkerOptimizationHint) ||
                (Command == MachOLoadCommands.DyldExportsTrie) ||
                (Command == MachOLoadCommands.DyldChainedFixups) ||
                (Command == MachOLoadCommands.AtomInfo) ||
                (Command == MachOLoadCommands.FunctionVariants) ||
                (Command == MachOLoadCommands.FunctionVariantFixups) ||
                (Command == MachOLoadCommands.LazyLoadDylibInfo))
            {
                Content = new LinkEditDataCommand(filebytes, offset + 8u, is_bigendian);
                MachOFile.LinkEditIndeces.Add(Command, Index);
            }
            else if (Command == MachOLoadCommands.EncryptionInfo)
            {
                Content = new EncryptionInfoCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if ((Command == MachOLoadCommands.DyldInfo) ||
                (Command == MachOLoadCommands.DyldInfoOnly))
            {
                Content = new DyldInfoCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if ((Command == MachOLoadCommands.VersionMinMacOSX) ||
                (Command == MachOLoadCommands.VersionMinIPhoneOS) ||
                (Command == MachOLoadCommands.VersionMinTVOS) ||
                (Command == MachOLoadCommands.VersionMinWatchOS))
            {
                Content = new VersionMinCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.Main)
            {
                Content = new EntryPointCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.SourceVersion)
            {
                Content = new SourceVersionCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.EncryptionInfo64)
            {
                Content = new EncryptionInfoCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.LinkerOption)
            {
                Content = new LinkerOptionCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.Note)
            {
                Content = new NoteCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.BuildVersion)
            {
                Content = new BuildVersionCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.FileSetEntry)
            {
                Content = new FileSetEntryCommand(filebytes, offset + 8u, is_bigendian);
            }
            else if (Command == MachOLoadCommands.TargetTriple)
            {
                Content = new TargetTripleCommand(filebytes, offset + 8u, is_bigendian);
            }
            else
            {
                // Prepage
                var arr = new byte[Size - 8u];
                Array.Copy(filebytes, offset + 8u, arr, 0, arr.Length);
                Content = arr;
            }

            offset += Size;
        }


        public override String ToString()
        {
            return String.Format("@{{Command={0}; Size={1}; Content={2}}}",
                Command,
                Size,
                Content);
        }
    }
}
