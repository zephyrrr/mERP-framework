using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionMsgHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionSimpleMsg(Exception ex)
        {
            string caption = ex.Message;
            //string specialMsg = ParseSqlException(ex);
            //if (specialMsg != null)
            //{
            //    caption = specialMsg;
            //}

            return caption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionDetailMsg(Exception ex)
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
                sb.Append(GetExceptionMessage(ex));
                sb.Append(Environment.NewLine);

                sb.Append("Exception Stack Trace:");
                sb.Append(ex.StackTrace);
                sb.Append(Environment.NewLine);

                sb.Append("Exception Stack Trace Frams:");
                sb.Append(ExceptionHelper.FormatException(ex));
                sb.Append(Environment.NewLine);

                sb.Append("Environment Stack Trace:");
                sb.Append(Environment.StackTrace);
                sb.Append(Environment.NewLine);

                ex = ex.InnerException;
                idx++;
            }while(ex != null);

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

        private static string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.Message + Environment.NewLine + GetExceptionMessage(ex.InnerException);
            }
            else
            {
                return ex.Message;
            }
        }
    }
}
