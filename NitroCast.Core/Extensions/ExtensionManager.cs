using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NitroCast.Core;

namespace NitroCast.Core.Extensions
{
	/// <summary>
	/// Summary description for PluginHandler.
	/// </summary>
	public sealed class ExtensionManager
	{
		/* *****************************************************************
		 * Plugin manager implements a fully lazy singleton pattern.
		 * ***************************************************************** */
        private string extensionLibraryPath;
        private Dictionary<string, Assembly> extensionAssemblies;
        private Dictionary<string, OutputExtension> outputExtensions;

        public string ExtensionLibraryPath { get { return extensionLibraryPath; } set { extensionLibraryPath = value; } }
        public Dictionary<string, OutputExtension> OutputExtensions { get { return outputExtensions; } }

        private Dictionary<string, Type> objectExtensionTypes;

        public ExtensionManager()
        {
            extensionAssemblies = new Dictionary<string, Assembly>();
            outputExtensions = new Dictionary<string, OutputExtension>();
            objectExtensionTypes = new Dictionary<string, Type>();
        }

		#region Singleton Core

		public static ExtensionManager GetInstance()
		{
			return Nested.instance;
		}

		class Nested
		{
			static Nested()
			{
			}

			internal static readonly ExtensionManager instance = 
                new ExtensionManager();
		}

		#endregion

		#region Initialize

		public void Initialize(string[] fileNames)
		{
            Assembly assembly;
            Type[] typeArray;
            Type type;

            // Attempt to load extensions
            // TODO: LOAD DEFAULT EXTENSIONS FIRST!!! TO SET DEFAULT BUILDERS
			for(int i = 0; i < fileNames.Length; i++)
			{               
                
				assembly = Assembly.LoadFrom(fileNames[i]);
				typeArray = assembly.GetTypes();

                for (int typeIndex = 0; typeIndex < typeArray.Length; 
                    typeIndex++)
                {
                    type = typeArray[typeIndex];
                    AddExtension(type);
				}
			}

            // All Plugins Loaded
            // Setup Type Converters For Builders
            List<ValueTypeBuilder> vBuilders = new List<ValueTypeBuilder>(ValueTypeBuilder.Builders.Count);
            foreach (KeyValuePair<string, ValueTypeBuilder> builder in ValueTypeBuilder.Builders)
                vBuilders.Add(builder.Value);
            TypeConverters.ValueTypeBuilderConverter.Init(vBuilders);

            List<ReferenceTypeBuilder> rBuilders = new List<ReferenceTypeBuilder>(ReferenceTypeBuilder.Builders.Count);
            foreach (KeyValuePair<string, ReferenceTypeBuilder> builder in ReferenceTypeBuilder.Builders)
                rBuilders.Add(builder.Value);
            TypeConverters.ReferenceTypeBuilderConverter.Init(rBuilders);

            List<EnumTypeBuilder> eBuilders = new List<EnumTypeBuilder>(EnumTypeBuilder.Builders.Count);
            foreach (KeyValuePair<string, EnumTypeBuilder> builder in EnumTypeBuilder.Builders)
                eBuilders.Add(builder.Value);
            TypeConverters.EnumTypeBuilderConverter.Init(eBuilders);

            List<ClassFolderBuilder> fBuilders = new List<ClassFolderBuilder>(ClassFolderBuilder.Builders.Count);
            foreach (KeyValuePair<string, ClassFolderBuilder> builder in ClassFolderBuilder.Builders)
                fBuilders.Add(builder.Value);
            TypeConverters.ClassFolderBuilderConverter.Init(fBuilders);

            // Initialize Builders after the extensions are
            // finished loading.
            ValueTypeBuilder.RefreshCache();
		}

