using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 可保存Position和Size的Form
    /// </summary>
    public partial class PositionPersistForm : MyForm, ILayoutControl
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public PositionPersistForm()
        {
            InitializeComponent();
        }

        private AMS.Profile.IProfile m_profile = Feng.Windows.SystemProfileFile.DefaultUserProfile;

        private string GetFullName()
        {
            if (this.Owner != null)
            {
                return this.Owner.Name + "." + this.Name;
            }
            else
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadLayout();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            SaveLayout();

            base.OnClosed(e);
        }

        bool ILayoutControl.LoadLayout()
        {
            try
            {
                int width = m_profile.GetValue("Forms." + GetFullName() + ".Position", "Width", this.Width);
                int height = m_profile.GetValue("Forms." + GetFullName() + ".Position", "Height", this.Height);
                this.Size = new Size(width, height);

                int top = m_profile.GetValue("Forms." + GetFullName() + ".Position", "Top", this.Top);
                int left = m_profile.GetValue("Forms." + GetFullName() + ".Position", "Left", this.Left);
                this.Location = new Point(left, top);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ILayoutControl.SaveLayout()
        {
            try
            {
                m_profile.SetValue("Forms." + GetFullName() + ".Position", "Width", this.Width);
                m_profile.SetValue("Forms." + GetFullName() + ".Position", "Height", this.Height);
                m_profile.SetValue("Forms." + GetFullName() + ".Position", "Top", this.Top);
                m_profile.SetValue("Forms." + GetFullName() + ".Position", "Left", this.Left);
                return false;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return true;
        }
    }
}