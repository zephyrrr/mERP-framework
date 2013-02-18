using System;
using System.Collections;
using System.ComponentModel;
using System.Text;

namespace Feng.Windows.Forms
{
    public class MyMultilineTextBox2 : MyTextBox, IMultiDataValueControl
    {
        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyMultilineTextBox2()
            : base()
        {
            base.Multiline = true;
            base.MaxLength = 2048;
        }

        /// <summary>
        /// Default Multiline 
        /// </summary>
        [DefaultValue(true)]
        public new bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = value; }
        }


        /// <summary>
        /// Default MaxLength 
        /// </summary>
        [DefaultValue(2048)]
        public new Int32 MaxLength
        {
            get { return base.MaxLength; }
            set { base.MaxLength = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override object SelectedDataValue
        {
            get
            {
                string text = this.Text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }
                else
                {
                    return text;
                }
            }
            set
            {
                base.SelectedDataValue = value;
            }
        }

        /// <summary>
        /// 以数组形式表示的SelectedDataValue
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedDataValues
        {
            get
            {
                ArrayList ret = new ArrayList();
                if (this.SelectedDataValue != null)
                {
                    string s = this.SelectedDataValue.ToString();
                    string[] ss = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    ret.AddRange(ss);
                }
                return ret;
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    this.SelectedDataValue = value;
                }
                else
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (object i in value)
                        {
                            sb.Append(i.ToString());
                            sb.Append(Environment.NewLine);
                        }
                        this.SelectedDataValue = sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyMultilineTextBox's SelectedDataValue must be object[]", ex);
                    }
                }
            }
        }
    }
}
