using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.Grid;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// 日期时间输入Editor
    /// </summary>
    public class DateTimeEditor : Xceed.Grid.Editors.TextEditor
    {
        ///// <summary>
        ///// default constructor
        ///// </summary>
        //public TimeEditor() : this("99:99", true)
        //{
        //}

        private string m_datetimeFormat;
        /// <summary>
        /// Constructor
        /// // "yyyy-MM-dd"; "HH:mm"; "yy-MM-dd HH:mm"; "MM-dd HH:mm";
        /// </summary>
        /// <param name="format"></param>
        public DateTimeEditor(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            m_datetimeFormat = format;

            /*http://doc.xceedsoft.com/products/Xceedgrid/Editors_WinTextBox_control.html
             * # 	Digits or white space
9 	Digits only
A 	Alpha-numeric values only
a 	Alpha-numeric values or white space
@ 	Letters only
& 	Any printable character (ASCII characters from 32 to 126 and 128 to 255)*/

            char digit = '9';
            string mask = format.Replace('y', digit).Replace('M', digit).Replace('d', digit).Replace('H', digit).Replace('h', digit)
                .Replace('m', digit).Replace('s', digit);

            Initialize(mask);
        }

        void TemplateControl_DoubleClick(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox; 
            if (!t.ReadOnly && (string.IsNullOrEmpty(t.Text) || t.Text == m_maskText))
            {
                t.Text = System.DateTime.Now.ToString(m_datetimeFormat);
            }
        }

        /// <summary>
        /// constructor with mask
        /// </summary>
        /// <param name="mask"></param>
        private void Initialize(string mask)
        {
            this.Mask = mask;

            m_maskText = mask.Replace('9', this.MaskPromptChar);
        }


        /// <summary>
        /// Value=null时的Text
        /// </summary>
        private string m_maskText;

        /// <summary>
        /// 从DateTime转化到Text
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            // No need to call base since everything is handled here.
            // Set the value of the cell to the control currently editing the cell, NOT to the TemplateControl.
            //
            // Verify that the cell's value is not null as null is not supported by the underlying
            // TrackBar control. If it is null, set 0 as the control's value instead.
            if ((cell.Value != null) && (cell.Value != DBNull.Value) && (cell.Value != cell.NullValue))
            {
                //DateTime time = (DateTime)cell.Value;

                ((Xceed.Editors.WinTextBox)control).TextBoxArea.Text = ((DateTime)cell.Value).ToString(m_datetimeFormat);
            }
            else
            {
                ((Xceed.Editors.WinTextBox)control).TextBoxArea.Text = string.Empty;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //protected override CreateControlMode CreateControlMode
        //{
        //    get
        //    {
        //        // We override CreateControlMode to return ClonedInstance so that we can 
        //        // support all the CellEditorDisplayConditions. 
        //        // 
        //        // Using a ClonedInstace is only possible when deriving. It is not possible when creating
        //        // a custom CellEditorManager using events.
        //        return CreateControlMode.ClonedInstance;
        //    }
        //}


        /// <summary>
        /// // Because we have overridden CreateControlMode, we must return a cloned instance
        /// //of our TemplateControl in the CreateControl method.
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            // If you do not want to do the clone manually, you can use the static CloneControl method.
            var d = Xceed.UI.ThemedControl.CloneControl(this.TemplateControl) as Xceed.Editors.WinTextBox;
            d.TextBoxArea.DoubleClick += new EventHandler(TemplateControl_DoubleClick);
            return d;
        }

        /// <summary>
        /// 从Text转化到DateTime
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        /// <param name="returnDataType"></param>
        /// <returns></returns>
        protected override object GetControlValueCore(Control control, Cell cell, Type returnDataType)
        {
            string text = ((Xceed.Editors.WinTextBox) control).Text;

            if (text == m_maskText)
            {
                return null;
            }

            DateTime dt;
            bool convert = Feng.Windows.Forms.MyDateTimeTextBox.TryParseExact(text, m_datetimeFormat, out dt);
            if (!convert)
            {
                return cell.Value;
            }

            return dt;

            // Return the value of the control that is currently editing the cell and not the TemplateControl.
            //return Convert.ChangeType(((Xceed.Editors.WinTextBox)control).Text, returnDataType);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="control"></param>
        ///// <param name="cell"></param>
        ///// <param name="keyData"></param>
        ///// <returns></returns>
        //protected override bool IsInputKeyCore(Control control, Cell cell, Keys keyData)
        //{
        //    // There is no need to override IsInputKeyCore as base will call the underlying control's
        //    // IsInputKey method automatically.
        //    return base.IsInputKeyCore(control, cell, keyData);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cell"></param>
        ///// <param name="keyData"></param>
        ///// <returns></returns>
        //protected override bool IsActivationKeyCore(Cell cell, Keys keyData)
        //{
        //    // There is no need to call base as nothing happens there.
        //    return base.IsActivationKeyCore(cell, keyData);
        //}
    }
}