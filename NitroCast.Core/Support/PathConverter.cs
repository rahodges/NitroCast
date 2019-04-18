using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace NitroCast.Core
{
    public class PathConverter
    {
        const UInt32 FILE_ATTRIBUTE_DIRECTORY = 0x10;
        const Int32 MAX_PATH = 260;

        [DllImport("shlwapi", CharSet = CharSet.Auto)]
        static extern bool PathCombine(
            [Out] StringBuilder lpszDest,
            [In] string lpszDir,
            [In] string lpszFile
        );

        public static string GetAbsolutePath(string baseDirectory, string relativeFilename)
        {
            StringBuilder str = new StringBuilder(MAX_PATH);
            PathCombine(str, baseDirectory, relativeFilename);
            return str.ToString();
        }

        [DllImport("shlwapi.dll", CharSet=CharSet.Auto)]
        static extern bool PathRelativePathTo(
            [Out] StringBuilder pszPath, 
            [In] string pszFrom,
            [In] uint dwAttrFrom, 
            [In] string pszTo, 
            [In] uint dwAttrTo
        );

        public static string GetRelativePath(string baseDirectory, string absoluteFilename)
        {
            StringBuilder str = new StringBuilder(MAX_PATH);
            UInt32 dwAttr1 = FILE_ATTRIBUTE_DIRECTORY;
            UInt32 dwAttr2 = 0;
            Boolean bRet = PathRelativePathTo(str, baseDirectory, dwAttr1, absoluteFilename, dwAttr2);
            return str.ToString();
        }
    }
}
