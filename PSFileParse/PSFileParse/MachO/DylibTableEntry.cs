using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    public sealed class DylibTableEntry
    {
        public UInt32 Index { get; }
        public String Name { get; }
        public UnixTime Timestamp { get; }
        public String CurrentVersion { get; }
        public String CompatibilityVersion { get; }


        internal DylibTableEntry(UInt32 index, DylibCommand dylib_command)
        {
            Index = index;
            Name = dylib_command.Name;
            Timestamp = dylib_command.Timestamp;
            CurrentVersion = dylib_command.CurrentVersion;
            CompatibilityVersion = dylib_command.CompatibilityVersion;
        }


        public override String ToString()
        {
            return String.Format("@{{Name={0}; Timestamp={1}; CurrentVersion={2}; CompatibilityVersion={3}}}",
                Name,
                Timestamp,
                CurrentVersion,
                CompatibilityVersion);
        }
    }
}
