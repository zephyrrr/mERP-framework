using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleExceptionProcess : IExceptionProcess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessUnhandledException(Exception ex)
        {
            System.Console.WriteLine(Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessWithResume(Exception ex)
        {
            Logger.Info(Feng.Utils.ExceptionHelper.GetExceptionDetail(ex), ex);
            System.Console.WriteLine(Feng.Utils.ExceptionHelper.GetExceptionMessage(ex));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessWithNotify(Exception ex)
        {
            Logger.Info(Feng.Utils.ExceptionHelper.GetExceptionDetail(ex), ex);
            System.Console.WriteLine(Feng.Utils.ExceptionHelper.GetExceptionMessage(ex));
            return true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StringExceptionProcess : IExceptionProcess
    {
        private List<string> m_listExceptions = new List<string>();

        /// <summary>
        /// AddLog
        /// </summary>
        /// <param name="s"></param>
        public void AddLog(string s)
        {
            m_listExceptions.Add(s);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Exceptions
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < m_listExceptions.Count; ++i)
                {
                    sb.AppendLine(i.ToString() + ":");
                    sb.AppendLine(m_listExceptions[i].ToString());
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_listExceptions.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessUnhandledException(Exception ex)
        {
            m_listExceptions.Add("Unhandled: " + Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessWithResume(Exception ex)
        {
            m_listExceptions.Add("Resume: " + Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ProcessWithNotify(Exception ex)
        {
            m_listExceptions.Add("Notify: " + Feng.Utils.ExceptionHelper.GetExceptionDetail(ex));
            return true;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //public bool ProcessWithWrap(Exception ex)
        //{
        //    m_sbWrap.Add(Feng.Utils.ExceptionHelper.FormatException(ex));
        //    return true;
        //}
    }
}
