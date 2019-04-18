using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for ReferenceEntry.
	/// </summary>
	public class ReferenceEntry : IComparable
	{
		private string name = string.Empty;
		private string fileName = string.Empty;

		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				if(fileName != value)
				{
					fileName = value;			
					name = string.Empty;
				}
			}
		}

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

		public ReferenceEntry(string fileName)
		{
            this.fileName = fileName;			
		}

		public ReferenceEntry (XmlTextReader r)
		{
			if(r.Name != "ReferenceEntry")
				throw(new Exception(string.Format("Cannot load, expected ReferenceEntry, found '{0}'.",
					r.Name)));

			r.MoveToContent();
			r.Read();
			fileName = r.ReadElementString("FileName");
			r.ReadEndElement();
		}

		public void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ReferenceEntry");
			w.WriteElementString("FileName", fileName);
			w.WriteEndElement();
		}

		#region IComparer Methods

		public int CompareTo(ReferenceEntry x)
		{
			return name.CompareTo(x.Name);
		}

		int IComparable.CompareTo(object x)
		{
			return name.CompareTo(((ReferenceEntry) x).name);
		}

		#endregion
	}
}
