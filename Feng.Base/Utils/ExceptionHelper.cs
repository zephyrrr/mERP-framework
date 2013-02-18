using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Feng.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception ex)
        {
            if (ex.Data.Contains("Caption"))
                return (string)ex.Data["Caption"];
            else
                return ex.Message;
            //string specialMsg = ParseSqlException(ex);
            //if (specialMsg != null)
            //{
            //    caption = specialMsg;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetExceptionStackTrace(Exception ex)
        {
            try
            {
                //Writes error information to the log file including name of the file, line number & error message description
                StackTrace stackTrace = new StackTrace(ex, true);

                StringBuilder sb = new StringBuilder();
                for (int i = stackTrace.FrameCount - 1; i >= 0; --i)
                {
                    StackFrame sf = stackTrace.GetFrame(i);
                    string fileNames = sf.GetFileName();
                    int lineNumber = sf.GetFileLineNumber();

                    //These two lines are respnsible to find out name of the method
                    System.Reflection.MethodBase methodBase = sf.GetMethod();
                    string methodName = methodBase.Name;

                    sb.Append(string.Format("Error Message:{3} at File:{0}, Method name:{1}, line number:{2}", fileNames, methodName, lineNumber.ToString(), ex.Message));
                    sb.Append(System.Environment.NewLine);
                }
                return sb.ToString();
            }
            catch (Exception genEx)
            {
                return string.Format("Exception Format has a Exception {0}, Original Exception is {2}", genEx.Message, ex.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionDetail(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            int idx = 0;
            do
            {
                if (idx > 0)
                {
                    sb.Append(string.Format("Inner Exception Lvel {0}:", idx));
                    sb.Append(Environment.NewLine);
                }
                sb.Append("Exception Message: ");
                sb.Append(ex.Message);
                sb.Append(Environment.NewLine);

                sb.Append("Exception Stack Trace Frams:");
                sb.Append(GetExceptionStackTrace(ex));
                sb.Append(Environment.NewLine);

                sb.Append("Exception Stack Trace:");
                sb.Append(ex.StackTrace);
                sb.Append(Environment.NewLine);

                ex = ex.InnerException;
                idx++;
            } while (ex != null);
            
            sb.Append("Environment Stack Trace:");
            sb.Append(Environment.StackTrace);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }


        //private static string ParseSqlException(Exception ex)
        //{
        //    System.Data.SqlClient.SqlException sqlEx = ex as System.Data.SqlClient.SqlException;
        //    if (sqlEx == null)
        //        return null;

        //    // Todo: More sql exception numbers
        //    if (sqlEx.Number == 2627)
        //    {
        //        return "数据库中已存在重复数据，请查证后再保存！";
        //    }
        //    if (sqlEx.Number == 547)
        //    {
        //        return "违反数据库约束，请查证后再保存！";
        //    }
        //    return null;
        //}

        private static string GetExceptionMessageAllLevel(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.Message + Environment.NewLine + GetExceptionMessageAllLevel(ex.InnerException);
            }
            else
            {
                return ex.Message;
            }
        }
    }
}
