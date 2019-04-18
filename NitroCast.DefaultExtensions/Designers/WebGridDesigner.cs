using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NitroCast.DefaultPlugins.Designers
{
	/// <summary>
	/// Summary description for WebGridDesigner.
	/// </summary>
	public class WebGridDesigner : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ListBox listBox3;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WebGridDesigner()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.listBox3 = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(232, 104);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.Text = "comboBox1";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(232, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Search Drop Downs";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(136, 16);
			this.button1.Name = "button1";
			this.button1.TabIndex = 4;
			this.button1.Text = "<<";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(136, 48);
			this.button2.Name = "button2";
			this.button2.TabIndex = 5;
			this.button2.Text = ">>";
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 16);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 121);
			this.listBox1.TabIndex = 6;
			// 
			// listBox2
			// 
			this.listBox2.Location = new System.Drawing.Point(216, 16);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(120, 121);
			this.listBox2.TabIndex = 7;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(136, 80);
			this.button3.Name = "button3";
			this.button3.TabIndex = 8;
			this.button3.Text = "Up";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(136, 112);
			this.button4.Name = "button4";
			this.button4.TabIndex = 9;
			this.button4.Text = "Down";
			// 
			// listBox3
			// 
			this.listBox3.Location = new System.Drawing.Point(16, 32);
			this.listBox3.Name = "listBox3";
			this.listBox3.Size = new System.Drawing.Size(208, 95);
			this.listBox3.TabIndex = 10;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.button1,
																					this.button4,
																					this.button3,
																					this.listBox2,
																					this.listBox1,
																					this.button2});
			this.groupBox1.Location = new System.Drawing.Point(16, 136);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(344, 144);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Grid Layout";
			// 
			// WebGridDesigner
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 494);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.listBox3,
																		  this.label1,
																		  this.comboBox1});
			this.Name = "WebGridDesigner";
			this.Text = "WebGridDesigner";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
