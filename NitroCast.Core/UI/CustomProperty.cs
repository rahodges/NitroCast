using System;
using System.Collections.Generic;
using System.Text;

namespace NitroCast.Core.UI
{
    /// <summary>
    /// Custom property class 
    /// </summary>
    public class CustomProperty
    {
        private string sName = string.Empty;
        private bool bReadOnly = false;
        private bool bVisible = true;
        private object objValue = null;
        private string sDescription = string.Empty;

        public CustomProperty(string sName, object value, bool bReadOnly, bool bVisible)
        {
            this.sName = sName;
            this.objValue = value;
            this.bReadOnly = bReadOnly;
            this.bVisible = bVisible;
        }

        public bool ReadOnly { get { return bReadOnly; } }
        public string Name { get { return sName; } }
        public bool Visible { get { return bVisible; } }
        public object Value { get { return objValue; } set { objValue = value; } }
        public string Description { get { return sDescription; } set { sDescription = value; } }
    }
}