using PSFileParse.Auxiliary;
using PSFileParse.MachO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace PSFileParse.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "MachOFileInformation")]
    public sealed class GetMachOFileInformationCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ByFile")]
        public String Path { get; set; } = null;


        protected override void ProcessRecord()
        {
            try
            {
                UInt32 magic;
                Object macho;
                String fullpath = System.IO.Path.GetFullPath(Path);
                byte[] filebytes = File.ReadAllBytes(fullpath);
                WriteVerbose(String.Format("Analyzing '{0}'.", fullpath));

                magic = BinaryHelper.ToUInt32(filebytes, 0);

                if (Enum.IsDefined(typeof(MachOMagic), magic))
                {
                    macho = new MachOFile(fullpath, filebytes);
                }
                else if (Enum.IsDefined(typeof(FatMagic), magic))
                {
                    macho = new MachOFatFile(fullpath, filebytes, out List<String> warns);

                    foreach (var msg in warns)
                        WriteWarning(msg);
                }
                else
                {
                    throw new Exception("Invalid file magic.");
                }

                WriteObject(macho);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex,
                    "MachOFile",
                    ErrorCategory.InvalidData,
                    null));
            }
        }
    }
}
