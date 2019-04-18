using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NitroCast.Core.Extensions
{
    [TypeConverter(typeof(TypeConverters.ReferenceTypeBuilderConverter))]
    public class ReferenceTypeBuilder : Extension
    {
        private static ReferenceTypeBuilder defaultBuilder;
        private static Dictionary<string, ReferenceTypeBuilder> builders;

        public static Dictionary<string, ReferenceTypeBuilder> Builders
        {
            get { return builders; }
        }

        public static ReferenceTypeBuilder Default
        {
            get { return defaultBuilder; }
        }

        static ReferenceTypeBuilder()
        {
            builders = new Dictionary<string, ReferenceTypeBuilder>();
            
            defaultBuilder = new ReferenceTypeBuilder();
            defaultBuilder.Name = "Default";
            defaultBuilder.FullName = "";
            defaultBuilder.Description = "Default builder for reference types.";
            defaultBuilder.Copyright = "Copyright © 2007 Roy A.E. Hodges";
            defaultBuilder.ExtensionPath = string.Empty;
            defaultBuilder.Author = "Roy A.E. Hodges";
            defaultBuilder.AssemblyPath = string.Empty;
            defaultBuilder.OutputFileNameFormat = string.Empty;
            builders.Add(defaultBuilder.Name, defaultBuilder);
        }

        public override string ToString()
        {
            return Name;
        }

        #region virtual CreateClassField

        public virtual void CreateClassField(CodeWriter output,
            ReferenceField f, bool isInternal, bool instantiate)
        {
            if (f.IsArray)
            {
                output.WriteLine(
                    (isInternal ? "internal" : "private") + " {0}[] {1}" + 
                    (instantiate ? " = new {0}[];" : ";"),
                    f.ReferenceType.Name,
                    f.PrivateName);
            }
            else if (f.IsCollection)
            {
                output.WriteLine(
                    (isInternal ? "internal" : "private") + " {0}Collection {1}" +
                    (instantiate ? " = new {0}Collection;" : ";"),
                    f.ReferenceType.Name,
                    f.PrivateName);
            }
            else
            {
                output.WriteLine(
                    (isInternal ? "internal" : "private") + "{0} {1}" +
                    (instantiate ? " = new {0};" : ";"),
                    f.ReferenceType.Name,
                    f.PrivateName);
            }
        }

        #endregion

        #region virtual void CreateControlField

        public virtual void CreateControlField(CodeWriter output,
            ReferenceField f, bool instantiate)
        {
            if (f.IsClientEditEnabled)
            {
                if (!f.HasChildrenTables)
                {
                    output.WriteLine("private DropDownList dd{0}" +
                        (instantiate ? " = new DropDownList();" : ";"),
                        f.Name);
                }
                else
                {
                    output.WriteLine("private CheckBoxList cbl{0}" +
                        (instantiate ? " = new CheckBoxList();" : ";"),
                        f.Name);
                }
            }
            else if (f.IsClientViewEnabled)
            {
                output.WriteLine("private Literal lt{0}" +
                    (instantiate ? " = new Literal();" : ";"), 
                    f.Name);
            }
        }

        #endregion

        #region virtual void GetControlValue

        public virtual void GetControlValue(CodeWriter output,
            string className, ReferenceField f)
        {
            if (f.IsClientEditEnabled)
            {
                if (!f.HasChildrenTables)   // <---- Drop Down List
                {
                    output.WriteLine("if(dd{0}.SelectedItem != null && dd{0}.SelectedValue != \"null\")",
                        f.Name);
                    output.WriteLine("{");
                    output.Indent++;
                    if (f.ReferenceType.IsTableCoded)
                        output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(" +
                            "int.Parse(dd{1}.SelectedValue));",
                            className, f.Name, f.ReferenceType.Name);
                    else if (f.IsTableCoded)
                        output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(\"{3}\", " +
                            "int.Parse(dd{1}.SelectedValue));",
                            className, f.Name, f.ReferenceType.Name, f.TableName);
                    else
                        output.WriteLine("{0}.{1} = {2}.NewPlaceHolder({0}.{1}Table, int.Parse(dd{1}.SelectedValue));",
                            className, f.Name, f.ReferenceType.Name, f.TableName);
                    output.Indent--;
                    output.WriteLine("}");
                    output.WriteLine("else");
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine("{0}.{1} = null;", className, f.Name);
                    output.Indent--;
                    output.WriteLine("}");
                }
                else                        // <---- Check Box List
                {
                    output.WriteLine("{0}.{1} = new {2}Collection();",
                        className, f.Name, f.ReferenceType.Name);
                    output.WriteLine("foreach(ListItem i in cbl{0}.Items)", f.Name);
                    output.Indent++;
                    output.WriteLine("if(i.Selected)");
                    output.Indent++;
                    if (f.ReferenceType.IsTableCoded)
                        output.WriteLine("{0}.{1}.Add({2}.NewPlaceHolder(int.Parse(i.Value)));",
                        className, f.Name, f.ReferenceType.Name);
                    else if (f.IsTableCoded)
                        output.WriteLine("{0}.{1}.Add({2}.NewPlaceHolder(\"{3}\", int.Parse(i.Value)));",
                        className, f.Name, f.ReferenceType.Name, f.TableName);
                    else
                        output.WriteLine("{0}.{1}.Add({2}.NewPlaceHolder({0}.{1}Table, int.Parse(i.Value)));",
                        className, f.Name, f.ReferenceType.Name);
                    output.Indent--;
                    output.Indent--;
                    output.WriteLine("if({0}.{1}.Count == 0)", className, f.Name);
                    output.Indent++;
                    output.WriteLine("{0}.{1} = null;", className, f.Name);
                    output.Indent--;
                    output.WriteLine();
                }
            }
        }

        #endregion

        #region virtual void SetControlValue

        public virtual void SetControlValue(CodeWriter output,
            string className, ReferenceField f)
        {
            if (f.IsClientEditEnabled)
            {
                if (f.ReferenceMode == ReferenceMode.Normal)
                {
                    output.WriteLine("if ({0}.{1} != null)", className, f.Name);
                    output.Indent++;
                    output.WriteLine("foreach(ListItem item in dd{0}.Items)", f.Name);
                    output.Indent++;
                    output.WriteLine("item.Selected = {0}.{1}.ID.ToString() == item.Value;",
                        className, f.Name);
                    output.Indent--;
                    output.Indent--;
                    output.WriteLine("else if (dd{0}.Items.Count > 0)", f.Name);
                    output.Indent++;
                    output.WriteLine("dd{0}.SelectedIndex = 0;", f.Name);
                    output.Indent--;
                    output.WriteLine();
                }
                else if(f.ReferenceMode == ReferenceMode.Collection)
                {
                    output.WriteLine("if ({0}.{1} != null)",
                        className, f.Name);
                    output.Indent++;
                    output.WriteLine("foreach({0} objItem in {1}.{2})",
                        f.ReferenceType.Name, className, f.Name);
                    output.Indent++;
                    output.WriteLine("foreach(ListItem item in cbl{0}.Items)",
                        f.Name);
                    output.Indent++;
                    output.WriteLine("if(item.Value == objItem.ID.ToString())");
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine("item.Selected = true;");
                    output.WriteLine("break;");
                    output.Indent--;
                    output.WriteLine("}");
                    output.Indent--;
                    output.Indent--;
                    output.Indent--;
                }
            }
            else if(f.IsClientViewEnabled)
            {
                output.WriteLine("lt{0}.Text = {1}.{0}.ToString();",
                    f.Name, className);
            }
        }

        #endregion

        public virtual void RenderControl(CodeWriter output, ReferenceField f)
        {
            if (f.IsClientEditEnabled)
            {
                if (!f.HasChildrenTables)
                {
                    output.WriteLine("dd{0}.RenderControl(output);", f.Name);
                }
                else
                {
                    output.WriteLine("cbl{0}.RenderControl(output);", f.Name);
                }
            }
            else if (f.IsClientViewEnabled)
            {
                output.WriteLine("lt{0}.RenderControl(output);", f.Name);
            }
        }

        #region virtual void CreateControlProperties

        public virtual void CreateControlProperties(CodeWriter output,
            ReferenceField f, bool instantiate, bool enableViewState)
        {
            CreateControlProperties(output, f, instantiate, enableViewState,
                string.Empty);
        }

        public virtual void CreateControlProperties(CodeWriter output,
            ReferenceField f, bool instantiate, bool enableViewState,
            string addControlFormat)
        {
            if (f.IsClientEditEnabled)
            {
                if (f.ReferenceMode == ReferenceMode.Normal)
                {
                    output.WriteLine("dd{0} = new DropDownList();", f.Name);
                    output.WriteLine("dd{0}.ID = \"dd{0}\";", f.Name);
                    if (!enableViewState)
                        output.WriteLine("dd{0}.EnableViewState = false;", f.Name);

                    if (addControlFormat.Length > 0)
                    {
                        output.WriteLine(addControlFormat, 
                            string.Format("dd{0}", f.Name),
                            f.Caption.Length > 0 ? f.Caption : f.Name);
                    }
                    else
                    {
                        output.Write("Controls.Add(dd{0});", f.Name);
                    }
                }
                else if(f.ReferenceMode == ReferenceMode.Collection)
                {
                    output.WriteLine("cbl{0} = new CheckBoxList();", f.Name);
                    output.WriteLine("cbl{0}.ID = \"dd{0}\";", f.Name);
                    if (!enableViewState)
                        output.WriteLine("cbl{0}.EnableViewState = false;", f.Name);

                    if (addControlFormat.Length > 0)
                    {
                        output.WriteLine(addControlFormat,
                            string.Format("cbl{0}", f.Name),
                            f.Caption.Length > 0 ? f.Caption : f.Name);
                    }
                    else
                    {
                        output.Write("Controls.Add(cbl{0});", f.Name);
                    }
                }
            }
            else if (f.IsClientViewEnabled)
            {
                if (instantiate)
                    output.WriteLine("lt{0} = new Literal();",
                        f.Name);
                if (addControlFormat.Length > 0)
                {
                    output.WriteLine(addControlFormat,
                        string.Format("lt{0}", f.Name),
                        f.Caption.Length > 0 ? f.Caption : f.Name);
                }
                else
                {
                    output.WriteLine("Controls.Add(lt{0});", f.Name);
                }
            }
        }

        #endregion

        #region virtual void CreateControlBinding

        /// <summary>
        /// Writes control bindings for a ReferenceField. This even takes into
        /// account similar references to reduce resource overhead. Be careful though
        /// only use this method if you are sure you won't be adding multiple items
        /// to controls on PostBacks. NitroCast Default Extensions avoid this by 
        /// selecting optimal viewstate settings.
        /// </summary>
        /// <param name="output">The CodeWriter to output to.</param>
        /// <param name="f">ReferenceField to use.</param>
        /// <param name="useTracking">Optionally use the reference list to avoid
        /// creating duplicate instances of Managers and collections that would
        /// effect performance and database queries.</param>
        public virtual void CreateControlBinding(CodeWriter output, ReferenceField f)
        {
            if (f.IsClientEditEnabled)
            {                
                if (f.ReferenceType.IsTableCoded)
                {
                    if (!output.ReferenceUsed(f))
                    {
                        output.WriteLine("{0}Manager {1}Manager = new {0}Manager();",
                            f.ReferenceType.Name,
                            f.ReferenceType.ParentClassEntry.PrivateName,
                            f.Name);

                        output.WriteLine("{0}Collection {1}Collection = " +
                            "{1}Manager.GetCollection(string.Empty, string.Empty);",
                            f.ReferenceType.Name,
                            f.ReferenceType.ParentClassEntry.PrivateName);

                        // Save Reference For Later :)~ Take that!
                        output.SaveReference(f,
                            f.ReferenceType.ParentClassEntry.PrivateName +
                            "Collection");
                    }
                }
                else
                {
                    if (!output.ReferenceUsed(f))
                    {
                        // BLECH! A Custom Manager, Blah BLAH!
                        output.WriteLine("{0}Manager {1}Manager = new {0}Manager(\"{2}\");",
                            f.ReferenceType.Name, f.PrivateName, f.TableName);

                        output.WriteLine("{0}Collection {1}Collection = " +
                            "{1}Manager.GetCollection(string.Empty, string.Empty);",
                            f.ReferenceType.Name, f.PrivateName);

                        // Save another pesky reference for later!
                        // Don't worry so much bud, it keeps track of tables!
                        output.SaveReference(f, f.PrivateName + "Collection");
                    }
                }

                if (f.ReferenceMode == ReferenceMode.Normal)
                {
                    if (f.AllowNull)
                        output.WriteLine("dd{0}.Items.Add(new ListItem(\"             \", \"null\"));", f.Name);
                    output.WriteLine("foreach({0} itemObject in {1})",
                        f.ReferenceType.Name, output.GetReference(f));
                    output.Indent++;
                    output.WriteLine("dd{0}.Items.Add(new ListItem(itemObject.ToString(), " +
                        "itemObject.ID.ToString()));", f.Name);
                    output.Indent--;
                    output.WriteLine();
                }
                else if (f.ReferenceMode == ReferenceMode.Collection)
                {
                    output.WriteLine("foreach({0} itemObject in {1})",
                        f.ReferenceType.Name, output.GetReference(f));
                    output.Indent++;
                    output.WriteLine("cbl{0}.Items.Add(new ListItem(itemObject.ToString(), " +
                        "itemObject.ID.ToString()));", f.Name);
                    output.Indent--;
                    output.WriteLine();
                }
            }
        }

        #endregion
    }
}
