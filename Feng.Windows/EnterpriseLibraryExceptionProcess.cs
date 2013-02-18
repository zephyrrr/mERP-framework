using System;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Feng.Windows
{
	/// <summary>
	/// 异常处理
	/// </summary>
    public class EnterpriseLibraryExceptionProcess : IExceptionProcess
	{
        /// <summary>
        /// 用TextExceptionFormatter格式化Exception错误提示
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter formatter
                = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter(writer, ex);

            // Format the exception
            formatter.Format();

            return sb.ToString();
        }

        ///// <summary>
        ///// 得到Sql命令所包含的信息，包括命令、参数
        ///// </summary>
        ///// <param name="cmd">Sql命令</param>
        ///// <returns>得到的命令信息</returns>
        //private static string GetErrorMsg(DbCommand cmd)
        //{
        //    if (cmd == null)
        //        return "";

        //    System.Text.StringBuilder sb = new System.Text.StringBuilder("Sql语句：" + cmd.CommandText + Environment.NewLine);
        //    foreach (IDataParameter param in cmd.Parameters)
        //    {
        //        sb.Append(param.ParameterName + " = ");
        //        if (param.Value != null)
        //            sb.Append(param.Value.ToString());
        //        sb.Append(Environment.NewLine);
        //    }
        //    return sb.ToString();
        //}

		/// <summary>
        /// 处理未处理异常，对应于"Global Policy"
		/// Process any unhandled exceptions that occur in the application.
		/// This code is called by all UI entry points in the application (e.g. button click events)
		/// when an unhandled exception occurs.
		/// You could also achieve this by handling the Application.ThreadException event, however
		/// the VS2005 debugger will break before this event is called.
		/// </summary>
		/// <param name="ex">The unhandled exception</param>
		public virtual bool ProcessUnhandledException(Exception ex)
        {
            bool rethrow = ExceptionPolicy.HandleException(ex, "Global Policy");
            if (rethrow)
            {
                return false;
            }
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
			bool rethrow = ExceptionPolicy.HandleException(ex, "Handle and Resume Policy");
            System.Diagnostics.Debug.Assert(rethrow == false, "Resume Policy should not rethrow exception");

            return rethrow;
		}

        /// <summary>
        /// 提示用户
        /// </summary>
        /// <param name="ex"></param>
        public bool ProcessWithNotify(Exception ex)
        {
            if (ex is InvalidUserOperationException)
            {
                return ProcessWithNotifyInfo(ex);
            }
            else
            {
                return ProcessWithNotifyError(ex);
            }
        }

        /// <summary>
        /// 简单的提示用户，不是错误，只是程序中自定义的Exception
        /// </summary>
        private bool ProcessWithNotifyInfo(Exception ex)
        {
            bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy Info");
            System.Diagnostics.Debug.Assert(rethrow == false, "Notify Policy should not rethrow exception");

            return rethrow;
        }

		/// <summary>
        /// 异常出现后，提示用户，对应于"Notify Policy"
		/// Demonstrates handling of exceptions coming out of a layer. The policy
		/// demonstrated here will show how original exceptions can be notified
		/// </summary>
        private bool ProcessWithNotifyError(Exception ex)
		{
			bool rethrow = ExceptionPolicy.HandleException(ex, "Notify Policy Error");
            System.Diagnostics.Debug.Assert(rethrow == false, "Notify Policy should not rethrow exception");

            return rethrow;
		}

		///// <summary>
		///// Demonstrates handling of exceptions coming out of a layer. The policy
		///// demonstrated here will show how original exceptions can be propagated back out.
		///// </summary>
		//public static void ProcessWithPropagate(Exception ex)
		//{
		//    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");

		//    if (rethrow)
		//    {
		//        throw;
		//    }
		//}

        ///// <summary>
        ///// 异常出现后，包装异常并抛出，对应于"Wrap Policy"
        ///// Demonstrates handling of exceptions coming out of a layer. The policy
        ///// demonstrated here will show how original exceptions can be wrapped
        ///// with a different exception before being propagated back out.
        ///// </summary>
        //public bool ProcessWithWrap(Exception ex)
        //{
        //    bool rethrow = ExceptionPolicy.HandleException(ex, "Wrap Policy");
        //    System.Diagnostics.Debug.Assert(rethrow == true, "Notify Policy should rethrow exception");
        //    return rethrow;
        //}

		///// <summary>
		///// Demonstrates handling of exceptions coming out of a layer. The policy
		///// demonstrated here will show how original exceptions can be replaced
		///// with a different exception before being propagated back out.
		///// </summary>
		//public static void ProcessWithReplace(Exception ex)
		//{
		//    bool rethrow = ExceptionPolicy.HandleException(ex, "Replace Policy");

		//    if (rethrow)
		//    {
		//        throw ex;
		//    }
		//}
	}
}
