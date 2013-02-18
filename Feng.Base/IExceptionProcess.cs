using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 如何处理异常的接口
    /// </summary>
    public interface IExceptionProcess
    {
        /// <summary>
        /// 处理未处理异常
        /// </summary>
        /// <param name="ex">The unhandled exception</param>
        bool ProcessUnhandledException(Exception ex);

        /// <summary>
        /// 异常出现后，程序继续运行
        /// </summary>
        bool ProcessWithResume(Exception ex);

        /// <summary>
        /// 提示用户
        /// </summary>
        /// <param name="ex"></param>
        bool ProcessWithNotify(Exception ex);


        ///// <summary>
        ///// 异常出现后，包装异常并抛出，对应于"Wrap Policy"
        ///// Demonstrates handling of exceptions coming out of a layer. The policy
        ///// demonstrated here will show how original exceptions can be wrapped
        ///// with a different exception before being propagated back out.
        ///// </summary>
        //bool ProcessWithWrap(Exception ex);
    }
}
