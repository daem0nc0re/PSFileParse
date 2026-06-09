using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.MachO
{
    public sealed class RebaseFSA
    {
        public Int32 Index { get; }
        public RebaseOpcode Opcode { get; }
        public byte Immediate { get; }
        public UInt32 SegmentIndex { get; }
        public UInt64 SegmentOffset { get; }
        public RebaseType Type { get; }
        internal static List<RebaseEntry> Stack { get; } = new List<RebaseEntry>();


        internal RebaseFSA()
        {
            Index = -1;
            Opcode = RebaseOpcode.Done;
            Immediate = 0;
            SegmentIndex = 0;
            SegmentOffset = 0;
            Type = RebaseType.None;
            Stack.Clear();
        }


        internal RebaseFSA(
            byte[] filebytes,
            ref UInt32 offset,
            in RebaseFSA fsa,
            SegmentTableEntry[] segments,
            MachOSection[] sections,
            bool is64bit)
        {
            var ptr_len = is64bit ? 8u : 4u;
            var value = filebytes[offset++];
            Index = fsa.Index + 1;
            Opcode = (RebaseOpcode)(value & (byte)RebaseOpcode.OpcodeMask);
            Immediate = (byte)(value & (byte)RebaseOpcode.ImmediateMask);
            SegmentIndex = fsa.SegmentIndex;
            SegmentOffset = fsa.SegmentOffset;
            Type = fsa.Type;

            if (Opcode == RebaseOpcode.Done)
            {
                SegmentIndex = 0;
                SegmentOffset = 0;
                Type = RebaseType.None;
            }
            else if (Opcode == RebaseOpcode.SetTypeImm)
            {
                if ((Immediate > 0) && (SegmentIndex < 3))
                    Type = (RebaseType)Immediate;
                else
                    Type = RebaseType.None;
            }
            else if (Opcode == RebaseOpcode.SetSegmentAndOffsetULEB)
            {
                SegmentIndex = Immediate;
                SegmentOffset = LEB128.ToUInt64(filebytes, ref offset);
            }
            else if (Opcode == RebaseOpcode.AddAddrULEB)
            {
                SegmentOffset += LEB128.ToUInt64(filebytes, ref offset);
            }
            else if (Opcode == RebaseOpcode.AddAddrImmScaled)
            {
                SegmentOffset += (ptr_len * Immediate);
            }
            else if (Opcode == RebaseOpcode.DoRebaseImmTimes)
            {
                for (byte i = 0; i < Immediate; i++)
                {
                    Stack.Add(new RebaseEntry(
                        filebytes,
                        segments[SegmentIndex],
                        sections,
                        SegmentOffset,
                        Type,
                        is64bit));
                    SegmentOffset += ptr_len;
                }
            }
            else if (Opcode == RebaseOpcode.DoRebaseULEBTimes)
            {
                var repeat_count = LEB128.ToUInt64(filebytes, ref offset);

                for (byte i = 0; i < repeat_count; i++)
                {
                    Stack.Add(new RebaseEntry(
                        filebytes,
                        segments[SegmentIndex],
                        sections,
                        SegmentOffset,
                        Type,
                        is64bit));
                    SegmentOffset += ptr_len;
                }
            }
            else if (Opcode == RebaseOpcode.DoRebaseAddAddrULEB)
            {
                Stack.Add(new RebaseEntry(
                    filebytes,
                    segments[SegmentIndex],
                    sections,
                    SegmentOffset,
                    Type,
                    is64bit));
                SegmentOffset += ptr_len + LEB128.ToUInt64(filebytes, ref offset);
            }
            else if (Opcode == RebaseOpcode.DoRebaseULEBTimesSkippingULEB)
            {
                var repeat_count = LEB128.ToUInt64(filebytes, ref offset);
                var stride = LEB128.ToUInt64(filebytes, ref offset);

                for (byte i = 0; i < repeat_count; i++)
                {
                    Stack.Add(new RebaseEntry(
                        filebytes,
                        segments[SegmentIndex],
                        sections,
                        SegmentOffset,
                        Type,
                        is64bit));
                    SegmentOffset += stride + ptr_len;
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Opcode={0}; Immediate={1}; SegmentIndex={2}; SegmentOffset={3}; Type={4}}}",
                Opcode,
                Immediate,
                SegmentIndex,
                SegmentOffset,
                Type);
        }
    }
}
