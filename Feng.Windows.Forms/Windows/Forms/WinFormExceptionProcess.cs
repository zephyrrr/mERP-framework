using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class WinFormExceptionProcess : IExceptionProcess
    {
        public bool ProcessUnhandledException(Exception ex)
        {
            Logger.Fatal("程序发生严重错误，将关闭程序！请联系技术人员。", ex);

            //string errorMsg = "程序发生严重错误，将关闭程序！请联系技术人员。";
            //errorMsg += Environment.NewLine;
            //errorMsg += ex.Message;

            //System.Windows.Forms.MessageBox.Show(errorMsg, "应用程序错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);

            //System.Windows.Forms.Application.Exit();
            ShowExceptionDialog(ex, true);

            return false;
        }
        
        //private log4net.ILog m_log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Creates the error message and displays it.
        private void ShowExceptionDialog(Exception ex, bool isError)
        {
            string caption = Feng.Utils.ExceptionHelper.GetExceptionMessage(ex);
            if (SystemConfiguration.Roles != null && Array.IndexOf<string>(SystemConfiguration.Roles, SystemConfiguration.DeveloperRoleName) != -1)
            {
                string errorMsg = Feng.Utils.ExceptionHelper.GetExceptionDetail(ex);

                //return MessageForm.ShowError(errorMsg);
                using (Feng.Windows.Forms.ErrorReport form = new Feng.Windows.Forms.ErrorReport(caption, isError ? "错误" : "警告",
                    isError ? Feng.Windows.Forms.MessageImage.Error : Feng.Windows.Forms.MessageImage.Exclaim, errorMsg))
                {
                    form.ShowDialog();
                }
            }
            else
            {
                if (isError)
                    MessageForm.ShowError(caption);
                else
                    MessageForm.ShowWarning(caption);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessWithResume(Exception ex)
        {
            Logger.Info(ex.Message, ex);
            return false;
        }

        /// <summary>
        /// 提示用户
        /// </summary>
        /// <param name="ex"></param>
        public bool ProcessWithNotify(Exception ex)
        {
            if (ex is InvalidUserOperationException)
            {
                return ProcessWithNotifyInfo(ex);
            }
            else if (ex.GetType().FullName == "NHibernate.StaleObjectStateException")
            {
                ex.Data["Caption"] = "出现多人操作，请重试！";
                return ProcessWithNotifyInfo(ex);
            }
            else if (ex.GetType().FullName == "System.Data.SqlClient.SqlException")
            {
                ex.Data["Caption"] = "出现数据库错误，请重试！";
                return ProcessWithNotifyError(ex);
            }
            else
            {
                return ProcessWithNotifyError(ex);
            }
        }

        /// <summary>
        /// 简单的提示用户，不是错误，只是程序中自定义的Exception
        /// </summary>
        private bool ProcessWithNotifyInfo(Exception ex)
        {
            Logger.Info(ex.Message, ex);
            ShowExceptionDialog(ex, false);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool ProcessWithNotifyError(Exception ex)
        {
            Logger.Warn(ex.Message, ex);
            ShowExceptionDialog(ex, true);

            return false;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //public bool ProcessWithWrap(Exception ex)
        //{
        //    if (ex.GetType().FullName == "NHibernate.StaleObjectStateException")
        //    {
        //        throw new ApplicationException("出现多人操作，请重试！", ex);
        //    }
        //    else if (ex.GetType().FullName == "System.Data.SqlClient.SqlException")
        //    {
        //        throw new ApplicationException("出现数据库错误，请重试！", ex);
        //    }
        //    return true;
        //}
    }
}
