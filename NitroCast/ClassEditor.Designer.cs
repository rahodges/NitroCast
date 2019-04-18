namespace NitroCast
{
    partial class ClassEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassEditor));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabClass = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.btWebOutputPath = new System.Windows.Forms.Button();
            this.tbWebOutputPath = new System.Windows.Forms.TextBox();
            this.tbInterfaces = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbIsThreadSafe = new System.Windows.Forms.CheckBox();
            this.btOutputPath = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.summaryTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.namespaceTextBox = new System.Windows.Forms.TextBox();
            this.classNameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbCreateDateEnabled = new System.Windows.Forms.CheckBox();
            this.cbModifyDateEnabled = new System.Windows.Forms.CheckBox();
            this.tabClient = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.tbToStringOverride = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tbCaption = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleQueryEnabled = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ddConcurrencyType = new System.Windows.Forms.ComboBox();
            this.tableNameBox = new System.Windows.Forms.TextBox();
            this.hardCodeTables = new System.Windows.Forms.CheckBox();
            this.tabObjects = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.objectsTree = new NitroCast.Core.UI.TreeLE();
            this.cmObjects = new System.Windows.Forms.ContextMenu();
            this.miCut = new System.Windows.Forms.MenuItem();
            this.miCopy = new System.Windows.Forms.MenuItem();
            this.miPaste = new System.Windows.Forms.MenuItem();
            this.miDelete = new System.Windows.Forms.MenuItem();
            this.miBreak = new System.Windows.Forms.MenuItem();
            this.miNewObject = new System.Windows.Forms.MenuItem();
            this.miNewField = new System.Windows.Forms.MenuItem();
            this.miNewChild = new System.Windows.Forms.MenuItem();
            this.miNewEnum = new System.Windows.Forms.MenuItem();
            this.miNewFolder = new System.Windows.Forms.MenuItem();
            this.pgFields = new System.Windows.Forms.PropertyGrid();
            this.btNewChild = new System.Windows.Forms.Button();
            this.btNewField = new System.Windows.Forms.Button();
            this.btNewFolder = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.tabDependencies = new System.Windows.Forms.TabPage();
            this.depStatusLabel = new System.Windows.Forms.Label();
            this.depTree = new System.Windows.Forms.TreeView();
            this.label16 = new System.Windows.Forms.Label();
            this.btRefreshDependencies = new System.Windows.Forms.Button();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.codePreviewBox = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pluginDescription = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pluginFilenameLabel = new System.Windows.Forms.Label();
            this.codeGeneratorComboBox = new System.Windows.Forms.ComboBox();
            this.pluginCopyrightLabel = new System.Windows.Forms.Label();
            this.btGenerate = new System.Windows.Forms.Button();
            this.pluginAuthorLabel = new System.Windows.Forms.Label();
            this.tabCaching = new System.Windows.Forms.TabPage();
            this.rbNoCache = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbCacheName = new System.Windows.Forms.TextBox();
            this.rbCabCache = new System.Windows.Forms.RadioButton();
            this.rbAspCache = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbCachingEnabled = new System.Windows.Forms.CheckBox();
            this.tbCollectionCacheLifetime = new System.Windows.Forms.TextBox();
            this.lblClusterWarning = new System.Windows.Forms.Label();
            this.tbCacheLifetime = new System.Windows.Forms.TextBox();
            this.cbCollectionCachingEnabled = new System.Windows.Forms.CheckBox();
            this.btApply = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabClass.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabClient.SuspendLayout();
            this.tabDatabase.SuspendLayout();
            this.tabObjects.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabDependencies.SuspendLayout();
            this.tabPreview.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabCaching.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabClass);
            this.tabControl1.Controls.Add(this.tabClient);
            this.tabControl1.Controls.Add(this.tabDatabase);
            this.tabControl1.Controls.Add(this.tabObjects);
            this.tabControl1.Controls.Add(this.tabDependencies);
            this.tabControl1.Controls.Add(this.tabPreview);
            this.tabControl1.Controls.Add(this.tabCaching);
            this.tabControl1.ItemSize = new System.Drawing.Size(42, 18);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(10, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(487, 461);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabClass
            // 
            this.tabClass.Controls.Add(this.label14);
            this.tabClass.Controls.Add(this.btWebOutputPath);
            this.tabClass.Controls.Add(this.tbWebOutputPath);
            this.tabClass.Controls.Add(this.tbInterfaces);
            this.tabClass.Controls.Add(this.label11);
            this.tabClass.Controls.Add(this.groupBox5);
            this.tabClass.Controls.Add(this.btOutputPath);
            this.tabClass.Controls.Add(this.label5);
            this.tabClass.Controls.Add(this.tbOutputPath);
            this.tabClass.Controls.Add(this.summaryTextBox);
            this.tabClass.Controls.Add(this.label2);
            this.tabClass.Controls.Add(this.label1);
            this.tabClass.Controls.Add(this.namespaceTextBox);
            this.tabClass.Controls.Add(this.classNameTextBox);
            this.tabClass.Controls.Add(this.label6);
            this.tabClass.Controls.Add(this.groupBox2);
            this.tabClass.Location = new System.Drawing.Point(4, 22);
            this.tabClass.Name = "tabClass";
            this.tabClass.Size = new System.Drawing.Size(479, 435);
            this.tabClass.TabIndex = 0;
            this.tabClass.Text = "Class";
            this.tabClass.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 334);
            this.label14.Margin = new System.Windows.Forms.Padding(3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(133, 14);
            this.label14.TabIndex = 41;
            this.label14.Text = "Web Output Directory";
            // 
            // btWebOutputPath
            // 
            this.btWebOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btWebOutputPath.Location = new System.Drawing.Point(451, 365);
            this.btWebOutputPath.Name = "btWebOutputPath";
            this.btWebOutputPath.Size = new System.Drawing.Size(24, 20);
            this.btWebOutputPath.TabIndex = 40;
            this.btWebOutputPath.Text = "...";
            this.btWebOutputPath.Click += new System.EventHandler(this.btWebOutputPath_Click);
            // 
            // tbWebOutputPath
            // 
            this.tbWebOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWebOutputPath.Location = new System.Drawing.Point(6, 354);
            this.tbWebOutputPath.Multiline = true;
            this.tbWebOutputPath.Name = "tbWebOutputPath";
            this.tbWebOutputPath.Size = new System.Drawing.Size(439, 31);
            this.tbWebOutputPath.TabIndex = 39;
            // 
            // tbInterfaces
            // 
            this.tbInterfaces.Location = new System.Drawing.Point(5, 110);
            this.tbInterfaces.Name = "tbInterfaces";
            this.tbInterfaces.Size = new System.Drawing.Size(208, 20);
            this.tbInterfaces.TabIndex = 38;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 16);
            this.label11.TabIndex = 37;
            this.label11.Text = "Interfaces";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cbIsThreadSafe);
            this.groupBox5.Location = new System.Drawing.Point(219, 85);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(257, 45);
            this.groupBox5.TabIndex = 36;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Threading";
            // 
            // cbIsThreadSafe
            // 
            this.cbIsThreadSafe.Location = new System.Drawing.Point(8, 16);
            this.cbIsThreadSafe.Name = "cbIsThreadSafe";
            this.cbIsThreadSafe.Size = new System.Drawing.Size(144, 24);
            this.cbIsThreadSafe.TabIndex = 0;
            this.cbIsThreadSafe.Text = "Enable Thread Safety";
            // 
            // btOutputPath
            // 
            this.btOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOutputPath.Location = new System.Drawing.Point(451, 308);
            this.btOutputPath.Name = "btOutputPath";
            this.btOutputPath.Size = new System.Drawing.Size(24, 20);
            this.btOutputPath.TabIndex = 6;
            this.btOutputPath.Text = "...";
            this.btOutputPath.Click += new System.EventHandler(this.btOutputPath_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 275);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 21;
            this.label5.Text = "Output Directory";
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputPath.Location = new System.Drawing.Point(6, 297);
            this.tbOutputPath.Multiline = true;
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(439, 31);
            this.tbOutputPath.TabIndex = 5;
            // 
            // summaryTextBox
            // 
            this.summaryTextBox.AcceptsReturn = true;
            this.summaryTextBox.Location = new System.Drawing.Point(6, 152);
            this.summaryTextBox.Multiline = true;
            this.summaryTextBox.Name = "summaryTextBox";
            this.summaryTextBox.Size = new System.Drawing.Size(207, 117);
            this.summaryTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Summary";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Namespace";
            // 
            // namespaceTextBox
            // 
            this.namespaceTextBox.Location = new System.Drawing.Point(5, 26);
            this.namespaceTextBox.Name = "namespaceTextBox";
            this.namespaceTextBox.Size = new System.Drawing.Size(208, 20);
            this.namespaceTextBox.TabIndex = 2;
            // 
            // classNameTextBox
            // 
            this.classNameTextBox.Location = new System.Drawing.Point(5, 68);
            this.classNameTextBox.Name = "classNameTextBox";
            this.classNameTextBox.Size = new System.Drawing.Size(208, 20);
            this.classNameTextBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(2, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "Class Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbCreateDateEnabled);
            this.groupBox2.Controls.Add(this.cbModifyDateEnabled);
            this.groupBox2.Location = new System.Drawing.Point(219, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 72);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Special Fields";
            // 
            // cbCreateDateEnabled
            // 
            this.cbCreateDateEnabled.Location = new System.Drawing.Point(8, 16);
            this.cbCreateDateEnabled.Name = "cbCreateDateEnabled";
            this.cbCreateDateEnabled.Size = new System.Drawing.Size(104, 24);
            this.cbCreateDateEnabled.TabIndex = 33;
            this.cbCreateDateEnabled.Text = "Create Date";
            // 
            // cbModifyDateEnabled
            // 
            this.cbModifyDateEnabled.Location = new System.Drawing.Point(8, 40);
            this.cbModifyDateEnabled.Name = "cbModifyDateEnabled";
            this.cbModifyDateEnabled.Size = new System.Drawing.Size(104, 24);
            this.cbModifyDateEnabled.TabIndex = 34;
            this.cbModifyDateEnabled.Text = "Modify Date";
            this.cbModifyDateEnabled.CheckedChanged += new System.EventHandler(this.cbModifyDateEnabled_CheckedChanged);
            // 
            // tabClient
            // 
            this.tabClient.Controls.Add(this.label15);
            this.tabClient.Controls.Add(this.tbToStringOverride);
            this.tabClient.Controls.Add(this.tbDescription);
            this.tabClient.Controls.Add(this.label23);
            this.tabClient.Controls.Add(this.tbCaption);
            this.tabClient.Controls.Add(this.label22);
            this.tabClient.Location = new System.Drawing.Point(4, 22);
            this.tabClient.Name = "tabClient";
            this.tabClient.Size = new System.Drawing.Size(479, 435);
            this.tabClient.TabIndex = 7;
            this.tabClient.Text = "Client Properties";
            this.tabClient.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(177, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "ToString() Override";
            // 
            // tbToStringOverride
            // 
            this.tbToStringOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbToStringOverride.Location = new System.Drawing.Point(180, 26);
            this.tbToStringOverride.Multiline = true;
            this.tbToStringOverride.Name = "tbToStringOverride";
            this.tbToStringOverride.Size = new System.Drawing.Size(296, 388);
            this.tbToStringOverride.TabIndex = 30;
            // 
            // tbDescription
            // 
            this.tbDescription.AcceptsReturn = true;
            this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tbDescription.Location = new System.Drawing.Point(3, 68);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(171, 346);
            this.tbDescription.TabIndex = 28;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(2, 49);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(100, 16);
            this.label23.TabIndex = 29;
            this.label23.Text = "Description";
            // 
            // tbCaption
            // 
            this.tbCaption.Location = new System.Drawing.Point(3, 26);
            this.tbCaption.Name = "tbCaption";
            this.tbCaption.Size = new System.Drawing.Size(171, 20);
            this.tbCaption.TabIndex = 27;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(2, 7);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(100, 16);
            this.label22.TabIndex = 26;
            this.label22.Text = "Caption";
            // 
            // tabDatabase
            // 
            this.tabDatabase.Controls.Add(this.label3);
            this.tabDatabase.Controls.Add(this.simpleQueryEnabled);
            this.tabDatabase.Controls.Add(this.label7);
            this.tabDatabase.Controls.Add(this.ddConcurrencyType);
            this.tabDatabase.Controls.Add(this.tableNameBox);
            this.tabDatabase.Controls.Add(this.hardCodeTables);
            this.tabDatabase.Location = new System.Drawing.Point(4, 22);
            this.tabDatabase.Name = "tabDatabase";
            this.tabDatabase.Size = new System.Drawing.Size(479, 435);
            this.tabDatabase.TabIndex = 6;
            this.tabDatabase.Text = "Database";
            this.tabDatabase.UseVisualStyleBackColor = true;
            this.tabDatabase.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Default Table Name";
            // 
            // simpleQueryEnabled
            // 
            this.simpleQueryEnabled.AutoSize = true;
            this.simpleQueryEnabled.Location = new System.Drawing.Point(27, 82);
            this.simpleQueryEnabled.Name = "simpleQueryEnabled";
            this.simpleQueryEnabled.Size = new System.Drawing.Size(130, 17);
            this.simpleQueryEnabled.TabIndex = 32;
            this.simpleQueryEnabled.Text = "Simple Query Enabled";
            this.simpleQueryEnabled.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(0, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 16);
            this.label7.TabIndex = 31;
            this.label7.Text = "Concurrency";
            // 
            // ddConcurrencyType
            // 
            this.ddConcurrencyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddConcurrencyType.Location = new System.Drawing.Point(3, 122);
            this.ddConcurrencyType.Name = "ddConcurrencyType";
            this.ddConcurrencyType.Size = new System.Drawing.Size(208, 21);
            this.ddConcurrencyType.Sorted = true;
            this.ddConcurrencyType.TabIndex = 30;
            // 
            // tableNameBox
            // 
            this.tableNameBox.Location = new System.Drawing.Point(3, 26);
            this.tableNameBox.Name = "tableNameBox";
            this.tableNameBox.Size = new System.Drawing.Size(208, 20);
            this.tableNameBox.TabIndex = 17;
            // 
            // hardCodeTables
            // 
            this.hardCodeTables.Location = new System.Drawing.Point(27, 52);
            this.hardCodeTables.Name = "hardCodeTables";
            this.hardCodeTables.Size = new System.Drawing.Size(184, 24);
            this.hardCodeTables.TabIndex = 16;
            this.hardCodeTables.Text = "Hard Code Tables";
            // 
            // tabObjects
            // 
            this.tabObjects.Controls.Add(this.splitContainer1);
            this.tabObjects.Controls.Add(this.btNewChild);
            this.tabObjects.Controls.Add(this.btNewField);
            this.tabObjects.Controls.Add(this.btNewFolder);
            this.tabObjects.Controls.Add(this.btDown);
            this.tabObjects.Controls.Add(this.btUp);
            this.tabObjects.Location = new System.Drawing.Point(4, 22);
            this.tabObjects.Name = "tabObjects";
            this.tabObjects.Size = new System.Drawing.Size(479, 435);
            this.tabObjects.TabIndex = 5;
            this.tabObjects.Text = "Fields";
            this.tabObjects.UseVisualStyleBackColor = true;
            this.tabObjects.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(2, 36);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.objectsTree);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pgFields);
            this.splitContainer1.Size = new System.Drawing.Size(474, 396);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 19;
            this.splitContainer1.TabStop = false;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // objectsTree
            // 
            this.objectsTree.AllowDrop = true;
            this.objectsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectsTree.ContextMenu = this.cmObjects;
            this.objectsTree.HideSelection = false;
            this.objectsTree.HScrollPos = 0;
            this.objectsTree.LabelEdit = true;
            this.objectsTree.Location = new System.Drawing.Point(0, 0);
            this.objectsTree.Margin = new System.Windows.Forms.Padding(0);
            this.objectsTree.Name = "objectsTree";
            this.objectsTree.Size = new System.Drawing.Size(200, 396);
            this.objectsTree.TabIndex = 2;
            this.objectsTree.VScrollPos = 0;
            this.objectsTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.objectsTree_AfterCollapse);
            this.objectsTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.objectsTree_AfterLabelEdit);
            this.objectsTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.objectsTree_BeforeCollapse);
            this.objectsTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.objectsTree_DragDrop);
            this.objectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectsTree_AfterSelect);
            this.objectsTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.objectsTree_DragEnter);
            this.objectsTree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.objectsTree_BeforeLabelEdit);
            this.objectsTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.objectsTree_AfterExpand);
            this.objectsTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.objectsTree_ItemDrag);
            this.objectsTree.DragOver += new System.Windows.Forms.DragEventHandler(this.objectsTree_DragOver);
            // 
            // cmObjects
            // 
            this.cmObjects.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.miDelete,
            this.miBreak,
            this.miNewObject});
            this.cmObjects.Popup += new System.EventHandler(this.cmObjects_Popup);
            // 
            // miCut
            // 
            this.miCut.Index = 0;
            this.miCut.Text = "&Cut";
            this.miCut.Visible = false;
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Index = 1;
            this.miCopy.Text = "Cop&y";
            this.miCopy.Visible = false;
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Index = 2;
            this.miPaste.Text = "&Paste";
            this.miPaste.Visible = false;
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // miDelete
            // 
            this.miDelete.Index = 3;
            this.miDelete.Text = "&Delete";
            this.miDelete.Visible = false;
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // miBreak
            // 
            this.miBreak.Index = 4;
            this.miBreak.Text = "-";
            // 
            // miNewObject
            // 
            this.miNewObject.DefaultItem = true;
            this.miNewObject.Index = 5;
            this.miNewObject.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miNewField,
            this.miNewChild,
            this.miNewEnum,
            this.miNewFolder});
            this.miNewObject.Text = "New...";
            this.miNewObject.Visible = false;
            // 
            // miNewField
            // 
            this.miNewField.Index = 0;
            this.miNewField.Text = "New Field";
            this.miNewField.Click += new System.EventHandler(this.miNewField_Click);
            // 
            // miNewChild
            // 
            this.miNewChild.Index = 1;
            this.miNewChild.Text = "New Child";
            this.miNewChild.Click += new System.EventHandler(this.miNewChild_Click);
            // 
            // miNewEnum
            // 
            this.miNewEnum.Index = 2;
            this.miNewEnum.Text = "New Enum";
            this.miNewEnum.Click += new System.EventHandler(this.miNewEnum_Click);
            // 
            // miNewFolder
            // 
            this.miNewFolder.Index = 3;
            this.miNewFolder.Text = "New Folder";
            this.miNewFolder.Click += new System.EventHandler(this.miNewFolder_Click);
            // 
            // pgFields
            // 
            this.pgFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgFields.BackColor = System.Drawing.SystemColors.Window;
            this.pgFields.Location = new System.Drawing.Point(0, 0);
            this.pgFields.Name = "pgFields";
            this.pgFields.Size = new System.Drawing.Size(269, 396);
            this.pgFields.TabIndex = 445;
            this.pgFields.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.fieldProperties_PropertyValueChanged);
            // 
            // btNewChild
            // 
            this.btNewChild.Enabled = false;
            this.btNewChild.Location = new System.Drawing.Point(158, 7);
            this.btNewChild.Name = "btNewChild";
            this.btNewChild.Size = new System.Drawing.Size(72, 23);
            this.btNewChild.TabIndex = 18;
            this.btNewChild.Text = "New Child";
            this.btNewChild.Click += new System.EventHandler(this.btNewChild_Click);
            // 
            // btNewField
            // 
            this.btNewField.Enabled = false;
            this.btNewField.Location = new System.Drawing.Point(80, 7);
            this.btNewField.Name = "btNewField";
            this.btNewField.Size = new System.Drawing.Size(72, 23);
            this.btNewField.TabIndex = 17;
            this.btNewField.Text = "New Field";
            this.btNewField.Click += new System.EventHandler(this.btNewField_Click);
            // 
            // btNewFolder
            // 
            this.btNewFolder.Location = new System.Drawing.Point(2, 7);
            this.btNewFolder.Name = "btNewFolder";
            this.btNewFolder.Size = new System.Drawing.Size(72, 23);
            this.btNewFolder.TabIndex = 16;
            this.btNewFolder.Text = "New Folder";
            this.btNewFolder.Click += new System.EventHandler(this.btNewFolder_Click);
            // 
            // btDown
            // 
            this.btDown.FlatAppearance.BorderSize = 0;
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.Location = new System.Drawing.Point(314, 7);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(72, 23);
            this.btDown.TabIndex = 14;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btUp
            // 
            this.btUp.Image = global::NitroCast.Properties.Resources.UpArrow;
            this.btUp.Location = new System.Drawing.Point(236, 7);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(72, 23);
            this.btUp.TabIndex = 15;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // tabDependencies
            // 
            this.tabDependencies.Controls.Add(this.depStatusLabel);
            this.tabDependencies.Controls.Add(this.depTree);
            this.tabDependencies.Controls.Add(this.label16);
            this.tabDependencies.Controls.Add(this.btRefreshDependencies);
            this.tabDependencies.Location = new System.Drawing.Point(4, 22);
            this.tabDependencies.Name = "tabDependencies";
            this.tabDependencies.Padding = new System.Windows.Forms.Padding(3);
            this.tabDependencies.Size = new System.Drawing.Size(479, 435);
            this.tabDependencies.TabIndex = 9;
            this.tabDependencies.Text = "Dependencies";
            this.tabDependencies.UseVisualStyleBackColor = true;
            // 
            // depStatusLabel
            // 
            this.depStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.depStatusLabel.AutoSize = true;
            this.depStatusLabel.Location = new System.Drawing.Point(88, 393);
            this.depStatusLabel.Name = "depStatusLabel";
            this.depStatusLabel.Size = new System.Drawing.Size(124, 13);
            this.depStatusLabel.TabIndex = 4;
            this.depStatusLabel.Text = "No dependencies found.";
            // 
            // depTree
            // 
            this.depTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.depTree.Location = new System.Drawing.Point(6, 24);
            this.depTree.Name = "depTree";
            this.depTree.Size = new System.Drawing.Size(467, 358);
            this.depTree.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 8);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Dependencies";
            // 
            // btRefreshDependencies
            // 
            this.btRefreshDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRefreshDependencies.Location = new System.Drawing.Point(6, 388);
            this.btRefreshDependencies.Name = "btRefreshDependencies";
            this.btRefreshDependencies.Size = new System.Drawing.Size(75, 23);
            this.btRefreshDependencies.TabIndex = 2;
            this.btRefreshDependencies.Text = "Refresh";
            this.btRefreshDependencies.UseVisualStyleBackColor = true;
            this.btRefreshDependencies.Click += new System.EventHandler(this.btRefreshDependencies_Click);
            // 
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.codePreviewBox);
            this.tabPreview.Controls.Add(this.groupBox4);
            this.tabPreview.Controls.Add(this.groupBox3);
            this.tabPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Size = new System.Drawing.Size(479, 435);
            this.tabPreview.TabIndex = 2;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;
            this.tabPreview.Visible = false;
            // 
            // codePreviewBox
            // 
            this.codePreviewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.codePreviewBox.Location = new System.Drawing.Point(5, 193);
            this.codePreviewBox.Name = "codePreviewBox";
            this.codePreviewBox.Size = new System.Drawing.Size(468, 236);
            this.codePreviewBox.TabIndex = 15;
            this.codePreviewBox.Text = "";
            this.codePreviewBox.WordWrap = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.pluginDescription);
            this.groupBox4.Location = new System.Drawing.Point(253, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(223, 180);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Description";
            // 
            // pluginDescription
            // 
            this.pluginDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginDescription.Location = new System.Drawing.Point(6, 19);
            this.pluginDescription.Multiline = true;
            this.pluginDescription.Name = "pluginDescription";
            this.pluginDescription.ReadOnly = true;
            this.pluginDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.pluginDescription.Size = new System.Drawing.Size(211, 155);
            this.pluginDescription.TabIndex = 7;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.pluginFilenameLabel);
            this.groupBox3.Controls.Add(this.codeGeneratorComboBox);
            this.groupBox3.Controls.Add(this.pluginCopyrightLabel);
            this.groupBox3.Controls.Add(this.btGenerate);
            this.groupBox3.Controls.Add(this.pluginAuthorLabel);
            this.groupBox3.Location = new System.Drawing.Point(2, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 180);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Preview Options";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Code Plugin";
            // 
            // pluginFilenameLabel
            // 
            this.pluginFilenameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginFilenameLabel.Location = new System.Drawing.Point(11, 129);
            this.pluginFilenameLabel.Margin = new System.Windows.Forms.Padding(3);
            this.pluginFilenameLabel.Name = "pluginFilenameLabel";
            this.pluginFilenameLabel.Size = new System.Drawing.Size(228, 16);
            this.pluginFilenameLabel.TabIndex = 5;
            // 
            // codeGeneratorComboBox
            // 
            this.codeGeneratorComboBox.DisplayMember = "Name";
            this.codeGeneratorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codeGeneratorComboBox.ItemHeight = 13;
            this.codeGeneratorComboBox.Location = new System.Drawing.Point(11, 38);
            this.codeGeneratorComboBox.Name = "codeGeneratorComboBox";
            this.codeGeneratorComboBox.Size = new System.Drawing.Size(228, 21);
            this.codeGeneratorComboBox.TabIndex = 10;
            this.codeGeneratorComboBox.SelectedIndexChanged += new System.EventHandler(this.codeGeneratorComboBox_SelectedIndexChanged);
            // 
            // pluginCopyrightLabel
            // 
            this.pluginCopyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginCopyrightLabel.Location = new System.Drawing.Point(11, 85);
            this.pluginCopyrightLabel.Margin = new System.Windows.Forms.Padding(3);
            this.pluginCopyrightLabel.Name = "pluginCopyrightLabel";
            this.pluginCopyrightLabel.Size = new System.Drawing.Size(228, 39);
            this.pluginCopyrightLabel.TabIndex = 4;
            // 
            // btGenerate
            // 
            this.btGenerate.Location = new System.Drawing.Point(164, 151);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(75, 23);
            this.btGenerate.TabIndex = 9;
            this.btGenerate.Text = "Preview";
            this.btGenerate.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // pluginAuthorLabel
            // 
            this.pluginAuthorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginAuthorLabel.Location = new System.Drawing.Point(11, 65);
            this.pluginAuthorLabel.Margin = new System.Windows.Forms.Padding(3);
            this.pluginAuthorLabel.Name = "pluginAuthorLabel";
            this.pluginAuthorLabel.Size = new System.Drawing.Size(228, 16);
            this.pluginAuthorLabel.TabIndex = 3;
            // 
            // tabCaching
            // 
            this.tabCaching.Controls.Add(this.rbNoCache);
            this.tabCaching.Controls.Add(this.label10);
            this.tabCaching.Controls.Add(this.label9);
            this.tabCaching.Controls.Add(this.label8);
            this.tabCaching.Controls.Add(this.tbCacheName);
            this.tabCaching.Controls.Add(this.rbCabCache);
            this.tabCaching.Controls.Add(this.rbAspCache);
            this.tabCaching.Controls.Add(this.label13);
            this.tabCaching.Controls.Add(this.label12);
            this.tabCaching.Controls.Add(this.cbCachingEnabled);
            this.tabCaching.Controls.Add(this.tbCollectionCacheLifetime);
            this.tabCaching.Controls.Add(this.lblClusterWarning);
            this.tabCaching.Controls.Add(this.tbCacheLifetime);
            this.tabCaching.Controls.Add(this.cbCollectionCachingEnabled);
            this.tabCaching.Location = new System.Drawing.Point(4, 22);
            this.tabCaching.Name = "tabCaching";
            this.tabCaching.Padding = new System.Windows.Forms.Padding(3);
            this.tabCaching.Size = new System.Drawing.Size(479, 435);
            this.tabCaching.TabIndex = 10;
            this.tabCaching.Text = "Cache";
            this.tabCaching.UseVisualStyleBackColor = true;
            this.tabCaching.Click += new System.EventHandler(this.tabCaching_Click);
            // 
            // rbNoCache
            // 
            this.rbNoCache.AutoSize = true;
            this.rbNoCache.Location = new System.Drawing.Point(31, 29);
            this.rbNoCache.Name = "rbNoCache";
            this.rbNoCache.Size = new System.Drawing.Size(51, 17);
            this.rbNoCache.TabIndex = 42;
            this.rbNoCache.Text = "None";
            this.rbNoCache.UseVisualStyleBackColor = true;
            this.rbNoCache.CheckedChanged += new System.EventHandler(this.rbNoCache_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 41;
            this.label10.Text = "Cache Objects";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Caching Class";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Name:";
            // 
            // tbCacheName
            // 
            this.tbCacheName.Enabled = false;
            this.tbCacheName.Location = new System.Drawing.Point(91, 98);
            this.tbCacheName.Name = "tbCacheName";
            this.tbCacheName.Size = new System.Drawing.Size(204, 20);
            this.tbCacheName.TabIndex = 38;
            // 
            // rbCabCache
            // 
            this.rbCabCache.AutoSize = true;
            this.rbCabCache.Location = new System.Drawing.Point(31, 76);
            this.rbCabCache.Name = "rbCabCache";
            this.rbCabCache.Size = new System.Drawing.Size(199, 17);
            this.rbCabCache.TabIndex = 37;
            this.rbCabCache.Text = "Enterprise Caching Application Block";
            this.rbCabCache.UseVisualStyleBackColor = true;
            this.rbCabCache.CheckedChanged += new System.EventHandler(this.rbCabCache_CheckedChanged);
            // 
            // rbAspCache
            // 
            this.rbAspCache.AutoSize = true;
            this.rbAspCache.Checked = true;
            this.rbAspCache.Location = new System.Drawing.Point(31, 52);
            this.rbAspCache.Name = "rbAspCache";
            this.rbAspCache.Size = new System.Drawing.Size(98, 17);
            this.rbAspCache.TabIndex = 36;
            this.rbAspCache.TabStop = true;
            this.rbAspCache.Text = "ASP.net Cache";
            this.rbAspCache.UseVisualStyleBackColor = true;
            this.rbAspCache.CheckedChanged += new System.EventHandler(this.rbAspCache_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(215, 183);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "min";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(215, 153);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "min";
            // 
            // cbCachingEnabled
            // 
            this.cbCachingEnabled.Checked = true;
            this.cbCachingEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCachingEnabled.Location = new System.Drawing.Point(32, 148);
            this.cbCachingEnabled.Name = "cbCachingEnabled";
            this.cbCachingEnabled.Size = new System.Drawing.Size(120, 24);
            this.cbCachingEnabled.TabIndex = 26;
            this.cbCachingEnabled.Text = "Object Caching";
            // 
            // tbCollectionCacheLifetime
            // 
            this.tbCollectionCacheLifetime.Location = new System.Drawing.Point(158, 180);
            this.tbCollectionCacheLifetime.MaxLength = 5;
            this.tbCollectionCacheLifetime.Name = "tbCollectionCacheLifetime";
            this.tbCollectionCacheLifetime.Size = new System.Drawing.Size(51, 20);
            this.tbCollectionCacheLifetime.TabIndex = 33;
            // 
            // lblClusterWarning
            // 
            this.lblClusterWarning.ForeColor = System.Drawing.Color.Red;
            this.lblClusterWarning.Location = new System.Drawing.Point(6, 208);
            this.lblClusterWarning.Name = "lblClusterWarning";
            this.lblClusterWarning.Size = new System.Drawing.Size(289, 40);
            this.lblClusterWarning.TabIndex = 30;
            this.lblClusterWarning.Text = "Warning: Distributed software must impliment synchronization methods.";
            // 
            // tbCacheLifetime
            // 
            this.tbCacheLifetime.Location = new System.Drawing.Point(158, 150);
            this.tbCacheLifetime.MaxLength = 5;
            this.tbCacheLifetime.Name = "tbCacheLifetime";
            this.tbCacheLifetime.Size = new System.Drawing.Size(51, 20);
            this.tbCacheLifetime.TabIndex = 32;
            this.tbCacheLifetime.Text = "5";
            // 
            // cbCollectionCachingEnabled
            // 
            this.cbCollectionCachingEnabled.Location = new System.Drawing.Point(32, 178);
            this.cbCollectionCachingEnabled.Name = "cbCollectionCachingEnabled";
            this.cbCollectionCachingEnabled.Size = new System.Drawing.Size(120, 24);
            this.cbCollectionCachingEnabled.TabIndex = 31;
            this.cbCollectionCachingEnabled.Text = "Collection Caching";
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.Location = new System.Drawing.Point(415, 465);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(75, 23);
            this.btApply.TabIndex = 2;
            this.btApply.Text = "Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // ClassEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 491);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.tabControl1);
            this.HelpButton = true;
            this.Location = new System.Drawing.Point(50, 50);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 525);
            this.Name = "ClassEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ClassEditor";
            this.Load += new System.EventHandler(this.ClassEditor_Load);
            this.SizeChanged += new System.EventHandler(this.ClassEditor_SizeChanged);
            this.Shown += new System.EventHandler(this.ClassEditor_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClassEditor_FormClosed);
            this.Move += new System.EventHandler(this.ClassEditor_Move);
            this.tabControl1.ResumeLayout(false);
            this.tabClass.ResumeLayout(false);
            this.tabClass.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabClient.ResumeLayout(false);
            this.tabClient.PerformLayout();
            this.tabDatabase.ResumeLayout(false);
            this.tabDatabase.PerformLayout();
            this.tabObjects.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabDependencies.ResumeLayout(false);
            this.tabDependencies.PerformLayout();
            this.tabPreview.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tabCaching.ResumeLayout(false);
            this.tabCaching.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btOutputPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.TextBox summaryTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox namespaceTextBox;
        private System.Windows.Forms.TextBox classNameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tableNameBox;
        private System.Windows.Forms.CheckBox hardCodeTables;
        private NitroCast.Core.UI.TreeLE objectsTree;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox pluginDescription;
        private System.Windows.Forms.Label pluginFilenameLabel;
        private System.Windows.Forms.Label pluginCopyrightLabel;
        private System.Windows.Forms.Label pluginAuthorLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox codeGeneratorComboBox;
        private System.Windows.Forms.ContextMenu cmObjects;
        private System.Windows.Forms.MenuItem miNewObject;
        private System.Windows.Forms.MenuItem miDelete;

        private System.Windows.Forms.MenuItem miCopy;
        private System.Windows.Forms.MenuItem miPaste;
        private System.Windows.Forms.MenuItem miCut;
        private System.Windows.Forms.MenuItem miBreak;

        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbCaption;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckBox cbModifyDateEnabled;
        private System.Windows.Forms.CheckBox cbCreateDateEnabled;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbIsThreadSafe;

        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btNewFolder;
        private System.Windows.Forms.Button btNewField;
        private System.Windows.Forms.Button btNewChild;
        private System.Windows.Forms.MenuItem miNewChild;
        private System.Windows.Forms.MenuItem miNewField;
        private System.Windows.Forms.TabPage tabClass;
        private System.Windows.Forms.TabPage tabClient;
        private System.Windows.Forms.TabPage tabDatabase;
        private System.Windows.Forms.TabPage tabObjects;
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbInterfaces;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ddConcurrencyType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btWebOutputPath;
        private System.Windows.Forms.TextBox tbWebOutputPath;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbToStringOverride;
        private System.Windows.Forms.CheckBox simpleQueryEnabled;
        private System.Windows.Forms.TabPage tabDependencies;
        private System.Windows.Forms.Button btRefreshDependencies;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TreeView depTree;
        private System.Windows.Forms.Label depStatusLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuItem miNewEnum;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.MenuItem miNewFolder;
        private System.Windows.Forms.TabPage tabCaching;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbCollectionCacheLifetime;
        private System.Windows.Forms.TextBox tbCacheLifetime;
        private System.Windows.Forms.CheckBox cbCollectionCachingEnabled;
        private System.Windows.Forms.CheckBox cbCachingEnabled;
        private System.Windows.Forms.Label lblClusterWarning;
        private System.Windows.Forms.RadioButton rbCabCache;
        private System.Windows.Forms.RadioButton rbAspCache;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbCacheName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton rbNoCache;
        private System.Windows.Forms.PropertyGrid pgFields;
        private System.Windows.Forms.RichTextBox codePreviewBox;
    }
}
