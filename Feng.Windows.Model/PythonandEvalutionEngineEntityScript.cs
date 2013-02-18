using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    public class PythonandEvalutionEngineEntityScript : PythonandEvalutionEngineScript, IEntityScript
    {
        private PythonEntityScript m_entityPython = new PythonEntityScript();

        /// <summary>
        /// 计算表达式
        /// 语法：    expression if expression else expression
        /// Example: $%提箱点编号% if str(%任务性质%) == "进口拆箱" else %装货地%$
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object CalculateExpression(string expression, object entity)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }
            expression = PythonScript.TryRemoveExpressionQuotos(expression);

            bool? isEvalutionEngineExpression = IsEvaluationEngineExpression(expression);
            if (isEvalutionEngineExpression.HasValue && isEvalutionEngineExpression.Value)
            {
                string ownExpression = EntityHelper.ReplaceEntity(expression, entity);
                object result = m_evaluationEngine.CalculateExpression(ownExpression, null);
                ExceptionProcess.ProcessWithResume(new NotSupportedException(string.Format("{0} 请使用Python表达式.", expression)));
                return result;
            }

            try
            {
                try
                {
                    return m_entityPython.CalculateExpression(expression, entity);
                }
                catch (Exception)
                {
                    string ownExpression = EntityHelper.ReplaceEntity(expression, entity);
                    ownExpression = ownExpression.Replace("\"\"", "None");
                    return m_entityPython.CalculateExpression(ownExpression, entity);
                }
            }
            catch (Exception ex)
            {
                if (!isEvalutionEngineExpression.HasValue && ex.GetType().ToString().Contains("Microsoft.Scripting.SyntaxErrorException"))
                {
                    string ownExpression = EntityHelper.ReplaceEntity(expression, entity);
                    object result = null;
                    try
                    {
                        result = m_evaluationEngine.CalculateExpression(ownExpression, null);
                        ExceptionProcess.ProcessWithResume(new NotSupportedException(string.Format("{0} 请使用Python表达式.", expression)));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return result;
                }
                else
                {
                    throw;
                }
            }
        }

        public object GetPropertyValue(object entity, string fullPropertyName)
        {
            return m_entityPython.GetPropertyValue(entity, fullPropertyName);
        }

        public void SetPropertyValue(object entity, string fullPropertyName, object setValue)
        {
            m_entityPython.SetPropertyValue(entity, fullPropertyName, setValue);
        }
    }
}
