using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using NitroCast.Core.Extensions;
using NitroCast.Core.Support;

namespace NitroCast.Core
{
    public class EnumField : ClassField, ICloneable, IComparable
    {
        EnumType enumType;
        EnumTypeBuilder builder;

        private bool isClientEditEnabled = true;
        private bool isClientViewEnabled = true;	

        internal Dictionary<Type, EnumFieldExtension> extensions;
        private ReadOnlyDictionary<Type, EnumFieldExtension> readOnlyExtensions;

        public EnumType EnumType
        {
            get { return enumType; }
            set { enumType = value; }
        }

        [Category("Design"),
            Description("The builder to use for generating code snippets."),
            DefaultValue(typeof(EnumTypeBuilder), "Default")]
        public EnumTypeBuilder Builder
        {
            get { return builder; }
            set
            {
                builder = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Client Appearance"),
            DefaultValue(true),
            Description("Allows the field to be edited in editors; used by code generators.")]
        public bool IsClientEditEnabled
        {
            get
            {
                return isClientEditEnabled;
            }
            set
            {
                isClientEditEnabled = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Client Appearance"),
            DefaultValue(true),
            Description("Allows the field to be viewed in viewers; used by code generators.")]
        public bool IsClientViewEnabled
        {
            get
            {
                return isClientViewEnabled;
            }
            set
            {
                isClientViewEnabled = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public ReadOnlyDictionary<Type, EnumFieldExtension> Extensions
        {
            get { return readOnlyExtensions; }
        }

        public EnumField()
        {
            extensions = new Dictionary<Type, EnumFieldExtension>();
            readOnlyExtensions = new ReadOnlyDictionary<Type, EnumFieldExtension>(extensions);
            builder = EnumTypeBuilder.Default;
        }

        public EnumField(XmlTextReader r, string version) : this()
        {
            string enumTypeName;
            string enumTypeNameSpace;

            if (r.Name != "EnumField")
            {
                throw (new Exception(string.Format("Cannot load, expected EnumField, found '{0}'.", r.Name)));                
            }

            r.MoveToAttribute("Name");
            this.Name = r.Value;
            r.MoveToAttribute("CustomPrivateName");
            //this.customPrivateName = r.Value;
            CustomPrivateName = string.Empty;
            r.MoveToAttribute("CustomColumnName");
            //this.customColumnName = r.Value;
            CustomColumnName = string.Empty;
            r.MoveToContent();
            r.Read();

            enumTypeName = r.ReadElementString("EnumTypeName");
            if (r.LocalName == "Builder")
                EnumTypeBuilder.Builders.TryGetValue(r.ReadElementString("Builder"), out builder);

            enumTypeNameSpace = r.ReadElementString("EnumTypeNameSpace");
            if(r.Name == "Caption") 
                Caption = r.ReadElementString("Caption");
            if(r.Name == "Description")
                Description = r.ReadElementString("Description");

            if (r.Name == "Extensions")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();

                    while (r.Name == "Extension")
                    {
                            EnumFieldExtension newExtension = (EnumFieldExtension)
                                    ObjectExtension.Build(r, version);
                            extensions.Add(newExtension.GetType(), newExtension);
                    }

                    r.ReadEndElement();
                }
                else
                {
                    r.Read();
                }
            }

            r.ReadEndElement();

            enumType = new EnumType(enumTypeName, enumTypeNameSpace);
        }

        public override void WriteXml(XmlTextWriter w)
        {
            w.WriteStartElement("EnumField");
            w.WriteAttributeString("Name", Name);
            w.WriteAttributeString("CustomPrivateName", CustomPrivateName);
            w.WriteAttributeString("CustomColumnName", CustomColumnName);
            w.WriteElementString("EnumTypeName", enumType.Name);
            w.WriteElementString("Builder", builder.Name);
            w.WriteElementString("EnumTypeNameSpace", enumType.NameSpace);
            w.WriteElementString("Caption", Caption);
            w.WriteElementString("Description", Description);

            w.WriteStartElement("Extensions");
            foreach (EnumFieldExtension extension in extensions.Values)
            {
                extension.writeXml(w);
            }
            w.WriteEndElement();

            w.WriteEndElement();
        }

        public EnumFieldExtension GetExtension(Type type)
        {
            EnumFieldExtension extension;

            if (type.BaseType != typeof(EnumFieldExtension))
                throw new Exception("Invalid extension type requested.");

            if (!extensions.TryGetValue(type, out extension))
            {
                extension = (EnumFieldExtension)
                    Activator.CreateInstance(type);

                extensions.Add(type, extension);
            }

            return extension;
        }

        #region ICloneable Methods

        object ICloneable.Clone()
        {
            return Clone();
        }

        public EnumField Clone()
        {
            EnumField e = new EnumField();
            e.Name = Name;
            e.enumType = enumType;
            e.Caption = Caption;
            e.Description = Description;
            e.CustomColumnName = CustomColumnName;
            e.CustomPrivateName = CustomPrivateName;                        
            return e;
        }

        #endregion

        #region IComparable Methods

        public int CompareTo(EnumField x)
        {
            return Name.CompareTo(x.Name);
        }

        int IComparable.CompareTo(object x)
        {
            return Name.CompareTo(((EnumField)x).Name);
        }

        #endregion
    }
}
