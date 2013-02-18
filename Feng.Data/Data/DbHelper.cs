using System;
using System.Data;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;


namespace Feng.Data
{
	/// <summary>
	/// 数据库操作类
    /// 对于多个Database,
    /// <code>
    /// Microsoft.Practices.EnterpriseLibrary.Data.Database db = DbHelper.Instance.CreateDatabase("LocalConnectionString");
    /// System.Data.DataSet ds = db.ExecuteDataSet(new System.Data.SQLite.SQLiteCommand("SELECT * FROM 系统设置_菜单名称"));
    /// </code>
	/// </summary>
	public class DbHelper
	{
		#region "Constructor"
        /// <summary>
        /// Constructor(Please call CreateDbHelper first and then use Instance)
        /// </summary>
        private DbHelper(Database db)
        {
            m_db = db;
        }

        private Database m_db;
        ///// <summary>
        ///// 静态构造函数
        ///// </summary>
        //static DbHelper()
        //{
        //    CreateDbHelper();
        //}

		private static DbHelper s_instance;
        private static bool s_intanceCreated = false;
		/// <summary>
		/// 数据库操作类实例
		/// </summary>
		public static DbHelper Instance
		{
			get 
            {
                if (!s_intanceCreated)
                {
                    s_intanceCreated = true;
                    try
                    {
                        s_instance = new DbHelper(Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase());
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                    }
                }
                return s_instance; 
            }
		}

        ///// <summary>
        ///// CreateDbHelper(must call it before use DbHelper)
        ///// </summary>
        ///// <param name="database"></param>
        //private static void CreateDbHelper(Database database)
        //{
        //    s_instance = new DbHelper();
        //    s_database = database;
        //}

        private static Dictionary<string, DbHelper> m_databases = new Dictionary<string, DbHelper>();

        /// <summary>
        /// CreateDatabase
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DbHelper CreateDatabase(string name)
        {
            if (!m_databases.ContainsKey(name))
            {
                try
                {
                    m_databases[name] = new DbHelper(Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase(name));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    m_databases[name] = null;
                }
            }
            return m_databases[name];
        }


        //private const string s_defaultDatabaseName = "Default";
        /// <summary>
        /// Database in EnterpriseLibrary
        /// </summary>
        public Database Database
        {
            get
            {
                return m_db;
            }
        }

		#endregion

		#region "Get Data From Database"
		/// <summary>
		/// Constructs the data which was extracted from the database according to user's query.
		/// </summary>
		/// <param name="reader">SqlReader - holds the queried data.</param>
		///<returns>Queried data in DataTable.</returns>
		private static DataTable ConstructData(IDataReader reader, Action<System.Data.DataRow> action = null)
		{
			if (reader.IsClosed)
				throw new InvalidOperationException("Attempt to use a closed IDataReader");

			DataTable dataTable = new DataTable();

			// constructs the columns data.
			for (int i = 0; i < reader.FieldCount; i++)
				dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));

			// constructs the table's data.
			while (reader.Read())
			{
				object[] row = new object[reader.FieldCount];
				reader.GetValues(row);

                if (action == null)
                {
                    dataTable.Rows.Add(row);
                }
                else
                {
                    var r = dataTable.NewRow();
                    r.ItemArray = row;
                    action(r);
                }
			}
			// Culture info.
			//dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

			// Accepts changes.
			dataTable.AcceptChanges();

