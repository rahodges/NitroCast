using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NitroCast.Core;
using NitroCast.Core.Extensions;

namespace NitroCast.Extensions.Default
{
    public class WebEditorReferenceFieldExtension : ReferenceFieldExtension
    {
        bool editorEnabled;
        string editorNote;

        string listText;

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

        [Category("Web ListItem"),
            Description("The source code for binding ListItems to DropDownLists, " +
                "CheckBoxLists and ComboBoxes."),
            DefaultValue("ToString()")]
        public string ListText
        {
            get { return listText; }
            set { listText = value; }
        }

        public WebEditorReferenceFieldExtension()
            : base()
        {
            editorEnabled = true;
            editorNote = string.Empty;
            listText = "ToString()";
        }

        public static WebEditorReferenceFieldExtension Find(ReferenceField f)
        {
            return (WebEditorReferenceFieldExtension)
                f.GetExtension(typeof(WebEditorReferenceFieldExtension));
        }
    }
}
