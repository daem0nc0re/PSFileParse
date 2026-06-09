using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class TableIndex
    {
        public String Table { get; }
        public Object Index { get; }

        private static Dictionary<MetadataTableIdentifier, bool> WideIndexStatus = new Dictionary<MetadataTableIdentifier, bool>()
        {
            { MetadataTableIdentifier.Module, false },
            { MetadataTableIdentifier.TypeRef, false },
            { MetadataTableIdentifier.TypeDef, false },
            { MetadataTableIdentifier.Field, false },
            { MetadataTableIdentifier.MethodDef, false },
            { MetadataTableIdentifier.Param, false },
            { MetadataTableIdentifier.InterfaceImpl, false },
            { MetadataTableIdentifier.MemberRef, false },
            { MetadataTableIdentifier.Constant, false },
            { MetadataTableIdentifier.CustomAttribute, false },
            { MetadataTableIdentifier.FieldMarshal, false },
            { MetadataTableIdentifier.DeclSecurity, false },
            { MetadataTableIdentifier.ClassLayout, false },
            { MetadataTableIdentifier.FieldLayout, false },
            { MetadataTableIdentifier.StandAloneSig, false },
            { MetadataTableIdentifier.EventMap, false },
            { MetadataTableIdentifier.Event, false },
            { MetadataTableIdentifier.PropertyMap, false },
            { MetadataTableIdentifier.Property, false },
            { MetadataTableIdentifier.MethodSemantics, false },
            { MetadataTableIdentifier.MethodImpl, false },
            { MetadataTableIdentifier.ModuleRef, false },
            { MetadataTableIdentifier.TypeSpec, false },
            { MetadataTableIdentifier.ImplMap, false },
            { MetadataTableIdentifier.FieldRVA, false },
            { MetadataTableIdentifier.Assembly, false },
            { MetadataTableIdentifier.AssemblyProcessor, false },
            { MetadataTableIdentifier.AssemblyOS, false },
            { MetadataTableIdentifier.AssemblyRef, false },
            { MetadataTableIdentifier.AssemblyRefProcessor, false },
            { MetadataTableIdentifier.AssemblyRefOS, false },
            { MetadataTableIdentifier.File, false },
            { MetadataTableIdentifier.ExportedType, false },
            { MetadataTableIdentifier.ManifestResource, false },
            { MetadataTableIdentifier.NestedClass, false },
            { MetadataTableIdentifier.GenericParam, false },
            { MetadataTableIdentifier.MethodSpec, false },
            { MetadataTableIdentifier.GenericParamConstraint, false }
        };


        internal TableIndex(
            byte[] filebytes,
            ref UInt32 offset,
            MetadataTableIdentifier ref_table_id)
        {
            Table = ref_table_id.ToString();

            if (WideIndexStatus[ref_table_id])
            {
                Index = BinaryHelper.ToUInt32(filebytes, offset);
                offset += 4;
            }
            else
            {
                Index = BinaryHelper.ToUInt16(filebytes, offset);
                offset += 2;
            }
        }


        internal static void InitializeWideIndexStatus(
            Dictionary<MetadataTableIdentifier, UInt32> entry_counts)
        {
            var key_ids = new List<MetadataTableIdentifier>(WideIndexStatus.Keys);

            foreach (var id in key_ids)
                WideIndexStatus[id] = false;

            foreach (var entry in entry_counts)
            {
                if (entry.Value > UInt16.MaxValue)
                    WideIndexStatus[entry.Key] = true;
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Table={0}; Index={1}}}", Table, Index);
        }
    }
}
