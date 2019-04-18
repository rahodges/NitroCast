using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NitroCast.Core;
using NitroCast.Core.Extensions;

namespace NitroCast.Extensions.Default
{
    public class WebGridEnumFieldExtension : EnumFieldExtension
    {
        bool gridEnabled;
                
        [Category("Web Grid"),
            Description("Displays the item in the data grid."),
            DefaultValue(true),
            Browsable(true)]
        public bool GridEnabled
        {
            get { return gridEnabled; }
            set { gridEnabled = value; }
        }

        public WebGridEnumFieldExtension()
            : base()
        {
            gridEnabled = false;
        }
    }
}