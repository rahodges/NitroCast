namespace NitroCast
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.defaultMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuNew = new System.Windows.Forms.MenuItem();
            this.menuOpen = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuSave = new System.Windows.Forms.MenuItem();
            this.menuSaveAs = new System.Windows.Forms.MenuItem();
            this.menuExport = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuView = new System.Windows.Forms.MenuItem();
            this.menuView_ModelExplorer = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuCompile = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // defaultMenu
            // 
            this.defaultMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuView,
            this.menuItem3,
            this.menuItem4});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuItem12,
            this.menuItem13,
            this.menuItem14,
            this.menuSave,
            this.menuSaveAs,
            this.menuExport,
            this.menuItem11,
            this.menuItem15,
            this.menuItem10,
            this.menuItem1});
            this.menuFile.Text = "File";
            // 
            // menuNew
            // 
            this.menuNew.Index = 0;
            this.menuNew.Text = "&New NitroGen File...";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Index = 1;
            this.menuOpen.Text = "&Open...";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.Text = "-";
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 3;
            this.menuItem13.Text = "&Close";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 4;
            this.menuItem14.Text = "-";
            // 
            // menuSave
            // 
            this.menuSave.Index = 5;
            this.menuSave.Text = "&Save";
            this.menuSave.Visible = false;
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Index = 6;
            this.menuSaveAs.Text = "Save &As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuExport
            // 
            this.menuExport.Index = 7;
            this.menuExport.Text = "&Export";
            this.menuExport.Click += new System.EventHandler(this.menuExport_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 8;
            this.menuItem11.Text = "-";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 9;
            this.menuItem15.Text = "Recent &Files";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 10;
            this.menuItem10.Text = "-";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 11;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuView
            // 
            this.menuView.Index = 1;
            this.menuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuView_ModelExplorer});
            this.menuView.Text = "View";
            this.menuView.Popup += new System.EventHandler(this.menuView_Popup);
            // 
            // menuView_ModelExplorer
            // 
            this.menuView_ModelExplorer.Index = 0;
            this.menuView_ModelExplorer.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftL;
            this.menuView_ModelExplorer.Text = "Model Explorer";
            this.menuView_ModelExplorer.Click += new System.EventHandler(this.menuView_ModelExplorer_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAbout});
            this.menuItem4.Text = "Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 0;
            this.menuAbout.Text = "About NitroCast...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 401);
            this.HelpButton = true;
            this.IsMdiContainer = true;
            this.Menu = this.defaultMenu;
            this.Name = "MainForm";
            this.Text = "NitroCast";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuSave;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuView;
        private System.Windows.Forms.MenuItem menuView_ModelExplorer;
        private System.Windows.Forms.MenuItem menuNew;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuCompile;
        private NitroCast.Controls.ProgressStatusBar progressStatusBar;
        private System.Windows.Forms.MainMenu defaultMenu;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuSaveAs;
        private System.Windows.Forms.MenuItem menuOpen;
        private System.Windows.Forms.MenuItem menuAbout;
        private System.Windows.Forms.MenuItem menuExport;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuItem15;
    }
}