using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml;

namespace NitroCast.Core.Extensions
{
	/// <summary>
	/// Summary description for ClassOutputConnector.
	/// </summary>
	public class OutputExtensionConnector
	{		
		ModelClass				_parentClassEntry;
		OutputExtension		_parentPlugin;
		NameValueCollection 	_config;

		public OutputExtension ParentPlugin
		{
			get { return _parentPlugin; }
		}
                       
		public OutputExtensionConnector(ModelClass classEntry, OutputExtension outputPlugin)
		{
			_parentClassEntry = classEntry;
			_parentPlugin = outputPlugin;
			_config = new NameValueCollection();
		}

		public void ExecutePlugin()
		{
			_parentPlugin.Init(_parentClassEntry, _config);
			_parentPlugin.Execute();
		}

		public OutputExtensionConnector(XmlTextReader r, ModelClass classEntry)
		{
			_parentClassEntry = classEntry;
			_config = new NameValueCollection();

			if(r.Name != "ClassOutputConnector")
				throw new Exception(string.Format("Source file does not match NitroCast DTD; " +
					"expected 'ClassOutputConnector', found '{0}'.", r.Name));

			r.MoveToContent();
			string pluginName = r.ReadElementString("ParentPlugin");

			ExtensionManager m = ExtensionManager.GetInstance();

            _parentPlugin = m.OutputExtensions[pluginName];

			if(_parentPlugin == null)
				throw new Exception(string.Format("Cannot connect to OutputPlugin '{0}'.",
					pluginName));

			if(r.Name == "Config")
			{
				if(!r.IsEmptyElement)
				{
					r.Read();
					while(r.NodeType != XmlNodeType.EndElement)
						_config.Add(r.Name, r.ReadElementString(r.Name));
					r.ReadEndElement();
				}
				else
					r.ReadEndElement();
			}

			r.ReadEndElement();
		}

		public void WriteXml(XmlTextWriter w)
		{
			w.WriteStartElement("ClassOutputConnector");
			w.WriteElementString("ParentPlugin", _parentPlugin.Name);		
			w.WriteStartElement("Config");
			for(int x = 0; x < _config.Count; x++)
				w.WriteElementString(_config.Keys[x], _config[x]);
			w.WriteEndElement();
			w.WriteEndElement();
		}
	}
}