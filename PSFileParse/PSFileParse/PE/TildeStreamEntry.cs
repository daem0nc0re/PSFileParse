using PSFileParse.Auxiliary;
using PSFileParse.PE.CLRMetadataTable;
using System;
using System.Collections.Generic;

namespace PSFileParse.PE
{
    // 
    // struct TILDE_STREAM_HEADER
    // {
    //     unsigned int  Reserved0;
    //     unsigned char MajorVersion;
    //     unsigned char MinorVersion;
    //     unsigned char HeapSizes;
    //     unsigned char Reserved1;
    //     unsigned long long Valid;
    //     unsigned long long Sorted;
    //     unsigned int  Rows[];
    //     unsigned char Tables[1];
    // };
    // 
    public sealed class TildeStreamEntry
    {
        public UInt32 Reserved0 { get; }
        public byte MajorVersion { get; }
        public byte MinorVersion { get; }
        public CLRHeapSizeFlags HeapSizes { get; }
        public byte Reserved1 { get; }
        public MetadataTableFlags Valid { get; }
        public MetadataTableFlags Sorted { get; }
        public Dictionary<String, Object[]> Table { get; }


        internal TildeStreamEntry(
            byte[] filebytes,
            UInt32 offset,
            UInt32 str_table_offset,
            GuidStreamEntry[] guid_table,
            UInt32 blob_table_offset)
        {
            Reserved0 = BinaryHelper.ToUInt32(filebytes, offset);
            MajorVersion = filebytes[offset + 4];
            MinorVersion = filebytes[offset + 5];
            HeapSizes = (CLRHeapSizeFlags)filebytes[offset + 6];
            Reserved1 = filebytes[offset + 7];
            Valid = (MetadataTableFlags)BinaryHelper.ToUInt64(filebytes, offset + 8);
            Sorted = (MetadataTableFlags)BinaryHelper.ToUInt64(filebytes, offset + 16);
            Globals.UseWideStringIndex = (HeapSizes & CLRHeapSizeFlags.UseWideStringIndex) != 0;
            Globals.UseWideGuidIndex = (HeapSizes & CLRHeapSizeFlags.UseWideGuidIndex) != 0;
            Globals.UseWideBlobIndex = (HeapSizes & CLRHeapSizeFlags.UseWideBlobIndex) != 0;

            if (Valid != 0UL)
            {
                var table_types = new List<MetadataTableIdentifier>();
                var table_base = offset + 24u;
                var flags = (UInt64)Valid;
                var counts = new Dictionary<MetadataTableIdentifier, UInt32>();
                Table = new Dictionary<String, Object[]>();

                for (UInt16 i = 0; i < 0x2D; i++)
                {
                    if ((flags & 1) != 0)
                    {
                        table_types.Add((MetadataTableIdentifier)i);
                        counts.Add((MetadataTableIdentifier)i,
                            BinaryHelper.ToUInt32(filebytes, table_base) + 1u);
                        table_base += 4u;
                    }

                    flags >>= 1;
                }

                TableIndex.InitializeWideIndexStatus(counts);
                CodedIndex.InitializeWideIndexStatus(counts);

                foreach (var table_type in table_types)
                {
                    // .NET metadata uses 1 based index. So insert null entry to index 0.
                    var count = counts[table_type];
                    Table.Add(table_type.ToString(), new Object[count]);

                    if (table_type == MetadataTableIdentifier.Module)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ModuleEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                guid_table);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.TypeRef)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new TypeRefEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.TypeDef)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new TypeDefEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.Field)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new FieldEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.MethodDef)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new MethodDefEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.Param)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ParamEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.InterfaceImpl)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new InterfaceImplEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.MemberRef)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new MemberRefEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.Constant)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ConstantEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.CustomAttribute)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new CustomAttributeEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.FieldMarshal)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new FieldMarshalEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.DeclSecurity)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new DeclSecurityEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.ClassLayout)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ClassLayoutEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.FieldLayout)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new FieldLayoutEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.StandAloneSig)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new StandAloneSigEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.EventMap)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new EventMapEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.Event)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new EventEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.PropertyMap)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new PropertyMapEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.Property)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new PropertyEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.MethodSemantics)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new MethodSemanticsEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.MethodImpl)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new MethodImplEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.ModuleRef)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ModuleRefEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.TypeSpec)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new TypeSpecEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.ImplMap)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ImplMapEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.FieldRVA)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new FieldRVAEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.Assembly)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new AssemblyEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.AssemblyProcessor)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new AssemblyProcessorEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.AssemblyOS)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new AssemblyOSEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.AssemblyRef)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new AssemblyRefEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.AssemblyRefProcessor)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new AssemblyRefProcessorEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.AssemblyRefOS)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new AssemblyRefOSEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.File)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new FileEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.ExportedType)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ExportedTypeEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.ManifestResource)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new ManifestResourceEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.NestedClass)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new NestedClassEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.GenericParam)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new GenericParamEntry(
                                filebytes,
                                ref table_base,
                                i,
                                str_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.MethodSpec)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new MethodSpecEntry(
                                filebytes,
                                ref table_base,
                                i,
                                blob_table_offset);
                        }
                    }
                    else if (table_type == MetadataTableIdentifier.GenericParamConstraint)
                    {
                        for (UInt32 i = 1; i < count; i++)
                        {
                            Table[table_type.ToString()][i] = new GenericParamConstraintEntry(
                                filebytes,
                                ref table_base,
                                i);
                        }
                    }
                }
            }
        }
    }
}
