using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFileParse.MachO
{
    // 
    // typedef struct __CodeDirectory {
    //     uint32_t magic;
    //     uint32_t length;
    //     uint32_t version;
    //     uint32_t flags;
    //     uint32_t hashOffset;
    //     uint32_t identOffset;
    //     uint32_t nSpecialSlots;
    //     uint32_t nCodeSlots;
    //     uint32_t codeLimit;
    //     uint8_t hashSize;
    //     uint8_t hashType;
    //     uint8_t platform;
    //     uint8_t pageSize;
    //     uint32_t spare2;
    //     char end_earliest[0];
    //     /* Version 0x20100 */
    //     uint32_t scatterOffset;
    //     char end_withScatter[0];
    //     /* Version 0x20200 */
    //     uint32_t teamOffset;
    //     char end_withTeam[0];
    //     /* Version 0x20300 */
    //     uint32_t spare3;
    //     uint64_t codeLimit64;
    //     char end_withCodeLimit64[0];
    //     /* Version 0x20400 */
    //     uint64_t execSegBase;
    //     uint64_t execSegLimit;
    //     uint64_t execSegFlags;
    //     char end_withExecSeg[0];
    //     /* Version 0x20500 */
    //     uint32_t runtime;
    //     uint32_t preEncryptOffset;
    //     char end_withPreEncryptOffset[0];
    //     /* Version 0x20600 */
    //     uint8_t linkageHashType;
    //     uint8_t linkageApplicationType;
    //     uint16_t linkageApplicationSubType;
    //     uint32_t linkageOffset;
    //     uint32_t linkageSize;
    //     char end_withLinkage[0];
    // } CS_CodeDirectory
    //
    public sealed class CSCodeDirectory
    {
    }
}
