using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NitroCast.Core;
using NitroCast.Core.Extensions;

namespace NitroCast.Extensions.Default
{
    public class WebGridReferenceFieldExtension : ReferenceFieldExtension
    {
        string gridFormat;
        bool gridEnabled;

        [Category("Web Grid"),
            Description("Specifies the formatting string for the value."),
            DefaultValue(true),
            Browsable(true)]
        public string GridFormat
        {
            get { return gridFormat; }
            set { gridFormat = value; }
        }

        [Category("Web Grid"),
            Description("Displays the item in the data grid."),
            DefaultValue(true),
            Browsable(true)]
        public bool GridEnabled
        {
            get { return gridEnabled; }
            set { gridEnabled = value; }
        }

        public WebGridReferenceFieldExtension()
            : base()
        {
            gridFormat = string.Empty;
            gridEnabled = false;
        }
    }
}