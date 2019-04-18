using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NitroCast.Core;

namespace NitroCast
{
    public partial class EnumEditor : Form
    {
        ModelEnum editEnum;

        public EnumEditor()
        {
            InitializeComponent();
            typeComboBox.Items.Add(ModelEnumUnderlyingType.Byte);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.Int16);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.Int32);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.Int64);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.SByte);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.UInt16);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.UInt32);
            typeComboBox.Items.Add(ModelEnumUnderlyingType.UInt64);

            this.Icon = Properties.Resources.EnumEditor;
        }

        public EnumEditor(ModelEnum modelEnum) : this()
        {
            nameTextBox.Text = modelEnum.Name;
            editEnum = modelEnum;
            editEnum.Editor = this;

            Text = modelEnum.Name;
            nameTextBox.Text = modelEnum.Name;
            namespaceTextBox.Text = modelEnum.Namespace;
            typeComboBox.SelectedItem = editEnum.UnderlyingType;
        }

        private void EnumEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            editEnum.Editor = null;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            editEnum.Name = nameTextBox.Text;
            editEnum.Namespace = namespaceTextBox.Text;
            editEnum.UnderlyingType = (ModelEnumUnderlyingType)
                typeComboBox.SelectedItem;
        }
    }
}