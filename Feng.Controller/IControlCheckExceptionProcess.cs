using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 处理<see cref="ControlCheckException"/>的处理程序
    /// </summary>
    public interface IControlCheckExceptionProcess : IDisposable
    {
        /// <summary>
        /// 清除错误
        /// </summary>
        void ClearAllError();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invalidControl"></param>
        /// <param name="msg"></param>
        void ShowError(object invalidControl, string msg);
    }
}
