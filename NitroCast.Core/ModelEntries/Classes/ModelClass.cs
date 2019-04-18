using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using NitroCast.Core.Extensions;

namespace NitroCast.Core
{
	/// <summary>
	/// Describes a child field for the model, children fields must be NitroCast compatable
	/// children.
	/// </summary>
	public class ModelClass : ICloneable, IComparable, IModelEntry
	{
		public bool IsExpanded;				// Saves state of designer
		public bool IsSelected;				// Saves state of designer

        string name = "MyClass";
        string caption = string.Empty;
        string description = string.Empty;
        string summary = "Summary of MyClass";

        string _namespace = "MyNamespace";
        string interfaces = string.Empty;
        string outputpath = string.Empty;
        string webOutputPath = string.Empty;
        string defaultTableName = string.Empty;		// if empty... hardCodeTables is turned off.
        string toStringOverride = string.Empty;

        bool isTableCoded = false;		            	// if checked... defaultTableName is used to hard code table.
        bool isThreadSafe = false;
        
        bool simpleQueryEnabled = false;        
        ConcurrencyType concurrency = ConcurrencyType.None;

        // Cache Tab
        bool isCachingEnabled = false;
        bool isCollectionCachingEnabled = false;
        TimeSpan cacheLifetime = TimeSpan.FromMinutes(30);        
        TimeSpan collectionCacheLifetime = TimeSpan.FromMinutes(5);
        string cacheClass = "AspCache";
        string cacheName = string.Empty;

        ClassFolderCollection folders = new ClassFolderCollection();
        MetaAttributeCollection attributes = new MetaAttributeCollection();

        object editor = null;
        DataModel parentModel = null;

        ClassOutputConnectorCollection outputConnectors = new ClassOutputConnectorCollection();

        RevisionTag revisionTag;

        internal Dictionary<Type, ModelClassExtension> extended;        

		#region Client Properties

		public string Name
		{
            get
            {
                return name;
            }
            set
            {                
                name = value;
                this.OnUpdated(new ModelClassEventArgs(this));
            }
		}

		public string Namespace
		{
			get { return _namespace; }
			set { _namespace = value; }
		}

		public string Interfaces
		{
			get { return interfaces; }
			set { interfaces = value; }
		}

		public string Caption
		{
			get { return caption; }
			set { caption = value; }
		}

