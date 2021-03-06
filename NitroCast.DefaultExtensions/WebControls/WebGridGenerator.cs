using System;
using NitroCast.Core;
using NitroCast.Core.Extensions;

namespace NitroCast.Extensions.Default
{
	/// <summary>
	/// Summary description for WebGridGenerator.
	/// </summary>
	[ExtensionAttribute("Web Grid Class",
		 "Roy A.E. Hodges",
		 "Copyright � 2003 Roy A.E. Hodges. All Rights Reserved.",
		 "{0}Grid.cs",
		 "Default web grid for the object.",
		 "\\Default\\Object Class",true)]
	public class WebGridGenerator : OutputExtension
	{
		public WebGridGenerator()
		{
            IsWebControl = true;
            ExtensionType = OutputExtensionType.ModelClass;
            this.ValueFieldExtensionType = typeof(WebGridValueFieldExtension);
		}

		public override string Render()
		{
			CodeWriter output = new CodeWriter();
            
            output.WriteLine("/* ********************************************************** *");
            output.WriteLine(" * AMNS NitroCast v1.0 Web Grid Generator - GreyFox             *");
            output.WriteLine(" * Autogenerated by NitroCast � 2004 Roy A.E Hodges             *");
            output.WriteLine(" * All Rights Reserved                                        *");
            output.WriteLine(" * ---------------------------------------------------------- *");
            output.WriteLine(" * Source code may not be reproduced or redistributed without *");
            output.WriteLine(" * written expressed permission from the author.              *");
            output.WriteLine(" * Permission is granted to modify source code by licencee.   *");
            output.WriteLine(" * These permissions do not extend to third parties.          *");
            output.WriteLine(" * ********************************************************** */");
            output.WriteLine();

			output.WriteLine("using System;");
			output.WriteLine("using System.ComponentModel;");
			output.WriteLine("using System.Web;");
			output.WriteLine("using System.Web.UI;");
			output.WriteLine("using Amns.GreyFox.Web.UI.WebControls;");
			//
			// Add imported namespaces
			//
			int importCount = -1;
			bool addImport = true;
			string[] namespacelist = new string[_modelClass.ReferenceFields.Count];
			foreach(ReferenceField c in _modelClass.ReferenceFields)
			{
				addImport = true;

				foreach(string name in namespacelist)
					if(c.ReferenceType.NameSpace == name | c.ReferenceType.NameSpace == _modelClass.Namespace)
					{
						addImport = false;
						break;
					}

				if(addImport)
				{
					importCount++;
					namespacelist[importCount] = c.ReferenceType.NameSpace;										
				}
			}

			for(int x = 0; x <= importCount; x++)
				output.WriteLine("using {0};", namespacelist[x]);
			
			output.WriteLine();
			output.WriteLine("namespace {0}.Web.UI.WebControls", _modelClass.Namespace);
			output.WriteLine("{");
			
			output.Indent++;
			output.WriteLine("/// <summary>");
			output.WriteXmlSummary(string.Format("A custom grid for {0}.", _modelClass.Name));
			output.WriteLine("/// </summary>");
			output.Write("[ToolboxData(\"<{0}:");
			output.Write("{0}Grid runat=server>", _modelClass.Name);
			output.Write("</{0}:");
			output.WriteLine("{0}Grid>\")]", _modelClass.Name);
			
			//
			// Class Start
			//
			output.WriteLine("public class {0}Grid : TableGrid", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
		
			//
			// Private Fields
			//
			if(!_modelClass.IsTableCoded)
			{
				if(_modelClass.DefaultTableName != string.Empty)
					output.WriteLine("private string tableName = \"{0}\";", _modelClass.DefaultTableName);
				else
					output.WriteLine("private string tableName;");
			}
			output.WriteLine();

			output.WriteLine("#region Public Properties");
            
			if(!_modelClass.IsTableCoded)
			{
				output.WriteLine("[Bindable(false),");
				output.Indent++;
				output.WriteLine("Category(\"Data\"),");
				output.WriteLine("DefaultValue(\"{0}\")]", _modelClass.DefaultTableName);
				output.Indent--;
				output.WriteProperty("string", "tableName", "TableName");
				output.WriteLine();
			}

			output.WriteLine("#endregion");
			output.WriteLine();

            output.WriteLine("protected override void OnInit(EventArgs e)");
            output.WriteLine("{");
            output.Indent++;

            output.WriteLine("base.OnInit(e);");
            output.WriteLine("features = TableWindowFeatures.ClipboardCopier | ");
            output.Indent++;
            output.WriteLine("TableWindowFeatures.Scroller |");
            output.WriteLine("TableWindowFeatures.WindowPrinter |");
            output.WriteLine("TableWindowFeatures.ClientSideSelector;");
            output.Indent--;            
            
            //output.WriteLine("components = TableWindowComponents.Toolbar |");
            //output.Indent++;
            //output.WriteLine("TableWindowComponents.ViewPane;");
            //output.Indent--;

            output.Indent--;
            output.WriteLine("}");
            output.WriteLine();

			output.WriteLine("#region Rendering");
			output.WriteLine();
        
			output.WriteLine("/// <summary> ");
			output.WriteLine("/// Render this control to the output parameter specified.");
			output.WriteLine("/// </summary>");
			output.WriteLine("/// <param name=\"output\"> The HTML writer to write out to </param>");
			output.WriteLine("protected override void RenderContent(HtmlTextWriter output)");
			output.WriteLine("{");
			output.Indent++;

			if(_modelClass.IsTableCoded)
				output.WriteLine("{0}Manager m = new {0}Manager();", _modelClass.Name);
			else
				output.WriteLine("{0}Manager m = new {0}Manager(tableName);", _modelClass.Name);

			if(_modelClass.ReferenceFields.Count > 0)
				output.WriteLine("{0}Collection {1}Collection = m.GetCollection(string.Empty, string.Empty, null);",
					_modelClass.Name, _modelClass.PrivateName);
			else
				output.WriteLine("{0}Collection {1}Collection = m.GetCollection(string.Empty, string.Empty);",
					_modelClass.Name, _modelClass.PrivateName);

            #region Header Locked Row

            bool commaEnabled = false;
            string headerRow = "";               // <<----- REPLACE WITH STRINGBUILDER

            foreach (ClassFolder folder in _modelClass.Folders)
            {
                foreach (object i in folder.Items)
                {
                    if (i is ValueField)
                    {
                        ValueField f = (ValueField)i;

                        WebGridValueFieldExtension e = (WebGridValueFieldExtension)
                            f.GetExtension(typeof(WebGridValueFieldExtension));

                        if (e.GridEnabled)
                        {
                            if (commaEnabled) headerRow += ", ";
                            headerRow += "\"" + (f.Caption.Length > 0 ? f.Caption : f.Name) + "\"";
                            commaEnabled = true;
                        }
                    }
                    else if (i is ReferenceField)
                    {
                        ReferenceField f = (ReferenceField)i;
                        WebGridReferenceFieldExtension e = (WebGridReferenceFieldExtension)
                            f.GetExtension(typeof(WebGridReferenceFieldExtension));
                        if (e.GridEnabled)
                        {
                            if (commaEnabled) headerRow += ", ";
                            headerRow += "\"" + (f.Caption.Length > 0 ? f.Caption : f.Name) + "\"";
                            commaEnabled = true;
                        }
                    }
                    else if (i is EnumField)
                    {
                        EnumField f = (EnumField)i;
                        WebGridEnumFieldExtension e = (WebGridEnumFieldExtension)
                            f.GetExtension(typeof(WebGridEnumFieldExtension));
                        if (e.GridEnabled)
                        {
                            if (commaEnabled) headerRow += ", ";
                            headerRow += "\"" + (f.Caption.Length > 0 ? f.Caption : f.Name) + "\"";
                            commaEnabled = true;
                        }
                    }
                }
            }

            if (headerRow.Length > 0)
            {
                output.WriteLine("// Render Header Row");
                output.WriteLine("this.headerLockEnabled = true;");
                output.Write("RenderRow(this.HeaderRowCssClass, ");
                output.Write(headerRow);
                output.WriteLine(");");
                output.WriteLine();
            }

            #endregion

            output.WriteLine("bool rowflag = false;");
			output.WriteLine("string rowCssClass;");

			output.WriteLine("//");
			output.WriteLine("// Render Records");
			output.WriteLine("//");
			output.WriteLine("foreach({0} {1} in {1}Collection)", _modelClass.Name, _modelClass.PrivateName);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("if(rowflag) rowCssClass = defaultRowCssClass;");
			output.WriteLine("else rowCssClass = alternateRowCssClass;");

			output.WriteLine("rowflag = !rowflag;");
			output.WriteLine("output.WriteBeginTag(\"tr\");");
			output.WriteLine("output.WriteAttribute(\"i\", {0}.ID.ToString());", _modelClass.PrivateName);
			output.WriteLine("output.WriteLine(HtmlTextWriter.TagRightChar);");
			output.WriteLine("output.Indent++;");
            output.WriteLine();

            foreach (ClassFolder folder in _modelClass.Folders)
            {
                foreach (object i in folder.Items)
                {
                    if (i is ValueField)
                    {
                        ValueField f = (ValueField)i;
                        WebGridValueFieldExtension e = (WebGridValueFieldExtension)
                                f.GetExtension(typeof(WebGridValueFieldExtension));
                        if(e.GridEnabled)
                        {
                            output.WriteLine("//");
                            output.WriteLine("// Render Main Representation of Record");
                            output.WriteLine("//");
                            output.WriteLine("output.WriteBeginTag(\"td\");");
                            output.WriteLine("output.WriteAttribute(\"valign\", \"top\");");
                            output.WriteLine("output.WriteAttribute(\"class\", rowCssClass);");
                            output.WriteLine("output.Write(HtmlTextWriter.TagRightChar);");                            
                            if(e.GridFormat.Length > 0)
                                output.WriteLine("output.Write({0}.{1}.ToString(\"{2}\"));", _modelClass.PrivateName, f.Name, e.GridFormat);
                            else
                                output.WriteLine("output.Write({0}.{1});", _modelClass.PrivateName, f.Name);
                            output.WriteLine("output.WriteEndTag(\"td\");");
                            output.WriteLine("output.WriteLine();");
                            output.WriteLine();
                        }
                    }
                    else if (i is ReferenceField)
                    {
                        ReferenceField c = (ReferenceField)i;
                        WebGridReferenceFieldExtension e = (WebGridReferenceFieldExtension)
                            c.GetExtension(typeof(WebGridReferenceFieldExtension));
                        if (e.GridEnabled)
                        {
                            output.WriteLine("//");
                            output.WriteLine("// Render Main Representation of Record");
                            output.WriteLine("//");
                            output.WriteLine("output.WriteBeginTag(\"td\");");
                            output.WriteLine("output.WriteAttribute(\"valign\", \"top\");");
                            output.WriteLine("output.WriteAttribute(\"class\", rowCssClass);");
                            output.WriteLine("output.Write(HtmlTextWriter.TagRightChar);");
                            output.WriteLine("if({0}.{1} != null)", _modelClass.PrivateName, c.Name);
                            output.WriteLine("{");
                            output.Indent++;
                            if (e.GridFormat.Length > 0)
                                output.WriteLine("output.Write({0}.{1}.ToString(\"{2}\"));", _modelClass.PrivateName, c.Name, e.GridFormat);
                            else
                                output.WriteLine("output.Write({0}.{1});", _modelClass.PrivateName, c.Name);
                            output.Indent--;
                            output.WriteLine("}");
                            output.WriteLine("else");
                            output.WriteLine("{");
                            output.Indent++;
                            output.WriteLine("output.Write(\"null\");");
                            output.Indent--;
                            output.WriteLine("}");
                            output.WriteLine("output.WriteEndTag(\"td\");");
                            output.WriteLine("output.WriteLine();");
                            output.WriteLine();
                        }
                    }
                    else if (i is EnumField)
                    {
                        EnumField f = (EnumField)i;
                        WebGridEnumFieldExtension e = (WebGridEnumFieldExtension)
                            f.GetExtension(typeof(WebGridEnumFieldExtension));
                        if (e.GridEnabled)
                        {
                            output.WriteLine("//");
                            output.WriteLine("// Render Main Representation of Record");
                            output.WriteLine("//");
                            output.WriteLine("output.WriteBeginTag(\"td\");");
                            output.WriteLine("output.WriteAttribute(\"valign\", \"top\");");
                            output.WriteLine("output.WriteAttribute(\"class\", rowCssClass);");
                            output.WriteLine("output.Write(HtmlTextWriter.TagRightChar);");
                            output.WriteLine("output.Write({0}.{1}.ToString());", _modelClass.PrivateName, f.Name);
                            output.WriteLine("output.WriteEndTag(\"td\");");
                            output.WriteLine("output.WriteLine();");
                            output.WriteLine();
                        }
                    }
                }
            }			

			output.WriteLine("output.Indent--;");
			output.WriteLine("output.WriteEndTag(\"tr\");");
			output.WriteLine("output.WriteLine();");
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			//
			// Class End
			//
			output.Indent--;
			output.WriteLine("}");
			
			//
			// Namespace End
			//
			output.Indent--;
			output.WriteLine("}");

			return output.ToString();
		}
	}
}
