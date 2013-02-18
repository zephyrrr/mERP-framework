using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace Feng.Map.Obsolete
{
    internal class SQLiteGoogleMapChinaOffsetCache2 : PureGoogleMapChinaOffsetCache2
    {
        private string cache;
        private string ConnectionString;
        private bool Created;
        private string db;
        private string gtileCache;
        private static readonly string sqlSelect = "SELECT [OffsetX], [OffsetY] FROM [GoogleMapOffsetCache] WHERE X={0} AND Y={1} AND Zoom={2}";
        private static readonly string sqlCreate = "CREATE TABLE IF NOT EXISTS GoogleMapOffsetCache (X INTEGER NOT NULL, Y INTEGER NOT NULL, Zoom INTEGER NOT NULL, OffsetX INTEGER NOT NULL, OffsetY INTEGER NOT NULL); CREATE INDEX IF NOT EXISTS IndexOfGoogleMapOffsetCache ON GoogleMapOffsetCache (X, Y, Zoom);";

        //private static bool AlterDBAddTimeColumn(string file)
        //{
        //    bool flag = true;
        //    try
        //    {
        //        if (File.Exists(file))
        //        {
        //            using (SQLiteConnection connection = new SQLiteConnection())
        //            {
        //                connection.ConnectionString = string.Format("Data Source=\"{0}\";FailIfMissing=False;Page Size=32768;Pooling=True", file);
        //                connection.Open();
        //                using (DbTransaction transaction = connection.BeginTransaction())
        //                {
        //                    bool? nullable = null;
        //                    try
        //                    {
        //                        using (DbCommand command = new SQLiteCommand("SELECT CacheTime FROM GoogleMapOffsetCache", connection))
        //                        {
        //                            command.Transaction = transaction;
        //                            using (DbDataReader reader = command.ExecuteReader())
        //                            {
        //                                reader.Close();
        //                            }
        //                            nullable = false;
        //                        }
        //                    }
        //                    catch (Exception exception)
        //                    {
        //                        if (!exception.Message.Contains("no such column: CacheTime"))
        //                        {
        //                            throw exception;
        //                        }
        //                        nullable = true;
        //                    }
        //                    try
        //                    {
        //                        if (nullable.HasValue && nullable.Value)
        //                        {
        //                            using (DbCommand command2 = connection.CreateCommand())
        //                            {
        //                                command2.Transaction = transaction;
        //                                command2.CommandText = "ALTER TABLE Tiles ADD CacheTime DATETIME";
        //                                command2.ExecuteNonQuery();
        //                            }
        //                            transaction.Commit();
        //                            nullable = false;
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        transaction.Rollback();
        //                        flag = false;
        //                    }
        //                }
        //                connection.Close();
        //                return flag;
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

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

        //public static bool ExportMapDataToDB(string sourceFile, string destFile)
        //{
        //    bool flag = true;
        //    try
        //    {
        //        if (!File.Exists(destFile))
        //        {
        //            flag = CreateEmptyDB(destFile);
        //        }
        //        if (!flag)
        //        {
        //            return flag;
        //        }
        //        using (SQLiteConnection connection = new SQLiteConnection())
        //        {
        //            connection.ConnectionString = string.Format("Data Source=\"{0}\";Page Size=32768;Pooling=True", sourceFile);
        //            connection.Open();
        //            if (connection.State == ConnectionState.Open)
        //            {
        //                using (SQLiteConnection connection2 = new SQLiteConnection())
        //                {
        //                    connection2.ConnectionString = string.Format("Data Source=\"{0}\";Page Size=32768;Pooling=True", destFile);
        //                    connection2.Open();
        //                    if (connection2.State == ConnectionState.Open)
        //                    {
        //                        using (SQLiteCommand command = new SQLiteCommand(string.Format("ATTACH DATABASE \"{0}\" AS Source", sourceFile), connection2))
        //                        {
        //                            command.ExecuteNonQuery();
        //                        }
        //                        using (SQLiteTransaction transaction = connection2.BeginTransaction())
        //                        {
        //                            try
        //                            {
        //                                List<long> list = new List<long>();
        //                                using (SQLiteCommand command2 = new SQLiteCommand("SELECT id, X, Y, Zoom, Type FROM Tiles;", connection))
        //                                {
        //                                    using (SQLiteDataReader reader = command2.ExecuteReader())
        //                                    {
        //                                        while (reader.Read())
        //                                        {
        //                                            long item = reader.GetInt64(0);
        //                                            using (SQLiteCommand command3 = new SQLiteCommand(string.Format("SELECT id FROM Tiles WHERE X={0} AND Y={1} AND Zoom={2} AND Type={3};", new object[] { reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4) }), connection2))
        //                                            {
        //                                                using (SQLiteDataReader reader2 = command3.ExecuteReader())
        //                                                {
        //                                                    if (!reader2.Read())
        //                                                    {
        //                                                        list.Add(item);
        //                                                    }
        //                                                }
        //                                                continue;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                foreach (long num2 in list)
        //                                {
        //                                    using (SQLiteCommand command4 = new SQLiteCommand(string.Format("INSERT INTO Tiles(X, Y, Zoom, Type) SELECT X, Y, Zoom, Type FROM Source.Tiles WHERE id={0}; INSERT INTO TilesData(id, Tile) Values((SELECT last_insert_rowid()), (SELECT Tile FROM Source.TilesData WHERE id={0}));", num2), connection2))
        //                                    {
        //                                        command4.Transaction = transaction;
        //                                        command4.ExecuteNonQuery();
        //                                        continue;
        //                                    }
        //                                }
        //                                list.Clear();
        //                                transaction.Commit();
        //                            }
        //                            catch
        //                            {
        //                                transaction.Rollback();
        //                                flag = false;
        //                            }
        //                            return flag;
        //                        }
        //                    }
        //                    return flag;
        //                }
        //            }
        //            return flag;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        flag = false;
        //    }
        //    return flag;
        //}

        public System.Drawing.Size? GetOffsetFromCache(GMap.NET.GPoint pos, int zoom)
        {
            System.Drawing.Size? ret = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = this.ConnectionString;
                    connection.Open();
                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format(sqlSelect, new object[] { pos.X, pos.Y, zoom});
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (reader.Read())
                            {
                                ret = new System.Drawing.Size(reader.GetInt32(0), reader.GetInt32(1));
                            }
                            reader.Close();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                ret = null;
            }
            return ret;
        }

        public bool PutOffsetToCache(GMap.NET.GPoint pos, int zoom, System.Drawing.Size offset)
        {
            bool flag = true;
            if (this.Created)
            {
                try
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
                                command.CommandText = "INSERT INTO GoogleMapOffsetCache([X], [Y], [Zoom], [OffsetX], [OffsetY] ) VALUES(@x, @y, @zoom, @offsetx, @offsety)";
                                command.Parameters.Add(new SQLiteParameter("@x", pos.X));
                                command.Parameters.Add(new SQLiteParameter("@y", pos.Y));
                                command.Parameters.Add(new SQLiteParameter("@zoom", zoom));
                                command.Parameters.Add(new SQLiteParameter("@offsetx", offset.Width));
                                command.Parameters.Add(new SQLiteParameter("@offsety", offset.Height));
                                command.ExecuteNonQuery();
                            }
                            //using (DbCommand command2 = connection.CreateCommand())
                            //{
                            //    command2.Transaction = transaction;
                            //    command2.CommandText = "INSERT INTO TilesData(id, Tile) VALUES((SELECT last_insert_rowid()), @p1)";
                            //    command2.Parameters.Add(new SQLiteParameter("@p1", tile.GetBuffer()));
                            //    command2.ExecuteNonQuery();
                            //}
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

        //public string GtileCache
        //{
        //    get
        //    {
        //        return this.gtileCache;
        //    }
        //}
    }
}

