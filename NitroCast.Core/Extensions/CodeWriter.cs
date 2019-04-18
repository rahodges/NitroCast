using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace NitroCast.Core.Extensions
{
    public enum LineDependency { Children, NoChildren, CodedTable, NonCodedTable }

	/// <summary>
	/// Summary description for CodeWriter.
	/// </summary>
	public class CodeWriter
	{
		int indent = 0;
		int indentWidth = 4;
		int colwidth = 80;	
		bool newLine = false;
        //bool indentDetect = false;		// detects an open curly brace and indents the next line;
        //bool outdentDetect = false;		// detects an outdent and outdents the next line;
		StringBuilder s;
		DataModel currentModel;
		ModelClass currentClass;
		ValueField currentField;
		ReferenceField currentChild;
        bool commaLine = false;
                        
        Dictionary<string, string> referenceTrackingList;

		#region Properties

		public DataModel CurrentModel
		{
			get
			{
				return currentModel;
			}
			set
			{
				currentModel = value;
			}
		}

		public ModelClass CurrentClass
		{
			get
			{
				return currentClass;
			}
			set
			{
				currentClass = value;
			}
		}

		public ReferenceField CurrentChild
		{
			get
			{
				return currentChild;
			}
			set
			{
				currentChild = value;
			}
		}

		public ValueField CurrentField
		{
			get
			{
				return currentField;
			}
			set
			{
				currentField = value;
			}
		}

		public int Indent
		{
			get
			{
				return indent;
			}
			set
			{
                //indentDetect = false;
                //outdentDetect = false;
				indent = value;
			}
		}

        public int ColumnWidth
        {
			get
			{
				return colwidth;
			}
			set
			{
				colwidth = value;
			}
		}

		#endregion

		public CodeWriter()
		{
			s = new StringBuilder();
            referenceTrackingList = new Dictionary<string, string>();
        }

        #region Tracking Lists

        public void SaveReference(ReferenceField field, string value)
        {
            referenceTrackingList.Add(
                field.ReferenceType.NameSpace + "." + 
                field.ReferenceType.Name + 
                field.TableName, value);
        }

        public void ClearReference(ReferenceField field, string value)
        {
            referenceTrackingList.Remove(
                field.ReferenceType.NameSpace + "." + 
                field.ReferenceType.Name +
                field.TableName);
        }

        public bool ReferenceUsed(ReferenceField field)
        {
            return referenceTrackingList.ContainsKey(
                field.ReferenceType.NameSpace+ "." + 
                field.ReferenceType.Name +
                field.TableName);
        }
        
        public string GetReference(ReferenceField field)
        {
            return referenceTrackingList[
                field.ReferenceType.NameSpace+ "." + 
                field.ReferenceType.Name +
                field.TableName];
        }

        #endregion
        
        #region Write Methods

        private bool separationFlag;			// true if seperation writer is on
		private bool separationIndentFlag;		// true if separation writer auto indented the lines
		private string leftSeparator;
		private string rightSeparator;

		public void StartSeparation()
		{
			separationFlag = true;
		}
		
		public void WriteSeparationStart(string startSeparator)
		{
			separationFlag = true;
			WriteLine(startSeparator);
			indent++;
			separationIndentFlag = true;
		}

        public void WriteSeparationStart(string startSeparator, params string[] args)
        {
            WriteSeparationStart(string.Format(startSeparator, args));
        }

		public void WriteSeparationReset(string text, string nextLeftSeparator, string nextRightSeparator)
		{
			separationFlag = false;
			WriteSeparation(text, nextLeftSeparator, nextRightSeparator);
		}

		public void WriteSeparation(string text)
		{
			if(separationFlag)
			{
                if(!newLine)
				    WriteLine(rightSeparator);
				Write(leftSeparator);
				Write(text);
			}
			else
			{
                if (newLine)
                {
                    indent++;
                    Write(leftSeparator);
                    Write(text);
                }
                else
                {
                    Write(text);
                    if (!separationIndentFlag)
                        indent++;
                }
				separationFlag = true;
				separationIndentFlag = true;
			}
		}

		public void WriteSeparation(string text, string leftSeparator, string rightSeparator)
		{
            this.leftSeparator = leftSeparator;            
			WriteSeparation(text);
            this.rightSeparator = rightSeparator;
		}

		public void WriteSeparationEnd()
		{
			separationFlag = false;
			if(separationIndentFlag)
				indent--;
			separationIndentFlag = false;
		}

		public void WriteSeparationEnd(string endSeparator)
		{
			if(separationFlag)
				WriteLine(endSeparator);

			separationFlag = false;			

			if(separationIndentFlag)
				indent--;
			
			separationIndentFlag = false;
		}

		public void Write(string text)
		{
			if(text == null)
				return;

//			if(!text.EndsWith(";"))
//			{
//
//				if(text.StartsWith("}") & outdentDetect)
//				{
//					indent--;
//					outdentDetect = false;
//				}
//
//				if(indentDetect & !text.StartsWith("{")) 
//				{
//					indent++;
//					indentDetect = false;
//				}
//
//				indentDetect = text.StartsWith("{");
//				//			| 
//				//				text.StartsWith("if") | 
//				//				text.StartsWith("for") | 
//				//				text.StartsWith("while");
//
//				if(!text.StartsWith("}"))
//				{
//					outdentDetect = true;
//				}
//			}

			if(s.Length != 0 && newLine)
				for(int x = 0; x < indent; x++)
					s.Append("\t");
			s.Append(parse(text));
			if(text.Length > 0)
				newLine = text.EndsWith("\r\n");
		}

		public void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		public void WriteDirect(string text)
		{
			s.Append(text);
		}

		public void WriteLine()
		{
			s.Append("\r\n");
			newLine = true;
		}

		public void WriteLine(string text)
		{
			Write(text);
			s.Append("\r\n");
			newLine = true;
		}

        /// <summary>
        /// Writes a line by suffixing the previous line with a trailing comma
        /// if that last line was a comma line as well.
        /// </summary>
        /// <param name="text"></param>
        public void WriteCommaLine(string text)
        {
            if (!newLine & commaLine)
            {
                s.Append(",\r\n");
            }
            if (commaLine)
            {
                for (int x = 0; x < indent; x++)
                    s.Append("\t");
            }
            Write(text);
            commaLine = true;
        }

        public void WriteCommaLine(string format, params string[] args)
        {
            WriteCommaLine(string.Format(format, args));
        }

        public void ResetCommaLine()
        {
            commaLine = false;
        }

        public void WriteLine(LineDependency dependency, string text)
        {
            switch (dependency)
            {
                case LineDependency.Children:
                    if (currentClass.ReferenceFields.Count > 0)
                    {
                        WriteLine(text);
                    }
                    break;
                case LineDependency.NoChildren:
                    if (currentClass.ReferenceFields.Count == 0)
                    {
                        WriteLine(text);
                    }
                    break;
                case LineDependency.CodedTable:
                    if (currentClass.IsTableCoded)
                    {
                        WriteLine(text);
                    }
                    break;
                case LineDependency.NonCodedTable:
                    if (!currentClass.IsTableCoded)
                    {
                        WriteLine(text);
                    }
                    break;
            }
        }

		public void WriteLine(string format, params object[] args)
		{
			Write(string.Format(format, args));
			s.Append("\r\n");
			newLine = true;
		}

        public void WriteLine(LineDependency dependency, string format, params object[] args)
        {
            switch (dependency)
            {
                case LineDependency.Children:
                    if (currentClass.ReferenceFields.Count > 0)
                    {
                        WriteLine(format, args);
                    }
                    break;
                case LineDependency.NoChildren:
                    if (currentClass.ReferenceFields.Count == 0)
                    {
                        WriteLine(format, args);
                    }
                    break;
                case LineDependency.CodedTable:
                    if (currentClass.IsTableCoded)
                    {
                        WriteLine(format, args);
                    }
                    break;
                case LineDependency.NonCodedTable:
                    if (!currentClass.IsTableCoded)
                    {
                        WriteLine(format, args);
                    }
                    break;
            }
        }

		public void WriteIndentedLine()
		{
			indent++;
			WriteLine();
		}

		public void WriteIndentedLine(string text)
		{
			indent++;
			WriteLine(text);
		}

		public void WriteIndentedLine(string format, params object[] args)
		{
			indent++;
			WriteLine(format, args);
		}

		public void WriteOutdentedLine(string text)
		{
			indent--;
			WriteLine(text);
		}

		public void WriteOutdentedLine(string format, params object[] args)
		{
			indent--;
			WriteLine(format, args);
		}

		public void WriteXmlSummary(string format, params object[] args)
		{
			WriteXmlSummary(string.Format(format, args));
		}

		public void WriteXmlSummary(string summary)
		{	
			if(summary == null)
				return;

			StringBuilder x = new StringBuilder();
			int colIndex = 0;
			bool lineBreak;

			WriteLine("/// <summary>");

			foreach(char c in summary.ToCharArray())
			{
				colIndex++;
				lineBreak = (colIndex > (colwidth - 4 - (indent * indentWidth)) & c == ' ') |
					c == '\n';
				x.Append(c);

				if(lineBreak)
				{					
					WriteLine("/// {0}", x);
					colIndex = 0;
					x.Length = 0;
				}				
			}

			if(x.Length > 0)
				WriteLine("/// {0}", x);

			WriteLine("/// </summary>");				
		}

		public void WriteRegionBegin(string regionSummary)
		{
			Write("#region ");
			WriteLine(regionSummary);
			WriteLine();
		}

		public void WriteRegionEnd()
		{
			WriteLine("#endregion");
			WriteLine();
		}

		#endregion

		#region SuperParse Methods

		private string parse(string s)
		{
			char[] chars = s.ToCharArray();
			int u = chars.GetUpperBound(0);
			int index = 0;
			char c;
			bool captureStart = false;
			bool captureEnabled = false;
			StringBuilder command = new StringBuilder();
			StringBuilder output = new StringBuilder();

			while(index <= u)
			{
				c = chars[index];
				index++;
				switch(c)
				{
					case '<':
						captureStart = true;
						break;
					case '.':
						if(captureStart)
							captureEnabled = true;
						else
							output.Append('.');
						break;
					case '>':

						// Parse command.
						if(captureEnabled)
						{
							switch(command.ToString())
							{
								case "c":
									output.Append(currentClass.PrivateName);
									break;
								case "C":
									output.Append(currentClass.Name);
									break;
							}
							command.Length = 0;
							captureEnabled = false;
							captureStart = false;
						}
						else
						{
							output.Append('>');
						}						
						break;
					default:
						if(captureEnabled)
						{
							command.Append(c);
						}
						else if(captureStart)
						{
							output.Append('<');
							output.Append(c);
							captureStart = false;
						}
						else
						{
							output.Append(c);
						}
						break;
				}
			}

			return output.ToString();
		}

		#endregion

		#region Generic OleDb Database Writers

        public void WriteParameterLines(string objectInstance, object item, string commandInstance, bool commentsEnabled, string sprocPrefix)
        {
            if (item is ReferenceField)
            {
                WriteParameterLines(objectInstance, (ReferenceField)item, commandInstance, commentsEnabled, sprocPrefix);
            }
            else if (item is ValueField)
            {
                WriteParameterLines(objectInstance, (ValueField)item, commandInstance, commentsEnabled, sprocPrefix);
            }
            else if (item is EnumField)
            {
                WriteParameterLines(objectInstance, (EnumField)item, commandInstance, commentsEnabled, sprocPrefix);
            }
        }

        public void WriteParameterLines(string objectInstance, ReferenceField c, string commandInstance, bool commentsEnabled, string sprocPrefix)
		{
            // Skip Collections
            if (c.ReferenceMode == ReferenceMode.Collection)
                return;

			if (commentsEnabled)
			{				
				WriteLine("// {0} Parameters", c.Name);
			}

			WriteLine("if({0}.{1} == null)", objectInstance, c.PrivateName);
			if (!c.IsTableCoded)
			{
				WriteLine("{");
				Indent++;
                WriteLine("{0}.Parameters.Add(\"" + sprocPrefix + "{1}Table\", OleDbType.Integer).Value = DBNull.Value;", 
					commandInstance, c.Name);
                WriteLine("{0}.Parameters.Add(\"" + sprocPrefix + "{1}ID\", OleDbType.Integer).Value = DBNull.Value;", 
					commandInstance, c.Name);
				Indent--;
				WriteLine("}");
			}
			else
			{
				Indent++;
                WriteLine("{0}.Parameters.Add(\"" + sprocPrefix + "{1}ID\", OleDbType.Integer).Value = DBNull.Value;", 
					commandInstance, c.Name);
				Indent--;
			}

			WriteLine("else");

			if (!c.IsTableCoded)
			{
				WriteLine("{");
				Indent++;
                WriteLine("{0}.Parameters.Add(\"" + sprocPrefix + "{1}Table\", OleDbType.Integer).Value = {2}.{3}.Table;", 
					commandInstance, c.Name, objectInstance, c.PrivateName);
                WriteLine("{0}.Parameters.Add(\"" + sprocPrefix + "{1}ID\", OleDbType.Integer).Value = {2}.{3}.ID;", 
					commandInstance, c.Name, objectInstance, c.PrivateName);
				Indent--;
				WriteLine("}");
			}
			else
			{
			    	Indent++;
                    WriteLine("{0}.Parameters.Add(\"" + sprocPrefix + "{1}ID\", OleDbType.Integer).Value = {2}.{3}.ID;", 
					commandInstance, c.Name, objectInstance, c.PrivateName);
				Indent--;
			}			
		}

        public void WriteParameterLines(string objectInstance, 
            ValueField f, string commandInstance, bool commentsEnabled, 
            string sprocPrefix)
        {
            if (commentsEnabled)
            {
                WriteLine("// {0} Parameters", f.Name);
            }

            if (f.IsNullable & f.ValueType.NullValue != string.Empty)
            {
                WriteLine("if({0}.{1} == {2})",
                    objectInstance, f.PrivateName, f.ValueType.NullValue);
                Indent++;
                WriteLine("{0}.Parameters.Add(\"{1}{2}\", {3}).Value = DBNull.Value;",
                    commandInstance, sprocPrefix, f.Name, f.ValueType.DotNetDbType);
                Indent--;
                WriteLine("else");
                Indent++;
                WriteLine("{0}.Parameters.Add(\"{1}{2}\", {3}).Value = {4}.{5};",
                    commandInstance, sprocPrefix, f.Name, f.ValueType.DotNetDbType,
                    objectInstance, string.Format(f.ValueType.DataWriterFormat, f.PrivateName));
                Indent--;
            }
            else
            {
                WriteLine("{0}.Parameters.Add(\"{1}{2}\", {3}).Value = {4}.{5};",
                    commandInstance, sprocPrefix, f.Name, f.ValueType.DotNetDbType,
                    objectInstance, string.Format(f.ValueType.DataWriterFormat, f.PrivateName));
            }
        }

        public void WriteParameterLines(string objectInstance, 
            EnumField f, string commandInstance, bool commentsEnabled, 
            string sprocPrefix)
        {
            if (commentsEnabled)
            {
                WriteLine("// {0} Parameters", f.Name);
            }
             
            WriteLine("{0}.Parameters.Add(\"{1}{2}\", {3}).Value = " +
                "{4}.{5}.ToString();",
                commandInstance, sprocPrefix, f.Name, 
                f.EnumType.ParentEnumEntry.ValueType.DotNetDbType,
                objectInstance, f.PrivateName);
        }

		#endregion

		#region Generic Accessors

		/// <summary>
		/// Writes a property out into code. Be sure to include a WriteLine() at the end for readability.
		/// </summary>
		/// <param name="dataType">Type datatype of the property.</param>
		/// <param name="privateName">Type private name of the property</param>
		/// <param name="publicName">Type public name of the property</param>
		public void WriteProperty(string dataType, string privateName, string publicName)
		{
			//
			// Public Properties
			//			
			WriteLine("public {0} {1}", dataType, publicName);
			WriteLine("{");
			Indent++;
			WriteLine("get");
			WriteLine("{");
			Indent++;
			WriteLine("return {0};", privateName);
			Indent--;
			WriteLine("}");
			WriteLine("set");
			WriteLine("{");
			Indent++;
			WriteLine("{0} = value;", privateName);
			Indent--;
			WriteLine("}");
			Indent--;
			WriteLine("}");
		}

		#endregion

		public void Backspace(int count)
		{
			s.Length -= count;
		}

		public override string ToString()
		{
			return s.ToString();
		}
	}
}
