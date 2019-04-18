using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml;

namespace NitroCast.Core.Extensions
{    
    public class ObjectExtension
    {
        public ObjectExtension() { }

        public virtual void ReadXml(XmlTextReader r, string fileversion)
        {
            PropertyDescriptor property;
            PropertyDescriptorCollection properties;

            properties = TypeDescriptor.GetProperties(this.GetType());

            while (r.NodeType != XmlNodeType.EndElement)
            {
                property = properties.Find(r.Name, false);

                if (property != null)
                {
                    property.SetValue(this,
                        property.Converter.ConvertFromString(r.ReadElementString()));
                }
            }
        }

        internal void writeXml(XmlTextWriter w)
        {
            w.WriteStartElement("Extension");
            w.WriteAttributeString("assemblyQualifiedName", this.GetType().AssemblyQualifiedName);
            WriteXml(w);
            w.WriteEndElement();
        }

        public virtual void WriteXml(XmlTextWriter w)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(this.GetType());
            foreach (PropertyDescriptor property in properties)
            {
                w.WriteElementString(property.Name,
                    property.GetValue(this).ToString());
            }
        }

        public static ObjectExtension Build(XmlTextReader r, string fileversion)
        {
            ObjectExtension extension;            
            Type extensionType;

            string[] typeProperties;
            string assembly;
            string typeName;
            string version;
            string culture;
            string publicKeyToken;

            extension = null;

            //"NitroCast.Extensions.Default.WebEditorValueFieldExtension, NitroCast.Extensions.Default, Version=1.7.2963.39843, Culture=neutral, PublicKeyToken=a97020972fc24439"
            
            if (r.Name == "Extension")
            {
                if (!r.IsEmptyElement)
                {
                    r.MoveToAttribute("assemblyQualifiedName");
                    typeProperties = r.Value.Split(new string[] {", " }, StringSplitOptions.RemoveEmptyEntries);

                    typeName = typeProperties[0].Replace("DbModel.Plugins", "NitroCast.Extensions.Default"); // <--- For Backward Compatibility
                    assembly = typeProperties[1];
                    version = typeProperties[2].Split('=')[1];
                    culture = typeProperties[3].Split('=')[1];
                    publicKeyToken = typeProperties[4].Split('=')[1];
                    
                    r.MoveToContent();
                    r.Read();

                    if (typeName.Length > 0 & assembly.Length > 0)
                    {
                        extensionType = ExtensionManager.GetInstance().GetExtension(typeName);
                        extension = (ObjectExtension)Activator.CreateInstance(extensionType);
                        extension.ReadXml(r, fileversion);
                    }
                    else
                    {
                        throw new Exception("Extension improperly formatted.");
                    }

                    // Move forward to end node in case there are properties
                    // that are no longer parsed.
                    while (r.NodeType != XmlNodeType.EndElement)
                        r.Read();
                    r.ReadEndElement();

                    return extension;
                }
                else
                {
                    r.Read();
                }
            }

            return null;
        }
    }
}