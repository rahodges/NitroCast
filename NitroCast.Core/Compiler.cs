using System;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using Microsoft.CSharp;

namespace NitroCast.Core
{
    public class Compiler
    {
        private CompilerErrorCollection errors = null;

        public Compiler()
        {            
        }

        public System.Reflection.Assembly Compile(DataModel model)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            CompilerResults results = null;

            StringBuilder sb = new StringBuilder();

            parameters.OutputAssembly = "SourceCodeManager";
            parameters.ReferencedAssemblies.Add("system.dll");
            parameters.ReferencedAssemblies.Add("system.data.dll");
            parameters.ReferencedAssemblies.Add("system.web.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files\\Microsoft Enterprise Library 3.1 - May 2007\\Bin\\Microsoft.Practices.EnterpriseLibrary.Caching.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files\\Microsoft Enterprise Library 3.1 - May 2007\\Bin\\Microsoft.Practices.EnterpriseLibrary.Common.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files\\Microsoft Enterprise Library 3.1 - May 2007\\Bin\\Microsoft.Practices.EnterpriseLibrary.Data.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files\\Microsoft Enterprise Library 3.1 - May 2007\\Bin\\Microsoft.Practices.ObjectBuilder.dll");
            parameters.CompilerOptions = "/t:library";
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.IncludeDebugInformation = false;
            parameters.OutputAssembly = model.DefaultNamespace + ".dll";

            results = provider.CompileAssemblyFromSource(parameters, model.ExportSourceCode());

            if (results.Errors.Count != 0)
            {
                errors = results.Errors;
                throw new Exception("Code compilation errors occurred.");
            }
            else
            {
                return results.CompiledAssembly;
            }
        }
    }
}
