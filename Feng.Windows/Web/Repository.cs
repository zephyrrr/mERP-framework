using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Feng.Web
{
    public class Repository : IRepository
    {
        #region "Operation"
        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="entity"></param>
        public void Save(object entity) { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="entity"></param>
        public void Update(object entity) { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(object entity) { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 新增或修改保存
        /// </summary>
        /// <param name="entity"></param>
        public void SaveOrUpdate(object entity) { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 开始Transaction
        /// </summary>
        /// <returns></returns>
        public void BeginTransaction() { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 提交Transaction
        /// </summary>
        public void CommitTransaction() { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 回滚Transaction
        /// </summary>
        public void RollbackTransaction() { throw new NotSupportedException("not suppored in web Repository!"); }

        public void Initialize(object proxy, object owner) { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// 相当于NHibernate中的Lock
        /// </summary>
        /// <param name="obj"></param>
        public void Attach(object obj) { throw new NotSupportedException("not suppored in web Repository!"); }

        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="obj"></param>
        public void Refresh(object obj) { throw new NotSupportedException("not suppored in web Repository!"); }

        #endregion

        public object Tag
        {
            get;
            set;
        }

        public Repository()
        {
        }
        public void Dispose()
        {
        }

        private MyHttpClient m_client = new MyHttpClient();

        public object Get(Type type, object id)
        {
            throw new NotSupportedException("not suppored in web Repository!");
        }

        public System.Collections.IList List(Type persistantClass)
        {
            throw new NotSupportedException("not suppored in web Repository!");
        }

        public IList<T> List<T>(string queryString = null, Dictionary<string, object> parameters = null)
            where T : class, new()
        {
            string m_serviceAddress = string.Format("{0}/{1}/Generated/DSS_{2}.svc/", SystemConfiguration.Server, SystemConfiguration.ApplicationName, typeof(T).Name);
            StringBuilder query = new StringBuilder();
            query.Append("?");
            query.Append("format=json&");
            if (!string.IsNullOrEmpty(queryString))
            {
                if (!queryString.Contains("where"))
                {
                    queryString = " where " + queryString;
                }
                string[] ss = queryString.Split(new string[] { "where", "order by" }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 1)
                {
                    string se = ss[1];
                    if (parameters != null)
                    {
                        foreach (var kvp in parameters)
                        {
                            if (kvp.Key != "FirstResult" && kvp.Key != "MaxResults")
                            {
                                se = se.Replace(":" + kvp.Key, kvp.Value.ToString());
                            }
                        }
                    }
                    query.Append(string.Format("exp={0}&", se));
                }
                if (ss.Length > 2)
                {
                    query.Append(string.Format("order={0}&", ss[2]));
                }
            }
            if (parameters != null)
            {
                if (parameters.ContainsKey("FirstResult"))
                {
                    query.Append(string.Format("first={0}&", parameters["FirstResult"]));
                }
                if (parameters.ContainsKey("MaxResults"))
                {
                    query.Append(string.Format("count={0}&", parameters["MaxResults"]));
                }
            }
            string addr = m_serviceAddress + query.ToString();
            try
            {
                var s = m_client.GetSync(addr);

                System.Collections.IEnumerable data = Newtonsoft.Json.JsonConvert.DeserializeObject(s) as System.Collections.IEnumerable;
                var list = new List<T>();
                var type = typeof(T);
                foreach (var i in data)
                {
                    T item = new T();
                    foreach (var p in typeof(T).GetProperties())
                    {
                        if (!p.CanWrite)
                            continue;

                        var value = ((i as Newtonsoft.Json.Linq.JObject)[p.Name]);
                        if (p.PropertyType.IsValueType || p.PropertyType.IsEnum
                            || p.PropertyType == typeof(string))
                        {
                            var v2 = value.ToObject(p.PropertyType);
                            if (p.PropertyType.IsEnum)
                            {
                                v2 = Enum.Parse(p.PropertyType, v2.ToString());
                            }
                            type.InvokeMember(p.Name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                                null, item, new object[] { v2 }, null, null, null);
                        }
                        else
                        {
                            if (p.PropertyType.GetInterface("IEnumerable") != null)
                            {
                                continue;
                            }
                            object v2 = Feng.Utils.ReflectionHelper.CreateInstanceFromType(p.PropertyType);
                            string[] sIdNames = new string[] {"Id", "Name"};
                            foreach(var sIdName in sIdNames)
                            {
                                if (v2.GetType().GetProperty(sIdName) != null)
                                {
                                    var v = value.ToObject(v2.GetType().GetProperty(sIdName).PropertyType);
                                    if (v == null)
                                    {
                                        type.InvokeMember(p.Name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                                            null, item, new object[] { null }, null, null, null);
                                    }
                                    else
                                    {
                                        Feng.Utils.ReflectionHelper.SetObjectValue(v2, sIdName, v);
                                        type.InvokeMember(p.Name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                                            null, item, new object[] { v2 }, null, null, null);
                                    }
                                    break;
                                }
                            }
                            
                        }
                    }
                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when access " + addr, ex);
            }    
        }
    }
}
