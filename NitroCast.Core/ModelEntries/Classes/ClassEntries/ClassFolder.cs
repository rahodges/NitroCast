using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using NitroCast.Core.Extensions;
using NitroCast.Core.Support;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DataContainer.
	/// </summary>
	public class ClassFolder : ModelEntry, IDesignerState
	{
		#region Designer State
		
		private bool _isExpanded;						
		private bool _isSelected;	
		private bool _isItemListExpanded;	

		[Browsable(false)]
		public bool IsExpanded
		{
			get { return _isExpanded; }
			set { _isExpanded = value; }
		}

		[Browsable(false)]
		public bool IsSelected
		{
			get { return _isSelected; }
			set { _isSelected = value; }
		}

        [Category("Design"),
            Description("The builder to use for generating code snippets."),
            DefaultValue(typeof(ClassFolderBuilder), "Default")]
        public ClassFolderBuilder Builder
        {
            get
            {
                return builder;
            }
            set
            {
                builder = value;
            }
        }

        [Browsable(false)]
        public bool IsItemListExpanded
		{
			get { return _isItemListExpanded; }
			set { _isItemListExpanded = value; }
		}

		#endregion

        bool _isReadOnly;
        bool _isBrowsable;
        bool _isPartition;
        ClassFolderDataMode _dataMode;
        ArrayList items;
        ModelClass _parentClass;
        ClassFolderBuilder builder;

        internal Dictionary<Type, ClassFolderExtension> extensions;
        private ReadOnlyDictionary<Type, ClassFolderExtension> readOnlyExtensions;

		#region Client Properties

        [Browsable(false)]
        public string ProgramName
        {
            get { return Name.Replace(" ", "_"); }
        }

		[Category("Data"), 
		Description("Specifies the datamode for the folder."),
		DefaultValue(ClassFolderDataMode.Integrated)]
		public ClassFolderDataMode DataMode
		{
			get { return _dataMode; }
			set {_dataMode = value; }
		}

        [Category("Data"), Browsable(true),
        Description("Marks the folder as a read/write partition which " +
            "can assist query performance if used properly.")]
        public bool IsPartition
        {
            get { return _isPartition; }
            set { _isPartition = value; }
        }

        public ReadOnlyDictionary<Type, ClassFolderExtension> Extensions
        {
            get { return readOnlyExtensions; }
        }

		#endregion

		#region Hidden Properties

        [Category("System"), Browsable(false),
        Description("Marks the folder as an uneditable system folder in design mode.")]
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        [Category("System"), Browsable(false),
        Description("Marks the folder as browsable in design mode.")]
        public bool IsBrowsable
        {
            get { return _isBrowsable; }
            set { _isBrowsable = value; }
        }

        [Browsable(false)]
        public ModelClass ParentClass
        {
            get { return _parentClass; }
            set { _parentClass = value; }
        }

		[Browsable(false)]
		public ReferenceFieldCollection Children
		{
			get 
			{ 
				ReferenceFieldCollection children = new ReferenceFieldCollection();
				foreach(object i in items)
					if(i is ReferenceField)
						children.Add((ReferenceField) i);
				return children;
			}
		}

		[Browsable(false)]
		public ValueFieldCollection Fields
		{
			get 
			{ 
				ValueFieldCollection fields = new ValueFieldCollection();
				foreach(object i in items)
					if(i is ValueField)
						fields.Add((ValueField) i);
				return fields;
			}			
		}

		[Browsable(false)]
		public ArrayList Items
		{
			get { return items; }
		}

		#endregion
        
		public ClassFolder()
		{
            _isBrowsable = true;
            _dataMode = ClassFolderDataMode.Integrated;
            items = new ArrayList();
            extensions = new Dictionary<Type, ClassFolderExtension>();
            readOnlyExtensions = new ReadOnlyDictionary<Type, ClassFolderExtension>(extensions);
            _isExpanded = true;
            builder = ClassFolderBuilder.Default;
		}

		public ClassFolder(string name) : this()
		{
			Name = name;
		}

		public ClassFolder(XmlTextReader r, string version) : this()
		{			
			if(r.Name != "ClassFolder")
				throw new Exception(string.Format("Source file does not match NitroCast DTD; " +
					"expected 'ClassModel', found '{0}'.", r.Name));

            base.ParseXml(r);

            if (r.Name == "Builder")
                ClassFolderBuilder.Builders.TryGetValue(r.ReadElementString("Builder"), out builder);

			_isExpanded = bool.Parse(r.ReadElementString("IsExpanded"));
			_isItemListExpanded = bool.Parse(r.ReadElementString("IsItemListExpanded"));

			if(r.Name == "IsReadOnly")
				_isReadOnly = bool.Parse(r.ReadElementString("IsReadOnly"));
			else
				_isReadOnly = false;

			if(r.Name == "IsBrowsable")
				_isBrowsable = bool.Parse(r.ReadElementString("IsBrowsable"));
			else
				_isBrowsable = true;

            if (r.Name == "IsPartition")
                _isPartition = bool.Parse(r.ReadElementString("IsPartition"));
            else
                _isPartition = false;

			if(r.Name == "DataMode")
				_dataMode = (ClassFolderDataMode) Enum.Parse(typeof(ClassFolderDataMode), r.ReadElementString("DataMode"), true);
			else
				_dataMode = ClassFolderDataMode.Integrated;

			if(r.Name == "Items")
			{
				if(!r.IsEmptyElement)
				{
					r.Read();

					while((r.Name == "FieldEntry" |
                        r.Name == "ChildEntry" |
                        r.Name == "EnumField"))
					{
						//r.Read();
						switch(r.LocalName)
						{
							case "FieldEntry":
                                ValueField vf = new ValueField(r, version);
                                vf.ParentFolder = this;
								items.Add(vf);
								break;
							case "ChildEntry":
                                ReferenceField rf = new ReferenceField(r, version);
                                rf.ParentFolder = this;
                                items.Add(rf);
								break;
							case "EnumField":
                                EnumField ef = new EnumField(r, version);
                                ef.ParentFolder = this;
								items.Add(ef);
								break;
						}					
					}
					r.ReadEndElement();
				}
				else
					r.Read();
			}

            if (r.Name == "Extensions")
            {
                if (!r.IsEmptyElement)
                {
                    r.Read();

                    while (r.Name == "Extension")
                    {
                        ClassFolderExtension newExtension = (ClassFolderExtension)
                                ObjectExtension.Build(r, version);
                        extensions.Add(newExtension.GetType(), newExtension);
                        r.ReadEndElement();
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

		public override void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ClassFolder");

            base.WriteXml(w);

            w.WriteElementString("Builder", builder.Name);
			w.WriteElementString("IsExpanded", _isExpanded.ToString());
			w.WriteElementString("IsItemListExpanded", _isItemListExpanded.ToString());
			w.WriteElementString("IsReadOnly", _isReadOnly.ToString());
			w.WriteElementString("IsBrowsable", _isBrowsable.ToString());
            w.WriteElementString("IsPartition", _isPartition.ToString());
			w.WriteElementString("DataMode", _dataMode.ToString());

			w.WriteStartElement("Items");
			foreach(object i in items)
			{
				if(i is ValueField)
                    ((ValueField) i).WriteXml(w);
				else if(i is ReferenceField)
					((ReferenceField) i).WriteXml(w);
				else if(i is EnumField)
                    ((EnumField)i).WriteXml(w);
			}
			w.WriteEndElement();

            w.WriteStartElement("Extensions");
            foreach (ClassFolderExtension extension in extensions.Values)
            {
                extension.writeXml(w);
            }
            w.WriteEndElement();

			w.WriteEndElement();
		}


        public void MoveDown(int i)
        {
            if (i + 1 < items.Count)
            {
                object swap = items[i + 1];
                items[i + 1] = items[i];
                items[i] = swap;
            }
        }

        public void MoveUp(int i)
        {
            if (i > 0)
            {
                object swap = items[i - 1];
                items[i - 1] = items[i];
                items[i] = swap;
            }
        }

        public ClassFolderExtension GetExtension(Type type)
        {
            try
            {
                return extensions[type];
            }
            catch
            {
                // Make a default
                return (ClassFolderExtension)
                    Activator.CreateInstance(type);
            }
        }

		#region Design State Methods

		public void LoadDesignState(XmlTextReader r)
		{
			// Since the design state manager already dives into the xml to find the guid...
			// we skip the following code.

//			if(r.Name != "ClassFolderDesignState")
//				throw new Exception(string.Format("Source file does not match NitroCast DTD; " +
//					"expected 'ClassFolderDesignState', found '{0}'.", r.Name));
//			
//			r.MoveToAttribute("Guid");
//			_name = r.Value;
//			r.MoveToContent();
//			r.Read();

			_isExpanded = bool.Parse(r.ReadElementString("IsExpanded"));
			_isSelected = bool.Parse(r.ReadElementString("IsSelected"));
			_isItemListExpanded = bool.Parse(r.ReadElementString("IsItemListExpanded"));

			r.ReadEndElement();
		}

		public void SaveDesignState(XmlTextWriter w)
		{
			w.WriteStartElement("ClassFolderDesignState");
			w.WriteAttributeString("Guid", ID.ToString());
			w.WriteElementString("IsExpanded", _isExpanded.ToString());
			w.WriteElementString("IsSelected", _isSelected.ToString());
			w.WriteElementString("IsItemListExpanded", _isItemListExpanded.ToString());
			w.WriteEndElement();
		}

		#endregion

		public ClassFolder Copy()
		{
			ClassFolder f = new ClassFolder();

            f.Name = this.Name;
            f.Caption = this.Caption;
            f.Description = this.Description;
			f.ID = this.ID;

			f._isBrowsable = this._isBrowsable;
            f._isReadOnly = this._isReadOnly;
            f._isPartition = this._isPartition;
			f._isExpanded = this._isExpanded;
			f._isItemListExpanded = this._isItemListExpanded;			
			f._isSelected = this._isSelected;

			foreach(object i in items)
			{
				if(i is ValueField)
					f.items.Add(((ValueField) i).Clone());
				if(i is ReferenceField)
					f.items.Add(((ReferenceField) i).Clone());
                if (i is EnumField)
                    f.items.Add(((EnumField)i).Clone());
			}			

			return f;
		}
	}
}