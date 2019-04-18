using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DmProject.
	/// </summary>
	public class DmProject
	{
		private string _name;
		private string _description;

        //private DmProjectUserOptions _options;
        //private DmTableSchema[] _schemas;

		#region properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		#endregion

		public DmProject()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
