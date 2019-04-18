using System;
using System.Xml;
using System.Reflection;

namespace NitroCast.Core.Extensions
{	
	/// <summary>
	/// Describes a child field for the model, children fields must be NitroCast compatable
	/// children.
	/// </summary>
	public class Extension : ICloneable
	{
		private string name;
		private string fullName;
		private string author;
		private string copyright;
		private string outputFileNameFormat;
		private string description;
		private string extensionPath;
		private string assemblyPath;
		
		#region public properties

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

		public string FullName
		{
			get
			{
				return fullName;
			}
			set
			{
				fullName = value;
			}
		}

		public string Author
		{
			get
			{
				return author;
			}
			set
			{
				author = value;
			}
		}

		public string Copyright
		{
			get
			{
				return copyright;
			}
			set
			{
				copyright = value;
			}
		}

		public string OutputFileNameFormat
		{
			get
			{
				return outputFileNameFormat;
			}
			set
			{
				outputFileNameFormat = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public string ExtensionPath
		{
			get
			{
				return extensionPath;
			}
			set
			{
				extensionPath = value;
			}
		}

		public string AssemblyPath
		{
			get
			{
				return assemblyPath;
			}
			set
			{
				assemblyPath = value;
			}
		}

		#endregion
		
		public Extension()
		{
			
		}

//		public ModelPlugin (XmlTextReader r, DataDataTypeManager DataTypeManager)
//		{
//			if(r.Name != "ChildEntry")
//				throw new Exception(string.Format("Cannot load, expected ChildEntry, found '{0}'.",
//					r.Name));
//			
//			r.MoveToAttribute("PropertyName"); 
//			this.propertyName = r.Value;
//			r.MoveToContent();
//			r.Read();
//
//			customNamesEnabled = bool.Parse(r.ReadElementString("CustomNamesEnabled"));
//			customPrivateName = r.ReadElementString("CustomPrivateName");
//			customColumnName = r.ReadElementString("CustomColumnName");
//            
//			dataType = DataTypeManager.FindChildType(r.ReadElementString("DataType"));
//			caption = r.ReadElementString("Caption");
//			tableName = r.ReadElementString("TableName");
//			isUnique = bool.Parse(r.ReadElementString("IsUnique"));
//			entryType = parseChildEntryType(r.ReadElementString("ChildEntryType"));
//
//			if(r.Name == "PluginAttributes" && !r.IsEmptyElement)
//			{
//				r.Read();
//				while(r.LocalName == "PluginAttribute")
//					attributes.Add(new PluginAttribute(r));
//				r.ReadEndElement();
//			}
//			else
//				r.Read();
//
//			r.ReadEndElement();
//		}

//		public void WriteXml(XmlTextWriter w)
//		{
//			w.WriteStartElement("ChildEntry");
//			w.WriteAttributeString("PropertyName", propertyName);
//			w.WriteElementString("CustomNamesEnabled", customNamesEnabled.ToString());
//			w.WriteElementString("CustomPrivateName", customPrivateName);
//			w.WriteElementString("CustomColumnName", customColumnName);
//			w.WriteElementString("DataType", dataType.Name);
//			w.WriteElementString("Caption", caption);
//			w.WriteElementString("TableName", tableName);
//			w.WriteElementString("IsUnique", isUnique.ToString());
//			w.WriteElementString("ChildEntryType", entryType.ToString());
//			
//			w.WriteStartElement("PluginAttributes");
//			if(attributes != null)
//			{
//				foreach(PluginAttribute attribute in attributes)
//					attribute.WriteXml(w);
//			}
//			w.WriteEndElement();
//
//			w.WriteEndElement();
//		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public Extension Clone()
		{
			Extension p = new Extension();
			
			p.name = name;
			p.fullName = fullName;
			p.author = author;
			p.copyright = copyright;
			p.outputFileNameFormat = outputFileNameFormat;
			p.description = description;
			p.extensionPath = extensionPath;
			p.assemblyPath = assemblyPath;

			return p;
		}

		public event EventHandler Updated;
		protected virtual void OnUpdated(EventArgs e)
		{
			if(Updated != null)
				Updated(this, e);
		}
	}
}
