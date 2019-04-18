using System;

namespace NitroCast.Core
{	
	public delegate void DataModelEventHandler(object sender, DataModelEventArgs e);

	/// <summary>
	/// Summary description for DataModelEventArgs.
	/// </summary>
	public class DataModelEventArgs : EventArgs
	{
		string				__text;
		string				__description;
		string				__eventClass;

		ProgressBarConfig	__progressConfig;

		public string Text
		{
			get { return __text; }
		}

		public string Description
		{
			get { return __description; }
		}

		public string EventClass
		{
			get { return __eventClass; }
		}

		public ProgressBarConfig ProgressConfig
		{
			get { return __progressConfig; }
		}

		public DataModelEventArgs(string text, string description, string eventClass)
		{
			__text = text;
			__description = description;
			__eventClass = eventClass;
			__progressConfig = null;
		}

		public DataModelEventArgs(string text, string description, string eventClass, ProgressBarConfig progressConfig)
		{
			__text = text;
			__description = description;
			__eventClass = eventClass;
			__progressConfig = progressConfig;
		}
	}
}
