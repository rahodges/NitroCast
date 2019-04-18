using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Classes that inherit this class "author" code.
	/// </summary>
	public class DmTableSchemaAuthor
	{
        //private Guid _guid;
		private string _name;
		private string _description;

		#region properties

        //public Guid Guid
        //{
        //    get { return _guid; }
        //}

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

		public DmTableSchemaAuthor()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
