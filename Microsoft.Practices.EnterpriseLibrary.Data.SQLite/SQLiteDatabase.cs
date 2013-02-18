//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.SQLite.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SQLite
{
	/// <summary>
	///		Provides helper methods to make working with a Sql Server Compact Edition database
	///		easier.
	/// </summary>
	/// <remarks>
	///		<para>
	///			Because Sql Server CE has no connection pooling and the cost of opening the initial
	///			connection is high, this class implements a simple connection pool.
	///		</para>
	///		<para>
	///			Sql Server CE requires full trust to run, so it cannot be used in partial trust
	///			environments.
	///		</para>
	/// </remarks>
    [ConfigurationElementType(typeof(SQLiteDatabaseData))]
	public class SQLiteDatabase : Database
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="SQLiteDatabase"/> class with a connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public SQLiteDatabase(string connectionString)
            : base(connectionString, SQLiteFactory.Instance)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteDatabase"/> class with a connection string
        /// and an instrumentation provider.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="instrumentationProvider">The instrumentation provider.</param>
        public SQLiteDatabase(string connectionString, IDataInstrumentationProvider instrumentationProvider)
            : base(connectionString, SQLiteFactory.Instance, instrumentationProvider)
        {
            
        }

        ///// <summary>
        /////		This will close the "keep alive" connection that is kept open once you first access
        /////		the database through this class. Calling this method will close the "keep alive"
        /////		connection for all instances. The next database access will open a new "keep alive"
        /////		connection.
        ///// </summary>
        //public void CloseSharedConnection()
        //{
        //    SQLiteConnectionPool.CloseSharedConnection(this);
        //}

        ///// <summary>
        /////		<para>Creates a connection for this database.</para>
        ///// </summary>
        ///// <remarks>
        /////		This method has been overridden to support keeping a single connection open until you
        /////		explicitly close it with a call to <see cref="CloseSharedConnection"/>.
        ///// </remarks>
        ///// <returns>
        /////		<para>The <see cref="DbConnection"/> for this database.</para>
        ///// </returns>
        ///// <seealso cref="DbConnection"/>        
        //public override DbConnection CreateConnection()
        //{
        //    using (DatabaseConnectionWrapper wrapper = SQLiteConnectionPool.CreateConnection(this))
        //    {
        //        wrapper.AddRef();
        //        wrapper.Connection.ConnectionString = ConnectionString;
        //        return wrapper.Connection;
        //    }
        //}

        ///// <summary>
        ///// Gets a "wrapped" connection for use outside a transaction.
        ///// </summary>
        ///// <returns>The wrapped connection.</returns>
        //protected override DatabaseConnectionWrapper GetWrappedConnection()
        //{
        //    return SQLiteConnectionPool.CreateConnection(this, true);
        //}

	    /// <summary>
		///		Don't need an implementation for Sql Server CE.
		/// </summary>
		/// <param name="discoveryCommand"></param>
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
            throw new NotImplementedException("The method or operation is not implemented.");
		}

        //internal void SetConnectionString(DbConnection connection)
        //{
        //    connection.ConnectionString = ConnectionString;
        //}


		/// <summary>
		/// Builds a value parameter name for the current database.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <returns>A correctly formated parameter name.</returns>
		public override string BuildParameterName(string name)
		{
			if (name[0] != ParameterToken)
			{
				return name.Insert(0, new string(ParameterToken, 1));
			}
			return name;
		}

		/// <summary>
		/// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
		/// </summary>
		/// <value>
		/// <para>The '@' symbol.</para>
		/// </value>
		protected char ParameterToken
		{
			get { return '@'; }
		}
	}
}
