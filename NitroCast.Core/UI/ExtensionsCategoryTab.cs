using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NitroCast.Core.Extensions;

namespace NitroCast.Core.UI
{

    // This component adds a TypeCategoryTab to the propery browser 
    // that is available for any components in the current design mode document.
    [PropertyTabAttribute(typeof(ExtensionsCategoryTab), PropertyTabScope.Document)]
    public class ExtensionsCategoryTabComponent : System.ComponentModel.Component
    {
        public ExtensionsCategoryTabComponent()
        {
        }
    }

    // A TypeCategoryTab property tab lists properties by the 
    // category of the type of each property.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class ExtensionsCategoryTab : PropertyTab
    {
        [BrowsableAttribute(true)]
        // This string contains a Base-64 encoded and serialized example property tab image.
        private string img = "AAEAAAD/////AQAAAAAAAAAMAgAAAFRTeXN0ZW0uRHJhd2luZywgVmVyc2lvbj0xLjAuMzMwMC4w" +
            "LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWIwM2Y1ZjdmMTFkNTBhM2EFAQAAABVTeXN0ZW0uRHJhd2luZ" +
            "y5CaXRtYXABAAAABERhdGEHAgIAAAAJAwAAAA8DAAAA9gAAAAJCTfYAAAAAAAAANgAAACgAAAAIAAAACAAAAAEAGAAAAA" +
            "AAAAAAAMQOAADEDgAAAAAAAAAAAAD///////////////////////////////////9ZgABZgADzPz/zPz/zPz9AgP/////" +
            "/////gAD/gAD/AAD/AAD/AACKyub///////+AAACAAAAAAP8AAP8AAP9AgP////////9ZgABZgABz13hz13hz13hAgP//" +
            "////////gAD/gACA/wCA/wCA/wAA//////////+AAACAAAAAAP8AAP8AAP9AgP////////////////////////////////////8L";

        public ExtensionsCategoryTab()
        {

        } 

        /// <summary>
        /// Gets Extension Properties for objects provided by NitroCast Extensions. This
        /// will search all available extensions for extensions properties available to be
        /// set on the component. If extensions are found, extensions for the object are
        /// ensured to be created and associated to the grid accordingly.
        /// </summary>
        /// <param name="component">The component selected for the properties grid.</param>
        /// <param name="attributes">Attributes on the component.</param>
        /// <returns></returns>
        public override System.ComponentModel.PropertyDescriptorCollection 
            GetProperties(object component, System.Attribute[] attributes)
        {   
            PropertyDescriptorCollection temp;
            List<PropertyDescriptor> tempProps;

            ExtensionManager.GetInstance().AddExtensions(component);

            tempProps = new List<PropertyDescriptor>();

            if (component is ValueField)
            {
                ValueField vf = (ValueField)component;

                foreach (ValueFieldExtension extension in vf.extensions.Values)
                {
                    temp = TypeDescriptor.GetProperties(extension, false);
                    foreach (PropertyDescriptor p in temp)
                    {
                        tempProps.Add(new BoundPropertyDescriptor(extension, 
                            p, attributes));
                    }
                }
            }
            else if (component is ReferenceField)
            {
                ReferenceField rf = (ReferenceField)component;

                foreach (ReferenceFieldExtension extension in rf.extensions.Values)
                {
                    temp = TypeDescriptor.GetProperties(extension);
                    foreach (PropertyDescriptor p in temp)
                    {
                        tempProps.Add(new BoundPropertyDescriptor(extension,
                            p, attributes));
                    }
                }
            }
            else if (component is EnumField)
            {
                EnumField ef = (EnumField)component;

                foreach (EnumFieldExtension extension in ef.extensions.Values)
                {
                    temp = TypeDescriptor.GetProperties(extension);
                    foreach (PropertyDescriptor p in temp)
                    {
                        tempProps.Add(new BoundPropertyDescriptor(extension,
                            p, attributes));
                    }
                }
            }
            return new PropertyDescriptorCollection(tempProps.ToArray());
        }

        public override System.ComponentModel.PropertyDescriptorCollection GetProperties(object component)
        {
            return this.GetProperties(component, null);
        }

        // Provides the name for the property tab.
        public override string TabName
        {
            get
            {
                return "Extensions";
            }
        }

        // Provides an image for the property tab.
        public override System.Drawing.Bitmap Bitmap
        {
            get
            {
                Bitmap bmp = new Bitmap(DeserializeFromBase64Text(img));
                return bmp;
            }
        }

        // This method can be used to retrieve an Image from a block of Base64-encoded text.
        private Image DeserializeFromBase64Text(string text)
        {
            Image img = null;
            byte[] memBytes = Convert.FromBase64String(text);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(memBytes);
            img = (Image)formatter.Deserialize(stream);
            stream.Close();
            return img;
        }
    }
}