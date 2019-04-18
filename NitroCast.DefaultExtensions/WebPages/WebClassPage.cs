using System;
using NitroCast.Core;
using NitroCast.Core.Extensions;

namespace NitroCast.Extensions.Default
{
	/// <summary>
	/// Summary description for WebGridGenerator.
	/// </summary>
    [ExtensionAttribute("Web Class Page",
         "Roy A.E. Hodges",
         "Copyright © 2003 Roy A.E. Hodges. All Rights Reserved.",
         "{0}Page.aspx",
         "Default web class page for the object.",
         "\\Default\\Object Class", true)]
	public class WebClassPageGenerator : OutputExtension
	{
        public WebClassPageGenerator()
		{
            IsWebPage = true;
            ExtensionType = OutputExtensionType.ModelClass;
		}

		public override string Render()
		{
			CodeWriter output = new CodeWriter();
			output.WriteLine("<%@ Register TagPrefix=\"cc1\" Namespace=\"{0}.Web.UI.WebControls\" Assembly=\"{0}\" %>",
                _modelClass.Namespace);
            output.WriteLine("<%@ Page language=\"c#\" CodeFile=\"{0}Page.aspx.cs\" " +
                "Inherits=\"{1}Web.Administration.{0}Page\" trace=\"false\" %>",
                _modelClass.Name, _modelClass.ParentModel.Name);

            output.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\" >");
            output.WriteLine("<html>");
            output.Indent++;
            output.WriteLine("<head>");
            output.Indent++;
            output.WriteLine("<title>Enterprise Services</title>");
            output.WriteLine("<link href=\"./Themes/Admin.css\" type=\"text/css\" rel=\"stylesheet\">");
            output.Indent--;
            output.WriteLine("</head>");
            output.WriteLine("<body>");
            output.Indent++;
            output.WriteLine("<form id=\"Default\" method=\"post\" runat=\"server\">");
            output.Indent++;
            output.WriteLine("<table id=\"MainPage\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"2\" height=\"100%\">");
            output.Indent++;
            output.WriteLine("<tr>");
            output.Indent++;
            output.WriteLine("<td class=\"bodyLine\" bgcolor=\"#ffffff\">");
            output.Indent++;
            output.WriteLine("<table id=\"Table1\" cellSpacing=\"0\" cellPadding=\"3\" width=\"100%\" border=\"0\" height=\"100%\">");
            output.Indent++;
            output.WriteLine("<tr>");
            output.Indent++;
            output.WriteLine("<td class=\"bgTop\" colSpan=\"2\">");
            output.Indent++;
            output.WriteLine("<table id=\"Header\" cellSpacing=\"0\" cellPadding=\"3\" width=\"100%\" border=\"0\">");
            output.Indent++;
            output.WriteLine("<tr><td><h1>Enterprise Services</h1></td></tr>");
            output.WriteLine("<tr><td>Menu | Menu</td></tr>");
            output.Indent--;
            output.WriteLine("</table>");
            output.Indent--;
            output.WriteLine("</td>");
            output.Indent--;
            output.WriteLine("</tr>");

            output.WriteLine("<tr>");
            output.Indent++;
            output.WriteLine("<td vAlign=\"top\">");
            output.Indent++;
            output.WriteLine("<table id=\"LeftMenu\" class=\"forumLine\" cellSpacing=\"1\" cellPadding=\"3\" width=\"200\" border=\"0\">");
            output.Indent++;
            output.WriteLine("<tr><th>{0}</th></tr>", _modelClass.Caption);
            output.WriteLine("<tr><td>{0} can be accessed here</td></tr>", _modelClass.Description);
            output.Indent--;
            output.WriteLine("</table>");
            output.Indent--;
            output.WriteLine("</td>");
            output.WriteLine("<td vAlign=\"top\" width=\"100%\">");
            output.Indent++;
            
            output.WriteLine("<cc1:{0}Grid id=\"{0}Grid1\" runat=\"server\" Width=\"100%\" BorderWidth=\"0px\" Height=\"100%\" ", 
                _modelClass.Name);
            output.Indent++;
            output.WriteLine("CellSpacing=\"1px\" CellPadding=\"3px\" CssClass=\"forumLine\" HeaderCssClass=\"thHead\" SubHeaderCssClass=\"catHead\"");
            output.WriteLine("HeaderRowCssClass=\"rowHead\" AlternateRowCssClass=\"row2\" SelectedRowCssClass=\"row3\" DefaultRowCssClass=\"row1\"");
            output.WriteLine("Text=\"{0}\"", _modelClass.Caption);
            output.WriteLine("></cc1:{0}Grid>",
                _modelClass.Name);
            output.Indent--;
            
            output.WriteLine("<cc1:{0}Editor id=\"{0}Editor1\" runat=\"server\" Width=\"100%\" BorderWidth=\"0px\"",
                _modelClass.Name);
            output.Indent++;
            output.WriteLine("CellSpacing=\"1px\" CellPadding=\"3px\" CssClass=\"forumLine\" HeaderCssClass=\"thHead\" SubHeaderCssClass=\"catHead\" Visible=\"false\"");
            output.WriteLine("Text=\"{0} Editor\"", _modelClass.Caption);
            output.WriteLine("></cc1:{0}Editor>",
                _modelClass.Name);
            output.Indent--;

            output.WriteLine("<cc1:{0}View id=\"{0}View1\" runat=\"server\" Width=\"100%\" BorderWidth=\"0px\"",
                _modelClass.Name);
            output.Indent++;
            output.WriteLine("CellSpacing=\"1px\" CellPadding=\"3px\" CssClass=\"forumLine\" HeaderCssClass=\"thHead\" SubHeaderCssClass=\"catHead\" Visible=\"false\"");
            output.WriteLine("Text=\"{0} View\"", _modelClass.Caption);
            output.WriteLine("></cc1:{0}View>",
                _modelClass.Name);
            output.Indent--;
            output.Indent--;
            
            output.WriteLine("</td>");
            output.Indent--;
            output.WriteLine("</tr>");

            output.WriteLine("<tr>");
            output.Indent++;
            output.WriteLine("<td colSpan=\"2\" class=\"bgBottom\" height=\"37\" align=\"center\">");
            output.Indent++;
            output.WriteLine("<p class=\"copyright\">Copyright © 2006 Anyone Corp.</p>");
            output.Indent--;
            output.WriteLine("</td>");
            output.Indent--;
            output.WriteLine("</tr>");
            output.Indent--;
            output.WriteLine("</table>");
            output.Indent--;
            output.WriteLine("</td>");
            output.Indent--;
            output.WriteLine("</tr>");
            output.Indent--;
            output.WriteLine("</table>");
            output.Indent--;
            output.WriteLine("</form>");
            output.Indent--;
            output.WriteLine("</body>");
            output.Indent--;
            output.WriteLine("</html>");

            return output.ToString();
		}
	}
}
