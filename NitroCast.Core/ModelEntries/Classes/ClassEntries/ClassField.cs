using System;
using System.ComponentModel;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for ClassField.
	/// </summary>
	public abstract class ClassField : ModelEntry
	{
        string _customPrivateName = string.Empty;
        string _customColumnName = string.Empty;

        RevisionTag _revisionTag;

        private ClassFolder _parentFolder;
        
        #region Name Properties
		
    	[Category("Design"),
		Description("Custom private name for object.")]
		public string CustomPrivateName
		{
			get
			{
				return _customPrivateName;
			}
			set
			{
				_customPrivateName = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data Tier"),
		Description("Custom column name for persistance layer.")]
		public string CustomColumnName
		{
			get
			{
				return _customColumnName;
			}
			set
			{
				_customColumnName = value;
				OnChanged(EventArgs.Empty);
			}
		}

		#endregion

		#region Dynamic Properties

		[Category("Design"), Browsable(false), 
        Description("The private name of the child object.")]
		public string PrivateName
		{
			get
			{
                string name = Name;

				if(_customPrivateName != string.Empty)
					return _customPrivateName;
                if (name == null)
					return string.Empty;
                if (name == string.Empty)
					return string.Empty;
                string tempPropertyName = name.Substring(0, 1).ToLower() + 
                    name.Substring(1, name.Length - 1);
				
				if(tempPropertyName == "class")
					return "_class";
				else
					return tempPropertyName;
			}
		}

		[Category("Data Tier"), Browsable(false),
		Description("Column name for persistance layer.")]
		public string ColumnName
		{
			get
			{
				if(_customColumnName != string.Empty)
					return _customColumnName;

				return Name;
			}
		}

    	#endregion

        #region Version Control

        [Browsable(false)]
        public RevisionTag RevisionTag
        {
            get { return _revisionTag; }
            set { _revisionTag = value; }
        }

        #endregion

        [Browsable(false)]
        public ClassFolder ParentFolder
        {
            get { return _parentFolder; }
            set { _parentFolder = value; }
        }

        public event EventHandler Changed;
		protected virtual void OnChanged(EventArgs e)
		{
			if(Changed != null)
				Changed(this, e);            
		}
	}
}