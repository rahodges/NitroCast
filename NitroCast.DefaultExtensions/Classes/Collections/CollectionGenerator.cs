using System;
using System.Reflection;
using NitroCast.Core.Extensions;
using NitroCast.Core;

namespace NitroCast.CodeGenerators
{
	/// <summary>
	/// Summary description for ManagerClassGenerator.
	/// </summary>
	[ExtensionAttribute("Default Collection Class",
		 "Roy A.E. Hodges",
		 "Copyright � 2003 Roy A.E. Hodges. All Rights Reserved.",
		 "{0}Collection.cs",
		 "Default collection class for the __classObject in designed.",
		 "\\Default\\Collection Class")]
	public class CollectionGenerator : OutputExtension
	{
		public override string Render()
		{
//			CodeNamespace codeNamespace = new CodeNamespace(___classObject.Name_Space);
//			codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
//			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
//		
//			CodeTypeDeclaration codeType = new CodeTypeDeclaration(___classObject.Name + "Collection : IList, ICloneable");
//			codeNamespace.Types.Add(codeType);
//			codeType.IsClass = true;

			CodeWriter output = new CodeWriter();
			output.CurrentClass = _modelClass;
			output.ColumnWidth = 65;

            output.WriteLine("/* ********************************************************** *");
            output.WriteLine(" * AMNS NitroCast v1.0 Class Collection Object Business Tier    *");
            output.WriteLine(" * Autogenerated by NitroCast � 2007 Roy A.E Hodges             *");
            output.WriteLine(" * All Rights Reserved                                        *");
            output.WriteLine(" * ---------------------------------------------------------- *");
            output.WriteLine(" * Source code may not be reproduced or redistributed without *");
            output.WriteLine(" * written expressed permission from the author.              *");
            output.WriteLine(" * Permission is granted to modify source code by licencee.   *");
            output.WriteLine(" * These permissions do not extend to third parties.          *");
            output.WriteLine(" * ********************************************************** */");
            output.WriteLine();

			output.WriteLine("using System;");
			output.WriteLine("using System.Collections;");
			output.WriteLine();
			output.WriteLine("namespace {0}", _modelClass.Namespace);
			output.WriteLine("{");
			
			output.Indent++;
			output.WriteLine("/// <summary>");
			output.WriteXmlSummary(_modelClass.Summary);
			//			output.WriteLine("/// Summary for {0}List", name);
			output.WriteLine("/// </summary>");
			output.WriteLine("public class {0}Collection : IList, ICloneable ", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("private int count = 0;");									// Virtual Item Count
			output.WriteLine("private {0}[] {0}Array ;", _modelClass.Name);
			output.WriteLine();

			output.WriteLine("public {0}Collection() : base()", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}Array = new {0}[15];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public {0}Collection(int capacity) : base()", _modelClass.Name);
			output.WriteLine("{");
            output.Indent++;
			output.WriteLine("{0}Array = new {0}[capacity];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			#region IList Implementation

			output.WriteRegionBegin("IList Implemenation");
			output.WriteLine("public bool IsFixedSize");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return false;");
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public bool IsReadOnly");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return false;");
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("object IList.this[int index]");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return this[index];");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine("set");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}Array[index] = ({0}) value;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public {0} this[int index]", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("if(index > count - 1)");
			output.Indent++;
			output.WriteLine("throw(new Exception(\"Index out of bounds.\"));");
			output.Indent--;
			output.WriteLine("return {0}Array[index];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine("set");
			output.WriteLine("{");
			output.Indent++;			
			output.WriteLine("OnCollectionChanged(EventArgs.Empty);");
			output.WriteLine("{0}Array[index] = value;", _modelClass.Name);			
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("int IList.Add(object value)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return Add(({0}) value);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public int Add({0} value)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("OnCollectionChanged(EventArgs.Empty);");
			output.WriteLine("lock(this)");						// LOCK
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("count++;");
			output.WriteLine("// Resize the array if the count is greater than the length ");
			output.WriteLine("// of the array.");			
			output.WriteLine("if(count > {0}Array.GetUpperBound(0) + 1)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}[] temp{0}Array = new {0}[count * 2];", _modelClass.Name);
			output.WriteLine("Array.Copy({0}Array, temp{0}Array, count - 1);", _modelClass.Name);
			output.WriteLine("{0}Array = temp{0}Array;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine("{0}Array[count - 1] = value;", _modelClass.Name);
			output.Indent--;									// END LOCK
			output.WriteLine("}");
			output.WriteLine("return count -1;");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();	

			output.WriteLine("public void Clear()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("OnCollectionChanged(EventArgs.Empty);");
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("count = 0;");
			output.WriteLine("{0}Array = new {0}[15];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			
			output.WriteLine("bool IList.Contains(object value)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return Contains(({0}) value);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();			

			output.WriteLine("public bool Contains({0} value)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return IndexOf(value) != -1;");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("int IList.IndexOf(object value)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return IndexOf(({0}) value);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			
			output.WriteLine("public int IndexOf({0} value)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("for(int x = 0; x < count; x++)");
			output.Indent++;
            output.WriteLine("if(Object.ReferenceEquals({0}Array[x], value))", _modelClass.Name);
			output.Indent++;
			output.WriteLine("return x;");
			output.Indent--;
			output.Indent--;
			output.WriteLine("return -1;");
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

            

			output.WriteLine("void IList.Insert(int index, object value)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("Insert(index, ({0}) value);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public void Insert(int index, {0} value)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("OnCollectionChanged(EventArgs.Empty);");
			output.WriteLine("lock(this)");						// LOCK
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("count++;");						// Increment item count to accomidate insertion
			output.WriteLine("// Resize the array if the count is greater than the length ");
			output.WriteLine("// of the array.");			
			output.WriteLine("if(count > {0}Array.GetUpperBound(0) + 1)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}[] temp{0}Array = new {0}[count * 2];", _modelClass.Name);
			output.WriteLine("Array.Copy({0}Array, temp{0}Array, count - 1);", _modelClass.Name);
			output.WriteLine("{0}Array = temp{0}Array;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine("for(int x = index + 1; x == count - 2; x ++)");
			output.Indent++;
			output.WriteLine("{0}Array[x] = {0}Array[x - 1];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("{0}Array[index] = value;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("void IList.Remove(object value)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("Remove(({0}) value);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public void Remove({0} value)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("OnCollectionChanged(EventArgs.Empty);");
			output.WriteLine("int index = IndexOf(value);");
			output.WriteLine("if(index == -1)");
			output.Indent++;
			output.WriteLine("throw(new Exception(\"{0} not found in collection.\"));", _modelClass.Name);
			output.Indent--;
			output.WriteLine("RemoveAt(index);");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public void RemoveAt(int index)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("OnCollectionChanged(EventArgs.Empty);");
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("for(int x = index + 1; x <= count - 1; x++)");
			output.Indent++;
			output.WriteLine("{0}Array[x-1] = {0}Array[x];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("{0}Array[count - 1] = null;", _modelClass.Name);
			output.WriteLine("count--;");
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteRegionEnd();

            #endregion

			#region ICollection Implementation

			output.WriteRegionBegin("ICollection Implementation");
			output.WriteLine("public int Count");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return count;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public bool IsSynchronized");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return {0}Array.IsSynchronized;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public object SyncRoot");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return {0}Array;", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public void CopyTo(Array array, int index)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}Array.CopyTo(array, index);", _modelClass.Name);			
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteRegionEnd();

			#endregion

			#region IEnumerator Implementation
			
			output.WriteRegionBegin("IEnumerator Implementation");
			output.WriteLine("public Enumerator GetEnumerator()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return new Enumerator({0}Array, count);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("IEnumerator IEnumerable.GetEnumerator()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return GetEnumerator();");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			// begin enumerator
			output.WriteLine("public class Enumerator : IEnumerator");
			output.WriteLine("{");
			
			output.Indent++;
			output.WriteLine("private {0}[] {0}Array;", _modelClass.Name);
			output.WriteLine("private int cursor;");
			output.WriteLine("private int virtualCount;");
			output.WriteLine();
			
			output.WriteLine("public Enumerator({0}[] {0}Array, int virtualCount)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("this.{0}Array = {0}Array;", _modelClass.Name);
			output.WriteLine("this.virtualCount = virtualCount;");
			output.WriteLine("cursor = -1;");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			
			output.WriteLine("public void Reset()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("cursor = -1;");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public bool MoveNext()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("if(cursor < {0}Array.Length)", _modelClass.Name);
			output.Indent++;
			output.WriteLine("cursor++;");
			output.Indent--;
			output.WriteLine("return(!(cursor == virtualCount));", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("public {0} Current", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("if((cursor < 0) || (cursor == virtualCount))", _modelClass.Name);
			output.Indent++;
			output.WriteLine("throw(new InvalidOperationException());");
			output.Indent--;		
			output.WriteLine("return {0}Array[cursor];", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("Object IEnumerator.Current");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("get");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return Current;");
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");

			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteRegionEnd();
			
			#endregion

			#region ICloneable Implementation

            output.WriteLine("/// <summary>");
            output.WriteLine("/// Makes a shallow copy of the current {0}Collection.", _modelClass.Name);
            output.WriteLine("/// as the parent object.");
            output.WriteLine("/// </summary>");
            output.WriteLine("/// <returns>{0}Collection</returns>", _modelClass.Name);
			output.WriteRegionBegin("ICloneable Implementation");
			output.WriteLine("object ICloneable.Clone()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("return Clone();");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

            output.WriteLine("/// <summary>");
            output.WriteLine("/// Makes a shallow copy of the current {0}Collection.", _modelClass.Name);
            output.WriteLine("/// as the parent object.");
            output.WriteLine("/// </summary>");
            output.WriteLine("/// <returns>{0}Collection</returns>", _modelClass.Name);
			output.WriteLine("public {0}Collection Clone()", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}Collection cloned{0} = new {0}Collection(count);", _modelClass.Name);
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("foreach({0} item in this)", _modelClass.Name);
			output.Indent++;
			output.WriteLine("cloned{0}.Add(item);", _modelClass.Name);
			output.Indent--;		
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine("return cloned{0};", _modelClass.Name);			
			output.Indent--;
			output.WriteLine("}");			
			output.WriteLine();

            output.WriteLine("/// <summary>");
            output.WriteLine("/// Makes a deep copy of the current {0}.", _modelClass.Name);
            output.WriteLine("/// </summary>");
            output.WriteLine("/// <param name=\"isolation\">Placeholders are used to isolate the ");
            output.WriteLine("/// items in the {0}Collection from their children.</param>", _modelClass.Name);
			output.WriteLine("public {0}Collection Copy(bool isolated)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("{0}Collection isolatedCollection = new {0}Collection(count);", _modelClass.Name);
            output.WriteLine();
            
            // Start lock statement
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;

            output.WriteLine("if(isolated)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("for(int i = 0; i < count; i++)", _modelClass.Name);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("isolatedCollection.Add({0}Array[i].NewPlaceHolder());", _modelClass.Name);
            output.Indent--;
            output.WriteLine("}");
            output.Indent--;
            output.WriteLine("}");

            output.WriteLine("else");
            output.WriteLine("{");
            output.Indent++;
			output.WriteLine("for(int i = 0; i < count; i++)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;            
            output.WriteLine("isolatedCollection.Add({0}Array[i].Copy());", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");

            // End lock statement
            output.Indent--;
            output.WriteLine("}");

            output.WriteLine("return isolatedCollection;");

            // End Method
			output.Indent--;
			output.WriteLine("}");			
			output.WriteLine();
			output.WriteRegionEnd();

			#endregion

			#region Events

			output.WriteRegionBegin("Events");
			output.WriteLine("public event EventHandler CollectionChanged;");
			output.WriteLine();

			output.WriteLine("protected virtual void OnCollectionChanged(EventArgs e)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("if(CollectionChanged != null)");
			output.Indent++;
			output.WriteLine("CollectionChanged(this, e);");
			output.Indent--;
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteRegionEnd();

			#endregion

			#region Sort Methods

			output.WriteRegionBegin("Sort Methods");
			output.WriteXmlSummary("Sorts the collection by id.");
			output.WriteLine("public void Sort()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("Array.Sort({0}Array, 0, count);", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteRegionEnd();

			output.WriteRegionBegin("Find Methods");
			output.WriteXmlSummary("Finds a record by ID.");
			output.WriteLine("public <.C> Find(int id)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("lock(this)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("for(int x = 0; x < count; x++)");
			output.Indent++;
			output.WriteLine("if(<.C>Array[x].ID == id)");
			output.Indent++;
			output.WriteLine("return <.C>Array[x];");
			output.Indent--;
			output.Indent--;
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine("return null;");			
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteRegionEnd();

			#endregion

			output.WriteLine("#region ToString() Override Method");
			output.WriteLine();
			output.WriteLine("public override string ToString()");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("string lineBreak;");
			output.WriteLine();

			output.WriteLine("if(System.Web.HttpContext.Current != null)");
			output.WriteLine("lineBreak = \"<br />\";");
			output.WriteLine("else");
			output.WriteLine("lineBreak = \"\\r\\n\";");
			output.WriteLine();

			output.WriteLine("System.Text.StringBuilder s = new System.Text.StringBuilder();");
			output.WriteLine("for(int x = 0; x < count; x++)", _modelClass.Name);
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("if(x != 0)");
			output.Indent++;
			output.WriteLine("s.Append(lineBreak);");
			output.Indent--;
			output.WriteLine("s.Append({0}Array[x].ToString());", _modelClass.Name);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteLine("return s.ToString();");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

                //output.WriteLine("#region Encoded String");
                //output.WriteLine();
                //output.WriteLine("public string ToEncodedString(string separator, string lastSeparator)");
                //output.WriteLine("{");
                //output.Indent++;
                //output.WriteLine("System.Text.StringBuilder s = new System.Text.StringBuilder();");
                //output.WriteLine("for (int i = 0; i < this.Count; i++)");
                //output.WriteLine("{");
                //output.Indent++;
                //output.WriteLine("if (i > 0)");
                //output.WriteLine("{");
                //output.Indent++;
                //output.WriteLine("if (this.Count == 2 | this.Count == i+1)");
                //output.Indent++;
                //output.WriteLine("s.Append(lastSeparator);");
                //output.Indent--;
                //output.WriteLine("else");
                //output.Indent++;
                //output.WriteLine("s.Append(separator);");
                //output.Indent--;
                //output.Indent--;
                //output.WriteLine("}");
                //output.WriteLine("s.Append(this[i].ToString());");
                //output.Indent--;
                //output.WriteLine("}");
                //output.WriteLine("return s.ToString();");
                //output.Indent--;
                //output.WriteLine("}");
                //output.WriteLine();
                //output.WriteLine("#endregion");
                //output.WriteLine();

			if(CustomCode != null && CustomCode.Length > 0)
			{
				output.WriteLine("//--- Begin Custom Code ---");
				output.WriteDirect(CustomCode);
				output.WriteLine("//--- End Custom Code ---");
			}

			// end class
			output.Indent--;
			output.WriteLine("}");

			// end namespace
			output.Indent--;
			output.WriteLine("}");

			return output.ToString();
		}
	}
}