		public string Summary
		{
			get { return summary; }
			set { summary = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public string OutputPath
		{
			get { return outputpath; }
			set { outputpath = value; }
		}

        public string WebOutputPath
        {
            get { return webOutputPath; }
            set { webOutputPath = value; }
        }

		public bool IsTableCoded
		{
			get { return isTableCoded; }
			set { isTableCoded = value; }
		}

		public string DefaultTableName
		{
			get { return defaultTableName; }
			set { defaultTableName = value; }
		}

        public bool SimpleQueryEnabled
        {
            get { return simpleQueryEnabled; }
            set { simpleQueryEnabled = value; }
        }

		public bool IsThreadSafe
		{
			get { return isThreadSafe; }
			set { isThreadSafe = value; }
		}

		public bool IsCachingEnabled
		{
			get { return isCachingEnabled; }
			set { isCachingEnabled = value; }
		}

        public string CacheClass
        {
            get { return cacheClass; }
            set { cacheClass = value; }
        }

        public string CacheName
        {
            get { return cacheName; }
            set { cacheName = value; }
        }

        public TimeSpan CacheLifetime
        {
            get { return cacheLifetime; }
            set { cacheLifetime = value; }
        }

		public bool IsCollectionCachingEnabled
		{
			get { return isCollectionCachingEnabled; }
			set { isCollectionCachingEnabled = value; }
		}

        public TimeSpan CollectionCacheLifetime
        {
            get { return collectionCacheLifetime; }
            set { collectionCacheLifetime = value; }
        }

		public ConcurrencyType Concurrency
		{
			get { return concurrency; }
			set { concurrency = value; }
		}

		public ClassOutputConnectorCollection OutputConnectors
		{
			get { return outputConnectors; }
			set { outputConnectors = value; }
		}

        public string ToStringOverride
        {
            get { return toStringOverride; }
            set { toStringOverride = value; }
        }

		public bool IsCreateDateEnabled
		{
			get 
			{ 
				ClassFolder systemFolder = folders["_system"];
				
				if(systemFolder != null)
					foreach(object i in systemFolder.Items)
						if(i is ValueField && ((ValueField) i).Name == "CreateDate")
							return true;
				
				return false;
			}
			set 
			{
				ClassFolder systemFolder = folders["_system"];

				//Check to make sure CreateDate is in the system folder
				if(value)
				{
					foreach(object i in systemFolder.Items)
						if(i is ValueField && ((ValueField) i).Name == "CreateDate")
							return;
					ValueField createDate = new ValueField();
					createDate.Name = "CreateDate";
					createDate.ValueType = DataTypeManager.ValueTypes["DateTime"];
					createDate.IsNullable = false;
					createDate.DefaultValue = "DateTime.Now";
					createDate.IsClientEditEnabled = false;
					createDate.IsClientViewEnabled = true;
					systemFolder.Items.Add(createDate);				
				}
				else
				{

					int i = -1;
					for(int x = 0; x < systemFolder.Items.Count; x++)
						if(systemFolder.Items[x] is ValueField && 
							((ValueField) systemFolder.Items[x]).Name == "CreateDate")
						{
							i = x;
							break;
						}
					if(i != -1)
						systemFolder.Items.RemoveAt(i);
				}
			}
		}

		public bool IsModifyDateEnabled
		{
			get 
			{
				ClassFolder systemFolder = folders["_system"];

				if(systemFolder != null)
					foreach(object i in systemFolder.Items)
						if(i is ValueField && ((ValueField) i).Name == "ModifyDate")
							return true;

				return false; 
			}
			set { 
				
				//Check to make sure CreateDate is in the system folder
				if(value)				
				{	
					ClassFolder systemFolder = folders["_system"];

					foreach(object i in systemFolder.Items)
						if(i is ValueField && ((ValueField) i).Name == "ModifyDate")
							return;
					ValueField modifyDate = new ValueField();
					modifyDate.Name = "ModifyDate";
					modifyDate.ValueType = DataTypeManager.ValueTypes["DateTime"];
					modifyDate.IsNullable = false;
					modifyDate.DefaultValue = "DateTime.Now";
					modifyDate.IsClientEditEnabled = false;
					modifyDate.IsClientViewEnabled = true;
					systemFolder.Items.Add(modifyDate);
				}
				else
				{
					ClassFolder systemFolder = folders["_system"];

					int i = -1;
					for(int x = 0; x < systemFolder.Items.Count; x++)
						if(systemFolder.Items[x] is ValueField && 
							((ValueField) systemFolder.Items[x]).Name == "ModifyDate")
						{
							i = x;
							break;
						}
					if(i != -1)
						systemFolder.Items.RemoveAt(i);
				}
			}
		}

        public MetaAttributeCollection Attributes
        {
            get
            {
                return attributes;
            }
            set
            {
                attributes = value;
            }
        }

		#endregion

		#region Client Children & Field Properties

		/// <summary>
		/// Accesses the folder collection for the class entry.
		/// </summary>
		public ClassFolderCollection Folders
		{
			get { return folders; }
		}

		/// <summary>
		/// Property that access ALL children in all folders
		/// </summary>
		public ReferenceFieldCollection ReferenceFields
		{
			get
			{
				ReferenceFieldCollection c = new ReferenceFieldCollection();
				foreach(ClassFolder folder in folders)
					foreach(object i in folder.Items)
						if(i is ReferenceField)
							c.Add((ReferenceField) i);
				return c;
			}
		}

		/// <summary>
		/// Property that accesses ALL fields in all folders
		/// </summary>
		public ValueFieldCollection ValueFields
		{
			get
			{
				ValueFieldCollection c = new ValueFieldCollection();
				foreach(ClassFolder folder in folders)
					foreach(object i in folder.Items)
						if(i is ValueField)
							c.Add((ValueField) i);
				return c;
			}
		}

        /// <summary>
        /// Property that accesses ALL fields in all folders
        /// </summary>
        public EnumFieldCollection EnumFields
        {
            get
            {
                EnumFieldCollection c = new EnumFieldCollection();
                foreach (ClassFolder folder in folders)
                    foreach (object i in folder.Items)
                        if (i is EnumField)
                            c.Add((EnumField)i);
                return c;
            }
        }

		#endregion

		#region Model Properties - Should not be visible to client

		public DataModel ParentModel
		{
			get
			{
				return parentModel;
			}
			set
			{
				parentModel = value;
			}
		}

		/// <summary>
		/// Gets and sets the Class Entry's associated editor.
		/// </summary>
		public object Editor
		{
			get
			{
				return editor;
			}
			set
			{
				editor = value;                
			}
		}

		#endregion

		#region Model Aggregate Properties

		public string[] Namespaces
		{
			get
			{
				ArrayList namespaces = new ArrayList();
				bool addNamespace;
				foreach(ReferenceField c in ReferenceFields)
				{
					addNamespace = true;

					foreach(object o in namespaces)
						addNamespace &= (string) o == c.ReferenceType.NameSpace;

					if(addNamespace)
						namespaces.Add(c.ReferenceType.NameSpace);
				}

				return (string[]) namespaces.ToArray(Type.GetType("String"));
			}
		}

		public string PrivateName
		{
			get { return name.Substring(0,1).ToLower() + name.Substring(1, name.Length - 1); }
		}

        public bool IsParitioned
        {
            get
            {
                foreach (ClassFolder folder in this.Folders)
                {
                    if (folder.IsPartition)
                        return true;
                }

                return false;
            }
        }

		#endregion

        public RevisionTag RevisionTag
        {
            get { return revisionTag; }
            set { revisionTag = value; }
        }
		
		#region Model Events

        public event ModelClassEventHandler Updated;
        protected virtual void OnUpdated(ModelClassEventArgs e)
        {
            if (Updated != null)
                Updated(this, e);
        }

		#endregion

		#region Model Methods


		#endregion
		
		public ModelClass()
		{
			EnsureSystemFolderExists();
            extended = new Dictionary<Type, ModelClassExtension>();
		}

        public ValueField GetFirstUserValueField()
        {
            ValueField f;

            foreach (ClassFolder folder in Folders)
            {
                if (folder.Name != "_system")
                {
                    foreach (object item in folder.Items)
                    {
                        if (item is ValueField)
                        {
                            f = (ValueField)item;
                            if (f.ValueType.DotNetType == "string")
                                return f;
                        }
                    }
                }
            }

            return null;
        }

        public ModelClass(XmlTextReader r, string version)
        {
            extended = new Dictionary<Type, ModelClassExtension>();

            // BE SURE TO CREATE SYSTEM FOLDER!

            if (r.Name != "ClassObject")
                throw new Exception(string.Format("Source file does not match NitroCast DTD; " +
                    "expected 'ClassModel', found '{0}'.", r.Name));

            r.MoveToAttribute("Name");
            name = r.Value;
            r.MoveToAttribute("NameSpace");
            _namespace = r.Value;
            r.MoveToContent();
            r.Read();

            outputpath = r.ReadElementString("OutputPath");
            if (r.Name == "WebOutputPath")
                webOutputPath = r.ReadElementString("WebOutputPath");
            defaultTableName = r.ReadElementString("DefaultTableName");
            isTableCoded = bool.Parse(r.ReadElementString("DefaultTableNameHardCoded"));
            caption = r.ReadElementString("Caption");
            description = r.ReadElementString("Description");
            summary = r.ReadElementString("Summary");

            if (r.Name == "IsCachingEnabled")
                isCachingEnabled = bool.Parse(r.ReadElementString("IsCachingEnabled"));
            if (r.Name == "CacheClass")
                cacheClass = r.ReadElementString("CacheClass");
            if (r.Name == "CacheName")
                cacheName = r.ReadElementString("CacheName");
            if (r.Name == "AspNetCachingEnabled")
                isCachingEnabled = bool.Parse(r.ReadElementString("AspNetCachingEnabled"));
            if (r.Name == "CacheLifetime")
                cacheLifetime = TimeSpan.Parse(r.ReadElementString("CacheLifetime"));
            if (r.Name == "IsCollectionCachingEnabled")
                isCollectionCachingEnabled = bool.Parse(r.ReadElementString("IsCollectionCachingEnabled"));
            if (r.Name == "CollectionCacheLifetime")
                collectionCacheLifetime = TimeSpan.Parse(r.ReadElementString("CollectionCacheLifetime"));
            if (r.Name == "LockType")
                concurrency = (ConcurrencyType)Enum.Parse(typeof(ConcurrencyType), r.ReadElementString("LockType"), true);
            if (r.Name == "Concurrency")
                concurrency = (ConcurrencyType)Enum.Parse(typeof(ConcurrencyType), r.ReadElementString("Concurrency"), true);
            if (r.Name == "IsThreadSafe")
                isThreadSafe = bool.Parse(r.ReadElementString("IsThreadSafe"));
            if (r.Name == "IsCreateDateEnabled")
                r.ReadElementString("IsCreateDateEnabled");
            if (r.Name == "IsModifyDateEnabled")
                r.ReadElementString("IsModifyDateEnabled");
            if (r.Name == "Interfaces")
                interfaces = r.ReadElementString("Interfaces");
            if (r.Name == "ToStringOverride")
                toStringOverride = r.ReadElementString("ToStringOverride");
            if (r.Name == "SimpleQueryEnabled")
                simpleQueryEnabled = bool.Parse(r.ReadElementString("SimpleQueryEnabled"));

            #region Obsolete Code

            // Place old version of fields data in data folder
            if (r.Name == "Fields")
            {
                // Make sure there is a default folder!
                if (folders.Count == 0)
                {
                    ClassFolder defaultFolder = new ClassFolder("Default");
                    defaultFolder.ParentClass = this;
                    folders.Add(defaultFolder);
                }

                if (!r.IsEmptyElement)
                {
                    r.Read();
                    while (r.LocalName == "FieldEntry")
                    {
                        ValueField field = new ValueField(r, version);
                        field.ParentFolder = folders[0];
                        folders[0].Items.Add(field);
                    }
                    r.ReadEndElement();
                }
                else
                    r.Read();
            }

            // Place old version of children data in data folder
            if (r.Name == "Children")
            {
                // Make sure there is a default folder!
                if (folders.Count == 0)
                {
                    ClassFolder defaultFolder = new ClassFolder("Default");
                    folders.Add(defaultFolder);
                }

                if (!r.IsEmptyElement)
                {
                    r.Read();
                    while (r.LocalName == "ChildEntry")
                    {
                        ReferenceField field = new ReferenceField(r, version);
                        field.ParentFolder = folders[0];
                        folders[0].Items.Add(field);
                    }
                    r.ReadEndElement();
                }
                else
                    r.Read();
            }

            #endregion

            if (r.Name == "ClassFolders")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();
                    while (r.LocalName == "ClassFolder")
                    {
                        ClassFolder folder = new ClassFolder(r, version);
                        folder.ParentClass = this;
                        folders.Add(folder);
                    }
                    r.ReadEndElement();
                }
                else
                    r.Read();
            }

            if (r.Name == "OutputPlugins")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();
                    while (r.LocalName == "ClassOutputConnector")
                        outputConnectors.Add(new OutputExtensionConnector(r, this));
                    r.ReadEndElement();
                }
                else
                    r.Read();
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
                        ModelClassExtension newExtension = (ModelClassExtension)
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

