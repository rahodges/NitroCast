using System;
using System.ComponentModel;
using System.Text;

namespace NitroCast.Core
{
    public class RevisionTag
    {
        DataModelVersion _appendVersion;
        DataModelVersion _deleteVersion;

        [Category("Version Control"), Description("The version this was appended to the model.")]
        public DataModelVersion AppendVersion
        {
            get { return _appendVersion; }
            set { _appendVersion = value; }
        }

        [Category("Version Control"), Description("The version this was removed from the model.")]
        public DataModelVersion DeleteVersion
        {
            get { return _deleteVersion; }
            set { _deleteVersion = value; }
        }
    }
}
