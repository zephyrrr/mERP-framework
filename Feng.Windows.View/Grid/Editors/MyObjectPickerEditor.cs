using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Xceed.Editors;
using Xceed.Grid;
using Xceed.UI;
using Xceed.Utils;
using Feng.Windows.Forms;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// MyObjectPicker输入Editor
    /// </summary>
    public class MyObjectPickerEditor : Xceed.Grid.Editors.TextEditor
    {
        /// <summary>
        /// Dispose
        /// Because this.TemplateControl is not override, but new
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose单个editor的时候，不能dispose整个template
                this.TemplateControl.Dispose();
            }

            base.Dispose(disposing);
        }

        public MyObjectPickerEditor()
            : this(new MyObjectPicker())
        {
            this.InitializeTemplateControl();
        }

        //private string m_windowId, m_displayMember, m_searchExpression;
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public MyObjectPickerEditor(string windowId, string displayMember, string searchExpression)
        //    : this(new MyObjectPicker())
        //{
        //    this.m_windowId = windowId;
        //    this.m_displayMember = displayMember;
        //    this.m_searchExpression = searchExpression;

        //    Feng.Utils.ControlFactory.InitObjectPicker(this.TemplateControl, windowId, displayMember, searchExpression);

        //    this.InitializeTemplateControl();
        //}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyObjectPickerEditor(MyObjectPicker template)
            : base(template)
        {
        }

        //internal static void CommonSetControlAppearance(WinComboBox control, Cell cell)
        //{
        //    TextEditor.CommonSetControlAppearance(control, cell);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        /// <param name="returnDataType"></param>
        /// <returns></returns>
        protected override object GetControlValueCore(Control control, Cell cell, System.Type returnDataType)
        {
            MyObjectPicker box = control as MyObjectPicker;
            return box.SelectedDataValue;
        }

        private void InitializeTemplateControl()
        {
            this.TemplateControl.TextBoxArea.SelectOnFocus = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKeyCore(Control control, Cell cell, Keys keyData)
        {
            //MyOptionPicker box = control as MyOptionPicker;
            if (((keyData == Keys.Right) || (keyData == Keys.Left)))
            {
                return false;
            }
            if ((keyData != Keys.Up) && (keyData != Keys.Down))
            {
                return base.IsInputKeyCore(control, cell, keyData);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void SetControlAppearanceCore(Control control, Cell cell)
        {
            //Feng.Utils.ReflectionHelper.RunStaticMethod(typeof(Xceed.Grid.Editors.ComboBoxEditor), "CommonSetControlAppearance", new object[] { control, cell });

            //CommonSetControlAppearance(control as WinComboBox, cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            MyObjectPicker box = control as MyObjectPicker;
            object obj2 = cell.Value;
            if (((obj2 == null) || (obj2 == DBNull.Value)) || obj2.Equals(cell.NullValue))
            {
                box.SelectedDataValue = null;
                box.TextBoxArea.RawText = string.Empty;
            }
            else
            {
                box.SelectedDataValue = obj2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new MyObjectPicker TemplateControl
        {
            get { return (base.TemplateControl as MyObjectPicker); }
        }


        /// <summary>
        /// CreateControl
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            MyObjectPicker ret = this.TemplateControl.Clone() as MyObjectPicker;
            ret.DisplayMember = this.TemplateControl.DisplayMember;
            ret.SearchExpressionParam = this.TemplateControl.SearchExpressionParam;

            for (int i = 0; i < this.TemplateControl.DropDownControl.Columns.Count; ++i)
            {
                ret.DropDownControl.Columns[i].Visible = this.TemplateControl.DropDownControl.Columns[i].Visible;
                ret.DropDownControl.Columns[i].ReadOnly = this.TemplateControl.DropDownControl.Columns[i].ReadOnly;
                ret.DropDownControl.Columns[i].Width = this.TemplateControl.DropDownControl.Columns[i].Width;
            }
            for (int i = 0; i < this.TemplateControl.DropDownControl.DataRows.Count; ++i)
            {
                ret.DropDownControl.DataRows[i].Visible = this.TemplateControl.DropDownControl.DataRows[i].Visible;
                ret.DropDownControl.DataRows[i].ReadOnly = this.TemplateControl.DropDownControl.DataRows[i].ReadOnly;
                ret.DropDownControl.DataRows[i].Height = this.TemplateControl.DropDownControl.DataRows[i].Height;
            }

            return ret;
        }

        protected override void DeactivateControlCore(Control control, Cell cell)
        {
            MyObjectPicker c = control as MyObjectPicker;
            for (int i = 0; i < this.TemplateControl.DropDownControl.Columns.Count; ++i)
            {
                this.TemplateControl.DropDownControl.Columns[i].Width = c.DropDownControl.Columns[i].Width;
            }

            base.DeactivateControlCore(control, cell);
        }
    }
}