using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class MyThreeStateCheckbox : MyCheckBox
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyThreeStateCheckbox()
            : base()
        {
            this.CheckState = CheckState.Indeterminate;
            this.ThreeState = true;
        }

        /// <summary>
        /// Default ThreeState
        /// </summary>
        [DefaultValue(true)]
        public new bool ThreeState
        {
            get { return base.ThreeState; }
            set { base.ThreeState = value; }
        }

        /// <summary>
        /// Default CheckState
        /// </summary>
        [DefaultValue(System.Windows.Forms.CheckState.Indeterminate)]
        public new System.Windows.Forms.CheckState CheckState
        {
            get { return base.CheckState; }
            set { base.CheckState = value; }
        }

        /// <summary>
        /// SelectedDataValue = (bool)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override object SelectedDataValue
        {
            get
            {
                switch (base.CheckState)
                {
                    case CheckState.Unchecked:
                        return false;
                    case CheckState.Checked:
                        return true;
                    case CheckState.Indeterminate:
                        return null;
                    default:
                        throw new NotSupportedException("MyCheckBox's SelectedDataValue must be bool");
                }
            }
            set
            {
                if (value == null)
                {
                    this.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    try
                    {
                        bool b = Feng.Utils.ConvertHelper.ToBoolean(value).Value;
                        base.CheckState = b ? CheckState.Checked : CheckState.Unchecked;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyThreeStateCheckbox's SelectedDataValue must be bool", ex);
                    }
                }
            }
        }
    }
}