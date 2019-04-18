using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NitroCast.Core
{
    public class EnumItem : ICloneable, IComparable
    {
        string name;
        string value;

        public string Name { get { return name; } set { name = value; } }
        public string Value { get { return value; } set { this.value = value; } }

        public EnumItem()
        {
            name = string.Empty;
            value = string.Empty;
        }

        public EnumItem(XmlTextReader r, string version)
        {
            if (r.Name != "EnumItem")
            {
                throw (new Exception(string.Format("Cannot load, expected EnumItem, found '{0}'.",
                    r.Name)));
            }

            r.MoveToAttribute("Name");
            name = r.ReadElementString("Name");
            r.MoveToContent();
            r.Read();

            if (r.Name == "Value")
            {
                value = r.ReadElementString("Value");
            }

            r.ReadEndElement();
        }

        public void WriteXml(XmlTextWriter w)
        {
            w.WriteStartElement("EnumItem");
            w.WriteAttributeString("Name", name);
            w.WriteElementString("Value", value);
            w.WriteEndElement();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public EnumItem Clone()
        {
            EnumItem item = new EnumItem();
            item.name = name;
            item.value = value;
            return item;
        }

        int IComparable.CompareTo(object x)
        {
            return name.CompareTo(((EnumItem)x).name);
        }

        public int CompareTo(EnumItem x)
        {
            return name.CompareTo(x.name);
        } 
    }
}
