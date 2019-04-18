using System;
using System.Collections;
using System.ComponentModel;
using System.Text;

namespace NitroCast.Core.UI
{
    /// <summary>
    /// Custom PropertyDescriptor
    /// </summary>
    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        CustomProperty property;

        public override Type ComponentType { get { return null; } }
        public override string Description { get { return property.Name; } }
        public override string Category { get { return string.Empty; } }
        public override string DisplayName { get { return property.Name; } }
        public override bool IsReadOnly { get { return property.ReadOnly; } }
        public override Type PropertyType { get { return property.Value.GetType(); } }

        public CustomPropertyDescriptor(ref CustomProperty property, Attribute[] attrs)
            : base(property.Name, attrs)
        {
            this.property = property;
        }

        #region PropertyDescriptor specific

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return property.Value;
        }

        public override void ResetValue(object component)
        {
            //Have to implement
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override void SetValue(object component, object value)
        {
            property.Value = value;
        }
        
        #endregion
    }
}