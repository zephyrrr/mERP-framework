using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionProcess
    {
        private static bool DoDefaultExceptionProcess(Exception ex)
        {
            IMessageBox box = ServiceProvider.GetService<IMessageBox>();
            if (box != null)
            {
                box.ShowError(ex.Message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static bool ProcessUnhandledException(Exception ex)
        {
            IExceptionProcess ep = ServiceProvider.GetService<IExceptionProcess>();
            if (ep != null)
            {
                return ep.ProcessUnhandledException(ex);
            }
            else
            {
                return DoDefaultExceptionProcess(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool ProcessWithResume(Exception ex)
        {
            IExceptionProcess ep = ServiceProvider.GetService<IExceptionProcess>();
            if (ep != null)
            {
                if (SystemConfiguration.IsInDebug)
                {
                    ex.Data["Caption"] = "Debug版本异常，Release版本不显示。";
                    return ep.ProcessWithNotify(ex);
                }
                else
                {
                    return ep.ProcessWithResume(ex);
                }
            }
            else
            {
                DoDefaultExceptionProcess(ex);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool ProcessWithNotify(Exception ex)
        {
            IExceptionProcess ep = ServiceProvider.GetService<IExceptionProcess>();
            if (ep != null)
            {
                return ep.ProcessWithNotify(ex);
            }
            else
            {
                DoDefaultExceptionProcess(ex);
                return true;
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //public static bool ProcessWithWrap(Exception ex)
        //{
        //    IExceptionProcess ep = ServiceProvider.GetService<IExceptionProcess>();
        //    if (ep != null)
        //    {
        //        return ep.ProcessWithWrap(ex);
        //    }
        //    else
        //    {
        //        DoDefaultExceptionProcess(ex);
        //        return true;
        //    }
        //}
    }
}
