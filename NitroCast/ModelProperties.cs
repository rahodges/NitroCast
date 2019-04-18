using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NitroCast.Core;

namespace NitroCast
{
	/// <summary>
	/// Summary description for ModelProperties.
	/// </summary>
	public class ModelProperties : Form
	{
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox tbDefaultNamespace;
		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Button btApply;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescription;
        private TextBox connectionStringTextBox;
        private Label connectionLabel;
        private CheckBox connectionConfigKeyCheckBox;
        private CheckBox connectionStringCodedCheckBox;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button DBPrefixButton;
		private System.Windows.Forms.Label label4;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;

		private DataModel model;

		public ModelProperties(DataModel model)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.model = model;
			this.model.Editor = this;
			
			//
			// Initialize Fields
			//
			tbName.Text = model.Name;
			tbDefaultNamespace.Text = model.DefaultNamespace;
			tbDescription.Text = model.Description;
            connectionStringTextBox.Text = model.ConnectionString;
            connectionStringCodedCheckBox.Checked = model.ConnectionStringIsCoded;
            connectionConfigKeyCheckBox.Checked = model.ConnectionStringIsConfigKey;

            // Setup Control States
            connectionConfigKeyCheckBox.Enabled = connectionStringCodedCheckBox.Checked;
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDefaultNamespace = new System.Windows.Forms.TextBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btApply = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.connectionConfigKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.connectionStringCodedCheckBox = new System.Windows.Forms.CheckBox();
            this.connectionStringTextBox = new System.Windows.Forms.TextBox();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.DBPrefixButton = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(9, 25);
            this.tbName.MaxLength = 255;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(216, 20);
            this.tbName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Default Namespace";
            // 
            // tbDefaultNamespace
            // 
            this.tbDefaultNamespace.Location = new System.Drawing.Point(9, 65);
            this.tbDefaultNamespace.MaxLength = 255;
            this.tbDefaultNamespace.Name = "tbDefaultNamespace";
            this.tbDefaultNamespace.Size = new System.Drawing.Size(216, 20);
            this.tbDefaultNamespace.TabIndex = 3;
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(153, 232);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 4;
            this.btOk.Text = "OK";
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(233, 232);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "Cancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btApply
            // 
            this.btApply.Location = new System.Drawing.Point(313, 232);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(75, 23);
            this.btApply.TabIndex = 6;
            this.btApply.Text = "Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Description";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(9, 105);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(353, 75);
            this.tbDescription.TabIndex = 8;
            // 
            // connectionConfigKeyCheckBox
            // 
            this.connectionConfigKeyCheckBox.Location = new System.Drawing.Point(7, 81);
            this.connectionConfigKeyCheckBox.Name = "connectionConfigKeyCheckBox";
            this.connectionConfigKeyCheckBox.Size = new System.Drawing.Size(77, 17);
            this.connectionConfigKeyCheckBox.TabIndex = 3;
            this.connectionConfigKeyCheckBox.Text = "Config Key";
            // 
            // connectionStringCodedCheckBox
            // 
            this.connectionStringCodedCheckBox.Location = new System.Drawing.Point(7, 57);
            this.connectionStringCodedCheckBox.Name = "connectionStringCodedCheckBox";
            this.connectionStringCodedCheckBox.Size = new System.Drawing.Size(138, 17);
            this.connectionStringCodedCheckBox.TabIndex = 2;
            this.connectionStringCodedCheckBox.Text = "Code Connection String";
            this.connectionStringCodedCheckBox.CheckedChanged += new System.EventHandler(this.connectionStringCodedCheckBox_CheckedChanged);
            // 
            // connectionStringTextBox
            // 
            this.connectionStringTextBox.Location = new System.Drawing.Point(6, 25);
            this.connectionStringTextBox.Name = "connectionStringTextBox";
            this.connectionStringTextBox.Size = new System.Drawing.Size(353, 20);
            this.connectionStringTextBox.TabIndex = 1;
            // 
            // connectionLabel
            // 
            this.connectionLabel.AutoSize = true;
            this.connectionLabel.Location = new System.Drawing.Point(6, 9);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(128, 13);
            this.connectionLabel.TabIndex = 0;
            this.connectionLabel.Text = "Default Connection String";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Default Database Prefix";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(136, 20);
            this.textBox1.TabIndex = 1;
            // 
            // DBPrefixButton
            // 
            this.DBPrefixButton.Location = new System.Drawing.Point(150, 23);
            this.DBPrefixButton.Name = "DBPrefixButton";
            this.DBPrefixButton.Size = new System.Drawing.Size(56, 23);
            this.DBPrefixButton.TabIndex = 0;
            this.DBPrefixButton.Text = "Reset";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Location = new System.Drawing.Point(6, 7);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(382, 219);
            this.tabControl2.TabIndex = 10;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbDescription);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.tbName);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.tbDefaultNamespace);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(374, 193);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "General";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.connectionConfigKeyCheckBox);
            this.tabPage5.Controls.Add(this.connectionLabel);
            this.tabPage5.Controls.Add(this.connectionStringCodedCheckBox);
            this.tabPage5.Controls.Add(this.connectionStringTextBox);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(374, 193);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Database Connection";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.label4);
            this.tabPage6.Controls.Add(this.textBox1);
            this.tabPage6.Controls.Add(this.DBPrefixButton);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(374, 193);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Database Prefix";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // ModelProperties
            // 
            this.AcceptButton = this.btApply;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(393, 260);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelProperties";
            this.Text = "Model Properties";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ModelProperties_Load);
            this.Closed += new System.EventHandler(this.ModelProperties_Closed);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void saveSettings()
		{
			model.Name = tbName.Text;
			model.DefaultNamespace = tbDefaultNamespace.Text;
			model.Description = tbDescription.Text;
            model.ConnectionString = connectionStringTextBox.Text;
            model.ConnectionStringIsCoded = connectionStringCodedCheckBox.Checked;
            model.ConnectionStringIsConfigKey = connectionConfigKeyCheckBox.Checked;
		}

		private void btOk_Click(object sender, System.EventArgs e)
		{
            saveSettings();
			this.Close();
		}

		private void btCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btApply_Click(object sender, System.EventArgs e)
		{
			saveSettings();
		}

		private void ModelProperties_Closed(object sender, System.EventArgs e)
		{
			model.Editor = null;
		}

        private void ModelProperties_Load(object sender, EventArgs e)
        {

        }

        private void connectionStringCodedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            connectionConfigKeyCheckBox.Enabled = connectionStringCodedCheckBox.Checked;
        }
	}
}
