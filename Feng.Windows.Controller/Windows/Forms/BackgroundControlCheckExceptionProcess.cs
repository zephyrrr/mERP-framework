using System;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class BackgroundControlCheckExceptionProcess : IControlCheckExceptionProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            ClearAllError();
        }

        private Control m_invalidControl;
        private System.Drawing.Color m_bakColor;

        /// <summary>
        /// 清除错误
        /// </summary>
        public void ClearAllError()
        {
            if (m_invalidControl != null)
            {
                m_invalidControl.BackColor = m_bakColor;
                m_invalidControl = null;
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        public void ShowError(object invalidControl, string msg)
        {
            IWindowControl windowControl = invalidControl as IWindowControl;
            if (windowControl != null)
            {
                //ClearAllError();

                m_invalidControl = windowControl.Control;
                m_bakColor = m_invalidControl.BackColor;
                m_invalidControl.BackColor = System.Drawing.Color.Pink;
            }
            else
            {
                ServiceProvider.GetService<IMessageBox>().ShowWarning(msg);
            }
        }
    }
}
