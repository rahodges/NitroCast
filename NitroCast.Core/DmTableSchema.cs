using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for DmTableSchema.
	/// </summary>
	public class DmTableSchema
	{		
        //private Guid _guid;
		private string _name;
		private string _description;

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

		public DmTableSchema()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
