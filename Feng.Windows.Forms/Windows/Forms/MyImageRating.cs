using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Forms
{
    public class MyImageRating : StarRatingControl, IDataValueControl
    {
        #region "IDataValueControl"

        /// <summary>
        /// 
        /// </summary>
        public virtual object SelectedDataValue
        {
            get { return base.StarCount; }
            set
            {
                if (value == null)
                {
                    base.SelectedStar = 0;
                }
                else
                {
                    try
                    {
                        int? d = Feng.Utils.ConvertHelper.ToInt(value);
                        if (!d.HasValue)
                        {
                            base.SelectedStar = 0;
                        }
                        else
                        {
                            base.SelectedStar = d.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyImageRating's SelectedDataValue must be Int32", ex);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// ReadOnly = !Enable
        /// </summary>
        public bool ReadOnly
        {
            get { return !base.Enabled; }
            set
            {
                if (base.Enabled != !value)
                {
                    base.Enabled = !value;
                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;

        /// <summary>
        /// 对显示控件设置ReadOnly
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion
    }
}