            EnsureSystemFolderExists();
        }

        public void WriteXml(XmlTextWriter w)
        {
            w.WriteStartElement("ClassObject");
            w.WriteAttributeString("Name", name);
            w.WriteAttributeString("NameSpace", _namespace);
            w.WriteElementString("OutputPath", outputpath);
            w.WriteElementString("WebOutputPath", webOutputPath);
            w.WriteElementString("DefaultTableName", defaultTableName);
            w.WriteElementString("DefaultTableNameHardCoded", isTableCoded.ToString());
            w.WriteElementString("Caption", caption);
            w.WriteElementString("Description", description);
            w.WriteElementString("Summary", summary);
            w.WriteElementString("IsCachingEnabled", isCachingEnabled.ToString());
            w.WriteElementString("CacheClass", cacheClass);
            w.WriteElementString("CacheName", cacheName);
            w.WriteElementString("CacheLifetime", cacheLifetime.ToString());
            w.WriteElementString("IsCollectionCachingEnabled", isCollectionCachingEnabled.ToString());
            w.WriteElementString("CollectionCacheLifetime", collectionCacheLifetime.ToString());
            w.WriteElementString("Concurrency", concurrency.ToString());
            w.WriteElementString("IsThreadSafe", isThreadSafe.ToString());
            w.WriteElementString("Interfaces", interfaces);
            w.WriteElementString("ToStringOverride", toStringOverride);
            w.WriteElementString("SimpleQueryEnabled", simpleQueryEnabled.ToString());

            w.WriteStartElement("ClassFolders");
            foreach (ClassFolder folder in folders)
                folder.WriteXml(w);
            w.WriteEndElement();		// Folders

            w.WriteStartElement("OutputPlugins");
            foreach (OutputExtensionConnector connector in outputConnectors)
                connector.WriteXml(w);
            w.WriteEndElement();

            w.WriteStartElement("MetaAttributes");
            if (attributes != null)
            {
                foreach (MetaAttribute attribute in attributes)
                    attribute.WriteXml(w);
            }
            w.WriteEndElement();

            w.WriteStartElement("Extensions");
            foreach (ModelClassExtension extension in extended.Values)
            {
                extension.writeXml(w);
            }
            w.WriteEndElement();

            w.WriteEndElement();		// ClassObject
        }

