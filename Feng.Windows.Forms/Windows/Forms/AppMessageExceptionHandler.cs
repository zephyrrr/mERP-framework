//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block QuickStart
//===============================================================================
// Copyright ?Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using Feng.Utils;

namespace Feng.Windows.Forms
{
	/// <summary>
	/// Summary description for GlobalPolicyExceptionHandler.
	/// </summary>
    [Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementType(typeof(Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.CustomHandlerData))]
    public class AppMessageExceptionHandler : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler
	{
		/// <summary>
		/// AppMessageExceptionHandler
		/// </summary>
        /// <param name="ignore"></param>
        public AppMessageExceptionHandler(NameValueCollection ignore)
		{
		}

		/// <summary>
		/// HandleException
		/// </summary>
        /// <param name="exception"></param>
		/// <param name="handlingInstanceId"></param>
		/// <returns></returns>
		public Exception HandleException(Exception exception, Guid handlingInstanceId)
		{
			this.ShowThreadExceptionDialog(exception);

			return exception;
		}

		// Creates the error message and displays it.
		private void ShowThreadExceptionDialog(Exception ex)
		{
            if (SystemConfiguration.IsInDebug)
            {
                string errorMsg = ExceptionHelper.GetExceptionDetail(ex);
                string caption = ExceptionHelper.GetExceptionMessage(ex);

                //return MessageForm.ShowError(errorMsg);
                using (ErrorReport form = new ErrorReport(caption, "´íÎó", MessageImage.Error, errorMsg))
                {
                    form.ShowDialog();
                }
            }
            else
            {
                //return DialogResult.OK;
                string caption = ExceptionHelper.GetExceptionMessage(ex);
                MessageForm.ShowError(caption);
            }
        }
	}
}
