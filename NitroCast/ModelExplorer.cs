using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using NitroCast.Core;
using NitroCast.Core.Extensions;
using NitroCast.Core.UI;

namespace NitroCast
{
	/// <summary>
	/// Summary description for SolutionExplorer.
	/// </summary>
	public class ModelExplorer : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TreeView modelTree;

		private DataModel model;
        private System.Windows.Forms.ContextMenu cmObjects;
                

		public ModelExplorer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
            this.Closing += new CancelEventHandler(this.modelExplorer_Cancel);

            ImageList myImageList = new ImageList();	
		
			myImageList.Images.Add(Properties.Resources.FolderClosed);
			myImageList.Images.Add(Properties.Resources.FolderOpen);
			myImageList.Images.Add(Properties.Resources.VSObject_Field);
			myImageList.Images.Add(Properties.Resources.VSObject_Class);
			myImageList.Images.Add(Properties.Resources.VSObject_Assembly);
            myImageList.Images.Add(Properties.Resources.VSObject_Enum);
            myImageList.TransparentColor = Color.Magenta;

			modelTree.ImageList = myImageList;

			modelTree.Enabled = false;
		}

        public void Clear()
        {
            this.model = null;

            modelTree.BeginUpdate();
            modelTree.Nodes.Clear();
            modelTree.EndUpdate();
        }

        public void BindModel(DataModel model)
		{
            modelTree.Enabled = true;

			this.model = model;

            model.Changed += new EventHandler(model_Changed);

            modelTree.BeginUpdate();

            TreeNode modelNode = new TreeNode();
            modelNode.Tag = model;
            modelTree.Nodes.Add(modelNode);

            TreeNode referencesNode = new TreeNode("References");
            modelNode.Nodes.Add(referencesNode);

            TreeNode foldersNode = new TreeNode("Classes");
            modelNode.Nodes.Add(foldersNode);

            TreeNode pluginNode = new TreeNode("Plugins");
            modelNode.Nodes.Add(pluginNode);

            modelTree.EndUpdate();

			refreshTree();
		}

		private void model_Changed(object sender, EventArgs e)
		{
			refreshTree();
		}

		#region Refresh Tree

		private void refreshTree()
		{
			object selectedTag = null;
			ModelClass modelClass;
            ModelEnum modelEnum;
            TreeNode modelNode;
            TreeNode foldersNode;
            TreeNode referencesNode;

            modelNode = modelTree.Nodes[0];
            referencesNode = modelNode.Nodes[0];
            foldersNode = modelNode.Nodes[1];

			if(modelTree.SelectedNode != null)
			{
				selectedTag = modelTree.SelectedNode.Tag;
			}

			modelTree.BeginUpdate();

			modelTree.Nodes[0].Text = string.Format("Model '{0}' ({1} classes)", 
				model.Name, model.CalcClassCount());            

			ReferenceEntryCollection sortedReferences = 
				model.References.Clone();
			sortedReferences.Sort();

			modelTree.Nodes[0].Nodes[0].Nodes.Clear();
			foreach(ReferenceEntry r in sortedReferences)
			{
				TreeNode n = new TreeNode();
                n = newTreeNode(r);
                referencesNode.Nodes.Add(n);

				if(selectedTag != null && n.Tag == selectedTag)
				{
					modelTree.SelectedNode = n;
				}

				foreach(ReferenceType type in DataTypeManager.ReferenceTypes)
				{
					if(type.ParentReferenceEntry != null && 
						type.ParentReferenceEntry.Name == r.Name)
					{
						TreeNode childNode = new TreeNode();
                        childNode = newTreeNode(type);
						n.Nodes.Add(childNode);

						if(selectedTag != null && 
							childNode.Tag == selectedTag)
						{
							modelTree.SelectedNode = childNode;
						}
					}
				}
			}
            referencesNode.Expand();

			ModelFolderCollection sortedFolders = model.Folders.Clone();
			sortedFolders.Sort(ModelEntryCompareKey.Name);

			foldersNode.Nodes.Clear();

			foreach(ModelFolder folder in sortedFolders)
			{
				TreeNode folderNode = new TreeNode();
				folderNode.Tag = folder;
				folderNode.Text = folder.Name;
				foldersNode.Nodes.Add(folderNode);

				if(selectedTag != null && 
					folderNode.Tag == selectedTag)
				{
					modelTree.SelectedNode = folderNode;
				}

				folderNode.Nodes.Clear();

				folder.Items.Sort((IComparer) new ModelEntryComparer(ModelEntryCompareKey.Name));

				foreach(object i in folder.Items)
				{
					if(i is ModelClass)
					{
						modelClass = (ModelClass) i;
                        TreeNode n = newTreeNode(modelClass);
						folderNode.Nodes.Add(n);

						if(selectedTag != null && 
							n.Tag == selectedTag)
						{
							modelTree.SelectedNode = n;
						}                        
					}
                    else if (i is ModelEnum)
                    {
                        modelEnum = (ModelEnum)i;
                        TreeNode n = newTreeNode(modelEnum);                        
                        folderNode.Nodes.Add(n);

                        if (selectedTag != null &&
                            n.Tag == selectedTag)
                        {
                            modelTree.SelectedNode = n;
                        }
                    }
				}

				// Be sure to expand or collapse the folder after
				// the items have been added and removed or else
				// the folders will all be collapsed by default.
				if(folder.IsExpanded)
				{
					folderNode.Expand();
					folderNode.SelectedImageIndex = 1;
					folderNode.ImageIndex = 1;
				}
				else
				{					
					folderNode.SelectedImageIndex = 0;
					folderNode.ImageIndex = 0;
				}
			}
            foldersNode.Expand();

			if(modelTree.SelectedNode != null)
			{
				modelTree.SelectedNode.EnsureVisible();
			}

            modelNode.Expand();

			modelTree.EndUpdate();
		}

        private TreeNode newTreeNode(object obj)
        {
            TreeNode n = new TreeNode();

            if (obj is ModelClass)
            {
                ModelClass c = (ModelClass)obj;
                n.Tag = c;
                n.Text = c.Name;
                n.ImageIndex = 3;
                n.SelectedImageIndex = 3;
                c.Updated += new ModelClassEventHandler(classUpdated);
            }
            else if (obj is ModelEnum)
            {
                ModelEnum e = (ModelEnum)obj;
                n.Tag = e;
                n.Text = e.Name;
                n.ImageIndex = 5;
                n.SelectedImageIndex = 5;
            }
            else if (obj is ReferenceEntry)
            {
                ReferenceEntry r = (ReferenceEntry)obj;
                n.Tag = r;
                n.Text = r.Name;
                n.ImageIndex = 4;
                n.SelectedImageIndex = 4;
            }
            else if (obj is ReferenceType)
            {
                ReferenceType type = (ReferenceType)obj;
                n.Tag = type;
                n.Text = type.Name;
                n.ImageIndex = 3;
                n.SelectedImageIndex = 3;
            }

            return n;
        }

        private void classUpdated(object sender, ModelClassEventArgs e)
        {
            // Search Tree and Update Node                        
            TreeNode n = findNode(modelTree.Nodes, e.ParentClass);
            if (n != null)
            {
                modelTree.BeginUpdate();
                n.Text = e.ParentClass.Name;
                modelTree.EndUpdate();
            }
        }

        private TreeNode findNode(TreeNodeCollection nodes, object tag)
        {            
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == tag)
                    return node;
                if (node.Nodes.Count > 0)
                    return findNode(node.Nodes, tag);
            }

            return null;
        }

		#endregion

		protected void modelExplorer_Cancel(Object sender, CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
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
            this.cmObjects = new System.Windows.Forms.ContextMenu();
            this.modelTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // cmObjects
            // 
            this.cmObjects.Popup += new System.EventHandler(this.cmObjects_Popup);
            // 
            // modelTree
            // 
            this.modelTree.AllowDrop = true;
            this.modelTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.modelTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modelTree.ContextMenu = this.cmObjects;
            this.modelTree.HideSelection = false;
            this.modelTree.Location = new System.Drawing.Point(2, 2);
            this.modelTree.Name = "modelTree";
            this.modelTree.Size = new System.Drawing.Size(236, 414);
            this.modelTree.TabIndex = 1;
            this.modelTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterCollapse);
            this.modelTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.modelTree_AfterLabelEdit);
            this.modelTree.DoubleClick += new System.EventHandler(this.modelTree_DoubleClick);
            this.modelTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.modelTree_BeforeCollapse);
            this.modelTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.modelTree_DragDrop);
            this.modelTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.objectsTree_MouseDown);
            this.modelTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.modelTree_DragEnter);
            this.modelTree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.modelTree_BeforeLabelEdit);
            this.modelTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterExpand);
            this.modelTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.modelTree_ItemDrag);
            this.modelTree.DragOver += new System.Windows.Forms.DragEventHandler(this.modelTree_DragOver);
            // 
            // ModelExplorer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(240, 418);
            this.Controls.Add(this.modelTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ModelExplorer";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Model Explorer";
            this.Resize += new System.EventHandler(this.ModelExplorer_Resize);
            this.ResumeLayout(false);

		}
		#endregion

        #region Double Click Event Handler

        private void modelTree_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = this.modelTree.SelectedNode;

            if (selectedNode.Parent == null &&
                selectedNode.Tag is DataModel)
            {
                modelProperties(this, e);
            }						
			else
			{
                if (selectedNode.Parent.Tag is ModelFolder)
                {
                    if (selectedNode.Tag is ModelClass)
                    {
                        ModelClass editClass = (ModelClass)selectedNode.Tag;

                        // Bring the edit window to focus if there is already one attached to the field
                        if (editClass.Editor != null)
                        {
                            ((ClassEditor)editClass.Editor).Focus();
                            return;
                        }

                        ClassEditor classEditor = new ClassEditor(editClass);
                        classEditor.MdiParent = this.MdiParent;
                        classEditor.Show();
                    }
                    else if (selectedNode.Tag is ModelEnum)
                    {
                        ModelEnum editEnum = (ModelEnum)selectedNode.Tag;

                        if (editEnum.Editor != null)
                        {
                            ((EnumEditor)editEnum.Editor).Focus();
                            return;
                        }

                        EnumEditor enumEditor = new EnumEditor(editEnum);
                        enumEditor.MdiParent = this.MdiParent;
                        enumEditor.Show();
                    }
                }
			}
        }

        #endregion

        #region Mouse Down Context Menu Event Handler

        private void objectsTree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{		
			TreeNode selectedNode;
			MenuItem item;
		
			if(e.Button == MouseButtons.Right & e.Clicks == 1)
			{
				selectedNode = modelTree.GetNodeAt(e.X, e.Y);
				modelTree.SelectedNode = selectedNode;				
						
				// Set Popup Menu
				cmObjects.MenuItems.Clear();

				if(selectedNode.Tag == null)				
				{
					if(selectedNode.Text == "Classes")
					{
						item = new MenuItem("A&dd folder...", new System.EventHandler(addFolder));
						cmObjects.MenuItems.Add(0, item);				
					}
					else if(selectedNode.Text == "References")
					{
						item = new MenuItem("A&dd reference...", new System.EventHandler(addReference));
						cmObjects.MenuItems.Add(0, item);
					}
				}
				else if(selectedNode.Tag is ModelFolder)
				{
					item = new MenuItem("Add &class...", new System.EventHandler(addClass));
					cmObjects.MenuItems.Add(0, item);
                    item = new MenuItem("Add &enum...", new System.EventHandler(addEnum));
                    cmObjects.MenuItems.Add(1, item);
					item = new MenuItem("&Delete", new System.EventHandler(deleteFolder));
					cmObjects.MenuItems.Add(2, item);
				}
				else if(selectedNode.Tag is ModelClass)
				{
					item = new MenuItem("A&dd class...", new System.EventHandler(addClass));
					cmObjects.MenuItems.Add(0, item);
                    item = new MenuItem("Add &enum...", new System.EventHandler(addEnum));
                    cmObjects.MenuItems.Add(1, item);
					item = new MenuItem("&Delete class", new System.EventHandler(deleteClass));
					cmObjects.MenuItems.Add(2, item);
				}
				else if(selectedNode.Tag is ReferenceEntry)
				{
					item = new MenuItem("A&dd reference...", new System.EventHandler(addReference));
					cmObjects.MenuItems.Add(0, item);
					item = new MenuItem("&Delete reference", new System.EventHandler(deleteReference));
					cmObjects.MenuItems.Add(1, item);
				}
                else if (selectedNode.Tag is ModelEnum)
                {
                    item = new MenuItem("A&dd class...", new System.EventHandler(addClass));
                    cmObjects.MenuItems.Add(0, item);
                    item = new MenuItem("Add &enum...", new System.EventHandler(addEnum));
                    cmObjects.MenuItems.Add(1, item);
                    item = new MenuItem("&Delete enum", new System.EventHandler(deleteEnum));
                    cmObjects.MenuItems.Add(2, item);
                }
                else if (selectedNode.Tag is DataModel)
                {
                    item = new MenuItem("&Properties...", new System.EventHandler(modelProperties));
                    cmObjects.MenuItems.Add(0, item);
                }
			}
        }

        #endregion

        private void modelProperties(object sender, EventArgs e)
        {
            ModelProperties modelProperties = new ModelProperties(model);
            modelProperties.MdiParent = this.ParentForm;
            modelProperties.Show();
        }

		#region Model Folder Event Handling

		private void addFolder(object sender, System.EventArgs e)
		{
			ModelFolder folder;

			folder = new ModelFolder("New Folder");
			model.Folders.Add(folder);

			refreshTree();
		}

		private void deleteFolder(object sender, System.EventArgs e)
		{
			TreeNode selectedNode;
			ModelFolder folder;

			selectedNode = modelTree.SelectedNode;

			if(selectedNode.Tag is ModelFolder)
			{
				folder = (ModelFolder) selectedNode.Tag;

				if(folder.Items.Count == 0)
				{
					model.Folders.Remove(folder);
					refreshTree();
				}
			}
		}

		#endregion

		#region Class Entry Event Handling

		private void addClass(object sender, System.EventArgs e)
		{			
			TreeNode selectedNode;
			ModelFolder folder;

			selectedNode = modelTree.SelectedNode;

			// Find Current Folder
			while(selectedNode.Parent != null &
				!(selectedNode.Tag is ModelFolder))
			{
				selectedNode = selectedNode.Parent;
			}

			if(selectedNode.Tag is ModelFolder)
			{
				folder = (ModelFolder) selectedNode.Tag;
			}
			else
			{
				return;
			}
			
			// Instantiate new class object
			ModelClass newClass = new ModelClass();
			newClass.ParentModel = this.model;
			newClass.Namespace = this.model.DefaultNamespace;
			folder.Items.Add(newClass);

			// Add class object to DataTypeManager
			DataTypeManager.ReferenceTypes.Add(new ReferenceType(newClass, null));

			// Instantiate new class object editor
			ClassEditor newClassEditor = new ClassEditor(newClass);
			newClassEditor.MdiParent = this.MdiParent;
			newClassEditor.Show();

			refreshTree();
		}

		private void deleteClass(object sender, System.EventArgs e)
		{
			TreeNode selectedNode;
			ModelFolder parentFolder;
			ModelClass deleteClass;			
			ModelClass scanClass;

			parentFolder = null;
			selectedNode = modelTree.SelectedNode;

			if(selectedNode.Tag is ModelClass)
			{
				// Check to make sure there are no immediate dependencies
				// otherwise display error. Also find the entry's parent
				// folder if the class can be deleted.

				deleteClass = (ModelClass)selectedNode.Tag;

				foreach(ModelFolder folder in model.Folders)
				{
					foreach(object i in folder.Items)
					{
						if(i is ModelClass)
						{
                            scanClass = (ModelClass)i;

							if(deleteClass == scanClass)
							{
								parentFolder = folder;
							}
							
							foreach(ReferenceField child in scanClass.ReferenceFields)
							{
								if(child.ReferenceType.ParentClassEntry == deleteClass)
								{
									return;
								}
							}
						}
					}
				}

				// Since the loop completed, that means there
				// were not references found. Delete the class.
				if(parentFolder != null)
				{
					parentFolder.Items.Remove(deleteClass);
                    DataTypeManager.DeleteDataType(deleteClass);
					refreshTree();
				}
			}
		}

		#endregion

        #region Enum Entry Event Handling

        private void addEnum(object sender, System.EventArgs e)
        {
            TreeNode selectedNode;
            ModelFolder folder;

            selectedNode = modelTree.SelectedNode;

            // Find Current Folder
            while (selectedNode.Parent != null &
                !(selectedNode.Tag is ModelFolder))
            {
                selectedNode = selectedNode.Parent;
            }

            if (selectedNode.Tag is ModelFolder)
            {
                folder = (ModelFolder)selectedNode.Tag;
            }
            else
            {
                return;
            }

            // Instantiate new class object
            ModelEnum newEnum = new ModelEnum();
            newEnum.Name = "NewEnum";
            newEnum.ParentModel = this.model;
            newEnum.Namespace = this.model.DefaultNamespace;
            folder.Items.Add(newEnum);

            // Add class object to DataTypeManager
            DataTypeManager.EnumTypes.Add(new EnumType(newEnum, null));

            // Instantiate new class object editor
            EnumEditor newEnumEditor = new EnumEditor(newEnum);
            newEnumEditor.MdiParent = this.MdiParent;
            newEnumEditor.Show();

            refreshTree();
        }

        private void deleteEnum(object sender, System.EventArgs e)
        {
            TreeNode selectedNode;
            ModelFolder parentFolder;
            ModelEnum deleteEnum;
            ModelEnum scanEnum;
            ModelClass scanClass;

            parentFolder = null;
            selectedNode = modelTree.SelectedNode;

            if (selectedNode.Tag is ModelEnum)
            {
                // Check to make sure there are no immediate dependencies
                // otherwise display error. Also find the entry's parent
                // folder if the class can be deleted.

                deleteEnum = (ModelEnum)selectedNode.Tag;

                foreach (ModelFolder folder in model.Folders)
                {
                    foreach (object i in folder.Items)
                    {
                        if (i is ModelEnum)
                        {
                            scanEnum = (ModelEnum)i;

                            if(deleteEnum == scanEnum)
                            {
                                parentFolder = folder;
                            }
                        }

                        if (i is ModelClass)
                        {   
                            scanClass = (ModelClass)i;

                            foreach(EnumField ef in scanClass.EnumFields)
                            {
                                if (ef.EnumType.ParentEnumEntry == deleteEnum)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                // Since the loop completed, that means there
                // were not references found. Delete the class.
                if (parentFolder != null)
                {
                    parentFolder.Items.Remove(deleteEnum);
                    refreshTree();
                }
            }
        }

    
        #endregion

        #region Reference Entry Event Handling

        private void addReference(object sender, System.EventArgs e)
		{
            if (model.FileName.Length > 0)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "NitroGen Files (*.NitroGen)|*.NitroGen";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    model.AddReference(
                        PathConverter.GetRelativePath(
                        openFileDialog1.FileName, model.FileName));

                refreshTree();
            }
		}

		private void deleteReference(object sender, System.EventArgs e)
		{
		}

		#endregion

		#region Copy/Paste Handling

		private ModelClass copyClass;

		private void miPaste_Click(object sender, System.EventArgs e)
		{
			if(copyClass != null)
			{
				model.Folders[0].Items.Add(copyClass);

				// Add class object to DataTypeManager
				DataTypeManager.ReferenceTypes.Add(new ReferenceType(copyClass, null));
				refreshTree();
			}
		}

		private void miCopy_Click(object sender, System.EventArgs e)
		{
			TreeNode n = this.modelTree.SelectedNode;
			if(n.Parent.Text == "Classes")
			{
				ModelClass c = (ModelClass) n.Tag;
                copyClass = c.Copy();
				copyClass.Name = "Copy of " + copyClass.Name;
			}
		}

		#endregion

		#region Tree Label Edit Event Handlers

		private void modelTree_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			TreeNode selectedNode;
			selectedNode = modelTree.SelectedNode;
			if(!(selectedNode.Tag is ModelFolder) & !(selectedNode.Tag is ModelClass))
			{
				e.CancelEdit = true;
			}
		}

		private void modelTree_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			e.Node.Text = e.Label;

			if(e.Node.Tag is ModelFolder)
			{
				((ModelFolder) e.Node.Tag).Name = e.Label;
			}
			else if(e.Node.Tag is ModelClass)
			{
				((ModelClass) e.Node.Tag).Name = e.Label;
			}

			refreshTree();
		}

		#endregion

		#region Drag Drop Events

		private void modelTree_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Point pt;
			TreeNode sourceNode;
			TreeNode destinationNode;
			
			sourceNode = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode", false);

			if(sourceNode != null)
			{
				pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
				destinationNode = ((TreeView)sender).GetNodeAt(pt);
				
				if(destinationNode.Tag != sourceNode.Tag)
				{
					if(destinationNode.Tag is ModelFolder && 
						sourceNode.Tag is ModelClass)
					{
						((ModelFolder) destinationNode.Tag).Items.Add(sourceNode.Tag);
						((ModelFolder) sourceNode.Parent.Tag).Items.Remove(sourceNode.Tag);
						refreshTree();
					}
					else if(destinationNode.Tag is ModelClass &&
						destinationNode.Parent.Tag is ModelFolder &&
						sourceNode.Tag is ModelClass)
					{
						((ModelFolder) destinationNode.Parent.Tag).Items.Add(sourceNode.Tag);
						((ModelFolder) sourceNode.Parent.Tag).Items.Remove(sourceNode.Tag);
						refreshTree();
					}
				}
			}
		}

		private void modelTree_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void modelTree_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			DoDragDrop(e.Item, DragDropEffects.Move);
		}

		private void modelTree_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Point pt;			
			pt = modelTree.PointToClient(new Point(e.X, e.Y));
			modelTree.SelectedNode = modelTree.GetNodeAt(pt);
		}

		#endregion

		#region Tree Expand/Collapse Event Handling

		private void modelTree_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode node = e.Node;

            if (node.Tag is ModelFolder)
            {
                node.SelectedImageIndex = 1;
                node.ImageIndex = 1;
                ((ModelFolder)e.Node.Tag).IsExpanded = true;
            }
		}

		private void modelTree_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode node = e.Node;

			if(node.Tag is ModelFolder)
			{
				node.SelectedImageIndex = 0;
				node.ImageIndex = 0;
				((ModelFolder) e.Node.Tag).IsExpanded = false;
			}
		}

		#endregion

        private void cmObjects_Popup(object sender, EventArgs e)
        {

        }

        private void modelTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is DataModel)
                e.Cancel = true;
        }

        private void ModelExplorer_Resize(object sender, EventArgs e)
        {
            
        }

	}
}