        /// <summary>
        /// Adds a plugin to the plugin manager's plugin lists. If the plugin
        /// does not implement ClassOutputPlugin or EnumOutputPlugin then the
        /// type to be added will be ignored.
        /// </summary>
        /// <param name="type">The plugin to add.</param>
        public void AddExtension(Type type)
        {
            ExtensionAttribute attribute;
            object[] attributes;

            attributes = type.GetCustomAttributes(typeof(ExtensionAttribute), 
                false);

            if (type.BaseType != null)
            {
                switch (type.BaseType.Name)
                {
                    case "ValueFieldExtension":
                        {
                            objectExtensionTypes.Add(type.FullName, type);
                            break;
                        }
                    case "ReferenceFieldExtension":
                        {
                            objectExtensionTypes.Add(type.FullName, type);
                            break;
                        }
                    case "EnumFieldExtension":
                        {
                            objectExtensionTypes.Add(type.FullName, type);
                            break;
                        }
                    case "ClassFolderExtension":
                        {
                            objectExtensionTypes.Add(type.FullName, type);
                            break;
                        }
                    case "OutputExtension":
                        {
                            if (attributes.Length > 0)
                            {
                                attribute = (ExtensionAttribute)attributes[0];

                                OutputExtension p = (OutputExtension)
                                    Activator.CreateInstance(type);

                                p.Name = attribute.Name;
                                p.FullName = type.FullName;
                                p.Author = attribute.Author;
                                p.Copyright = attribute.Copyright;
                                p.OutputFileNameFormat =
                                    attribute.OutputFileNameFormat;
                                p.Description = attribute.Description;
                                p.ExtensionPath = attribute.ExtensionPath;
                                p.AssemblyPath = type.Assembly.Location;
                                p.IsWebControl = attribute.IsWebControl;

                                outputExtensions.Add(p.Name, p);
                            }
                            break;
                        }
                    case "ValueTypeBuilder":
                        {
                            attribute = (ExtensionAttribute)attributes[0];

                            ValueTypeBuilder b = (ValueTypeBuilder)
                                Activator.CreateInstance(type);

                            b.Name = attribute.Name;
                            b.FullName = type.FullName;
                            b.Author = attribute.Author;
                            b.Copyright = b.Copyright;
                            b.OutputFileNameFormat = string.Empty;
                            b.Description = attribute.Description;
                            b.ExtensionPath = string.Empty;
                            b.AssemblyPath = type.Assembly.Location;

                            ValueTypeBuilder.Builders.Add(b.Name, b);
                            break;
                        }
                    case "ReferenceTypeBuilder":
                        {
                            attribute = (ExtensionAttribute)attributes[0];

                            ReferenceTypeBuilder b = (ReferenceTypeBuilder)
                                Activator.CreateInstance(type);

                            b.Name = attribute.Name;
                            b.FullName = type.FullName;
                            b.Author = attribute.Author;
                            b.Copyright = b.Copyright;
                            b.OutputFileNameFormat = string.Empty;
                            b.Description = attribute.Description;
                            b.ExtensionPath = string.Empty;
                            b.AssemblyPath = type.Assembly.Location;

                            ReferenceTypeBuilder.Builders.Add(b.Name, b);
                            break;
                        }
                    case "EnumTypeBuilder":
                        {
                            attribute = (ExtensionAttribute)attributes[0];

                            EnumTypeBuilder b = (EnumTypeBuilder)
                                Activator.CreateInstance(type);

                            b.Name = attribute.Name;
                            b.FullName = type.FullName;
                            b.Author = attribute.Author;
                            b.Copyright = b.Copyright;
                            b.OutputFileNameFormat = string.Empty;
                            b.Description = attribute.Description;
                            b.ExtensionPath = string.Empty;
                            b.AssemblyPath = type.Assembly.Location;

                            EnumTypeBuilder.Builders.Add(b.Name, b);
                            break;
                        }
                }
            }
        }

		#endregion

        #region Extension Handling

        public Type GetExtension(string name)
        {
            Type type;

            if(!objectExtensionTypes.TryGetValue(name, out type))
            {
                throw new ExtensionManagerException("Cannot find extension '" + name + "'.");
            }

            return type;
        }

        public void AddExtensions(object obj)
        {
            if (obj is ValueField)
            {
                AddExtensions((ValueField)obj);
                
            }
            else if (obj is ReferenceField)
            {
                AddExtensions((ReferenceField)obj);

            }
            else if (obj is EnumField)
            {
                AddExtensions((EnumField)obj);
            }
        }

        public void AddExtensions(ValueField field)
        {
            foreach (OutputExtension extension in outputExtensions.Values)
            {
                if (extension.ValueFieldExtensionType != null)
                {
                    if (!field.extensions.ContainsKey(extension.ValueFieldExtensionType))
                    {
                        ValueFieldExtension newExtension = (ValueFieldExtension)
                            Activator.CreateInstance(
                            extension.ValueFieldExtensionType);
                        field.extensions.Add(newExtension.GetType(), newExtension);
                    }
                }
            }
        }

        public void AddExtensions(ReferenceField field)
        {
            foreach (OutputExtension extension in outputExtensions.Values)
            {
                if (extension.ReferenceFieldExtensionType != null)
                {
                    if (!field.extensions.ContainsKey(extension.ReferenceFieldExtensionType))
                    {
                        ReferenceFieldExtension newExtension = (ReferenceFieldExtension)
                            Activator.CreateInstance(
                            extension.ReferenceFieldExtensionType);
                        field.extensions.Add(newExtension.GetType(), newExtension);
                    }
                }
            }
        }

        public void AddExtensions(EnumField field)
        {
            foreach (OutputExtension extension in outputExtensions.Values)
            {
                if (extension.EnumFieldExtensionType != null)
                {
                    if (!field.extensions.ContainsKey(extension.EnumFieldExtensionType))
                    {
                        EnumFieldExtension newExtension = (EnumFieldExtension)
                            Activator.CreateInstance(
                            extension.EnumFieldExtensionType);                        
                        field.extensions.Add(newExtension.GetType(), newExtension);
                    }
                }
            }
        }

        #endregion
    }

    public class ExtensionManagerException : System.Exception
    {
        public ExtensionManagerException(string message)
            : base(message)
        {
        }
    }
}