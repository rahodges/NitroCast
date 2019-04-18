using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NitroCast.Core.UI
{
    public class DoubleBufferForm : Form
    {
        public DoubleBufferForm()
        {
            EnableDoubleBuffer();
        }

        public void EnableDoubleBuffer()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint,
               true);
            this.UpdateStyles();
        }
    }
}
