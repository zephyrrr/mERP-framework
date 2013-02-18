using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Display-Value对应的Viewer, Editor
    /// </summary>
    public interface INameValueControl
    {
        /// <summary>
        /// 获取显示文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetDisplay(object value);

        /// <summary>
        /// 获取内部值
        /// </summary>
        /// <param name="displayText"></param>
        /// <returns></returns>
        object GetValue(string displayText);
    }
}
