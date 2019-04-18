using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DataTypeMap.
	/// </summary>
	[TypeConverter(typeof(TypeConverters.ValueTypeConverter))]
	public class ValueType : IModelEntry
	{
		/* =================================================================================
		 *	The example values are for a DateTime type; see comments to the right.
		 * ================================================================================= */

		string _name;								// DateTime (DateEditor)	Char                Int
		string _programType;						// DateTime					char                int
		bool _isNullible;							// false					true                true

		bool _isClass = false;						// false					false               false
        string _dotNetType = string.Empty;          // DateTime                 Char                Int
		string _dotNetDbType = string.Empty;		// DbType.DateTime			DbType.String       DbType.Integer
		string _jetSqlType = string.Empty;			// DATETIME					VARCHAR(1)          LONG
        string _sqlServerType = string.Empty;       // DATETIME                 VARCHAR(1)          LONG

		string _nullValue = string.Empty;			// DateTime.Min				null                0
		string _defaultValue = string.Empty;		// DateTime.Min				null                0
		string _defaultValueFormat = string.Empty;	// 
		bool _lengthEnabled;						// false					false               false
		bool _byteBufferEnabled;					// false					false               false
		string _dataReaderFormat;					// r.GetDateTime({0})		r.GetChar({0})      {0}.GetInt16({1})     		// 0=column
		string _dataWriterFormat;					// {0}						{0}.ToString()	    {0}            	            // 0=editor

        string _editorNamespace = string.Empty;		// Namespace of the type editor                 
		string _editorClass = string.Empty;			// DateEditor			                                                    // class of the typeditor
		string _editorPrefix;					    // de
		string _editorSetValueFormat;	    		// {0}.Date
		string _editorGetValueFormat;		    	// {0}.Date
		List<string> _editorProperties;				    // .Width = Unit.Pixel(175); .EnableViewState = false;

        string _validatorNamespace = string.Empty;  // string.Empty             System.Web.UI.WebControls
        string _validatorClass = string.Empty;      //                          RangeValidator
        string _validatorPrefix;                    //                          rv
        List<string> _validatorProperties;              //                          rv{0}.ID = this.ID + \"_rv{0}\"; 
                                                    //                          rv{0}.TargetControl = tb{0};
                                                    //                          rv{0}.ErrorText = "*"

        string _requiredValidatorNamespace = string.Empty;  //                          System.Web.UI.WebControls
        string _requiredValidatorClass = string.Empty;      //                          RequiredFieldValidator
        string _requiredValidatorPrefix = string.Empty;     //                          rfv
        List<string> _requiredValidatorProperties;              //                          frv{0}.ID = this.ID + \"_frv{0}\";
                                                            //                          frv{0}.TargetControl = tb{0};
                                                            //                          frv{0}.ErrorText = "*";

        string _rangeValidatorNamespace = string.Empty;  //                          System.Web.UI.WebControls
        string _rangeValidatorClass = string.Empty;      //                          RequiredFieldValidator
        string _rangeValidatorPrefix = string.Empty;     //                          rfv
        List<string> _rangeValidatorProperties; 



		// Obsolete Versions - Do Not Use!
		bool _adhocQuotesEnabled;					// false

		#region Properties

		public string Name
		{
			get { return _name; }
		}

		public string ProgramType
		{
			get { return _programType; }		
		}

        public string DotNetType
        {
            get { return _dotNetType; }
        }

		public string DotNetDbType
		{
			get { return _dotNetDbType; }
		}

		public string DbType
		{
			get { return _jetSqlType; }
		}

        public String SqlServerType
        {
            get { return _sqlServerType; }
        }

		public string DefaultValue
		{
			get { return _defaultValue; }
		}		

		/// <summary>
		/// Specifies the default null value.
		// </summary>
		public string NullValue
		{
			get { return _nullValue; }
			set { _nullValue = value; }
		}

		public bool IsNullible
		{
			get { return _isNullible; }
			set	{ _isNullible =  value; }
		}

		private string DefaultValueFormat
		{
			get { return _defaultValueFormat; }
		}

		public bool LengthEnabled
		{
			get
			{
				return _lengthEnabled;
			}
		}

		public bool ByteBufferEnabled
		{
			get
			{
				return _byteBufferEnabled;
			}
		}

		public bool IsClass
		{
			get
			{
				return _isClass;
			}
		}

		/// <summary>
		/// Must be in the following format '{0}.GetString({1})' where {0} is the reader name and
		/// {1} is the index of the row.
		/// </summary>
		public string DataReaderFormat
		{
			get
			{
				return _dataReaderFormat;
			}
			set
			{
				_dataReaderFormat = value;
			}
		}

		public string DataWriterFormat
		{
			get
			{
				return _dataWriterFormat;
			}
			set
			{
				_dataWriterFormat = value;
			}
		}

		public string TypeEditorClass
		{
			get
			{
				return _editorClass;
			}
			set
			{
				_editorClass = value;
			}
		}

		public string TypeEditorPrefix
		{
			get
			{
				return _editorPrefix;
			}
			set
			{
				_editorPrefix = value;
			}
		}

		public string TypeEditorSetValueFormat
		{
			get
			{
				return _editorSetValueFormat;
			}
			set
			{
				_editorSetValueFormat = value;
			}
		}

		public string TypeEditorGetValueFormat
		{
			get
			{
				return _editorGetValueFormat;
			}
			set
			{
				_editorGetValueFormat = value;
			}
		}

		public List<string> EditorProperties
		{
			get
			{
				return _editorProperties;
			}
			set
			{
				_editorProperties = value;
			}
		}

		public string TypeEditorNamespace
		{
			get
			{
				return _editorNamespace;
			}
			set
			{
				_editorNamespace = value;
			}
		}

		/// <summary>
		/// Specifies whether quotes contain data in adhoc queries (eg. TEXT and DATETIME).
		/// </summary>
		public bool AdhocQuotesEnabled
		{
			get
			{
				return _adhocQuotesEnabled;
			}
		}

		#endregion

        #region Validator Properties

        public string ValidatorNamespace
        {
            get { return _validatorNamespace; }
            set { _validatorNamespace = value; }
        }

        public string ValidatorClass
        {
            get { return _validatorClass; }
            set { _validatorClass = value; }
        }

        public string ValidatorPrefix
        {
            get { return _validatorPrefix; }
            set { _validatorPrefix = value; }
        }

        public List<string> ValidatorProperties
        {
            get { return _validatorProperties; }
            set { _validatorProperties = value; }
        }

        #endregion

        #region Required Validator Properties

        public string RequiredValidatorNamespace
        {
            get { return _requiredValidatorNamespace; }
            set { _requiredValidatorNamespace = value; }
        }

        public string RequiredValidatorClass
        {
            get { return _requiredValidatorClass; }
            set { _requiredValidatorClass = value; }
        }

        public string RequiredValidatorPrefix
        {
            get { return _requiredValidatorPrefix; }
            set { _requiredValidatorPrefix = value; }
        }

        public List<string> RequiredValidatorProperties
        {
            get { return _requiredValidatorProperties; }
            set { _requiredValidatorProperties = value; }
        }

        #endregion

        #region Range Validator Properties

        public string RangeValidatorNamespace
        {
            get { return _rangeValidatorNamespace; }
            set { _rangeValidatorNamespace = value; }
        }

        public string RangeValidatorClass
        {
            get { return _rangeValidatorClass; }
            set { _rangeValidatorClass = value; }
        }

        public string RangeValidatorPrefix
        {
            get { return _rangeValidatorPrefix; }
            set { _rangeValidatorPrefix = value; }
        }

        public List<string> RangeValidatorProperties
        {
            get { return _rangeValidatorProperties; }
            set { _rangeValidatorProperties = value; }
        }

        #endregion

        #region Constructors

        public ValueType(string name, string programType, 
			string dotNetType, string dotNetDbType, string dbType, string sqlServerType,
			string dataReaderFormat, string dataWriterFormat)
		{
			this._name = name;
			this._programType = programType;
            this._dotNetType = dotNetType;
			this._dotNetDbType = dotNetDbType;
			this._jetSqlType = dbType;
            this._sqlServerType = sqlServerType;
			this._dataReaderFormat = dataReaderFormat;
			this._dataWriterFormat = dataWriterFormat;
			this._editorNamespace = "System.Web.UI.WebControls";
			this._editorClass = "TextBox";
			this._editorPrefix = "tb";
			this._editorSetValueFormat = "{0}.Text";
			this._editorGetValueFormat = "{0}.Text";
			this._editorProperties = null;
			this._lengthEnabled = false;
			this._adhocQuotesEnabled = false;
			this._defaultValue = string.Empty;
		}

		public ValueType(string name, string programType, 
            string dotNetType,
			string dotNetDbType, string dbType, string sqlServerType,
			string dataReaderFormat, string dataWriterFormat,
			string typeEditorNamespace, string typeEditorClass,	string typeEditorPrefix, 
			string typeEditorSetValueFormat, string typeEditorGetValueFormat)
		{
			this._name = name;
			this._programType = programType;
            this._dotNetType = dotNetType;
			this._dotNetDbType = dotNetDbType;
			this._jetSqlType = dbType;
            this._sqlServerType = sqlServerType;
			this._dataReaderFormat = dataReaderFormat;
			this._dataWriterFormat = dataWriterFormat;
			this._editorNamespace = typeEditorNamespace;
			this._editorClass = typeEditorClass;
			this._editorPrefix = typeEditorPrefix;
			this._editorSetValueFormat = typeEditorSetValueFormat;
			this._editorGetValueFormat = typeEditorGetValueFormat;
			this._editorProperties = null;
			this._lengthEnabled = false;
			this._adhocQuotesEnabled = false;
			this._defaultValue = string.Empty;
		}

		public ValueType(string name, string programType, 
            string dotNetType,
            string dotNetDbType, string dbType, string sqlServerType,
			string dataReaderFormat, string dataWriterFormat,
			string typeEditorNamespace, string typeEditorClass,	string typeEditorPrefix, 
			string typeEditorSetValueFormat, string typeEditorGetValueFormat,
			bool lengthEnabled, bool adhocQuotesEnabled, string defaultValue)
		{
			this._name = name;
			this._programType = programType;
            this._dotNetType = dotNetType;
			this._dotNetDbType = dotNetDbType;
			this._jetSqlType = dbType;
            this._sqlServerType = sqlServerType;
			this._dataReaderFormat = dataReaderFormat;
			this._dataWriterFormat = dataWriterFormat;
			this._editorNamespace = typeEditorNamespace;
			this._editorClass = typeEditorClass;
			this._editorPrefix = typeEditorPrefix;
			this._editorSetValueFormat = typeEditorSetValueFormat;
			this._editorGetValueFormat = typeEditorGetValueFormat;
			this._editorProperties = null;
			this._lengthEnabled = lengthEnabled;
			this._adhocQuotesEnabled = adhocQuotesEnabled;
			this._defaultValue = defaultValue;
		}

		#endregion

		public string MakeReaderMethod(string readerName, int columnIndex)
		{
			try
			{
				return string.Format(_dataReaderFormat, readerName, columnIndex);
			}
			catch
			{
				throw(new Exception("Reader method not formatted correctly."));
			}
		}

		public string MakeReaderMethod(string readerName, int columnIndex, string offsetIndex)
		{
			try
			{
				return string.Format(_dataReaderFormat, readerName, columnIndex.ToString() + "+" + offsetIndex);
			}
			catch
			{
				throw(new Exception("Reader method not formatted correctly."));
			}
		}

		public override string ToString()
		{
			return _name;
		}

		#region Deserialization Constructor 

        public ValueType(XmlTextReader r)
		{
			if(r.Name != "FieldDataType")
				throw new Exception(string.Format("Cannot load, expected FieldDataType, found '{0}'.",
					r.Name));

			r.MoveToAttribute("Name");
			_name = r.Value;
			r.MoveToAttribute("ProgramType");
			_programType = r.Value;
			r.MoveToContent();
			r.Read();

			_isClass = bool.Parse(r.ReadElementString("IsClass"));
            _dotNetType = r.ReadElementString("DotNetType");
			_dotNetDbType = r.ReadElementString("DotNetDbType");
			_jetSqlType = r.ReadElementString("DbType");
            if (r.Name == "SqlServerType") _sqlServerType = r.ReadElementString("SqlServerType");
			_defaultValue = r.ReadElementString("DefaultValue");
			_defaultValueFormat = r.ReadElementString("DefaultValueFormat");
			_lengthEnabled = bool.Parse(r.ReadElementString("LengthEnabled"));
			_byteBufferEnabled = bool.Parse(r.ReadElementString("ByteBufferEnabled"));
			_adhocQuotesEnabled = bool.Parse(r.ReadElementString("AdhocQuotesEnabled"));
			_dataReaderFormat = r.ReadElementString("DataReaderFormat");
			_dataWriterFormat = r.ReadElementString("DataWriterFormat");
			_editorNamespace = r.ReadElementString("TypeEditorNamespace");
			_editorClass = r.ReadElementString("TypeEditorClass");
			_editorPrefix = r.ReadElementString("TypeEditorPrefix");
			_editorSetValueFormat = r.ReadElementString("TypeEditorSetValueFormat");
			_editorGetValueFormat = r.ReadElementString("TypeEditorGetValueFormat");

            string[] editorProperties = 
                r.ReadElementString("TypeEditorProperties").Split('\n');
            if (editorProperties.GetUpperBound(0) > 0)
            {
                _editorProperties = new List<string>();
                foreach (string property in editorProperties)
                    _editorProperties.Add(property);
            }

			r.ReadEndElement();
		}

		#endregion

		#region Serialization Method

		public void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("FieldDataType");
			w.WriteAttributeString("Name", _name);
			w.WriteAttributeString("ProgramType", _programType);
			w.WriteElementString("IsClass", _isClass.ToString());
            w.WriteElementString("DotNetType", _dotNetType);
			w.WriteElementString("DotNetDbType", _dotNetDbType);
			w.WriteElementString("DbType", _jetSqlType);
            w.WriteElementString("SqlServerType", _sqlServerType);
			w.WriteElementString("DefaultValue", _defaultValue);
			w.WriteElementString("DefaultValueFormat", _defaultValueFormat);
			w.WriteElementString("LengthEnabled", _lengthEnabled.ToString());
			w.WriteElementString("ByteBufferEnabled", _byteBufferEnabled.ToString());
			w.WriteElementString("AdhocQuotesEnabled", _adhocQuotesEnabled.ToString());
			w.WriteElementString("DataReaderFormat", _dataReaderFormat);
			w.WriteElementString("DataWriterFormat", _dataWriterFormat);
			w.WriteElementString("TypeEditorNamespace", _editorNamespace);
			w.WriteElementString("TypeEditorClass", _editorClass);
			w.WriteElementString("TypeEditorPrefix", _editorPrefix);
			w.WriteElementString("TypeEditorSetValueFormat", _editorSetValueFormat);
			w.WriteElementString("TypeEditorGetValueFormat", _editorGetValueFormat);

            if (_editorProperties == null)
                w.WriteElementString("TypeEditorProperties", string.Empty);
            else
            {
                String propertyString = string.Empty;
                foreach (string property in _editorProperties)
                    propertyString += property + "\n";
                w.WriteElementString("TypeEditorProperties", propertyString);
            }
			w.WriteEndElement();
		}

		#endregion

		public event EventHandler Updated;
		protected virtual void OnUpdated(EventArgs e)
		{
			if(Updated != null)
				Updated(this, e);
		}
	}
}