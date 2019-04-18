using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using NitroCast.Core.Extensions;
using NitroCast.Core.Support;

namespace NitroCast.Core
{
	/// <summary>
	/// Describes a child field for the model, children fields must be NitroCast compatable
	/// children.
	/// </summary>
	public class ReferenceField : ClassField, ICloneable, IComparable
	{
		ReferenceType refType;
        ReferenceTypeBuilder builder;
		
		bool isTableCoded;

		string tableName;			// If the table isn't hard coded, hard code this table name
									// in the child field. If empty, do leave the tablename as a field.
		bool isUnique;				// Is the child field unique? Unique child fields will trigger
									// deletions when the object is deleted.
		bool enableCache;			// caches the objects when loading and saving in collections.

		bool isIndexed;
		bool cascadeSaveEnabled;	// Use to create code that cascades save events to this child.
        bool allowNull = true;
									
        ReferenceMode refMode = ReferenceMode.Normal;

		// Address			vertical placement
		// Address/Private	horizontal placement (forms engine interprets left)
		// Address/Public	horizontal placement (forms engine interprets right)
		string group;						// Field Group for forms engines

        internal Dictionary<Type, ReferenceFieldExtension> extensions;
        private ReadOnlyDictionary<Type, ReferenceFieldExtension> readOnlyExtensions;

		#region Client Editor Defaults Fields

        private bool isClientGridEnabled = false;
		private bool isClientEditEnabled = true;
		private bool isClientViewEnabled = true;		
		private bool isClientCollectionAddEnabled		= false;
		private bool isClientCollectionDeleteEnabled	= false;
		private bool isClientCollectionEditEnabled		= false;

		#endregion

        #region Designer Fields

        public bool IsExpanded;	
		public bool IsSelected;		

		#endregion

		private MetaAttributeCollection attributes;

		#region public properties

        #region Non-Browsable Properties

        [Browsable(false)]
        public bool IsArray
        {
            get
            {
                return refMode == ReferenceMode.Array;
            }
        }

        [Browsable(false)]
        public bool IsCollection
        {
            get
            {
                return refMode == ReferenceMode.Collection;
            }
        }

        //[Browsable(false)]
        //public bool haschildrentables
        //{
        //    get
        //    {
        //        return !(entryType == ChildEntryType.Normal);
        //    }
        //}

        [Browsable(false)]
        public bool HasChildrenTables
        {
            get
            {
                return !(refMode == ReferenceMode.Normal);
            }
        }

        #endregion

        #region Design Properties

