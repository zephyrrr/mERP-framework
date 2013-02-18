using System;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageForm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="defaultNo"></param>
        /// <returns></returns>
        public static bool ShowYesNo(string msg, string caption = "确定", bool defaultNo = false)
        {
            var m = ServiceProvider.GetService<IMessageBox>();
            if (m != null)
            {
                return m.ShowYesNo(msg, caption);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static void ShowError(string msg, string caption = "错误")
        {
            var m = ServiceProvider.GetService<IMessageBox>();
            if (m != null)
            {
                m.ShowError(msg, caption);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public static void ShowWarning(string msg, string caption = "警告")
        {
            var m = ServiceProvider.GetService<IMessageBox>();
            if (m != null)
            {
                m.ShowWarning(msg, caption);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public static void ShowInfo(string msg, string caption = "信息")
        {
            var m = ServiceProvider.GetService<IMessageBox>();
            if (m != null)
            {
                m.ShowInfo(msg, caption);
            }
        }
    }
}
