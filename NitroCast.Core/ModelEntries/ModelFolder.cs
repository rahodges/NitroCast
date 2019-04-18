using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using NitroCast.Core.Extensions;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DataContainer.
	/// </summary>
	public class ModelFolder : ModelEntry, IDesignerState
	{
		#region Designer State
		
		bool _isExpanded;						
		bool _isSelected;	
		bool _isItemListExpanded;	

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

		[Browsable(false)]
		public bool IsItemListExpanded
		{
			get { return _isItemListExpanded; }
			set { _isItemListExpanded = value; }
		}

		#endregion

		bool _isReadOnly = false;
		bool _isBrowsable = true;
        
		DataModel _parentModel;

        ArrayList _items = new ArrayList();
        List<ModelFolderExtension> extended;

		#region Client Properties

		[Browsable(false)]
		public ReferenceFieldCollection Children
		{
			get 
			{ 
				ReferenceFieldCollection children = new ReferenceFieldCollection();
				foreach(object i in _items)
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
				foreach(object i in _items)
					if(i is ValueField)
						fields.Add((ValueField) i);
				return fields;
			}			
		}

		[Browsable(false)]
		public ArrayList Items
		{
			get { return _items; }
		}

		[Browsable(false), 
			Description("Marks the folder as an uneditable system folder.")]
		public bool IsReadOnly
		{
			get { return _isReadOnly; }
			set { _isReadOnly = value; }	 
		}

		[Browsable(false), 
		Description("Marks the folder as browsable.")]
		public bool IsBrowsable
		{
			get { return _isBrowsable; }
			set { _isBrowsable = value; }	 
		}

		public DataModel ParentModel
		{
			get { return _parentModel; }
			set { _parentModel = value; }
		}

		#endregion

        public ModelFolder()
		{
            extended = new List<ModelFolderExtension>();
		}

		public ModelFolder(string name) : this()
		{
            Name = name;
		}

		public ModelFolder(XmlTextReader r, DataModel parentModel, string fileVersion) : this()
		{
			this.ParentModel = parentModel;

			if(r.Name != "ModelFolder")
				throw new Exception(string.Format("Source file does not match NitroCast DTD; " +
					"expected 'ClassModel', found '{0}'.", r.Name));

            base.ParseXml(r);

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

			if(r.Name == "Items")
			{
				if(!r.IsEmptyElement)
				{
					r.Read();
					while(r.Name == "ClassObject" | r.Name =="EnumObject")
					{
                        if (r.Name == "ClassObject")
                        {
                            ModelClass c = new ModelClass(r, fileVersion);
                            c.ParentModel = this.ParentModel;
                            _items.Add(c);
                        }
                        else if (r.Name == "EnumObject")
                        {
                            ModelEnum e = new ModelEnum(r, fileVersion);
                            e.ParentModel = this.ParentModel;
                            _items.Add(e);
                        }
					}
					r.ReadEndElement();
				}
				else
					r.Read();
			}
			
			r.ReadEndElement();
		}

		public override void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ModelFolder");

            base.WriteXml(w);

			w.WriteElementString("IsExpanded", _isExpanded.ToString());
			w.WriteElementString("IsItemListExpanded", _isItemListExpanded.ToString());
			w.WriteElementString("IsReadOnly", _isReadOnly.ToString());
			w.WriteElementString("IsBrowsable", _isBrowsable.ToString());

			w.WriteStartElement("Items");
			foreach(object i in _items)
			{
				if(i is ModelClass)
                    ((ModelClass) i).WriteXml(w);
                if (i is ModelEnum)
                    ((ModelEnum)i).WriteXml(w);
			}
			w.WriteEndElement();

			w.WriteEndElement();
		}

		#region Design State Methods

		public void LoadDesignState(XmlTextReader r)
		{
			_isExpanded = bool.Parse(r.ReadElementString("IsExpanded"));
			_isSelected = bool.Parse(r.ReadElementString("IsSelected"));
			_isItemListExpanded = bool.Parse(r.ReadElementString("IsItemListExpanded"));

			r.ReadEndElement();
		}

		public void SaveDesignState(XmlTextWriter w)
		{
			w.WriteStartElement("ModelFolderDesignState");
			w.WriteAttributeString("Guid", ID.ToString());
			w.WriteElementString("IsExpanded", _isExpanded.ToString());
			w.WriteElementString("IsSelected", _isSelected.ToString());
			w.WriteElementString("IsItemListExpanded", _isItemListExpanded.ToString());
			w.WriteEndElement();
		}

		#endregion


        public void MoveDown(int i)
        {
            if (i + 1 < _items.Count)
            {
                object swap = _items[i + 1];
                _items[i + 1] = _items[i];
                _items[i] = swap;
            }
        }

        public void MoveUp(int i)
        {
            if (i > 0)
            {
                object swap = _items[i - 1];
                _items[i - 1] = _items[i];
                _items[i] = swap;
            }
        }

		public ModelFolder Copy()
		{
			ModelFolder f = new ModelFolder();
            f.Caption = this.Caption;
            f.Description = this.Description;
            f.ID = this.ID;
			f._isBrowsable = this._isBrowsable;
			f._isExpanded = this._isExpanded;
			f._isItemListExpanded = this._isItemListExpanded;
			f._isReadOnly = this._isReadOnly;
			f._isSelected = this._isSelected;
            f.Name = this.Name;

			foreach(object i in _items)
			{
				if(i is ModelClass)
					f._items.Add(((ModelClass) i).Clone());
                if (i is ModelEnum)
                    f._items.Add(((ModelEnum)i).Clone());
			}			

			return f;
		}
	}
}