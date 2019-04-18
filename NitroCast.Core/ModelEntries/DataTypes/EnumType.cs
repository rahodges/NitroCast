using System;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DataTypeMap.
	/// </summary>
	[TypeConverter(typeof(TypeConverters.EnumTypeConverter))]
	public class EnumType : IModelEntry
	{
		private string name;							// cache name of child class
		private string nameSpace;						// cache namespace of child class

		private ModelEnum parentEnumEntry;			    // A pointer to the enum entry.
		private ReferenceEntry parentReferenceEntry;	// A pointer to the reference.
        
		/// <summary>
		/// The class name of the child type.
		/// </summary>
		public string Name
		{
			get
			{
				if(parentEnumEntry != null)
					return parentEnumEntry.Name;

				return name;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public ModelEnum ParentEnumEntry
		{
			get
			{
				return parentEnumEntry;
			}
			set
			{
				parentEnumEntry = value;
			}
		}

		public ReferenceEntry ParentReferenceEntry
		{
			get
			{
				return parentReferenceEntry;
			}
			set
			{
				parentReferenceEntry = value;
			}
		}

		/// <summary>
		/// Identifies custom child datatypes which are neither from models or references.
		/// </summary>
		public bool IsCustom
		{
			get
			{
				return parentEnumEntry == null;
			}
		}

		/// <summary>
		/// Identifies datatypes with internal parent class entries in the model.
		/// </summary>
		public bool IsInternal
		{
			get
			{
				return parentEnumEntry != null;
			}
		}

		/// <summary>
		/// Identifies datatypes loaded from a reference.
		/// </summary>
		public bool IsFromReference
		{
			get
			{
				return parentReferenceEntry != null;
			}
		}

		/// <summary>
		/// The namespace the child type belongs to.
		/// </summary>
		public string NameSpace
		{
			get
			{
				if(parentEnumEntry != null)
					return parentEnumEntry.Namespace;

				return nameSpace;
			}
		}

		/// <summary>
		/// The source file of the child class, if it comes from a file.
		/// </summary>
		public string SourceFileName
		{
			get
			{
				if(parentReferenceEntry != null)
					return parentReferenceEntry.FileName;
				if(parentEnumEntry != null)
					return parentEnumEntry.ParentModel.FileName;

				throw new Exception("No source file name exists.");
			}
		}

//		public void MakeInternal(ClassEntry e)
//		{
//			isInternal = true;
//			classEntry = e;
//		}
//
//		public void MakeReference(ClassEntry e)
//		{
//			isFromReference = true;
//			classEntry = e;
//		}

		/// <summary>
		/// Creates a Custom ChildDataType.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameSpace"></param>
		/// <param name="isTableCoded"></param>
		/// <param name="defaultTableName"></param>
		public EnumType (string name, string nameSpace)
		{
			this.name = name;
			this.nameSpace = nameSpace;
			this.parentEnumEntry = null;
			this.parentReferenceEntry = null;
		}

		/// <summary>
		/// Instantiates a ChildDataType. Be sure to set the reference entry if it is loaded from a reference.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="e"></param>
		public EnumType (ModelEnum e, ReferenceEntry r)
		{
			parentEnumEntry = e;
			parentReferenceEntry = r;
		}

		public EnumType (XmlTextReader r)
		{
			if(r.Name != "EnumDataType")
				throw new Exception(string.Format("Cannot load, expected EnumDataType, found '{0}'.",
					r.Name));

			r.MoveToAttribute("IsInternal");
//			isInternal = bool.Parse(r.Value);
            r.MoveToAttribute("Name");
			name = r.Value;
			r.MoveToAttribute("NameSpace");
			nameSpace = r.Value;
            r.MoveToContent();
			r.Read();

			r.ReadElementString("SourceFileName");

			r.ReadEndElement();
		}

		public void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ChildDataType");
			w.WriteAttributeString("IsInternal", IsInternal.ToString());
			w.WriteAttributeString("Name", name);
			w.WriteAttributeString("NameSpace", nameSpace);
			w.WriteElementString("SourceFileName", SourceFileName);

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