using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.Grid;
using Feng.Windows.Forms;

namespace Feng.Grid.Editors
{
    public class MyObjectTextEditor : Xceed.Grid.Editors.TextEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyObjectTextEditor()
            : this(new MyObjectTextBox())
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyObjectTextEditor(MyObjectTextBox template)
            : base(template)
        {
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
            MyObjectTextBox box = control as MyObjectTextBox;
            return box.SelectedDataValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            MyObjectTextBox box = control as MyObjectTextBox;
            object obj2 = cell.Value;
            box.SelectedDataValue = obj2;
        }

        /// <summary>
        /// 
        /// </summary>
        public new MyObjectTextBox TemplateControl
        {
            get { return (base.TemplateControl as MyObjectTextBox); }
        }


        /// <summary>
        /// CreateControl
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            MyObjectTextBox ret = this.TemplateControl.Clone() as MyObjectTextBox;
            return ret;
        }
    }
}
