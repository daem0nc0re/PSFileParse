using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;
using System.IO;

namespace PSFileParse.MachO
{
    public sealed class BindFSA
    {
        public Int32 Index { get; }
        public BindOpcode Opcode { get; }
        public byte Immediate { get; }
        public UInt32 SegmentIndex { get; }
        public UInt64 SegmentOffset { get; }
        public BindType Type { get; }
        public UInt64 DylibOrdinal { get; }
        public Int64 Addend { get; }
        public bool WeakImport { get; }
        public String Dylib { get; }
        public String Symbol { get; }
        internal static List<BindEntry> Stack { get; } = new List<BindEntry>();


        internal BindFSA()
        {
            Index = -1;
            Opcode = BindOpcode.Done;
            Immediate = 0;
            SegmentIndex = 0;
            SegmentOffset = 0;
            Type = BindType.None;
            DylibOrdinal = 0;
            Addend = 0;
            WeakImport = false;
            Dylib = null;
            Symbol = null;
            Stack.Clear();
        }


        internal BindFSA(
            byte[] filebytes,
            ref UInt32 offset,
            in BindFSA fsa,
            SegmentTableEntry[] segments,
            MachOSection[] sections,
            DylibTableEntry[] dylibs,
            bool is64bit)
        {
            var ptr_len = is64bit ? 8u : 4u;
            var value = filebytes[offset++];
            Index = fsa.Index + 1;
            Opcode = (BindOpcode)(value & (byte)BindOpcode.OpcodeMask);
            Immediate = (byte)(value & (byte)BindOpcode.ImmediateMask);
            SegmentIndex = fsa.SegmentIndex;
            SegmentOffset = fsa.SegmentOffset;
            Type = fsa.Type;
            DylibOrdinal = fsa.DylibOrdinal;
            WeakImport = false;
            Dylib = fsa.Dylib;
            Symbol = fsa.Symbol;
            Addend = fsa.Addend;

            if (Opcode == BindOpcode.Done)
            {
                SegmentIndex = 0;
                SegmentOffset = 0;
                Type = BindType.None;
                DylibOrdinal = 0;
                Dylib = null;
                Symbol = null;
                Addend = 0;
            }
            else if (Opcode == BindOpcode.SetDylibOrdinalImm)
            {
                DylibOrdinal = Immediate;

                if (DylibOrdinal != 0)
                    Dylib = Path.GetFileName(dylibs[DylibOrdinal - 1].Name);
            }
            else if (Opcode == BindOpcode.SetDylibOrdinalULEB)
            {
                DylibOrdinal = LEB128.ToUInt64(filebytes, ref offset);

                if (DylibOrdinal != 0)
                    Dylib = Path.GetFileName(dylibs[DylibOrdinal - 1].Name);
            }
            else if (Opcode == BindOpcode.SetDylibSpecialImm)
            {
                if (Immediate == 0)
                    DylibOrdinal = 0;
                else
                    DylibOrdinal = value;

                if (DylibOrdinal != 0)
                    Dylib = Path.GetFileName(dylibs[DylibOrdinal - 1].Name);
            }
            else if (Opcode == BindOpcode.SetSymbolTrailingFlagsImm)
            {
                WeakImport = ((Immediate & (byte)BindSymbolFlags.WeakImport) != 0);
                Symbol = BinaryHelper.GetUTF8String(filebytes, ref offset);
            }
            else if (Opcode == BindOpcode.SetTypeImm)
            {
                Type = (BindType)Immediate;
            }
            else if (Opcode == BindOpcode.SetAddendSLEB)
            {
                Addend = LEB128.ToInt64(filebytes, ref offset);
            }
            else if (Opcode == BindOpcode.SetSegmentAndOffsetULEB)
            {
                SegmentIndex = Immediate;
                SegmentOffset = LEB128.ToUInt64(filebytes, ref offset);
            }
            else if (Opcode == BindOpcode.AddAddrULEB)
            {
                SegmentOffset += LEB128.ToUInt64(filebytes, ref offset);
            }
            else if (Opcode == BindOpcode.DoBind)
            {
                Stack.Add(new BindEntry(
                    segments[SegmentIndex],
                    sections,
                    SegmentOffset,
                    Type,
                    WeakImport,
                    Dylib,
                    Symbol));
                SegmentOffset += ptr_len;
            }
            else if (Opcode == BindOpcode.DoBindAddAddrULEB)
            {
                Stack.Add(new BindEntry(
                    segments[SegmentIndex],
                    sections,
                    SegmentOffset,
                    Type,
                    WeakImport,
                    Dylib,
                    Symbol));
                SegmentOffset += ptr_len + LEB128.ToUInt64(filebytes, ref offset);
            }
            else if (Opcode == BindOpcode.DoBindAddAddrImmScaled)
            {
                Stack.Add(new BindEntry(
                    segments[SegmentIndex],
                    sections,
                    SegmentOffset,
                    Type,
                    WeakImport,
                    Dylib,
                    Symbol));
                SegmentOffset += ((ptr_len * Immediate) + ptr_len);
            }
            else if (Opcode == BindOpcode.DoBindULEBTimesSkippingULEB)
            {
                var repeat_count = LEB128.ToUInt64(filebytes, ref offset);
                var stride = LEB128.ToUInt64(filebytes, ref offset);

                for (UInt64 i = 0; i < repeat_count; i++)
                {
                    Stack.Add(new BindEntry(
                        segments[SegmentIndex],
                        sections,
                        SegmentOffset,
                        Type,
                        WeakImport,
                        Dylib,
                        Symbol));
                    SegmentOffset += stride + ptr_len;
                }
            }
            else if (Opcode == BindOpcode.Threaded)
            {
                // Not supported
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Opcode={0}; Immediate={1}; SegmentIndex={2}; SegmentOffset={3}; Type={4}; DylibOrdinal={5}; Addend={6}; WeakImport={7}; Dylib={8}; Symbol={9}}}",
                Opcode,
                Immediate,
                SegmentIndex,
                SegmentOffset,
                Type,
                DylibOrdinal,
                Addend,
                WeakImport,
                Dylib,
                Symbol);
        }
    }
}
