using System;
using System.Collections.Generic;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class Script
    {
        private const string c_pythonHeader = "# # -*- coding: utf-8 -*-  \r\n" + "import clr; \r\n" + "import System; \r\n";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static object ExecuteStatement(string statement)
        {
            return ExecuteStatement(statement, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object ExecuteStatement(string statement, Dictionary<string, object> processParams)
        {
            if (string.IsNullOrEmpty(statement))
            {
                return null;
            }

            object ret = ServiceProvider.GetService<IScript>().ExecuteStatement(c_pythonHeader + System.Environment.NewLine + statement, processParams);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object CalculateExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }
            string key = "EntityHelper.CalculateExpression:" + expression;
            return Cache.TryGetCache<object>(key, new Func<object>(delegate()
            {
                return ServiceProvider.GetService<IScript>().CalculateExpression(expression, null);
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object CalculateExpression(string expression, Dictionary<string, object> processParams)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }
            return ServiceProvider.GetService<IScript>().CalculateExpression(expression, processParams);
        }
    }
}
