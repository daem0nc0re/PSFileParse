using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSFileParse.MachO
{
    public sealed class ExportsTrieEntry
    {
        public UInt64 Size { get; }
        public byte NumberOfChildren { get; }
        public byte[] Value { get; }
        public Dictionary<String, ExportsTrieEntry> Children { get; }


        public ExportsTrieEntry(byte[] trie_data, ref UInt64 offset)
        {
            var children = new Dictionary<String, ExportsTrieEntry>();
            Size = LEB128.ToUInt64(trie_data, ref offset);

            if (Size > 0)
            {
                Value = new byte[Size];

                for (UInt64 i = 0; i < Size; i++)
                    Value[i] = trie_data[offset++];
            }

            NumberOfChildren = trie_data[offset++];

            for (UInt32 i = 0u; i < NumberOfChildren; i++)
            {
                var str = BinaryHelper.GetUTF8String(trie_data, ref offset);
                var next_offset = LEB128.ToUInt64(trie_data, ref offset);
                children.Add(
                    str,
                    new ExportsTrieEntry(trie_data, ref next_offset));
            }

            Children = children;
        }


        public override String ToString()
        {
            return String.Format("@{{Size={0}; NumberOfChildren={1}; Value={2}; Children={3}}}",
                Size,
                NumberOfChildren,
                Value,
                Children);
        }


        internal static Dictionary<String, ExportInfo> Dump(
            ExportsTrieEntry trie)
        {
            var results = new Dictionary<String, ExportInfo>();

            if (trie.Children.Count == 0)
            {
                if (trie.Value != null)
                    results.Add(String.Empty, new ExportInfo(trie.Value));
            }
            else
            {
                Dictionary<String, ExportsTrieEntry> node = trie.Children;

                while (node.Count > 0)
                {
                    var nexts = new Dictionary<String, ExportsTrieEntry>();

                    foreach (var entry in node)
                    {
                        if (entry.Value.Value != null)
                            results.Add(entry.Key, new ExportInfo(entry.Value.Value));

                        if (entry.Value.Children.Count == 0)
                            continue;

                        foreach (var child in entry.Value.Children)
                        {
                            nexts.Add(
                                String.Format("{0}{1}", entry.Key, child.Key),
                                child.Value);
                        }
                    }

                    node = nexts;
                }
            }

            return results.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
