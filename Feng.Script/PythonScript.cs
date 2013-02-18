using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Feng
{
    /// <summary>
    /// 采用Python计算表达式
    /// </summary>
    public class PythonScript : IScript, IFileScript
    {
        public object ExecuteFile(string fileName, Dictionary<string, object> processParams = null)
        {
            return Feng.Scripts.PythonHelper.ExecutePythonFile(fileName, processParams);
        }

        /// <summary>
        /// 如果表达式两边有$，则移除
        /// </summary>
        /// <param name="expression"></param>
        public static string TryRemoveExpressionQuotos(string expression)
        {
            if (string.IsNullOrEmpty(expression) || expression.Length <= 2)
            {
                return expression;
            }
            if (expression[0] == '$' && expression[expression.Length - 1] == '$')
            {
                return expression.Substring(1, expression.Length - 2);
            }
            else
            {
                return expression;
            }
        }

        /// <summary>
        /// 执行Python statement, 返回结果必须是result变量
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object ExecuteStatement(string statement, Dictionary<string, object> processParams)
        {
            return Feng.Scripts.PythonHelper.ExecutePythonStatement(statement, processParams);
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object CalculateExpression(string expression, Dictionary<string, object> processParams)
        {
            expression = TryRemoveExpressionQuotos(expression);

            return Feng.Scripts.PythonHelper.ExecutePythonExpression(expression, processParams);
        }
    }
}
