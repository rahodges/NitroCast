using System;
using System.Collections.Generic;
using System.Xml;
using NitroCast.Core.Extensions;

namespace NitroCast.Core
{
    public enum ModelEnumUnderlyingType
    {
        Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64
    }

    /// <summary>
    /// Summary description for EnumEntry.
    /// </summary>
    public class ModelEnum : ICloneable, IComparable, IModelEntry
    {
        string name;
        string nameSpace;
        ModelEnumUnderlyingType underlyingType;
        EnumItemCollection items;

        object editor;
        DataModel parentModel;

        MetaAttributeCollection attributes;
        internal Dictionary<Type, ModelEnumExtension> extended;

        #region Properties

        public string Name { get { return name; } set { name = value; } }
        public string Namespace { get { return nameSpace; } set { nameSpace = value; } }
        public ModelEnumUnderlyingType UnderlyingType { get { return underlyingType; } set { underlyingType = value; } }
        public EnumItemCollection Items { get { return items; } set { items = value; } }
        public MetaAttributeCollection Attributes { get { return attributes; } set { attributes = value; } }
        public DataModel ParentModel { get { return parentModel; } set { parentModel = value; } }
        public object Editor { get { return editor; } set { editor = value; } }

        public ValueType ValueType
        {
            get
            {
                foreach (ValueType type in DataTypeManager.ValueTypes)
                {
                    if (type.DotNetType == UnderlyingType.ToString())
                    {
                        return type;
                    }
                }

                throw (new Exception("Cannot find DotNetDbType for " +
                    UnderlyingType.ToString()));
            }
        }

        #endregion

        public ModelEnum()
        {
            name = string.Empty;
            nameSpace = string.Empty;
            underlyingType = ModelEnumUnderlyingType.Int32;
            attributes = new MetaAttributeCollection();
            editor = null;
            parentModel = null;
            extended = new Dictionary<Type, ModelEnumExtension>();
        }

        public ModelEnum(XmlTextReader r, string version) : this()
        {
            if (r.Name != "EnumObject")
                throw new Exception(string.Format("Cannot load, expected " +
                    "EnumObject, found '{0}'.", r.Name));

            r.MoveToAttribute("Name");
            name = r.Value;
            r.MoveToAttribute("NameSpace");
            nameSpace = r.Value;
            r.MoveToContent();
            r.Read();

            underlyingType = (ModelEnumUnderlyingType)
                Enum.Parse(typeof(ModelEnumUnderlyingType),
                r.ReadElementString("UnderlyingType"));

            if (r.Name == "EnumItems" && !r.IsEmptyElement)
            {
                r.Read();
                while (r.LocalName == "EnumItem" && !r.IsEmptyElement)
                {
                    items.Add(new EnumItem(r, version));
                }
                r.ReadEndElement();
            }

            if (r.Name == "MetaAttributes")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();
                    while (r.LocalName == "MetaAttribute")
                        attributes.Add(new MetaAttribute(r));
                    r.ReadEndElement();
                }
                else
                {
                    r.Read();
                }
            }


            if (r.Name == "Extensions")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();

                    while (r.Name == "Extension")
                    {
                        ModelEnumExtension newExtension = (ModelEnumExtension)
                                ObjectExtension.Build(r, version);
                        extended.Add(newExtension.GetType(), newExtension);
                    }

                    r.ReadEndElement();
                }
                else
                {
                    r.Read();
                }
            }

            r.ReadEndElement();
        }

        public void WriteXml(XmlTextWriter w)
        {
            w.WriteStartElement("EnumObject");
            w.WriteAttributeString("Name", name);
            w.WriteAttributeString("NameSpace", nameSpace);
            w.WriteElementString("UnderlyingType", underlyingType.ToString());

            w.WriteStartElement("MetaAttributes");
            if (attributes != null)
            {
                foreach (MetaAttribute attribute in attributes)
                {
                    attribute.WriteXml(w);
                }
            }
            w.WriteEndElement();

            w.WriteStartElement("Extensions");
            foreach (ModelEnumExtension extension in extended.Values)
            {
                extension.writeXml(w);
            }
            w.WriteEndElement();

            w.WriteEndElement();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public ModelEnum Clone()
        {
            ModelEnum clone = new ModelEnum();
            clone.Name = name;
            clone.UnderlyingType = underlyingType;
            clone.Items = items.Clone();
            return clone;
        }

        int IComparable.CompareTo(object x)
        {
            return name.CompareTo(((ModelEnum)x).name);
        }

        public int CompareTo(ModelEnum x)
        {
            return name.CompareTo(x.name);
        }
    }
}