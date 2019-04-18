using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using NitroCast.Core.Extensions;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for NitroCast.
	/// </summary>
	public class DataModel
	{
		string name = string.Empty;
		string defaultNamespace = string.Empty;
        bool connectionStringIsCoded = false;
        bool connectionStringIsConfigKey = false;
        string connectionString;
		string fileName = string.Empty;	
		string description = string.Empty;
        DataModelVersion version = new DataModelVersion();
		DataTable plugins;

        ModelFolderCollection folders = new ModelFolderCollection();
        ReferenceEntryCollection references = new ReferenceEntryCollection();

        public object CopyObject;
		object editor;

		#region Public Properties

		public string DefaultNamespace
		{
			get
			{
				return defaultNamespace;
			}
			set
			{
				defaultNamespace = value;
				OnChanged(EventArgs.Empty);
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public bool ConnectionStringIsCoded
        {
            get { return connectionStringIsCoded; }
            set { connectionStringIsCoded = value; }
        }

        public bool ConnectionStringIsConfigKey
        {
            get { return connectionStringIsConfigKey; }
            set { connectionStringIsConfigKey = value; }
        }

		public string FileName
		{
			get
			{
				return fileName;
			}
		}

        public ModelFolderCollection Folders
        {
            get
            {
                return folders;
            }
        }

		public ReferenceEntryCollection References
		{
			get
			{
				return references;
			}
		}

		public DataTable Plugins
		{
			get
			{
				return plugins;
			}
			set
			{
				plugins = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnChanged(EventArgs.Empty);
			}
		}		

		/// <summary>
		/// Gets and sets the Class Entry's associated editor.
		/// </summary>
		public object Editor
		{
			get
			{
				return editor;
			}
			set
			{
				editor = value;
			}
		}

		#endregion
		
		public DataModel()
		{
			
		}

        private void checkReferences()
        {
            foreach (ReferenceEntry r in references)
                if (!File.Exists(r.FileName))
                    throw new System.IO.FileNotFoundException("File does not exist.", fileName);
        }

		public void Close()
		{
		}

		#region Events

        private void model_Changed(object sender, EventArgs e)
        {
            OnChanged(e);
        }

		public event EventHandler Changed;
		protected virtual void OnChanged(EventArgs e)
		{
			if(Changed != null)
				Changed(this, e);
		}

		public event DataModelEventHandler ProgressStart;
		protected virtual void OnProgressStart(object sender, DataModelEventArgs e)
		{
			if(ProgressStart != null)
				ProgressStart(this, e);
		}

		public event DataModelEventHandler ProgressUpdate;
		protected virtual void OnProgressUpdate(object sender, DataModelEventArgs e)
		{
			if(ProgressUpdate != null)
				ProgressUpdate(this, e);
		}

		public event DataModelEventHandler ProgressStop;
		protected virtual void OnProgressStop(object sender, DataModelEventArgs e)
		{
			if(ProgressStop != null)
				ProgressStop(this, e);
		}

        public event EventHandler SaveError;
        protected virtual void OnSaveError(object sender, EventArgs e)
        {
            if (SaveError != null)
                SaveError(this, e);
        }

		#endregion

		#region Class Methods

		public ModelClass NewClass()
		{
			ModelClass newClass = new ModelClass();
			newClass.Updated += new ModelClassEventHandler(model_Changed);
			folders[0].Items.Add(newClass);
			return newClass;
		}

		#endregion

		#region Add/Remove Reference Methods

		public void AddReference(string fileName)
		{
			ReferenceEntry r = new ReferenceEntry(fileName);
			references.Add(r);

			DataModel m = new DataModel();
			m.Load(fileName);

			foreach(ModelFolder folder in m.Folders)
			{
				foreach(object i in folder.Items)
				{
					if(i is ModelClass)
					{
						DataTypeManager.AddDataType(new ReferenceType((ModelClass) i, r));
					}
				}
			}				
		}

		public void RemoveReference(string fileName)
		{
			int i = 0;

			// Remove from reference list
			while(i < references.Count)
			{			
				if(references[i].FileName == fileName)
				{
					references.RemoveAt(i);
				}

				i++;
			}

			// Remove datatypes associated with the reference
			i = 0;
			while(i < DataTypeManager.ReferenceTypes.Count)
			{		
				// If a datatype is found with the association,
				// remove the datatype and continue the search.
				// Be sure NOT to increment, because the removal
				// will already 'pull in' the next value.
				if(DataTypeManager.ReferenceTypes[i].SourceFileName 
					== fileName)
				{
					DataTypeManager.ReferenceTypes.RemoveAt(i);
					continue;
				}

				i++;
			}
		}		

		#endregion

		#region Statistics

		public int CalcClassCount()
		{
			int count = 0;
			
			foreach(ModelFolder f in folders)
			{
				foreach(object item in f.Items)
				{
					if(item is ModelClass)
					{
						count++;
					}
				}
			}
			
			return count;
		}

		#endregion

		#region Load Methods

		public void Load(string fileName)
		{
			ModelClass classEntry;
			ReferenceField childEntry;
            EnumField enumEntry;

            FileInfo file = new FileInfo(fileName);

			// Progress Start Event
			this.OnProgressStart(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadText, name),
                string.Empty,
				"LOAD", new ProgressBarConfig(0, 5, 0, 1)));

			//
			// Test for file existence.
			//
			if(!File.Exists(fileName))
			{
                // Try Loading the NitroCast File
                fileName = fileName.Replace(".DbModel", ".NitroGen");

                if (!File.Exists(fileName))
                {
                    this.OnProgressStop(this, new DataModelEventArgs(
                         string.Format(Properties.Resources.ProgressBarLoadFailed, file.Name),
                         string.Empty, "LOAD"));
                    throw new System.IO.FileNotFoundException(
                        string.Format(Properties.Resources.ProgressBarLoadFailed, file.Name));
                }
			}

			this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadInitialize, file.Name),
                string.Empty, "LOAD"));

			#region Clear References and Classes

			references.Clear();
			folders.Clear();

			#endregion

			this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadParse, file.Name),
                string.Empty, "LOAD"));

			#region Parse Model

			this.fileName = fileName;

			XmlTextReader r = new XmlTextReader(fileName);
			r.WhitespaceHandling = WhitespaceHandling.None;

			r.MoveToContent();                       
            
            if(!(r.Name == "dbModel" | r.Name == "NitroCast"))
                throw new Exception("Source file does not match NitroCast DTD.");

			r.MoveToAttribute("version");
			string version = r.Value;
			r.MoveToContent();		

			switch(version)
			{
//				case "1.0":
//					r.Read();
//					parse1_0File(r);
//					break;
//				case "1.1":
//					r.Read();
//					parse1_1File(r);
//					break;
				case "1.11":
					r.Read();
					parse1_11File(r);
					break;
				default:
					r.Close();
					throw new Exception(string.Format("Source file version '{0}' incompatible.", version));
			}
			r.Close();

			#endregion

			this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadReferences, file.Name),
                string.Empty, "LOAD"));

			#region Load Referenced Data Types

			//
			// Create DataTypes For References Class Entries
			//
			foreach(ReferenceEntry reference in references)
			{
				DataModel m = new DataModel();

                m.Load(PathConverter.GetAbsolutePath(file.DirectoryName, reference.FileName));
                reference.Name = m.Name;
                
                foreach(ModelFolder folder in m.Folders)
				{
					foreach(object item in folder.Items)
					{
						if(item is ModelClass)
						{
							DataTypeManager.AddDataType(new ReferenceType((ModelClass)item, reference));
						}
                        else if (item is ModelEnum)
                        {
                            DataTypeManager.AddDataType(new EnumType((ModelEnum)item, reference));
                        }
					}
				}					
			}

			#endregion

			this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadBuild, file.Name),
                string.Empty, "LOAD"));

			#region Load Internal Data Types

			foreach(ModelFolder f in folders)
			{
				foreach(object item in f.Items)
				{
					if(item is ModelClass)
					{
                        DataTypeManager.AddDataType(new ReferenceType((ModelClass)item, null));
					}
                    else if (item is ModelEnum)
                    {
                        DataTypeManager.AddDataType(new EnumType((ModelEnum)item, null));
                    }
				}
			}				

			#endregion

			this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadSort, file.Name),
                string.Empty, "LOAD"));
			
			DataTypeManager.ReferenceTypes.Sort(ModelEntryCompareKey.Name);
			DataTypeManager.ValueTypes.Sort(ModelEntryCompareKey.Name);
            DataTypeManager.EnumTypes.Sort(ModelEntryCompareKey.Name);

			this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarLoadAssociate, file.Name),
                string.Empty, "LOAD"));

			#region Associate Datatypes to Children
			
			//
			// Associate DataTypes to class children.
			//
			foreach(ModelFolder folder in folders)
			{
				foreach(object item in folder.Items)
				{
					if(item is ModelClass)
					{
						classEntry = (ModelClass) item;

						foreach(ClassFolder classFolder in classEntry.Folders)
						{
							foreach(object folderItem in classFolder.Items)
							{
								if(folderItem is ReferenceField)
								{
									childEntry = (ReferenceField) folderItem;

									if(childEntry.ReferenceType.IsCustom)
									{
										foreach(ReferenceType cType in DataTypeManager.ReferenceTypes)
										{
                                            if (childEntry.ReferenceType.Name == cType.Name &
                                                childEntry.ReferenceType.NameSpace == cType.NameSpace)
                                            {
                                                childEntry.ReferenceType = cType;
                                            }
										}
									}
								}
                                else if (folderItem is EnumField)
                                {
                                    enumEntry = (EnumField)folderItem;

                                    if (enumEntry.EnumType.IsCustom)
                                    {
                                        foreach (EnumType cType in DataTypeManager.EnumTypes)
                                        {
                                            if (enumEntry.EnumType.Name == cType.Name &
                                                enumEntry.EnumType.NameSpace == cType.NameSpace)
                                            {
                                                enumEntry.EnumType = cType;
                                            }
                                        }
                                    }
                                }
							}
						}
					}
				}
			}

			#endregion

			this.OnProgressStop(this, new DataModelEventArgs(string.Empty, string.Empty, "LOAD"));
		}

		#region Parse 1.11 Model File

		private void parse1_11File(XmlTextReader r)
		{
			string fileVersion;
			ModelFolder defaultFolder = null;

			fileVersion = "1.11";

			name = r.ReadElementString("Name");
			defaultNamespace = r.ReadElementString("DefaultNamespace");
            if (r.LocalName == "Description")
            {
                description = r.ReadElementString("Description");
            }
            if (r.LocalName == "ConnectionString")
            {
                connectionString = r.ReadElementString("ConnectionString");
            }
            if (r.LocalName == "ConnectionStringIsCoded")
            {
                connectionStringIsCoded = bool.Parse(r.ReadElementString("ConnectionStringIsCoded"));
            }
            if (r.LocalName == "ConnectionStringIsConfigKey")
            {
                connectionStringIsConfigKey = bool.Parse(r.ReadElementString("ConnectionStringIsConfigKey"));
            }
            if (r.LocalName == "VersionMajor")
            {
                version.Major = int.Parse(r.ReadElementString("VersionMajor"));
            }
            if (r.LocalName == "VersionMinor")
            {
                version.Minor = int.Parse(r.ReadElementString("VersionMinor"));
            }
            if (r.LocalName == "VersionBuild")
            {
                version.Build = int.Parse(r.ReadElementString("VersionBuild"));
            }

			if(r.Name == "References" && !r.IsEmptyElement)
			{
				r.Read();

				while(r.LocalName == "ReferenceEntry")
				{
					references.Add(new ReferenceEntry(r));
				}

				r.ReadEndElement();
			}
			else
			{
				r.Read();
			}

			//
			// Skip Field Data Types on 1.1 models
			//
			while(r.Name != "ChildDataTypes")
			{
				r.Read();
			}
            			
			if(r.Name == "ChildDataTypes" && !r.IsEmptyElement)
			{
				r.Read();
				while(r.LocalName == "ChildDataType")
                    DataTypeManager.AddDataType(new ReferenceType(r));
				r.ReadEndElement();
			}
			else
				r.Read();

			if((r.Name == "ModelFolders" | r.Name == "ClassObjects") && !r.IsEmptyElement)
			{
				r.Read();
				while(r.LocalName == "ClassObject" | r.LocalName == "ModelFolder")
				{
					if(r.LocalName == "ClassObject")
					{
						// Create a default folder for the class object if it
						// is not already created.
						if(defaultFolder == null)
						{
							defaultFolder = new ModelFolder();
							defaultFolder.Caption = "General";
							defaultFolder.Description = "General";
							defaultFolder.Name = "General";
							defaultFolder.ParentModel = this;
							defaultFolder.IsBrowsable = true;
							defaultFolder.IsExpanded = true;
							this.Folders.Add(defaultFolder);
						}

						// Load ClassEntry
						ModelClass c = new ModelClass(r, fileVersion);
						c.ParentModel = this;

						// Add Class Entry to Default Folder
						defaultFolder.Items.Add(c);
					}

					// Add Model Folder
					if(r.LocalName == "ModelFolder")
					{
						ModelFolder f = new ModelFolder(r, this, fileVersion);
						folders.Add(f);
					}
				}
				r.ReadEndElement();
			}
			else
				r.Read();
		}

		#endregion

		#endregion

		#region Save Methods

		public void Save()
		{
			if(fileName == string.Empty)
				throw new Exception("Filename is empty.");

			SaveAs(fileName);
		}

		public void SaveAs(string fileName)
        {
			// Calculate Progress Steps
			int steps;
            XmlTextWriter w;

            w = null;

			steps = references.Count + 
				DataTypeManager.ValueTypes.Count + 
				DataTypeManager.ReferenceTypes.Count +
				folders.Count;

			// Progress Start Event
			this.OnProgressStart(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarSaveText, name),
                string.Empty,
                "SAVE", new ProgressBarConfig(0, 5, 0, 1)));

			this.fileName = fileName;

            try
            {
                w = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            }
            catch
            {
                OnSaveError(this, EventArgs.Empty);
                this.OnProgressStop(this, new DataModelEventArgs(
                    string.Format(Properties.Resources.ProgressBarSaveFailed, name),
                    string.Empty, "SAVE"));
                return;
            }

			w.Formatting = Formatting.Indented;

			w.WriteStartDocument();
			w.WriteStartElement("NitroCast");
			w.WriteAttributeString("version", "1.11");

			w.WriteElementString("Name", name);
			w.WriteElementString("DefaultNamespace", defaultNamespace);
			w.WriteElementString("Description", description);
            w.WriteElementString("ConnectionString", connectionString);
            w.WriteElementString("ConnectionStringIsCoded", connectionStringIsCoded.ToString());
            w.WriteElementString("ConnectionStringIsConfigKey", connectionStringIsConfigKey.ToString());
            w.WriteElementString("VersionMajor", version.Major.ToString());
            w.WriteElementString("VersionMinor", version.Minor.ToString());
            w.WriteElementString("VersionBuild", version.Build.ToString());

            this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarSaveReferences, name),
                string.Empty, "SAVE"));
            w.WriteStartElement("References");
			foreach(ReferenceEntry r in references)
			{				
				r.WriteXml(w);
			}
			w.WriteEndElement();


            // UNUSED ===========================================================
			w.WriteStartElement("FieldDataTypes");
			foreach(ValueType fType in DataTypeManager.ValueTypes)
			{				
				fType.WriteXml(w);
			}
			w.WriteEndElement();

            // UNUSED ===========================================================
			w.WriteStartElement("ChildDataTypes");
			foreach(ReferenceType cType in DataTypeManager.ReferenceTypes)
			{				
				if(cType.IsFromReference | cType.IsInternal)
					continue;
				cType.WriteXml(w);
			}
			w.WriteEndElement();
            
			w.WriteStartElement("ModelFolders");
			foreach(ModelFolder folder in folders)			
            {                
                this.OnProgressUpdate(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarSaveFolders, folder),
                string.Empty, "SAVE"));
				folder.WriteXml(w);				
			}
			w.WriteEndElement();

			w.WriteEndElement();

			w.WriteEndDocument();
			w.Flush();
			w.Close();

			this.OnProgressStop(this, new DataModelEventArgs(string.Empty, string.Empty, "SAVE"));
        }
        
        #endregion

        #region Export Methods

        public void Export()
        {
            int modelClassExtensions;
            int modelEnumExtensions;
            int maxCount;
            System.IO.FileInfo modelFile;
            ExtensionManager pluginManager;
            ModelClass classEntry;

            modelFile = new System.IO.FileInfo(fileName);
            pluginManager = ExtensionManager.GetInstance();

            modelClassExtensions = 0;
            modelEnumExtensions = 0;
            foreach (OutputExtension extension in pluginManager.OutputExtensions.Values)
            {
                if (extension.ExtensionType == OutputExtensionType.ModelClass)
                {
                    modelClassExtensions++;
                }
                else if (extension.ExtensionType == OutputExtensionType.ModelEnum)
                {
                    modelEnumExtensions++;
                }
            }

            maxCount = 0;
            foreach (ModelFolder folder in folders)
            {
                foreach (object item in folder.Items)
                {
                    if (item is ModelClass)
                    {
                        maxCount += modelClassExtensions;
                    }
                    else if (item is ModelEnum)
                    {
                        maxCount += modelEnumExtensions;
                    }
                }
            }

            this.OnProgressStart(this, new DataModelEventArgs(
                string.Format(Properties.Resources.ProgressBarExportText, name),
                string.Empty, "EXPORT",
                new ProgressBarConfig(0, maxCount, 0, 1)));

            foreach (ModelFolder folder in folders)
            {
                foreach (object i in folder.Items)
                {
                    if (i is ModelClass)
                    {
                        classEntry = (ModelClass)i;

                        foreach (OutputExtension extension in pluginManager.OutputExtensions.Values)
                        {
                            extension.Init(classEntry, null);

                            if (extension.IsWebControl & classEntry.WebOutputPath != string.Empty)
                            {
                                extension.FileName = PathConverter.GetAbsolutePath(modelFile.DirectoryName,
                                    classEntry.WebOutputPath) + "/" +
                                    string.Format(extension.OutputFileNameFormat, classEntry.Name);
                            }
                            else
                            {
                                extension.FileName = PathConverter.GetAbsolutePath(modelFile.DirectoryName,
                                    classEntry.OutputPath) + "/" +
                                    string.Format(extension.OutputFileNameFormat, classEntry.Name);
                            }
                            
                            this.OnProgressUpdate(this, new DataModelEventArgs(
                                string.Format(Properties.Resources.ProgressBarExportClass, 
                                name, folder, classEntry.Name, extension.Name),
                                string.Empty, "EXPORT"));

                            extension.Load();
                            extension.Execute();
                        }
                    }
                }
            }

            // This is here to allow enough time to display the completed
            // progress bar, otherwise it will always look like the last
            // few steps are incomplete.
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(100));

            this.OnProgressStop(this, new DataModelEventArgs(
                Properties.Resources.ProgressBarExportComplete, 
                string.Empty, "EXPORT"));
        }

        public void Compile()
        {
            Compiler c = new Compiler();
            c.Compile(this);
        }

        public string[] ExportSourceCode()
        {
            ArrayList sources;            
            ExtensionManager pluginManager;
            ModelClass classEntry;
            string[] sourceStrings;

            pluginManager = ExtensionManager.GetInstance();

            sources = new ArrayList();

            foreach (ModelFolder folder in folders)
            {
                foreach (object i in folder.Items)
                {
                    if (i is ModelClass)
                    {
                        classEntry = (ModelClass)i;

                        foreach (OutputExtension extension in pluginManager.OutputExtensions.Values)
                        {
                            if (!extension.IsWebControl & !extension.IsWebPage)
                            {
                                extension.Init(classEntry, null);
                                extension.Load();
                                sources.Add(extension.Render());
                            }
                        }
                    }
                }
            }

            sourceStrings = new string[sources.Count];
            for(int i = 0; i <sources.Count; i++)                
            {
                sourceStrings[i] = (string) sources[i];
            }

            return sourceStrings;
        }

        #endregion
    }
}