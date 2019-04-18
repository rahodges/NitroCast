using System;
using System.Collections.Generic;
using System.Data;
using NitroCast.Core.Extensions;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DataDataTypeManager.
	/// </summary>
	public class DataTypeManager
	{
		private static ValueTypeCollection valueTypes;
		private static ReferenceTypeCollection referenceTypes;
        private static EnumTypeCollection enumTypes;

        public static ValueTypeCollection ValueTypes { get { return valueTypes; } }
        public static ReferenceTypeCollection ReferenceTypes { get { return referenceTypes; } }
        public static EnumTypeCollection EnumTypes { get { return enumTypes; } }      
        
		public static void Init()
		{
			//
			// Instantiate type collections.
			//
            valueTypes = Make(null);
			referenceTypes = new ReferenceTypeCollection();
            enumTypes = new EnumTypeCollection();
			
			// ValueTypes load at startup and never to be
            // reloaded, only TypeConverters for References and
            // enums need reloaded ???
            TypeConverters.ValueTypeConverter.Init(valueTypes);            
            initTypeConverters();
		}

		public static void Clear()
		{
            // ValueTypes DO NOT NEED CLEARED!!!
			referenceTypes.Clear();
            enumTypes.Clear();
            initTypeConverters();
		}

        private static void initTypeConverters()
        {
            TypeConverters.ReferenceTypeConverter.Init(referenceTypes);            
            TypeConverters.EnumTypeConverter.Init(enumTypes);
        }

        public static ValueType AddDataType(ValueType f)
		{
			for(int i = 0; i < valueTypes.Count; i++)
				if(valueTypes[i].Name == f.Name)
					throw(new Exception("FieldDataType already exists in type manager."));
			valueTypes.Add(f);
            return f;
		}

        public static ReferenceType AddDataType(ReferenceType c)
        {
            for (int i = 0; i < referenceTypes.Count; i++)
            {
                if (referenceTypes[i].Equals(c) | referenceTypes[i].Name == c.Name)
                {
                    if (referenceTypes[i].ParentClassEntry == null &
                        c.ParentClassEntry != null)
                    {
                        referenceTypes[i].ParentClassEntry = c.ParentClassEntry;
                    }

                    if (referenceTypes[i].ParentReferenceEntry == null &
                        c.ParentReferenceEntry != null)
                    {
                        referenceTypes[i].ParentReferenceEntry = c.ParentReferenceEntry;
                    }

                    return referenceTypes[i];
                }
            }

            referenceTypes.Add(c);

            return c;
        }

        public static EnumType AddDataType(EnumType e)
        {
            for (int i = 0; i < enumTypes.Count; i++)
            {
                if (enumTypes[i].Equals(e) | enumTypes[i].Name == e.Name)
                {
                    if (enumTypes[i].ParentEnumEntry == null &
                        e.ParentEnumEntry != null)
                    {
                        enumTypes[i].ParentEnumEntry = e.ParentEnumEntry;
                    }

                    if (enumTypes[i].ParentReferenceEntry == null &
                        e.ParentReferenceEntry != null)
                    {
                        enumTypes[i].ParentReferenceEntry = e.ParentReferenceEntry;
                    }

                    return enumTypes[i];
                }
            }

            enumTypes.Add(e);

            return e;
        }

        public static void DeleteDataType(ModelClass modelClass)
        {
            ReferenceType refType = null;

            for(int i = 0; i < referenceTypes.Count; i++)
            {
                if(referenceTypes[i].ParentClassEntry == modelClass)
                    refType = referenceTypes[i];
            }

            if (refType != null)
            {
                DeleteDataType(refType);
            }
        }

        public static void DeleteDataType(ValueType type)
        {
            valueTypes.Remove(type);
        }

        public static void DeleteDataType(ReferenceType type)
        {
            referenceTypes.Remove(type);
        }

        public static void DeleteDataType(EnumType type)
        {
            enumTypes.Remove(type);
        }

		public static ValueType FindFieldType(string name)
		{
			foreach(ValueType fType in valueTypes)
				if(fType.Name.Equals(name))
					return fType;
			throw new Exception(string.Format("Cannot find field type '{0}'.", name));
		}
		
		public static ReferenceType FindChildType(string name)
		{
			foreach(ReferenceType cType in referenceTypes)
				if(cType.Name == name)
					return cType;
			return null;
		}

        public static EnumType FindEnumType(string name)
        {
            foreach (EnumType cType in enumTypes)
                if (cType.Name == name)
                    return cType;
            return null;
        }
        
		public static ValueTypeCollection Make(ValueTypeBuilder builder)
		{
            ValueType fdt;
            ValueTypeCollection values;

            values = new ValueTypeCollection(20);

            #region bool
                                    
            fdt = new ValueType("bool", "bool", "Boolean", 
                "DbType.Boolean", "BIT", "BIT", 
				"{0}.GetBoolean({1})", "{0}",
				"System.Web.UI.WebControls", "CheckBox", "cb",
				"{0}{1}.Checked = {2}.{1};", "{2}.{1} = {0}{1}.Checked;");
            if (builder != null) 
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region byte

            fdt = new ValueType("byte", "byte", "Byte",
                "DbType.Byte", "BYTE", "TINYINT",
                "{0}.GetByte({1})", "{0}",
                "System.Web.UI.WebControls", "TextBox", "tb",
                "{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = byte.Parse({0}{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);
            
            #endregion

            #region char 

            fdt = new ValueType("char", "char", "Char", 
                "DbType.String", "TEXT(1)", "NVARCHAR(1)", 
				"{0}.GetChar({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = char.Parse({0}{1}.Text);",
				false, true, string.Empty);
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region decimal

            fdt = new ValueType("decimal", "decimal", "Decimal",
                "DbType.Currency", "CURRENCY", "MONEY",
                "{0}.GetDecimal({1})", "{0}",
                "System.Web.UI.WebControls", "TextBox", "tb",
                "{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = decimal.Parse({0}{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region double
            
            fdt = new ValueType("double", "double", "Double", 
                "DbType.Double", "DOUBLE", "FLOAT", 
				"{0}.GetDouble({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = double.Parse({0}{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region float
            
            fdt = new ValueType("float", "float", "Single",
                "DbType.Single", "SINGLE", "REAL", 
				"{0}.GetFloat({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = float.Parse({0}{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region short

            fdt = new ValueType("short", "short", "Int16",
                "DbType.Int16", "SHORT", "SMALLINT",
                "{0}.GetInt16({1})", "{0}",
                "System.Web.UI.WebControls", "TextBox", "tb",
                "{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = short.Parse({0}{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region int

            fdt = new ValueType("int", "int", "Int32",
                "DbType.Int32", "LONG", "INT", 
				"{0}.GetInt32({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = int.Parse({0}{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region string

            fdt = new ValueType("string", "string", "String", 
                "DbType.String", "TEXT({0})", "NVARCHAR({0})", 
				"{0}.GetString({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1};", "{2}.{1} = {0}{1}.Text;",
				true, true, string.Empty);
			fdt.NullValue = "null";
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region string (MEMO)

            fdt = new ValueType("string (MEMO)", "string", "String", 
                "DbType.String", "MEMO", "NTEXT",
				"{0}.GetString({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
                "{0}{1}.Text = {2}.{1};", "{2}.{1} = {0}{1}.Text;",
				false, true, string.Empty);
            fdt.NullValue = "null";
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region string (MEMO) - FTB

            fdt = new ValueType("string (MEMO) - FTB", "string", "String",
                "DbType.String", "MEMO", "NTEXT",
				"{0}.GetString({1})", "{0}",
				"FreeTextBoxControls", "FreeTextBox", "ftb",
                "{0}{1}.Text = {2}.{1};", "{2}.{1} = {0}{1}.Text;",
				false, true, string.Empty);
			fdt.EditorProperties = new System.Collections.Generic.List<string>();
            fdt.EditorProperties.Add("ftb{0}.ID = this.ID + \"_{0}\";");
            fdt.EditorProperties.Add("ftb{0}.Width = Unit.Percentage(100);");
			fdt.NullValue = "null"; // = "string.Empty
            values.Add(fdt);

            #endregion

            #region string (MEMO) - FCKeditor

            fdt = new ValueType("string (MEMO) - FCKeditor", "string", "String",
                "DbType.String", "MEMO", "NTEXT", 
				"{0}.GetString({1})", "{0}",
				"FredCK", "FCKeditor", "fcke",
                "{0}{1}.Value = {2}.{1};", "{2}.{1} = {0}{1}.Value;",
				false, true, string.Empty);
			fdt.EditorProperties = new System.Collections.Generic.List<string>();
            fdt.EditorProperties.Add("fcke{0}.ID = this.ID + \"_{0}\";");
            fdt.EditorProperties.Add("fcke{0}.Width = Unit.Percentage(100);");
			fdt.NullValue = "null"; // = "string.Empty";
            values.Add(fdt);

            #endregion

            #region DateTime

            fdt = new ValueType("DateTime", "DateTime", "DateTime",
                "DbType.Date", "DATETIME", "DATETIME", 
				"{0}.GetDateTime({1})", "{0}",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = DateTime.Parse({0}{1}.Text);");
			fdt.NullValue = "DateTime.MinValue";
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region DateTime (DateEditor)

            fdt = new ValueType("DateTime (DateEditor)", "DateTime", 
                "DateTime", "DbType.Date", "DATETIME", "DATETIME", 
				"{0}.GetDateTime({1})", "{0}",
				"Amns.GreyFox.Web.UI.WebControls", "DateEditor", "de",
				"{0}{1}.Date = {2}.{1};", "{2}.{1} = {0}{1}.Date;");
			fdt.EditorProperties = new System.Collections.Generic.List<string>();
            fdt.EditorProperties.Add("de{0}.ID = this.ID + \"_{0}\";");
            fdt.EditorProperties.Add("de{0}.AutoAdjust = true;");
			fdt.NullValue = "DateTime.MinValue";
            values.Add(fdt);

            #endregion

            #region TimeSpan

            fdt = new ValueType("TimeSpan", "TimeSpan", "TimeSpan", 
                "DbType.Double", "DOUBLE", "FLOAT", 
				"TimeSpan.FromTicks((long) {0}.GetDouble({1}))", "{0}.Ticks",
				"System.Web.UI.WebControls", "TextBox", "tb",
				"{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = TimeSpan.Parse({0}{1}.Text);");
			fdt.NullValue = "TimeSpan.Zero";
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            #region GUID
                        
            fdt = new ValueType("Guid", "Guid", "Guid",
                "DbType.Guid", "GUID", "UNIQUEIDENTIFIER", 
                "{0}.GetGuid({1})", "{0}",
                "System.Web.UI.WebControls", "TextBox", "tb",
                "{0}{1}.Text = {2}.{1}.ToString();", "{2}.{1} = new Guid({0}.{1}.Text);");
            if (builder != null)
                builder.SetEditorProperties(fdt);
            values.Add(fdt);

            #endregion

            //fieldTypes.Add(new FieldDataType("object", "BINARY", false, "{0}.GetBytes({1},0,{2},0,int.MaxValue)"));

            return values;
		}
	}
}
