using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFileScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        object ExecuteFile(string fileName, Dictionary<string, object> processParams);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// 执行statement
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        object ExecuteStatement(string statement, Dictionary<string, object> processParams);

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        object CalculateExpression(string expression, Dictionary<string, object> processParams);
    }
}
