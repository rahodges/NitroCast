using System;
using NitroCast.Core;
using NitroCast.Core.Extensions;

namespace NitroCast.Extensions.Default
{
	/// <summary>
	/// Summary description for WebGridGenerator.
	/// </summary>
    [ExtensionAttribute("Web Class Page Codebehind",
         "Roy A.E. Hodges",
         "Copyright © 2003 Roy A.E. Hodges. All Rights Reserved.",
         "{0}Page.aspx.cs",
         "Default web class page codebehind file for the object.",
         "\\Default\\Object Class", true)]
	public class WebClassPageCodebehindGenerator : OutputExtension
	{
        public WebClassPageCodebehindGenerator()
		{
            IsWebPage = true;
            ExtensionType = OutputExtensionType.ModelClass;
		}

		public override string Render()
		{
			CodeWriter output = new CodeWriter();

            output.WriteLine("using System;");
            output.WriteLine("using System.Web;");
            output.WriteLine("using Amns.GreyFox.Web.UI.WebControls;");
            output.WriteLine();

            output.WriteLine("namespace {0}Web.Administration", _modelClass.ParentModel.Name);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("/// <summary>");
            output.WriteLine("/// Default NitroCast Class Page Codebehind");
            output.WriteLine("/// </summary>");
            output.WriteLine("public partial class {0}Page : System.Web.UI.Page", _modelClass.Name);
            output.WriteLine("{");
            output.Indent++;
            //output.WriteLine("protected {0}.Web.UI.WebControls.{1}Grid {1}Grid1;", ___classObject.Namespace, ___classObject.Name);
            //output.WriteLine("protected {0}.Web.UI.WebControls.{1}Editor {1}Editor1;", ___classObject.Namespace, ___classObject.Name);
            //output.WriteLine("protected {0}.Web.UI.WebControls.{1}View {1}View1;", ___classObject.Namespace, ___classObject.Name);
            output.WriteLine();
            output.WriteLine("private void Page_Load(object sender, System.EventArgs e)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("{0}Grid1.ToolbarClicked += new ToolbarEventHandler({0}Grid1_ToolbarClicked);", _modelClass.Name);
            output.WriteLine("{0}Editor1.Cancelled += new EventHandler(showGrid);", _modelClass.Name);
            output.WriteLine("{0}Editor1.Updated += new EventHandler(showGrid);", _modelClass.Name);
            output.WriteLine("{0}View1.OkClicked += new EventHandler(showGrid);", _modelClass.Name);
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine();

            #region Web Form Designer generated code

            output.WriteLine("#region Web Form Designer generated code");
            output.WriteLine("override protected void OnInit(EventArgs e)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("//");
            output.WriteLine("// CODEGEN: This call is required by the ASP.NET Web Form Designer.");
            output.WriteLine("InitializeComponent();");
            output.WriteLine("base.OnInit(e);");
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine();
            output.WriteLine("/// <summary>");
            output.WriteLine("/// Required method for Designer support - do not modify");
            output.WriteLine("/// the contents of this method with the code editor.");
            output.WriteLine("/// </summary>");
            output.WriteLine("private void InitializeComponent()");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("this.Load += new System.EventHandler(this.Page_Load);");
            output.WriteLine();
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("#endregion");
            output.WriteLine();

            #endregion

            output.WriteLine("private void resetControls()");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("{0}Grid1.Visible = false;", _modelClass.Name);
            output.WriteLine("{0}Editor1.Visible = false;", _modelClass.Name);
            output.WriteLine("{0}View1.Visible = false;", _modelClass.Name);
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine();

            output.WriteLine("private void showGrid(object sender, EventArgs e)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("{0}Grid1.Visible = true;", _modelClass.Name);
            output.WriteLine("{0}Editor1.Visible = false;", _modelClass.Name);
            output.WriteLine("{0}View1.Visible = false;", _modelClass.Name);
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine();

            output.WriteLine("private void {0}Grid1_ToolbarClicked(object sender, ToolbarEventArgs e)", _modelClass.Name);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("switch(e.SelectedToolbarItem.Command)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("case \"new\":");
            output.Indent++;
            output.WriteLine("resetControls();");
            output.WriteLine("{0}Editor1.{0}ID = 0;", _modelClass.Name);
            output.WriteLine("{0}Editor1.Visible = true;", _modelClass.Name);
            output.WriteLine("break;");
            output.Indent--;
            output.WriteLine("case \"view\":");            
            output.Indent++;
            output.WriteLine("resetControls();");
            output.WriteLine("{0}View1.{0}ID = {0}Grid1.SelectedID;", _modelClass.Name);
            output.WriteLine("{0}View1.Visible = true;", _modelClass.Name);
            output.WriteLine("break;");
            output.Indent--;
            output.WriteLine("case \"edit\":");
            output.Indent++;
            output.WriteLine("resetControls();");
            output.WriteLine("{0}Editor1.{0}ID = {0}Grid1.SelectedID;", _modelClass.Name);
            output.WriteLine("{0}Editor1.Visible = true;", _modelClass.Name);
            output.WriteLine("break;");
            output.Indent--;
            output.Indent--;
            output.WriteLine("}");
            output.Indent--;
            output.WriteLine("}");

            output.Indent--;
            output.WriteLine("}");

            output.Indent--;
            output.WriteLine("}");

            return output.ToString();
        }
	}
}