        public void Move(object src, object dst)
        {
            if (src is ClassField && dst is ClassFolder)
            {
                Move((ClassField)src, (ClassFolder)dst);
            }
            else if (src is ClassFolder && dst is ModelClass)
            {
                Move((ClassFolder)src, (ModelClass)dst);
            }
        }

        public void Move(ClassFolder src, ModelClass dst)
        {
            src.ParentClass.Folders.Remove(src);
            dst.Folders.Add(src);
            src.ParentClass = dst;
        }

        public void Move(ClassField src, ClassFolder dst)            
        {
            src.ParentFolder.Items.Remove(src);
            dst.Items.Add(src);
            src.ParentFolder = dst;            
        }

        public void Move(object src, object dst, int index)
        {
            if (src is ClassField && dst is ClassFolder)
            {
                Move((ClassField)src, (ClassFolder)dst, index);
            }
            else if (src is ClassFolder && dst is ClassFolder)
            {
                Move((ClassFolder)src, (ClassFolder)dst, index);
            }
        }

        public void Move(ClassField src, ClassFolder dst, int index)
        {
            src.ParentFolder.Items.Remove(src);
            dst.Items.Insert(index, src);
            src.ParentFolder = dst;            
        }

        public void Move(ClassFolder src, ModelClass dst, int index)
        {
            src.ParentClass.Folders.Remove(src);
            dst.Folders.Insert(index, src);
            src.ParentClass = dst;
        }

