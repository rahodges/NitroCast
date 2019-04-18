using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using NitroCast.Core.Extensions;
using NitroCast.Core.Support;

namespace NitroCast.Core
{
	/// <summary>
	/// Describes a class field for the model.
	/// </summary>
	public class ValueField : ClassField, ICloneable, IComparable
	{
        private ValueType valueType;			    // Converter automatically converts to the most appropriate DbType
        private ValueTypeBuilder builder;           // The builder associated with the valueType
        
        // BUILDERS ARE THE BREAD AND BUTTER! WHOAH!

		public bool IsExpanded;			        	// Saves state of designer
		public bool IsSelected;					    // Saves state of designer		

		private int length;	                        // Used for specifying lengths
		private string defaultFormat;		    	// Moniker used for plugin formatting (can be overridden)
		private string defaultValue;		    	// Default Value		
		private bool validatorRequired;		    	// Sets the field to be required by validators.
		private string validatorRegEx;		    	// Regular Expression for Validation by plugins
		private string validatorError;		    	// Validation error message used by plugins        
		
		private bool isNullable;			    	// Field is nullable.
		private string nullValue;			    	// The null value to use for the field. This will override the null value on the datatype.
		private bool useDefaultValueOnNull;	    	// Manager code should replace a null value in a database with the default value in code.

		private bool isIndexed;             		// Field is indexed.
		private bool isUnique;	
		private string group;				        // Field Group for forms engines

        private bool isClientGridEnabled;           // Specifies that the field is viewable in client grid controls;
		private bool isClientEditEnabled;	        // Specifies that the field is editable in client controls;
		private bool isClientViewEnabled;       	// Specifies that the field is viewable in client controls;

		// Address			vertical placement
		// Address/Private	horizontal placement (forms engine interprets left)
		// Address/Public	horizontal placement (forms engine interprets right)

		private MetaAttributeCollection attributes;
        internal Dictionary<Type, ValueFieldExtension> extensions;
        private ReadOnlyDictionary<Type, ValueFieldExtension> readOnlyExtensions;

		#region Public Properties

        [Category("Design"),
			Description("The .NET datatype of the field.")]
		public ValueType ValueType
		{
            get { return valueType; }
			set
			{
				valueType = value;
				OnChanged(EventArgs.Empty);
			}
		}

        [Category("Design"),
            Description("The builder to use for generating code snippets."),
            DefaultValue(typeof(ValueTypeBuilder), "Default")]
        public ValueTypeBuilder Builder
        {
            get { return builder; }
            set
            {
                builder = value;
                OnChanged(EventArgs.Empty);
            }
        }

