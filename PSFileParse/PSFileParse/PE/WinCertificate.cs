using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.PE
{
    // 
    // struct WIN_CERTIFICATE
    // {
    //     int           dwLength;
    //     short         wRevision;
    //     short         wCertificateType;   // WIN_CERT_TYPE_xxx
    //     unsigned char bCertificate[ANYSIZE_ARRAY];
    // };
    // 
    public sealed class WinCertificate
    {
        public UInt32 Length { get; }
        public WinCertRevision Revision { get; }
        public WinCertType CertificateType { get; }
        public byte[] Certificate { get; }


        internal WinCertificate(byte[] filebytes, UInt32 offset)
        {
            Length = BinaryHelper.ToUInt32(filebytes, offset);
            Revision = (WinCertRevision)BinaryHelper.ToUInt16(filebytes, offset + 4);
            CertificateType = (WinCertType)BinaryHelper.ToUInt16(filebytes, offset + 6);
            Certificate = new byte[Length - 8];

            for (UInt32 i = 0; i < Length - 8; i++)
                Certificate[i] = filebytes[i + offset + 8];
        }


        public override String ToString()
        {
            return String.Format("@{{Length={0}; Revision={1}; CertificateType={2}; Certificate={3}}}",
                Length,
                Revision,
                CertificateType,
                Certificate);
        }
    }
}
