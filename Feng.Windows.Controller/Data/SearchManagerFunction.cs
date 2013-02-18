using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Feng.Search;

namespace Feng.Data
{
    /// <summary>
    /// 用于SQL Function的查找管理器
    /// <example>functionName = "函数查询_固定资产折旧(@TimeGe,@TimeLe)", defaultOrder = "购入时间"。需要带@参数</example>
    /// </summary>
    public class SearchManagerFunction : SearchManager
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="defaultOrder"></param>
        public SearchManagerFunction(string functionName, string defaultOrder)
            : base(functionName, defaultOrder)
        {
            int idx = functionName.IndexOf('(');
            if (idx != -1)
            {
                int idx2 = functionName.IndexOf(')', idx);
                System.Diagnostics.Debug.Assert(idx != -1, "( and ) must match!");

                string strParam = functionName.Substring(idx + 1, idx2 - idx - 1);
                m_funcParams = strParam.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            m_functionName = functionName;
            m_defaultOrder = defaultOrder;
        }
        private string m_functionName;
        private string m_defaultOrder;
        private string[] m_funcParams = null;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerFunction sm = new SearchManagerFunction(this.m_functionName, m_defaultOrder);
            Copy(this, sm);
            return sm;
        }

        //private void ProcessCmd(DbCommand cmd)
        //{
        //    foreach (string s in m_funcParams)
        //    {
        //        if (!cmd.Parameters.Contains(s))
        //        {
        //            cmd.CommandText = cmd.CommandText.Replace(s, "default");
        //        }
        //        else
        //        {
        //            // remove like %
        //            string likeString = cmd.Parameters[s].Value as string;
        //            if (!string.IsNullOrEmpty(likeString))
        //            {
        //                if (likeString[0] == '%' && likeString[likeString.Length - 1] == '%')
        //                {
        //                    cmd.Parameters[s].Value = likeString.Substring(1, likeString.Length - 2);
        //                }
        //            }

        //            // remove where clause
        //            int idx = cmd.CommandText.IndexOf("WHERE");
        //            idx = cmd.CommandText.IndexOf(s, idx);

        //            int idx2 = idx;

        //            // jump to "=, >="
        //            idx2--;
        //            while (cmd.CommandText[idx2] == ' ')
        //                idx2--;

        //            // jump to space
        //            idx2--;
        //            while (cmd.CommandText[idx2] != ' ')
        //                idx2--;

        //            // jump to propertyName
        //            idx2--;
        //            while (cmd.CommandText[idx2] == ' ')
        //                idx2--;

        //            // jump to space
        //            idx2--;
        //            while (cmd.CommandText[idx2] != ' ')
        //                idx2--;

        //            cmd.CommandText = cmd.CommandText.Replace(cmd.CommandText.Substring(idx2 + 1, idx - idx2 - 1 + s.Length), "1 = 1");
        //        }
        //    }
        //}

        private ISearchExpression RemoveFunctionParamSearchExpression(ISearchExpression se, Dictionary<string, object> deletedParam)
        {
            if (se == null)
                return null;
            if (se is LogicalExpression)
            {
                LogicalExpression le = se as LogicalExpression;
                ISearchExpression ls = RemoveFunctionParamSearchExpression(le.LeftHandSide, deletedParam);
                ISearchExpression rs = RemoveFunctionParamSearchExpression(le.RightHandSide, deletedParam);
                switch (le.LogicOperator)
                {
                    case LogicalOperator.And:
                        return SearchExpression.And(ls, rs);
                    case LogicalOperator.Or:
                        return SearchExpression.Or(ls, rs);
                    case LogicalOperator.Not:
                        return SearchExpression.Not(ls);
                    default:
                        throw new NotSupportedException("Not Supported LogicalOperator!");
                }
            }
            else if (se is SimpleExpression)
            {
                SimpleExpression cse = se as SimpleExpression;

                string paramName = SearchManager.CreateParamName(cse, null);
                if (Array.IndexOf(m_funcParams, paramName) != -1)
                {
                    deletedParam[paramName] = cse.Values;
                    return null;
                }
                else
                {
                    return cse;
                }
            }
            else
            {
                return se;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        protected override DbCommand CreateDbCommand(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ISearchExpression newSe = RemoveFunctionParamSearchExpression(searchExpression, dict);

            DbCommand cmd = base.CreateDbCommand(newSe, searchOrders);

            foreach (string funcParam in m_funcParams)
            {
                cmd.Parameters.Add(DbHelper.Instance.CreateParameter(funcParam, dict.ContainsKey(funcParam) ? dict[funcParam] : System.DBNull.Value));
            }

            //ProcessCmd(cmd);

            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        protected override DbCommand CreateCountDbCommand(ISearchExpression searchExpression)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ISearchExpression newSe = RemoveFunctionParamSearchExpression(searchExpression, dict);

            DbCommand cmd = base.CreateCountDbCommand(newSe);

            foreach (string funcParam in m_funcParams)
            {
                cmd.Parameters.Add(DbHelper.Instance.CreateParameter(funcParam, dict.ContainsKey(funcParam) ? dict[funcParam] : System.DBNull.Value));
            }

            //ProcessCmd(cmd);

            return cmd;
        }
    }
}
