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
    /// 用于Sql的查找管理器
    /// </summary>
    public class SearchManager : AbstractSearchManager
    {
        #region "Constructor"
        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        public SearchManager(string tableName, string defaultOrder)
            : this(tableName, defaultOrder, null, null)
        {
            
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="selectClause"></param>
        /// <param name="groupByClause"></param>
        public SearchManager(string tableName, string defaultOrder, string selectClause, string groupByClause)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (string.IsNullOrEmpty(defaultOrder))
            {
                throw new ArgumentNullException("defaultOrder");
            }
            m_tableName = tableName;
            m_defaultOrder = defaultOrder;

            //if (string.IsNullOrEmpty(selectSql))
            //{
            //    throw new ArgumentNullException("selectSql");
            //}
            //if (string.IsNullOrEmpty(groupSql))
            //{
            //    throw new ArgumentNullException("groupSql");
            //}
            m_selectSql = selectClause;
            m_groupBySql = groupByClause;

            if (!string.IsNullOrEmpty(m_groupBySql))
            {
                if (string.IsNullOrEmpty(selectClause))
                {
                    throw new ArgumentException("selectSql must not be null when groupBySql is not null!", "selectClause");
                }

                //// 当group时，不支持分页
                m_dataCanPage = false;
            }

            m_selectAsColumns = ParseSelectCommand(m_selectSql);
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="selectClause"></param>
        /// <param name="groupByClause"></param>
        /// <param name="usePage"></param>
        public SearchManager(string tableName, string defaultOrder, string selectClause, string groupByClause, bool usePage)
            : this(tableName, defaultOrder, selectClause, groupByClause)
        {
            this.EnablePage = usePage;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManager sm = new SearchManager(this.m_tableName, m_defaultOrder, m_selectSql, m_groupBySql, this.EnablePage);
            Copy(this, sm);
            return sm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected void Copy(SearchManager src, SearchManager dest)
        {
            dest.m_dataCanPage = src.m_dataCanPage;
            dest.m_selectAsColumns = src.m_selectAsColumns;

            base.Copy(src, dest);
        }
        #endregion

        #region "DbCommand"
        private bool m_dataCanPage = true;
        private string m_tableName;
        private string m_defaultOrder;
        private string m_selectSql;
        private string m_groupBySql;

        internal string TableName
        {
            get { return m_tableName; }
        }

        internal string GroupBySql
        {
            get { return m_groupBySql; }
        }
        
        //// <summary>
        ///// 设置基本信息
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="primaryKeyName"></param>
        //private void SetTableName(string tableName, string primaryKeyName)
        //{

        //}

        // Todo: SearchManangerFunction 中参数命名，如果多个的时候有问题
        private static string GetParameterName(DbParameterCollection dbParams, string baseName)
        {
            baseName = baseName.Replace('(', '9').Replace(')', '9').Replace('*', '9');

            if (dbParams == null || !dbParams.Contains(baseName))
            {
                return baseName;
            }
            int i = 0;
            while (true)
            {
                string name = baseName + i.ToString();
                if (!dbParams.Contains(name))
                {
                    return name;
                }
                i++;
            }
        }

        /// <summary>
        /// 查找字符串是否是精确模式还是模糊模式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsAcurateMode(string s)
        {
            if (s.EndsWith("#"))
            {
                s = s.Substring(0, s.Length - 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string FillLikeCommand(DbParameterCollection dbParams, string columnName, string paraName, object data, bool not, bool or)
        {
            if (data is string)
            {
                string s = data.ToString();
                if (IsAcurateMode(s))
                {
                    dbParams.Add(DbHelper.Instance.CreateParameter(paraName, s.Substring(0, s.Length - 1)));
                    return (or ? " OR " : "") + columnName + (not ? "!=" : " = ") + paraName;
                }
                else
                {
                    dbParams.Add(DbHelper.Instance.CreateParameter(paraName, "%" + data + "%"));
                    return (or ? " OR " : "") + columnName + (not ? " NOT LIKE " : " LIKE ") + paraName;
                }
            }
            else
            {
                dbParams.Add(DbHelper.Instance.CreateParameter(paraName, data));
                return (or ? " OR " : "") + columnName + (not ? "!=" : " = ") + paraName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cse"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        protected static string CreateParamName(SimpleExpression cse, DbParameterCollection dbParams)
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
                    return GetParameterName(dbParams, simpleParamName);
                case SimpleOperator.Ge:
                case SimpleOperator.Gt:
                case SimpleOperator.Le:
                case SimpleOperator.Lt:
                    return GetParameterName(dbParams, complexParamName);
                case SimpleOperator.Eq:
                case SimpleOperator.GInG:
                case SimpleOperator.InG:
                case SimpleOperator.Like:
                    return GetParameterName(dbParams, simpleParamName);
                default:
                    throw new NotSupportedException("Invalid SimpleExpression Operator of " + cse.Operator);
            }
        }

        internal static string FillDbCommand(ISearchExpression se, DbParameterCollection dbParams, bool isHaving, ref int paramCnt, Dictionary<string, string> selectAsColumns)
        {
            if (se == null)
                return null;
            if (se is QueryExpression)
                return null;

            if (se is LogicalExpression)
            {
                LogicalExpression le = se as LogicalExpression;

                switch (le.LogicOperator)
                {
                    case LogicalOperator.And:
                        {
                            string b1 = FillDbCommand(le.LeftHandSide, dbParams, isHaving, ref paramCnt, selectAsColumns);
                            string b2 = FillDbCommand(le.RightHandSide, dbParams, isHaving, ref paramCnt, selectAsColumns);
                            if (!string.IsNullOrEmpty(b1) && !string.IsNullOrEmpty(b2))
                            {
                                return "(" + b1 + " AND " + b2 + ")";
                            }
                            else if (!string.IsNullOrEmpty(b1) && string.IsNullOrEmpty(b2))
                            {
                                return b1;
                            }
                            else if (string.IsNullOrEmpty(b1) && !string.IsNullOrEmpty(b2))
                            {
                                return b2;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    case LogicalOperator.Or:
                        {
                            string b1 = FillDbCommand(le.LeftHandSide, dbParams, isHaving, ref paramCnt, selectAsColumns);
                            string b2 = FillDbCommand(le.RightHandSide, dbParams, isHaving, ref paramCnt, selectAsColumns);
                            if (!string.IsNullOrEmpty(b1) && !string.IsNullOrEmpty(b2))
                            {
                                return "(" + b1 + " OR " + b2 + ")";
                            }
                            else if (!string.IsNullOrEmpty(b1) && string.IsNullOrEmpty(b2))
                            {
                                return b1;
                            }
                            else if (string.IsNullOrEmpty(b1) && !string.IsNullOrEmpty(b2))
                            {
                                return b2;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    case LogicalOperator.Not:
                        {
                            string b1 = FillDbCommand(le.LeftHandSide, dbParams, isHaving, ref paramCnt, selectAsColumns);
                            if (!string.IsNullOrEmpty(b1))
                            {
                                return "(NOT " + b1 + ")";
                            }
                            else
                            {
                                return null;
                            }
                        }
                    default:
                        throw new ArgumentException("Invalid LogicalOperator!", "se");
                }
            }
            else if (se is SimpleExpression)
            {
                SimpleExpression cse = se as SimpleExpression;

                if (cse.FullPropertyName.Contains(":"))
                {
                    throw new ArgumentException("Not supported in Sql SearchManager with ':' pattern");
                }

                bool isInHaving = IsInHaving(se, selectAsColumns);
                if ((isHaving && isInHaving) || (!isHaving && !isInHaving))
                {
                    string columnName = cse.FullPropertyName;
                    if (isHaving)
                    {
                        columnName = selectAsColumns[columnName];
                    }
                    //string paraName = "@" + GetParameterName(cmd, cse.FullPropertyName + cse.Operator.ToString());
                    string paraName = CreateParamName(cse, dbParams);

                    IList arrayData = cse.Values as IList;
                    if (arrayData != null)
                    {
                        paramCnt += arrayData.Count;
                    }
                    else
                    {
                        paramCnt++;
                    }

                    switch (cse.Operator)
                    {
                        case SimpleOperator.Eq:
                            dbParams.Add(DbHelper.Instance.CreateParameter(paraName, cse.Values));
                            return columnName + " = " + paraName;
                        case SimpleOperator.NotEq:
                            dbParams.Add(DbHelper.Instance.CreateParameter(paraName, cse.Values));
                            return columnName + " <> " + paraName;
                        case SimpleOperator.EqProperty:
                            return columnName + " = " + cse.Values;
                        case SimpleOperator.NotEqProperty:
                            return columnName + " <> " + cse.Values;
                        case SimpleOperator.Like:
                            return FillLikeCommand(dbParams, columnName, paraName, cse.Values, false, false);
                        case SimpleOperator.InG:
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(columnName + " IN (");
                                IList data = cse.Values as IList;
                                System.Diagnostics.Debug.Assert(data != null, "SimpleOperator.InG's Values should be ArrayList!");

                                for (int i = 0; i < data.Count; ++i)
                                {
                                    string paraNameI = paraName + i.ToString();
                                    sb.Append(paraNameI);
                                    if (i != data.Count - 1)
                                    {
                                        sb.Append(",");
                                    }

                                    dbParams.Add(DbHelper.Instance.CreateParameter(paraNameI, data[i]));
                                }
                                sb.Append(')');
                                return sb.ToString();
                            }
                        case SimpleOperator.IsNotNull:
                            return columnName + " IS NOT NULL";
                        case SimpleOperator.IsNull:
                            return columnName + " IS NULL";
                        case SimpleOperator.Gt:
                            dbParams.Add(DbHelper.Instance.CreateParameter(paraName, cse.Values));
                            return columnName + " > " + paraName;
                        case SimpleOperator.Lt:
                            dbParams.Add(DbHelper.Instance.CreateParameter(paraName, cse.Values));
                            return columnName + " > " + paraName;
                        case SimpleOperator.Ge:
                            dbParams.Add(DbHelper.Instance.CreateParameter(paraName, cse.Values));
                            return columnName + " >= " + paraName;
                        case SimpleOperator.Le:
                            dbParams.Add(DbHelper.Instance.CreateParameter(paraName, cse.Values));
                            return columnName + " <= " + paraName;
                        case SimpleOperator.GInG:
                            {
                                StringBuilder sb = new StringBuilder();
                                IList data = cse.Values as IList;
                                System.Diagnostics.Debug.Assert(data != null, "SimpleOperator.InG's Values should be ArrayList!");

                                //if (data.Count <= 1)
                                //{
                                    sb.Append("( ");
                                    sb.Append(FillLikeCommand(dbParams, columnName, paraName, data[0], false, false));
                                    for (int i = 1; i < data.Count; ++i)
                                    {
                                        string paraNameI = paraName + i.ToString();

                                        sb.Append(FillLikeCommand(dbParams, columnName, paraNameI, data[i], false, true));
                                    }
                                    sb.Append(" )");
                                //}
                                //else
                                //{
                                //    sb.Append(columnName + " IN (");
                                //    for (int i = 0; i < data.Count; ++i)
                                //    {
                                //        string paraNameI = paraName + i.ToString();
                                //        sb.Append(paraNameI);
                                //        if (i != data.Count - 1)
                                //        {
                                //            sb.Append(",");
                                //        }

                                    //        dbParams.Add(DbHelper.Instance.CreateParameter(paraNameI, data[i]));
                                //    }
                                //    sb.Append(')');
                                //}
                                return sb.ToString();
                            }
                        case SimpleOperator.Any:
                            return null;
                        case SimpleOperator.Sql:
                            {
                                return cse.FullPropertyName;
                            }
                        default:
                            throw new NotSupportedException("Not supported SimpleOperator");
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new NotSupportedException("Invalid SearchExpression of " + se.ToString());
            }
        }

        /// <summary>
        /// 判断条件语句是否在having子句中
        /// </summary>
        /// <param name="se"></param>
        /// <param name="selectAsColumns"></param>
        /// <returns></returns>
        private static bool IsInHaving(ISearchExpression se, Dictionary<string, string> selectAsColumns)
        {
            //if (se is LogicalExpression)
            //{
            //    LogicalExpression lfc = se as LogicalExpression;
            //    if (lfc.LogicOperator != LogicalOperator.Not)
            //    {
            //        return IsInHaving(lfc.LeftHandSide) || IsInHaving(lfc.RightHandSide);
            //    }
            //    else
            //    {
            //        return IsInHaving(lfc.LeftHandSide);
            //    }
            //}
            if (se is SimpleExpression)
            {
                SimpleExpression cfc = se as SimpleExpression;
                return selectAsColumns.ContainsKey(cfc.FullPropertyName);
            }
            else
            {
                throw new NotSupportedException("invalid ISearchExpression in judge IsInHaving!");
            }
        }

        /// <summary>
        /// 根据查询条件创建Sql命令
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        protected virtual DbCommand CreateDbCommand(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            DbCommand cmd = DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            if (searchExpression is QueryExpression)
            {
                string s = (searchExpression as QueryExpression).Query;
                cmd.CommandText = s;
                return cmd;
            }

            int paramCnt = 0;

            if (this.EnablePage && m_dataCanPage)
            {
                cmd.CommandText = "SELECT " + (IsResultDistinct ? "DISTINCT" : "") + " * FROM (" + GetSelectCommand() + ", ROW_NUMBER() OVER(";
                cmd.CommandText += GetSqlOrders(searchOrders, true, m_defaultOrder);
                cmd.CommandText += " ) AS RowNumber FROM " + m_tableName;

                // Add Where 
                if (searchExpression != null)
                {
                    string s = FillDbCommand(searchExpression, cmd.Parameters, false, ref paramCnt, m_selectAsColumns);
                    if (!string.IsNullOrEmpty(s))
                    {
                        cmd.CommandText += " WHERE " + s;
                    }
                }

                string groupBy = GetGroupCommand(m_groupBySql);
                if (!string.IsNullOrEmpty(groupBy))
                {
                    cmd.CommandText += groupBy;

                    // Add Having
                    if (searchExpression != null)
                    {
                        string s = FillDbCommand(searchExpression, cmd.Parameters, true, ref paramCnt, m_selectAsColumns);
                        if (!string.IsNullOrEmpty(s))
                        {
                            cmd.CommandText += " HAVING " + s;
                        }
                    }
                }

                cmd.CommandText += ") AS TEMPTABLE";

                cmd.CommandText += " WHERE RowNumber >= @RowNumber1";
                cmd.Parameters.Add(DbHelper.Instance.CreateParameter("@RowNumber1", base.FirstResult + 1));
                if (base.MaxResult != -1)
                {
                    cmd.CommandText += " AND RowNumber < @RowNumber2";
                    cmd.Parameters.Add(DbHelper.Instance.CreateParameter("@RowNumber2", base.FirstResult + base.MaxResult + 1));
                }
            }
            else
            {
                cmd.CommandText = GetSelectCommand() + " FROM " + m_tableName;

                // Add Where 
                if (searchExpression != null)
                {
                    string s = FillDbCommand(searchExpression, cmd.Parameters, false, ref paramCnt, m_selectAsColumns);
                    if (!string.IsNullOrEmpty(s))
                    {
                        cmd.CommandText += " WHERE " + s;
                    }
                }

                string groupBy = GetGroupCommand(m_groupBySql);
                if (!string.IsNullOrEmpty(groupBy))
                {
                    cmd.CommandText += groupBy;

                    // Add Having
                    if (searchExpression != null)
                    {
                        string s = FillDbCommand(searchExpression, cmd.Parameters, true, ref paramCnt, m_selectAsColumns);
                        if (!string.IsNullOrEmpty(s))
                        {
                            cmd.CommandText += " HAVING " + s;
                        }
                    }
                }

                cmd.CommandText += GetSqlOrders(searchOrders, true, m_defaultOrder);
            }

            if (paramCnt > 2100)
            {
                throw new NotSupportedException("您输入的参数过多，系统最多只支持2100个参数！");
            }

            return cmd;
        }

        internal static string GetGroupCommand(string groupBySql)
        {
            if (string.IsNullOrEmpty(groupBySql))
                return string.Empty;
            else
                return " " + groupBySql;
        }

        private string GetSelectCommand()
        {
            if (string.IsNullOrEmpty(m_selectSql))
            {
                m_selectAsColumns = new Dictionary<string, string>();
                return "SELECT *";
            }
            else
            {
                return m_selectSql;
            }
        }

        private Dictionary<string, string> m_selectAsColumns;
        internal Dictionary<string, string> SelectAsColumns
        {
            get { return m_selectAsColumns; }
        }
        internal static Dictionary<string, string> ParseSelectCommand(string selectSql)
        {
            Dictionary<string, string> selectAsColumns = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(selectSql))
            {
                selectSql = selectSql.ToUpper();
                selectSql = selectSql.Replace("SELECT", "");
                // Todo: 不支持类似于 DATEADD(dd, - 390, GETDATE()) AS 结算期限 的
                string[] ss = selectSql.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ss)
                {
                    int idx = s.LastIndexOf("AS");
                    if (idx != -1)
                    {
                        System.Diagnostics.Debug.Assert(idx > 1 && idx + 3 < s.Length, "SELECT format is invalid!");

                        string left = s.Substring(0, idx - 1).Trim();
                        string right = s.Substring(idx + 3).Trim();

                        if (left.Contains("(") && left.Contains(")"))   // having must in group
                        {
                            selectAsColumns[right] = left;
                        }
                    }
                }
            }
            return selectAsColumns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        protected virtual DbCommand CreateCountDbCommand(ISearchExpression searchExpression)
        {
            DbCommand cmd = DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            if (searchExpression is QueryExpression)
            {
                string str = (searchExpression as QueryExpression).Query;
                str = str.ToUpper();
                int idx = str.IndexOf(' ');
                int idx2 = str.IndexOf("FROM");
                str = str.Replace(str.Substring(idx, idx2 - idx), " COUNT(*) ");
                cmd.CommandText = str;
                return cmd;
            }

            int paramCnt = 0;
            cmd.CommandText = "SELECT COUNT(*) FROM " + m_tableName;

            // Add Where 
            if (searchExpression != null)
            {
                string s = FillDbCommand(searchExpression, cmd.Parameters, false, ref paramCnt, m_selectAsColumns);
                if (!string.IsNullOrEmpty(s))
                {
                    cmd.CommandText += " WHERE " + s;
                }
            }

            string groupBy = GetGroupCommand(m_groupBySql);
            if (!string.IsNullOrEmpty(groupBy))
            {
                cmd.CommandText += groupBy;

                // Add Having
                if (searchExpression != null)
                {
                    string s = FillDbCommand(searchExpression, cmd.Parameters, true, ref paramCnt, m_selectAsColumns);
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

            return cmd;
        }

        internal static string GetSqlOrders(IList<ISearchOrder> searchOrders, bool must, string defaultOrder)
        {
            StringBuilder sb = new StringBuilder();
            if (searchOrders != null)
            {
                bool firstOrder = true;
                foreach (ISearchOrder searchOrder in searchOrders)
                {
                    if (searchOrder.PropertyName.Contains(":"))
                    {
                        throw new NotSupportedException("Not supported in Sql SearchManager with ':' pattern");
                    }

                    if (firstOrder)
                    {
                        sb.Append(" ORDER BY ");
                        firstOrder = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    sb.Append(searchOrder.PropertyName + (searchOrder.Ascending ? " ASC " : " DESC "));
                }
            }
            if (sb.Length == 0)
            {
                if (must)
                {
                    if (string.IsNullOrEmpty(defaultOrder))
                    {
                        throw new ArgumentException("DefaultOrder must be supplied!", "defaultOrder");
                    }
                    return " ORDER BY " + defaultOrder;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return sb.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 读入数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(CreateDbCommand(searchExpression, searchOrders));
            dt.TableName = m_tableName;

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
                for (int i = 0; i < Math.Min(dt.Rows.Count - this.FirstResult, this.MaxResult); ++i)
                {
                    dt2.ImportRow(dt.Rows[i + this.FirstResult]);
                }
                this.Result = dt2.DefaultView;
            }
            else
            {
                this.Result = dt.DefaultView;
            }
        }

        /// <summary>
        /// 
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
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(CreateDbCommand(searchExpression, searchOrders));
            dt.TableName = m_tableName;

            return dt.DefaultView;
        }

        //private static string ReplaceWhereAndToWhere(string where)
        //{
        //    int nStrAnd = where.IndexOf("WHERE AND");
        //    if (nStrAnd != -1)
        //    {
        //        where = where.Remove(nStrAnd, 9);
        //        where = where.Insert(nStrAnd, "WHERE");
        //    }
        //    else
        //    {
        //        nStrAnd = where.LastIndexOf("WHERE");
        //        where = where.Remove(nStrAnd, 5);
        //    }
        //    return where;
        //}
    }
}