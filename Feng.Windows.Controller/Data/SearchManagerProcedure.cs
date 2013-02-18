using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data.Common;
using System.Diagnostics;
using Feng.Collections;
using Feng.Search;

namespace Feng.Data
{
    /// <summary>
    /// 用于SQL Procedure的查找管理器
    /// <example>procedureName = "过程查询_固定资产折旧"(不需要带@参数）</example>
    /// </summary>
    public class SearchManagerProcedure : AbstractSearchManager
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="procedureName"></param>
        public SearchManagerProcedure(string procedureName)
            : base()
        {
            m_procedureName = procedureName;
            this.EnablePage = false;
        }


        private string m_procedureName;

        private void FillDbCommand(ISearchExpression condition, DbCommand cmd, ref int paramCnt)
        {
            SimpleExpression cse = condition as SimpleExpression;
            if (cse == null)
            {
                LogicalExpression l = condition as LogicalExpression;

                FillDbCommand(l.LeftHandSide, cmd, ref paramCnt);
                FillDbCommand(l.RightHandSide, cmd, ref paramCnt);
            }
            else
            {
                string simpleParamName = "@" + cse.FullPropertyName;
                string complexParamName = "@" + cse.FullPropertyName + cse.Operator.ToString();
                switch (cse.Operator)
                {
                    case SimpleOperator.Any:
                    case SimpleOperator.EqProperty:
                    case SimpleOperator.IsNotNull:
                    case SimpleOperator.IsNull:
                    case SimpleOperator.NotEq:
                    case SimpleOperator.NotEqProperty:
                    case SimpleOperator.Sql:
                        throw new NotSupportedException(cse.Operator + " is not supported in procedure!");
                    case SimpleOperator.Ge:
                    case SimpleOperator.Gt:
                    case SimpleOperator.Le:
                    case SimpleOperator.Lt:
                        cmd.Parameters.Add(DbHelper.Instance.CreateParameter(complexParamName, cse.Values));
                        break;
                    case SimpleOperator.Eq:
                    case SimpleOperator.GInG:
                    case SimpleOperator.InG:
                    case SimpleOperator.Like:
                        cmd.Parameters.Add(DbHelper.Instance.CreateParameter(simpleParamName, cse.Values));
                        break;
                }

                paramCnt++;
            }
        }

        /// <summary>
        /// 根据查询条件创建Sql命令
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public DbCommand CreateDbCommand(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            DbCommand cmd = DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            int paramCnt = 0;

            cmd.CommandText = m_procedureName;

            FillDbCommand(searchExpression, cmd, ref paramCnt);

            return cmd;
        }


        /// <summary>
        /// 读入数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(CreateDbCommand(searchExpression, searchOrders));

            base.Result = dt.DefaultView;
            base.Count = dt.Rows.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public override int GetCount(ISearchExpression searchExpression)
        {
            System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(CreateDbCommand(searchExpression, null));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            return DbHelper.Instance.ExecuteDataTable(CreateDbCommand(searchExpression, searchOrders)).DefaultView;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProcedure sm = new SearchManagerProcedure(this.m_procedureName);
            Copy(this, sm);
            return sm;
        }
    }
}