using System;
using System.Xml;

namespace NitroCast.Core.Extensions
{
	/// <summary>
	/// Marks plugins with the ability to generate code from the model.
	/// </summary>
	public interface IOutputExtension
	{
		string CustomCode
		{
			get;
			set;
		}

		void	Init(ModelClass c, XmlDocumentFragment config);
		void	PreExecute();
		string	Render();
		void	Execute();
	}
}
