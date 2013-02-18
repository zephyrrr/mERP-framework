using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 采用Python和Evaluation Engine计算（为了和以前的配置兼容）
    /// </summary>
    public class PythonandEvalutionEngineScript : IScript
    {
        private PythonScript m_python = new PythonScript();
        protected EvaluationEngineScript m_evaluationEngine = new EvaluationEngineScript();

        /// <summary>
        /// 执行statement
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object ExecuteStatement(string statement, Dictionary<string, object> processParams)
        {
            try
            {
                return m_python.ExecuteStatement(statement, processParams);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("statement is not supported in PythonandEvalutionEngineScript!", ex);
            }
        }

        protected static bool? IsEvaluationEngineExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return true;

            if (expression.IndexOf("==") != -1)
                return false;

            if (expression.IndexOf("AND") != -1
                || expression.IndexOf("OR") != -1
                || expression.IndexOf("NOT") != -1
                || expression.IndexOf("iif") != -1
                || expression.IndexOf("<>") != -1)
            {
                return true;
            }
            return null;
        }

        /// <summary>
        /// 计算简单表达式，无参数.
        /// 注意，iif不能嵌套。应该用中间变量，例如 $a := iif[%票.卸箱地编号% = "900125", "900005", %票.卸箱地编号%];iif[%票.卸箱地编号% = 900125", "900005", a]$
        /// 注意，最后不能加分号。
        /// 要计算的，前后加$， 要实体类中变量的，加%%
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object CalculateExpression(string expression, Dictionary<string, object> processParams)
        {
            expression = PythonScript.TryRemoveExpressionQuotos(expression);

            bool? isEvaluationEngineExp = IsEvaluationEngineExpression(expression);
            if (isEvaluationEngineExp.HasValue && isEvaluationEngineExp.Value)
            {
                ExceptionProcess.ProcessWithResume(new NotSupportedException(string.Format("{0} 请使用Python表达式.", expression)));
                return m_evaluationEngine.CalculateExpression(expression, processParams);
            }
            else
            {
                // 尝试先以Python方式运行
                try
                {
                    if (expression.Contains("result"))
                    {
                        return m_python.ExecuteStatement(expression, processParams);
                    }
                    else
                    {
                        return m_python.CalculateExpression(expression, processParams);
                    }
                }
                catch (Exception ex)
                {
                    if (!isEvaluationEngineExp.HasValue && ex is Microsoft.Scripting.SyntaxErrorException || ex.InnerException is Microsoft.Scripting.SyntaxErrorException)
                    {
                        object result = null;
                        try
                        {
                            result = m_evaluationEngine.CalculateExpression(expression, processParams);
                            ExceptionProcess.ProcessWithResume(new NotSupportedException(string.Format("{0} 请使用Python表达式.", expression)));
                        }
                        catch (Exception)
                        {
                            throw ex;
                        }

                        return result;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        
    }
}
