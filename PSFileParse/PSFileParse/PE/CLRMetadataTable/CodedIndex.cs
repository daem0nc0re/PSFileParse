using PSFileParse.Auxiliary;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE.CLRMetadataTable
{
    public sealed class CodedIndex
    {
        public String Table { get; }
        public Object Index { get; }

        internal enum TypeId
        {
            TypeDefOrRef = 0,
            HasConstant,
            HasCustomAttribute,
            HasFieldMarshal,
            HasDeclSecurity,
            MemberRefParent,
            HasSemantics,
            MethodDefOrRef,
            MemberForwarded,
            Implementation,
            CustomAttributeType,
            ResolutionScope,
            TypeOrMethodDef
        }

        private static readonly Dictionary<TypeId, UInt64> MaximumShortValues = new Dictionary<TypeId, UInt64>
        {
            { TypeId.TypeDefOrRef, 0x3FFF },
            { TypeId.HasConstant, 0x3FFF },
            { TypeId.HasCustomAttribute, 0x07FF },
            { TypeId.HasFieldMarshal, 0x7FFF },
            { TypeId.HasDeclSecurity, 0x3FFF },
            { TypeId.MemberRefParent, 0x1FFF },
            { TypeId.HasSemantics, 0x7FFF },
            { TypeId.MethodDefOrRef, 0x7FFF },
            { TypeId.MemberForwarded, 0x7FFF },
            { TypeId.Implementation, 0x3FFF },
            { TypeId.CustomAttributeType, 0x1FFF },
            { TypeId.ResolutionScope, 0x3FFF },
            { TypeId.TypeOrMethodDef, 0x7FFF }
        };
        private static readonly Dictionary<TypeId, MetadataTableIdentifier[]> ReferencedTables = new Dictionary<TypeId, MetadataTableIdentifier[]>
        {
            {
                TypeId.TypeDefOrRef,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.TypeDef,
                    MetadataTableIdentifier.TypeRef,
                    MetadataTableIdentifier.TypeSpec
                }
            },
            {
                TypeId.HasConstant,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.Field,
                    MetadataTableIdentifier.Param,
                    MetadataTableIdentifier.Property
                }
            },
            {
                TypeId.HasCustomAttribute,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.MethodDef,
                    MetadataTableIdentifier.Field,
                    MetadataTableIdentifier.TypeRef,
                    MetadataTableIdentifier.TypeDef,
                    MetadataTableIdentifier.Param,
                    MetadataTableIdentifier.InterfaceImpl,
                    MetadataTableIdentifier.MemberRef,
                    MetadataTableIdentifier.Module,
                    MetadataTableIdentifier.DeclSecurity,
                    MetadataTableIdentifier.Property,
                    MetadataTableIdentifier.Event,
                    MetadataTableIdentifier.StandAloneSig,
                    MetadataTableIdentifier.ModuleRef,
                    MetadataTableIdentifier.TypeSpec,
                    MetadataTableIdentifier.Assembly,
                    MetadataTableIdentifier.AssemblyRef,
                    MetadataTableIdentifier.File,
                    MetadataTableIdentifier.ExportedType,
                    MetadataTableIdentifier.ManifestResource,
                    MetadataTableIdentifier.GenericParam,
                    MetadataTableIdentifier.GenericParamConstraint,
                    MetadataTableIdentifier.MethodSpec
                }
            },
            {
                TypeId.HasFieldMarshal,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.Field,
                    MetadataTableIdentifier.Param
                }
            },
            {
                TypeId.HasDeclSecurity,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.TypeDef,
                    MetadataTableIdentifier.MethodDef,
                    MetadataTableIdentifier.Assembly
                }
            },
            {
                TypeId.MemberRefParent,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.TypeDef,
                    MetadataTableIdentifier.TypeRef,
                    MetadataTableIdentifier.ModuleRef,
                    MetadataTableIdentifier.MethodDef,
                    MetadataTableIdentifier.TypeSpec
                }
            },
            {
                TypeId.HasSemantics,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.Event,
                    MetadataTableIdentifier.Property
                }
            },
            {
                TypeId.MethodDefOrRef,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.MethodDef,
                    MetadataTableIdentifier.MemberRef
                }
            },
            {
                TypeId.MemberForwarded,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.Field,
                    MetadataTableIdentifier.MethodDef
                }
            },
            {
                TypeId.Implementation,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.File,
                    MetadataTableIdentifier.AssemblyRef,
                    MetadataTableIdentifier.ExportedType
                }
            },
            {
                TypeId.CustomAttributeType,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.MethodDef,
                    MetadataTableIdentifier.MemberRef,
                }
            },
            {
                TypeId.ResolutionScope,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.Module,
                    MetadataTableIdentifier.ModuleRef,
                    MetadataTableIdentifier.AssemblyRef,
                    MetadataTableIdentifier.TypeRef
                }
            },
            {
                TypeId.TypeOrMethodDef,
                new MetadataTableIdentifier[]
                {
                    MetadataTableIdentifier.TypeDef,
                    MetadataTableIdentifier.MethodDef
                }
            }
        };
        private static readonly Dictionary<TypeId, bool> WideIndexStatus = new Dictionary<TypeId, bool>
        {
            { TypeId.TypeDefOrRef, false },
            { TypeId.HasConstant, false },
            { TypeId.HasCustomAttribute, false },
            { TypeId.HasFieldMarshal, false },
            { TypeId.HasDeclSecurity, false },
            { TypeId.MemberRefParent, false },
            { TypeId.HasSemantics, false },
            { TypeId.MethodDefOrRef, false },
            { TypeId.MemberForwarded, false },
            { TypeId.Implementation, false },
            { TypeId.CustomAttributeType, false },
            { TypeId.ResolutionScope, false },
            { TypeId.TypeOrMethodDef, false }
        };


        internal CodedIndex(byte[] filebytes, ref UInt32 offset, TypeId id)
        {
            UInt32 enc_value;
            bool is_wide = WideIndexStatus[id];

            if (is_wide)
            {
                enc_value = BinaryHelper.ToUInt32(filebytes, offset);
                offset += 4u;
            }
            else
            {
                enc_value = BinaryHelper.ToUInt16(filebytes, offset);
                offset += 2u;
            }

            if (id == TypeId.TypeDefOrRef)
            {
                var tag = enc_value & 0x3;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 2) & 0x3FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 2) & 0x3FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.TypeDef.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.TypeRef.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.TypeSpec.ToString();
            }
            else if (id == TypeId.HasConstant)
            {
                var tag = enc_value & 0x3;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 2) & 0x3FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 2) & 0x3FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.Field.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.Param.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.Property.ToString();
            }
            else if (id == TypeId.HasCustomAttribute)
            {
                var tag = enc_value & 0x1F;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 5) & 0x07FFFFFF);
                else
                    Index = (UInt16)((enc_value >> 5) & 0x07FF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.Field.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.TypeRef.ToString();
                else if (tag == 3)
                    Table = MetadataTableIdentifier.TypeDef.ToString();
                else if (tag == 4)
                    Table = MetadataTableIdentifier.Param.ToString();
                else if (tag == 5)
                    Table = MetadataTableIdentifier.InterfaceImpl.ToString();
                else if (tag == 6)
                    Table = MetadataTableIdentifier.MemberRef.ToString();
                else if (tag == 7)
                    Table = MetadataTableIdentifier.Module.ToString();
                else if (tag == 8)
                    Table = MetadataTableIdentifier.DeclSecurity.ToString();
                else if (tag == 9)
                    Table = MetadataTableIdentifier.Property.ToString();
                else if (tag == 10)
                    Table = MetadataTableIdentifier.Event.ToString();
                else if (tag == 11)
                    Table = MetadataTableIdentifier.StandAloneSig.ToString();
                else if (tag == 12)
                    Table = MetadataTableIdentifier.ModuleRef.ToString();
                else if (tag == 13)
                    Table = MetadataTableIdentifier.TypeSpec.ToString();
                else if (tag == 14)
                    Table = MetadataTableIdentifier.Assembly.ToString();
                else if (tag == 15)
                    Table = MetadataTableIdentifier.AssemblyRef.ToString();
                else if (tag == 16)
                    Table = MetadataTableIdentifier.File.ToString();
                else if (tag == 17)
                    Table = MetadataTableIdentifier.ExportedType.ToString();
                else if (tag == 18)
                    Table = MetadataTableIdentifier.ManifestResource.ToString();
                else if (tag == 19)
                    Table = MetadataTableIdentifier.GenericParam.ToString();
                else if (tag == 20)
                    Table = MetadataTableIdentifier.GenericParamConstraint.ToString();
                else if (tag == 21)
                    Table = MetadataTableIdentifier.MethodSpec.ToString();
            }
            else if (id == TypeId.HasFieldMarshal)
            {
                var tag = enc_value & 0x1;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 1) & 0x7FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 1) & 0x7FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.Field.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.Param.ToString();
            }
            else if (id == TypeId.HasDeclSecurity)
            {
                var tag = enc_value & 0x3;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 2) & 0x3FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 2) & 0x3FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.TypeDef.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.Assembly.ToString();
            }
            else if (id == TypeId.MemberRefParent)
            {
                var tag = enc_value & 0x7;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 3) & 0x1FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 3) & 0x1FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.TypeDef.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.TypeRef.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.ModuleRef.ToString();
                else if (tag == 3)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
                else if (tag == 4)
                    Table = MetadataTableIdentifier.TypeSpec.ToString();
            }
            else if (id == TypeId.HasSemantics)
            {
                var tag = enc_value & 0x1;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 1) & 0x7FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 1) & 0x7FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.Event.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.Property.ToString();
            }
            else if (id == TypeId.MethodDefOrRef)
            {
                var tag = enc_value & 0x1;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 1) & 0x7FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 1) & 0x7FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.MemberRef.ToString();
            }
            else if (id == TypeId.MemberForwarded)
            {
                var tag = enc_value & 0x1;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 1) & 0x7FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 1) & 0x7FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.Field.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
            }
            else if (id == TypeId.Implementation)
            {
                var tag = enc_value & 0x3;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 2) & 0x3FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 2) & 0x3FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.File.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.AssemblyRef.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.ExportedType.ToString();
            }
            else if (id == TypeId.CustomAttributeType)
            {
                var tag = enc_value & 0x7;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 3) & 0x1FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 3) & 0x1FFF);

                if (tag == 2)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
                else if (tag == 3)
                    Table = MetadataTableIdentifier.MemberRef.ToString();
            }
            else if (id == TypeId.ResolutionScope)
            {
                var tag = enc_value & 0x3;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 2) & 0x3FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 2) & 0x3FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.Module.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.ModuleRef.ToString();
                else if (tag == 2)
                    Table = MetadataTableIdentifier.AssemblyRef.ToString();
                else if (tag == 3)
                    Table = MetadataTableIdentifier.TypeRef.ToString();
            }
            else if (id == TypeId.TypeOrMethodDef)
            {
                var tag = enc_value & 0x1;

                if (is_wide)
                    Index = (UInt32)((enc_value >> 1) & 0x7FFFFFFF);
                else
                    Index = (UInt16)((enc_value >> 1) & 0x7FFF);

                if (tag == 0)
                    Table = MetadataTableIdentifier.TypeDef.ToString();
                else if (tag == 1)
                    Table = MetadataTableIdentifier.MethodDef.ToString();
            }
        }


        internal static void InitializeWideIndexStatus(
            Dictionary<MetadataTableIdentifier, UInt32> entry_counts)
        {
            var key_ids = new List<TypeId>(WideIndexStatus.Keys);

            foreach (var typeid in key_ids)
                WideIndexStatus[typeid] = false;

            foreach (var typeid in key_ids)
            {
                foreach (var table_type in ReferencedTables[typeid])
                {
                    if (!entry_counts.ContainsKey(table_type))
                        continue;

                    if (entry_counts[table_type] > MaximumShortValues[typeid])
                    {
                        WideIndexStatus[typeid] = true;
                        break;
                    }
                }
            }
        }


        public override String ToString()
        {
            return String.Format("@{{Table={0}; Index={1}}}", Table, Index);
        }
    }
}
