using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NitroCast.Core;
using NitroCast.Core.Extensions;
using NitroCast.Core.Support;
using NitroCast.Core.UI;

namespace NitroCast
{
	public partial class ClassEditor : Form
	{
        ModelClass editClass;

		#region Constructor

        public ClassEditor()
        {
            InitializeComponent();

            // Load Concurrency Types
            ddConcurrencyType.Items.Add(ConcurrencyType.None);
            ddConcurrencyType.Items.Add(ConcurrencyType.OptimisticFull);
            ddConcurrencyType.Items.Add(ConcurrencyType.OptimisticPartial);
            ddConcurrencyType.Items.Add(ConcurrencyType.Pessimistic);

            // Load Plugins
            ExtensionManager pluginManager = ExtensionManager.GetInstance();
            {
                foreach (OutputExtension p in pluginManager.OutputExtensions.Values)
                    codeGeneratorComboBox.Items.Add(p);
            }                                    

            // Set Icon
            this.Icon = Properties.Resources.ClassEditor;

            // Objects Tree Icons
            ImageList objectsTreeImages = new ImageList();
            objectsTreeImages.Images.Add(Properties.Resources.FolderClosed);
            objectsTreeImages.Images.Add(Properties.Resources.FolderOpen);
            objectsTreeImages.Images.Add(Properties.Resources.VSObject_Field_Shortcut);
            objectsTreeImages.Images.Add(Properties.Resources.VSObject_Class_Shortcut);            
            objectsTreeImages.Images.Add(Properties.Resources.VSObject_Enum_Shortcut);
            objectsTreeImages.TransparentColor = Color.Magenta;
            objectsTree.ImageList = objectsTreeImages;
            

            // Dependency Tree Icons
            ImageList depTreeImages = new ImageList();
            depTreeImages.Images.Add(Properties.Resources.FolderClosed);
            depTreeImages.Images.Add(Properties.Resources.FolderOpen);
            depTreeImages.Images.Add(Properties.Resources.VSObject_Class);
            depTreeImages.Images.Add(Properties.Resources.VSObject_Class_Shortcut);
            depTreeImages.TransparentColor = Color.Magenta;
            depTree.ImageList = depTreeImages;

            pgFields.PropertyTabs.AddTabType(typeof(ExtensionsCategoryTab));
        }

		public ClassEditor(ModelClass modelClass) : this()
		{
            editClass = modelClass;
            editClass.Editor = this;

            this.Text = modelClass.Name;

            namespaceTextBox.Text = editClass.Namespace;
            classNameTextBox.Text = editClass.Name;
            summaryTextBox.Text = editClass.Summary;
            tbCaption.Text = editClass.Caption;
            tbDescription.Text = editClass.Description;
            tableNameBox.Text = editClass.DefaultTableName;
            hardCodeTables.Checked = editClass.IsTableCoded;
            tbOutputPath.Text = editClass.OutputPath;
            tbWebOutputPath.Text = editClass.WebOutputPath;
            
            cbIsThreadSafe.Checked = editClass.IsThreadSafe;            
            cbCreateDateEnabled.Checked = editClass.IsCreateDateEnabled;
            cbModifyDateEnabled.Checked = editClass.IsModifyDateEnabled;
            ddConcurrencyType.SelectedItem = editClass.Concurrency;
            ddConcurrencyType.Enabled = cbModifyDateEnabled.Checked;
            tbInterfaces.Text = editClass.Interfaces;
            tbToStringOverride.Text = editClass.ToStringOverride;
            simpleQueryEnabled.Checked = editClass.SimpleQueryEnabled;

            // Cache Tab
            rbNoCache.Checked = editClass.CacheClass == "NoCache";
            rbAspCache.Checked = editClass.CacheClass == "AspCache";
            rbCabCache.Checked = editClass.CacheClass == "CabCache";
            tbCacheName.Text = editClass.CacheName;
            lblClusterWarning.Visible = editClass.IsCachingEnabled;

            cbCachingEnabled.Checked = editClass.IsCachingEnabled;
            cbCachingEnabled.Enabled = !rbNoCache.Checked;
            tbCacheLifetime.Text = editClass.CacheLifetime.TotalMinutes.ToString();
            tbCacheLifetime.ReadOnly = !cbCachingEnabled.Checked;
            cbCollectionCachingEnabled.Checked = editClass.IsCollectionCachingEnabled;
            cbCollectionCachingEnabled.Enabled = !rbNoCache.Checked;
            tbCollectionCacheLifetime.Text = editClass.CollectionCacheLifetime.TotalMinutes.ToString();
            tbCollectionCacheLifetime.ReadOnly = !cbCollectionCachingEnabled.Checked;

            // Set CodeGenerator
            codeGeneratorComboBox.SelectedIndex = 0;
		}

		#endregion

        #region Apply

