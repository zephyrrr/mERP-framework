using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Feng.Search;

namespace Feng.Data
{
    /// <summary>
    /// 直接采用Sql语句的查询管理器
    /// </summary>
    public class SearchManagerQuery : AbstractSearchManager
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManagerQuery(string queryString)
            : base()
        {
            this.EnablePage = true;

            ParseQueryString(queryString);
        }

        private bool m_dataCanPage = false;

        /// <summary>
        /// 根据查询条件读入数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Data.DataTable dt = GetData(searchExpression, searchOrders) as System.Data.DataTable;

            if (this.EnablePage && m_dataCanPage)
            {
                base.Count = GetCount(searchExpression);
            }
            else
            {
                base.Count = dt.Rows.Count;
            }

            // 需要分页但数据库读取不能分页的时候，读出全部数据然后选择性返回
            if (this.EnablePage && !m_dataCanPage)
            {
                System.Data.DataTable dt2 = dt.Clone();
                for (int i = 0; i < Math.Min(dt.Rows.Count, this.MaxResult); ++i)
                {
                    dt2.ImportRow(dt.Rows[i + this.FirstResult]);
                }
                base.Result = dt2.DefaultView;
            }
            else
            {
                base.Result = dt.DefaultView;
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Data.DataView dv = DbHelper.Instance.ExecuteDataTable(CreateDbCommand(searchExpression, searchOrders)).DefaultView;

            return dv;
        }

        private void ParseQueryString(string queryString)
        {
            m_queryString = queryString.ToUpper();

            int idx = m_queryString.IndexOf("GROUP BY");
            if (idx != -1)
            {
                m_selectSql = m_queryString.Substring(0, idx);
                m_groupBySql = m_queryString.Substring(idx);

                int idx2 = m_selectSql.IndexOf("FROM");
                string selectSql = m_selectSql.Substring(0, idx2);
                m_selectAsColumns = SearchManager.ParseSelectCommand(selectSql);
            }
            else
            {
                m_selectSql = m_queryString;
                m_groupBySql = null;
                m_selectAsColumns = new Dictionary<string, string>();
            }
        }

        private string m_queryString;
        private string m_selectSql;
        private string m_groupBySql;
        private Dictionary<string, string> m_selectAsColumns;

        /// <summary>
        /// 根据查询条件创建Sql命令
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public virtual DbCommand CreateDbCommand(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            DbCommand cmd = DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            if (searchExpression is QueryExpression)
            {
                string s = (searchExpression as QueryExpression).Query;
                cmd.CommandText = s;
            }
            else
            {
                int paramCnt = 0;

                if (this.EnablePage && m_dataCanPage)
                {
                }
                else
                {
                    cmd.CommandText = m_selectSql;

                    // Add Where 
                    if (searchExpression != null)
                    {
                        string s = SearchManager.FillDbCommand(searchExpression, cmd.Parameters, false, ref paramCnt, m_selectAsColumns);
                        if (!string.IsNullOrEmpty(s))
                        {
                            cmd.CommandText += " WHERE " + s;
                        }
                    }

                    string groupBy = SearchManager.GetGroupCommand(m_groupBySql);
                    if (!string.IsNullOrEmpty(groupBy))
                    {
                        cmd.CommandText += groupBy;

                        // Add Having
                        if (searchExpression != null)
                        {
                            string s = SearchManager.FillDbCommand(searchExpression, cmd.Parameters, true, ref paramCnt, m_selectAsColumns);
                            if (!string.IsNullOrEmpty(s))
                            {
                                cmd.CommandText += " HAVING " + s;
                            }
                        }
                    }

                    cmd.CommandText += SearchManager.GetSqlOrders(searchOrders, false, null);
                }

                if (paramCnt > 2100)
                {
                    throw new NotSupportedException("您输入的参数过多，系统最多只支持2100个参数！");
                }
            }

            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public virtual DbCommand CreateCountDbCommand(ISearchExpression searchExpression)
        {
            DbCommand cmd = DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            if (searchExpression is QueryExpression)
            {
                string str = (searchExpression as QueryExpression).Query;
                str = str.ToUpper();
                int idx = str.IndexOf("' '");
                int idx2 = str.IndexOf("FROM");
                str = str.Replace(str.Substring(idx, idx2 - idx), " COUNT(*) ");
                cmd.CommandText = str;
            }
            else
            {
                int paramCnt = 0;
                // Add Where 
                if (searchExpression != null)
                {
                    string s = SearchManager.FillDbCommand(searchExpression, cmd.Parameters, false, ref paramCnt, m_selectAsColumns);
                    if (!string.IsNullOrEmpty(s))
                    {
                        cmd.CommandText += " WHERE " + s;
                    }
                }

                string groupBy = SearchManager.GetGroupCommand(m_groupBySql);
                if (!string.IsNullOrEmpty(groupBy))
                {
                    cmd.CommandText += groupBy;

                    // Add Having
                    if (searchExpression != null)
                    {
                        string s = SearchManager.FillDbCommand(searchExpression, cmd.Parameters, true, ref paramCnt, m_selectAsColumns);
                        if (!string.IsNullOrEmpty(s))
                        {
                            cmd.CommandText += " HAVING " + s;
                        }
                    }
                }

                if (paramCnt > 2100)
                {
                    throw new NotSupportedException("您输入的参数过多，系统最多只支持2100个参数！");
                }
            }

            return cmd;
        }

        /// <summary>
        /// 查询数据条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public override int GetCount(ISearchExpression searchExpression)
        {
            if (this.EnablePage && m_dataCanPage)
            {
                return (int)DbHelper.Instance.ExecuteScalar(CreateCountDbCommand(searchExpression));
            }
            else
            {
                return base.Count;
            }
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerQuery sm = new SearchManagerQuery(m_queryString);
            Copy(this, sm);
            return sm;
        }
    }
}
