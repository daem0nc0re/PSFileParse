using PSFileParse.Auxiliary;
using System;

namespace PSFileParse.MachO
{
    //
    // typedef struct __CodeDirectory {
    //     uint32_t magic;                                 /* magic number (CSMAGIC_CODEDIRECTORY) */
    //     uint32_t length;                                /* total length of CodeDirectory blob */
    //     uint32_t version;                               /* compatibility version */
    //     uint32_t flags;                                 /* setup and mode flags */
    //     uint32_t hashOffset;                    /* offset of hash slot element at index zero */
    //     uint32_t identOffset;                   /* offset of identifier string */
    //     uint32_t nSpecialSlots;                 /* number of special hash slots */
    //     uint32_t nCodeSlots;                    /* number of ordinary (code) hash slots */
    //     uint32_t codeLimit;                             /* limit to main image signature range */
    //     uint8_t hashSize;                               /* size of each hash in bytes */
    //     uint8_t hashType;                               /* type of hash (cdHashType* constants) */
    //     uint8_t platform;                               /* platform identifier; zero if not platform binary */
    //     uint8_t pageSize;                               /* log2(page size in bytes); 0 => infinite */
    //     uint32_t spare2;                                /* unused (must be zero) */
    // 
    //     char end_earliest[0];
    // 
    //     /* Version 0x20100 */
    //     uint32_t scatterOffset;                 /* offset of optional scatter vector */
    //     char end_withScatter[0];
    // 
    //     /* Version 0x20200 */
    //     uint32_t teamOffset;                    /* offset of optional team identifier */
    //     char end_withTeam[0];
    // 
    //     /* Version 0x20300 */
    //     uint32_t spare3;                                /* unused (must be zero) */
    //     uint64_t codeLimit64;                   /* limit to main image signature range, 64 bits */
    //     char end_withCodeLimit64[0];
    // 
    //     /* Version 0x20400 */
    //     uint64_t execSegBase;                   /* offset of executable segment */
    //     uint64_t execSegLimit;                  /* limit of executable segment */
    //     uint64_t execSegFlags;                  /* executable segment flags */
    //     char end_withExecSeg[0];
    // 
    //     /* Version 0x20500 */
    //     uint32_t runtime;
    //     uint32_t preEncryptOffset;
    //     char end_withPreEncryptOffset[0];
    // 
    //     /* Version 0x20600 */
    //     uint8_t linkageHashType;
    //     uint8_t linkageApplicationType;
    //     uint16_t linkageApplicationSubType;
    //     uint32_t linkageOffset;
    //     uint32_t linkageSize;
    //     char end_withLinkage[0];
    // 
    //     /* followed by dynamic content as located by offset fields above */
    // } CS_CodeDirectory
    // 
    public sealed class CSCodeDirectory
    {
        public CSMagic Magic { get; }
        public UInt32 Length { get; }
        public UInt32 Version { get; }
        public UInt32 Flags { get; }
        public UInt32 HashOffset { get; }
        public UInt32 IdentOffset { get; }
        public UInt32 NumberOfSpecialSlots { get; }
        public UInt32 NumberOfCodeSlots { get; }
        public UInt32 CodeLimit { get; }
        public byte HashSize { get; }
        public CSHashType HashType { get; }
        public PlatformIdentifier Platform { get; }
        public byte PageSize { get; }
        public UInt32 Spare2 { get; }
        public Object /* UInt32 */ ScatterOffset { get; }
        public Object /* UInt32 */ TeamOffset { get; }
        public Object /* UInt32 */ Spare3 { get; }
        public Object /* UInt64 */ CodeLimit64 { get; }
        public Object /* UInt64 */ ExecSegBase { get; }
        public Object /* UInt64 */ ExecSegLimit { get; }
        public Object /* UInt64 */ ExecSegFlags { get; }
        public Object /* UInt32 */ Runtime { get; }
        public Object /* UInt32 */ PreEncryptOffset { get; }
        public Object /* byte */ LinkageHashType { get; }
        public Object /* byte */ LinkageApplicationType { get; }
        public Object /* UInt16 */ LinkageApplicationSubType { get; }
        public Object /* UInt32 */ LinkageOffset { get; }
        public Object /* UInt32 */ LinkageSize { get; }
        public CSHashBlob CDHash { get; }
        public String Identifier { get; }
        public String TeamIdentifier { get; }
        public CSSpecialSlot[] SpecialSlots { get; }
        public CSCodeSlot[] CodeSlots { get; }


