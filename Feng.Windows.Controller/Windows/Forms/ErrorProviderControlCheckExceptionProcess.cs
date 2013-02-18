using System;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 用于WinForm的<see cref="IControlCheckExceptionProcess"/>
    /// </summary>
    public class ErrorProviderControlCheckExceptionProcess : IControlCheckExceptionProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && m_errorProvider != null)
            {
                m_errorProvider.Clear();
                m_errorProvider.Dispose();
                m_errorProvider = null;
            }
        }

        private System.Windows.Forms.ErrorProvider m_errorProvider = new System.Windows.Forms.ErrorProvider();

        /// <summary>
        ///   Shows a blinking icon next to the textbox with an error message. </summary>
        /// <param name="control"></param>
        /// <param name="message">
        ///   The message to show when the cursor is placed over the icon. </param>
        /// <remarks>
        ///   Although doing so is not expected, this method may be overriden by derived classes. </remarks>
        private void ShowErrorIcon(Control control, string message)
        {
            if (control == null)
            {
                return;
            }

            if (m_errorProvider == null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }
                m_errorProvider = new ErrorProvider();
                m_errorProvider.BlinkRate = 500;
                m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            }
            m_errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
            m_errorProvider.SetIconPadding(control, 0);
            m_errorProvider.SetError(control, message);
        }

        /// <summary>
        /// ClearError
        /// </summary>
        public void ClearAllError()
        {
            if (m_firstInvalidControl != null)
            {
                ShowErrorIcon(m_firstInvalidControl, string.Empty);
                m_firstInvalidControl = null;
            }
            m_errorProvider.Clear();
        }

        private Control m_firstInvalidControl;

        /// <summary>
        /// 缺省处理ControlValueNullException方式
        /// </summary>
        /// <param name="ex"></param>
        public void ShowError(object invalidControl, string msg)
        {
            if (invalidControl == null)
            {
                throw new ArgumentNullException("invalidControl");
            }

            IWindowControl windowControl = invalidControl as IWindowControl;
            if (windowControl != null)
            {
                //ClearError();

                if (m_firstInvalidControl == null)
                {
                    m_firstInvalidControl = windowControl.Control;
                    //if (ex.InvalidDataControl is Feng.Windows.Forms.DataControlWrapper)
                    //{
                    //    m_invalidControl = ex.InvalidDataControl as Control;
                    //}
                    m_firstInvalidControl.Focus();
                }

                ShowErrorIcon(windowControl.Control, msg);
            }
            else
            {
                ServiceProvider.GetService<IMessageBox>().ShowWarning(msg);
            }
        }
    }
}