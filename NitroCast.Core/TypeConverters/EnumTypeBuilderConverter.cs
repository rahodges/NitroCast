using System;
using System.Collections.Generic;
using System.ComponentModel;
using NitroCast.Core.Extensions;

namespace NitroCast.Core.TypeConverters
{
	/// <summary>
	/// Summary description for EnumTypeConverter.
	/// </summary>
	public class EnumTypeBuilderConverter : StringConverter
	{
        // Source DataTypes Collection
        private static List<EnumTypeBuilder> builders;

        public static void Init(List<EnumTypeBuilder> builders)
        {
            EnumTypeBuilderConverter.builders = builders;
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
            return new StandardValuesCollection(builders);
        }

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
            System.Type sourceType)
        {
            if (sourceType == typeof(EnumType))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object val)
        {
            string name = val.ToString();

            for (int x = 0; x < builders.Count; x++)
                if (builders[x].Name == name)
                    return builders[x];

            throw (new NotSupportedException("The text could not be converted to a supported type."));
        }
	}
}