			return dataTable;
		}

        private const string s_logStart = "";
        private static StringBuilder m_logSb;
        private static void LogCommand(DbCommand command)
        {
            if (Logger.IsDebugEnabled)
            {
                if (m_logSb == null)
                {
                    m_logSb = new StringBuilder();
                }
                else
                {
                    m_logSb.Remove(0, m_logSb.Length);
                }
                m_logSb.Append("Execute Database Command: ");
                m_logSb.Append(command.CommandText);
                m_logSb.Append(" With paramters: ");
                foreach (DbParameter i in command.Parameters)
                {
                    m_logSb.Append(i.ParameterName);
                    m_logSb.Append(":");
                    m_logSb.Append(i.Value == null ? "null" : i.Value.ToString());
                }

                Logger.Debug(m_logSb.ToString());
            }
        }

        private const int s_defaultCommandTimeOut = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string command)
        {
            DbCommand cmd = m_db.GetSqlStringCommand(command);
            return ExecuteDataReader(cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(DbCommand command)
        {
            try
            {
                LogCommand(command);

                command.CommandTimeout = s_defaultCommandTimeOut;
                if (command.Transaction == null)
                {
                    return m_db.ExecuteReader(command);
                }
                else
                {
                    if (command.Connection == null)
                    {
                        command.Connection = command.Transaction.Connection;
                    }
                    return m_db.ExecuteReader(command, command.Transaction);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("SQL Command Error: {0}", command.CommandText), ex);
            }
        }

		/// <summary>
		/// 从数据库取得包含指定数据的DataTable
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DataTable</returns>
		private DataTable ExecuteDataTable(DbCommand command, Action<System.Data.DataRow> action)
		{
            using (var reader = ExecuteDataReader(command))
            {
                DataTable dataTable = ConstructData(reader, action);
                return dataTable;
            }
		}

        public DataTable ExecuteDataTable(DbCommand command)
        {
            return ExecuteDataTable(command, null);
        }

		/// <summary>
		/// 从数据库取得包含指定数据的DataTable
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DataTable</returns>
		public DataTable ExecuteDataTable(string command)
		{
            DbCommand cmd = m_db.GetSqlStringCommand(command);
            return ExecuteDataTable(cmd);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="action"></param>
        public void ExecuteDataTable(string cmd, Action<System.Data.DataRow> action)
        {
            DbCommand command = m_db.GetSqlStringCommand(cmd);
            ExecuteDataTable(command, action);
        }

		/// <summary>
		/// 从数据库取得包含指定数据的DataSet
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DataSet</returns>
		public DataSet ExecuteDataSet(string command)
		{
            DbCommand cmd = m_db.GetSqlStringCommand(command);

            LogCommand(cmd);
            try
            {
                return m_db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("SQL Command Error: {0}", command), ex);
            }
		}

		/// <summary>
		/// 从数据库取得包含指定数据的DateView
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DateView</returns>
		public DataView ExecuteDataView(DbCommand command)
		{
			return ExecuteDataTable(command).DefaultView;
		}

		/// <summary>
		/// 从数据库取得包含指定数据的DateView
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DateView</returns>
		public DataView ExecuteDataView(string command)
		{
            DbCommand cmd = m_db.GetSqlStringCommand(command);
            cmd.CommandTimeout = s_defaultCommandTimeOut;
            return ExecuteDataTable(command).DefaultView;
		}

		/// <summary>
		/// 从数据库取得包含指定数据的DataRow
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DataRow</returns>
		public DataRow ExecuteDataRow(string command)
		{
            DbCommand cmd = m_db.GetSqlStringCommand(command);
            cmd.CommandTimeout = s_defaultCommandTimeOut;
            return ExecuteDataRow(cmd);
		}

		/// <summary>
		/// 从数据库取得包含指定数据的DataRow
		/// </summary>
		/// <param name="command">要取得数据的Sql命令</param>
		/// <returns>返回的DataRow</returns>
		public DataRow ExecuteDataRow(DbCommand command)
		{
			DataTable dt = ExecuteDataTable(command);
			DataRow dr = null;
			if (dt.Rows.Count > 0)
				dr = dt.Rows[0];
			return dr;
		}

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteNonQuery(DbCommand command)
        {
            LogCommand(command);

            try
            {
                command.CommandTimeout = s_defaultCommandTimeOut;
                if (command.Transaction == null)
                {
                    m_db.ExecuteNonQuery(command);
                }
                else
                {
                    if (command.Connection == null)
                    {
                        command.Connection = command.Transaction.Connection;
                    }
                    m_db.ExecuteNonQuery(command, command.Transaction);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("SQL Command Error: {0}", command.CommandText), ex);
            }
        }

		/// <summary>
		/// 执行命令
		/// </summary>
		/// <param name="command"></param>
        public void ExecuteNonQuery(string command)
		{
            DbCommand cmd = m_db.GetSqlStringCommand(command);
            ExecuteNonQuery(cmd);
		}

        /// <summary>
        /// 执行命令，返回单个值
        /// </summary>
        /// <param name="command"></param>
        public object ExecuteScalar(DbCommand command)
        {
            LogCommand(command);

            try
            {
                command.CommandTimeout = s_defaultCommandTimeOut;
                if (command.Transaction == null)
                {
                    return m_db.ExecuteScalar(command);
                }
                else
                {
                    if (command.Connection == null)
                    {
                        command.Connection = command.Transaction.Connection;
                    }
                    return m_db.ExecuteScalar(command, command.Transaction);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("SQL Command Error: {0}", command.CommandText), ex);
            }
           
        }

        /// <summary>
        /// 执行命令，返回单个值
        /// </summary>
        /// <param name="command"></param>
        public object ExecuteScalar(string command)
        {
            DbCommand cmd = m_db.GetSqlStringCommand(command);
            return ExecuteScalar(cmd);
        }

        /// <summary>
        /// BulkCopy
        /// </summary>
        /// <param name="dtSrc"></param>
        /// <param name="dtDesc"></param>
        public void BulkCopy(System.Data.DataTable dtSrc, string dtDesc)
        {
            using (DbConnection connection = m_db.CreateConnection())
            {
                System.Data.SqlClient.SqlConnection cn = connection as System.Data.SqlClient.SqlConnection;
                if (cn == null)
                {
                    throw new NotSupportedException("BuldCopy is only supported in MS SQL SERVER now!");
                }
                cn.Open();
                using (System.Data.SqlClient.SqlBulkCopy copy = new System.Data.SqlClient.SqlBulkCopy(cn))
                {
                    copy.BulkCopyTimeout = 3600 * 24;
                    for (int i = 0; i < dtSrc.Columns.Count; ++i)
                    {
                        copy.ColumnMappings.Add(i, i);
                    }
                    copy.DestinationTableName = dtDesc;
                    copy.WriteToServer(dtSrc);
                }
            }
        }
		#endregion

		#region "Transaction"
		/// <summary>
		/// 开始一个Transaction，新建一个连接并打开连接
		/// </summary>
		/// <returns></returns>
		public DbTransaction BeginTransaction()
		{
            DbConnection connection = m_db.CreateConnection();
			connection.Open();
			return connection.BeginTransaction();
		}

		/// <summary>
		/// RollbackTransaction
		/// </summary>
        /// <param name="txn"></param>
		public void RollbackTransaction(DbTransaction txn)
		{
            if (txn == null)
            {
                throw new ArgumentNullException("txn");
            }
            txn.Rollback();
		}

		/// <summary>
		/// CommitTransaction
		/// </summary>
        /// <param name="txn"></param>
        public void CommitTransaction(DbTransaction txn)
		{
            if (txn == null)
            {
                throw new ArgumentNullException("txn");
            }
            txn.Commit();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
		public delegate void SetCommand(MyDbCommand cmd);

        /// <summary>
        /// UpdateTransaction
        /// </summary>
        /// <param name="cmds"></param>
        /// <param name="setCommand"></param>
		public static void UpdateTransaction(ICollection<MyDbCommand> cmds, SetCommand setCommand)
		{
			//MyDbCommand cmdCurrent = null;

			foreach (MyDbCommand cmd in cmds)
			{
                try
                {
                    if (cmd == null || cmd.Command == null)
                    {
                        throw new ArgumentException("cmds has null Command", "cmds");
                    }
                    //cmdCurrent = cmd;

                    // Do at out delegate
                    setCommand(cmd);

                    object actualResult = null;
                    if (cmd.Command.CommandText.IndexOf("INSERT", StringComparison.Ordinal) != -1
                        || cmd.Command.CommandText.IndexOf("UPDATE", StringComparison.Ordinal) != -1
                        || cmd.Command.CommandText.IndexOf("DELETE", StringComparison.Ordinal) != -1)
                    {
                        actualResult = cmd.Command.ExecuteNonQuery();
                    }
                    else if (cmd.Command.CommandText.IndexOf("SELECT", StringComparison.Ordinal) != -1)
                    {
                        actualResult = cmd.Command.ExecuteScalar();
                    }
                    else
                    {
                        throw new NotSupportedException("Invalid Sql Command Format. Now only INSERT,UPDATE,DELETE,SELECT Supported!");
                    }

                    switch (cmd.ExpectedResultType)
                    {
                        case ExpectedResultTypes.Any:
                            break;
                        case ExpectedResultTypes.GreaterThanOrEqualZero:
                            if ((int)actualResult < 0)
                            {
                                throw new DBConcurrencyException(cmd.ErrorMsg);
                            }
                            break;
                        case ExpectedResultTypes.GreaterThanZero:
                            if ((int)actualResult <= 0)
                            {
                                throw new DBConcurrencyException(cmd.ErrorMsg);
                            }
                            break;
                        case ExpectedResultTypes.Special:
                            if ((actualResult == null && cmd.ExpectedResult != null)
                                || (actualResult != null && cmd.ExpectedResult == null))
                            {
                                throw new DBConcurrencyException(cmd.ErrorMsg);
                            }

                            if (actualResult.ToString() != cmd.ExpectedResult.ToString())
                            {
                                throw new DBConcurrencyException(cmd.ErrorMsg);
                            }
                            break;
                        default:
                            throw new NotSupportedException("Invalid MyDbCommand ExpectedType!");
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(cmd.Command.CommandText + " 执行错误", ex);
                }
			}
		}

		

		/// <summary>
		/// 执行一系列命令，给定Transaction
		/// 在外部开始Transaction，外部处理Exception
		/// </summary>
		/// <param name="cmds"></param>
		/// <param name="txn"></param>
		public static void UpdateTransaction(ICollection<MyDbCommand> cmds, DbTransaction txn)
		{
			if (cmds == null)
			{
				throw new ArgumentNullException("cmds");
			}
			if (cmds.Count == 0)
			{
				return;
			}
			if (txn == null)
			{
				throw new ArgumentNullException("txn");
			}
			
			UpdateTransaction(cmds, new SetCommand(delegate(MyDbCommand cmd)
			{
				cmd.Command.Transaction = txn;
				cmd.Command.Connection = txn.Connection;
			}));
		}

		/// <summary>
		/// 批量执行一组Sql命令,并作为一个TransAction
		/// </summary>
		/// <param name="cmds">要执行的Sql命令组，以MyDbCommand类型存储</param>
		/// <exception cref="Exception">Throw if there is some error in sql operation</exception>
		public void UpdateTransaction(ICollection<MyDbCommand> cmds)
		{
			if (cmds == null)
			{
				throw new ArgumentNullException("cmds");
			}
			if (cmds.Count == 0)
			{
				return;
			}

			DbTransaction txn = null;
			try
			{
				txn = BeginTransaction();
				UpdateTransaction(cmds, txn);
				CommitTransaction(txn);
			}
			catch (Exception)
			{
				RollbackTransaction(txn);
                throw;
			}
		}

		/// <summary>
		/// 执行命令，检查是否符合给定要求
        /// 如发生<see cref="DBConcurrencyException"/>, return false
		/// </summary>
		/// <param name="cmd">Command</param>
		public bool ExecuteMyCommand(MyDbCommand cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			IList<MyDbCommand> cmds = new List<MyDbCommand>();
			cmds.Add(cmd);

			DbTransaction txn = null;
			try
			{
				txn = BeginTransaction();
				UpdateTransaction(cmds, txn);
				CommitTransaction(txn);
			}
			catch (DBConcurrencyException)
			{
                RollbackTransaction(txn);
				return false;
			}
			catch (Exception)
			{
                RollbackTransaction(txn);
                throw;
			}

			return true;
		}
		#endregion

		#region "#Utils"
        /// <summary>
        /// CreateCommand
        /// </summary>
        /// <returns></returns>
        public DbCommand CreateCommand()
        {
            var p = DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            return p;
        }
        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string parameterName, object value)
        {
            var p = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = value;
            return p;
        }

        /// <summary>
        /// SchemaTable
        /// ColumnName, ColumnOrdinal, ColumnSize, NumbericPrecision, NumbericScale, IsUnique,
        /// DataType, AllowDBNull, ProviderTyper, IsIdentity, IsAutoIncrement, IsLong, IsReadOnly,
        /// ProviderSpecificDataType, DataTypeName, 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetSchema(string tableName)
        {
            string query = String.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT * FROM {0}", tableName);
            DbCommand cmd = m_db.GetSqlStringCommand(query);
            DataTable schema;
            using (IDataReader reader = m_db.ExecuteReader(cmd))
            {
                schema = reader.GetSchemaTable();
            }

            return schema;
        }
        /// <summary>
        /// 获取数据库中定义的View，Function，Procedure，Trigger的定义
        /// 列有object_id, name, definition, type(View, Function, Procedure, Trigger)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetViewFuncProcTrigs()
        {
            string sql = "SELECT o.[object_id], o.name, m.definition, " + 
                " CASE WHEN OBJECTPROPERTY(o.[object_id],'IsView') = 1 THEN 'View'" +
                     " WHEN OBJECTPROPERTY(o.[object_id],'IsScalarFunction') = 1 " + 
                        " OR OBJECTPROPERTY(o.[object_id],'IsTableFunction') = 1 THEN 'Function'" + 
                     " WHEN OBJECTPROPERTY(o.[object_id],'IsProcedure') = 1 THEN 'Procedure'" + 
                     " WHEN OBJECTPROPERTY(o.[object_id],'IsTrigge') = 1 THEN 'Trigger'" + 
                " END as type" +
                " FROM sys.objects AS o JOIN sys.sql_modules AS m ON o.[object_id]=m.[object_id]" + 
                " WHERE OBJECTPROPERTY(o.[object_id],'IsEncrypted') = 0 AND OBJECTPROPERTY(o.[object_id],'IsExecuted') = 1" + 
                    " AND o.is_ms_shipped = 0" + 
                " ORDER BY type";
            return Instance.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetSqlObjectDefinition(string name)
        {
            string sql = "SELECT m.definition FROM sys.objects AS o JOIN sys.sql_modules AS m ON o.[object_id]=m.[object_id] " +
                " WHERE o.name = '" + name + "'";
            object r = Instance.ExecuteScalar(sql);
            if (r == null)
                return null;
            else
                return (string)r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetUserDataTables()
        {
            string sql = "SELECT name FROM sys.objects WHERE type = 'U' AND name not like 'AD_%'";
            DataTable dt = Instance.ExecuteDataTable(sql);
            string[] ret = new string[dt.Rows.Count];
            for (int i = 0; i < ret.Length; ++i)
            {
                ret[i] = (string)dt.Rows[i][0];
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetAdDataTables()
        {
            string sql = "SELECT name FROM sys.objects WHERE type = 'U' AND name like 'AD_%'";
            DataTable dt = Instance.ExecuteDataTable(sql);
            string[] ret = new string[dt.Rows.Count];
            for (int i = 0; i < ret.Length; ++i)
            {
                ret[i] = (string)dt.Rows[i][0];
            }
            return ret;
        }

        /// <summary>
        /// 执行整批Sql命令
        /// </summary>
        /// <param name="scriptFilePath"></param>
        public static void ExecuteSqlScript(string scriptFilePath)
        {
            string delimiter = "^go";
            StreamReader scriptFileStreamReader = new StreamReader(scriptFilePath);
            string completeScript = scriptFileStreamReader.ReadToEnd();

            Database databse = DbHelper.Instance.m_db;

            DbConnection connection = databse.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                IDbCommand cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                string[] sqlCommands = Regex.Split(completeScript, delimiter,
                                                   RegexOptions.IgnoreCase | RegexOptions.Multiline);
                foreach (string sqlCommand in sqlCommands)
                {
                    if (sqlCommand.Trim().Length > 0)
                    {
                        cmd.CommandText = sqlCommand;
                        cmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                connection.Close();
                scriptFileStreamReader.Close();
            }
        }

        /// <summary>
        /// 把数据表中数据以Sql语句形式导出
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tableName"></param>
        public static void BackupDbTable(string fileName, string tableName)
        {
            DataTable dt = DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + tableName);
            StringBuilder prefix = new StringBuilder();
            prefix.Append("INSERT INTO ");
            prefix.Append(tableName + " (");
            
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, true))
            {
                StringBuilder sb = new StringBuilder();
                foreach(DataRow row in dt.Rows)
                {
                    sb.Remove(0, sb.Length);
                    sb.Append(prefix);

                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("[" + column.ColumnName + "],");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(") VALUES (");
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append(row[column.ColumnName] == DBNull.Value ? "NULL" : "'" + row[column.ColumnName].ToString() + "'");
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    sb.Append(Environment.NewLine);

                    sw.WriteLine(sb.ToString());
                }
            }
        }

		#endregion
	};
}
