using System;
using System.Collections;
using System.ComponentModel;
using System.Text;

namespace NitroCast.Core.UI
{
    /// <summary>
    /// Custom PropertyDescriptor
    /// </summary>
    public class BoundPropertyDescriptor : PropertyDescriptor
    {
        object component;
        PropertyDescriptor descriptor;        

        public override Type ComponentType { get { return null; } }
        public override bool IsReadOnly { get { return descriptor.IsReadOnly; } }
        public override Type PropertyType { get { return descriptor.PropertyType; } }

        public BoundPropertyDescriptor(object component,
            PropertyDescriptor descriptor, 
            Attribute[] attrs)
            : base(descriptor, attrs)
        {
            this.component = component;
            this.descriptor = descriptor;
        }

        #region PropertyDescriptor specific

        public override bool CanResetValue(object component)
        {
            return descriptor.CanResetValue(this.component);
        }

        public override object GetValue(object component)
        {
            return descriptor.GetValue(this.component);
        }

        public override void ResetValue(object component)
        {
            descriptor.ResetValue(this.component);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return descriptor.ShouldSerializeValue(this.component);
        }

        public override void SetValue(object component, object value)
        {
            descriptor.SetValue(this.component, value);
        }
        
        #endregion
    }
}