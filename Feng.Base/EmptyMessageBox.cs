using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class EmptyMessageBox : IMessageBox
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="defaultNo"></param>
        /// <returns></returns>
        public bool ShowYesNo(string msg, string caption, bool defaultNo)
        {
            return !defaultNo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public void ShowError(string msg, string caption)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public void ShowWarning(string msg, string caption)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public void ShowInfo(string msg, string caption)
        {
        }
    }
}
