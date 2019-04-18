using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for NitroCastUtilities.
	/// </summary>
	public class NitroCastUtilities
	{
		public NitroCastUtilities()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static System.Drawing.Icon GetIcon(string identifier)
		{
			return new System.Drawing.Icon(new System.IO.StreamReader(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(identifier)).BaseStream);
		}

//		Private Function GetIcon(ByVal strIdentifier As String) As System.Drawing.Icon
//																			  ' use the strIdentifier argument to retrieve the 
//		' appropriate resource from the assembly
//		With New System.IO.StreamReader( _
//											 [Assembly].GetEntryAssembly. _
//															GetManifestResourceStream(strIdentifier))
//		' read the resource from the returned stream
//		GetIcon = New System.Drawing.Icon(.BaseStream)
//		' close the stream
//		.Close()
//			 End With
//				 End Function

	}
}
