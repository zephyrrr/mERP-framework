using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggerExceptionProcess : IExceptionProcess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessUnhandledException(Exception ex)
        {
            Logger.Fatal(Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }

        /// <summary>
        /// 异常出现后，程序继续运行，对应于"Handle and Resume Policy"
        /// Demonstrates handling of exceptions coming out of a layer. The policy
        /// demonstrated here will show how original exceptions can be supressed
        /// and the program is resumed
        /// </summary>
        public bool ProcessWithResume(Exception ex)
        {
            Logger.Debug(Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }

        /// <summary>
        /// 提示用户
        /// </summary>
        /// <param name="ex"></param>
        public bool ProcessWithNotify(Exception ex)
        {
            Logger.Info(Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }


        ///// <summary>
        ///// 异常出现后，包装异常并抛出，对应于"Wrap Policy"
        ///// Demonstrates handling of exceptions coming out of a layer. The policy
        ///// demonstrated here will show how original exceptions can be wrapped
        ///// with a different exception before being propagated back out.
        ///// </summary>
        //public bool ProcessWithWrap(Exception ex)
        //{
        //    Logger.Info(Feng.Utils.ExceptionHelper.FormatException(ex));
        //    return true;
        //}
    }
}
