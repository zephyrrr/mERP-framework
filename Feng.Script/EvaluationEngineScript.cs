using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 采用原有的 Evaluation Engine at http://www.codeproject.com/KB/recipes/EvaluationEngine/
    /// </summary>
    public class EvaluationEngineScript : IScript
    {
        /// <summary>
        /// 执行statement
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object ExecuteStatement(string statement, Dictionary<string, object> processParams)
        {
            throw new NotSupportedException("statement is not supported!");
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object CalculateExpression(string expression, Dictionary<string, object> processParams)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }
            if (processParams != null)
            {
                throw new ArgumentException("processParams is not supported in EvaluationEngineScript!");
            }

            EvaluationEngine.Parser.Token token = new EvaluationEngine.Parser.Token(expression);
            EvaluationEngine.Evaluate.Evaluator eval = new EvaluationEngine.Evaluate.Evaluator(token);
            string ErrorMsg = string.Empty;
            string result = string.Empty;
            try
            {
                if (eval.Evaluate(out result, out ErrorMsg) == false)
                {
                    throw new InvalidOperationException("Error evaluating the tokens: " + ErrorMsg + System.Environment.NewLine
                        + expression);
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (!string.IsNullOrEmpty(result) && result.Length >= 2)
            {
                if (result[0] == '\"' && result[result.Length - 1] == '\"')
                {
                    result = result.Substring(1, result.Length - 2);
                }
            }
            return result;
        }
    }
}
