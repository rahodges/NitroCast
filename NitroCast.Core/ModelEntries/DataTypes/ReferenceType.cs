using System;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DataTypeMap.
	/// </summary>
	[TypeConverter(typeof(TypeConverters.ReferenceTypeConverter))]
	public class ReferenceType : IModelEntry
	{
		private string name;							// cache name of child class
		private string nameSpace;						// cache namespace of child class
		private bool isTableCoded;						// If not, the table should have a default table
		private string defaultTableName;				// If empty and table not coded, this tablename is the
														// default tablename used by the ChildDataType

		private ModelClass parentClassEntry;			// A pointer to the class entry.
		private ReferenceEntry parentReferenceEntry;	// A pointer to the reference.
        
		/// <summary>
		/// The class name of the child type.
		/// </summary>
		public string Name
		{
			get
			{
				if(parentClassEntry != null)
					return parentClassEntry.Name;

				return name;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public ModelClass ParentClassEntry
		{
			get
			{
				return parentClassEntry;
			}
			set
			{
				parentClassEntry = value;
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
				return parentClassEntry == null;
			}
		}

		/// <summary>
		/// Identifies datatypes with internal parent class entries in the model.
		/// </summary>
		public bool IsInternal
		{
			get
			{
				return parentClassEntry != null;
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
				if(parentClassEntry != null)
					return parentClassEntry.Namespace;

				return nameSpace;
			}
		}

		/// <summary>
		/// The default table in the database the classes are stored to. If the child
		/// codes table names, this will reflect the child's coded table.
		/// </summary>
		public string DefaultTableName
		{
			get
			{
				if(parentClassEntry != null)
					return parentClassEntry.DefaultTableName;
				return defaultTableName;
			}
		}

		/// <summary>
		/// If the child has coded table names, then this will be true.
		/// </summary>
		public bool IsTableCoded
		{
			get
			{
				if(parentClassEntry != null)
					return parentClassEntry.IsTableCoded;
				return isTableCoded;
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
				if(parentClassEntry != null)
					return parentClassEntry.ParentModel.FileName;

				throw new Exception("No source file name exists.");
			}
		}

//		public void MakeInternal(ClassEntry c)
//		{
//			isInternal = true;
//			classEntry = c;
//		}
//
//		public void MakeReference(ClassEntry c)
//		{
//			isFromReference = true;
//			classEntry = c;
//		}

		/// <summary>
		/// Creates a Custom ChildDataType.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameSpace"></param>
		/// <param name="isTableCoded"></param>
		/// <param name="defaultTableName"></param>
		public ReferenceType (string name, string nameSpace)
		{
			this.name = name;
			this.nameSpace = nameSpace;
			this.parentClassEntry = null;
			this.parentReferenceEntry = null;
		}

		/// <summary>
		/// Instantiates a ChildDataType. Be sure to set the reference entry if it is loaded from a reference.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="c"></param>
		public ReferenceType (ModelClass c, ReferenceEntry r)
		{
			parentClassEntry = c;
			parentReferenceEntry = r;
		}

		public ReferenceType (XmlTextReader r)
		{
			if(r.Name != "ChildDataType")
				throw new Exception(string.Format("Cannot load, expected ChildDataType, found '{0}'.",
					r.Name));

			r.MoveToAttribute("IsInternal");
//			isInternal = bool.Parse(r.Value);
            r.MoveToAttribute("Name");
			name = r.Value;
			r.MoveToAttribute("NameSpace");
			nameSpace = r.Value;
            r.MoveToContent();
			r.Read();

            this.isTableCoded = bool.Parse(r.ReadElementString("IsTableCoded"));
			this.defaultTableName = r.ReadElementString("DefaultTableName");
			r.ReadElementString("SourceFileName");

			r.ReadEndElement();
		}

		public void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ChildDataType");
			w.WriteAttributeString("IsInternal", IsInternal.ToString());
			w.WriteAttributeString("Name", name);
			w.WriteAttributeString("NameSpace", nameSpace);
			w.WriteElementString("IsTableCoded", isTableCoded.ToString());
			w.WriteElementString("DefaultTableName", defaultTableName);
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