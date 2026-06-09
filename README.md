# PSFileParse
PowerShell module to get file format information as PowerShell objects.

## Objective
I developed this PowerShell module to teach my colleagues about file formats, but it should also be useful for automating simple file analysis.

## Environment
This module should work in the following environments:

* Windows PowerShell 
* PowerShell 7+ (Windows, macOS, Linux)

## Usage
The cmdlets in this module simply convert the results of file format analysis into PowerShell objects, and they accept the file path to be analyzed as the `-Path` parameter.
The naming convention for cmdlets is `Get-<FormatName>FileInformation`.

```
PS C:\> Import-Module path/to/PSFileParse.dll -Verbose
PS C:\> $obj = Get-<FormatName>FileInformation -Path path/to/file.bin -Verbose
```

## Cmdlets
The PowerShell module currently implements the following cmdlets.
I will add cmdlets that parse other file formats, with a focus on executable files.

| Cmdlet | Description |
| :--- | :--- |
| `Get-MachOFileInformation` | Analyze Mach-O file format |
| `Get-PEFileInformation` | Analyze PE file format |

## Special Methods
Some custom object have special methods.

* __`PSFileParse.PE.ImageResourceDirectory.Export(string output_dir)`__:

    This method tries to export data from resource directory in PE file format.
    For example, if you want to export resource directory items as files, use as follows:

    ```
    PS C:\Works> Import-Module C:\Works\PSFileParse.dll -Verbose
    VERBOSE: Importing cmdlet 'Get-MachOFileInformation'.
    VERBOSE: Importing cmdlet 'Get-PEFileInformation'.

    PS C:\Works> $pe = Get-PEFileInformation -Path C:\Windows\SystemResources\shell32.dll.mun -Verbose
    VERBOSE: Analyzing 'C:\Windows\SystemResources\shell32.dll.mun'.

    PS C:\Works> $pe.DataDirectories.Resource.Export("$pwd/shell32_res")

    PS C:\Works> Get-ChildItem -Recurse -Path $pwd/shell32_res -File | %{ $_.FullName }
    C:\Works\shell32_res\AVI\150\1033.bin
    C:\Works\shell32_res\AVI\151\1033.bin
    C:\Works\shell32_res\AVI\152\1033.bin
    C:\Works\shell32_res\AVI\160\1033.bin
    C:\Works\shell32_res\AVI\161\1033.bin
    C:\Works\shell32_res\AVI\162\1033.bin
    C:\Works\shell32_res\AVI\163\1033.bin
    C:\Works\shell32_res\AVI\164\1033.bin
    C:\Works\shell32_res\AVI\165\1033.bin
    C:\Works\shell32_res\AVI\166\1033.bin
    C:\Works\shell32_res\AVI\167\1033.bin
    C:\Works\shell32_res\AVI\168\1033.bin
    C:\Works\shell32_res\AVI\169\1033.bin
    C:\Works\shell32_res\Bitmap\133\1033.dib
    C:\Works\shell32_res\Bitmap\134\1033.dib
    C:\Works\shell32_res\Bitmap\135\1033.dib
    --snip--
    ```

    If you only want the icon files, run as follows:

    ```
    PS C:\Works> $pe.DataDirectories.Resource.Entries | Format-Table
    
    Index Identifier  Content
    ----- ----------  -------
        0 AVI         @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        1 FTR         @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        2 LIBRARY     @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        3 MUI         @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        4 ORDERSTREAM @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        5 TYPELIB     @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        6 UIFILE      @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        7 XML         @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        8 XSD         @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
        9 Cursor      @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
       10 Bitmap      @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
       11 Icon        @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
       12 GroupCursor @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
       13 GroupIcon   @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
       14 Version     @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
       15 Manifest    @{Characteristics=0; TimeDateStamp=1970/01/01 (Thu.) 00:00:00 UTC; MajorVersion=0; MinorVersion=0;...
    
    
    PS C:\Works> $pe.DataDirectories.Resource.Entries[11].Content.Export("$pwd/shell32_icons")
    PS C:\Works> Get-ChildItem -Recurse -Path $pwd/shell32_icons -File | %{ $_.FullName }
    C:\Works\shell32_icons\Icon\1\1033.png
    C:\Works\shell32_icons\Icon\10\1033.dib
    C:\Works\shell32_icons\Icon\100\1033.dib
    C:\Works\shell32_icons\Icon\1000\1033.dib
    C:\Works\shell32_icons\Icon\1001\1033.dib
    C:\Works\shell32_icons\Icon\1002\1033.dib
    C:\Works\shell32_icons\Icon\1003\1033.dib
    C:\Works\shell32_icons\Icon\1004\1033.png
    C:\Works\shell32_icons\Icon\1005\1033.dib
    --snip--
    ```


## References
## Mach-O
* [GitHub - apple-oss-distributions/xnu: EXTERNAL_HEADERS/mach-o](https://github.com/apple-oss-distributions/xnu/tree/main/EXTERNAL_HEADERS/mach-o)
* [GitHub - apple-oss-distributions/xnu: osfmk/mach](https://github.com/apple-oss-distributions/xnu/tree/main/osfmk/mach)
* [GitHub - apple-oss-distributions/cctools](https://github.com/apple-oss-distributions/cctools)
* [GitHub - apple-oss-distributions/dyld](https://github.com/apple-oss-distributions/dyld)

### Portable Executable Format / Common Object File Format
* [Microsoft - PE Format](https://learn.microsoft.com/en-us/windows/win32/debug/pe-format)
* [OS DEV - COFF](https://wiki.osdev.org/COFF)
* [Microsoft SDK Headers (e.g. `winnt.h`, `CorHdr.h`)](https://learn.microsoft.com/en-us/windows/apps/windows-sdk/downloads)
* [Microsoft - windows-docs-rs](https://microsoft.github.io/windows-docs-rs/doc/windows/index.html)
* [Mandiant - Tracking Malware with Import Hashing](https://cloud.google.com/blog/topics/threat-intelligence/tracking-malware-import-hashing)
* [ECMA-335 - Common Language Infrastructure (CLI)](https://ecma-international.org/publications-and-standards/standards/ecma-335/)
* [GitHub - dotnet/runtime](https://github.com/dotnet/runtime)