        [Category("Design"),
            Description("Specifies the datatype of the child. Only datatypes from the" +
            "current and referenced models are available.")]
        public ReferenceType ReferenceType
        {
            get
            {
                return refType;
            }
            set
            {
                refType = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Design"),
            Description("The builder to use for generating code snippets."),
            DefaultValue(typeof(ReferenceTypeBuilder), "Default")]
        public ReferenceTypeBuilder Builder
        {
            get { return builder; }
            set
            {
                builder = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Design"), 
		    DefaultValue(ReferenceMode.Normal),
		    Description("Specifies whether the child is a single instance, collection or array.")]
		public ReferenceMode ReferenceMode
		{
			get
			{
				return refMode;
			}
			set
			{
				refMode = value;
				OnChanged(EventArgs.Empty);
			}
        }

        [Category("Design"),
            Description("Specifies the group name to place the child in.")]
        public string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
                OnChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region Data Tier Properties

        [Category("Data Tier"), DefaultValue(false),
		Description("Allows the persistance layer to use an index for the child.")]
		public bool IsIndexed
		{
			get
			{
				return isIndexed;
			}
			set
			{
				isIndexed = value;
				OnChanged(EventArgs.Empty);
			}
        }

        [Category("Data Tier"),
            Description("Enables code generator to call save routines for this child when saving the class."),
            DefaultValue(false)]
        public bool CascadeSaveEnabled
        {
            get
            {
                return cascadeSaveEnabled;
            }
            set
            {
                cascadeSaveEnabled = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Data Tier"),
            Description("The table name to pesist the child object to. May only be changed if the selected datatype " +
            "has uncoded table names.")]
        public string TableName
        {
            get
            {
                if (refType != null && refType.IsTableCoded)
                    return refType.DefaultTableName;

                return tableName;
            }
            set
            {
                //if(dataType != null && dataType.IsTableCoded)
                //	throw(new Exception("Cannot set table name, ChildDataType has coded tables."));

                tableName = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Data Tier"),
            DefaultValue(false),
            Description("Informs the persistance layer that this child is unique.")]
        public bool IsUnique
        {
            get
            {
                return isUnique;
            }
            set
            {
                isUnique = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Data Tier"), DefaultValue(false),
            Description("Allows the persistance layer to cache lookups on this object during " +
            "collection processing. This can improve or adversely affect resource usage.")]
        public bool EnableCache
        {
            get
            {
                return enableCache;
            }
            set
            {
                enableCache = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Data Tier"), DefaultValue(true),
            Description("Allows null values.")]
        public bool AllowNull
        {
            get
            {
                return allowNull;
            }
            set
            {
                allowNull = value;
                OnChanged(EventArgs.Empty);
            }
        }

        [Category("Data Tier"), DefaultValue(false),
            Description("Hard codes the table name in the persistence layer.")]
        public bool IsTableCoded
        {
            get
            {
                if (refType != null && refType.IsTableCoded)
                    return true;
                return isTableCoded;
            }
            set
            {
                //if(dataType != null && dataType.IsTableCoded)
                //	throw(new Exception("Cannot set IsTableCoded, ChildDataType has coded tables."));

                isTableCoded = value;
                OnChanged(EventArgs.Empty);
            }
        }

        #endregion

		#region Client Appearance Defaults

        [Category("Client Appearance"),
            DefaultValue(false),
            Description("Allows the field to be viewed in grids; used by code generators.")]
        public bool IsClientGridEnabled
        {
            get
            {
                return isClientGridEnabled;
            }
            set
            {
                isClientGridEnabled = value;
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

        [Category("Client Appearance"),
		DefaultValue(false),
		Description("Enables client side additions of items in children collections.")]
		public bool IsClientCollectionAddEnabled
		{
			get
			{
				return isClientCollectionAddEnabled;
			}
			set
			{
				isClientCollectionAddEnabled = value;
				OnChanged(EventArgs.Empty);
			}
		}

        [Category("Client Appearance"),
		DefaultValue(false),
		Description("Enables client side removals of items in children collections.")]
		public bool IsClientCollectionDeleteEnabled
		{
			get
			{
				return isClientCollectionDeleteEnabled;
			}
			set
			{
				isClientCollectionDeleteEnabled = value;
				OnChanged(EventArgs.Empty);
			}
		}

        [Category("Client Appearance"),
		DefaultValue(false),
		Description("Enables client side edits of items in children collections.")]
		public bool IsClientCollectionEditEnabled
		{
			get
			{
				return isClientCollectionEditEnabled;
			}
			set
			{
				isClientCollectionEditEnabled = value;
				OnChanged(EventArgs.Empty);
			}
		}

		#endregion

        [Browsable(false)]
        public ReadOnlyDictionary<Type, ReferenceFieldExtension> Extensions
        {
            get { return readOnlyExtensions; }
        }

		#endregion
		
		public ReferenceField()
		{
            refMode = ReferenceMode.Normal;
            extensions = new Dictionary<Type, ReferenceFieldExtension>();
            readOnlyExtensions = new ReadOnlyDictionary<Type, ReferenceFieldExtension>(extensions);
            builder = ReferenceTypeBuilder.Default;
		}

		public ReferenceField (XmlTextReader r, string version) : this()
		{
            string refTypeName;
            string refTypeNameSpace;

			if(r.Name != "ChildEntry")
				throw new Exception(string.Format("Cannot load, expected ChildEntry, found '{0}'.",
					r.Name));
			
			r.MoveToAttribute("PropertyName"); 
			this.Name = r.Value;
			r.MoveToContent();
			r.Read();
			
			CustomPrivateName = string.Empty;
			r.ReadElementString("CustomPrivateName");
			CustomColumnName = string.Empty;
			r.ReadElementString("CustomColumnName");
			refTypeName = r.ReadElementString("DataType");
            if (r.Name == "Builder")
                ReferenceTypeBuilder.Builders.TryGetValue(r.ReadElementString("Builder"), out builder);
			refTypeNameSpace = r.ReadElementString("DataTypeNameSpace");
            if (r.LocalName == "AllowNull")
                allowNull = bool.Parse(r.ReadElementString("AllowNull"));
			Caption = r.ReadElementString("Caption");
			Description = r.ReadElementString("Description");
			isTableCoded = bool.Parse(r.ReadElementString("IsTableCoded"));
			tableName = r.ReadElementString("TableName");
			isUnique = bool.Parse(r.ReadElementString("IsUnique"));
			refMode = ParseChildEntryType(r.ReadElementString("ChildEntryType"));
			if(r.LocalName == "Group")
				group = r.ReadElementString("Group");
			if(r.LocalName == "CascadeSaveEnabled")
				cascadeSaveEnabled = bool.Parse(r.ReadElementString("CascadeSaveEnabled"));
			else
				cascadeSaveEnabled = true;
			if(r.LocalName == "EnableCache")
				enableCache = bool.Parse(r.ReadElementString("EnableCache"));
			else
				enableCache = false;
            if (r.LocalName == "IsClientGridEnabled") 
				isClientGridEnabled = bool.Parse(r.ReadElementString("IsClientGridEnabled"));
			if (r.LocalName == "IsClientEditEnabled") 
				isClientEditEnabled = bool.Parse(r.ReadElementString("IsClientEditEnabled"));
			if (r.LocalName == "IsClientViewEnabled") 
				isClientViewEnabled = bool.Parse(r.ReadElementString("IsClientViewEnabled"));
			if (r.LocalName == "IsClientCollectionAddEnabled") 
				isClientCollectionAddEnabled = bool.Parse(r.ReadElementString("IsClientCollectionAddEnabled"));
			if (r.LocalName == "IsClientCollectionDeleteEnabled") 
				isClientCollectionDeleteEnabled = bool.Parse(r.ReadElementString("IsClientCollectionDeleteEnabled"));
			if (r.LocalName == "IsClientCollectionEditEnabled") 
				isClientCollectionEditEnabled = bool.Parse(r.ReadElementString("IsClientCollectionEditEnabled"));

            if (r.Name == "MetaAttributes")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();
                    while (r.LocalName == "MetaAttribute")
                        attributes.Add(new MetaAttribute(r));
                    
                    r.ReadEndElement();
                }
                else { r.Read(); }
            }

            if (r.Name == "Extensions")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();

                    while (r.Name == "Extension")
                    {
                        ReferenceFieldExtension newExtension = (ReferenceFieldExtension)
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

			// Create datatype
			refType = new ReferenceType(refTypeName, refTypeNameSpace);
		}

		public override void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ChildEntry");
			w.WriteAttributeString("PropertyName", Name);
//			w.WriteElementString("CustomNamesEnabled", customNamesEnabled.ToString());
			w.WriteElementString("CustomPrivateName", CustomPrivateName);
			w.WriteElementString("CustomColumnName", CustomColumnName);
			w.WriteElementString("DataType", refType.Name);
            w.WriteElementString("Builder", builder.Name);
			w.WriteElementString("DataTypeNameSpace", refType.NameSpace);
            w.WriteElementString("AllowNull", allowNull.ToString());
			w.WriteElementString("Caption", Caption);
			w.WriteElementString("Description", Description);
			w.WriteElementString("IsTableCoded", isTableCoded.ToString());
			w.WriteElementString("TableName", tableName);
			w.WriteElementString("IsUnique", isUnique.ToString());
			w.WriteElementString("ChildEntryType", refMode.ToString());
			w.WriteElementString("Group", group);
			w.WriteElementString("CascadeSaveEnabled", cascadeSaveEnabled.ToString());
			w.WriteElementString("EnableCache", enableCache.ToString());
            w.WriteElementString("IsClientGridEnabled", isClientGridEnabled.ToString());
			w.WriteElementString("IsClientEditEnabled", isClientEditEnabled.ToString());
			w.WriteElementString("IsClientViewEnabled", isClientViewEnabled.ToString());
			w.WriteElementString("IsClientCollectionAddEnabled", isClientCollectionAddEnabled.ToString());
			w.WriteElementString("IsClientCollectionDeleteEnabled", isClientCollectionDeleteEnabled.ToString());
			w.WriteElementString("IsClientCollectionEditEnabled", isClientCollectionEditEnabled.ToString());
            			
			w.WriteStartElement("MetaAttributes");
			if(attributes != null)
			{
				foreach(MetaAttribute attribute in attributes)
					attribute.WriteXml(w);
			}
			w.WriteEndElement();

            w.WriteStartElement("Extensions");
            foreach (ReferenceFieldExtension extension in extensions.Values)
            {
                extension.writeXml(w);
            }
            w.WriteEndElement();

			w.WriteEndElement();
		}

        public static ReferenceMode ParseChildEntryType(string s)
        {
            switch (s)
            {
                case "Array":
                    return ReferenceMode.Array;
                case "Collection":
                    return ReferenceMode.Collection;
                default:
                    return ReferenceMode.Normal;
            }
        }

		object ICloneable.Clone()
		{
			return Clone();
		}

		public ReferenceField Clone()
		{
			ReferenceField c = new ReferenceField();

			if(c.attributes != null)
				c.attributes = attributes.Clone();

			c.refType = refType;
			c.Name = Name;
			c.CustomPrivateName = CustomPrivateName;
			c.CustomColumnName = CustomColumnName;
			c.refMode = refMode;
			
			c.Caption = Caption;
			c.Description = Description;
			c.refMode = refMode;
			c.isIndexed = isIndexed;
			c.isUnique = isUnique;

			c.isTableCoded = isTableCoded;
			c.tableName = tableName;
			c.group = group;
			c.cascadeSaveEnabled = cascadeSaveEnabled;
			c.enableCache = enableCache;
			c.isClientCollectionAddEnabled = isClientCollectionAddEnabled;
			c.isClientCollectionDeleteEnabled = isClientCollectionDeleteEnabled;
			c.isClientCollectionEditEnabled = isClientCollectionEditEnabled;
			c.isClientEditEnabled = isClientEditEnabled;
			c.isClientGridEnabled = isClientGridEnabled;
			c.isClientViewEnabled = isClientViewEnabled;

			return c;
		}

		#region IComparable Methods

		public int CompareTo(ReferenceField x)
		{
			return Name.CompareTo(x.Name);
		}

		int IComparable.CompareTo(object x)
		{
			return Name.CompareTo(((ReferenceField) x).Name);
		}

		#endregion

		public override string ToString()
		{
			return Name;
		}

        public ReferenceFieldExtension GetExtension(Type type)
        {
            ReferenceFieldExtension extension;

            if (type.BaseType != typeof(ReferenceFieldExtension))
                throw new Exception("Invalid extension type requested.");

            if (!extensions.TryGetValue(type, out extension))
            {
                extension = (ReferenceFieldExtension)
                    Activator.CreateInstance(type);

                extensions.Add(type, extension);
            }

            return extension;
        }
	}

	public enum ReferenceMode: byte
	{
		Normal,
		Collection,
		Array
	}
}
