using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    public class MySplitContainer : System.Windows.Forms.SplitContainer, ILayoutControl
    {
        private string GetParentFormName()
        {
            int idx = 0;
            System.Windows.Forms.Control control = this.Parent;
            while (control != null && !(control is System.Windows.Forms.Form))
            {
                control = control.Parent;
                idx += 1;
            }
            return control.Name + "." + idx.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool LoadLayout()
        {
            if (this.FixedPanel != System.Windows.Forms.FixedPanel.None)
                return true;

            try
            {
                AMS.Profile.IProfile m_profile = Feng.Windows.SystemProfileFile.DefaultUserProfile;

                int distance = m_profile.GetValue(GetParentFormName(), "SplitterDistance", -1);
                int width = m_profile.GetValue(GetParentFormName(), "SplitterWidth", -1);
                if (distance != -1 && width != -1)
                {
                    if (this.Orientation == System.Windows.Forms.Orientation.Vertical)
                    {
                        this.SplitterDistance = this.Width * distance / width;
                    }
                    else
                    {
                        this.SplitterDistance = this.Height * distance / width;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveLayout()
        {
            if (this.FixedPanel != System.Windows.Forms.FixedPanel.None)
                return true;

            try
            {
                AMS.Profile.IProfile m_profile = Feng.Windows.SystemProfileFile.DefaultUserProfile;
                m_profile.SetValue(GetParentFormName(), "SplitterDistance", (int)(this.SplitterDistance));
                if (this.Orientation == System.Windows.Forms.Orientation.Vertical)
                {
                    m_profile.SetValue(GetParentFormName(), "SplitterWidth", (int)(this.Width));
                }
                else
                {
                    m_profile.SetValue(GetParentFormName(), "SplitterWidth", (int)(this.Height));
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return false;
            }
            return true;
        }
    }
}
