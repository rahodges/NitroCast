using System;
using System.Collections;
using System.ComponentModel;

namespace NitroCast.Core.TypeConverters
{
	/// <summary>
	/// Summary description for FieldDataTypeConverter.
	/// </summary>
	public class ValueTypeConverter : StringConverter
	{
		// Source DataTypes Collection
		private static ValueTypeCollection fieldDataTypes;

		public static void Init(ValueTypeCollection fieldDataTypes)
		{
			ValueTypeConverter.fieldDataTypes = fieldDataTypes;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(fieldDataTypes);	
		}

		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
		{
			if(sourceType == typeof(ValueType))
				return true;
			else 
				return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object val)
		{
			string name = val.ToString();

			for(int x = 0; x < fieldDataTypes.Count; x++)
				if(fieldDataTypes[x].Name == name)
					return fieldDataTypes[x];
			
			throw(new NotSupportedException("The text could not be converted to a supported type."));
		}
	}
}
