using System;
using System.Xml;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DesignerState.
	/// </summary>
	public interface IDesignerState
	{        
		Guid ID { get; }
		void LoadDesignState(XmlTextReader r);
		void SaveDesignState(XmlTextWriter w);		
	}
}
