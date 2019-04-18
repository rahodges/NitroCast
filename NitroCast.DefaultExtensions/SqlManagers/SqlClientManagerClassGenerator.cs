using System;
using System.Collections;
using System.Data;
using Amns.DbModel.Core;
using Amns.DbModel.Core.Extensions;

namespace Amns.DbModel.DefaultPlugins.SqlManagers
{
	/// <summary>
	/// Summary description for ManagerClassGenerator.
	/// </summary>
	[PluginAttribute("Default SqlClientManager Class",
		 "Roy A.E. Hodges",
		 "Copyright © 2003-2004 Roy A.E. Hodges. All Rights Reserved.",
		 "{0}Manager.cs",
		 "Default datalayer manager class for the _classObject in design. This manager utilizes \"System.Data.SqlClient\".",
		 "\\Default\\SqlClientManager Class")]
	public class SqlClientManagerClassGenerator : ClassOutputPlugin
	{
		const string sprocPrefix = "@";

		public SqlClientManagerClassGenerator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Generate Method

		public override string Render()
		{
			if(_classObject.Fields.Count == 0)
				return "Error: Object class contains no fields.";

			CodeWriter output = new CodeWriter();
			output.CurrentClass = _classObject;
            			
			output.WriteLine("using System;");
			output.WriteLine("using System.Data;");
			output.WriteLine("using System.Data.SqlClient;");
			output.WriteLine("using System.Text;");
			if(_classObject.IsCachingEnabled)
			{
				output.WriteLine("using System.Web;");
				output.WriteLine("using System.Web.Caching;");
			}

			//
			// Add imported namespaces
			//
			int importCount = -1;
			bool addImport = true;
			string[] namespacelist = new string[20];
			foreach(ChildEntry c in _classObject.Children)
			{
				addImport = true;

				foreach(string name in namespacelist)
					if(c.DataType.NameSpace == name | c.DataType.NameSpace == _classObject.Namespace)
					{
						addImport = false;
						break;
					}
				
				if(addImport)
				{
					importCount++;
					namespacelist[importCount] = c.DataType.NameSpace;										
				}
								
				//				if(c.DataType.ParentClassEntry != null)
				foreach(ChildEntry subChild in c.DataType.ParentClassEntry.Children)
				{
					addImport = true;
	
					foreach(string name in namespacelist)
						if(subChild.DataType.NameSpace == name | subChild.DataType.NameSpace == _classObject.Namespace)
						{
							addImport = false;
							break;
						}

					if(addImport)
					{
						importCount++;
						namespacelist[importCount] = subChild.DataType.NameSpace;
					}
				}				
			}

			for(int x = 0; x <= importCount; x++)
				output.WriteLine("using {0};", namespacelist[x]);

			output.WriteLine();
			output.WriteLine("namespace {0}", _classObject.Namespace);
			output.WriteLine("{");
			output.Indent++;

			#region GetCollectionParseChildren Enum

			if(_classObject.Children.Count > 0 & _classObject.Children.LastOneToManyRelation != -1)
			{
				output.Write("public enum <.C>Flags : int { ");
				
				for(int x = 0; x < _classObject.Children.Count; x++)
				{
					if(_classObject.Children[x].HasChildrenTables)
						continue;

					if(!_classObject.Children[x].IsTableCoded)
						continue;
                    
					output.WriteSeparation(_classObject.Children[x].Name, string.Empty, ",");
					
					if(_classObject.Children[x].DataType.ParentClassEntry != null)
						foreach(ChildEntry subChild in _classObject.Children[x].DataType.ParentClassEntry.Children)
						{
							output.WriteSeparation(_classObject.Children[x].Name + subChild.Name,
								string.Empty, ",");
						}
				}

				//
				// Remove trailing comma
				//
				output.WriteSeparationEnd("};");
				output.WriteLine();
			}

			#endregion

			output.WriteLine("/// <summary>");
			output.WriteLine("/// Datamanager for <.C> objects.");
			output.WriteLine("/// </summary>");
			output.WriteLine("public class <.C>Manager");
			output.WriteLine("{");

			output.Indent++;
			output.WriteLine("// Fields");
			output.WriteLine("private string connectionString;");
			if(!_classObject.IsTableCoded)
				output.WriteLine("private string tableName;");

			output.WriteLine();

			// Caching
			if(_classObject.IsCachingEnabled)
			{
				if(_classObject.IsTableCoded)
				{
					output.WriteLine("// Caching Fields");
					output.WriteLine("private static bool cacheEnabled	= true;");
					output.WriteLine("private static <.C>Cache cache	= new <.C>Cache();");
					output.WriteLine();
					output.WriteLine("public static <.C>Cache Cache");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("get { return cache; }");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine();
					output.WriteLine("public static bool CacheEnabled");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("get { return cacheEnabled; }");
					output.WriteLine("set { cacheEnabled = value; }");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine();
				}
				else
				{
					output.WriteLine("// Hashtable to cache separate tables");
					output.WriteLine("private static bool cacheEnabled	= true;");
					output.WriteLine("private static System.Collections.Hashtable caches = new System.Collections.Hashtable(5);");
					output.WriteLine("// Caching Fields");
					output.WriteLine("public static <.C>Cache GetCache(string tableName)");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("if(caches[tableName] == null)");
					output.WriteLine("\treturn null;");
					output.WriteLine("return (<.C>Cache) caches[tableName];");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine("public static bool CacheEnabled");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("get { return cacheEnabled; }");
					output.WriteLine("set { cacheEnabled = value; }");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine();
				}
			}

			#region Readonly Field List For GetCollection method

			output.WriteLine("public static readonly string[] InnerJoinFields = new string[] {");
			output.Indent++;
			output.WriteLine("\"{0}ID\", ", _classObject.Name);
			foreach(ChildEntry c in _classObject.Children)
			{
				if(c.HasChildrenTables)
					continue;
				if(!c.DataType.IsTableCoded & !c.IsTableCoded)
					output.WriteLine("\"{0}Table\", ", c.Name);
				output.WriteLine("\"{0}ID\", ", c.Name);
			}
			for(int x = 0; x < _classObject.Fields.Count - 1; x++)
				output.WriteLine("\"{0}\", ", _classObject.Fields[x].ColumnName);
			output.WriteLine("\"{0}\" ", _classObject.Fields[_classObject.Fields.Count - 1].ColumnName);
			output.Indent--;
			output.WriteLine("};");
			output.WriteLine();

			#endregion

			output.WriteLine("#region Default DbModel Constructors");
			output.WriteLine();

			#region Constructors

			if(_classObject.IsTableCoded)
				output.WriteLine("public <.C>Manager(string connectionString)");
			else
				output.WriteLine("public <.C>Manager(string connectionString, string tableName)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("this.connectionString = connectionString;");
			if(!_classObject.IsTableCoded)
				output.WriteLine("this.tableName = tableName;");
			output.Indent--;
			output.WriteLine("}");

			output.WriteLine();
			output.WriteLine("#endregion");
			output.WriteLine();
			
			#endregion

			#region Insert Method

			output.WriteLine("#region Default DbModel Insert Method");
			output.WriteLine();

			output.WriteLine("/// <summary>");
			output.WriteLine("/// Inserts a {0} into the database. All children should have been", _classObject.Name);
			output.WriteLine("/// saved to the database before insertion. New children will not be");
			output.WriteLine("/// related to this object in the database.");
			output.WriteLine("/// </summary>");
			output.WriteLine("/// <param name=\"_{0}\">The {0} to insert into the database.</param>",
				_classObject.Name);
			if(_classObject.IsTableCoded)
				output.WriteLine("internal static int _insert({0} {1})", _classObject.Name, _classObject.PrivateName);
			else
				output.WriteLine("internal static int _insert({0} {1})", _classObject.Name, _classObject.PrivateName);
			output.WriteLine("{");
			output.Indent++;
			
			if(_classObject.IsCreateDateEnabled)
			{
				output.WriteLine("// Set Create Date to Now");
				output.WriteLine("<.c>.CreateDate = DateTime.Now.ToUniversalTime();");
				output.WriteLine();
			}

			if(_classObject.IsModifyDateEnabled)
			{
				output.WriteLine("// Set Modify Date to Now");
				output.WriteLine("<.c>.ModifyDate = DateTime.Now.ToUniversalTime();");
				output.WriteLine();
			}

			output.WriteLine("SqlConnection dbConnection = new SqlConnection({0}.connectionString);", _classObject.PrivateName);
			output.WriteLine("SqlCommand dbCommand = new SqlCommand();");
			output.WriteLine("dbCommand.Connection = dbConnection;");
			
			output.Write("dbCommand.CommandText = \"INSERT INTO ");

			if(_classObject.IsTableCoded)
				output.Write("{0}", _classObject.DefaultTableName);
			else
				output.Write(sprocPrefix + "TableName");

			output.Write(" (");
	
			foreach(ChildEntry c in _classObject.Children)
			{
				if(!c.IsTableCoded)
					output.WriteSeparation(string.Format("{0}Table", c.Name), "\"", ",\" +");
				if(!c.HasChildrenTables)
					output.WriteSeparation(string.Format("{0}ID", c.Name), "\"", ",\" +");
			}

			foreach(FieldEntry f in _classObject.Fields)
				output.WriteSeparation(string.Format("{0}", f.ColumnName), "\"", ",\" +");

			output.WriteSeparationReset(") VALUES (", "\"", "\" +");

			//
			// Output Query Parameters
			//

			foreach(ChildEntry c in _classObject.Children)
			{
				if(!c.IsTableCoded)
					output.WriteSeparation(string.Format("{0}{1}Table", sprocPrefix, c.Name), "\"", ",\" +");
				if(!c.HasChildrenTables)
					output.WriteSeparation(string.Format("{0}{1}ID", sprocPrefix, c.Name), "\"", ",\" +");
			}

			foreach(FieldEntry f in _classObject.Fields)
				output.WriteSeparation(string.Format("{0}{1}", sprocPrefix, f.ColumnName), "\"", ",\" +");

			output.WriteSeparationEnd(");\";");
			output.WriteLine();

			//
			// Output TableName Parameter
			//
			if(!_classObject.IsTableCoded)
			{
				output.WriteLine("//");
				output.WriteLine("// Output TableName Parameter ");
				output.WriteLine("//");
				output.WriteLine("dbCommand.CommandText = dbCommand.CommandText.Replace(\"" + sprocPrefix + "TableName\", {0}.tableName);", _classObject.PrivateName);
				output.WriteLine();
			}

			//
			// Output Stored Proc Parameter Collection
			//

			foreach(ChildEntry c in _classObject.Children)
			{
				if(c.HasChildrenTables)
					continue;
				output.WriteDbChildParameterLines(_classObject.PrivateName,
					c, "dbCommand", true, sprocPrefix);
			}				

			foreach(FieldEntry f in _classObject.Fields)
			{
				output.WriteLine("dbCommand.Parameters.Add(\"in{0}\", {1});",
					f.Name, f.DataType.DotNetDbType);
				if(f.IsNullable & f.DataType.NullValue != string.Empty)
				{
					output.WriteLine("if({0}.{1} == {2})", 
						_classObject.PrivateName, f.PrivateName, f.DataType.NullValue);
					output.Indent++;
					output.WriteLine("dbCommand.Parameters[\"in{0}\"].Value = DBNull.Value;",
						f.Name);
					output.Indent--;
					output.WriteLine("else");
					output.Indent++;
					output.WriteLine("dbCommand.Parameters[\"in{0}\"].Value = {1}.{2};",
						f.Name, _classObject.PrivateName, string.Format(f.DataType.DataWriterFormat, f.PrivateName));
					output.Indent--;
				}
				else
				{
					output.WriteLine("dbCommand.Parameters[\"in{0}\"].Value = {1}.{2};",
						f.Name, _classObject.PrivateName, string.Format(f.DataType.DataWriterFormat, f.PrivateName));
				}
			}
								
			output.WriteLine();			
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("dbCommand.ExecuteNonQuery();");
			//output.WriteLine("dbCommand.CommandType = CommandType.Text;");
			output.WriteLine("dbCommand.CommandText = \"SELECT @@IDENTITY AS IDVal\";");
			output.WriteLine("int id = (int) dbCommand.ExecuteScalar();");

			// Save children array relationships, not the actual objects
			foreach(ChildEntry c in _classObject.Children)
			{
				if(!c.HasChildrenTables)
					continue;

				// NOTE! ITEMS SHOULD BE SAVED WITH VALID ID'S BEFORE THIS OCCURS!
				// Call the save method on the object if the object is not saved.
				// Think of somehow throwing an exception for objects in the array that
				// do not have an id, perhaps this is not so easy.

				output.WriteLine();
				output.WriteLine("// Save child relationships for {0}.", c.Name);
				output.WriteLine("if({0}.{1} != null)", _classObject.PrivateName, c.PrivateName);
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("dbCommand.Parameters.Clear();");
				output.WriteLine("dbCommand.CommandText = \"INSERT INTO {0}Children_{1} \" +", _classObject.DefaultTableName, c.Name);
				output.Indent++;
				output.WriteLine("\"({0}ID, {1}ID)\" + ", _classObject.Name, c.DataType.Name);
				output.WriteLine("\" VALUES (" + sprocPrefix + "{0}ID, " + sprocPrefix + "{1}ID);\";", _classObject.Name, c.DataType.Name);
				output.Indent--;
				output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID\", SqlDbType.Int);", _classObject.Name);
				output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID\", SqlDbType.Int);", c.DataType.Name);
				output.WriteLine("foreach({0} item in {1}.{2})", c.DataType.Name, _classObject.PrivateName, c.PrivateName);
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID\"].Value = id;", _classObject.Name);				
				output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID\"].Value = item.ID;", c.DataType.Name);
				output.WriteLine("dbCommand.ExecuteNonQuery();");
				output.Indent--;
				output.WriteLine("}");
				output.Indent--;
				output.WriteLine("}");
			}

			output.WriteLine();
			output.WriteLine("dbConnection.Close();");

			// Add item to cache
			if(_classObject.IsCachingEnabled)
			{
				output.WriteLine("// Store <.c> in cache.");
				output.WriteLine("if(cacheEnabled) cacheStore(<.c>);");
			}

			output.WriteLine("return id;");
			
			
			// End Insert Method Declaration
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Update

			output.WriteLine("#region Default DbModel Update Method");
			output.WriteLine();

			// Write Update Method
			output.WriteLine("internal static void _update({0} {1})", _classObject.Name, _classObject.PrivateName);
			output.WriteLine("{");
			output.Indent++;
			
			output.WriteLine("SqlConnection dbConnection = new SqlConnection({0}.connectionString);", _classObject.PrivateName);
			output.WriteLine("SqlCommand dbCommand = new SqlCommand();");
			output.WriteLine("dbCommand.Connection = dbConnection;");
			output.Write("dbCommand.CommandText = \"UPDATE ");
			if(_classObject.IsTableCoded)
				output.Write(_classObject.DefaultTableName);
			else
				output.Write(sprocPrefix + "TableName");
			output.Write(" SET ");

			foreach(ChildEntry c in _classObject.Children)
				if(!c.HasChildrenTables)
				{
					if(!c.IsTableCoded)
						output.WriteSeparation(string.Format("{0}Table=" + sprocPrefix + "{0}Table", c.Name), "\"", ",\" +");
					output.WriteSeparation(string.Format("{0}ID=" + sprocPrefix + "{0}ID", c.Name), "\"", ",\" +");
				}

			foreach(FieldEntry f in _classObject.Fields)
				output.WriteSeparation(string.Format("{0}=" + sprocPrefix + "{0}", f.ColumnName, f.ColumnName), "\"", ",\" +");

			// Uncomment line for SQL Server Compatability
			output.WriteSeparationEnd();
			if(_classObject.Concurrency == ConcurrencyType.Optimistic)
				output.WriteLine(" WHERE {0}ID=" + sprocPrefix + "{0}ID AND " +
					"ModifyDate=inConcurrencyDate;", _classObject.Name);
			else
				output.WriteLine(" WHERE {0}ID=" + sprocPrefix + "{0}ID\";", _classObject.Name);
			
			output.WriteLine();

			//
			// Output TableName Parameter
			//
			if(!_classObject.IsTableCoded)
			{
				output.WriteLine("//");
				output.WriteLine("// Output TableName Parameter ");
				output.WriteLine("//");
				//				output.WriteLine("dbCommand.Parameters.Add(\"@TableName\", SqlType.VarChar);");
				//				output.WriteLine("dbCommand.Parameters[\"@TableName\"].Value = {0}.tableName;", _classObject.PrivateName);
				output.WriteLine("dbCommand.CommandText = dbCommand.CommandText.Replace(\"" + sprocPrefix + "TableName\", {0}.tableName);", _classObject.PrivateName);
				output.WriteLine();
			}

			if(_classObject.Concurrency == ConcurrencyType.Optimistic)
			{
				output.WriteLine("dbCommand.Parameters.Add(\"inConcurrencyDate\", SqlDbType.DateTime);");
				output.WriteLine("dbcommand.Parameters[\"inConcurrencyDate\"].Value = <.c>.ModifyDate;");
				output.WriteLine();
			}

			if(_classObject.IsModifyDateEnabled)
			{
				output.WriteLine("// Set Modify Date to Now");
				output.WriteLine("<.c>.ModifyDate = DateTime.Now.ToUniversalTime();");
				output.WriteLine();
			}

			//
			// Output Stored Proc Parameter Collection
			//

			foreach(ChildEntry c in _classObject.Children)
			{
				if(c.HasChildrenTables)
					continue;
				output.WriteDbChildParameterLines(_classObject.PrivateName,
					c, "dbCommand", true, sprocPrefix);
			}				

			foreach(FieldEntry f in _classObject.Fields)
			{
				output.WriteLine("dbCommand.Parameters.Add(\"in{0}\", {1});",
					f.Name, f.DataType.DotNetDbType);
				if(f.IsNullable & f.DataType.NullValue != string.Empty)
				{
					output.WriteLine("if({0}.{1} == {2})", 
						_classObject.PrivateName, f.PrivateName, f.DataType.NullValue);
					output.Indent++;
					output.WriteLine("dbCommand.Parameters[\"in{0}\"].Value = DBNull.Value;",
						f.Name);
					output.Indent--;
					output.WriteLine("else");
					output.Indent++;
					output.WriteLine("dbCommand.Parameters[\"in{0}\"].Value = {1}.{2};",
						f.Name, _classObject.PrivateName, string.Format(f.DataType.DataWriterFormat, f.PrivateName));
					output.Indent--;
				}
				else
				{
					output.WriteLine("dbCommand.Parameters[\"in{0}\"].Value = {1}.{2};",
						f.Name, _classObject.PrivateName, string.Format(f.DataType.DataWriterFormat, f.PrivateName));
				}
			}


			// Uncomment following lines for SQL Server Compatability
			//
			// Output ID Parameter
			//
			output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID\", SqlType.Int);", _classObject.Name);
			output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID\"].Value = {1}.iD;", _classObject.Name, _classObject.PrivateName);

			
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("dbCommand.ExecuteNonQuery();");
			
			// Update children array relationships, not the actual objects
			foreach(ChildEntry c in _classObject.Children)
				if(c.HasChildrenTables)
				{
					output.WriteLine();
					output.WriteLine("if({0}.{1} != null)", _classObject.PrivateName, c.PrivateName);
					output.WriteLine("{");
					output.Indent++;

					// NOTE! ITEMS SHOULD BE SAVED WITH VALID ID'S BEFORE THIS OCCURS!
					// Call the save method on the object if the object is not saved.
					// Think of somehow throwing an exception for objects in the array that
					// do not have an id, perhaps this is not so easy.

					output.WriteLine();				
					// Delete existing rows.
					output.WriteLine("// Delete child relationships for {0}.", c.Name);
					if(c.DataType.ParentClassEntry.PrivateName != _classObject.PrivateName)
					{
						output.WriteLine("dbCommand.CommandText = \"DELETE * FROM {0}Children_{1} WHERE {2}ID=" + sprocPrefix + "{2}ID;\";", 
							_classObject.DefaultTableName, c.Name, _classObject.Name);
						output.WriteLine("dbCommand.Parameters.Clear();");
						output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID\", SqlType.Int);", _classObject.Name);
						output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID\"].Value = {1}.iD;", _classObject.Name, _classObject.PrivateName);
						output.WriteLine("dbCommand.ExecuteNonQuery();");
						output.WriteLine();
						// Insert updated rows.
						output.WriteLine("// Save child relationships for {0}.", c.Name);
						output.Write("dbCommand.CommandText = \"INSERT INTO {0}Children_{1} ", _classObject.DefaultTableName, c.Name);
						output.WriteLine("({0}ID, {1}ID) VALUES (" + sprocPrefix + "{0}ID, " + sprocPrefix + "{1}ID);\";",
							_classObject.Name, c.DataType.Name);
						output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID\", SqlType.Int);", c.DataType.Name);
                    
						output.WriteLine("foreach({0} {1} in {2}.{3})", c.DataType.Name, c.DataType.ParentClassEntry.PrivateName, _classObject.PrivateName, c.PrivateName);
						output.WriteLine("{");
						output.Indent++;
						output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID\"].Value = {1}.ID;", c.DataType.Name, c.DataType.ParentClassEntry.PrivateName);
						output.WriteLine("dbCommand.ExecuteNonQuery();");
						output.Indent--;
						output.WriteLine("}");
					}
					else
					{
						output.WriteLine("dbCommand.CommandText = \"DELETE * FROM {0}Children_{1} WHERE {2}ID_1=" + sprocPrefix + "{2}ID_1;\";", 
							_classObject.DefaultTableName, c.Name, _classObject.Name);
						output.WriteLine("dbCommand.Parameters.Clear();");
						output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID_1\", SqlType.Int);", _classObject.Name);
						output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID_1\"].Value = {1}.iD;", _classObject.Name, _classObject.PrivateName);
						output.WriteLine("dbCommand.ExecuteNonQuery();");
						output.WriteLine();
						// Insert updated rows.
						output.WriteLine("// Save child relationships for {0}.", c.Name);
						output.Write("dbCommand.CommandText = \"INSERT INTO {0}Children_{1} ", _classObject.DefaultTableName, c.Name);
						output.WriteLine("({0}ID_1, {1}ID_2) VALUES (" + sprocPrefix + "{0}ID_1, " + sprocPrefix + "{1}ID_2);\";",
							_classObject.Name, c.DataType.Name);
						output.WriteLine("dbCommand.Parameters.Add(\"" + sprocPrefix + "{0}ID_2\", SqlType.Int);", c.DataType.Name);
                    
						output.WriteLine("foreach({0} child{1} in {2}.{3})", c.DataType.Name, c.DataType.ParentClassEntry.Name, _classObject.PrivateName, c.PrivateName);
						output.WriteLine("{");
						output.Indent++;
						output.WriteLine("dbCommand.Parameters[\"" + sprocPrefix + "{0}ID_2\"].Value = child{1}.ID;", c.DataType.Name, c.DataType.ParentClassEntry.Name);
						output.WriteLine("dbCommand.ExecuteNonQuery();");
						output.Indent--;
						output.WriteLine("}");
					}

					output.Indent--;
					output.WriteLine("}");
				}

			output.WriteLine();
			output.WriteLine("dbConnection.Close();");
			
			// Add item to cache
			if(_classObject.IsCachingEnabled)
			{
				output.WriteLine("// Store <.c> in cache.");
				output.WriteLine("if(cacheEnabled) cacheStore(<.c>);");
			}

			// End Update Method Declaration
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Fill

			output.WriteLine("#region Default DbModel Fill Method");
			output.WriteLine();

			// Write Fill Method

			int rCount = -1;				// Use a counter to track reader indexes for objects.

			output.WriteLine("internal static bool _fill({0} {1})", _classObject.Name, _classObject.PrivateName);
			output.WriteLine("{");
			output.Indent++;

			// TODO: Make deepclones instead of shallow ones.
			// Add item to cache
			if(_classObject.IsCachingEnabled)
			{
				output.WriteLine("// Clone item from cache.");
				output.WriteLine("if(cacheEnabled)");
				output.WriteLine("{");
				output.Indent++;
				if(_classObject.IsTableCoded)
					output.WriteLine("<.C> cached<.C> = cacheFind(<.c>.iD);");
				else
					output.WriteLine("<.C> cached<.C> = cacheFind(<.c>.iD, <.c>.tableName);");
				output.WriteLine("if(cached<.C> != null)");
				output.WriteLine("{");
				output.WriteLine("cached<.C>.CopyTo(<.c>);");
				output.WriteLine("return <.c>.isSynced;");
				output.WriteLine("}");
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();
			}
			
			output.WriteLine("StringBuilder query = new StringBuilder(\"SELECT \");");
			output.WriteLine("query.Append(string.Join(\",\", InnerJoinFields));");

			if(_classObject.IsTableCoded)
			{
				output.WriteLine("query.Append(\" FROM {0} WHERE {1}ID=\");", 
					_classObject.DefaultTableName, _classObject.Name);
			}
			else
			{
				output.WriteLine("query.Append(\" FROM \");");
				output.WriteLine("query.Append({0}.tableName);", _classObject.PrivateName);
				output.WriteLine("query.Append(\" WHERE {0}ID=\");", _classObject.Name);
			}			
			
			output.WriteLine("query.Append({0}.iD);", _classObject.PrivateName);
			output.WriteLine("query.Append(\";\");");
			output.WriteLine();

			output.WriteLine("SqlConnection dbConnection = new SqlConnection({0}.connectionString);",
				_classObject.PrivateName);
			output.WriteLine("SqlCommand dbCommand = new SqlCommand(query.ToString(), dbConnection);");
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("SqlDataReader r = dbCommand.ExecuteReader(CommandBehavior.SingleRow);");
			output.WriteLine();
			
			output.WriteLine("if(!r.Read())");
			output.Indent++;
			output.Write("throw(new Exception(string.Format(\"Cannot find {0}ID ", _classObject.Name);
			output.WriteLine("'{0}'.\", ");
			output.Indent++;
			output.WriteLine("{0}.iD)));", _classObject.PrivateName);
			output.Indent--;
			output.WriteLine();

			output.Indent--;

			//
			// Create new object...
			//
			if(_classObject.IsTableCoded)
				output.WriteLine("FillFromReader({0}, {0}.connectionString, r, 0, 1);", _classObject.PrivateName);
			else
				output.WriteLine("FillFromReader({0}, {0}.connectionString, {0}.tableName, r, 0, 1);", _classObject.PrivateName);

			output.WriteLine();

			// Set the id of the object.
			// output.WriteLine("_{0}.iD = id;", _classObject.Name);

			// Output each SqlDataReader.GetData[x] for each item in the object.

			//			rCount = -1;
			//			for(int x = 0; x <= _classObject.Children.Count - 1; x++)
			//			{
			//				if(_classObject.Children[x].HasChildrenTables)
			//					continue;
			//
			//				rCount++;
			//				output.WriteLine("if(!r.IsDBNull({0}))", rCount);
			//				output.Indent++;						
			//				if(_classObject.Children[x].IsTableCoded)
			//				{
			//					//
			//					// If the Child's DataType does not have coded tables then hard code the table here.
			//					//
			//					if(!_classObject.Children[x].DataType.IsTableCoded)
			//						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder({0}.connectionString, \"{3}\", r.GetInt32({4}));",
			//							_classObject.PrivateName,
			//							_classObject.Children[x].PrivateName,
			//							_classObject.Children[x].DataType.Name,
			//							_classObject.Children[x].TableName,
			//							rCount);
			//					else
			//						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder({0}.connectionString, r.GetInt32({3}));",
			//							_classObject.PrivateName,
			//							_classObject.Children[x].PrivateName,
			//							_classObject.Children[x].DataType.Name,
			//							rCount);
			//				}
			//				else
			//				{
			//					rCount++;
			//					output.WriteLine("{0}.{1} = {2}.NewPlaceHolder({0}.connectionString, r.GetString({3}), r.GetInt32({4}));",
			//						_classObject.PrivateName,
			//						_classObject.Children[x].PrivateName,
			//						_classObject.Children[x].DataType.Name,
			//						rCount-1,
			//						rCount);
			//				}
			//				output.Indent--;
			//			}
			//
			//			for(int x = 0; x <= _classObject.Fields.Count - 1; x++)
			//			{
			//				rCount++;
			//				if(_classObject.Fields[x].IsNullable)
			//				{
			//					output.WriteLine("if(!r.IsDBNull({0})) ", rCount);
			//					output.Indent++;
			//					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
			//						_classObject.Fields[x].PrivateName, 
			//						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount));
			//					output.Indent--;
			//
			//					//
			//					// Set the default value or null value
			//					//
			//					if(_classObject.Fields[x].UseDefaultValueOnNull)
			//					{
			//						output.WriteLine("else");
			//						output.Indent++;
			//						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
			//							_classObject.Fields[x].PrivateName,
			//							_classObject.Fields[x].DefaultValue);
			//						output.Indent--;
			//					}
			//					else if(_classObject.Fields[x].DataType.NullValue != string.Empty)
			//					{
			//						output.WriteLine("else");
			//						output.Indent++;
			//						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
			//							_classObject.Fields[x].PrivateName,
			//							_classObject.Fields[x].DataType.NullValue);
			//						output.Indent--;
			//					}
			//				}
			//				else
			//				{
			//					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
			//						_classObject.Fields[x].PrivateName, 
			//						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount));
			//				}
			//				output.WriteLine();
			//			}

			// End Fill Method Declaration
			output.WriteLine("r.Close();");
			output.WriteLine("dbConnection.Close();");

			// Add item to cache
			if(_classObject.IsCachingEnabled)
			{
				output.WriteLine("// Store <.c> in cache.");
				output.WriteLine("if(cacheEnabled) cacheStore(<.c>);");
			}

			output.WriteLine("return true;");

			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region GetCollection

			bool useCache = false;
			foreach(ChildEntry c in _classObject.Children)
			{
				useCache |= c.EnableCache;
				foreach(ChildEntry subChild in c.DataType.ParentClassEntry.Children)
					useCache |= subChild.EnableCache;
			}

			output.WriteLine("#region Default DbModel GetCollection Method");
			output.WriteLine();

			if(_classObject.Children.LastOneToManyRelation != -1)
				output.WriteLine("public <.C>Collection GetCollection(string whereClause, string sortClause, params <.C>Flags[] optionFlags)");
			else
				output.WriteLine("public <.C>Collection GetCollection(string whereClause, string sortClause)");
			output.WriteLine("{");
			output.Indent++;
			
			if(useCache)
				output.WriteLine("int cacheId = 0;");

			output.WriteLine("StringBuilder query = new StringBuilder(\"SELECT \");", _classObject.DefaultTableName, _classObject.Name);
			output.WriteLine("foreach(string columnName in InnerJoinFields)");
			output.WriteLine("{");
			output.Indent++;
			if(_classObject.IsTableCoded)
				output.WriteLine("query.Append(\"{0}.\");", _classObject.DefaultTableName);
			else
			{
				output.WriteLine("query.Append(tableName);");
				output.WriteLine("query.Append('.');");
			}
			output.WriteLine("query.Append(columnName);");
			output.WriteLine("query.Append(\",\");");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			//
			// Add
			//
			if(_classObject.Children.LastOneToManyRelation != -1)
			{
				// TODO: Link children tables of like datatypes together in the same cache.

				output.WriteLine("int innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;");
				foreach(ChildEntry c in _classObject.Children)
				{
					// This is obvious, cannot join this at all.
					if(c.HasChildrenTables)
						continue;

					// Skip children without coded tables which is not possible with SQL. This would
					// require that SQL inner joins the child's table dynamically and join on the
					// child's id from those tables as well. This is quite messy, so just skip it.
					if(!c.IsTableCoded)
						continue;

					output.WriteLine("int {0}Offset = -1;", c.PrivateName);
					
					// Do not setup cache object for unique children
					if(c.EnableCache)
						output.WriteLine("{0}IndexedList {1}Cache = new {0}IndexedList();",
							c.DataType.Name, c.PrivateName, c.DataType.Name);
					
					foreach(ChildEntry subChild in c.DataType.ParentClassEntry.Children)
					{
						if(subChild.HasChildrenTables)
							continue;
						output.WriteLine("int {0}{1}Offset = -1;", c.PrivateName, subChild.Name);
	
						// Do not setup cache object for subchildren that are unique
						if(subChild.EnableCache)
							output.WriteLine("{0}IndexedList {1}{2}Cache = new {0}IndexedList();",
								subChild.DataType.Name, c.PrivateName, subChild.Name);
					}
				}
				output.WriteLine();

				//
				// Output INNER JOIN Expressions
				//
				output.WriteLine("//");
				output.WriteLine("// Append Option Flag Fields");
				output.WriteLine("//");
				output.WriteLine("if(optionFlags != null)");
				output.Indent++;
				output.WriteLine("for(int x = 0; x < optionFlags.Length; x++)");
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("switch(optionFlags[x])");
				output.WriteLine("{");
				output.Indent++;
				foreach(ChildEntry c in _classObject.Children)
				{
					// This is obvious, cannot join this at all.
					if(c.HasChildrenTables)
						continue;

					// Skip children without coded tables which is not possible with SQL. This would
					// require that SQL inner joins the child's table dynamically and join on the
					// child's id from those tables as well. This is quite messy, so just skip it.
					if(!c.IsTableCoded)
						continue;

					output.WriteLine("case <.C>Flags.{0}:", c.Name);
					output.Indent++;

					output.WriteLine("for(int i = 0; i <= {0}Manager.InnerJoinFields.GetUpperBound(0); i++)", c.DataType.Name);
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("query.Append(\"{0}.\");", c.TableName);
					output.WriteLine("query.Append({0}Manager.InnerJoinFields[i]);", c.DataType.Name);
					output.WriteLine("query.Append(\",\");");
					output.Indent--;
					output.WriteLine("}");

					output.WriteLine("{0}Offset = innerJoinOffset;", c.PrivateName);
					output.WriteLine("innerJoinOffset = {0}Offset + {1}Manager.InnerJoinFields.GetUpperBound(0) + 1;", 
						c.PrivateName, c.DataType.Name);

					output.WriteLine("break;");
					output.Indent--;

					foreach(ChildEntry subChild in c.DataType.ParentClassEntry.Children)
					{
						if(subChild.HasChildrenTables)
							continue;
						output.WriteLine("case <.C>Flags.{0}{1}:", c.Name, subChild.Name);
						output.Indent++;
						
						if(subChild.IsTableCoded)
						{
							output.WriteLine("for(int i = 0; i <= {0}Manager.InnerJoinFields.GetUpperBound(0); i++)", subChild.DataType.Name);
							output.WriteLine("{");
							output.Indent++;
							output.WriteLine("query.Append(\"{0}.\");", subChild.TableName);
							output.WriteLine("query.Append({0}Manager.InnerJoinFields[i]);", subChild.DataType.Name);
							output.WriteLine("query.Append(\",\");");
							output.Indent--;
							output.WriteLine("}");

							output.WriteLine("{0}{1}Offset = innerJoinOffset;", c.PrivateName, subChild.Name);
							output.WriteLine("innerJoinOffset = {0}{1}Offset + {2}Manager.InnerJoinFields.GetUpperBound(0) + 1;", 
								c.PrivateName, subChild.Name, subChild.DataType.Name);
						}

						output.WriteLine("break;");
						output.Indent--;

					}
				}
								
				output.Indent--;
				output.WriteLine("}");

				output.Indent--;
				output.WriteLine("}");

				output.Indent--;
				output.WriteLine();
			}
            
			//
			// Remove Last Comma!
			//
			output.WriteLine("//");
			output.WriteLine("// Remove trailing comma");
			output.WriteLine("//");
			output.WriteLine("query.Length--;");			// Remove last comma

			//
			// Add the option flags code for objects that have relations
			//
			if(_classObject.Children.LastOneToManyRelation != -1)
			{
				output.WriteLine("if(optionFlags != null)");
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("query.Append(\" FROM \");");
				output.WriteLine();

				//
				// Start INNER JOIN expressions
				//
				output.WriteLine("//");
				output.WriteLine("// Start INNER JOIN expressions");
				output.WriteLine("//");
				output.WriteLine("for(int x = 0; x < optionFlags.Length; x++)");
				output.Indent++;
				output.WriteLine("query.Append(\"(\");");
				output.Indent--;
				output.WriteLine();

				//
				// Output PRIMARY TABLE
				// 
				if(_classObject.IsTableCoded)
					output.WriteLine("query.Append(\"{0}\");", _classObject.DefaultTableName);
				else			
					output.WriteLine("query.Append(tableName);");
				
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine("else");
				output.WriteLine("{");
				output.Indent++;
				if(_classObject.IsTableCoded)
					output.WriteLine("query.Append(\" FROM {0} \");", _classObject.DefaultTableName);
				else
				{
					output.WriteLine("query.Append(\" FROM \");");
					output.WriteLine("query.Append(tableName);");
				}
				output.Indent--;
				output.WriteLine("}");
			}
			else
			{
				if(_classObject.IsTableCoded)
					output.WriteLine("query.Append(\" FROM {0} \");", _classObject.DefaultTableName);
				else
				{
					output.WriteLine("query.Append(\" FROM \");");
					output.WriteLine("query.Append(tableName);");
				}
			}

			if(_classObject.Children.LastOneToManyRelation != -1)
			{
				//
				// Output INNER JOIN Expressions
				//
				output.WriteLine("//");
				output.WriteLine("// Finish INNER JOIN expressions");
				output.WriteLine("//");
				output.WriteLine("if(optionFlags != null)");
				output.Indent++;
				output.WriteLine("for(int x = 0; x < optionFlags.Length; x++)");
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("switch(optionFlags[x])");
				output.WriteLine("{");
				output.Indent++;
				foreach(ChildEntry c in _classObject.Children)
				{
					// This is obvious, cannot join this at all.
					if(c.HasChildrenTables)
						continue;

					// Skip children without coded tables which is not possible with SQL. This would
					// require that SQL inner joins the child's table dynamically and join on the
					// child's id from those tables as well. This is quite messy, so just skip it.
					if(!c.IsTableCoded)
						continue;

					output.WriteLine("case <.C>Flags.{0}:", c.Name);
					output.Indent++;

					if(_classObject.IsTableCoded & c.IsTableCoded)
					{
						output.WriteLine("query.Append(\" LEFT JOIN {0} ON {1}.{2}ID = {0}.{3}ID)\");",
							c.TableName, _classObject.DefaultTableName, c.Name, c.DataType.Name);
					}
					else if(!_classObject.IsTableCoded & c.IsTableCoded)
					{
						output.WriteLine("query.Append(\" LEFT JOIN {0} ON \");",
							c.TableName);
						output.WriteLine("query.Append(tableName);");
						output.WriteLine("query.Append(\".{0}ID = {1}.{2}ID)\");",
							c.Name, c.TableName, c.DataType.Name);
					}
					else if(_classObject.IsTableCoded & !c.IsTableCoded)
					{
						// TODO: Finish This!
					}

					output.WriteLine("break;");
					output.Indent--;

					// 
					// Apppend SubChildren Options
					//
					foreach(ChildEntry subChild in c.DataType.ParentClassEntry.Children)
					{
						// This is obvious, cannot join this at all.
						if(subChild.HasChildrenTables)
							continue;

						// Skip children without coded tables which is not possible with SQL. This would
						// require that SQL inner joins the child's table dynamically and join on the
						// child's id from those tables as well. This is quite messy, so just skip it.
						if(!subChild.IsTableCoded)
							continue;

						output.WriteLine("case <.C>Flags.{0}{1}:", c.Name, subChild.Name);
						output.Indent++;

						if(c.IsTableCoded & subChild.IsTableCoded)
						{
							output.WriteLine("query.Append(\" LEFT JOIN {0} ON {1}.{2}ID = {0}.{3}ID)\");",
								subChild.TableName, c.TableName, subChild.Name, subChild.DataType.Name);
						}
						else if(!c.IsTableCoded & subChild.IsTableCoded)
						{
							output.WriteLine("query.Append(\" LEFT JOIN {0} ON \");",
								subChild.TableName);
							output.WriteLine("query.Append(tableName);");
							output.WriteLine("query.Append(\".{0}ID = {1}.{2}ID)\");",
								subChild.Name, subChild.TableName, subChild.DataType.Name);
						}
						else if(c.IsTableCoded & !subChild.IsTableCoded)
						{
							// TODO: Finish This!
						}

						output.WriteLine("break;");
						output.Indent--;
					}					
				}
								
				output.Indent--;
				output.WriteLine("}");

				output.Indent--;
				output.WriteLine("}");

				output.Indent--;
				output.WriteLine();
			}

			output.WriteLine("//");
			output.WriteLine("// Render where clause");
			output.WriteLine("//");
			output.WriteLine("if(whereClause != string.Empty)");
			output.WriteLine("{");
			output.Indent++;
			output.WriteLine("query.Append(\" WHERE \");", _classObject.Name);
			output.WriteLine("query.Append(whereClause);");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("//");
			output.WriteLine("// Render sort clause ");
			output.WriteLine("//");
			output.WriteLine("if(sortClause != string.Empty)");
			output.WriteLine("{");
			output.Indent++;			
			output.WriteLine("query.Append(\" ORDER BY \");");
			output.WriteLine("query.Append(sortClause);");
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("//");
			output.WriteLine("// Render final semicolon");
			output.WriteLine("//");
			output.WriteLine("query.Append(\";\");");

			output.WriteLine("SqlConnection dbConnection = new SqlConnection(connectionString);");
			output.WriteLine("SqlCommand dbCommand = new SqlCommand(query.ToString(), dbConnection);");
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("SqlDataReader r = dbCommand.ExecuteReader();");
			output.WriteLine("{0}Collection {1}Collection = new {0}Collection();", _classObject.Name, _classObject.PrivateName);
			output.WriteLine("while(r.Read())");
			output.WriteLine("{");
			output.Indent++;

			//
			// Create new object...
			//
			if(_classObject.IsTableCoded)
				output.WriteLine("{0} {1} = new {0}(connectionString);", _classObject.Name, _classObject.PrivateName);
			else
				output.WriteLine("{0} {1} = new {0}(connectionString, tableName);", _classObject.Name, _classObject.PrivateName);
			output.WriteLine("{0}.iD = r.GetInt32(0);", _classObject.PrivateName);
			output.WriteLine("{0}.isSynced = true;", _classObject.PrivateName);
			output.WriteLine("{0}.isPlaceHolder = false;", _classObject.PrivateName);
			output.WriteLine();

			rCount = 0;				// Use a counter to track reader indexes for objects.
			
			for(int x = 0; x <= _classObject.Children.Count - 1; x++)
			{
				if(_classObject.Children[x].HasChildrenTables)
					continue;

				rCount++;
			}

			output.WriteLine("//");
			output.WriteLine("// Parse Fields From Database");
			output.WriteLine("//");
			for(int x = 0; x <= _classObject.Fields.Count - 1; x++)
			{
				rCount++;				
				if(_classObject.Fields[x].IsNullable)
				{
					output.WriteLine("if(!r.IsDBNull({0})) ", rCount);
					output.Indent++;
					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
						_classObject.Fields[x].PrivateName, 
						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount));
					output.Indent--;

					//
					// Set the default value or null value
					//
					if(_classObject.Fields[x].UseDefaultValueOnNull)
					{
						output.WriteLine("else");
						output.Indent++;
						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
							_classObject.Fields[x].PrivateName,
							_classObject.Fields[x].DefaultValue);
						output.Indent--;
					}
					else if(_classObject.Fields[x].DataType.NullValue != string.Empty)
					{
						output.WriteLine("else");
						output.Indent++;
						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
							_classObject.Fields[x].PrivateName,
							_classObject.Fields[x].DataType.NullValue);
						output.Indent--;
					}				
				}	
				else
				{
					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
						_classObject.Fields[x].PrivateName, 
						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount));
				}
				output.WriteLine();
			}
			output.WriteLine();

			// OBSOLETE CODE

			//			if(_classObject.IsTableCoded)
			//				output.WriteLine("{0} {1} = ParseFromReader(connectionString, r, 0, 1);", _classObject.Name, _classObject.PrivateName);
			//			else
			//				output.WriteLine("{0} {1} = ParseFromReader(connectionString, tableName, r, 0, 1);", _classObject.Name, _classObject.PrivateName);
			//			output.WriteLine();

			rCount = 0;

			//===============================================
			
			foreach(ChildEntry c in _classObject.Children)
			{
				// This is obvious, cannot join this at all.
				if(c.HasChildrenTables)
					continue;

				// Skip children without coded tables which is not possible with SQL. This would
				// require that SQL inner joins the child's table dynamically and join on the
				// child's id from those tables as well. This is quite messy, so just skip it.
				if(!c.IsTableCoded)
					continue;

				rCount++;

				output.WriteLine("//");
				output.WriteLine("// Fill {0} child", c.Name);
				output.WriteLine("//");
				output.WriteLine("if({0}Offset != -1)", c.PrivateName);
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("if(!r.IsDBNull({0}Offset) && r.GetInt32({0}Offset) != 0)", c.PrivateName);
				//				output.WriteLine("if(r.GetInt32({0}Offset) != 0)", c.PrivateName);

				output.WriteLine("{");
				output.Indent++;
								
				//
				// Search cache for child object.
				//
				if(c.EnableCache)
				{
					output.WriteLine("cacheId = {0}Cache.IndexOf(r.GetInt32({0}Offset));", c.PrivateName, _classObject.PrivateName, c.Name);
					output.WriteLine("if(cacheId != -1)");
					output.Indent++;
					output.WriteLine("{0}.{1} = {1}Cache[cacheId];", _classObject.PrivateName, c.PrivateName);
					output.Indent--;
					output.WriteLine("else");
					output.WriteLine("{");
					output.Indent++;
				}
				if(c.DataType.IsTableCoded)
					output.WriteLine("{0}.{1} = {2}Manager.ParseFromReader(connectionString, r, {1}Offset, {1}Offset+1);",
						_classObject.PrivateName, c.PrivateName, c.DataType.Name);
				else if(c.IsTableCoded)
					output.WriteLine("{0}.{1} = {2}Manager.ParseFromReader(connectionString, \"{3}\", r, {1}Offset, {1}Offset+1);",
						_classObject.PrivateName, c.PrivateName, c.DataType.Name, c.TableName);

				foreach(ChildEntry subChild in c.DataType.ParentClassEntry.Children)
				{
					if(subChild.HasChildrenTables)
						continue;

					output.WriteLine("//");
					output.WriteLine("// Fill {0} subchild {1}.", c.Caption, subChild.Caption);
					output.WriteLine("//");
					output.WriteLine("if({0}{1}Offset != -1)", c.PrivateName, subChild.Name, _classObject.PrivateName);
					output.WriteLine("{");
					output.Indent++;
				
					//
					// Search cache for child object.
					//
					if(subChild.EnableCache)
					{
						output.WriteLine("cacheId = {0}{1}Cache.IndexOf(r.GetInt32({0}{1}Offset));", c.PrivateName, subChild.Name);
						output.WriteLine("if(cacheId != -1)", c.PrivateName, subChild.Name);
						output.Indent++;
						output.WriteLine("{0}.{1}.{2} = {1}{2}Cache[cacheId];", _classObject.PrivateName, c.PrivateName, subChild.Name);
						output.Indent--;
						output.WriteLine("else");
						output.WriteLine("{");
						output.Indent++;
					}
					//					output.WriteLine("if({0}.{1}.{2}.IsPlaceHolder)", _classObject.PrivateName, c.PrivateName, subChild.Name);
					//					output.WriteLine("{");
					//					output.Indent++;
					if(subChild.DataType.IsTableCoded)
						output.WriteLine("{3}Manager.FillFromReader({0}.{1}.{2}, connectionString, r, {1}{2}Offset, {1}{2}Offset+1);",
							_classObject.PrivateName, c.PrivateName, subChild.Name, subChild.DataType.Name);
					else if(subChild.IsTableCoded)
						output.WriteLine("{4}Manager.FillFromReader({0}.{1}.{2}, connectionString, \"{3}\", r, {1}{2}Offset, {1}{2}Offset+1);",
							_classObject.PrivateName, c.PrivateName, subChild.Name, subChild.TableName, subChild.DataType.Name);
					if(subChild.EnableCache)
					{
						output.WriteLine("{0}{1}Cache.Add({2}.{0}.{1});", c.PrivateName, subChild.Name, _classObject.PrivateName);
						output.Indent--;
						output.WriteLine("}");
					}
					output.Indent--;
					output.WriteLine("}");
					
				}
				
				if(c.EnableCache)
				{
					output.WriteLine("{0}Cache.Add({1}.{2});", c.PrivateName, _classObject.PrivateName,
						c.PrivateName);

					output.Indent--;
					output.WriteLine("}");
				}

				output.Indent--;
				output.WriteLine("}");


				output.Indent--;
				output.WriteLine("}");

				//
				// Create a Placeholder for the object
				//
				output.WriteLine("else");
				output.WriteLine("{");
				output.Indent++;
				if(c.IsTableCoded)
				{
					output.WriteLine("if(!r.IsDBNull({0}) && r.GetInt32({0}) != 0)", rCount);
					//					output.WriteLine("if(r.GetInt32({0}) != 0)", rCount);

					output.Indent++;

					//
					// If the Child's DataType does not have coded tables then hard code the table here.
					//
					if(!c.DataType.IsTableCoded)
						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, \"{3}\", r.GetInt32({4}));",
							_classObject.PrivateName,
							c.PrivateName,
							c.DataType.Name,
							c.TableName,
							rCount);
					else
						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, r.GetInt32({3}));",
							_classObject.PrivateName,
							c.PrivateName,
							c.DataType.Name,
							rCount);
					output.Indent--;
				}
				else
				{
					//
					// Add an one to the offset counter to point to the ID instead of the table name... makes for cleaner code.
					output.WriteLine("if(!r.IsDBNull({0}Offset) && r.GetInt32({0}Offset) != 0)", c.PrivateName);
					//					output.WriteLine("if(r.GetInt32({0}Offset) != 0)", c.PrivateName);

					output.Indent++;
					output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, r.GetString({1}Offset-1), r.GetInt32({1}Offset));",
						_classObject.PrivateName,
						c.PrivateName,
						c.DataType.Name);
					output.Indent--;
				}
				output.Indent--;
				output.WriteLine("}");


			}

			if(_classObject.IsTableCoded)
				output.WriteLine("{0}Collection.Add({0});", _classObject.PrivateName);
			else
				output.WriteLine("{0}Collection.Add({0});", _classObject.PrivateName);

			// TODO: Write code to parse child objects into instantiated Class instead of using PlaceHolder objects.

			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();
			output.WriteLine("r.Close();");
			output.WriteLine("dbConnection.Close();");
			output.WriteLine("return {0}Collection;", _classObject.PrivateName);
			
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Parse From Reader Methods

			output.WriteLine("#region Default DbModel ParseFromReader Method");
			output.WriteLine();

			if(_classObject.IsTableCoded)
				output.WriteLine("public static {0} ParseFromReader(string connectionString, " +
					"SqlDataReader r, int idOffset, int dataOffset)", _classObject.Name);
			else
				output.WriteLine("public static {0} ParseFromReader(string connectionString, string tableName," +
					"SqlDataReader r, int idOffset, int dataOffset)", _classObject.Name);
			output.WriteLine("{");
			output.Indent++;

			//
			// Create new object...
			//
			if(_classObject.IsTableCoded)
			{
				output.WriteLine("{0} {1} = new {0}(connectionString);", _classObject.Name, _classObject.PrivateName);
				output.WriteLine("FillFromReader({0}, connectionString, r, idOffset, dataOffset);", _classObject.PrivateName);
			}
			else
			{
				output.WriteLine("{0} {1} = new {0}(connectionString, tableName);", _classObject.Name, _classObject.PrivateName);
				output.WriteLine("FillFromReader({0}, connectionString, tableName, r, idOffset, dataOffset);", _classObject.PrivateName);
			}
			
			//			output.WriteLine("{0}.iD = r.GetInt32(idOffset);", _classObject.PrivateName);
			//			output.WriteLine("{0}.isSynced = true;", _classObject.PrivateName);
			//			output.WriteLine("{0}.isPlaceHolder = false;", _classObject.PrivateName);
			//			output.WriteLine();
			//
			//			rCount = -1;				// Use a counter to track reader indexes for objects.
			//
			//			output.WriteLine("//");
			//			output.WriteLine("// Parse Children From Database");
			//			output.WriteLine("//");
			//			// Output each OleDbDataReader.GetData[x] for each item in the object.
			//			for(int x = 0; x <= _classObject.Children.Count - 1; x++)
			//			{
			//				if(_classObject.Children[x].HasChildrenTables)
			//					continue;
			//
			//				rCount++;
			//				
			//				output.Indent++;
			//				if(_classObject.Children[x].IsTableCoded)
			//				{
			//					output.WriteLine("if(!r.IsDBNull({0}+dataOffset) && r.GetInt32({0}+dataOffset) != 0)", rCount);
			////					output.WriteLine("if(r.GetInt32({0}+dataOffset) != 0)", rCount);
			//
			//					output.Indent++;
			//
			//					//
			//					// If the Child's DataType does not have coded tables then hard code the table here.
			//					//
			//					if(!_classObject.Children[x].DataType.IsTableCoded)
			//						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, \"{3}\", r.GetInt32({4}+dataOffset));",
			//							_classObject.PrivateName,
			//							_classObject.Children[x].PrivateName,
			//							_classObject.Children[x].DataType.Name,
			//							_classObject.Children[x].TableName,
			//							rCount);
			//					else
			//						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, r.GetInt32({3}+dataOffset));",
			//							_classObject.PrivateName,
			//							_classObject.Children[x].PrivateName,
			//							_classObject.Children[x].DataType.Name,
			//							rCount);
			//					output.Indent--;
			//				}
			//				else
			//				{
			//					//
			//					// Add an one to the offset counter to point to the ID instead of the table name... makes for cleaner code.
			//					output.WriteLine("if(!r.IsDBNull({0}+dataOffset) && r.GetInt32({0}+dataOffset) != 0)", rCount + 1);
			////					output.WriteLine("if(r.GetInt32({0}+dataOffset) != 0)", rCount + 1);
			//					output.Indent++;
			//
			//					rCount++;
			//					output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, r.GetString({3}+dataOffset), r.GetInt32({4}+dataOffset));",
			//						_classObject.PrivateName,
			//						_classObject.Children[x].PrivateName,
			//						_classObject.Children[x].DataType.Name,
			//						rCount-1,
			//						rCount);
			//					output.Indent--;
			//				}
			//				output.Indent--;
			//			}
			//			output.WriteLine();
			//
			//			output.WriteLine("//");
			//			output.WriteLine("// Parse Fields From Database");
			//			output.WriteLine("//");
			//			for(int x = 0; x <= _classObject.Fields.Count - 1; x++)
			//			{
			//				rCount++;				
			//				if(_classObject.Fields[x].IsNullable)
			//				{
			//					output.WriteLine("if(!r.IsDBNull({0}+dataOffset)) ", rCount);
			//					output.Indent++;
			//					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
			//						_classObject.Fields[x].PrivateName, 
			//						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount, "dataOffset"));
			//					output.Indent--;
			//
			//					//
			//					// Set the default value or null value
			//					//
			//					if(_classObject.Fields[x].UseDefaultValueOnNull)
			//					{
			//						output.WriteLine("else");
			//						output.Indent++;
			//						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
			//							_classObject.Fields[x].PrivateName,
			//							_classObject.Fields[x].DefaultValue);
			//						output.Indent--;
			//					}
			//					else if(_classObject.Fields[x].DataType.NullValue != string.Empty)
			//					{
			//						output.WriteLine("else");
			//						output.Indent++;
			//						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
			//							_classObject.Fields[x].PrivateName,
			//							_classObject.Fields[x].DataType.NullValue);
			//						output.Indent--;
			//					}				
			//				}	
			//				else
			//				{
			//					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
			//						_classObject.Fields[x].PrivateName, 
			//						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount, "dataOffset"));
			//				}
			//				output.WriteLine();
			//			}
			//			output.WriteLine();

			output.WriteLine("return {0};", _classObject.PrivateName);
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Fill From Reader Methods

			output.WriteLine("#region Default DbModel FillFromReader Method");
			output.WriteLine();

			output.WriteXmlSummary("Fills the {0} from a SqlDataReader.");
			if(_classObject.IsTableCoded)
				output.WriteLine("public static void FillFromReader({0} {1}, string connectionString, " +
					"SqlDataReader r, int idOffset, int dataOffset)", _classObject.Name, _classObject.PrivateName);
			else
				output.WriteLine("public static void FillFromReader({0} {1}, string connectionString, string tableName," +
					"SqlDataReader r, int idOffset, int dataOffset)", _classObject.Name, _classObject.PrivateName);
			output.WriteLine("{");
			output.Indent++;

			//
			// Create new object...
			//
			if(_classObject.IsTableCoded)
				output.WriteLine("{0}.connectionString = connectionString;", _classObject.PrivateName, _classObject.Name);
			else
			{
				output.WriteLine("{0}.connectionString = connectionString;", _classObject.PrivateName);
				output.WriteLine("{0}.tableName = tableName;", _classObject.PrivateName);
			}
			output.WriteLine("{0}.iD = r.GetInt32(idOffset);", _classObject.PrivateName);
			output.WriteLine("{0}.isSynced = true;", _classObject.PrivateName);
			output.WriteLine("{0}.isPlaceHolder = false;", _classObject.PrivateName);
			output.WriteLine();

			rCount = -1;				// Use a counter to track reader indexes for objects.

			output.WriteLine("//");
			output.WriteLine("// Parse Children From Database");
			output.WriteLine("//");
			// Output each SqlDataReader.GetData[x] for each item in the object.
			for(int x = 0; x <= _classObject.Children.Count - 1; x++)
			{
				if(_classObject.Children[x].HasChildrenTables)
					continue;

				rCount++;

				// Be sure that the manager does not create a placeholder for '0' or '-1'
				output.WriteLine("if(!r.IsDBNull({0}+dataOffset) && r.GetInt32({0}+dataOffset) > 0)", rCount);
				output.Indent++;

				if(_classObject.Children[x].IsTableCoded)
				{
					//
					// If the Child's DataType does not have coded tables then hard code the table here.
					//
					if(!_classObject.Children[x].DataType.IsTableCoded)
						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, \"{3}\", r.GetInt32({4}+dataOffset));",
							_classObject.PrivateName,
							_classObject.Children[x].PrivateName,
							_classObject.Children[x].DataType.Name,
							_classObject.Children[x].TableName,
							rCount);
					else
						output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, r.GetInt32({3}+dataOffset));",
							_classObject.PrivateName,
							_classObject.Children[x].PrivateName,
							_classObject.Children[x].DataType.Name,
							rCount);
				}
				else
				{
					rCount++;
					output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(connectionString, r.GetString({3}+dataOffset), r.GetInt32({4}+dataOffset));",
						_classObject.PrivateName,
						_classObject.Children[x].PrivateName,
						_classObject.Children[x].DataType.Name,
						rCount-1,
						rCount);
				}

				output.Indent--;
			}
			output.WriteLine();

			output.WriteLine("//");
			output.WriteLine("// Parse Fields From Database");
			output.WriteLine("//");
			for(int x = 0; x <= _classObject.Fields.Count - 1; x++)
			{
				rCount++;
				if(_classObject.Fields[x].IsNullable)
				{
					output.WriteLine("if(!r.IsDBNull({0}+dataOffset)) ", rCount);
					output.Indent++;
					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
						_classObject.Fields[x].PrivateName, 
						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount, "dataOffset"));
					output.Indent--;

					//
					// Set the default value or null value
					//
					if(_classObject.Fields[x].UseDefaultValueOnNull)
					{
						output.WriteLine("else");
						output.Indent++;
						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
							_classObject.Fields[x].PrivateName,
							_classObject.Fields[x].DefaultValue);
						output.Indent--;
					}
					else if(_classObject.Fields[x].DataType.NullValue != string.Empty)
					{
						output.WriteLine("else");
						output.Indent++;
						output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName,
							_classObject.Fields[x].PrivateName,
							_classObject.Fields[x].DataType.NullValue);
						output.Indent--;
					}
				}	
				else
				{
					output.WriteLine("{0}.{1} = {2};", _classObject.PrivateName, 
						_classObject.Fields[x].PrivateName, 
						_classObject.Fields[x].DataType.MakeReaderMethod("r", rCount, "dataOffset"));
				}
				output.WriteLine();
			}
			output.WriteLine();

			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Fill Children Methods

			output.WriteLine("#region Default DbModel Fill Methods");
			output.WriteLine();

			foreach(ChildEntry c in _classObject.Children)
			{
				if(!c.HasChildrenTables)
					continue;

				output.WriteLine("internal static void Fill{0}({1} _{1})", c.Name, _classObject.Name);
				output.WriteLine("{");
				output.Indent++;

				if(_classObject.IsTableCoded)
				{ 
					if(_classObject.Name == c.DataType.Name)
						output.WriteLine("StringBuilder s = new StringBuilder(\"SELECT {0}ChildID FROM {1}Children_{2} \");",
							c.DataType.Name, _classObject.DefaultTableName, c.ColumnName);
					else
						output.WriteLine("StringBuilder s = new StringBuilder(\"SELECT {0}ID FROM {1}Children_{2} \");",
							c.DataType.Name, _classObject.DefaultTableName, c.ColumnName);
				}
				else
				{
					if(_classObject.Name == c.DataType.Name)
						output.WriteLine("StringBuilder s = new StringBuilder(\"SELECT {0}ChildID FROM \");",
							c.DataType.Name);
					else
						output.WriteLine("StringBuilder s = new StringBuilder(\"SELECT {0}ID FROM \");",
							c.DataType.Name);
					output.WriteLine("s.Append(_{0}.tableName);", _classObject.Name);
					output.WriteLine("s.Append(\"Children_{0}\");", c.ColumnName);
				}

				output.WriteLine("s.Append(\"WHERE {0}ID=\");", _classObject.Name);
				output.WriteLine("s.Append(_{0}.iD);", _classObject.Name);
				output.WriteLine("s.Append(\";\");");
				output.WriteLine();

				output.WriteLine("SqlConnection dbConnection = new SqlConnection(_{0}.connectionString);", _classObject.Name);
				output.WriteLine("SqlCommand dbCommand = new SqlCommand(s.ToString(), dbConnection);");
				output.WriteLine("dbConnection.Open();");
				output.WriteLine("SqlDataReader r = dbCommand.ExecuteReader();");
				output.WriteLine();
				output.WriteLine("{0}Collection {1};", c.DataType.Name, c.PrivateName);
				output.WriteLine("if(_{0}.{1} != null)", _classObject.Name, c.PrivateName);
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("{0} = _{1}.{0};", c.PrivateName, _classObject.Name);
				output.WriteLine("{0}.Clear();", c.PrivateName);
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine("else");
				output.WriteLine("{");
				output.Indent++;
				output.WriteLine("{0} = new {1}Collection();", c.PrivateName, c.DataType.Name);
				output.WriteLine("_{0}.{1} = {1};", _classObject.Name, c.PrivateName);
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();

				if(c.EntryType == ChildEntryType.Array)
				{
					output.WriteLine("{0}[] {1}Array = new {0}[r.GetInt32(1)];", c.DataType.Name, c.PrivateName);
					output.WriteLine("int rowNum = 0;");
					output.WriteLine("do");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("{0}Array[rowNum] = {1}.NewPlaceHolder(_{2}.connectionString, r.GetInt32(0));",
						c.PrivateName, c.DataType.Name, _classObject.Name);
					output.WriteLine("rowNum++;");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine("while(r.Read());");
					output.WriteLine();
					output.WriteLine("dbConnection.Close();");
					output.WriteLine("_{0}.{1} = {1}Array;", _classObject.Name,
						c.PrivateName);
				}
				else if(c.EntryType == ChildEntryType.Collection)
				{
					output.WriteLine("while(r.Read())");
					output.Indent++;
					if(c.DataType.IsTableCoded)
						output.WriteLine("{0}.Add({1}.NewPlaceHolder(_{2}.connectionString, r.GetInt32(0)));",
							c.PrivateName, c.DataType.Name, _classObject.Name);
					else if(c.IsTableCoded)
						output.WriteLine("{0}.Add({1}.NewPlaceHolder(_{2}.connectionString, \"{3}\", r.GetInt32(0)));",
							c.PrivateName, c.DataType.Name, _classObject.Name, c.TableName);
					output.Indent--;
					output.WriteLine();
					output.WriteLine("r.Close();");
					output.WriteLine("dbConnection.Close();");
					output.WriteLine("_{0}.{1} = {2};", _classObject.Name,
						c.Name, c.PrivateName);
				}				

				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();
			}

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Delete

			output.WriteLine("#region Default DbModel Delete Method");
			output.WriteLine();

			// Write Delete Method
			if(_classObject.IsTableCoded)
				output.WriteLine("internal static void _delete(string connectionString, int id)");
			else
				output.WriteLine("internal static void _delete(string connectionString, string tableName, int id)");
            			
			output.WriteLine("{");
			output.Indent++;
			if(_classObject.IsTableCoded)
			{
				output.WriteLine("StringBuilder s = new StringBuilder(\"DELETE * FROM {0} WHERE {1}ID=\");", _classObject.DefaultTableName, _classObject.Name);
				output.WriteLine("s.Append(id);");
				output.WriteLine("s.Append(';');");
			}
			else
			{
				output.WriteLine("StringBuilder s = new StringBuilder(\"DELETE * FROM \");");
				output.WriteLine("s.Append(tableName);", _classObject.DefaultTableName);
				output.WriteLine("s.Append(\" WHERE {0}ID=\");", _classObject.Name);
				output.WriteLine("s.Append(id);");
				output.WriteLine("s.Append(';');");
			}

			output.WriteLine();
			output.WriteLine("SqlConnection dbConnection = new SqlConnection(connectionString);");
			output.WriteLine("SqlCommand dbCommand = new SqlCommand(s.ToString(), dbConnection);");
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("dbCommand.ExecuteNonQuery();");

			// Update children array relationships, not the actual objects
			foreach(ChildEntry c in _classObject.Children)
			{
				// NOTE! ITEMS SHOULD BE SAVED WITH VALID ID'S BEFORE THIS OCCURS!
				// Call the save method on the object if the object is not saved.
				// Think of somehow throwing an exception for objects in the array that
				// do not have an id, perhaps this is not so easy.

				if(!c.HasChildrenTables)
					continue;

				output.WriteLine();		
		
				if(_classObject.IsTableCoded)
				{
					// Delete existing rows.
					output.WriteLine("// Delete child relationships for {0}.", c.Name);
					output.WriteLine("s.Length = 0;");
					output.WriteLine("s.Append(\"DELETE * FROM {0}Children_{1} WHERE \");",
						_classObject.DefaultTableName, c.ColumnName);
					output.WriteLine("s.Append(\"{0}ID=\");", _classObject.Name);
					output.WriteLine("s.Append(id);", _classObject.Name);
					output.WriteLine("s.Append(\";\");");
					output.WriteLine("dbCommand.CommandText = s.ToString();");
					output.WriteLine("dbCommand.ExecuteNonQuery();");
				}
				else
				{
					// TODO: Output code that depends on dynamic tables.
				}
			}

			output.WriteLine();
			output.WriteLine("dbConnection.Close();");
			output.WriteLine();

			if(_classObject.IsCachingEnabled)
			{
				if(_classObject.IsTableCoded)
					output.WriteLine("cacheRemove(id);");
				else
					output.WriteLine("cacheRemove(id, tableName);");
			}
			

			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Create Table

			output.WriteLine("#region Default DbModel Create Table Methods");
			output.WriteLine();

			//
			// Write Create Table References Method
			//
			if(_classObject.Children.LastOneToManyRelation != -1)
			{
				if(_classObject.IsTableCoded)
					output.WriteLine("public static void CreateTableReferences(string connectionString)");
				else
					output.WriteLine("public static void CreateTableReferences(string connectionString, string tableName)");
				output.WriteLine("{");
				output.Indent++;

				if(_classObject.IsTableCoded)
				{
					output.WriteLine("StringBuilder query = new StringBuilder(\"ALTER TABLE {0} ADD \");", _classObject.DefaultTableName);
				}
				else
				{
					output.WriteLine("StringBuilder query = new StringBuilder(\"ALTER TABLE \");");
					output.WriteLine("query.Append(tableName);");
					output.WriteLine("query.Append(\" ADD \");");
				}

				int lastRelation = -1;
				for(int x = 0; x < _classObject.Children.Count; x++)
					if(_classObject.Children[x].IsTableCoded | !_classObject.Children[x].HasChildrenTables)
						lastRelation = x;

				for(int x = 0; x < _classObject.Children.Count; x++)
				{
					ChildEntry c = _classObject.Children[x];

					if(!c.IsTableCoded | c.HasChildrenTables)
						continue;

					if(_classObject.IsTableCoded)
					{
						output.Write("query.Append(\" CONSTRAINT {0}_{1}_FK FOREIGN KEY ({1}ID) REFERENCES {2}({3}ID)",
							_classObject.DefaultTableName, c.Name, c.TableName, c.DataType.Name);
					}
					else
					{
						output.WriteLine("query.Append(\" CONSTRAINT \");");
						output.WriteLine("query.Append(tableName);");
						output.Write("query.Append(\"_{0}_FK FOREIGN KEY ({0}ID) REFERENCES {1}({2}ID)",
							c.Name, c.TableName, c.DataType.Name);
					}
					if(x == lastRelation)
						output.WriteLine(";\");");
					else
						output.WriteLine(",\");");
				}			

				output.WriteLine("SqlConnection dbConnection = new SqlConnection(connectionString);");
				output.WriteLine("SqlCommand dbCommand = new SqlCommand(query.ToString(), dbConnection);");
				output.WriteLine("dbConnection.Open();");
				output.WriteLine("dbCommand.ExecuteNonQuery();");
				output.WriteLine();

				foreach(ChildEntry c in _classObject.Children)
				{
					if(!c.HasChildrenTables)
						continue;

					output.WriteLine("query.Length = 0;");
				
					if(_classObject.IsTableCoded)
						output.WriteLine("query.Append(\"ALTER TABLE {0}Children_{1} \");", _classObject.DefaultTableName, c.ColumnName);
					else
					{
						output.WriteLine("query.Append(\"ALTER TABLE \");");
						output.WriteLine("query.Append(tableName);");
						output.WriteLine("query.Append(\"Children_{0} \");", c.ColumnName);
					}

					if(_classObject.IsTableCoded)
					{
						if(c.IsTableCoded)
						{
							output.WriteLine("query.Append(\" ADD CONSTRAINT {0}Children_{2}_{0}_FK FOREIGN KEY ({0}ID) REFERENCES {1}({0}ID), CONSTRAINT {0}Children_{2}_{4}_FK FOREIGN KEY ({4}ID) REFERENCES {3}({4}ID);\");", _classObject.Name, _classObject.DefaultTableName, c.Name, c.TableName, c.DataType.Name);
							output.WriteLine("dbCommand.CommandText = query.ToString();");
							output.WriteLine("dbCommand.ExecuteNonQuery();");
							output.WriteLine();
						}
						else
						{
							output.WriteLine("query.Append(\" ADD CONSTRAINT {0}{2}_FK FOREIGN KEY ({0}ID) REFERENCES {1}({0}ID);\");", _classObject.Name, _classObject.DefaultTableName, c.Name, c.TableName, c.DataType.Name);
							output.WriteLine("dbCommand.CommandText = query.ToString();");
							output.WriteLine("dbCommand.ExecuteNonQuery();");
							output.WriteLine();
						}
					}
					else
					{
						output.WriteLine("query.Append(\" ADD CONSTRAINT {0}{2}{4}_FK FOREIGN KEY ({4}ID) REFERENCES {3}({4}ID);\");", _classObject.Name, _classObject.DefaultTableName, c.Name, c.TableName, c.DataType.Name);
						output.WriteLine("dbCommand.CommandText = query.ToString();");
						output.WriteLine("dbCommand.ExecuteNonQuery();");
						output.WriteLine();
					}
				}

				output.WriteLine("dbConnection.Close();");

				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();
			}

			//
			// Write Table Create Method
			//
			if(_classObject.IsTableCoded)
				output.WriteLine("public static void CreateTable(string connectionString)");
			else
				output.WriteLine("public static void CreateTable(string connectionString, string tableName)");
			output.WriteLine("{");
			output.Indent++;
			if(_classObject.IsTableCoded)
			{
				output.WriteLine("StringBuilder query = new StringBuilder(\"CREATE TABLE {0} \");", _classObject.DefaultTableName);
			}
			else
			{
				output.WriteLine("StringBuilder query = new StringBuilder(\"CREATE TABLE \");");
				output.WriteLine("query.Append(tableName);");
			}
			
			output.WriteLine("query.Append(\" ({0}ID COUNTER(1,1) CONSTRAINT {0}ID PRIMARY KEY, \" + ", _classObject.Name);
			output.Indent++;

			foreach(ChildEntry c in _classObject.Children)
			{
				if(c.HasChildrenTables)
					continue;
				if(!c.IsTableCoded)
					output.WriteLine("\"{0}Table TEXT(255),\" +", c.Name);
				output.Write("\"{0}ID LONG", c.Name);
				//				if(c.IsTableCoded)
				//					output.Write(" CONSTRAINT Foreign{0}Ref REFERENCES {1}", c.Name, c.TableName);
				output.WriteLine(",\" +");
			}

			if(_classObject.Fields.Count > 0)
				for(int x = 0; x < _classObject.Fields.Count; x++)
				{
					output.Write("\"");
					output.Write(_classObject.Fields[x].ColumnName);
					output.Write(" ");
					output.Write(string.Format(_classObject.Fields[x].DataType.DbType, _classObject.Fields[x].Length));
					
					if(_classObject.Fields[x].IsUnique)
					{
						output.Write(" CONSTRAINT ");
						if(_classObject.IsTableCoded)
						{
							output.Write("Unique");
							output.Write(_classObject.Fields[x].Name);
							output.Write(" UNIQUE");
						}
						else
						{
							output.WriteLine("\" + ");
							output.WriteLine("tableName +");
							output.Write("\"");
							output.Write(_classObject.Fields[x].Name);
							output.Write(" UNIQUE");
						}
					}

					if(x != _classObject.Fields.Count - 1)
						output.WriteLine(", \" +");
					else
						output.WriteLine(");\");");
				}

			output.Indent--;
			output.WriteLine();

			output.WriteLine("SqlConnection dbConnection = new SqlConnection(connectionString);");
			output.WriteLine("SqlCommand dbCommand = new SqlCommand(query.ToString(), dbConnection);");
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("dbCommand.ExecuteNonQuery();");
			output.WriteLine();

			foreach(ChildEntry c in _classObject.Children)
			{
				// Create Object Level Children Tables
				if(c.IsTableCoded & !c.DataType.IsTableCoded)
				{					
					output.WriteLine("//");
					output.WriteLine("// Create object level table for {0}.", c.Name);
					output.WriteLine("//");
					output.WriteLine("{0}Manager.CreateTable(connectionString, \"{1}\");",
						c.DataType.Name, c.TableName);
					output.WriteLine();
				}

				if(!c.HasChildrenTables)
					continue;

				output.WriteLine("//");
				output.WriteLine("// Create children table for {0}.", c.Name);
				output.WriteLine("//");
				output.WriteLine("query.Length = 0;");
				if(_classObject.IsTableCoded)
					output.WriteLine("query.Append(\"CREATE TABLE {0}Children_{1} \");", _classObject.DefaultTableName, c.ColumnName);
				else
				{
					output.WriteLine("query.Append(\"CREATE TABLE \");");
					output.WriteLine("query.Append(tableName);");
					output.WriteLine("query.Append(\"Children_{0} \");", c.ColumnName);
				}
				if(_classObject.Name == c.DataType.Name)
					output.WriteLine("query.Append(\"({0}ID LONG, {1}ChildID LONG);\");", _classObject.Name, c.DataType.Name);
				else
					output.WriteLine("query.Append(\"({0}ID LONG, {1}ID LONG);\");", _classObject.Name, c.DataType.Name);


				output.WriteLine("dbCommand.CommandText = query.ToString();");
				output.WriteLine("dbCommand.ExecuteNonQuery();");
				output.WriteLine();
			}			

			output.WriteLine("dbConnection.Close();");
			
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			output.WriteLine("#endregion");
			output.WriteLine();

			#endregion

			#region Delete Tables

			

			#endregion

			#region Cache Methods

			if(_classObject.IsCachingEnabled)
			{
				output.WriteLine("private static void cacheStore(<.C> <.c>)");
				output.WriteLine("{");
				output.Indent++;
				if(!_classObject.IsTableCoded)
				{
					output.WriteLine("<.C>Cache cache;");
					output.WriteLine("if(caches[<.c>.TableName] == null)");
					output.WriteLine("{");
					output.WriteLine("\tcache = new <.C>Cache();");
					output.WriteLine("\tcaches[<.c>.TableName] = cache;");
					output.WriteLine("\tcache.Add(<.c>.IsolateClone(), TimeSpan.FromMinutes(3));");
					output.WriteLine("\treturn;");
					output.WriteLine("}");
					output.WriteLine("else");
					output.WriteLine("\tcache = (<.C>Cache) caches[<.c>.TableName];");
				}
				output.WriteLine("// If the new object is already in the cache, replace the old object");
				output.WriteLine("// with the new one.");
				output.WriteLine("cache.CheckedAdd(<.c>.IsolateClone(), TimeSpan.FromMinutes(3));");
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();

				if(_classObject.IsTableCoded)
					output.WriteLine("private static <.C> cacheFind(int id)");
				else
					output.WriteLine("private static <.C> cacheFind(int id, string tableName)");
				output.WriteLine("{");
				output.Indent++;
				if(!_classObject.IsTableCoded)
				{
					output.WriteLine("if(caches[tableName] == null)");
					output.WriteLine("\treturn null;");
					output.WriteLine("<.C>Cache cache;");
					output.WriteLine("cache = (<.C>Cache) caches[tableName];");
				}
				output.WriteLine("return cache[id];");
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();

				if(_classObject.IsTableCoded)
					output.WriteLine("private static void cacheRemove(int id)");
				else
					output.WriteLine("private static void cacheRemove(int id, string tableName)");
				output.WriteLine("{");
				output.Indent++;
				if(!_classObject.IsTableCoded)
				{
					output.WriteLine("if(caches[tableName] == null)");
					output.WriteLine("\treturn;");
					output.WriteLine("<.C>Cache cache;");
					output.WriteLine("cache = (<.C>Cache) caches[tableName];");					
				}
				output.WriteLine("cache.Remove(id);");
				output.Indent--;
				output.WriteLine("}");
				output.WriteLine();
			}

			#endregion

			if(CustomCode != null && CustomCode.Length > 0)
			{
				output.WriteLine("//--- Begin Custom Code ---");
				output.Write(CustomCode);
				output.WriteLine("//--- End Custom Code ---");
			}
			                        
			// End Class Declaration
			output.Indent--;
			output.WriteLine("}");

			// End Namespace Declaration
			output.Indent--;
			output.WriteLine("}");
			output.WriteLine();

			return output.ToString();
		}

		#endregion

		private void genSqlReader(CodeWriter output, string sqlCommandField, string readerField, string connectionString)
		{			
			output.WriteLine("SqlConnection dbConnection = new SqlConnection({0});", connectionString);
			output.WriteLine("SqlCommand dbCommand = new SqlCommand({0}, dbConnection);", sqlCommandField);
			output.WriteLine("dbConnection.Open();");
			output.WriteLine("SqlDataReader {0} = dbCommand.ExecuteReader();", readerField);
		}

		private void genSqlCommand(CodeWriter output, string sqlCommandField, bool returnValue, bool returnIdentity, string connectionString)
		{
			output.WriteLine("SqlConnection dbConnection = new SqlConnection({0});", connectionString);
			output.WriteLine("SqlCommand dbCommand = new SqlCommand({0}, dbConnection);", sqlCommandField);
			output.WriteLine("dbConnection.Open();");
			
			if(returnValue)
			{
				if(returnIdentity)
				{
					output.WriteLine("try");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("dbCommand.ExecuteNonQuery();");
					output.WriteLine("dbCommand.CommandText = \"SELECT @@IDENTITY AS IDVal\";");
					output.WriteLine("dbCommand.CommandType = CommandType.Text;");
					output.WriteLine("return (int) dbCommand.ExecuteScalar();");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine("catch (SqlException e)");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("throw(e);");
					output.Indent--;
					output.WriteLine("}");
					output.WriteLine("finally");
					output.WriteLine("{");
					output.Indent++;
					output.WriteLine("dbConnection.Close();");
					output.Indent--;
					output.WriteLine("}");
				
					return;
				}
				
				output.WriteLine("int rowsAffected = dbCommand.ExecuteNonQuery();");
				output.WriteLine("dbConnection.Close();");				
				output.WriteLine("return rowsAffected;");
				
				return;				
			}

			output.WriteLine("dbCommand.ExecuteNonQuery();");
			output.WriteLine("dbConnection.Close();");
		}

		private void instantiateDbCommand(CodeWriter output, string connectionString)
		{
			output.WriteLine("SqlConnection dbConnection = new SqlConnection({0});", connectionString);
			output.WriteLine("SqlCommand dbCommand = new SqlCommand();");
			output.WriteLine("dbCommand.Connection = dbConnection;");
			output.WriteLine("dbConnection.Open();");
		}

		private void executeDbCommand(CodeWriter output, string sqlCommandField)
		{
			output.WriteLine("dbCommand.CommandText = \"{0}\";", sqlCommandField);
			output.WriteLine("dbCommand.ExecuteNonQuery();");
		}

		private void closeDbConnection(CodeWriter output)
		{
			output.WriteLine("dbConnection.Close();");
		}
	}
}