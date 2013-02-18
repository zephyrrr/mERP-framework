using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Xceed.Editors;
using Xceed.Grid;
using Xceed.UI;
using Xceed.Utils;
using Xceed.Grid.Editors;
using Feng.Windows.Forms;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// FreeComboBox ‰»ÎEditor
    /// </summary>
    public class MyFreeComboBoxEditor : Xceed.Grid.Editors.ComboBoxEditor
    {
        /// <summary>
        /// static Constructor
        /// </summary>
        static MyFreeComboBoxEditor()
        {
            MyFreeComboBoxEditor.ComboBoxEditorType = typeof (MyFreeComboBoxEditor);
        }

        /// <summary>
        /// Initializes a new instance of the ComboBoxEditor class.
        /// </summary>
        public MyFreeComboBoxEditor()
            : this(new MyFreeComboBox(MyFreeComboBoxEditor.ComboBoxEditorType, EnhancedBorderStyle.None))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ComboBoxEditor class specifying the WinComboBox
        /// control that will be used as a template.
        /// </summary>
        /// <param name="template">The WinComboBox to use as a template.</param>
        public MyFreeComboBoxEditor(MyFreeComboBox template)
            : base(template)
        {
            if (this.TemplateControl.BorderStyle == EnhancedBorderStyle.None)
            {
                this.TemplateControl.ImagePadding = new Xceed.Editors.Margins(0, 1, 0, 0);
            }
            this.InitializeTemplateControl();
        }

        /// <summary>
        /// Initializes a new instance of the ComboBoxEditor class specifying its data binding information.
        /// </summary>
        /// <param name="dataSource">The data source used to populate the combobox.</param>
        /// <param name="dataMember">The table to bind to within the <paramref name="dataSource" />.</param>
        /// <param name="valueMember">A string that specifies the field of the <paramref name="dataSource" /> from which
        /// to draw the value.</param>
        public MyFreeComboBoxEditor(object dataSource, string dataMember, string valueMember)
            : this(
                new MyFreeComboBox(MyFreeComboBoxEditor.ComboBoxEditorType, EnhancedBorderStyle.None, dataSource,
                                   dataMember, valueMember))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ComboBoxEditor class specifying its data binding information.
        /// </summary>
        /// <param name="dataSource">The data source used to populate the combobox.</param>
        /// <param name="dataMember">The table to bind to within the <paramref name="dataSource" />.</param>
        /// <param name="valueMember">A string that specifies the field of the <paramref name="dataSource" /> from which
        /// to draw the value.</param>
        /// <param name="displayFormat">The format with which the selected item is displayed in the combobox.</param>
        /// <remarks><para>
        /// Only the column names specified by <paramref name="displayFormat" /> and the <paramref name="valueMember" />
        /// parameters will be created in the dropdown. For example, setting <paramref name="displayFormat" />
        /// to "%FirstName% %LastName%" will result in only the "FirstName" and "LastName" columns being
        /// created in the dropdown and, for example, "Naomi Robitaille" when the item is selected.
        /// The column specified by <paramref name="valueMember" /> will also be created, however it will not visible
        /// unless it is also part of the <paramref name="displayFormat" />.
        /// </para></remarks>
        public MyFreeComboBoxEditor(object dataSource, string dataMember, string valueMember, string displayFormat)
            : this(
                new MyFreeComboBox(MyFreeComboBoxEditor.ComboBoxEditorType, EnhancedBorderStyle.None, dataSource,
                                   dataMember, valueMember, displayFormat))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ComboBoxEditor class specifying its data binding information.
        /// </summary>
        /// <remarks><para>
        /// Only the column names specified by <paramref name="displayFormat" /> and the <paramref name="valueMember" />
        /// parameters will be created in the dropdown. For example, setting <paramref name="displayFormat" />
        /// to "%FirstName% %LastName%" will result in only the "FirstName" and "LastName" columns being
        /// created in the dropdown and, for example, "Naomi Robitaille" when the item is selected.
        /// The column specified by <paramref name="valueMember" /> will also be created, however it will not visible
        /// unless it is also part of the <paramref name="displayFormat" />.
        /// </para></remarks>
        /// <param name="dataSource">The data source used to populate the combobox.</param>
        /// <param name="dataMember">The table to bind to within the <paramref name="dataSource" />.</param>
        /// <param name="valueMember">The field of the <paramref name="dataSource" /> from which
        /// to draw the value.</param>
        /// <param name="imageMember">A string that specifies the field of the <paramref name="dataSource" /> from which
        /// to draw the image.</param>
        /// <param name="imagePosition">An <see cref="T:Xceed.Editors.ImagePosition" /> value representing the position of the image.</param>
        /// <param name="imageSize">A <see cref="T:System.Drawing.Size" /> structure representing the size of the editors's image.</param>
        /// <param name="displayFormat">The format with which the selected item is displayed in the combobox.</param>
        public MyFreeComboBoxEditor(object dataSource, string dataMember, string valueMember, string imageMember,
                                    Xceed.Editors.ImagePosition imagePosition, Size imageSize, string displayFormat)
            : this(
                new MyFreeComboBox(MyFreeComboBoxEditor.ComboBoxEditorType, EnhancedBorderStyle.None, dataSource,
                                   dataMember, valueMember, imageMember, imagePosition, imageSize, displayFormat))
        {
        }

        /// <summary>
        /// Retrieves the value of the <paramref name="control" />.
        /// </summary>
        /// <param name="control">The control from which to retrieve the value.</param>
        /// <param name="cell">The cell currently being edited by the <paramref name="control" />.</param>
        /// <param name="returnDataType">A <see cref="T:System.Type" /> representing the datatype to which the
        /// <paramref name="control" />'s value must be converted.</param>
        /// <returns>The value that will be assigned to the cell being edited by the <paramref name="control" />, in the
        /// correct datatype.</returns>
        /// <remarks><para>
        /// If the cell's <see cref="P:Xceed.Grid.Cell.ParentRow" />'s <see cref="P:Xceed.Grid.CellRow.EnforceCellDataTypes" /> is <see langword="false" />,
        /// <paramref name="returnDataType" /> will be "object"; otherwise, <paramref name="returnDataType" /> will by equal to
        /// the parent column's <see cref="P:Xceed.Grid.Column.DataType" />.
        /// </para></remarks>
        protected override object GetControlValueCore(Control control, Cell cell, System.Type returnDataType)
        {
            //MyFreeComboBox box1 = control as MyFreeComboBox;
            //((MyComboBoxTextBoxArea)box1.TextBoxArea).OwnTextValidating();

            object res = base.GetControlValueCore(control, cell, returnDataType);

            return res;
        }

        private void InitializeTemplateControl()
        {
            this.TemplateControl.TextBoxArea.SelectOnFocus = true;
        }

        /// <summary>
        /// Assigns the value of the <paramref name="cell" /> to the <paramref name="control" />.
        /// </summary>
        /// <param name="control">The control to which to assign the cell's <see cref="P:Xceed.Grid.Cell.Value" />.</param>
        /// <param name="cell">The cell whose <see cref="P:Xceed.Grid.Cell.Value" /> to assign to the <paramref name="control" />.</param>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            //MyFreeComboBox box1 = control as MyFreeComboBox;

            base.SetControlValueCore(control, cell);
        }

        /// <summary>
        /// Gets the <see cref="T:System.Windows.Forms.Control" /> that will be used as a template to create the controls
        /// that will edit the content of cells.
        /// </summary>
        /// <value>A reference to the <see cref="T:System.Windows.Forms.Control" /> that will be used as a template to create the
        /// controls that will edit the content of cells.</value>
        /// <remarks><para>
        /// In the case where <see cref="T:Xceed.Grid.CreateControlMode" /> is set to <see cref="T:Xceed.Grid.CreateControlMode" />.SingleInstance (default),
        /// the template control will be used directly. If CreateControlMode is set to ClonedInstance,
        /// the <see cref="M:Xceed.Grid.Editors.TextEditor.CreateControl" /> method will be called to clone the template control when needed.
        /// </para></remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         Description(
             "The Control that will be used as a template to create the controls that will edit the content of cells."),
         Category("Data")]
        public new MyFreeComboBox TemplateControl
        {
            get { return (base.TemplateControl as MyFreeComboBox); }
        }

        /// <summary>
        /// CreateControl
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            return (this.TemplateControl.Clone() as Control);
        }

        internal static readonly System.Type ComboBoxEditorType;
    }
}