        private void btApply_Click(object sender, System.EventArgs e)
        {
            editClass.Name = this.classNameTextBox.Text;
            editClass.Namespace = namespaceTextBox.Text;
            editClass.Caption = tbCaption.Text;
            editClass.Description = tbDescription.Text;
            editClass.Summary = summaryTextBox.Text;
            editClass.Concurrency = (ConcurrencyType)ddConcurrencyType.SelectedItem;

            editClass.OutputPath = tbOutputPath.Text;
            editClass.WebOutputPath = tbWebOutputPath.Text;

            // Cache Tab
            if (rbNoCache.Checked) editClass.CacheClass = "NoCache";
            else if (rbAspCache.Checked) editClass.CacheClass = "AspCache";
            else if (rbCabCache.Checked) editClass.CacheClass = "CabCache";
            editClass.CacheName = tbCacheName.Text;

            editClass.IsCachingEnabled = cbCachingEnabled.Checked;
            editClass.CacheLifetime = TimeSpan.FromMinutes(double.Parse(tbCacheLifetime.Text));
            editClass.IsCollectionCachingEnabled = cbCollectionCachingEnabled.Checked;
            editClass.CollectionCacheLifetime = TimeSpan.FromMinutes(double.Parse(tbCollectionCacheLifetime.Text));
            editClass.IsThreadSafe = cbIsThreadSafe.Checked;

            editClass.DefaultTableName = tableNameBox.Text;
            editClass.IsTableCoded = hardCodeTables.Checked;

            editClass.IsCreateDateEnabled = cbCreateDateEnabled.Checked;
            editClass.IsModifyDateEnabled = cbModifyDateEnabled.Checked;
            editClass.Interfaces = tbInterfaces.Text;
            editClass.ToStringOverride = tbToStringOverride.Text;
            editClass.SimpleQueryEnabled = simpleQueryEnabled.Checked;

            this.Text = editClass.Name;
        }

        #endregion

        private void ClassEditor_Load(object sender, EventArgs e)
        {
            loadTab();
            loadSplitter();
            loadObjectsTree();
            loadWindowLoc();
        }

        private void ClassEditor_Shown(object sender, EventArgs e)
        {
            saveWindowState(true);
        }

        private void ClassEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveWindowState(false);
            editClass.Editor = null;
        }

 		#region Preview Tab

		private void generateButton_Click(object sender, System.EventArgs e)
		{
            System.IO.FileInfo modelFile = new System.IO.FileInfo(editClass.ParentModel.FileName);

			if(codeGeneratorComboBox.SelectedItem == null)
				return;

			OutputExtension plugin = (OutputExtension) codeGeneratorComboBox.SelectedItem;
			plugin.Init(editClass, null);
			plugin.FileName = PathConverter.GetAbsolutePath(modelFile.DirectoryName,
                editClass.OutputPath) + "\\" + 
				string.Format(plugin.OutputFileNameFormat, editClass.Name);
			plugin.Load();
            NitroCast.Controls.Highlighter h = new NitroCast.Controls.Highlighter();
            h.WriteToRTF(codePreviewBox, plugin.Render());
		}

