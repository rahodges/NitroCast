using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

namespace NitroCast.Core.Extensions
{
    /// <summary>
    /// Summary description for CodeGenerator.
    /// </summary>
    public class OutputExtension : Extension
    {
        private string _fileName;
        private string _customCode;
        private string _oldCode;
        private bool _readOnly;
        private bool _isWebControl;
        private bool _isWebPage;

        protected DataModel _dataModel;
        protected ModelFolder _modelFolder;
        protected ModelClass _modelClass;
        protected ModelEnum _modelEnum;
        protected OutputExtensionType _outputExtensionType;

        private Type _modelClassExtension;
        private Type _classFolderExtension;
        private Type _valueFieldExtension;
        private Type _referenceFieldExtension;
        private Type _enumFieldExtension;

        #region Public Properties

        public string FileName { get { return _fileName; } set { _fileName = value; } }
        public string CustomCode { get { return _customCode; } set { _customCode = value; } }
        public bool ReadOnly { get { return _readOnly; } set { _readOnly = value; } }
        public bool CustomCodeEnabled { get { return true; } }
        public bool IsWebControl { get { return _isWebControl; } set { _isWebControl = value; } }
        public bool IsWebPage { get { return _isWebPage; } set { _isWebPage = value; } }
        public OutputExtensionType ExtensionType { get { return _outputExtensionType; } set { _outputExtensionType = value; } }

        #endregion

        #region Extensions For Types

        public Type ModelClassExtensionType { get { return _modelClassExtension; } set { _modelClassExtension = value; } }
        public Type ClassFolderExtensionType { get { return _classFolderExtension; } set { _classFolderExtension = value; } }
        public Type ValueFieldExtensionType { get { return _valueFieldExtension; } set { _valueFieldExtension = value; } }
        public Type ReferenceFieldExtensionType { get { return _referenceFieldExtension; } set { _referenceFieldExtension = value; } }
        public Type EnumFieldExtensionType { get { return _enumFieldExtension; } set { _enumFieldExtension = value; } }

        #endregion

        public OutputExtension()
        {
            _outputExtensionType = OutputExtensionType.ModelClass;
        }

        public virtual void Init(ModelClass classObject, NameValueCollection config)
        {
            _modelClass = classObject;
        }

        #region Load

        public virtual void Load()
        {
            StringBuilder sb;
            FileInfo f;
            StreamReader sr;
            string input;
            bool captureCode = false;
            bool readOnly = false;

            //
            // If file already exists, extract custom code.
            // 
            if (File.Exists(_fileName))
            {
                sb = new StringBuilder();
                f = new FileInfo(_fileName);
                sr = f.OpenText();
                input = null;


                sr.BaseStream.Position = 0;
                _oldCode = sr.ReadToEnd();

                sr.BaseStream.Position = 0;

                while ((input = sr.ReadLine()) != null)
                {
                    if (input.IndexOf("// NitroCast MODE: READONLY") != -1)
                    {
                        sr.Close();
                        readOnly = true;
                        break;
                    }

                    if (input.IndexOf("//--- Begin Custom Code ---") != -1)
                    {
                        captureCode = true;
                        continue;
                    }

                    if (input.IndexOf("//--- End Custom Code ---") != -1)
                        break;

                    if (captureCode)
                    {
                        sb.Append(input + "\r\n");
                    }
                }

                sr.Close();

                _readOnly |= readOnly;
                _customCode = sb.ToString();
            }
            else
            {
                _customCode = string.Empty;
            }
        }

        #endregion

        #region Execute()

        public virtual void Execute()
        {
            FileInfo f;
            string directoryPath;
            DirectoryInfo directory;
            string output;
            FileStream fileStream;
            StreamWriter streamWriter;

            if (!_readOnly)
            {
                //try
                //{
                    output = Render();
                //}
                //catch (Exception e)
                //{
                //    throw (new Exception("Cannot execute plugin.", e));
                //}

                if (_oldCode != output)
                {
                    f = new FileInfo(_fileName);
                    directoryPath = f.DirectoryName;
                    directory = new DirectoryInfo(directoryPath);
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }

                    fileStream = new FileStream(_fileName, FileMode.OpenOrCreate);

                    fileStream.SetLength(0);
                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(output);
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
        }

        #endregion

        public virtual string Render()
        {
            return string.Empty;
        }
    }
}