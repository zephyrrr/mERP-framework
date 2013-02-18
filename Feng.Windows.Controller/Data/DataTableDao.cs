using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataTableDao : AbstractEventDao, IBatchDao
    {
        private enum OperateType
        {
            Insert,
            Update,
            Delete
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataTableDao()
        {
        }

        private bool m_isBatchOperation;
        private List<MyDbCommand> m_batchCmds = new List<MyDbCommand>();
        private List<Tuple<OperateType, System.Data.DataRow>> m_batchDataRows = new List<Tuple<OperateType, System.Data.DataRow>>();

        /// <summary>
        /// 不立即提交操作，而是放在缓存里，等ResumeOperation时提交
        /// </summary>
        public void SuspendOperation()
        {
            m_isBatchOperation = true;
        }

        /// <summary>
        /// 提交操作
        /// </summary>
        public void ResumeOperation()
        {
            try
            {
                DbHelper.Instance.UpdateTransaction(m_batchCmds);
                for (int i = 0; i < m_batchDataRows.Count; ++i)
                {
                    if (m_batchDataRows[i].Item1 == OperateType.Insert || m_batchDataRows[i].Item1 == OperateType.Update)
                    {
                        m_batchDataRows[i].Item2.AcceptChanges();
                    }
                    else
                    {
                        //m_batchDataRows[i].Second.Delete();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                m_batchCmds.Clear();
                m_isBatchOperation = false;
            }
        }

        /// <summary>
        /// 取消挂起的操作
        /// </summary>
        public void CancelSuspendOperation()
        {
            m_isBatchOperation = false;
        }

        private Dictionary<string, bool> m_insertColumns = null;
        private Dictionary<string, bool> m_updateColumns = null;
        private Dictionary<string, bool> m_deleteColumns = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="insertColumns"></param>
        /// <param name="updateColumns"></param>
        /// <param name="deleteColumns"></param>
        public DataTableDao(string insertColumns, string updateColumns, string deleteColumns)
        {
            GenerateDictColumns(ref m_insertColumns, insertColumns);
            GenerateDictColumns(ref m_updateColumns, updateColumns);
            GenerateDictColumns(ref m_deleteColumns, deleteColumns);
        }

        private void GenerateDictColumns(ref Dictionary<string, bool> dict, string columns)
        {
            if (!string.IsNullOrEmpty(columns))
            {
                dict = new Dictionary<string, bool>();
                string[] ss = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ss)
                {
                    dict[s] = true;
                }
            }
        }
        private bool IsInColumns(OperateType type, string columnName, bool inWhere)
        {
            if (columnName == "RowNumber")
                return false;

            switch (type)
            {
                case OperateType.Insert:
                    {
                        return m_insertColumns == null || m_insertColumns.ContainsKey(columnName);
                    }
                case OperateType.Update:
                    {
                        if (inWhere)
                            return true;
                        return m_updateColumns == null || m_updateColumns.ContainsKey(columnName);
                    }
                case OperateType.Delete:
                    {
                        return m_deleteColumns == null || m_deleteColumns.ContainsKey(columnName);
                    }
            }
            return false;
        }

        private Dictionary<string, Dictionary<string, System.Data.DataRow>> m_schemaTables = new Dictionary<string,Dictionary<string,System.Data.DataRow>>();
        private Dictionary<string, System.Data.DataRow> GetSchemaTable(string tableName)
        {
            if (!m_schemaTables.ContainsKey(tableName))
            {
                System.Data.DataTable schemaTable = DbHelper.Instance.GetSchema(tableName);
                m_schemaTables[tableName] = new Dictionary<string,System.Data.DataRow>();
                foreach(System.Data.DataRow row in schemaTable.Rows)
                {
                    m_schemaTables[tableName][row["ColumnName"].ToString()] = row;
                }
            }
            return m_schemaTables[tableName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(object entity)
        {
            System.Data.DataRowView row = entity as System.Data.DataRowView;
            System.Diagnostics.Debug.Assert(row != null, "DataTableDao's entity shold be DataRowView!");

            System.Data.Common.DbCommand cmd = GenerateInsertCommand(row.Row);
            MyDbCommand mCmd = new MyDbCommand(cmd, ExpectedResultTypes.Special, "1");

            if (m_isBatchOperation)
            {
                m_batchCmds.Add(mCmd);
                m_batchDataRows.Add(new Tuple<OperateType, System.Data.DataRow>(OperateType.Insert, row.Row));
            }
            else
            {
                bool ret = DbHelper.Instance.ExecuteMyCommand(mCmd);
                if (ret)
                {
                    row.Row.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(object entity)
        {
            System.Data.DataRowView row = entity as System.Data.DataRowView;
            System.Diagnostics.Debug.Assert(row != null, "DataTableDao's entity shold be DataRowView!");

            System.Data.Common.DbCommand cmd = GenerateUpdateCommand(row.Row);
            MyDbCommand mCmd = new MyDbCommand(cmd, ExpectedResultTypes.Special, "1");

            if (m_isBatchOperation)
            {
                m_batchCmds.Add(mCmd);
                m_batchDataRows.Add(new Tuple<OperateType, System.Data.DataRow>(OperateType.Update, row.Row));
            }
            else
            {
                bool ret = DbHelper.Instance.ExecuteMyCommand(mCmd);
                if (ret)
                {
                    row.Row.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(object entity)
        {
            System.Data.DataRowView row = entity as System.Data.DataRowView;
            System.Diagnostics.Debug.Assert(row != null, "DataTableDao's entity shold be DataRowView!");

            System.Data.Common.DbCommand cmd = GenerateDeleteCommand(row.Row);
            MyDbCommand mCmd = new MyDbCommand(cmd, ExpectedResultTypes.Special, "1");

            if (m_isBatchOperation)
            {
                m_batchCmds.Add(mCmd);
                m_batchDataRows.Add(new Tuple<OperateType, System.Data.DataRow>(OperateType.Delete, row.Row));
            }
            else
            {
                bool ret = DbHelper.Instance.ExecuteMyCommand(mCmd);
                if (ret)
                {
                    // 不需要在这里删除，会在DisplayManager里删除
                    //row.Row.Delete();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public override void SaveOrUpdate(object entity)
        {
            throw new NotImplementedException("SaveOrUpdate is not implemented in DataTableDao!");
        }

        private System.Data.Common.DbCommand GenerateDeleteCommand(System.Data.DataRow row)
        {
            string tableName = row.Table.TableName;

            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(tableName);
            sb.Append(" WHERE ");

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Delete, column.ColumnName, true))
                    continue;

                //if (column.ColumnName == idName)
                //    continue;
                string s1 = column.ColumnName;
                string s2 = "@DELETE_Original" + i.ToString();
                sb.Append("[" + s1 + "]" + " = " + s2);

                sb.Append(" AND ");
            }
            // remove last AND
            sb.Remove(sb.Length - 5, 5);

            System.Data.Common.DbParameter parameter;
            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Delete, column.ColumnName, true))
                    continue;

                string s1 = column.ColumnName;
                string s2 = "@DELETE_Original" + i.ToString();

                if (row[column.ColumnName, System.Data.DataRowVersion.Original] != System.DBNull.Value)
                {
                    parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                    parameter.ParameterName = s2;
                    parameter.Value = row[column.ColumnName, System.Data.DataRowVersion.Original];

                    cmd.Parameters.Add(parameter);
                }
                else
                {
                    cmd.CommandText = cmd.CommandText.Replace("[" + s1 + "]" + " = " + s2, "[" + s1 + "]" + " IS NULL ");
                }
            }
            return cmd;
        }

        private System.Data.Common.DbCommand GenerateUpdateCommand(System.Data.DataRow row)
        {
            string tableName = row.Table.TableName;
            Dictionary<string, System.Data.DataRow> schemaTable = GetSchemaTable(tableName);

            StringBuilder prefix = new StringBuilder();
            prefix.Append("UPDATE ");
            prefix.Append(tableName);
            prefix.Append(" SET ");

            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            sb.Append(prefix);

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Update, column.ColumnName, false))
                    continue;

                if (!schemaTable.ContainsKey(column.ColumnName))
                    continue;
                if (Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsIdentity"]).Value
                    && Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsAutoIncrement"]).Value)
                    continue;

                string s1 = column.ColumnName;
                string s2 = "@UPDATE" + i.ToString();
                sb.Append("[" + s1 + "]" + " = " + s2);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);

            sb.Append(" WHERE ");

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];

                // Update中Where还是要按照原有所有Column
                if (!IsInColumns(OperateType.Update, column.ColumnName, true))
                    continue;

                //if (column.ColumnName == idName)
                //    continue;
                string s1 = column.ColumnName;
                string s2 = "@UPDATE_Original" + i.ToString();
                sb.Append("[" + s1 + "]" + " = " + s2);

                // 不能按照i-1判断
                //if (i != row.Table.Columns.Count - 1)
                {
                    sb.Append(" AND ");
                }
            }
            // remove last AND
            sb.Remove(sb.Length - 5, 5);

            System.Data.Common.DbParameter parameter;
            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Update, column.ColumnName, false))
                    continue;

                if (!schemaTable.ContainsKey(column.ColumnName))
                    continue;
                if (Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsIdentity"]).Value
                    && Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsAutoIncrement"]).Value)
                    continue;

                string s1 = column.ColumnName;
                string s2 = "@UPDATE" + i.ToString();

                parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                parameter.ParameterName = s2;
                parameter.Value = row[column.ColumnName];
                cmd.Parameters.Add(parameter);
            }

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Update, column.ColumnName, true))
                    continue;

                string s1 = column.ColumnName;
                string s2 = "@UPDATE_Original" + i.ToString();

                if (row[column.ColumnName, System.Data.DataRowVersion.Original] != System.DBNull.Value)
                {
                    parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                    parameter.ParameterName = s2;
                    parameter.Value = row[column.ColumnName, System.Data.DataRowVersion.Original];

                    cmd.Parameters.Add(parameter);
                }
                else
                {
                    cmd.CommandText = cmd.CommandText.Replace("[" + s1 + "]" + " = " + s2, "[" + s1 + "]" + " IS NULL ");
                }
            }

            return cmd;
        }

        private System.Data.Common.DbCommand GenerateInsertCommand(System.Data.DataRow row)
        {
            string tableName = row.Table.TableName;
            Dictionary<string, System.Data.DataRow> schemaTable = GetSchemaTable(tableName);
            
            StringBuilder prefix = new StringBuilder();
            prefix.Append("INSERT INTO ");
            prefix.Append(tableName + " (");

            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            sb.Append(prefix);

            System.Data.Common.DbParameter parameter;
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Insert, column.ColumnName, false))
                    continue;

                if (!schemaTable.ContainsKey(column.ColumnName))
                    continue;
                if (Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsIdentity"]).Value
                    && Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsAutoIncrement"]).Value)
                    continue;

                sb.Append("[" + column.ColumnName + "],");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES (");
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Insert, column.ColumnName, false))
                    continue;

                if (!schemaTable.ContainsKey(column.ColumnName))
                    continue;
                if (Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsIdentity"]).Value
                    && Feng.Utils.ConvertHelper.ToBoolean(schemaTable[column.ColumnName]["IsAutoIncrement"]).Value)
                    continue;

                sb.Append("@INSERT" + i.ToString());
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");

            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (!IsInColumns(OperateType.Insert, column.ColumnName, false))
                    continue;

                parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                parameter.ParameterName = "@INSERT" + i.ToString();
                parameter.Value = row[column.ColumnName];
                cmd.Parameters.Add(parameter);
            }

            return cmd;
        }
    }
}
