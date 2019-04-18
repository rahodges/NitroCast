using System;

namespace NitroCast.Core.Extensions
{
	/// <summary>
	/// Summary for NitroCasterExtensionAttribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ExtensionAttribute : Attribute
	{
		string name;
		string author;
		string copyright;
        bool isWebControl;
		string outputFileNameFormat;
		string description;
		string extensionPath;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Author
		{
			get
			{
				return author;
			}
			set
			{
				author = value;
			}
		}

		public string Copyright
		{
			get
			{
				return copyright;
			}
			set
			{
				copyright = value;
			}
		}

		public string OutputFileNameFormat
		{
			get
			{
				return outputFileNameFormat;
			}
			set
			{
				outputFileNameFormat = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public string ExtensionPath
		{
			get
			{
				return extensionPath;
			}
			set
			{
				extensionPath = value;
			}
		}

        public bool IsWebControl
        {
            get { return isWebControl; }
            set { isWebControl = value; }
        }

		public ExtensionAttribute(string name, string author, string copyright,
			string outputFileNameFormat, string description, string extensionPath)
		{
			this.name = name;
			this.author = author;
			this.copyright = copyright;
			this.outputFileNameFormat = outputFileNameFormat;
			this.description = description;
			this.extensionPath = extensionPath;
            this.isWebControl = false;
		}

        public ExtensionAttribute(string name, string author, string copyright, 
            string outputFileNameFormat, string description, string extensionPath,
            bool isWebControl)
        {
            this.name = name;
            this.author = author;
            this.copyright = copyright;
            this.outputFileNameFormat = outputFileNameFormat;
            this.description = description;
            this.extensionPath = extensionPath;
            this.isWebControl = isWebControl;
        }
	}
}