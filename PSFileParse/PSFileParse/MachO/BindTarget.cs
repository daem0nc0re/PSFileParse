using System;
using System.IO;

namespace PSFileParse.MachO
{
    public sealed class BindTarget
    {
        public String Dylib { get; }
        public String Symbol { get; }


        internal BindTarget(String dylib, String symbol)
        {
            Dylib = Path.GetFileName(dylib);
            Symbol = symbol;
        }


        public override String ToString()
        {
            return String.Format("@{{Dylib={0}; Symbol={1}}}", Dylib, Symbol);
        }
    }
}
