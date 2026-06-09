using PSFileParse.PE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace PSFileParse.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "PEFileInformation")]
    public sealed class GetPEFileInformationCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ByFile")]
        public String Path { get; set; } = null;


        protected override void ProcessRecord()
        {
            try
            {
                PEFile pe;
                String fullpath = System.IO.Path.GetFullPath(Path);
                byte[] filebytes = File.ReadAllBytes(fullpath);
                WriteVerbose(String.Format("Analyzing '{0}'.", fullpath));
                pe = new PEFile(fullpath, filebytes, out List<String> warn_msgs);

                foreach (var warn in warn_msgs)
                    WriteWarning(warn);

                WriteObject(pe);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex,
                    "PEFile",
                    ErrorCategory.InvalidData,
                    null));
            }
        }
    }
}
