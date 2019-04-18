using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NitroCast.Core.Extensions
{
    [TypeConverter(typeof(TypeConverters.EnumTypeBuilderConverter))]
    public class EnumTypeBuilder : Extension
    {
        private static EnumTypeBuilder defaultBuilder;
        private static Dictionary<string, EnumTypeBuilder> builders;

        public static Dictionary<string, EnumTypeBuilder> Builders
        {
            get { return builders; }
        }

        public static EnumTypeBuilder Default
        {
            get { return defaultBuilder; }
        }

        static EnumTypeBuilder()
        {
            builders = new Dictionary<string, EnumTypeBuilder>();
            
            defaultBuilder = new EnumTypeBuilder();
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
            EnumField f, bool isInternal)
        {            
                output.WriteLine(
                    (isInternal ? "internal" : "private") + "{0} {1};",
                    f.EnumType.Name,
                    f.PrivateName);
        }

        #endregion

        #region virtual void CreateControlField

        public virtual void CreateControlField(CodeWriter output,
            EnumField f, bool instantiate)
        {
            if (f.IsClientEditEnabled)
            {
                output.WriteLine("private DropDownList dd{0}" +
                    (instantiate ? " = new DropDownList();" : ";"),
                    f.Name);
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
            string className, EnumField f)
        {
            if (f.IsClientEditEnabled)
            {
                output.WriteLine("if(dd{0}.SelectedValue != string.Empty)", f.Name);
                output.Indent++;
                output.WriteLine("{0}.{1} = ({2})", className, f.Name, f.EnumType.Name);
                output.Indent++;
                output.WriteLine("Enum.Parse(typeof({0}), dd{1}.SelectedValue);", f.EnumType, f.Name);
                output.Indent--;
                output.Indent--;
            }
        }

        #endregion

        #region virtual void SetControlValue

        public virtual void SetControlValue(CodeWriter output,
            string className, EnumField c)
        {
            if (c.IsClientEditEnabled)
            {
                output.WriteLine("foreach(ListItem item in dd{0}.Items)", c.Name);
                output.Indent++;
                output.WriteLine("item.Selected = {0}.{1}.ToString() == item.Value;",
                    className, c.Name);
                output.Indent--;
                output.WriteLine();
            }
            else
            {
                output.WriteLine("lt{0}.Text = {1}.{0}.ToString();",
                    c.Name, className);
            }
        }

        #endregion

        #region virtual void CreateControlProperties

        public virtual void CreateControlProperties(CodeWriter output,
            EnumField c, bool instantiate, bool enableViewState)
        {
            CreateControlProperties(output, c, instantiate, enableViewState,
                string.Empty);
        }

        public virtual void CreateControlProperties(CodeWriter output,
            EnumField f, bool instantiate, bool enableViewState,
            string addControlFormat)
        {
            if (f.IsClientEditEnabled)
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
            else if(f.IsClientViewEnabled)
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
        /// Writes control bindings for a EnumField. This even takes into
        /// account similar references to reduce resource overhead.
        /// </summary>
        /// <param name="output">The CodeWriter to output to.</param>
        /// <param name="f">EnumField to use.</param>
        /// <param name="useTracking">Optionally use the reference list to avoid
        /// creating duplicate instances of Managers and collections that would
        /// effect performance and database queries.</param>
        public virtual void CreateControlBinding(CodeWriter output, EnumField f)
        {
            if (f.IsClientEditEnabled)
            {
                output.WriteLine("foreach(string name in Enum.GetNames(typof({0})))",
                    f.EnumType.Name);
                output.Indent++;
                output.WriteLine("dd{0}.Items.Add(new ListItem(name, name));", f.Name);
                output.Indent--;
                output.WriteLine();
            }
        }

        #endregion
    }
}