		public void MoveDown(int i)
		{
			if(i + 1 < folders.Count)
			{
				ClassFolder swap = folders[i+1];
				folders[i+1] = folders[i];
				folders[i] = swap;
			}
		}

		public void MoveUp(int i)
		{
			if(i > 0)
			{
				ClassFolder swap = folders[i-1];
				folders[i-1] = folders[i];
				folders[i] = swap;
			}
		}

		#region Copy, Clear, Clone Methods

		public void Clear()
		{
			name = string.Empty;
			_namespace = string.Empty;
			summary = string.Empty;
			outputpath = string.Empty;
			defaultTableName = string.Empty;
			isTableCoded = false;
			folders.Clear();
			folders.Add(new ClassFolder("Default"));
		}

		public ModelClass Copy()
		{
			ModelClass c = new ModelClass();
			c.caption = caption;
			c.concurrency = concurrency;
			c.defaultTableName = defaultTableName;
			c.description = description;
			c.editor = null;
			c.folders = folders.Copy();
			c.isCachingEnabled = isCachingEnabled;
			c.isCollectionCachingEnabled = isCollectionCachingEnabled;
			c.isTableCoded = isTableCoded;
			c.isThreadSafe = isThreadSafe;
			c.name = name;
			c._namespace = _namespace;
			// c._outputConnectors = _outputConnectors.Copy();
			c.outputpath = outputpath;
			c.parentModel = parentModel;
			c.summary = summary;

			c.IsCreateDateEnabled = IsCreateDateEnabled;
			c.IsModifyDateEnabled = IsModifyDateEnabled;

			return c;
		}

