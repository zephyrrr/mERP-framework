using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Editors;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 可自由输入的ComboBox
    /// </summary>
    public class MyFreeComboBox : MyComboBox
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyFreeComboBox()
            : base()
        {
            base.ValidateText = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyFreeComboBox(MyFreeComboBox template)
            : base(template)
        {
            base.ValidateText = template.ValidateText;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        internal MyFreeComboBox(Type callerType, EnhancedBorderStyle borderStyle)
            : base(callerType, borderStyle)
        {
            base.ValidateText = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        internal MyFreeComboBox(Type callerType, EnhancedBorderStyle borderStyle, object dataSource, string dataMember,
                                string valueMember)
            : base(callerType, borderStyle, dataSource, dataMember, valueMember)
        {
            base.ValidateText = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="displayFormat"></param>
        internal MyFreeComboBox(Type callerType, EnhancedBorderStyle borderStyle, object dataSource, string dataMember,
                                string valueMember, string displayFormat)
            : base(callerType, borderStyle, dataSource, dataMember, valueMember, displayFormat)
        {
            base.ValidateText = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="imageMember"></param>
        /// <param name="imagePosition"></param>
        /// <param name="imageSize"></param>
        /// <param name="displayFormat"></param>
        internal MyFreeComboBox(Type callerType, EnhancedBorderStyle borderStyle, object dataSource, string dataMember,
                                string valueMember, string imageMember, ImagePosition imagePosition, Size imageSize,
                                string displayFormat)
            : base(
                callerType, borderStyle, dataSource, dataMember, valueMember, imageMember, imagePosition, imageSize,
                displayFormat)
        {
            base.ValidateText = false;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyFreeComboBox(this);
        }

        /// <summary>
        /// 设置ComboBox值时与界面显示对应的内部值
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override object SelectedDataValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return null;
                }
                else
                {
                    return this.Text;
                }
            }
            set
            {
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.ToString();
                }
            }
        }
    }
}