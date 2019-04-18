using System;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace NitroCast.Core
{
    public class ModelEntry : IModelEntry
    {
        Guid _id = Guid.Empty;
        string _name = string.Empty;
        string _caption = string.Empty;
        string _description = string.Empty;

        #region Public Properties

        [Category("Design"), Browsable(false), 
        Description("The global GUID for the field.")]
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Category("Design"), 
        Description("The name of the field.")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Category("Design"), 
        Description("The caption to use for the field in client controls.")]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        [Category("Design"), 
        Description("The description of the field for help in client controls.")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [Category("Code"), Browsable(false),
        Description("Specifies the name of the field in code templates.")]
        public string CodeName
        {
            get { return _name.Replace(" ", "_"); }
        }

        #endregion

        public virtual void ParseXml(XmlTextReader r)
        {            
            // Read Name
            r.MoveToAttribute("Name");
            _name = r.Value;
            r.MoveToContent();
            r.Read();

            // Get guid on element, if there is none, create a new one
            // This is used by several NitroCast components to save and load design state and
            // track datatypes.
            if (r.Name == "Guid")
            {
                ID = new Guid(r.ReadElementString("Guid"));
            }
            else
            {
                ID = Guid.NewGuid();
            }

            Caption = r.ReadElementString("Caption");
            Description = r.ReadElementString("Description");
        }

        public virtual void WriteXml(XmlTextWriter w)
        {
            w.WriteAttributeString("Name", _name);
            w.WriteElementString("Guid", _id.ToString());
            w.WriteElementString("Caption", _caption);
            w.WriteElementString("Description", _description);
        }
    }
}
