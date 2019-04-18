using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NitroCast
{    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            if (args.Length > 0)
                Application.Run(new MainForm(args[0]));
            else
                Application.Run(new MainForm());
        }
    }
}