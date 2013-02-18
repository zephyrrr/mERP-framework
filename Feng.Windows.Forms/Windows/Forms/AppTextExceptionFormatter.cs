////===============================================================================
//// Microsoft patterns & practices Enterprise Library
//// Exception Handling Application Block QuickStart
////===============================================================================
//// Copyright ?Microsoft Corporation.  All rights reserved.
//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
//// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
//// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//// FITNESS FOR A PARTICULAR PURPOSE.
////===============================================================================

//using System;
//using System.IO;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

//namespace Feng.Windows.Forms
//{
//    /// <summary>
//    /// Summary description for AppTextExceptionFormatter.
//    /// </summary>	
//    public class AppTextExceptionFormatter : TextExceptionFormatter
//    {
//        /// <summary>
//        /// AppTextExceptionFormatter
//        /// </summary>
//        /// <param name="writer"></param>
//        /// <param name="exception"></param>
//        public AppTextExceptionFormatter(TextWriter writer, Exception exception)
//            : base(writer, exception)
//        {
//        }

//        /// <summary>
//        /// WriteStackTrace
//        /// </summary>
//        /// <param name="stackTrace"></param>
//        protected override void WriteStackTrace(string stackTrace)
//        {

//        }

//        /// <summary>
//        /// WriteExceptionType
//        /// </summary>
//        /// <param name="exceptionType"></param>
//        protected override void WriteExceptionType(Type exceptionType)
//        {
//            base.Indent();
//            base.Writer.WriteLine("Type : {0}", exceptionType.FullName);
//        }
//    }
//}
