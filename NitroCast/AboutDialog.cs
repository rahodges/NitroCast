using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace NitroCast
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class AboutDialog : System.Windows.Forms.Form
    {
        private Button button1;
        private PictureBox pictureBox1;
        private Label versionLabel;
        private TextBox warningTextBox;
        private TextBox disclaimerTextBox;
        private Label label1;
        private Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.warningTextBox = new System.Windows.Forms.TextBox();
            this.disclaimerTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NitroCast.Properties.Resources.NitroCast_Setup_Banner;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(497, 71);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(9, 80);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(60, 13);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "Version 1.0";
            // 
            // warningTextBox
            // 
            this.warningTextBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.warningTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.warningTextBox.ForeColor = System.Drawing.Color.Black;
            this.warningTextBox.Location = new System.Drawing.Point(12, 169);
            this.warningTextBox.Multiline = true;
            this.warningTextBox.Name = "warningTextBox";
            this.warningTextBox.ReadOnly = true;
            this.warningTextBox.Size = new System.Drawing.Size(398, 52);
            this.warningTextBox.TabIndex = 4;
            // 
            // disclaimerTextBox
            // 
            this.disclaimerTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.disclaimerTextBox.Location = new System.Drawing.Point(12, 100);
            this.disclaimerTextBox.Multiline = true;
            this.disclaimerTextBox.Name = "disclaimerTextBox";
            this.disclaimerTextBox.ReadOnly = true;
            this.disclaimerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.disclaimerTextBox.Size = new System.Drawing.Size(470, 63);
            this.disclaimerTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(178, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Copyright © 2004-2008 Roy A.E. Hodges. All Rights Reserved.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(373, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "AMNS, NitroCast and NitroCast NitroGen are Trademarks of Roy A.E. Hodges";
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(494, 253);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.disclaimerTextBox);
            this.Controls.Add(this.warningTextBox);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About NitroCast";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AboutDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void AboutDialog_Load(object sender, EventArgs e)
        {
//			this.Focus();
//			this.Refresh();
//			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
//			this.Close();

            Assembly thisExe = Assembly.GetExecutingAssembly();
            string assemblyText = thisExe.GetName().Name;

            Version thisVersion = thisExe.GetName().Version;
            versionLabel.Text = string.Format("Version {0}.{1}.{2}.{3}", thisVersion.Major, thisVersion.Minor, thisVersion.Build,
                thisVersion.Revision);

            string copyrightText = "Copyright © 2003-2008 Roy A.E. Hodges. All Rights Reserved.";
            foreach (object attribute in thisExe.GetCustomAttributes(true))
            {
                if (attribute.GetType() == (typeof(AssemblyCopyrightAttribute)))
                {
                    copyrightText = ((AssemblyCopyrightAttribute)attribute).Copyright;
                    break;
                }
            }

            disclaimerTextBox.Text = Localization.Strings.Disclaimer;
            warningTextBox.Text = Localization.Strings.Warning;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
	}
}
