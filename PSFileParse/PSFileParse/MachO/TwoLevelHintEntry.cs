using System;
using System.IO;

namespace PSFileParse.MachO
{
    public sealed class TwoLevelHintEntry
    {
        public UInt32 Index { get; }
        public UInt32 SubImageIndex { get; }
        public UInt32 TableOfContentsIndex { get; }
        public SymbolOrdinal Ordinal { get; }
        public String Library { get; }
        public String Name { get; }


        internal TwoLevelHintEntry(
            TwoLevelHint hint,
            SymbolTableEntry[] symbols,
            DysymtabCommand dysymtab,
            DylibTableEntry[] libs)
        {
            Index = hint.Index;
            SubImageIndex = hint.SubImageIndex;
            TableOfContentsIndex = hint.TableOfContentsIndex;

            if ((symbols != null) &&
                (dysymtab != null) &&
                (dysymtab.NumberOfUndefinedSymbols != 0) &&
                (dysymtab.IndexToUndefinedSymbols + Index < (UInt32)symbols.Length))
            {
                var sym = symbols[dysymtab.IndexToUndefinedSymbols + Index];
                Ordinal = (SymbolOrdinal)(((UInt16)sym.Description >> 8) & 0xFF);
                Name = sym.Name;

                if ((Ordinal != SymbolOrdinal.SelfLibrary) &&
                    (Ordinal != SymbolOrdinal.DynamicLookup) &&
                    (Ordinal != SymbolOrdinal.Executable) &&
                    (libs != null))
                {
                    var ord = (byte)Ordinal - 1;

                    if (ord < libs.Length)
                        Library = Path.GetFileName(libs[ord].Name);
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{SubImageIndex={0}; TableOfContentsIndex={1}; Ordinal={2}; Library={3}; Name={4}}}",
                SubImageIndex,
                TableOfContentsIndex,
                Ordinal,
                Library,
                Name);
        }
    }
}
