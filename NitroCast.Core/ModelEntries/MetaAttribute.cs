using System;
using System.Xml;

namespace NitroCast.Core
{
	/// <summary>
	/// Extends a the NitroCast schema with attributes for use by plugins.
	/// </summary>
	public class MetaAttribute
	{
		string name;
		string attributeValue;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Value
		{
			get
			{
				return attributeValue;
			}
			set
			{
				attributeValue = value;
			}
		}

        public MetaAttribute()
        {
            name = string.Empty;
            attributeValue = string.Empty;
        }

		public MetaAttribute(string name, string attributeValue)
		{
			this.name = name;
			this.attributeValue = attributeValue;
		}

		public MetaAttribute(XmlReader r)
		{
            if (r.Name == "MetaAttribute")
            {
                r.MoveToAttribute("Name");
                name = r.Value;
                r.MoveToContent();
                r.Read();

                attributeValue = r.ReadElementString("Value");
                r.ReadEndElement();
            }
		}

		public void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("MetaAttribute");
			w.WriteAttributeString("Name", name);
			w.WriteElementString("Value", attributeValue);
			w.WriteEndElement();
		}

		public event EventHandler Updated;
		protected virtual void OnUpdated(EventArgs e)
		{
			if(Updated != null)
				Updated(this, e);
		}
	}
}