        internal CSCodeDirectory(byte[] filebytes, UInt32 offset)
        {
            Magic = (CSMagic)BinaryHelper.ToUInt32Big(filebytes, offset);
            Length = BinaryHelper.ToUInt32Big(filebytes, offset + 4);
            Version = BinaryHelper.ToUInt32Big(filebytes, offset + 8);
            Flags = BinaryHelper.ToUInt32Big(filebytes, offset + 12);
            HashOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 16);
            IdentOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 20);
            NumberOfSpecialSlots = BinaryHelper.ToUInt32Big(filebytes, offset + 24);
            NumberOfCodeSlots = BinaryHelper.ToUInt32Big(filebytes, offset + 28);
            CodeLimit = BinaryHelper.ToUInt32Big(filebytes, offset + 32);
            HashSize = filebytes[offset + 36];
            HashType = (CSHashType)filebytes[offset + 37];
            Platform = (PlatformIdentifier)filebytes[offset + 38];
            PageSize = filebytes[offset + 39];
            Spare2 = BinaryHelper.ToUInt32Big(filebytes, offset + 40);
            CDHash = new CSHashBlob(filebytes, offset, Length, HashType);

            if (IdentOffset > 0)
                Identifier = BinaryHelper.GetUTF8String(filebytes, offset + IdentOffset);

            if (NumberOfSpecialSlots > 0)
            {
                var slot_offset = offset + HashOffset;
                SpecialSlots = new CSSpecialSlot[NumberOfSpecialSlots];

                for (UInt32 i = 0u; i < NumberOfSpecialSlots; i++)
                {
                    slot_offset -= HashSize;
                    SpecialSlots[i] = new CSSpecialSlot(filebytes, slot_offset, i, HashType, HashSize);
                }
                    
            }

            if (NumberOfCodeSlots > 0)
            {
                var slot_offset = offset + HashOffset;
                CodeSlots = new CSCodeSlot[NumberOfCodeSlots];

                for (UInt32 i = 0u; i < NumberOfCodeSlots; i++)
                {
                    CodeSlots[i] = new CSCodeSlot(filebytes, slot_offset, i, HashType, HashSize);
                    slot_offset += HashSize;
                }
            }

            if (Version >= 0x20100u)
                ScatterOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 44);

            if (Version >= 0x20200u)
            {
                TeamOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 48);

                if ((UInt32)TeamOffset > 0)
                    TeamIdentifier = BinaryHelper.GetUTF8String(filebytes, offset + (UInt32)TeamOffset);
            }

            if (Version >= 0x20300u)
            {
                Spare3 = BinaryHelper.ToUInt32Big(filebytes, offset + 52);
                CodeLimit64 = BinaryHelper.ToUInt64Big(filebytes, offset + 56);
            }

            if (Version >= 0x20400u)
            {
                ExecSegBase = BinaryHelper.ToUInt64Big(filebytes, offset + 64);
                ExecSegLimit = BinaryHelper.ToUInt64Big(filebytes, offset + 72);
                ExecSegFlags = (CSExecSegFlags)BinaryHelper.ToUInt64Big(filebytes, offset + 80);
            }

            if (Version >= 0x20500u)
            {
                Runtime = BinaryHelper.ToUInt32Big(filebytes, offset + 88);
                PreEncryptOffset = BinaryHelper.ToUInt32Big(filebytes, offset + 92);
            }

            if (Version >= 0x20600u)
            {
                LinkageHashType = filebytes[offset + 96];
                LinkageApplicationType = (CSLinkageApplicationType)filebytes[offset + 97];
                LinkageApplicationSubType = (CSLinkageApplicationSubType)BinaryHelper.ToUInt16Big(filebytes, offset + 98);
                LinkageOffset = BinaryHelper.ToUInt32(filebytes, offset + 100);
                LinkageSize = BinaryHelper.ToUInt32(filebytes, offset + 104);
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Magic={0}; Length={1}; Version={2}; Flags={3}; HashOffset={4}; IdentOffset={5}; NumberOfSpecialSlots={6}; NumberOfCodeSlots={7}; CodeLimit={8}; HashSize={9}; HashType={10}; Platform={11}; PageSize={12}; Spare2={13}; ScatterOffset={14}; TeamOffset={15}; Spare3={16}; CodeLimit64={17}; ExecSegBase={18}; ExecSegLimit={19}; ExecSegFlags={20}; Runtime={21}; PreEncryptOffset={22}; LinkageHashType={23}; LinkageApplicationType={24}; LinkageApplicationSubType={25}; LinkageOffset={26}; LinkageSize={27}; CDHash={28}; Identifier={29}; TeamIdentifier= {30}}}",
                Magic,
                Length,
                Version,
                Flags,
                HashOffset,
                IdentOffset,
                NumberOfSpecialSlots,
                NumberOfCodeSlots,
                CodeLimit,
                HashSize,
                HashType,
                Platform,
                PageSize,
                Spare2,
                ScatterOffset,
                TeamOffset,
                Spare3,
                CodeLimit64,
                ExecSegBase,
                ExecSegLimit,
                ExecSegFlags,
                Runtime,
                PreEncryptOffset,
                LinkageHashType,
                LinkageApplicationType,
                LinkageApplicationSubType,
                LinkageOffset,
                LinkageSize,
                CDHash,
                Identifier,
                TeamIdentifier);
        }
    }
}
