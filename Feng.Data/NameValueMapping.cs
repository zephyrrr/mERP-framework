using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Feng
{
    /// <summary>
    /// 用于名称和实际值转换的类，类似于员工编号和姓名的转换
    /// </summary>
    public class NameValueMapping : ICloneable
    {
        private string m_valueMember;

        /// <summary>
        /// 实际值
        /// </summary>
        public string ValueMember
        {
            get { return m_valueMember; }
            set { m_valueMember = value; }
        }

        private string m_displayMember;

        /// <summary>
        /// 名称
        /// </summary>
        public string DisplayMember
        {
            get { return m_displayMember; }
            set { m_displayMember = value; m_memberVisible[value] = true;  }
        }

        private Dictionary<string, bool> m_memberVisible = new Dictionary<string, bool>();

        /// <summary>
        /// 从数据库读入的字段的是否显示属性
        /// </summary>
        public Dictionary<string, bool> MemberVisible
        {
            get { return m_memberVisible; }
        }

        private string m_tableName;
        ///// <summary>
        ///// 表名
        ///// </summary>
        //public string TableName
        //{
        //    get { return m_tableName; }
        //}

        private string m_name;

        /// <summary>
        /// 此项Mapping的名字，用于识别
        /// </summary>
        public string Name
        {
            get { return m_name; }
            internal set { m_name = value; }
        }

        private string m_whereQuery;
        /// <summary>
        /// Sql语句中的Where 或者是 Filter
        /// </summary>
        public string WhereQuery
        {
            get { return m_whereQuery; }
            internal set { m_whereQuery = value; }
        }

        private string m_parentName;
        /// <summary>
        /// 
        /// </summary>
        public string ParentName
        {
            get { return m_parentName; }
            internal set { m_parentName = value; }
        }

        private Dictionary<string, object> m_params = new Dictionary<string,object>();

        /// <summary>
        /// 查找时参数（运行时指定）
        /// </summary>
        public Dictionary<string, object> Params
        {
            get { return m_params; }
        }

        /// <summary>
        /// 把所有参数
        /// </summary>
        public void ResetParams()
        {
            string[] paramName = new string[m_params.Count];
            int idx = 0;
            foreach (string s in m_params.Keys)
            {
                paramName[idx] = s;
                idx++;
            }
            foreach (string s in paramName)
            {
                m_params[s] = System.DBNull.Value;
            }
        }

        /// <summary>
        /// 是否动态的（如果是动态的，需保存如不同的DataSet）
        /// </summary>
        public bool IsDynamic
        {
            get { return m_params.Count > 0; }
        }

        /// <summary>
        /// 初始化Mapping ,从数据库读入相关数据
        /// </summary>
        /// <param name="tableName">表名,也作为此项Mapping的名字</param>
        /// <param name="members">要在ComboBox中显示的Column，第一栏为ValueMember，第二栏为DisplayMember（如只有一栏，两者相同）</param>
        public NameValueMapping(string tableName, string[] members)
            : this(tableName, tableName, members)
        {
        }

        /// <summary>
        /// 初始化Mapping ,从数据库读入相关数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tableName"></param>
        /// <param name="members"></param>
        public NameValueMapping(string name, string tableName, string[] members)
            : this(name, tableName, members, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tableName"></param>
        /// <param name="members"></param>
        /// <param name="whereQuery"></param>
        public NameValueMapping(string name, string tableName, string[] members, string whereQuery)
            : this(name, tableName, members, whereQuery, null)
        {
        }

        /// <summary>
        /// 初始化Mapping ,从数据库读入相关数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tableName"></param>
        /// <param name="members"></param>
        /// <param name="whereQuery"></param>
        /// <param name="parentName"></param>
        public NameValueMapping(string name, string tableName, string[] members, string whereQuery, string parentName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (members == null || members.Length == 0)
            {
                throw new ArgumentNullException("members");
            }

            m_name = name;
            m_tableName = tableName;
            m_whereQuery = whereQuery;
            m_parentName = parentName;

            for (int i = 0; i < members.Length; ++i)
            {
                m_memberVisible[members[i]] = i < 2 ? true : false;
            }

            if (members.Length >= 2)
            {
                m_valueMember = members[0];
                m_displayMember = members[1];
            }
            else
            {
                m_valueMember = members[0];
                m_displayMember = members[0];
            }
        }

        /// <summary>
        /// 按照this复制一个NameValueMapping，以给定名字作为名称
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            string[] members = new string[m_memberVisible.Count];
            m_memberVisible.Keys.CopyTo(members, 0);
            NameValueMapping nv = new NameValueMapping(m_name, m_tableName, members, m_whereQuery);
            foreach (KeyValuePair<string, bool> kvp in m_memberVisible)
            {
                nv.m_memberVisible[kvp.Key] = m_memberVisible[kvp.Key];
            }
            nv.m_displayMember = this.m_displayMember;
            nv.m_valueMember = this.m_valueMember;

            return nv;
        }

        private System.Data.DataTable m_emtpyDt;
        internal System.Data.DataTable EmptyDataTable
        {
            get
            {
                if (m_emtpyDt == null)
                {
                    m_emtpyDt = new System.Data.DataTable();
                    foreach (KeyValuePair<string, bool> kvp in m_memberVisible)
                    {
                        m_emtpyDt.Columns.Add(kvp.Key);
                    }
                }
                return m_emtpyDt;
            }
        }
        /// <summary>
        /// 用于InitDataControl的相应数据库操作命令
        /// </summary>
        internal System.Data.Common.DbCommand SelectCommand
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("SELECT DISTINCT ");
                foreach (KeyValuePair<string, bool> kvp in m_memberVisible)
                {
                    sb.Append(string.Format("[{0}], ", kvp.Key));
                }

                sb.Remove(sb.Length - 2, 2);

                sb.Append(" FROM [" + m_tableName + "]");

                if (!string.IsNullOrEmpty(m_whereQuery))
                {
                    sb.Append(" WHERE " + m_whereQuery);
                }

                if (m_memberVisible[m_valueMember])
                {
                    sb.Append(" ORDER BY [" + m_valueMember + "]");
                }
                else
                {
                    sb.Append(" ORDER BY [" + m_displayMember + "]");
                }

                System.Data.Common.DbCommand cmd = Feng.Data.DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());

                foreach (KeyValuePair<string, object> kvp in m_params)
                {
                    System.Data.Common.DbParameter parameter = Feng.Data.DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                    if (kvp.Value.GetType().IsEnum)
                    {
                        parameter.ParameterName = kvp.Key;
                        parameter.Value = (int)kvp.Value;
                        //cmd.Parameters.AddWithValue(kvp.Key, (int)kvp.Value);
                    }
                    else
                    {
                        parameter.ParameterName = kvp.Key;
                        parameter.Value = kvp.Value;
                        //cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                    cmd.Parameters.Add(parameter);
                }

                return cmd;
            }
        }

        /// <summary>
        /// DatabaseName. default is null which use default database in app.config
        /// </summary>
        public string DatabaseName
        {
            get;
            set;
        }

        ///// <summary>
        ///// DataTable
        ///// </summary>
        //public System.Data.DataTable DataTable
        //{
        //    get { return NameValueMappingCollection.Instance.DataTable(this.Name); }
        //}

        /// <summary>
        /// DataTableChanged. Occured when reload in NameValueMappingCollection
        /// </summary>
        public event EventHandler DataSourceChanged;

        /// <summary>
        /// 
        /// </summary>
        internal void OnDataSourceChanged(EventArgs e)
        {
            if (DataSourceChanged != null)
            {
                DataSourceChanged(this, e);
            }
        }

        /// <summary>
        /// DataTableChanged. Occured when reload in NameValueMappingCollection
        /// </summary>
        public event CancelEventHandler DataSourceChanging;

        /// <summary>
        /// 
        /// </summary>
        internal void OnDataSourceChanging(CancelEventArgs e)
        {
            if (DataSourceChanging != null)
            {
                DataSourceChanging(this, e);
            }
        }

        /// <summary>
        /// Reload
        /// </summary>
        /// <param name="dsName"></param>
        public void Reload(string dsName)
        {
            NameValueMappingCollection.Instance.Reload(dsName, this.Name);
        }
    }
}