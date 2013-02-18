using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.Validation;
using Feng.Windows.Forms;

namespace Feng.Windows.Forms
{
    internal class ValidationHelper : IValidationManager, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose()
        {
            this.validationProvider1.Icon.Dispose();

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            validationProvider1.Dispose();
            m_cm = null;
        }

        private IControlManager m_cm;

        public ValidationHelper(IControlManager dm)
        {
            m_cm = dm;
        }

        #region "Validate"

        private Control GetControl(IDataControl dc)
        {
            Control vc = null;
            IWindowControl ldc = dc as IWindowControl;
            if (ldc != null)
            {
                vc = ldc.Control;
            }
            else
            {
                Control c = dc as Control;
                if (c != null)
                {
                    vc = c;
                }
            }
            return vc;
        }

        private MyValidationProvider validationProvider1 = new Feng.Windows.Forms.MyValidationProvider();

        /// <summary>
        /// AddValidation
        /// U should call it before load data
        /// </summary>
        /// <param name="dataControlName"></param>
        /// <param name="expression"></param>
        public void SetValidation(string dataControlName, ValidationExpression expression)
        {
            IDataControl dc = m_cm.DisplayManager.DataControls[dataControlName];

            Control vc = GetControl(dc);
            if (vc != null)
            {
                this.validationProvider1.SetValidationExpression(vc, expression);

                if (dc is IWindowControl)
                {
                    (dc as IWindowControl).Control.Leave -= new EventHandler(Control_Leave);
                    (dc as IWindowControl).Control.Leave += new EventHandler(Control_Leave);
                }
                //dc.SelectedDataValueChanged -= new EventHandler(dc_SelectedDataValueChanged);
                //dc.SelectedDataValueChanged += new EventHandler(dc_SelectedDataValueChanged);

                dc.ReadOnlyChanged += new EventHandler(dc_ReadOnlyChanged);
                dc.ReadOnlyChanged += new EventHandler(dc_ReadOnlyChanged);
            }
        }

        void Control_Leave(object sender, EventArgs e)
        {
            IDataControl dc = (sender as Control).Parent as IDataControl;
            m_cm.CheckControlValue(dc);
        }

        void dc_ReadOnlyChanged(object sender, EventArgs e)
        {
            IDataControl dc = sender as IDataControl;
            if (dc.ReadOnly)
            {
                m_cm.ControlCheckExceptionProcess.ShowError(dc, null);
            }
        }

        //void dc_SelectedDataValueChanged(object sender, EventArgs e)
        //{
        //    IDataControl dc = sender as IDataControl;
        //    m_cm.CheckControlValue(dc);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataControlName"></param>
        public void RemoveValidation(string dataControlName)
        {
            IDataControl dc = m_cm.DisplayManager.DataControls[dataControlName];

            Control vc = GetControl(dc);
            if (vc != null)
            {
                this.validationProvider1.SetValidationExpression(vc, null);
                this.validationProvider1.SetValidationError(vc, string.Empty);
            }
        }

        public void SetIcon(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                this.validationProvider1.Icon = new System.Drawing.Icon(typeof(ErrorProvider), "Error.ico");
            }
            else
            {
                this.validationProvider1.Icon = Feng.Windows.PdnResources.GetIconFromImage(ImageResource.Get("Icons." + resourceName + ".png").Reference);
            }
        }
        internal string ValidateControl(string dataControlName)
        {
            IDataControl dc = m_cm.DisplayManager.DataControls[dataControlName];
            Control vc = GetControl(dc);
            if (vc != null)
            {
                bool b = this.validationProvider1.Validate(vc, false, false);
                if (!b)
                {
                    string error = this.validationProvider1.GetValidationError(vc);
                    this.validationProvider1.SetValidationError(vc, string.Empty);
                    return error;
                }
            }

            return null;
        }

        #endregion
    }
}