using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NitroCast.Core.Extensions
{
    [TypeConverter(typeof(TypeConverters.ClassFolderBuilderConverter))]
    public class ClassFolderBuilder : Extension
    {
        #region BUILDER SUPPORT - DO NOT MESS WITH

        // The typeCache holds precompiled versions of types for
        // rapid code generation. This kind of caching is not 
        // necissary for reference types since reference types
        // are instantly generated.       --- DO NOT CHANGE! ---

        private static ClassFolderBuilder defaultBuilder;
        private static Dictionary<string, ClassFolderBuilder> builders;

        public static Dictionary<string, ClassFolderBuilder> Builders
        {
            get { return builders; }
        }

        public static ClassFolderBuilder Default
        {
            get { return defaultBuilder; }
        }

        static ClassFolderBuilder()
        {
            builders = new Dictionary<string, ClassFolderBuilder>();

            defaultBuilder = new ClassFolderBuilder();
            defaultBuilder.Name = "Default";
            defaultBuilder.FullName = "";
            defaultBuilder.Description = "Default builder for value types.";
            defaultBuilder.Copyright = "Copyright © 2007 Roy A.E. Hodges";
            defaultBuilder.ExtensionPath = string.Empty;
            defaultBuilder.Author = "Roy A.E. Hodges";
            defaultBuilder.AssemblyPath = string.Empty;
            defaultBuilder.OutputFileNameFormat = string.Empty;
            builders.Add(defaultBuilder.Name, defaultBuilder);            
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

        public virtual void CreateClassFields(CodeWriter output,
            ClassFolder folder, bool isInternal, bool instantiate)
        {
            foreach (object item in folder.Items)
            {
                if (item is ValueField)
                {
                    ValueField f = (ValueField)item;
                    f.Builder.CreateClassField(output, f, isInternal, instantiate);
                }
                else if (item is ReferenceField)
                {
                    ReferenceField f = (ReferenceField)item;
                    f.Builder.CreateClassField(output, f, isInternal, instantiate);
                }
                else if (item is EnumField)
                {
                    EnumField f = (EnumField)item;
                    f.Builder.CreateClassField(output, f, isInternal);
                }
            }
        }

        public virtual void CreateControlFields(CodeWriter output,
            ClassFolder folder, bool instantiate)
        {
            foreach (object item in folder.Items)
            {
                if (item is ValueField)
                {
                    ValueField f = (ValueField)item;
                    f.Builder.CreateControlField(output, f, instantiate);
                }
                else if (item is ReferenceField)
                {
                    ReferenceField f = (ReferenceField)item;
                    f.Builder.CreateControlField(output, f, instantiate);
                }
                else if (item is EnumField)
                {
                    EnumField f = (EnumField)item;
                    f.Builder.CreateControlField(output, f, instantiate);
                }
            }
        }

        public void CreateControlProperties(CodeWriter output,
            ClassFolder folder, bool instantiate, bool enableViewState)
        {
            CreateControlProperties(output, folder, instantiate, enableViewState,
                string.Empty);
        }

        public virtual void CreateControlProperties(CodeWriter output,
            ClassFolder folder, bool instantiate, bool enableViewState, 
            string addControlFormat)
        {
            foreach (object item in folder.Items)
            {
                if (item is ValueField)
                {
                    ValueField f = (ValueField)item;
                    f.Builder.CreateControlProperties(output, f, instantiate,
                        enableViewState, addControlFormat);
                    output.WriteLine();
                }
                else if (item is ReferenceField)
                {
                    ReferenceField f = (ReferenceField)item;
                    f.Builder.CreateControlProperties(output, f, instantiate,
                        enableViewState, addControlFormat);
                    output.WriteLine();
                }
                else if (item is EnumField)
                {
                    EnumField f = (EnumField)item;
                    f.Builder.CreateControlProperties(output, f, instantiate,
                        enableViewState, addControlFormat);
                    output.WriteLine();
                }
            }
        }

        public virtual void GetControlValues(CodeWriter output,
            string className, ClassFolder folder)
        {
            foreach (object item in folder.Items)
            {
                if (item is ValueField)
                {
                    ValueField f = (ValueField)item;
                    f.Builder.GetControlValue(output, className, f);
                }
                else if (item is ReferenceField)
                {
                    ReferenceField f = (ReferenceField)item;
                    f.Builder.GetControlValue(output, className, f);
                }
                else if (item is EnumField)
                {
                    EnumField f = (EnumField)item;
                    f.Builder.GetControlValue(output, className, f);
                }
            }
        }

        public virtual void SetControlValues(CodeWriter output,
            string className, ClassFolder folder)
        {
            foreach (object item in folder.Items)
            {
                if (item is ValueField)
                {
                    ValueField f = (ValueField)item;
                    f.Builder.SetControlValue(output, className, f);
                }
                else if (item is ReferenceField)
                {
                    ReferenceField f = (ReferenceField)item;
                    f.Builder.SetControlValue(output, className, f);
                }
                else if (item is EnumField)
                {
                    EnumField f = (EnumField)item;
                    f.Builder.SetControlValue(output, className, f);
                }
            }
        }

        public virtual void CreateControlBinding(CodeWriter output,
            ClassFolder folder)
        {
            foreach (object item in folder.Items)
            {
                if (item is ReferenceField)
                {
                    ReferenceField f = (ReferenceField)item;
                    f.Builder.CreateControlBinding(output, f);
                    output.WriteLine();
                }
                else if(item is EnumField)
                {
                    EnumField f = (EnumField)item;
                    f.Builder.CreateControlBinding(output, f);
                    output.WriteLine();
                }
            }
        }
    }
}