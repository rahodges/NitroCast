using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NitroCast.Core.Extensions
{
    [TypeConverter(typeof(TypeConverters.ValueTypeBuilderConverter))]
    public class ValueTypeBuilder : Extension
    {
        #region BUILDER SUPPORT - DO NOT MESS WITH

        // The typeCache holds precompiled versions of types for
        // rapid code generation. This kind of caching is not 
        // necissary for reference types since reference types
        // are instantly generated.       --- DO NOT CHANGE! ---

        private static ValueTypeBuilder defaultBuilder;
        private static Dictionary<string, ValueTypeBuilder> builders;
        private static Dictionary<ValueTypeBuilder, ValueTypeCollection> typeCache;

        public static Dictionary<string, ValueTypeBuilder> Builders
        {
            get { return builders; }
        }

        public static ValueTypeBuilder Default
        {
            get { return defaultBuilder; }
        }

        /// <summary>
        /// Initializes all builders by creating a cache of pre-built properties
        /// for ValueTypes. This should be called after any DataTypeExtensions have
        /// been loaded and MUST be called before NitroCast runs.
        /// </summary>
        public static void RefreshCache()
        {
            typeCache.Clear();

            foreach(KeyValuePair<string, ValueTypeBuilder> builder in builders)
            {
                ValueTypeCollection valueTypes = DataTypeManager.Make(builder.Value);
                typeCache.Add(builder.Value, valueTypes);
            }
        }

        static ValueTypeBuilder()
        {
            builders = new Dictionary<string, ValueTypeBuilder>();
            typeCache = new Dictionary<ValueTypeBuilder, ValueTypeCollection>();

            defaultBuilder = new ValueTypeBuilder();
            defaultBuilder.Name = "Default";
            defaultBuilder.FullName = "";
            defaultBuilder.Description = "Default builder for value types.";
            defaultBuilder.Copyright = "Copyright © 2007 Roy A.E. Hodges";
            defaultBuilder.ExtensionPath = string.Empty;
            defaultBuilder.Author = "Roy A.E. Hodges";
            defaultBuilder.AssemblyPath = string.Empty;
            defaultBuilder.OutputFileNameFormat = string.Empty;
            builders.Add(defaultBuilder.Name, defaultBuilder);
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

        #region Override for Inheriting Extensions

        public void SetEditorProperties(ValueType type)
        {
            setEditorIDProperty(type);

            switch (type.DotNetType)
            {
                case "Boolean":                    
                    //setRevValidator(type, @"^([Vv]+(erdade(iro)?)?|[Ff]+(als[eo])?|[Tt]+(rue)?|0|[\+\-]?1)$");
                    //setReqValidator(type);
                    break;
                case "Byte":
                    setReqValidator(type);
                    setRangeValidator(type, 0, 255);
                    break;
                case "Decimal":
                    setReqValidator(type);
                    setRangeValidator(type, decimal.Zero, decimal.MaxValue);
                    break;
                case "Int16":
                    setReqValidator(type);
                    setRangeValidator(type, 
                        (int)Int16.MinValue,
                        (int)Int16.MaxValue);
                    break;
                case "Int32":
                    setReqValidator(type);
                    setRangeValidator(type, Int32.MinValue, Int32.MaxValue);
                    break;
                case "DateTime":
                    setReqValidator(type);
                    setRevValidator(type, @"^(?=\d)(?:(?:(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})|(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))|(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{{2}}))($|\ (?=\d)))?(((0?[1-9]|1[012])(:[0-5]\d){{0,2}}(\ [AP]M))|([01]\d|2[0-3])(:[0-5]\d){{1,2}})?$");
                    break;
                case "TimeSpan":
                    setReqValidator(type);
                    setRevValidator(type,
                        @"^\s*-?(\d{{0,7}}|10[0-5]\d{{0,5}}|106[0-6]\d{{0,4}}|1067[0-4]\d{{0,3}}|10675[0-1]\d{{0,2}}|((\d{{0,7}}|10[0-5]\d{{0,5}}|106[0-6]\d{{0,4}}|1067[0-4]\d{{0,3}}|10675[0-1]\d{{0,2}})\.)?([0-1]?[0-9]|2[0-3]):[0-5]?[0-9](:[0-5]?[0-9](\.\d{{1,7}})?)?)\s*$");
                    break;
                case "Guid":
                    setReqValidator(type);
                    setRevValidator(type,
                        @"^\{?[a-fA-F\d]{{8}}-([a-fA-F\d]{{4}}-){{3}}[a-fA-F\d]{{12}}\}?$");
                    break;
            }
        }

        #endregion

        #region Private Helper Methods for Property Setting

        private void setEditorIDProperty(ValueType type)
        {
            if (type.EditorProperties == null)
                type.EditorProperties =
                    new System.Collections.Generic.List<string>();

            if (!type.EditorProperties.Contains(
                "{0}{1}.ID = \"{0}{1}\";"))
            {
                type.EditorProperties.Add(
                    "{0}{1}.ID = \"{0}{1}\";");
            }
        }

        private void setRevValidator(ValueType type,
            string regex)
        {
            if (type.ValidatorProperties != null)
                throw new Exception("RequiredFieldValidator is already set on this type.");

            type.ValidatorClass = "RegularExpressionValidator";
            type.ValidatorNamespace = "System.Web.UI.WebControls";
            type.ValidatorPrefix = "rev";

            if (type.ValidatorProperties == null)
                type.ValidatorProperties =
                    new System.Collections.Generic.List<string>();

            type.ValidatorProperties.Add("{0}{1}.ID = \"Rev{1}\";");
            type.ValidatorProperties.Add("{0}{1}.ControlToValidate = {2}{1}.ID;");
            type.ValidatorProperties.Add("{0}{1}.ValidationExpression = @\"" +
                regex + "\";");
            type.ValidatorProperties.Add("{0}{1}.ErrorMessage = \"*\";");
            type.ValidatorProperties.Add("{0}{1}.Display = ValidatorDisplay.Dynamic;");
            type.ValidatorProperties.Add("{0}{1}.SetFocusOnError = true;");
        }

        private void setRangeValidator(ValueType type, object min, object max)
        {
            if (min.GetType() != max.GetType())
                throw new Exception("Minimum type does not match maximum type.");
            if (type.RangeValidatorProperties != null)
                throw new Exception("RangeValidator is already set on this type.");

            if (type.RangeValidatorProperties == null)
                type.RangeValidatorProperties =
                    new System.Collections.Generic.List<string>();

            type.RangeValidatorClass = "RangeValidator";
            type.RangeValidatorNamespace = "System.Web.UI.WebControls";
            type.RangeValidatorPrefix = "rng";
            type.RangeValidatorProperties.Add("{0}{1}.ID = \"{0}{1}\";");
            type.RangeValidatorProperties.Add("{0}{1}.ControlToValidate = {2}{1}.ID;");
            type.RangeValidatorProperties.Add("{0}{1}.ErrorMessage = \"*\";");
            type.RangeValidatorProperties.Add("{0}{1}.Display = ValidatorDisplay.Dynamic;"); 
            if (min is DateTime)
            {
                type.RangeValidatorProperties.Add("{0}{1}.Type = " +
                    "ValidationDataType.Date;");                
            }
            else if (min is Int32)
            {
                type.RangeValidatorProperties.Add("{0}{1}.Type = " +
                    "ValidationDataType.Integer;");
            }
            else if (min is Double)
            {
                type.RangeValidatorProperties.Add("{0}{1}.Type = " +
                    "ValidationDataType.Double;");
            }
            else if (min is String)
            {
                type.RangeValidatorProperties.Add("{0}{1}.Type = " + 
                    "ValidationDataType.String;");
            }
            else if (min is decimal)
            {
                type.RangeValidatorProperties.Add("{0}{1}.Type = " +
                    "ValidationDataType.Double;");
            }
            type.RangeValidatorProperties.Add("{0}{1}.MinimumValue = \"" +
                min.ToString() + "\";");
            type.RangeValidatorProperties.Add("{0}{1}.MaximumValue = \"" +
                max.ToString() + "\";");
        }

        private void setReqValidator(ValueType type)
        {
            if (type.RequiredValidatorProperties != null)
                throw new Exception("RequiredFieldValidator is already set on this type.");

            if (type.RequiredValidatorProperties == null)
                type.RequiredValidatorProperties =
                    new System.Collections.Generic.List<string>();

            type.RequiredValidatorClass = "RequiredFieldValidator";
            type.RequiredValidatorNamespace = "System.Web.UI.WebControls";
            type.RequiredValidatorPrefix = "req";
            type.RequiredValidatorProperties.Add("{0}{1}.ID = \"{0}{1}\";");
            type.RequiredValidatorProperties.Add("{0}{1}.ControlToValidate = {2}{1}.ID;");
            type.RequiredValidatorProperties.Add("{0}{1}.ErrorMessage = \"*\";");
            type.RequiredValidatorProperties.Add("{0}{1}.Display = ValidatorDisplay.Dynamic;");
        }

        #endregion

        public void CreateClassField(CodeWriter output,
            ValueField f, bool isInternal, bool instantiate)
        {
            ValueType vt = typeCache[f.Builder][f.ValueType.Name];

            output.WriteLine(
                (isInternal ? "internal" : "private") + " {0} {1}" +
                (instantiate ? " = new {0};" : ";"),
                vt.ProgramType,
                f.PrivateName);
        }


        public void CreateControlField(CodeWriter output,
            ValueField f, bool instantiate)
        {
            ValueType vt = typeCache[f.Builder][f.ValueType.Name];

            if (f.IsClientEditEnabled)
            {
                output.WriteLine("private {2} {0}{1}" +
                    (instantiate ? " = new {2}();" : ";"),
                    vt.TypeEditorPrefix, f.Name, vt.TypeEditorClass);

                if (vt.ValidatorClass.Length > 0)
                {
                    output.WriteLine("private {2} {0}{1}" +
                        (instantiate ? " = new {2}();" : ";"),
                        vt.ValidatorPrefix, f.Name, vt.ValidatorClass);
                }
                if (vt.RequiredValidatorClass.Length > 0)
                {
                    output.WriteLine("private {2} {0}{1}" +
                        (instantiate ? " = new {2}();" : ";"),
                        vt.RequiredValidatorPrefix, f.Name, vt.RequiredValidatorClass);
                }
                if (vt.RangeValidatorClass.Length > 0)
                {
                    output.WriteLine("private {2} {0}{1}" +
                        (instantiate ? " = new {2}();" : ";"),
                        vt.RangeValidatorPrefix, f.Name, vt.RangeValidatorClass);
                }
            }
            else if (f.IsClientViewEnabled)
            {
                output.WriteLine("private Literal lt{0}" +
                    (instantiate ? " = new Literal();" : ";"),
                    f.Name);
            }
        }

        // The following methods have to look up the correct
        // builder in the cach to execute their method. If the
        // builder is missing an exception should be thrown.

        public void GetControlValue(CodeWriter output,
            string className, ValueField f)
        {
            ValueType vt = typeCache[f.Builder][f.ValueType.Name];

            if (f.IsClientEditEnabled)
            {
                output.WriteLine(
                    vt.TypeEditorGetValueFormat,
                    vt.TypeEditorPrefix,
                    f.Name,
                    className);
            }
        }

        public void SetControlValue(CodeWriter output,
            string className, ValueField f)
        {
            ValueType vt = typeCache[f.Builder][f.ValueType.Name];

            if (f.IsClientEditEnabled)
            {
                output.WriteLine(vt.TypeEditorSetValueFormat,
                    vt.TypeEditorPrefix,
                    f.Name,
                    className);
            }
            else if(f.IsClientViewEnabled)
            {
                output.WriteLine("lt{0}.Text = {1}.{0}.ToString();",
                    f.Name, className);
            }
        }

        public void CreateControlProperties(CodeWriter output,
            ValueField f, bool instantiate, bool enableViewState)
        {
            CreateControlProperties(output,
                f, instantiate, enableViewState, string.Empty);
        }

        public void CreateControlProperties(CodeWriter output,
            ValueField f, bool instantiate, bool enableViewState,
            string addControlFormat)
        {
            ValueType vt = typeCache[f.Builder][f.ValueType.Name];

            if (f.IsClientEditEnabled)
            {
                if (instantiate)
                    output.WriteLine("{0}{1} = new {2}();",
                        vt.TypeEditorPrefix,
                        f.Name,
                        vt.TypeEditorClass);

                // Add default properties to field type editor
                if (vt.EditorProperties != null)
                    for (int x = 0; x < vt.EditorProperties.Count; x++)
                        output.WriteLine(vt.EditorProperties[x],
                            vt.TypeEditorPrefix,
                            f.Name);
                if (!enableViewState)
                    output.WriteLine("{0}{1}.EnableViewState = false;",
                        vt.TypeEditorPrefix,
                        f.Name);

                //"Controls.Add({0}{1});",

                if (vt.ValidatorClass != string.Empty)
                {
                    if (instantiate)
                        output.WriteLine("{0}{1} = new {2}();",
                            vt.ValidatorPrefix,
                            f.Name,
                            vt.ValidatorClass);

                    // Add default properties to field type editor
                    if (vt.ValidatorProperties != null)
                        for (int x = 0; x < vt.ValidatorProperties.Count; x++)
                            output.WriteLine(vt.ValidatorProperties[x],
                                vt.ValidatorPrefix,
                                f.Name,
                                vt.TypeEditorPrefix);
                }

                if (vt.RequiredValidatorClass != string.Empty)
                {
                    if (instantiate)
                        output.WriteLine("{0}{1} = new {2}();",
                            vt.RequiredValidatorPrefix,
                            f.Name,
                            vt.RequiredValidatorClass);

                    // Add default properties to field type editor
                    if (vt.RequiredValidatorProperties != null)
                        for (int x = 0; x < vt.RequiredValidatorProperties.Count; x++)
                            output.WriteLine(vt.RequiredValidatorProperties[x],
                                vt.RequiredValidatorPrefix,
                                f.Name,
                                vt.TypeEditorPrefix);
                }

                if (vt.RangeValidatorClass != string.Empty)
                {
                    if (instantiate)
                        output.WriteLine("{0}{1} = new {2}();",
                            vt.RangeValidatorPrefix,
                            f.Name,
                            vt.RangeValidatorClass);

                    // Add default properties to field type editor
                    if (vt.RangeValidatorProperties != null)
                        for (int x = 0; x < vt.RangeValidatorProperties.Count; x++)
                            output.WriteLine(vt.RangeValidatorProperties[x],
                                vt.RangeValidatorPrefix,
                                f.Name,
                                vt.TypeEditorPrefix);
                }

                if (addControlFormat.Length > 0)
                {
                    string controlList = string.Format("{0}{1}", vt.TypeEditorPrefix, f.Name);
                    if (vt.ValidatorClass.Length > 0)
                        controlList += string.Format(", {0}{1}", vt.ValidatorPrefix, f.Name);
                    if (vt.RequiredValidatorClass.Length > 0)
                        controlList += string.Format(", {0}{1}", vt.RequiredValidatorPrefix, f.Name);
                    if (vt.RangeValidatorClass.Length > 0)
                        controlList += string.Format(", {0}{1}", vt.RangeValidatorPrefix, f.Name);

                    output.WriteLine(addControlFormat, controlList, 
                        f.Caption.Length > 0 ? f.Caption : f.Name);
                }
                else
                {
                    output.WriteLine("Controls.Add({0}{1});", vt.TypeEditorPrefix, f.Name);
                    if (vt.ValidatorClass.Length > 0)
                        output.WriteLine("Controls.Add({0}{1});", vt.ValidatorPrefix, f.Name);
                    if (vt.RequiredValidatorClass.Length > 0)
                        output.WriteLine("Controls.Add({0}{1});", vt.RequiredValidatorPrefix, f.Name);
                    if (vt.RangeValidatorClass.Length > 0)
                        output.WriteLine("Controls.Add({0}{1});", vt.RangeValidatorPrefix, f.Name);
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
    }
}