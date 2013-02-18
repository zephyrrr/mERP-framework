using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public class SQLiteGoogleMapChinaOffsetCache : PureGoogleMapChinaOffsetCache
    {
        private string cache;
        private string ConnectionString;
        private bool Created;
        private string db;
        private string gtileCache;
        private static readonly string sqlSelect = "SELECT [dlat], [dlon] FROM [KeyOffset] WHERE Id={0}";
        private static readonly string sqlCreate = "CREATE TABLE IF NOT EXISTS KeyOffset (Id INTEGER PRIMARY KEY NOT NULL, dlat INTEGER NOT NULL, dlon INTEGER NOT NULL);";
        private static readonly string sqlInsert = "INSERT INTO KeyOffset([Id], [dlat], [dlon] ) VALUES(@Id, @dlat, @dlon)";

        public static bool CreateEmptyDB(string file)
        {
            bool flag = true;
            try
            {
                string directoryName = Path.GetDirectoryName(file);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = string.Format("Data Source=\"{0}\";FailIfMissing=False;Page Size=32768;Pooling=True", file);
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        using (DbCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = sqlCreate;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        flag = false;
                    }
                    finally
                    {
                        if (transaction != null)
                        {
                            transaction.Dispose();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public GMap.NET.GPoint? GetOffsetFromCache(int key)
        {
            GMap.NET.GPoint? ret = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = this.ConnectionString;
                    connection.Open();
                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format(sqlSelect, new object[] { key });
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (reader.Read())
                            {
                                ret = new GMap.NET.GPoint(reader.GetInt32(1), reader.GetInt32(0));
                            }
                            reader.Close();
                        }
                    }
                    connection.Close();
                }
            }
            catch (System.Data.SQLite.SQLiteException)
            {
            }
            catch (Exception)
            {
            }
            return ret;
        }

        public bool PutOffsetToCache(int key, GMap.NET.GPoint offset)
        {
            bool flag = false;
            if (this.Created)
            {
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = this.ConnectionString;
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        using (DbCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = sqlInsert;
                            command.Parameters.Add(new SQLiteParameter("@Id", key));
                            command.Parameters.Add(new SQLiteParameter("@dlat", offset.Y));
                            command.Parameters.Add(new SQLiteParameter("@dlon", offset.X));
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        flag = true;
                    }
                    catch (System.Data.SQLite.SQLiteException)
                    {
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                    finally
                    {
                        if (transaction != null)
                        {
                            transaction.Dispose();
                        }
                    }
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool VacuumDb(string file)
        {
            bool flag = true;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = string.Format("Data Source=\"{0}\";FailIfMissing=True;Page Size=32768;Pooling=True", file);
                    connection.Open();
                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "vacuum;";
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public string CacheLocation
        {
            get
            {
                return this.cache;
            }
            set
            {
                this.cache = value;
                this.gtileCache = this.cache;
                string path = this.gtileCache;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                this.db = path + "OffsetDb.gmdb";
                if (!File.Exists(this.db))
                {
                    this.Created = CreateEmptyDB(this.db);
                }
                else
                {
                    this.Created = true;// AlterDBAddTimeColumn(this.db);
                }
                this.ConnectionString = string.Format("Data Source=\"{0}\";Page Size=32768;Pooling=True", this.db);
            }
        }
    }
}
