using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid.Editors;
using Xceed.Grid;
using Xceed.Editors;
using System.Windows.Forms;
using Feng.Windows.Forms;

namespace Feng.Grid.Editors
{
    public class MyIntegerEditor : MyNumericEditor
    {
        public MyIntegerEditor()
            : base(NumericTextBoxType.Integer)
        {
        }
    }
    public class MyLongEditor : MyNumericEditor
    {
        public MyLongEditor()
            : base(NumericTextBoxType.Long)
        {
        }
    }
    public class MyCurrencyEditor : MyNumericEditor
    {
        public MyCurrencyEditor()
            : base(NumericTextBoxType.Currency)
        {
        }
    }

    /// <summary>
    /// 下拉显示计算器 。
    /// </summary>
    public class MyNumericEditor : NumericEditor
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
                // ??dispose单个editor的时候，不能dispose整个template
                if (this.TemplateControl != null)
                {
                    this.TemplateControl.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected MyNumericEditor(MyNumericTextBox template)
            : base(template)
        {
            this.InitializeTemplateControl();
        }

        private void InitializeTemplateControl()
        {
            this.TemplateControl.TextBoxArea.SelectOnFocus = true;

            this.TemplateControl.SetNumericTextBoxFormat(m_type, m_format);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyNumericEditor()
            : this(NumericTextBoxType.Number)
        {
        }

        public MyNumericEditor(NumericTextBoxType textBoxType)
            : this(textBoxType, MyNumericTextBox.DefaultNumericTextBoxFormat)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="textBoxType"></param>
        /// <param name="format"></param>
        public MyNumericEditor(NumericTextBoxType textBoxType, string format)
            : this(new MyNumericTextBox(textBoxType))
        {
            m_format = format;
            m_type = textBoxType;

            InitializeTemplateControl();
        }

        private string m_format;
        private NumericTextBoxType m_type;
        protected override void SetControlAppearanceCore(System.Windows.Forms.Control control, Cell cell)
        {
            Feng.Utils.ReflectionHelper.RunStaticMethod(typeof(Xceed.Grid.Editors.TextEditor), "CommonSetControlAppearance", new object[] { control, cell });

            //NumberFormatInfo instance = NumberFormatInfo.GetInstance(cell.FormatProvider);
            //string formatSpecifier = cell.FormatSpecifier;
            //control.EditFormatProvider = instance;
            //control.DisplayFormatProvider = instance;
            //control.DisplayFormatSpecifier = formatSpecifier;

            var t = control as MyNumericTextBox;
            t.SetNumericTextBoxFormat(m_type, m_format);

            t.NullValue = cell.NullValue;
            t.DisplayNullText = cell.NullText;

            //base.SetControlAppearanceCore(control, cell);
        }

        public new MyNumericTextBox TemplateControl
        {
            get
            {
                return (base.TemplateControl as MyNumericTextBox);
            }
        }

        /// <summary>
        /// CreateControl
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            MyNumericTextBox c = this.TemplateControl.Clone() as MyNumericTextBox;
            return c;
        }
    }
}