using System;
using NitroCast.Core;
using Microsoft.Win32;

namespace NitroCast
{
	/// <summary>
	/// Summary description for Configuration.
	/// </summary>
	public class Configuration
	{
//		private NitroCasterPluginAttribute[] plugins;

		public Configuration()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
		public void Install()
		{
			Registry.LocalMachine.CreateSubKey(@"\SOFTWARE\AMNS\");            
			Registry.LocalMachine.CreateSubKey(@"\SOFTWARE\AMNS\NitroCast");
			Registry.LocalMachine.CreateSubKey(@"\SOFTWARE\AMNS\NitroCast\1.0");			
            
			Registry.LocalMachine.CreateSubKey(@"\SOFTWARE\AMNS\NitroCast\1.0\ToolWindows");
		}

		public void Load()
		{

		}
	}
}
