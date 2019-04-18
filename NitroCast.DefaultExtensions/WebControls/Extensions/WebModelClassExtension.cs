using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NitroCast.Core.Extensions;
using NitroCast.Core;

namespace NitroCast.Extensions.Default
{
    public class WebModelClassExtension : ModelClassExtension
    {
        string outputPath;

        [Category("Web"),
            Description("The extensions path to output the control to.")]
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        public WebModelClassExtension()
            : base()
        {
            outputPath = string.Empty;
        }

        public static WebModelClassExtension Find(ModelClass c)
        {
            return (WebModelClassExtension)
                c.GetExtension(typeof(WebModelClassExtension));
        }
    }
}