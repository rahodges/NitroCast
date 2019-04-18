using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NitroCast.Core.Extensions;
using NitroCast.Core;

namespace NitroCast.Extensions.Default
{
    public class WebEditorEnumFieldExtension : EnumFieldExtension
    {
        bool editorEnabled;
        string editorNote;

        [Category("Web Editor"),
            Description("Enables the editing of the field in the editor."),
            DefaultValue(true)]
        public bool EditorEnabled
        {
            get { return editorEnabled; }
            set { editorEnabled = value; }
        }

        [Category("Web Editor"),
            Description("A note to display under the field in the editor.")]            
        public string EditorNote
        {
            get { return editorNote; }
            set { editorNote = value; }
        }

        public WebEditorEnumFieldExtension()
            : base()
        {
            editorEnabled = true;
            editorNote = string.Empty;
        }

        public static WebEditorEnumFieldExtension Find(EnumField f)
        {
            return (WebEditorEnumFieldExtension)
                f.GetExtension(typeof(WebEditorEnumFieldExtension));
        }
    }
}