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
using Feng.Utils;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// MyOptionPicker输入Editor
    /// </summary>
    public class MyOptionPickerEditor : Xceed.Grid.Editors.TextEditor, INameValueControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetDisplay(object value)
        {
            return NameValueControlHelper.GetMultiString(m_nvName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public object GetValue(string displayText)
        {
            return NameValueControlHelper.GetMultiValue(m_nvName, displayText);
        }

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

        private string m_nvName;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="nvName"></param>
        public MyOptionPickerEditor(string nvName)
            : this(new MyOptionPicker())
        {
            this.m_nvName = nvName;

            this.InitializeTemplateControl();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyOptionPickerEditor(MyOptionPicker template)
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
            MyOptionPicker box = control as MyOptionPicker;
            if (box.SelectedDataValue == null)
            {
                return cell.NullValue;
            }
            System.Type underlyingType = Nullable.GetUnderlyingType(returnDataType);
            if (underlyingType != null)
            {
                returnDataType = underlyingType;
            }
            if (string.IsNullOrEmpty(box.ValueMember))
            {
                return Convert.ChangeType(box.TextBoxArea.RawText, returnDataType, cell.FormatProvider);
            }
            if (returnDataType != typeof (object))
            {
                return Convert.ChangeType(box.SelectedDataValue, returnDataType, cell.FormatProvider);
            }
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
            Feng.Utils.ReflectionHelper.RunStaticMethod(typeof(Xceed.Grid.Editors.TextEditor), "CommonSetControlAppearance", new object[] { control, cell });
            //CommonSetControlAppearance(control as WinComboBox, cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            MyOptionPicker box = control as MyOptionPicker;
            object obj2 = cell.Value;
            if (((obj2 == null) || (obj2 == DBNull.Value)) || obj2.Equals(cell.NullValue))
            {
                box.SelectedDataValue = null;
                box.TextBoxArea.RawText = string.Empty;
            }
            else if (string.IsNullOrEmpty(box.ValueMember))
            {
                box.SelectedDataValue = null;
                box.TextBoxArea.RawText = (string) Convert.ChangeType(obj2, typeof (string), cell.FormatProvider);
            }
            else
            {
                //Column column = box.DropDownControl.Columns[box.ValueMember];
                //if ((column != null) && (column.DataType != typeof (object)))
                //{
                //    System.Type dataType = column.DataType;
                //    System.Type underlyingType = Nullable.GetUnderlyingType(dataType);
                //    if (underlyingType != null)
                //    {
                //        dataType = underlyingType;
                //    }
                //    box.SelectedDataValue = Convert.ChangeType(obj2, dataType, cell.FormatProvider);
                //}
                //else
                {
                    box.SelectedDataValue = obj2.ToString();
                }
                if ((box.SelectedDataValue == null))
                {
                    box.TextBoxArea.RawText = (string) Convert.ChangeType(obj2, typeof (string), cell.FormatProvider);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new MyOptionPicker TemplateControl
        {
            get { return (base.TemplateControl as MyOptionPicker); }
        }


        /// <summary>
        /// CreateControl
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            MyOptionPicker ret = this.TemplateControl.Clone() as MyOptionPicker;
            for (int i = 0; i < this.TemplateControl.DropDownControl.Columns.Count; ++i)
            {
                ret.DropDownControl.Columns[i].Visible = this.TemplateControl.DropDownControl.Columns[i].Visible;
                ret.DropDownControl.Columns[i].ReadOnly = this.TemplateControl.DropDownControl.Columns[i].ReadOnly;
                //ret.DropDownControl.Columns[i].Width = this.TemplateControl.DropDownControl.Columns[i].Width;
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
            MyOptionPicker c = control as MyOptionPicker;
            for (int i = 0; i < this.TemplateControl.DropDownControl.Columns.Count; ++i)
            {
                this.TemplateControl.DropDownControl.Columns[i].Width = c.DropDownControl.Columns[i].Width;
            }

            base.DeactivateControlCore(control, cell);
        }
    }
}