		private void codeGeneratorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			OutputExtension p = (OutputExtension) codeGeneratorComboBox.SelectedItem;
			pluginAuthorLabel.Text = p.Author;
			pluginCopyrightLabel.Text = p.Copyright;
			pluginFilenameLabel.Text = string.Format(p.OutputFileNameFormat, editClass.Name);
			pluginDescription.Text = p.Description;
			btGenerate.Enabled = true;
		}

		#endregion

		#region Field and Children Explorer Popup Event Handlers

		private void miNewChild_Click(object sender, System.EventArgs e)
		{
			this.newChild();
		}

		private void miNewField_Click(object sender, System.EventArgs e)
		{
			this.newField();            
		}

        private void miNewEnum_Click(object sender, System.EventArgs e)
        {
            this.newEnum();
        }

        private void miNewFolder_Click(object sender, EventArgs e)
        {
            this.newFolder();
        }

		private void cmObjects_Popup(object sender, System.EventArgs e)
		{
            TreeNode selectedNode;

			miDelete.Visible = false;
			miCut.Visible = false;
			miCopy.Visible = false;
			miPaste.Visible = false;

            miBreak.Visible = false;
			miNewObject.Visible = false;
            miNewField.Visible = false;
            miNewChild.Visible = false;
            miNewEnum.Visible = false;
            miNewFolder.Visible = false;            

            selectedNode = objectsTree.SelectedNode;

            if (selectedNode != null)
            {                
                if (selectedNode.Tag is ClassFolder)
                {
                    miDelete.Visible = true;
                    miCopy.Visible = true;
                    miPaste.Visible = editClass.ParentModel.CopyObject != null;
                    miBreak.Visible = true;
                    miNewObject.Visible = true;
                    miNewField.Visible = true;
                    miNewChild.Visible = true;
                    miNewEnum.Visible = true;
                }
                else if (selectedNode.Tag is ClassField)
                {
                    miCut.Visible = true;
                    miCopy.Visible = true;
                    miDelete.Visible = true;
                }
                else if (selectedNode.Tag is ModelClass)
                {
                    miNewObject.Visible = true;
                    miNewFolder.Visible = true;
                    miPaste.Visible = editClass.ParentModel.CopyObject != null;
                }
            }
		}		

		#endregion

		#region Delete, Cut, Copy, Paste Events

        private void miDelete_Click(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = objectsTree.SelectedNode;

            if (selectedNode.Tag is ClassFolder)
            {
                editClass.Folders.Remove((ClassFolder)selectedNode.Tag);
                selectedNode.Remove();                
            }
            else if (selectedNode.Tag is ValueField)
            {
                ClassFolder folder = (ClassFolder)selectedNode.Parent.Tag;
                folder.Items.Remove(selectedNode.Tag);
                refreshLabel(selectedNode.Parent);
                selectedNode.Remove();
            }
            else if (selectedNode.Tag is ReferenceField)
            {
                ClassFolder folder = (ClassFolder)selectedNode.Parent.Tag;
                folder.Items.Remove(selectedNode.Tag);
                refreshLabel(selectedNode.Parent);
                selectedNode.Remove();
            }
            else if (selectedNode.Tag is EnumField)
            {
                ClassFolder folder = (ClassFolder)selectedNode.Parent.Tag;
                folder.Items.Remove(selectedNode.Tag);
                refreshLabel(selectedNode.Parent);
                selectedNode.Remove();
            }
        }

        private void miCut_Click(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = objectsTree.SelectedNode;

            if (selectedNode != null)
            {
                if (selectedNode.Tag is ClassFolder)
                {
                    editClass.ParentModel.CopyObject = selectedNode.Tag;

                    editClass.Folders.Remove((ClassFolder)selectedNode.Tag);
                    selectedNode.Remove();
                }
                if (selectedNode.Tag is ValueField)
                {
                    editClass.ParentModel.CopyObject = selectedNode.Tag;

                    ClassFolder folder = (ClassFolder)selectedNode.Parent.Tag;
                    folder.Items.Remove(selectedNode.Tag);
                    refreshLabel(selectedNode.Parent);
                    selectedNode.Remove();
                }
                else if (selectedNode.Tag is ReferenceField)
                {
                    editClass.ParentModel.CopyObject = selectedNode.Tag;

                    ClassFolder folder = (ClassFolder)selectedNode.Parent.Tag;
                    folder.Items.Remove(selectedNode.Tag);
                    refreshLabel(selectedNode.Parent);
                    selectedNode.Remove();
                }
                else if (selectedNode.Tag is EnumField)
                {
                    editClass.ParentModel.CopyObject = selectedNode.Tag;

                    ClassFolder folder = (ClassFolder)selectedNode.Parent.Tag;
                    folder.Items.Remove(selectedNode.Tag);
                    refreshLabel(selectedNode.Parent);
                    selectedNode.Remove();
                }
            }
        }

        private void miCopy_Click(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = objectsTree.SelectedNode;

            if (selectedNode != null)
            {
                if (selectedNode.Tag is ClassFolder)
                {
                    ClassFolder f = ((ClassFolder)selectedNode.Tag).Copy();
                    f.Name = "Copy of " + f.Name;
                    editClass.ParentModel.CopyObject = f;
                }
                if (selectedNode.Tag is ValueField)
                {
                    ValueField f = ((ValueField)selectedNode.Tag).Clone();
                    f.Name = "Copy of " + f.Name;
                    editClass.ParentModel.CopyObject = f;
                }
                else if (selectedNode.Tag is ReferenceField)
                {
                    ReferenceField f = ((ReferenceField)selectedNode.Tag).Clone();
                    f.Name = "Copy of " + f.Name;
                    editClass.ParentModel.CopyObject = f;
                }
                else if (selectedNode.Tag is EnumField)
                {
                    EnumField f = ((EnumField)selectedNode.Tag).Clone();
                    f.Name = "Copy of " + f.Name;
                    editClass.ParentModel.CopyObject = f;
                }
            }
        }

        private void miPaste_Click(object sender, System.EventArgs e)
        {
            object copyObject;
            TreeNode workingNode;
            ClassFolder folder;

            if (objectsTree.SelectedNode.Tag is ModelClass)
            {
                workingNode = objectsTree.SelectedNode;

                copyObject = editClass.ParentModel.CopyObject;
                editClass.ParentModel.CopyObject = null;

                if (copyObject is ClassFolder)
                {
                    ClassFolder copyFolder = (ClassFolder)copyObject;
                    editClass.Folders.Add(copyFolder);
                    TreeNode node = newTreeNode(copyFolder);
                    foreach (object item in copyFolder.Items)
                        node.Nodes.Add(newTreeNode(item));
                    workingNode.Nodes.Add(node);
                    refreshLabel(workingNode);
                }
            }
            if (objectsTree.SelectedNode.Tag is ClassFolder)
            {
                workingNode = objectsTree.SelectedNode;

                // Find the folder associated with the object
                // currently selected to use this folder as
                // the paste folder.
                
                if (workingNode.Tag is ClassFolder)
                {
                    folder = (ClassFolder)workingNode.Tag;                    
                }
                else if (workingNode.Parent != null &&
                    workingNode.Parent.Tag is ClassFolder)
                {
                    workingNode = workingNode.Parent;
                    folder = (ClassFolder)workingNode.Tag;                    
                }
                else
                {
                    return;
                }

                copyObject = editClass.ParentModel.CopyObject;
                editClass.ParentModel.CopyObject = null;

                // Since ValueField, ReferenceField and EnumField are
                // all objects that inherit ClassField, then we can
                // just cast the object into a ClassField to add it 
                // to the folder's item list and make a new treenode
                // with the ClassField's name.

                if (copyObject is ClassField)
                {
                    ClassField copyField = (ClassField)copyObject;
                    folder.Items.Add(copyField);
                    TreeNode node = newTreeNode(copyField);
                    workingNode.Nodes.Add(node);
                    refreshLabel(workingNode);
                    
                }
                else if (copyObject is ClassFolder)
                {
                    ClassFolder copyFolder = (ClassFolder)copyObject;
                    editClass.Folders.Add(copyFolder);
                    TreeNode node = newTreeNode(copyFolder);                    
                    objectsTree.Nodes[0].Nodes.Add(node);
                }
            }
        }

		#endregion
        
        #region Expand Collapse

        private void objectsTree_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode node = e.Node;

			if(node.Tag != null && node.Tag is ClassFolder)
			{
				node.SelectedImageIndex = 1;
				node.ImageIndex = 1;
				((ClassFolder) e.Node.Tag).IsExpanded = true;
			}
		}

		private void objectsTree_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode node = e.Node;

			if(node.Tag != null && node.Tag is ClassFolder)
			{
				node.SelectedImageIndex = 0;
				node.ImageIndex = 0;
				((ClassFolder) e.Node.Tag).IsExpanded = false;
			}
        }

        #endregion

        private void cbModifyDateEnabled_CheckedChanged(object sender, 
            System.EventArgs e)
		{			
			ddConcurrencyType.Enabled = cbModifyDateEnabled.Checked;
			if(!cbModifyDateEnabled.Checked)
				ddConcurrencyType.SelectedItem = ConcurrencyType.None;
		}
        
        #region New Field, Enum, Child and Folder Methods

        private void btNewField_Click(object sender, System.EventArgs e)
        {
            newField();
        }

        private void btNewChild_Click(object sender, System.EventArgs e)
        {
            newChild();
        }

        private void btNewFolder_Click(object sender, EventArgs e)
        {
            newFolder();
        }

		private void newField()
		{
            ClassFolder folder;            
            TreeNode newNode;
            ValueField newValue;

			folder = getCurrentFolder();

            if (folder == null)
            {
                throw (new Exception("Cannot find object to insert new field " +
                    "to."));
            }

            newValue = new ValueField();
            newValue.Name = folder.Fields.GenerateName();
            newValue.ValueType = DataTypeManager.ValueTypes[0];
            folder.Items.Add(newValue);

            newNode = newTreeNode(newValue);
            getTreeNode(folder).Nodes.Add(newNode);
            objectsTree.SelectedNode = newNode;
            refreshLabel(newNode.Parent);
            refreshPropertyGrid(newValue);
		}

        private void newEnum()
        {
            ClassFolder folder;
            TreeNode newNode;
            EnumField newEnum;

            folder = getCurrentFolder();

            if (folder == null)
            {
                throw (new Exception("Cannot find object to insert new field " +
                    "to."));
            }

            if (DataTypeManager.EnumTypes.Count == 0)
            {
                MessageBox.Show("Warning, no enums have been defined.",
                    "Class Editor", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            newEnum = new EnumField();
            newEnum.Name = folder.Fields.GenerateName();
            newEnum.EnumType = DataTypeManager.EnumTypes[0];
            folder.Items.Add(newEnum);

            newNode = newTreeNode(newEnum);
            getTreeNode(folder).Nodes.Add(newNode);
            objectsTree.SelectedNode = newNode;
            refreshLabel(newNode.Parent);
            refreshPropertyGrid(newEnum);
        }

		private void newChild()
		{
            ClassFolder folder;            
            TreeNode newNode;
            ReferenceField newRef;

			folder = getCurrentFolder();

			if(folder == null)
            {
				throw(new Exception("Cannot find object to insert new child " +
                    "to."));
            }

            if (DataTypeManager.ReferenceTypes.Count == 0)
            {
                MessageBox.Show("Warning, no classes have been defined.",
                    "Class Editor", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
			
			newRef = new ReferenceField();
			newRef.Name = folder.Children.GenerateName();
			newRef.ReferenceType = DataTypeManager.ReferenceTypes[0];			
			folder.Items.Add(newRef);

            newNode = newTreeNode(newRef);
            getTreeNode(folder).Nodes.Add(newNode);
            objectsTree.SelectedNode = newNode;
            refreshLabel(newNode.Parent);
            refreshPropertyGrid(newRef);
		}

        private void newFolder()
        {
            ClassFolder newFolder = new ClassFolder("New Folder");
            editClass.Folders.Add(newFolder);
        
            TreeNode newNode = newTreeNode(newFolder);
            objectsTree.Nodes[0].Nodes.Add(newNode);
            objectsTree.SelectedNode = newNode;
            refreshPropertyGrid(newFolder);
        }

		private ClassFolder getCurrentFolder()
		{
            TreeNode selectedNode = objectsTree.SelectedNode;

			ClassFolder folder = null;

			// Find the current folder...
			if(selectedNode != null)
			{
				if(selectedNode.Tag is ClassFolder)
					folder = (ClassFolder) selectedNode.Tag;
				else if(selectedNode.Tag is ClassField && 
                    selectedNode.Parent != null &&
                    selectedNode.Parent.Tag is ClassFolder)
					folder = (ClassFolder) selectedNode.Parent.Tag;
			}

			return folder;
		}

		#endregion

        #region General Tab - Output Paths

        private void btOutputPath_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo modelFile = 
                new System.IO.FileInfo(editClass.ParentModel.FileName);

            folderBrowserDialog1.Description = "Select a folder.";

            if (tbOutputPath.Text == "")
            {
                folderBrowserDialog1.SelectedPath = modelFile.DirectoryName;
            }
            else
            {
                folderBrowserDialog1.SelectedPath =
                    PathConverter.GetAbsolutePath(modelFile.DirectoryName, 
                    tbOutputPath.Text);
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbOutputPath.Text = 
                    PathConverter.GetRelativePath(modelFile.DirectoryName,
                    folderBrowserDialog1.SelectedPath);
            }
        }

        private void btWebOutputPath_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo modelFile = 
                new System.IO.FileInfo(editClass.ParentModel.FileName);

            folderBrowserDialog1.Description = "Select a folder.";

            if (tbOutputPath.Text == "")
            {
                folderBrowserDialog1.SelectedPath = modelFile.DirectoryName;
            }
            else
            {
                folderBrowserDialog1.SelectedPath =
                    PathConverter.GetAbsolutePath(modelFile.DirectoryName, 
                    tbWebOutputPath.Text);
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbWebOutputPath.Text = 
                    PathConverter.GetRelativePath(modelFile.DirectoryName,
                    folderBrowserDialog1.SelectedPath);
            }
        }

        #endregion

        #region Dependency Tab

        private void btRefreshDependencies_Click(object sender, EventArgs e)
        {
            // Walk the model's tree and find each Class and Field that
            // uses this Class.

            int count = 0;

            ModelClass classEntry;
            TreeNode classNode;
            ReferenceField childEntry;
            TreeNode childNode;

            classNode = null;

            depStatusLabel.Text = "Searching...";
            depTree.Nodes.Clear();

            foreach (ModelFolder folder in editClass.ParentModel.Folders)
            {
                foreach (object modelFolderEntry in folder.Items)
                {
                    if(modelFolderEntry is ModelClass)
                    {
                        classEntry = (ModelClass)modelFolderEntry;

                        foreach (ClassFolder classFolder in classEntry.Folders)
                        {
                            foreach (object classFolderEntry in classFolder.Items)
                            {
                                if (classFolderEntry is ReferenceField)
                                {
                                    childEntry = (ReferenceField)classFolderEntry;

                                    if (childEntry.ReferenceType.Name == editClass.Name &&
                                        childEntry.ReferenceType.NameSpace == editClass.Namespace)
                                    {
                                        // If the class node is null this means
                                        // that the current class being searched
                                        // has not yet been created in the
                                        // tree.
                                        if (classNode == null)
                                        {
                                            classNode = new TreeNode(classEntry.Name);
                                            classNode.Tag = classEntry;
                                            classNode.ImageIndex = 2;
                                            classNode.SelectedImageIndex = 2;                                            
                                            depTree.Nodes.Add(classNode);
                                        }

                                        childNode = new TreeNode(childEntry.Name);
                                        childNode.Tag = childEntry;
                                        childNode.ImageIndex = 3;
                                        childNode.SelectedImageIndex = 3;
                                        classNode.Nodes.Add(childNode);

                                        // Increase the dependency count.
                                        count++;
                                    }
                                }
                            }
                        }

                        // Be sure to clear the class node
                        // so that the next searched class
                        // knows to use a new class node rather
                        // than using the previous one.
                        if (classNode != null)
                            classNode.Expand();
                        classNode = null;
                    }
                }
            }

            
            if (count == 0)
            {
                depStatusLabel.Text = "0 dependencies found.";
            }
            else if (count == 1)
            {
                depStatusLabel.Text = "1 dependency found.";
            }
            else
            {
                depStatusLabel.Text = count.ToString() + " dependencies found.";
            }
        }

        #endregion

        #region Window State Methods

        private void ClassEditor_Move(object sender, EventArgs e)
        {
            saveWindowLoc();
        }

        private void ClassEditor_SizeChanged(object sender, EventArgs e)
        {
            saveWindowLoc();
        }

        private void loadWindowLoc()
        {
            string dataString = editClass.Attributes["WindowLoc"];
            string[] data = dataString.Split(',');

            if (data.Length > 1)
            {
                this.Location = new Point(int.Parse(data[0]), 
                    int.Parse(data[1]));
            }

            if (data.Length > 2)
            {
                this.Width = int.Parse(data[2]);
                this.Height = int.Parse(data[3]);
            }
        }

        private void saveWindowLoc()
        {
            editClass.Attributes["WindowLoc"] =
                Location.X.ToString() + "," +
                Location.Y.ToString() + "," +
                Width.ToString() + "," +
                Height.ToString();
        }

        private void saveWindowState(bool isOpen)
        {
            editClass.Attributes["WindowState"] =
                isOpen.ToString();
        }

        #endregion       

        #region Tab Methods

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveTab();
        }

        private void saveTab()
        {
            editClass.Attributes["WindowTab"] = tabControl1.SelectedTab.Name;
        }

        private void loadTab()
        {
            if (editClass.Attributes["WindowTab"] != string.Empty)
            {
                tabControl1.SelectTab(editClass.Attributes["WindowTab"]);
            }
        }

        #endregion

        #region Objects Splitter

        private void loadSplitter()
        {
            if (editClass.Attributes["FieldSplitterDistance"] != string.Empty)
            {
                splitContainer1.SplitterDistance = int.Parse(
                    editClass.Attributes["FieldSplitterDistance"]);
            }
        }

        private void splitter1_SplitterMoving(object sender,
            System.Windows.Forms.SplitterEventArgs e)
        {
            objectsTree.Width = e.SplitX - e.X;
            pgFields.Left = e.SplitY - e.X;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            editClass.Attributes["FieldSplitterDistance"] =
                splitContainer1.Panel1.Width.ToString();
        }
        
        #endregion

        #region Caching Tab

        private void tabCaching_Click(object sender, EventArgs e)
        {

        }

        private void rbAspCache_CheckedChanged(object sender, EventArgs e)
        {
            cbCachingEnabled.Enabled = true;
            cbCollectionCachingEnabled.Enabled = true;
            tbCacheLifetime.Enabled = true;
            tbCollectionCacheLifetime.Enabled = true;
            tbCacheName.Enabled = false;
        }

        private void rbCabCache_CheckedChanged(object sender, EventArgs e)
        {
            cbCachingEnabled.Enabled = true;
            cbCollectionCachingEnabled.Enabled = true;
            tbCacheLifetime.Enabled = true;
            tbCollectionCacheLifetime.Enabled = true;
            tbCacheName.Enabled = true;
        }

        private void rbNoCache_CheckedChanged(object sender, EventArgs e)
        {
            cbCachingEnabled.Enabled = false;
            cbCollectionCachingEnabled.Enabled = false;
            tbCacheLifetime.Enabled = false;
            tbCollectionCacheLifetime.Enabled = false;
            tbCacheName.Enabled = false;
        }

        private void cbCachingEnabled_CheckedChanged(object sender,
            System.EventArgs e)
        {
            lblClusterWarning.Visible = cbCachingEnabled.Checked;
            tbCacheLifetime.ReadOnly = !cbCachingEnabled.Checked;
        }

        private void cbCollectionCachingEnabled_CheckedChanged(object sender,
            EventArgs e)
        {
            lblClusterWarning.Visible = cbCollectionCachingEnabled.Checked;
            tbCollectionCacheLifetime.ReadOnly = !cbCollectionCachingEnabled.Checked;
        }

        #endregion

        #region Property Grid Methods

        private void refreshPropertyGrid()
        {
            pgFields.Refresh();
        }

        private void refreshPropertyGrid(object value)
        {
            pgFields.SelectedObject = value;
        }

        private void appendPropertyGrid()
        {
        }

        private void propertiesGridEx_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

        }

        #endregion

        #region Objects Tree Methods

        #region Drag and Drop

        private void objectsTree_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Point pt;
            TreeNode srcNode;
            TreeNode dstNode;
            TreeNode oldSrcParentNode;
            int insertIndex;

            srcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode", false);

            if (srcNode != null)
            {
                pt = objectsTree.PointToClient(new Point(e.X, e.Y));
                dstNode = objectsTree.GetNodeAt(pt);

                if (srcNode.Tag is ClassFolder && dstNode.Tag is ModelClass &&
                    srcNode.Parent.Tag != dstNode.Tag)
                {
                    oldSrcParentNode = srcNode.Parent;
                    ((ModelClass)dstNode.Tag).
                        Move(srcNode.Tag, dstNode.Tag);
                    objectsTree.BeginUpdate();
                    srcNode.Remove();
                    refreshLabel(oldSrcParentNode);
                    dstNode.Nodes.Add(srcNode);
                    refreshLabel(dstNode);
                    objectsTree.EndUpdate();
                    srcNode.TreeView.SelectedNode = srcNode;
                }
                if (srcNode.Tag is ClassField && dstNode.Tag is ClassFolder &&
                    srcNode.Parent.Tag != dstNode.Tag)
                {
                    oldSrcParentNode = srcNode.Parent;
                    ((ClassFolder)dstNode.Tag).ParentClass.
                        Move(srcNode.Tag, dstNode.Tag);
                    objectsTree.BeginUpdate();
                    srcNode.Remove();
                    refreshLabel(oldSrcParentNode);
                    dstNode.Nodes.Add(srcNode);
                    refreshLabel(dstNode);
                    objectsTree.EndUpdate();
                    srcNode.TreeView.SelectedNode = srcNode;
                }
                else if (srcNode.Tag is ClassField && dstNode.Tag is ClassField &&
                    srcNode.Parent.Tag != dstNode.Parent.Tag)
                {
                    insertIndex = ((ClassFolder)dstNode.Parent.Tag)
                        .Items.IndexOf(dstNode.Tag);

                    // Fix for odd placement in same folder.
                    if (srcNode.Parent.Index < dstNode.Parent.Index)
                        insertIndex++;

                    oldSrcParentNode = srcNode.Parent;

                    ((ClassFolder)dstNode.Parent.Tag).ParentClass.
                        Move(srcNode.Tag, dstNode.Parent.Tag, insertIndex);
                    objectsTree.BeginUpdate();
                    srcNode.Remove();
                    refreshLabel(oldSrcParentNode);
                    dstNode.Parent.Nodes.Insert(insertIndex, srcNode);
                    refreshLabel(dstNode.Parent);
                    objectsTree.EndUpdate();
                    srcNode.TreeView.SelectedNode = srcNode;
                }
            }
        }

        private void objectsTree_DragEnter(object sender,
            System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void objectsTree_ItemDrag(object sender,
            System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void objectsTree_DragOver(object sender,
            System.Windows.Forms.DragEventArgs e)
        {
            Point pt;
            pt = objectsTree.PointToClient(new Point(e.X, e.Y));
            objectsTree.SelectedNode = objectsTree.GetNodeAt(pt);
        }

        #endregion

        #region Label Editing

        private void objectsTree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            object selectedObject = e.Node.Tag;

            e.CancelEdit = !
                (selectedObject is ClassFolder |
                selectedObject is ValueField |
                selectedObject is ReferenceField |
                selectedObject is EnumField);

            if (selectedObject is ClassFolder)
            {
                e.Node.Text = ((ClassFolder)selectedObject).Name;
            }
        }

        private void objectsTree_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            string newName;
            object selectedObject;

            objectsTree.LabelEdit = false;
            newName = e.Label;
            selectedObject = e.Node.Tag;

            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (newName.IndexOfAny(new char[] { '@', '.', ',', '!', '\\', '/' }) == -1)
                    {
                        if (selectedObject is ClassField)
                        {
                            if (!SQLChecker.KeywordCheck(newName))
                            {
                                ((ClassField)selectedObject).Name = e.Label;
                                e.Node.Text = e.Label;
                                refreshPropertyGrid();
                            }
                            else
                            {
                                e.CancelEdit = true;
                                MessageBox.Show("Invalid name.\n" +
                                            "Reserved SQL Keyword.",
                                            "Property Name Edit");
                                objectsTree.LabelEdit = true;
                                e.Node.BeginEdit();
                            }
                        }
                        else if (selectedObject is ClassFolder)
                        {
                            ((ClassFolder)selectedObject).Name = e.Label;
                            e.Node.Text = e.Label + " (" +
                                ((ClassFolder)selectedObject).Items.Count.ToString() +
                                " items)";
                            refreshPropertyGrid();
                        }
                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid name.\n" +
                                    "Invalid characters in name.",
                                    "Property Name Edit");
                        objectsTree.LabelEdit = true;
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid name.\n" +
                        "Name must be specified.",
                        "Property Name Edit");
                    objectsTree.LabelEdit = true;
                    e.Node.BeginEdit();
                }
            }
        }

        #endregion

        #region Selection

        private void objectsTree_AfterSelect(object sender,
            System.Windows.Forms.TreeViewEventArgs e)
        {
            TreeNode selectedNode = objectsTree.SelectedNode;

            if (selectedNode != null && selectedNode.Tag != null)
            {
                if (selectedNode.Tag is ModelClass)
                {
                    refreshPropertyGrid(null);
                    btNewChild.Enabled = false;
                    btNewField.Enabled = false;                    
                }

                if (selectedNode.Tag is ClassFolder)
                {
                    refreshPropertyGrid(selectedNode.Tag);
                    btNewChild.Enabled = true;
                    btNewField.Enabled = true;
                }
                else if (selectedNode.Tag is ValueField)
                {
                    refreshPropertyGrid(selectedNode.Tag);
                    btNewChild.Enabled = false;
                    btNewField.Enabled = false;
                }
                else if (selectedNode.Tag is ReferenceField)
                {
                    refreshPropertyGrid(selectedNode.Tag);
                    btNewChild.Enabled = false;
                    btNewField.Enabled = false;
                }
                else if (selectedNode.Tag is EnumField)
                {
                    refreshPropertyGrid(selectedNode.Tag);
                    btNewChild.Enabled = false;
                    btNewField.Enabled = false;
                }

                editClass.Attributes["PropertyGridSelectLabel"] =
                    ((IModelEntry)selectedNode.Tag).Name;
            }
        }

        #endregion

        #endregion

        #region Tree Handling Methods

        private TreeNode getTreeNode(object tag)
        {
            return getTreeNode(objectsTree.Nodes, tag);
        }

        private TreeNode getTreeNode(TreeNodeCollection nodes, object tag)
        {
            TreeNode node = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Tag == tag)
                {
                    return nodes[i];
                }

                if (nodes[i].Nodes.Count > 0)
                {
                    node = getTreeNode(nodes[i].Nodes, tag);

                    if (node != null)
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        private void selectTreeNode(object tag)
        {
            TreeNode node = getTreeNode(tag);

            if (node != null)
            {
                objectsTree.SelectedNode = node;
                refreshPropertyGrid(tag);
            }
            else
            {
                objectsTree.SelectedNode = null;
                refreshPropertyGrid(null);
            }
        }

        /// <summary>
        /// Refreshes the objects tree and optionally rebinds the property grid
        /// to the currently selected node if it is editable.
        /// </summary>
        /// <param name="rebindPropertyGrid"></param>
        private void loadObjectsTree()
        {
            TreeNode baseNode;
            TreeNode folderNode;

            objectsTree.BeginUpdate();

            objectsTree.Nodes.Clear();

            baseNode = newTreeNode(editClass);
            objectsTree.Nodes.Add(baseNode);

            foreach (ClassFolder folder in editClass.Folders)
            {
                if (!folder.IsBrowsable)
                    continue;

                folderNode = newTreeNode(folder);
                baseNode.Nodes.Add(folderNode);

                if (editClass.Attributes["PropertyGridSelectLabel"] ==
                        ((ModelEntry)folderNode.Tag).Name)
                {
                    refreshPropertyGrid(folderNode.Tag);
                    objectsTree.SelectedNode = folderNode;
                }

                foreach (object i in folder.Items)
                {
                    TreeNode newNode = newTreeNode(i);

                    folderNode.Nodes.Add(newNode);

                    if (editClass.Attributes["PropertyGridSelectLabel"] ==
                            ((IModelEntry)newNode.Tag).Name)
                    {
                        refreshPropertyGrid(i);
                        objectsTree.SelectedNode = newNode;
                    }
                }

                // Be sure to expand or collapse the folder after
                // the items have been added and removed or else
                // the folders will all be collapsed by default.
                if (folder.IsExpanded)
                {
                    folderNode.Expand();
                }
            }

            baseNode.Expand();

            objectsTree.EndUpdate();

            if (objectsTree.SelectedNode != null)
            {
                objectsTree.SelectedNode.EnsureVisible();
            }
        }

        private void refreshLabel(object obj)
        {
            TreeNode node;
            ClassFolder folder;

            if (obj is TreeNode)
            {
                node = (TreeNode)obj;
                if (node.Tag is ClassFolder)
                {
                    folder = (ClassFolder)node.Tag;
                    node.Text = folder.Name + " (" +
                        folder.Items.Count.ToString() + " items)";
                }
            }
            else if (obj is ClassFolder)
            {
                node = getTreeNode(obj);
                refreshLabel(node);
            }
        }

        private TreeNode newTreeNode(object obj)
        {
            TreeNode node;

            node = new TreeNode();

            if (obj is ModelClass)
            {
                node.Text = ((ModelClass)obj).Name;
                node.Tag = obj;
                node.ImageIndex = 3;
                node.SelectedImageIndex = 3;
            }
            if (obj is ValueField)
            {
                node.Text = ((ValueField)obj).Name;
                node.Tag = obj;
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
            }
            else if (obj is EnumField)
            {
                node.Text = ((EnumField)obj).Name;
                node.Tag = obj;
                node.ImageIndex = 4;
                node.SelectedImageIndex = 4;
            }
            else if (obj is ReferenceField)
            {
                node.Text = ((ReferenceField)obj).Name;
                node.Tag = obj;
                node.ImageIndex = 3;
                node.SelectedImageIndex = 3;
            }
            else if (obj is ClassFolder)
            {
                node.Text = ((ClassFolder)obj).Name + " (" +
                    ((ClassFolder)obj).Items.Count.ToString() + " items)";
                node.Tag = obj;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
            }

            node.Tag = obj;

            return node;
        }

        private void btUp_Click(object sender, System.EventArgs e)
        {
            TreeNode selectedNode;
            TreeNode parent;
            int index;

            selectedNode = objectsTree.SelectedNode;
            parent = selectedNode.Parent;
            index = selectedNode.Index;

            if (selectedNode.Tag is ClassFolder &&
                index > 0)
            {
                ClassFolder f = (ClassFolder)selectedNode.Tag;
                editClass.MoveUp(editClass.Folders.IndexOf(f));

                objectsTree.BeginUpdate();
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index - 1, selectedNode);
                objectsTree.EndUpdate();
                objectsTree.SelectedNode = selectedNode;
            }

            if (parent != null &&
                parent.Tag is ClassFolder &&
                index > 0)
            {
                ClassFolder f = (ClassFolder)parent.Tag;
                f.MoveUp(f.Items.IndexOf(selectedNode.Tag));

                objectsTree.BeginUpdate();
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index - 1, selectedNode);
                objectsTree.EndUpdate();
                objectsTree.SelectedNode = selectedNode;
            }
        }

        private void btDown_Click(object sender, System.EventArgs e)
        {
            TreeNode selectedNode;
            TreeNode parent;
            int index;

            selectedNode = objectsTree.SelectedNode;
            parent = selectedNode.Parent;
            index = selectedNode.Index;

            if (selectedNode.Tag is ClassFolder &&
                index < parent.Nodes.Count)
            {
                editClass.MoveDown(editClass.Folders.IndexOf((ClassFolder)
                    selectedNode.Tag));

                objectsTree.BeginUpdate();
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index + 1, selectedNode);
                objectsTree.EndUpdate();
                objectsTree.SelectedNode = selectedNode;
            }

            if (parent != null &&
                parent.Tag is ClassFolder &&
                index < parent.Nodes.Count)
            {
                ClassFolder f = (ClassFolder)parent.Tag;
                f.MoveDown(f.Items.IndexOf(selectedNode.Tag));

                objectsTree.BeginUpdate();
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index + 1, selectedNode);
                objectsTree.EndUpdate();
                objectsTree.SelectedNode = selectedNode;
            }
        }

        #endregion

        #region PropertyGrid

        private void fieldProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            TreeNode treeNode;
            object selectedObject = pgFields.SelectedObject;
            string value = e.ChangedItem.Value.ToString();

            treeNode = getTreeNode(pgFields.SelectedObject);

            if (e.ChangedItem.Label == "Name")
            {
                if (value.Length > 0)
                {
                    if (value.IndexOfAny(new char[] { '@', '.', ',', '!', '\\', '/' }) == -1)
                    {
                        if (selectedObject is ClassField)
                        {
                            if (!SQLChecker.KeywordCheck(value))
                            {
                                ((ClassField)selectedObject).Name = value;
                                treeNode.Text = value;
                            }
                            else
                            {
                                ((ClassField)selectedObject).Name =
                                    e.OldValue.ToString();
                                pgFields.Refresh();
                                MessageBox.Show("Invalid name.\n" +
                                            "Reserved SQL Keyword.",
                                            "Property Name Edit");
                            }
                        }
                        else if (selectedObject is ClassFolder)
                        {
                            ((ClassFolder)selectedObject).Name = value;
                            treeNode.Text = value + " (" +
                                ((ClassFolder)selectedObject).Items.Count.ToString() +
                                " items)";
                        }
                    }
                    else
                    {
                        if (selectedObject is ClassField)
                        {
                            ((ClassField)selectedObject).Name =
                                e.OldValue.ToString();
                        }
                        else if (selectedObject is ClassFolder)
                        {
                            ((ClassFolder)selectedObject).Name =
                                e.OldValue.ToString();
                        }
                        pgFields.Refresh();
                        MessageBox.Show("Invalid name.\n" +
                                    "Invalid characters in name.",
                                    "Property Name Edit");
                    }
                }
                else
                {
                    if (selectedObject is ClassField)
                    {
                        ((ClassField)selectedObject).Name =
                            e.OldValue.ToString();
                    }
                    else if (selectedObject is ClassFolder)
                    {
                        ((ClassFolder)selectedObject).Name =
                            e.OldValue.ToString();
                    }
                    pgFields.Refresh();
                    MessageBox.Show("Invalid name.\n" +
                        "Name must be specified.",
                        "Property Name Edit");
                }
            }
        }

        #endregion

        private void objectsTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is ModelClass)
                e.Cancel = true;
        }
    }
}