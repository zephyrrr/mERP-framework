using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Utils
{
    public static class ConfigurationHelper
    {
        public static string GetDefaultServerName()
        {
            var c = System.Configuration.ConfigurationManager.ConnectionStrings[SecurityHelper.DataConnectionStringName];
            if (c != null)
                return GetServerName(c.ConnectionString);
            else
                return null;
        }

        /// <summary>
        /// Get Server Name from .config
        /// </summary>
        /// <returns></returns>
        public static string GetServerName(string connectionString)
        {
            string serverName = null;
            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.ConnectionString = connectionString;

                serverName = builder.DataSource;
                int idx = serverName.IndexOf(',');
                if (idx != -1)
                {
                    serverName = serverName.Substring(0, idx);
                }
                serverName = serverName.Trim();
            }
            catch (Exception)
            {
            }
            return serverName;
        }

        public static string GetDefaultServerDatabaseName()
        {
            var c = System.Configuration.ConfigurationManager.ConnectionStrings[SecurityHelper.DataConnectionStringName];
            if (c != null)
                return GetServerDatabaseName(c.ConnectionString);
            else
                return null;
        }

        /// <summary>
        /// GetServerDatabaseName
        /// </summary>
        /// <returns></returns>
        public static string GetServerDatabaseName(string connectionString)
        {
            string serverName = string.Empty;
            string dbName = string.Empty;
            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.ConnectionString = connectionString;

                serverName = builder.DataSource;
                dbName = builder.InitialCatalog;
            }
            catch (Exception)
            {
            }
            return serverName + ";" + dbName;
        }
    }
}
