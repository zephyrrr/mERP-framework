using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Feng.Data;

namespace Feng
{
    /// <summary>
    /// NameValueMapping的集合
    /// </summary>
    public sealed class NameValueMappingCollection : Singleton<NameValueMappingCollection>, IDisposable, IEnumerable<NameValueMapping>
    {
        #region "Constructor"

        /// <summary>
        /// Constructor
        /// </summary>
        public NameValueMappingCollection()
        {
        }

        private DataSet DefaultDataSet
        {
            get 
            {
                if (!m_dataSets.ContainsKey("Default"))
                {
                    m_dataSets["Default"] = new DataSet();
                }
                return m_dataSets["Default"]; 
            }
        }

        private DataSet GetDataSet(string dsName)
        {
            if (string.IsNullOrEmpty(dsName))
                return DefaultDataSet;
            if (!m_dataSets.ContainsKey(dsName))
                return DefaultDataSet;
            return m_dataSets[dsName];
        }

        /// <summary>
        /// Dispose RadioButton and remove it from parent's controls
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~NameValueMappingCollection()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var i in m_dataSets.Values)
                {
                    i.Dispose();
                }
                m_dataSets.Clear();
                m_dataSets = null;
            }
        }

        #endregion

        //#region "Special Nv"
        //private static Dictionary<string, NameValueMappingCollection> s_specialNameValueMappingCollections;
        ///// <summary>
        ///// 不止一个全局的NameValueMappingCollection，可以有各个不同名字的NameValueMappingCollection
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static NameValueMappingCollection GetSpecialNameValueMappingCollections(string name)
        //{
        //    if (s_specialNameValueMappingCollections == null)
        //    {
        //        s_specialNameValueMappingCollections = new Dictionary<string, NameValueMappingCollection>();
        //    }
        //    if (!s_specialNameValueMappingCollections.ContainsKey(name))
        //    {
        //        s_specialNameValueMappingCollections[name] = new NameValueMappingCollection();
        //    }
        //    return s_specialNameValueMappingCollections[name];
        //}
        //#endregion

        #region "Collections & Enum"

        /// <summary>
        /// 加入一个NameValueMapping
        /// </summary>
        /// <param name="nv">NameValueMapping</param>
        public void Add(NameValueMapping nv)
        {
            if (nv == null)
            {
                throw new ArgumentNullException("nv");
            }

            if (m_mappings.ContainsKey(nv.Name))
            {
                return;
            }

            m_mappings[nv.Name] = nv;
        }

        private const string s_enumFlag = "EnumType@";

        /// <summary>
        /// Type转换到保存于此处的名字
        /// </summary>
        /// <param name="enumType">类型（必须为Enum类型）</param>
        /// <param name="notUseEnum">是否使用Enum（如果为true，则用序号）</param>
        /// <returns></returns>
        private static string TypeToNameValueMappingName(Type enumType, bool notUseEnum)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType should be enum, not " + enumType, "enumType");
            }
            string s = s_enumFlag + enumType.ToString().Replace('.', '@');
            if (notUseEnum)
            {
                s += "@NotUseEnum";
            }
            return s;
        }

        /// <summary>
        /// Type转换到保存于此处的名字
        /// </summary>
        /// <param name="type">类型（必须为Enum类型）</param>
        /// <returns></returns>
        private static string TypeToNameValueMappingName(Type type)
        {
            return TypeToNameValueMappingName(type, false);
        }

        /// <summary>
        /// 加入一个NameValueMapping
        /// </summary>
        /// <param name="type">类型（必须为Enum类型）</param>
        /// <returns></returns>
        public string Add(Type type)
        {
            return Add(type, false);
        }

        /// <summary>
        /// 加入一个NameValueMapping
        /// </summary>
        /// <param name="enumType">类型（必须为Enum类型）</param>
        /// <param name="notUseEnum">是否使用Enum（如果为true，则ValueMember用序号，如果为False，则ValueMember用Enum）</param>
        /// <returns></returns>
        public string Add(Type enumType, bool notUseEnum)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType should be enum, not " + enumType, "enumType");
            }

            string nvName = TypeToNameValueMappingName(enumType, notUseEnum);
            if (m_mappings.ContainsKey(nvName))
            {
                return nvName;
            }

            NameValueMapping nv = new NameValueMapping(nvName, nvName,
                new string[] { EnumNameValue.IndexName, EnumNameValue.DescriptionName, EnumNameValue.ValueName, EnumNameValue.ValueTypeName});

            if (notUseEnum)
            {
                nv.ValueMember = EnumNameValue.IndexName;
                nv.DisplayMember = EnumNameValue.DescriptionName;
                nv.MemberVisible[EnumNameValue.ValueName] = false;
            }
            else
            {
                nv.ValueMember = EnumNameValue.ValueName;
                nv.DisplayMember = EnumNameValue.DescriptionName;
                nv.MemberVisible[EnumNameValue.ValueName] = false;
            }

            Add(nv);
            if (!m_dataSources.ContainsKey(nvName))
            {
                IList<EnumNameValue> list = Feng.Utils.EnumHelper.EnumToList(enumType);
                DataTable dt = new DataTable();
                dt.TableName = nvName;
                dt.Columns.Add(EnumNameValue.IndexName, typeof(int));
                dt.Columns.Add(EnumNameValue.ValueName, typeof(object));
                dt.Columns.Add(EnumNameValue.DescriptionName, typeof(string));
                dt.Columns.Add(EnumNameValue.ValueTypeName, typeof(Type));

                foreach (EnumNameValue e in list)
                {
                    System.Data.DataRow row = dt.NewRow();
                    row[EnumNameValue.IndexName] = e.Index;
                    row[EnumNameValue.ValueName] = e.Value;
                    row[EnumNameValue.DescriptionName] = e.Description;
                    row[EnumNameValue.ValueTypeName] = e.ValueType;
                    dt.Rows.Add(row);
                }
                m_dataSources[nvName] = dt.DefaultView;
                
                this.DefaultDataSet.Tables.Add(dt);
            }

            return nvName;
        }

        #endregion

        #region "Datasource"

        /// <summary>
        /// 清空全部
        /// </summary>
        public void Clear()
        {
            m_dataSources.Clear();
            foreach (var i in m_dataSets.Values)
            {
                i.Tables.Clear();
                i.Clear();
            }
            m_dataSets.Clear();
            m_mappings.Clear();
        }

        private IDictionary<string, DataSet> m_dataSets = new System.Collections.Concurrent.ConcurrentDictionary<string, DataSet>();  // 从数据库读入的DataTables
        private IDictionary<string, DataView> m_dataSources = new System.Collections.Concurrent.ConcurrentDictionary<string, DataView>();  // 以FilterName为Key
        private IDictionary<string, NameValueMapping> m_mappings = new System.Collections.Concurrent.ConcurrentDictionary<string, NameValueMapping>();   // 以NvName为Key
        private IDictionary<string, bool> m_loadedTables = new System.Collections.Concurrent.ConcurrentDictionary<string, bool>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nvName"></param>
        /// <returns></returns>
        public DataView GetDataSource(string nvName)
        {
            return GetDataSource(nvName, null);
        }

        /// <summary>
        /// GetDataSource
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataView GetDataSource(string nvName, string filter)
        {
            return GetDataSource(null, nvName, filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataView GetDataSource(string dsName, string nvName, string filter)
        {
            string newName = GetDataSourceName(dsName, nvName, filter);
            if (m_dataSources.ContainsKey(newName))
                return m_dataSources[newName];
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nvName"></param>
        /// <returns></returns>
        public string GetDataSourceName(string nvName)
        {
            return GetDataSourceName(nvName, null);
        }

        /// <summary>
        /// 取得DataSet中的某个DataTable
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string GetDataSourceName(string nvName, string filter)
        {
            return GetDataSourceName(null, nvName, filter);
        }

        /// <summary>
        /// 取得DataSet中的某个DataTable(可以有filter)
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string GetDataSourceName(string dsName, string nvName, string filter)
        {
            if (!this[nvName].IsDynamic)
                dsName = null;

            string newName = GetFilteredName(nvName, filter);
            if (!m_dataSources.ContainsKey(newName))
            {
                string[] ss = newName.Split(new string[] { s_filterFlag }, StringSplitOptions.RemoveEmptyEntries);

                m_loadedTables.Clear();
                // 从数据库读入（只读入最高层的）
                LoadDataFromDB(dsName, ss[0], false);
                m_loadedTables.Clear();

                if (ss.Length >= 2)
                {
                    CreateFilterDataView(dsName, ss[0], ss[1], newName);
                }
                else
                {
                    // DefaultView
                    CreateFilterDataView(dsName, ss[0], string.Empty, newName);
                }
            }

            string newNameWithDs = string.IsNullOrEmpty(dsName) ? newName : dsName + "." + newName;
            return newNameWithDs;
        }

        private const string s_filterFlag = " FILTER ";

        private string GetFilteredName(string nvName, string filter)
        {
            NameValueMapping nv = this[nvName];
            if (string.IsNullOrEmpty(filter))
            {
                if (!string.IsNullOrEmpty(nv.ParentName))
                {
                    return GetFilteredName(nv.ParentName, nv.WhereQuery);
                }
                else
                {
                    return nvName;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(nv.ParentName))
                {
                    return GetFilteredName(nv.ParentName, nv.WhereQuery + " AND " + filter);
                }
                else
                {
                    string newName = nvName + s_filterFlag + filter;
                    if (!m_mappings.ContainsKey(newName))
                    {
                        NameValueMapping newNv = this[nvName].Clone() as NameValueMapping;
                        newNv.Name = newName;
                        newNv.ParentName = nvName;
                        newNv.WhereQuery = filter;
                        m_mappings[newName] = newNv;
                    }
                    return newName;
                }
            }
        }

        private void CreateFilterDataView(string dsName, string tableName, string filter, string newName)
        {
            DataSet ds = GetDataSet(dsName);
            DataTable dt = ds.Tables[tableName];
            if (dt == null)
                return;

            DataView dv;
            if (string.IsNullOrEmpty(filter))
            {
                dv = dt.DefaultView;
            }
            else
            {
                dv = new DataView(dt);
                dv.RowFilter = filter;
            }

            newName = string.IsNullOrEmpty(dsName) ? newName : dsName + "." + newName;
            m_dataSources[newName] = dv;
            //string newName = GetFilteredName(nvName, filter);
            //DataTable dt1 = DataTable(nvName);
            //DataTable dt2;
            //if (m_dataSet.Tables.Contains(newName))
            //{
            //    dt2 = m_dataSet.Tables[newName];
            //    dt2.Rows.Clear();
            //    dt2.Load(dt1.CreateDataReader());
            //}
            //else
            //{
            //    dt2 = dt1.Copy();
            //    dt2.TableName = newName;
            //    m_dataSet.Tables.Add(dt2);
            //}

            ////  m_dataSet.Tables[newName].Rows.Clear();
            //DataView dv = dt2.DefaultView;
            //dv.RowFilter = filter;

            //Dictionary<DataRow, bool> existRows = new Dictionary<DataRow, bool>();
            //foreach (DataRowView rowView in dv)
            //{
            //    existRows[rowView.Row] = true;
            //}
            //List<DataRow> deletedRows = new List<DataRow>();
            //foreach (DataRow row in dt2.Rows)
            //{
            //    if (!existRows.ContainsKey(row))
            //    {
            //        deletedRows.Add(row);
            //    }
            //}
            //foreach (DataRow row in deletedRows)
            //{
            //    dt2.Rows.Remove(row);
            //}

            //// too slow
            ////for (int i = 0; i < dv.Count; i++)
            ////{
            ////    dt2.ImportRow(dv[i].Row);
            ////}

            //// too slow to
            ////int columnLength = dt1.Columns.Count;
            ////foreach (DataRowView view in dv)
            ////{
            ////    object[] objectArray = new object[dt1.Columns.Count];
            ////    for (int i = 0; i < objectArray.Length; i++)
            ////    {
            ////        objectArray[i] = view[i];
            ////    }

            ////    dt2.Rows.Add(objectArray);
            ////}

            //// two different table which will not cause dataSource refresh
            ////dt2 = dv.ToTable(newName);
            ////if (m_dataSet.Tables.Contains(newName))
            ////{
            ////    m_dataSet.Tables.Remove(newName);
            ////}
            ////m_dataSet.Tables.Add(dt2);

            //// 如果我们不设置回""，DataTable里的Row也会按照RowFilter设置Visible
            //dv.RowFilter = string.Empty;
            //return newName;
        }

        
        /// <summary>
        /// 全部更新
        /// </summary>
        public void Reload()
        {
            List<string> nvNames = new List<string>();
            foreach (KeyValuePair<string, DataSet> kvp in m_dataSets)
            {
                m_loadedTables.Clear();
                nvNames.Clear();
                foreach (DataTable dt in kvp.Value.Tables)
                {
                    // Not a Enum Type
                    if (!dt.TableName.StartsWith(s_enumFlag, StringComparison.Ordinal))
                    {
                        nvNames.Add(dt.TableName);
                    }
                }

                foreach (string nvName in nvNames)
                {
                    // TableName = nvName
                    LoadDataFromDB(kvp.Key, nvName, true);
                }

                m_loadedTables.Clear();
            }
        }

        /// <summary>
        /// 更新某个Nv
        /// </summary>
        /// <param name="nvName"></param>
        public void Reload(string dsName, string nvName)
        {
            string topName = FindTopParentNv(nvName);
            if (!this[topName].IsDynamic)
            {
                dsName = null;
            }
            m_loadedTables.Clear();

            LoadDataFromDB(dsName, topName, true);

            m_loadedTables.Clear();
        }

        ///// <summary>
        ///// 更新某个Nv
        ///// </summary>
        ///// <param name="nvName"></param>
        //public void Reload(string nvName)
        //{
        //    string topName = FindTopParentNv(nvName);
        //    if (m_mappings[topName].Params.Count == 0)
        //    {
        //        Reload(null, nvName);
        //    }
        //    else
        //    {
        //        foreach (string dsName in m_dataSets.Keys)
        //        {
        //            m_loadedTables.Clear();
        //            LoadDataFromDB(dsName, topName);
        //            m_loadedTables.Clear();
        //        }
        //    }
        //}

        /// <summary>
        /// FindTopParentNv
        /// </summary>
        /// <param name="nvName"></param>
        /// <returns></returns>
        public string FindTopParentNv(string nvName)
        {
            while (!string.IsNullOrEmpty(this[nvName].ParentName))
            {
                nvName = this[nvName].ParentName;
            }
            return nvName;
        }

        //private void LoadSibling(string nvName)
        //{
        //    if (!string.IsNullOrEmpty(this[nvName].ParentName))
        //    {
        //        foreach (NameValueMapping nv in this)
        //        {
        //            if (nv.Name != nvName && this[nvName].ParentName == nv.ParentName)
        //            {
        //                LoadInternal(nv.Name);
        //            }
        //        }
        //    }
        //}

        //private void LoadChild(string nvName)
        //{
        //    foreach (NameValueMapping nv in this)
        //    {
        //        if (nv.ParentName == nvName)
        //        {
        //            LoadInternal(nv.Name);
        //            LoadChild(nv.Name);
        //        }
        //    }
        //}

        //private void LoadParent(string nvName)
        //{
        //    if (!string.IsNullOrEmpty(this[nvName].ParentName))
        //    {
        //        foreach (NameValueMapping nv in this)
        //        {
        //            if (nv.Name == this[nvName].ParentName)
        //            {
        //                LoadInternal(nv.Name);

        //                LoadParent(nv.Name);
        //            }
        //        }
        //    }
        //}

        private string GetCacheKey(string nvName)
        {
            return string.Format("NameValueMappingCollection, {0}", nvName);
        }

        /// <summary>
        /// 从数据库读入数据（如果已经存在，则刷新）
        /// </summary>
        /// <param name="reload"></param>
        /// <param name="nvName"></param>
        /// <param name="dsName"></param>
        private void LoadDataFromDB(string dsName, string nvName, bool reload)
        {
            lock (this)
            {
                if (string.IsNullOrEmpty(nvName))
                {
                    throw new ArgumentNullException("nvName");
                }

                NameValueMapping nv = this[nvName];

                //string newName = nvName;
                if (!string.IsNullOrEmpty(nv.ParentName))
                {
                    //newName = GetFilteredName(nv.ParentName, nv.WhereQuery);
                    throw new ArgumentException("In LoadDataFromDB nv's Parent Must be null!", "nv");
                }
                if (nvName.Contains(s_filterFlag))
                {
                    throw new ArgumentException("In LoadDataFromDB nv must not contain s_filterFlag!", "nv");
                }

                if (!m_loadedTables.ContainsKey(nvName))
                {
                    if (nv.Name.StartsWith(s_enumFlag, StringComparison.Ordinal))
                    {
                        return;
                    }

                    DataSet ds = null;
                    if (!string.IsNullOrEmpty(dsName))
                    {
                        //if (!reload)
                        //{
                        //    if (!m_dataSets.ContainsKey(dsName))
                        //    {
                        //        m_dataSets[dsName] = new DataSet();
                        //    }
                        //}
                        if (!m_dataSets.ContainsKey(dsName))
                        {
                            m_dataSets[dsName] = new DataSet();
                        }
                        if (m_dataSets.ContainsKey(dsName))
                        {
                            ds = m_dataSets[dsName];
                        }
                    }
                    else
                    {
                        ds = this.DefaultDataSet;
                    }
                    if (ds == null)
                        return;

                    if (!reload && ds.Tables.Contains(nvName))
                    {
                        return;
                    }

                    System.ComponentModel.CancelEventArgs e = new System.ComponentModel.CancelEventArgs();
                    nv.OnDataSourceChanging(e);
                    if (e.Cancel)
                        return;

                    if (ds.Tables.Contains(nvName))
                    {
                        ds.Tables[nvName].Rows.Clear();
                    }
#if DEBUG
                    System.Data.DataTable dt = null;
                    if (nv.Params.Count == 0 && !reload)
                    {
                        dt = Cache.TryGetCache<DataTable>(GetCacheKey(nv.Name), new Func<DataTable>(delegate()
                        {
                            DbHelper db = null;
                            if (string.IsNullOrEmpty(nv.DatabaseName))
                            {
                                db = DbHelper.Instance;
                            }
                            else
                            {
                                db = DbHelper.CreateDatabase(nv.DatabaseName);
                            }
                            if (db != null)
                            {
                                return db.ExecuteDataTable(nv.SelectCommand);
                            }
                            else
                            {
                                return nv.EmptyDataTable;
                            }
                        }));
                    }
                    else
                    {
                        DbHelper db = null;
                        if (string.IsNullOrEmpty(nv.DatabaseName))
                        {
                            db = DbHelper.Instance;
                        }
                        else
                        {
                            db = DbHelper.CreateDatabase(nv.DatabaseName);
                        }
                        if (db != null)
                        {
                            dt = db.ExecuteDataTable(nv.SelectCommand);
                        }
                        else
                        {
                            dt = nv.EmptyDataTable;
                        }
                    }
                    if (dt != null)
                    {
                        dt.TableName = nv.Name;
                        if (ds.Tables.Contains(dt.TableName))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ds.Tables[dt.TableName].ImportRow(row);
                            }
                        }
                        else
                        {
                            ds.Tables.Add(dt);
                        }
                    }
#else
                if (string.IsNullOrEmpty(nv.DatabaseName))
                {
                    DbHelper.Instance.Database.LoadDataSet(nv.SelectCommand, ds, nv.Name);
                }
                else
                {
                    DbHelper.CreateDatabase(nv.DatabaseName).Database.LoadDataSet(nv.SelectCommand, ds, nv.Name);
                }
#endif
                    m_loadedTables[nvName] = true;

                    nv.OnDataSourceChanged(System.EventArgs.Empty);
                }
            }
        }


        #endregion

        #region "Find"

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="nvName"></param>
        /// <returns></returns>
        public bool Contains(string nvName)
        {
            return m_mappings.ContainsKey(nvName);
        }

        //private Dictionary<string, string> m_alias = new Dictionary<string, string>();

        ///// <summary>
        ///// AddAias
        ///// </summary>
        ///// <param name="alias"></param>
        ///// <param name="realName"></param>
        //public void AddAias(string alias, string realName)
        //{
        //    m_alias[alias] = realName;
        //}

        //public Dictionary<string, string> Alias
        //{
        //    get { return m_alias; }
        //}

        /// <summary>
        /// 按照名称得到某个NameValueMapping
        /// </summary>
        /// <param name="nvName"></param>
        /// <returns></returns>
        public NameValueMapping this[string nvName]
        {
            get
            {
                //if (m_alias.ContainsKey(name))
                //{
                //    name = m_alias[name];
                //}

                if (!m_mappings.ContainsKey(nvName))
                {
                    throw new ArgumentException(string.Format("There is no {0} in NameValueMappingCollection!", nvName), "nvName");
                }
                return m_mappings[nvName];
            }
        }

        /// <summary>
        /// 在某个DataTable（一般是编号，名称Table）上根据名称寻找编号
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object FindIdFromName(string nvName, object name)
        {
            NameValueMapping nv = this[nvName];
            object ret = FindColumn2FromColumn1(nvName, nv.DisplayMember, nv.ValueMember, name);
            return ret;
        }

        /// <summary>
        /// 在某个DataTable（一般是编号，名称Table）上根据编号寻找名称
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindNameFromId(string nvName, object id)
        {
            NameValueMapping nv = this[nvName];
            object ret = FindColumn2FromColumn1(nvName, nv.ValueMember, nv.DisplayMember, id);
            if (ret != null)
            {
                return ret.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 在某个DataTable上根据fromString Column寻找对应的toString Column上的内容
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="fromColumnName"></param>
        /// <param name="toColumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object FindColumn2FromColumn1(string nvName, string fromColumnName, string toColumnName, object value)
        {
            string[] ss = nvName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length == 1)
                return FindColumn2FromColumn1(null, nvName, fromColumnName, toColumnName, value);
            else if (ss.Length == 2)
                return FindColumn2FromColumn1(ss[0], ss[1], fromColumnName, toColumnName, value);
            else
                throw new ArgumentException("Invalid nvName of " + nvName, "nvName");
        }

        /// <summary>
        /// 在某个DataTable上根据fromString Column寻找对应的toString Column上的内容
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="nvName"></param>
        /// <param name="fromColumnName"></param>
        /// <param name="toColumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object FindColumn2FromColumn1(string dsName, string nvName, string fromColumnName, string toColumnName, object value)
        {
            if (value == null)
            {
                return null;
            }

            DataView dv = GetDataSource(dsName, nvName, null);
            DataTable dt = dv.Table;
            if (dt.Rows.Count == 0)
                return null;

            DataRow[] rows = null;

            if (dt.TableName.StartsWith(s_enumFlag))
            {
                rows = dt.Select(dt.Columns[fromColumnName].ColumnName + " = " + (int)Utils.ConvertHelper.ChangeType(value, (Type)dt.Rows[0][EnumNameValue.ValueTypeName]));
            }
            else
            {
                rows = dt.Select(dt.Columns[fromColumnName].ColumnName + " = '" + value.ToString() + "'");
            }

            if (rows.Length > 1)
            {
                throw new ArgumentException(string.Format("there is more than one row that correspond the give name when parameters is {0}, {1}, {2}", nvName, fromColumnName, toColumnName), "nvName");
            }
            else if (rows.Length == 1)
            {
                return rows[0][toColumnName] == System.DBNull.Value ? null : rows[0][toColumnName];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region "Interface"

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<NameValueMapping> GetEnumerator()
        {
            return m_mappings.Values.GetEnumerator();
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}