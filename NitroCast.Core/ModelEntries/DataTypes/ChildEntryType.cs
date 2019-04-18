using System;

namespace DbModel.Core.ModelEntries
{
	/// <summary>
	/// Summary description for ChildEntryType.
	/// </summary>
	public class ChildEntryType
	{
		string _name;
		bool _isCollectionHandler;

		string _editorControls;			// Code to define the editor controls
		string _editorCreate;			// Code to instantiate and configure the editor controls
		string _editorSave;				// Code to save the editor controls
		string _editorLoad;				// Code to load the editor controls
		string _editorRender;			// Code to render the editor

		public ChildEntryType()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
