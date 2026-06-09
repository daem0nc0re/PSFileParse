using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct CLR_METADATA_HEADER
    // {
    //     unsigned int      Signature;       // Should be 0x424A5342 (BSJB)
    //     unsigned short    MajorVersion;
    //     unsigned short    MinorVersion;
    //     unsigned int      Reserved;
    //     unsigned int      Length;
    //     unsigned char     Version[m];      // UTF-8 String
    //     unsigned char     Padding[n];      // Padding for 4 bytes alignment
    //     unsigned short    Flags;           // Must be 0
    //     unsigned short    Streams;         // Number Of Stream Headers
    //     CLR_STREAM_HEADER StreamHeaders[];
    // };
    // 
    public sealed class CLRMetadata
    {
        public FileSignature Signature { get; }
        public UInt16 MajorVersion { get; }
        public UInt16 MinorVersion { get; }
        public UInt32 Reserved { get; }
        public String Version { get; }
        public UInt16 Flags { get; }
        public UInt16 NumberOfStreams { get; }
        public CLRMetadataStream[] Streams { get; }


        public CLRMetadata(byte[] filebytes, UInt32 offset)
        {
            UInt32 str_len;
            Signature = new FileSignature(filebytes, offset, 4);
            MajorVersion = BinaryHelper.ToUInt16(filebytes, offset + 2);
            MinorVersion = BinaryHelper.ToUInt16(filebytes, offset + 4);
            Reserved = BinaryHelper.ToUInt16(filebytes, offset + 8);
            Version = BinaryHelper.GetAnsiString(filebytes, offset + 16);
            str_len = (UInt32)Version.Length + 1u;

            if ((str_len % 4u) != 0)
                str_len += 4u - (str_len % 4u);

            Flags = BinaryHelper.ToUInt16(filebytes, offset + str_len + 16);
            NumberOfStreams = BinaryHelper.ToUInt16(filebytes, offset + str_len + 18);

            if (NumberOfStreams > 0)
            {
                UInt32 header_offset = offset + str_len + 20;
                var idx_array = new UInt32[4]; // 0: #~, 1: #Strings, 2: #GUID, 3: #Blob
                var stream_names = new String[NumberOfStreams];
                var streamhdr_offsets = new UInt32[NumberOfStreams];
                Streams = new CLRMetadataStream[NumberOfStreams];

                for (UInt16 i = 0; i < NumberOfStreams; i++)
                {
                    var name = BinaryHelper.GetAnsiString(
                        filebytes,
                        header_offset + 8);
                    var name_len = (UInt32)name.Length + 1u;

                    if ((name_len % 4u) != 0)
                        name_len += 4u - (name_len % 4u);

                    stream_names[i] = name;
                    streamhdr_offsets[i] = header_offset;
                    header_offset += name_len + 8u;
                }

                for (UInt16 i = 0; i < NumberOfStreams; i++)
                {
                    if (stream_names[i] == "#~")
                    {
                        // Due to reference issue, not #~ stream at this time
                        idx_array[0] = i;
                        continue;
                    }
                    else if (stream_names[i] == "#Strings")
                    {
                        idx_array[1] = i;
                    }
                    else if (stream_names[i] == "#GUID")
                    {
                        idx_array[2] = i;
                    }
                    else if (stream_names[i] == "#Blob")
                    {
                        idx_array[3] = i;
                    }

                    Streams[i] = new CLRMetadataStream(
                        filebytes,
                        streamhdr_offsets[i],
                        i,
                        offset,
                        0,
                        null,
                        0);
                }

                // Parse #~ stream at last
                Streams[idx_array[0]] = new CLRMetadataStream(
                    filebytes,
                    streamhdr_offsets[idx_array[0]],
                    idx_array[0],
                    offset,
                    Streams[idx_array[1]].Offset + offset,
                    (GuidStreamEntry[])Streams[idx_array[2]].Data,
                    Streams[idx_array[3]].Offset + offset);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Signature={0}; MajorVersion={1}; MinorVersion={2}; Reserved={3}; Version={4}; Flags={5}; NumberOfStreams={6}; Streams={7}}}",
                Signature,
                MajorVersion,
                MinorVersion,
                Reserved,
                Version,
                Flags,
                NumberOfStreams,
                Streams);
        }
    }
}