		#endregion

		public override string ToString()
		{
			return name;
		}

		private void EnsureSystemFolderExists()
		{
			foreach(ClassFolder f in folders)
				if(f.Name == "_system" & f.IsReadOnly & !f.IsExpanded)
					return;

			ClassFolder systemFolder = new ClassFolder("_system");
			systemFolder.Caption = "System Folder";
			systemFolder.Description = "Internal system folder for supporting NitroCast class entry options.";
			systemFolder.IsReadOnly = true;			
			systemFolder.IsBrowsable = false;
            systemFolder.ParentClass = this;
			folders.Add(systemFolder);
		}

        public ModelClassExtension GetExtension(Type type)
        {
            try
            {
                return extended[type];
            }
            catch
            {
                // Make a default
                return (ModelClassExtension)
                    Activator.CreateInstance(type);
            }
        }

		#region IComparable Methods

		public int CompareTo(ModelClass x)
		{
			return name.CompareTo(x.Name);
		}

		int IComparable.CompareTo(object x)
		{
			return name.CompareTo(((ModelClass) x).name);
		}

		#endregion

		object ICloneable.Clone()
		{
			return Clone();
		}

		public ModelClass Clone()
		{
			ModelClass c = new ModelClass();

			c.caption = caption;
			c.concurrency = concurrency;
			c.defaultTableName = defaultTableName;
			c.description = description;
			c.editor = null;												// Clear the editor or else disaster will strike!
			c.folders = folders.Copy();									// Folders MUST be copied instead of cloned.
			c.interfaces = interfaces;
			c.isCachingEnabled = isCachingEnabled;
			c.isCollectionCachingEnabled = isCollectionCachingEnabled;
			c.isTableCoded = isTableCoded;
			c.isThreadSafe = isThreadSafe;
			c.name = name;
			c._namespace = _namespace;
			c.outputConnectors = outputConnectors;
			c.outputpath = outputpath;
			c.parentModel = parentModel;
			c.summary = summary;

			return c;
		}
	}

    public class ModelClassEventArgs : EventArgs
    {
        public ModelClass ParentClass { get; set; }

        public ModelClassEventArgs(ModelClass modelClass)
        {
            ParentClass = modelClass;
        }
    }

    public delegate void ModelClassEventHandler(object sender, ModelClassEventArgs e);
}