using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using Feng.Utils;

namespace Feng
{
    /// <summary>
    /// 采用Python计算表达式
    /// </summary>
    public class PythonEntityScript : PythonScript, IEntityScript
    {
        /// <summary>
        /// 执行Python statement, 返回结果必须是result变量
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal object ExecuteStatement(string statement, object entity)
        {
            if (string.IsNullOrEmpty(statement))
                return null;

            object ret = null;
            if (entity is IEntity || (entity == null && statement.Contains("entity")))
            {
                statement = EntityHelper.ReplaceEntityParameters(statement, entity, true);

                ret = base.ExecuteStatement(statement, new Dictionary<string, object> { { "entity", entity } });
            }
#if !SILVERLIGHT
            else if (entity is System.Data.DataRowView || entity is IDictionary<string, object>
                || (entity == null && statement.Contains("row")))
            {
                statement = EntityHelper.ReplaceEntityParameters(statement, entity, false);
                ret = base.ExecuteStatement(statement, new Dictionary<string, object> { { "row", entity } });
            }
#endif
            else if (entity == null)
            {
                //Match m = s_regexEntityParamter.Match(statement);
                //if (m.Success)
                //{
                //    return false;
                //}
                //else
                {
                    return base.ExecuteStatement(statement, null);
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("Now only IEntity or DataRowView is supported, {0} is not supported !", entity.GetType().ToString()));
            }
            return ret;
        }

        public object CalculateExpression(string expression, object entity)
        {
            if (expression.Contains("result"))
            {
                return ExecuteStatement(expression, entity);
            }
            else
            {
                return CalculateExpressionInternal(expression, entity);
            }
        }

        /// <summary>
        /// 计算表达式
        /// 语法：    expression if expression else expression
        /// Example: $%提箱点编号% if str(%任务性质%) == "进口拆箱" else %装货地%$
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private object CalculateExpressionInternal(string expression, object entity)
        {
            if (string.IsNullOrEmpty(expression))
                return null;

            expression = PythonScript.TryRemoveExpressionQuotos(expression);

            object ret = null;
            if (entity is IEntity || (entity == null && expression.Contains("entity")))
            {
                expression = EntityHelper.ReplaceEntityParameters(expression, entity, true);

                ret = base.CalculateExpression(expression, new Dictionary<string, object> { { "entity", entity } });
            }
#if !SILVERLIGHT
            else if (entity is System.Data.DataRowView || entity is IDictionary<string, object>
                || (entity == null && expression.Contains("row")))
            {
                expression = EntityHelper.ReplaceEntityParameters(expression, entity, false);
                ret = base.CalculateExpression(expression, new Dictionary<string, object> { { "row", entity } });
            }
#endif
            else if (entity == null)
            {
                if (expression.Contains("%"))
                {
                    expression = EntityHelper.ReplaceEntityParameters(expression, entity, true, true);
                    ret = base.CalculateExpression(expression, new Dictionary<string, object> { });
                }
                else
                {
                    return base.CalculateExpression(expression, null);
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("Now only IEntity or DataRowView is supported, {0} is not supported !", entity.GetType().ToString()));
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fullPropertyName"></param>
        /// <returns></returns>
        public object GetPropertyValue(object entity, string fullPropertyName)
        {
            if (entity == null)
                return null;

            if (entity is IEntity)
            {
                Type type = entity.GetType();

                object nextValue = null;
                object nowValue = entity;

                string[] ss = fullPropertyName.Split(new char[] { '.' });

                for (int i = 0; i < ss.Length; ++i)
                {
                    // NHibernate proxy type
                    //while (!type.FullName.StartsWith(m_defaultAssemblyNamespace))
                    //    type = type.BaseType;

                    try
                    {
                        nextValue = type.InvokeMember(ss[i], BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public, null, nowValue, null);
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        // 有时候确实没这个属性，但为了不需要更多的配置，默认读取。例如大写金额
                        // ExceptionProcess.ProcessWithNotify(ex);
                        throw;
                        // return null;
                    }

                    nowValue = nextValue;

                    if (nextValue == null)
                    {
                        nowValue = null;
                        break;
                    }
                    if (i < ss.Length - 1)
                    {
                        type = nextValue.GetType();
                    }
                }
                return nowValue;
            }
#if !SILVERLIGHT
            else if (entity is System.Data.DataRowView)
            {
                try
                {
                    object ret = ((System.Data.DataRowView)entity)[fullPropertyName];
                    if (ret == System.DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return ret;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    // 有时候确实没这个属性，但为了不需要更多的配置，默认读取。例如大写金额
                    //ExceptionProcess.ProcessWithNotify(ex);
                    return null;
                }
            }
#endif
            else if (entity is IDictionary<string, object>)
            {
                IDictionary<string, object> d = entity as IDictionary<string, object>;
                if (d.ContainsKey(fullPropertyName))
                {
                    return d[fullPropertyName];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("Now only IEntity or DataRowView is supported, {0} is not supported !", entity.GetType().ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fullPropertyName"></param>
        /// <param name="setValue"></param>
        public void SetPropertyValue(object entity, string fullPropertyName, object setValue)
        {
            if (entity is IEntity)
            {
                Type type = entity.GetType();

                object nextValue = null;
                object nowValue = entity;

                string[] ss = fullPropertyName.Split(new char[] { '.' });

                for (int i = 0; i < ss.Length - 1; ++i)
                {
                    // NHibernate Proxy
                    //while (!type.FullName.StartsWith(DataUnboundGridT.DefaultAssemblyNamespace))
                    //    type = type.BaseType;

                    nextValue = type.InvokeMember(ss[i], BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public,
                        null, nowValue, null);
                    nowValue = nextValue;

                    if (nextValue == null)
                    {
                        nowValue = null;
                        break;
                    }
                    if (i < ss.Length - 1)
                    {
                        type = nextValue.GetType();
                    }
                }
                //dm.Item = nowValue;

                if (nowValue == null)
                {
                    // 有时候为确实没数据，不用设置也不用提示错误
                    if (setValue != null)
                    {
                        throw new InvalidOperationException("Now value is null, so can't save " + fullPropertyName);
                    }
                    return;
                }
                type.InvokeMember(ss[ss.Length - 1], System.Reflection.BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public,
                    null, nowValue, new object[] { setValue }, null, null, null);
            }
#if !SILVERLIGHT
            else if (entity is System.Data.DataRowView)
            {
                if (setValue == null)
                {
                    ((System.Data.DataRowView)entity)[fullPropertyName] = System.DBNull.Value;
                }
                else
                {
                    ((System.Data.DataRowView)entity)[fullPropertyName] = setValue;
                }
            }
#endif
            else if (entity is IDictionary<string, object>)
            {
                ((IDictionary<string, object>)entity)[fullPropertyName] = setValue;
            }
            else
            {
                throw new NotSupportedException(string.Format("Now only IEntity or DataRowView is supported, {0} is not supported !", entity.GetType().ToString()));
            }
        }
    }
}
