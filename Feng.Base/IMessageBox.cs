using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageBox
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="defaultNo"></param>
        /// <returns></returns>
        bool ShowYesNo(string msg, string caption, bool defaultNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        void ShowError(string msg, string caption);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        void ShowWarning(string msg, string caption);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        void ShowInfo(string msg, string caption);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MessageBoxExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static bool ShowYesNoDefaultNo(this IMessageBox box, string msg, string caption)
        {
            return box.ShowYesNo(msg, caption, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static bool ShowYesNo(this IMessageBox box, string msg, string caption)
        {
            return box.ShowYesNo(msg, caption, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ShowYesNo(this IMessageBox box, string msg)
        {
            return box.ShowYesNo(msg, "确定", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="msg"></param>
        public static void ShowError(this IMessageBox box, string msg)
        {
            box.ShowError(msg, "错误");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="msg"></param>
        public static void ShowWarning(this IMessageBox box, string msg)
        {
            box.ShowWarning(msg, "警告");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="msg"></param>
        public static void ShowInfo(this IMessageBox box, string msg)
        {
            box.ShowInfo(msg, "信息");
        }
        
    }

    
}
