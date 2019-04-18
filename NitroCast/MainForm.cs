using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.Data;
using System.Reflection;
using System.Security.Permissions;
using NitroCast.Core;
using NitroCast.Core.Extensions;



namespace NitroCast
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
    /// using System.Security.Permissions;
    
	public partial class MainForm : Form
	{
		private DataModel model = new DataModel();
		private ModelExplorer mExplorer;
        private ModelOutput mOutput;

        public MainForm()
        {
            InitializeComponent();
            initialize();
        }

        public MainForm(string filename) : this()
        {
            loadFile(filename);
        }

        private void initialize()
        {
            ExtensionManager pluginManager = ExtensionManager.GetInstance();
            pluginManager.Initialize(new string[] { Application.StartupPath + "\\NitroCast.Extensions.Default.dll" });

            DataTypeManager.Init();

            this.Icon = Properties.Resources.NitroCast;
            this.Text = Localization.Strings.NitroCast;

            progressStatusBar = new NitroCast.Controls.ProgressStatusBar();

            StatusBarPanel pnlInfo = new StatusBarPanel();
            StatusBarPanel pnlProgress = new StatusBarPanel();
            pnlInfo.Text = "Ready";
            pnlInfo.Width = 450;
            pnlProgress.AutoSize = StatusBarPanelAutoSize.Spring;

            progressStatusBar.Panels.Add(pnlInfo);
            progressStatusBar.Panels.Add(pnlProgress);
            progressStatusBar.ShowPanels = true;
            progressStatusBar.ProgressPanelIndex = 1;
            this.Controls.Add(progressStatusBar);

            

            mOutput = new ModelOutput();
            mOutput.MdiParent = this;
            mOutput.Show();

            mExplorer = new ModelExplorer();
            mExplorer.MdiParent = this;
            mExplorer.Show();
        }

		private void model_Changed(object sender, EventArgs e)
		{
			this.Text = model.Name + " - " + Localization.Strings.NitroCast;
		}

		#region Save, Load Methods

		private void menuSave_Click(object sender, System.EventArgs e)
		{
			if(model.FileName != string.Empty)
				model.Save();
		}

		private void menuSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "NitroCast Files (*.NitroGen)|*.NitroGen";
			saveFileDialog1.FilterIndex = 2;
			saveFileDialog1.RestoreDirectory = true;
 
			if(saveFileDialog1.ShowDialog() == DialogResult.OK)
				model.SaveAs(saveFileDialog1.FileName);

			menuSave.Visible = model.FileName != string.Empty;
		}

		private void menuOpen_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = "NitroGen Files (*.NitroGen)|*.NitroGen";
			openFileDialog1.FilterIndex = 0;
			openFileDialog1.RestoreDirectory = true;

			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{
                loadFile(openFileDialog1.FileName);
			}
		}

		#endregion

        private void close()
        {
            if (model != null)
            {
                foreach (ModelFolder folder in model.Folders)
                {
                    foreach (object item in folder.Items)
                    {
                        if (item is ModelClass)
                        {
                            ModelClass c = (ModelClass)item;
                            if (c.Editor != null)
                            {
                                ClassEditor editor = (ClassEditor)c.Editor;
                                editor.Close();
                            }
                        }
                        else if (item is EnumEditor)
                        {
                            ModelEnum e = (ModelEnum)item;
                            if (e.Editor != null)
                            {
                                EnumEditor editor = (EnumEditor)e.Editor;
                                editor.Close();
                            }
                        }
                    }
                }

                //
                // Clear DataTypes!
                //
                DataTypeManager.Clear();

                mExplorer.Clear();
            }
        }

        private void loadFile(string filename)
        {
            close();

            ModelClass entry;

            model.Changed += new EventHandler(model_Changed);
            model.ProgressStart += new DataModelEventHandler(dataModel_ProgressStart);
            model.ProgressUpdate += new DataModelEventHandler(dataModel_ProgressUpdate);
            model.ProgressStop += new DataModelEventHandler(dataModel_ProgressStop);
            model.SaveError += new EventHandler(model_SaveError);

            //try
            //{
                model.Load(filename);
            //}
            //catch
            //{
            //    MessageBox.Show(Localization.Strings.Load_Error,
            //        Localization.Strings.NitroCast, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    close();
            //    return;
            //}

            // Trigger model change event
            model_Changed(null, EventArgs.Empty);

            menuSave.Visible = model.FileName != string.Empty;

            mExplorer.BindModel(model);

            foreach (ModelFolder folder in model.Folders)
            {
                foreach (object folderItem in folder.Items)
                {
                    if (folderItem is ModelClass)
                    {
                        entry = (ModelClass)folderItem;

                        if (entry.Attributes["WindowState"] == "True")
                        {
                            ClassEditor editor = new ClassEditor(entry);
                            editor.MdiParent = this;
                            editor.Show();
                        }
                    }
                }
            }
        }

		private void dataModel_ProgressStart(object sender, DataModelEventArgs e)
		{
			this.progressStatusBar.Panels[0].Text = e.Text;
			e.ProgressConfig.SetProgressBar(this.progressStatusBar.ProgressBar);
		}

		private void dataModel_ProgressUpdate(object sender, DataModelEventArgs e)
		{
			this.progressStatusBar.Panels[0].Text = e.Text;
			this.progressStatusBar.ProgressBar.PerformStep();
		}

		private void dataModel_ProgressStop(object sender, DataModelEventArgs e)
		{
			this.progressStatusBar.ProgressBar.Value = 0;
			this.progressStatusBar.Panels[0].Text = "Ready";
		}

        private void model_SaveError(object sender, System.EventArgs e)
        {
            MessageBox.Show(
                string.Format(Properties.Resources.ErrorSave, model.Name),
                Properties.Resources.AppTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

		private void menuExport_Click(object sender, System.EventArgs e)
		{            
            model.Export();			
		}

		private void menuNew_Click(object sender, System.EventArgs e)
		{
            close();

            model.Changed += new EventHandler(model_Changed);
            model.ProgressStart += new DataModelEventHandler(dataModel_ProgressStart);
            model.ProgressUpdate += new DataModelEventHandler(dataModel_ProgressUpdate);
            model.ProgressStop += new DataModelEventHandler(dataModel_ProgressStop);
            model.SaveError += new EventHandler(model_SaveError);
                        
			model = new DataModel();
			model.Changed += new EventHandler(model_Changed);

            menuSave.Visible = model.FileName != string.Empty;

            mExplorer.BindModel(model);
		}

		private void menuView_Popup(object sender, System.EventArgs e)
		{
            if (mExplorer == null)
            {
                menuView_ModelExplorer.Enabled = false;
                menuView_ModelExplorer.Checked = false;
            }
            else
            {
                menuView_ModelExplorer.Enabled = true;
                menuView_ModelExplorer.Checked = mExplorer.Visible;
            }
		}

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = false;
		}

		private void menuAbout_Click(object sender, System.EventArgs e)
		{
			AboutDialog aboutDialog1 = new AboutDialog();
			aboutDialog1.ShowDialog();
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void menuView_ModelExplorer_Click(object sender, EventArgs e)
        {
            if (!menuView_ModelExplorer.Checked)
                mExplorer.Show();
            else
                mExplorer.Hide();
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {

        }

        private void menuCompile_Click(object sender, EventArgs e)
        {
            model.Compile();
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            close();
        }
	}
}