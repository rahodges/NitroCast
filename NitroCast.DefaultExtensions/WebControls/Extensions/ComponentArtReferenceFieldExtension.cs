using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NitroCast.Core;
using NitroCast.Core.Extensions;
using System.Web.UI.WebControls;

namespace NitroCast.Extensions.Default
{
    public class ComponentArtReferenceFieldExtension : ReferenceFieldExtension
    {
        string cssClass;
        string hoverCssClass;
        string focusedCssClass;
        string textBoxCssClass;
        string dropDownCssClass;
        string itemCssClass;
        string itemHoverCssClass;
        string selectedItemCssClass;
        string dropHoverImageUrl;
        string dropImageUrl;
        System.Web.UI.WebControls.Unit width;

        [Category("ComponentArt"),
            Description("The CSS class to use for the item."),
            DefaultValue("comboBox")]
        public string CssClass
        {
            get
            {
                return cssClass;
            }
            set
            {
                cssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for the item, on hover."),
            DefaultValue("comboBoxHover")]
        public string HoverCssClass
        {
            get
            {
                return hoverCssClass;
            }
            set
            {
                hoverCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for the item when it's focused."),
            DefaultValue("comboBoxHover")]
        public string FocusedCssClass
        {
            get
            {
                return focusedCssClass;
            }
            set
            {
                focusedCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for the text box."),
            DefaultValue("comboTextBox")]
        public string TextBoxCssClass
        {
            get
            {
                return textBoxCssClass;
            }
            set
            {
                textBoxCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for the dropdown."),
            DefaultValue("comboDropDown")]
        public string DropDownCssClass
        {
            get
            {
                return dropDownCssClass;
            }
            set
            {
                dropDownCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for dropdown items."),
            DefaultValue("comboItem")]
        public string ItemCssClass
        {
            get
            {
                return itemCssClass;
            }
            set
            {
                itemCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for dropdown items on hover."),
            DefaultValue("comboItemHover")]
        public string ItemHoverCssClass
        {
            get
            {
                return itemHoverCssClass;
            }
            set
            {
                itemHoverCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The CSS class to use for the selected item."),
            DefaultValue("comboItemHover")]
        public string SelectedItemCssClass
        {
            get
            {
                return selectedItemCssClass;
            }
            set
            {
                selectedItemCssClass = value;
            }
        }

        [Category("ComponentArt"),
            Description("The image to use for expanding the dropdown, on hover."),
            DefaultValue("combobox_images/drop_hover.gif")]
        public string DropHoverImageUrl
        {
            get
            {
                return dropHoverImageUrl;
            }
            set
            {
                dropHoverImageUrl = value;
            }
        }

        [Category("ComponentArt"),
            Description("The image to use for expanding the dropdown."),
            DefaultValue("combobox_images/drop.gif")]
        public string DropImageUrl
        {
            get
            {
                return dropImageUrl;
            }
            set
            {
                dropImageUrl = value;
            }
        }

        [Category("ComponentArt"),
            Description("The width of the item."),            
            DefaultValue(typeof(Unit), "300")]
        public Unit Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public ComponentArtReferenceFieldExtension()
            : base()
        {
            cssClass = "comboBox";
            hoverCssClass = "comboBoxHover";
            focusedCssClass = "comboBoxHover";
            textBoxCssClass = "comboTextBox";
            dropDownCssClass = "comboDropDown";
            itemCssClass = "comboItem";
            itemHoverCssClass = "comboItemHover";
            selectedItemCssClass = "comboItemHover";
            dropHoverImageUrl = "combobox_images/drop_hover.gif";
            dropImageUrl = "combobox_images/drop.gif";
            width = Unit.Pixel(300);
        }

        public static ComponentArtReferenceFieldExtension Find(ReferenceField f)
        {
            return (ComponentArtReferenceFieldExtension)
                f.GetExtension(typeof(ComponentArtReferenceFieldExtension));
        }
    }
}
