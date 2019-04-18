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
    public class ComponentArtReferenceBuilder : ReferenceTypeBuilder
    {
        public override void CreateControlField(CodeWriter output,
            ReferenceField c, bool instantiate)
        {
            output.WriteLine("private ComponentArt.Web.UI.ComboBox combo{0}" +
                (instantiate ? " = new ComponentArt.Web.UI.ComboBox();" : ";"),
                c.Name);
        }

        public override void GetControlValue(CodeWriter output,
            string className, ReferenceField c)
        {
            if (c.IsClientEditEnabled)
            {
                if (c.ReferenceMode == ReferenceMode.Normal)
                {
                    output.WriteLine("if(combo{0}.SelectedItem != null)",
                        c.Name);
                    output.WriteLine("{");
                    output.Indent++;
                    if (c.ReferenceType.IsTableCoded)
                        output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(" +
                            "int.Parse(combo{1}.SelectedValue));",
                            className, c.Name, c.ReferenceType.Name);
                    else if (c.IsTableCoded)
                        output.WriteLine("{0}.{1} = {2}.NewPlaceHolder(\"{3}\", " +
                            "int.Parse(combo{0}.SelectedValue));",
                            className, c.Name, c.ReferenceType.Name, c.TableName);
                    else
                        output.WriteLine("{0}.{1} = {2}.NewPlaceHolder({0}.{1}Table, combo{1}.SelectedValue);",
                            className, c.Name, c.ReferenceType.Name, c.TableName);
                    output.Indent--;
                    output.WriteLine("}");
                    output.WriteLine("else");
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine("{0}.{1} = null;", className, c.Name);
                    output.Indent--;
                    output.WriteLine("}");
                }
                else if(c.ReferenceMode == ReferenceMode.Collection)
                {
                    output.WriteLine("{0}Manager {1}Manager = new {0}Manager();", c.ReferenceType.Name, c.PrivateName);
                    output.WriteLine("{0}.{1} = {2}Manager.DecodeString(tb{1}.Text, \"\\r\\n\");", className, c.Name, c.PrivateName);
                    output.WriteLine();
                }
            }
        }

        public override void SetControlValue(CodeWriter output,
            string className, ReferenceField c)
        {
            WebEditorReferenceFieldExtension e = (WebEditorReferenceFieldExtension)
                c.GetExtension(typeof(WebEditorReferenceFieldExtension));

            if (c.IsClientEditEnabled)
            {
                if (c.ReferenceMode == ReferenceMode.Normal)
                {
                    output.WriteLine("if({0}.{1} != null)", className, c.Name);
                    output.WriteLine("{");
                    output.Indent++;
                    if(e.ListText.Length > 0)
                        output.WriteLine("combo{0}.Text = {1}.{0}.{2};", c.Name, className, e.ListText);
                    else
                        output.WriteLine("combo{0}.Text = {1}.{0}.ToString()", c.Name, className);
                    output.WriteLine("foreach(ComponentArt.Web.UI.ComboBoxItem item in combo{0}.Items)", c.Name);
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine("if(item.Value == {0}.{1}.ID.ToString())", className, c.Name);
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine("combo{0}.SelectedItem = item;", c.Name);
                    output.WriteLine("break;");
                    output.Indent--;
                    output.WriteLine("}");
                    output.Indent--;
                    output.WriteLine("}");
                    output.Indent--;
                    output.WriteLine("}");
                    output.WriteLine("else");
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine("// Necissary to clear prior ViewState - if only we don't need it.");
                    output.WriteLine("combo{0}.Text = string.Empty;", c.Name);
                    output.WriteLine("combo{0}.SelectedItem = null;", c.Name);
                    output.Indent--;
                    output.WriteLine("}");
                }
                else if (c.ReferenceMode == ReferenceMode.Collection)
                {
                    output.WriteLine("tb{0}.Text = {1}.{0}.ToEncodedString(\"<br>\");",
                        c.Name, className);
                }
            }
            else
            {
                output.WriteLine("lt{0}.Text = {1}.{0}.ToString();", c.Name, className);
            }
        }

        public override void CreateControlProperties(CodeWriter output,
            ReferenceField f, bool instantiate, bool enableViewState,
            string addControlFormat)
        {
            ComponentArtReferenceFieldExtension extension = (ComponentArtReferenceFieldExtension)
                f.GetExtension(typeof(ComponentArtReferenceFieldExtension));
            WebEditorReferenceFieldExtension editExtension = (WebEditorReferenceFieldExtension)
                f.GetExtension(typeof(WebEditorReferenceFieldExtension));

            if (f.IsClientEditEnabled)
            {
                if (f.ReferenceMode == ReferenceMode.Normal)
                {
                    // TODO - MAKE THE CSS CLASSES EXTENSIONS ON REFERENCES

                    output.WriteLine("combo{0} = new ComponentArt.Web.UI.ComboBox();", f.Name);
                    output.WriteLine("combo{0}.ID = \"combo{0}\";", f.Name);
                    output.WriteLine("combo{0}.CssClass = \"{1}\";", f.Name, extension.CssClass);
                    output.WriteLine("combo{0}.HoverCssClass = \"{1}\";", f.Name, extension.HoverCssClass);
                    output.WriteLine("combo{0}.FocusedCssClass = \"{1}\";", f.Name, extension.FocusedCssClass);
                    output.WriteLine("combo{0}.TextBoxCssClass = \"{1}\";", f.Name, extension.TextBoxCssClass);
                    output.WriteLine("combo{0}.DropDownCssClass = \"{1}\";", f.Name, extension.DropDownCssClass);
                    output.WriteLine("combo{0}.ItemCssClass = \"{1}\";", f.Name ,extension.ItemCssClass);
                    output.WriteLine("combo{0}.ItemHoverCssClass = \"{1}\";", f.Name, extension.ItemHoverCssClass);
                    output.WriteLine("combo{0}.SelectedItemCssClass = \"{1}\";", f.Name, extension.SelectedItemCssClass);
                    output.WriteLine("combo{0}.DropHoverImageUrl = \"{1}\";", f.Name, extension.DropHoverImageUrl);
                    output.WriteLine("combo{0}.DropImageUrl = \"{1}\";", f.Name, extension.DropImageUrl);
                    if (!f.AllowNull)
                        output.Write("combo{0}.TextBoxEnabled = false;", f.Name);
                    if (extension.Width.Type == System.Web.UI.WebControls.UnitType.Pixel)
                        output.WriteLine("combo{0}.Width = Unit.Pixel(" + extension.Width.Value.ToString() + ");", f.Name);
                    else if (extension.Width.Type == System.Web.UI.WebControls.UnitType.Percentage)
                        output.WriteLine("combo{0}.Width = Unit.Percentage(" + extension.Width.Value.ToString() + ");", f.Name);
                    if (!enableViewState)
                        output.WriteLine("// combo{0}.EnableViewState = false;      // This is not " +
                            " yet possible, ComponentArt's ComboBox Requires ViewState!!!", f.Name);
                    output.WriteLine(addControlFormat,
                        string.Format("combo{0}", f.Name),
                        f.Caption != string.Empty ? f.Caption : f.Name);
                }
                else if (f.ReferenceMode == ReferenceMode.Collection)
                {
                    output.WriteLine("tb{0} = new TextBox();", f.Name);
                    output.WriteLine("tb{0}.ID = this.ID + \"_ref\";", f.Name);
                    output.WriteLine("tb{0}.EnableViewState = false;", f.Name);
                    output.WriteLine("tb{0}.Rows = 10;", f.Name);
                    output.WriteLine("tb{0}.MaxLength = 1500;", f.Name);
                    output.WriteLine("tb{0}.TextMode = TextBoxMode.MultiLine;", f.Name);
                    output.WriteLine("tb{0}.Width = Unit.Pixel(350);", f.Name);
                    output.WriteLine(addControlFormat,
                        string.Format("tb{0}", f.Name),
                        f.Caption != string.Empty ? f.Caption : f.Name);

                    output.WriteLine("Panel {0}Panel = new Panel();", f.Name);
                    output.WriteLine("{0}Panel.Controls.Add(new LiteralControl(\"<div style=\\\"float:left\\\">\"));", f.Name);
                    output.WriteLine("combo{0} = new ComponentArt.Web.UI.ComboBox();", f.Name);
                    output.WriteLine("combo{0}.CssClass = \"{1}\";", f.Name, extension.CssClass);
                    output.WriteLine("combo{0}.HoverCssClass = \"{1}\";", f.Name, extension.HoverCssClass);
                    output.WriteLine("combo{0}.FocusedCssClass = \"{1}\";", f.Name, extension.FocusedCssClass);
                    output.WriteLine("combo{0}.TextBoxCssClass = \"{1}\";", f.Name, extension.TextBoxCssClass);
                    output.WriteLine("combo{0}.DropDownCssClass = \"{1}\";", f.Name, extension.DropDownCssClass);
                    output.WriteLine("combo{0}.ItemCssClass = \"{1}\";", f.Name, extension.ItemCssClass);
                    output.WriteLine("combo{0}.ItemHoverCssClass = \"{1}\";", f.Name, extension.ItemHoverCssClass);
                    output.WriteLine("combo{0}.SelectedItemCssClass = \"{1}\";", f.Name, extension.SelectedItemCssClass);
                    output.WriteLine("combo{0}.DropHoverImageUrl = \"{1}\";", f.Name, extension.DropHoverImageUrl);
                    output.WriteLine("combo{0}.DropImageUrl = \"{1}\";", f.Name, extension.DropImageUrl);
                    output.WriteLine("combo{0}.Width = Unit.Pixel(300);", f.Name);
                    if (!enableViewState)
                        output.WriteLine("// combo{0}.EnableViewState = false;      // This is not " +
                            " yet possible, ComponentArt's ComboBox Requires ViewState!!!", f.Name);
                    output.WriteLine("{0}Panel.Controls.Add({0}Panel);", f.Name);
                    output.WriteLine("{0}Panel.Controls.Add(new LiteralControl(\"</div><div><input type=\\\"button\\\" value=\\\"Add\\\" \" +", f.Name);
                    output.WriteLine("\t\"align=\\\"right\\\" onClick=\\\"\" +");
                    output.WriteLine("\ttb{0}.ClientID + \".value += (\" + tb{0}.ClientID + \".value != '' ? '\\\\r\\\\n' : '') + \" +", f.Name);
                    output.WriteLine("\tcombo{0}.ClientObjectId + \".getSelectedItem().Text;\\\"></div>\"));", f.Name);
                    output.WriteLine(addControlFormat,
                        string.Format("{0}Panel", f.ParentFolder),
                        "&nbsp;");
                }

                if (editExtension.EditorNote.Length > 0)
                {
                    output.WriteLine(addControlFormat,
                        string.Format("new LiteralControl(\"{0}\")", editExtension.EditorNote),
                        "&nbsp;");
                }
            }
            else if (f.IsClientViewEnabled)
            {
                output.WriteLine("lt{0} = new Literal();", f.Name);
                output.WriteLine("lt{0}.EnableViewState = false;", f.Name);
                output.WriteLine(addControlFormat,
                    string.Format("lt{0}", f.Name),
                    f.Caption != string.Empty ? f.Caption : f.Name);
            }
        }

        public override void CreateControlBinding(CodeWriter output, ReferenceField c)
        {
            WebEditorReferenceFieldExtension extension = (WebEditorReferenceFieldExtension)
                c.GetExtension(typeof(WebEditorReferenceFieldExtension));

            if (c.IsClientEditEnabled)
            {
                if (c.ReferenceType.IsTableCoded)
                {
                    if (!output.ReferenceUsed(c))
                    {
                        output.WriteLine("{0}Manager {1}Manager = new {0}Manager();",
                            c.ReferenceType.Name,
                            c.ReferenceType.ParentClassEntry.PrivateName,
                            c.Name);

                        output.WriteLine("{0}Collection {1}Collection = " +
                            "{1}Manager.GetCollection(string.Empty, string.Empty);",
                            c.ReferenceType.Name,
                            c.ReferenceType.ParentClassEntry.PrivateName);

                        // Save Reference For Later :)~ Take that!
                        output.SaveReference(c,
                            c.ReferenceType.ParentClassEntry.PrivateName +
                            "Collection");
                    }
                }
                else
                {
                    if (!output.ReferenceUsed(c))
                    {
                        // BLECH! A Custom Manager, Blah BLAH!
                        output.WriteLine("{0}Manager {1}Manager = new {0}Manager(\"{2}\");",
                            c.ReferenceType.Name, c.PrivateName, c.TableName);

                        output.WriteLine("{0}Collection {1}Collection = " +
                            "{1}Manager.GetCollection(string.Empty, string.Empty);",
                            c.ReferenceType.Name, c.PrivateName);

                        // Save another pesky reference for later!
                        // Don't worry so much bud, it keeps track of tables!
                        output.SaveReference(c, c.PrivateName + "Collection");
                    }
                }

                output.WriteLine("foreach({0} itemObject in {1})",
                    c.ReferenceType.Name, output.GetReference(c));
                output.WriteLine("{");
                output.Indent++;
                output.WriteLine("ComponentArt.Web.UI.ComboBoxItem item = " +
                    "new ComponentArt.Web.UI.ComboBoxItem();");
                output.WriteLine("item.Text = itemObject." +
                    extension.ListText + ";");
                output.WriteLine("item.Value = itemObject.ID.ToString();");
                output.WriteLine("combo{0}.Items.Add(item);", c.Name);
                output.Indent--;
                output.WriteLine("}");
                output.WriteLine();
            }
        }
    }    
}