		[Category("Data"), DefaultValue(75),
			Description("Length of the field; only applies to length enabled data types.")]
		public int Length
		{
            get { return length; }
			set
			{
				length = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data"), DefaultValue(false),
			Description("Enables null values in the fields associated database column.")]
		public bool IsNullable
		{
            get { return isNullable; }
			set
			{
				isNullable = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data"), DefaultValue(""),
		Description("Overrides the datatype's null value. Use with caution!")]
		public string NullValue
		{
            get { return nullValue; }
			set
			{
				nullValue = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data"), DefaultValue(false),
		Description("Replaces a null field from the database with the default value.")]
		public bool UseDefaultValueOnNull
		{
            get { return useDefaultValueOnNull; }
			set
			{
				useDefaultValueOnNull = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data"), 
			Browsable(false),
			DefaultValue(false),
			Description("Specifies that this field is unique. (DO NOT USE!)")]
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

		[Category("Appearance"),
			Browsable(false),
			Description("Specifies the group name to associate the field to. This field is obsolete.")]
		public string Group
		{
            get { return group; }
			set
			{
				group = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Appearance"), 
			DefaultValue(""),
			Description("Specifies the default format string for displaying the field. " + 
				"If set, this will override the datatype's default format string.")]
		public string DefaultFormat
		{
            get { return defaultFormat; }
			set
			{
				defaultFormat = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data"),
			Description("Sets a default value to use in code; this must be understood by the language code" +
				"is output to.")]
		public string DefaultValue
		{
			get
			{
				if(defaultValue != string.Empty)
					return defaultValue;
				
				return valueType.DefaultValue;
			}
			set
			{
				defaultValue = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Data"), DefaultValue(false), 
			Description("Specifies that the field's associated database column should be indexed.")]
		public bool IsIndexed
		{
            get { return isIndexed; }
			set
			{
				isIndexed = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Code"), 
			DefaultValue(false),
			Description("Requires that the field's validation expression passes before saves to the " +
				"database.")]
		public bool ValidatorRequired
		{
            get { return validatorRequired; }
			set
			{
				validatorRequired = value;
				OnChanged(EventArgs.Empty);
			}
		}
		
		[Category("Code")]
		public string ValidatorRegEx
		{
            get { return validatorRegEx; }
			set
			{
				validatorRegEx = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Code")]
		public string ValidatorError
		{
            get { return validatorError; }
			set
			{
				validatorError = value;
				OnChanged(EventArgs.Empty);
			}
		}

        [Category("Appearance"),
            DefaultValue(false),
            Description("Allows the field to be viewed in grids; used by code generators.")]
        public bool IsClientGridEnabled
        {
            get { return isClientGridEnabled; }
            set
            {
                isClientGridEnabled = value;
                OnChanged(EventArgs.Empty);
            }
        }

		[Category("Appearance"),
			DefaultValue(true),
			Description("Allows the field to be edited in editors; used by code generators.")]
		public bool IsClientEditEnabled
		{
            get { return isClientEditEnabled; }
			set
			{
				isClientEditEnabled = value;
				OnChanged(EventArgs.Empty);
			}
		}

		[Category("Appearance"),
			DefaultValue(true),
			Description("Allows the field to be viewed in viewers; used by code generators.")]
		public bool IsClientViewEnabled
		{
            get { return isClientViewEnabled; }
			set
			{
				isClientViewEnabled = value;
				OnChanged(EventArgs.Empty);
			}
		}

        [Browsable(false)]
        public string TypeEditorControl
        {
            get { return valueType.TypeEditorPrefix + Name; }
        }

        [Browsable(false)]
        public string TypeValidatorControl
        {
            get { return valueType.ValidatorPrefix + Name; }
        }

		#endregion

        [Browsable(false)]
        public ReadOnlyDictionary<Type, ValueFieldExtension> Extensions
        {
            get { return readOnlyExtensions; }
        }

		public ValueField()
		{
			defaultValue = string.Empty;
            length = 75;
            extensions = new Dictionary<Type, ValueFieldExtension>();
            readOnlyExtensions = new ReadOnlyDictionary<Type, ValueFieldExtension>(extensions);
            isClientEditEnabled = true;
            isClientViewEnabled = true;
            builder = ValueTypeBuilder.Default;
		}

		public ValueField (XmlTextReader r, string version) : this()
		{
            if (r.Name != "FieldEntry")
            {
                throw new Exception(string.Format("Cannot load, expected FieldEntry, found '{0}'.",
                    r.Name));
            }

			r.MoveToAttribute("PropertyName"); 
			this.Name = r.Value;
			r.MoveToAttribute("CustomPrivateName");
			//this.customPrivateName = r.Value;
			CustomPrivateName = string.Empty;
			r.MoveToAttribute("CustomColumnName");
			//this.customColumnName = r.Value;
			CustomColumnName = string.Empty;
			r.MoveToContent();
			r.Read();
            
			valueType = DataTypeManager.FindFieldType(r.ReadElementString("DataType"));
            if (r.Name == "Builder")
                ValueTypeBuilder.Builders.TryGetValue(r.ReadElementString("Builder"), out builder);
            
            length = int.Parse(r.ReadElementString("Length"));
			defaultFormat = r.ReadElementString("DefaultFormat");
			defaultValue = r.ReadElementString("DefaultValue");
			Caption = r.ReadElementString("Caption");;
			Description = r.ReadElementString("Description");
			validatorRequired = bool.Parse(r.ReadElementString("ValidatorRequired"));
			validatorRegEx = r.ReadElementString("ValidatorRegEx");
			validatorError = r.ReadElementString("ValidatorError");
			isNullable = bool.Parse(r.ReadElementString("IsNullable"));
			if (r.LocalName == "NullValue") nullValue = r.ReadElementString("NullValue");
			if (r.LocalName == "UseDefaultValueOnNull") useDefaultValueOnNull = bool.Parse(r.ReadElementString("UseDefaultValueOnNull"));
			if (r.LocalName == "IsIndexed") isIndexed = bool.Parse(r.ReadElementString("IsIndexed"));
			if (r.LocalName == "IsUnique") isUnique = bool.Parse(r.ReadElementString("IsUnique"));
			if (r.LocalName == "Group") group = r.ReadElementString("Group");
            if (r.LocalName == "IsClientGridEnabled") isClientGridEnabled = bool.Parse(r.ReadElementString("IsClientGridEnabled"));
			if (r.LocalName == "IsClientEditEnabled") isClientEditEnabled = bool.Parse(r.ReadElementString("IsClientEditEnabled"));
			if (r.LocalName == "IsClientViewEnabled") isClientViewEnabled = bool.Parse(r.ReadElementString("IsClientViewEnabled"));

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
                        ValueFieldExtension newExtension = (ValueFieldExtension)
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
		}

		public override void WriteXml(XmlTextWriter w)
		{
            w.WriteStartElement("FieldEntry");
			w.WriteAttributeString("PropertyName",			Name);
			w.WriteAttributeString("CustomPrivateName",		CustomPrivateName);
			w.WriteAttributeString("CustomColumnName",		CustomColumnName);
			w.WriteElementString("DataType",				valueType.Name);
            w.WriteElementString("Builder",                 builder.Name);
			w.WriteElementString("Length",					length.ToString());
			w.WriteElementString("DefaultFormat",			defaultFormat);
			w.WriteElementString("DefaultValue",			defaultValue);
			w.WriteElementString("Caption",					Caption);
			w.WriteElementString("Description",				Description);
			w.WriteElementString("ValidatorRequired",		validatorRequired.ToString());
			w.WriteElementString("ValidatorRegEx",			validatorRegEx);
			w.WriteElementString("ValidatorError",			validatorError);
			w.WriteElementString("IsNullable",				isNullable.ToString());
			w.WriteElementString("NullValue",				nullValue);
			w.WriteElementString("UseDefaultValueOnNull",	useDefaultValueOnNull.ToString());
			w.WriteElementString("IsIndexed",				isIndexed.ToString());
			w.WriteElementString("IsUnique",				isUnique.ToString());
			w.WriteElementString("Group",					group);
            w.WriteElementString("IsClientGridEnabled", isClientGridEnabled.ToString());
			w.WriteElementString("IsClientEditEnabled", isClientEditEnabled.ToString());
			w.WriteElementString("IsClientViewEnabled",	isClientViewEnabled.ToString());
			
			w.WriteStartElement("MetaAttributes");
			if(attributes != null)
			{
				foreach(MetaAttribute attribute in attributes)
					attribute.WriteXml(w);
			}
			w.WriteEndElement();

            w.WriteStartElement("Extensions");
            foreach (ValueFieldExtension extension in extensions.Values)
            {
                extension.writeXml(w);
            }
            w.WriteEndElement();

			w.WriteEndElement();
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public ValueField Clone()
		{
			ValueField f = new ValueField();
			if(f.attributes != null)
				f.attributes = attributes.Clone();
			
			f.valueType = valueType;
			f.Name = Name;
			f.CustomColumnName = CustomPrivateName;
			f.CustomColumnName = CustomColumnName;
		
			f.length = length;
			f.defaultFormat = defaultFormat;
			f.defaultValue = defaultValue;
			f.Caption = Caption;
			f.Description = Description;
			f.validatorError = validatorError;
			f.validatorRegEx = validatorRegEx;
			f.validatorRequired = validatorRequired;
			f.isIndexed = isIndexed;
			f.isNullable = isNullable;
			f.isUnique = isUnique;
			f.group = group;
			
			return f;
		}

        public override string ToString()
        {
            return Name;
        }

        public ValueFieldExtension GetExtension(Type type)
        {
            ValueFieldExtension extension;

            if (type.BaseType != typeof(ValueFieldExtension))
                throw new Exception("Invalid extension type requested.");

            if (!extensions.TryGetValue(type, out extension))
            {
                extension = (ValueFieldExtension)
                    Activator.CreateInstance(type);

                extensions.Add(type, extension);
            }

            return extension;
        }
		
		#region IComparable Methods

		public int CompareTo(ValueField x)
		{
			return Name.CompareTo(x.Name);
		}

		int IComparable.CompareTo(object x)
		{
			return Name.CompareTo(((ValueField) x).Name);
		}

		#endregion
	}
}