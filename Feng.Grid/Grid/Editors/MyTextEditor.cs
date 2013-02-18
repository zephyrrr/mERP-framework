using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.Grid.Editors;
using Xceed.Grid;
using Xceed.Editors;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// 文本输入Editor，当遇到空字符串时，返回null
    /// </summary>
    public class MyTextEditor : TextEditor
    {
        /// <summary>
        /// 默认CharacterCasing.Normal
        /// </summary>
        public MyTextEditor()
            : this("Normal")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">Upper, Lower, Normal</param>
        public MyTextEditor(string format)
            : base()
        {
            switch(format)
            {
                case "Upper":
                    this.TemplateControl.TextBoxArea.CharacterCasing = CharacterCasing.Upper;
                    break;
                case "Lower":
                    this.TemplateControl.TextBoxArea.CharacterCasing = CharacterCasing.Lower;
                    break;
                case "Normal":
                    this.TemplateControl.TextBoxArea.CharacterCasing = CharacterCasing.Normal;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public MyTextEditor(WinTextBox template)
            : base(template)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateControl"></param>
        /// <param name="propertyName"></param>
        /// <param name="handleActivationClick"></param>
        protected MyTextEditor(WinTextBox templateControl, string propertyName, bool handleActivationClick)
            : base(templateControl, propertyName, handleActivationClick)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            base.SetControlValueCore(control, cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        /// <param name="returnDataType"></param>
        /// <returns></returns>
        protected override object GetControlValueCore(Control control, Cell cell, System.Type returnDataType)
        {
            object res = base.GetControlValueCore(control, cell, returnDataType);

            if (res != null && string.IsNullOrEmpty(res.ToString()))
            {
                return null;
            }

            return res;
        }
    }
}