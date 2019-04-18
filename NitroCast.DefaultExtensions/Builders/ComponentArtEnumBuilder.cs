using System;
using System.Collections.Generic;
using System.Text;
using NitroCast.Core;
using NitroCast.Core.Extensions;
using NitroCast.Extensions.Default;

namespace NitroCast.DefaultPlugins.Builders
{
    [ExtensionAttribute("ComponentArt ComboBox",
     "Roy A.E. Hodges",
     "Copyright © 2007 Roy A.E. Hodges. All Rights Reserved.",
     "",
     "Extensions to generate ComponentArt controls.",
     "")]
    public class ComponentArtEnumBuilder : EnumTypeBuilder
    {
        public override void CreateControlField(CodeWriter output,
            EnumField f, bool instantiate)
        {
            output.WriteLine("private ComponentArt.Web.UI.ComboBox combo{0}" +
                (instantiate ? " = new ComponentArt.Web.UI.ComboBox();" : ";"),
                f.Name);
        }

        public override void GetControlValue(CodeWriter output,
            string className, EnumField f)
        {
             output.WriteLine("if(combo{0}.SelectedItem != null)", f.Name);
            output.Indent++;
            output.WriteLine("{0}.{1} = ({2})", className, f.Name, f.EnumType.Name);
            output.Indent++;
            output.WriteLine("Enum.Parse(typeof({0}), combo{1}.SelectedItem.Value);", f.EnumType, f.Name);
            output.Indent--;
            output.Indent--;
        }

        public override void SetControlValue(CodeWriter output,
            string className, EnumField f)
        {
            output.WriteLine("foreach(ComponentArt.Web.UI.ComboBoxItem item in combo{0}.Items)", f.Name);
            output.Indent++;
            output.WriteLine("if(item.Value == {0}.{1}.ToString())", className, f.Name);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("combo{0}.SelectedItem = item;", f.Name);
            output.WriteLine("break;");
            output.Indent--;
            output.WriteLine("}");
            output.Indent--;
        }

        public override void CreateControlProperties(CodeWriter output,
            EnumField f, bool instantiate, bool enableViewState,
            string addControlFormat)
        {
            ComponentArtEnumFieldExtension extension = (ComponentArtEnumFieldExtension)
                f.GetExtension(typeof(ComponentArtEnumFieldExtension));

            // TODO - MAKE THE CSS CLASSES EXTENSIONS ON REFERENCES

            output.WriteLine("combo{0} = new ComponentArt.Web.UI.ComboBox();", f.Name);
            output.WriteLine("combo{0}.ID = \"combo{0}\";", f.Name);
            output.WriteLine("combo{0}.CssClass = \"" + extension.CssClass + "\";", f.Name);
            output.WriteLine("combo{0}.HoverCssClass = \"" + extension.HoverCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.FocusedCssClass = \"" + extension.FocusedCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.TextBoxCssClass = \"" + extension.TextBoxCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.DropDownCssClass = \"" + extension.DropDownCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.ItemCssClass = \"" + extension.ItemCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.ItemHoverCssClass = \"" + extension.ItemHoverCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.SelectedItemCssClass = \"" + extension.SelectedItemCssClass + "\";", f.Name);
            output.WriteLine("combo{0}.DropHoverImageUrl = \"" + extension.DropHoverImageUrl + "\";", f.Name);
            output.WriteLine("combo{0}.DropImageUrl = \"" + extension.DropImageUrl + "\";", f.Name);
            if (extension.Width.Type == System.Web.UI.WebControls.UnitType.Pixel)
                output.WriteLine("combo{0}.Width = Unit.Pixel(" + extension.Width.Value.ToString() + ");", f.Name);
            else if (extension.Width.Type == System.Web.UI.WebControls.UnitType.Percentage)
                output.WriteLine("combo{0}.Width = Unit.Percentage(" + extension.Width.Value.ToString() + ");", f.Name);
            output.WriteLine("combo{0}.TextBoxEnabled = false;", f.Name);
            if (!enableViewState)
                output.WriteLine("// combo{0}.EnableViewState = false;      // This is not " +
                    " yet possible, ComponentArt's ComboBox Requires ViewState!!!", f.Name);

            if (addControlFormat.Length > 0)
            {
                output.WriteLine(addControlFormat,
                    string.Format("combo{0}", f.Name),
                    f.Caption.Length > 0 ? f.Caption : f.Name);
            }
            else
            {
                output.Write("Controls.Add(combo{0});", f.Name);
            }
        }

        public override void CreateControlBinding(CodeWriter output, EnumField f)
        {
            output.WriteLine("foreach(string name in Enum.GetNames(typeof({0})))",
                f.EnumType.Name);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("ComponentArt.Web.UI.ComboBoxItem item = new ComponentArt.Web.UI.ComboBoxItem();");
            output.WriteLine("item.Text = name;");
            output.WriteLine("item.Value = name;");
            output.WriteLine("combo{0}.Items.Add(item);", f.Name);
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine();
        }
    }    
}