using System;
using System.Collections;
using System.ComponentModel;

namespace NitroCast.Core.TypeConverters
{
	/// <summary>
	/// Summary description for ChildDataTypeConverter.
	/// </summary>
	public class ReferenceTypeConverter : StringConverter
	{
		// Source DataTypes Collection
		private static ReferenceTypeCollection referenceDataTypes;

		public static void Init(ReferenceTypeCollection childDataTypes)
		{
			ReferenceTypeConverter.referenceDataTypes = childDataTypes;
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
			return new StandardValuesCollection(referenceDataTypes);	
		}

		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, 
			System.Type sourceType)
		{
			if(sourceType == typeof(ReferenceType))
				return true;
			else 
				return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object val)
		{
			string name = val.ToString();

			for(int x = 0; x < referenceDataTypes.Count; x++)
				if(referenceDataTypes[x].Name == name)
					return referenceDataTypes[x];
			
			throw(new NotSupportedException("The text could not be converted to a supported type."));
		}
	}
}