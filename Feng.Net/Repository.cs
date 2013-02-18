using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Feng.Net
{
    public class Repository : IRepository
    {
        #region "Operation"
        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="entity"></param>
        public void Save(object entity) 
        {
            string postJson = Utils.TypeHelper.SerializeTypeFromRealToWS(entity);
            string addr = GetServiceAddress(entity.GetType());
            bool r = true;
            try
            {
                var s = m_client.PutSync(addr, postJson);
                r = Convert.ToBoolean(s);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when put " + addr, ex);
            }
            if (!r)
            {
                throw new InvalidUserOperationException("operation failed!");
            }
        }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="entity"></param>
        public void Update(object entity) 
        {
            string postJson = Utils.TypeHelper.SerializeTypeFromRealToWS(entity);
            string addr = GetServiceAddress(entity.GetType());
            bool r = true;
            try
            {
                var s = m_client.PostSync(addr, postJson);
                r = Convert.ToBoolean(s);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when post " + addr, ex);
            }
            if (!r)
            {
                throw new InvalidUserOperationException("operation failed!");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(object entity) 
        {
            if (entity == null)
                return;

            var id = entity.GetType().InvokeMember("Identity", System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                        null, entity, null, null, null, null);
            if (id == null)
                return;
            bool r = true;
            string addr = GetServiceAddress(entity.GetType()) + "/" + id.ToString();
            try
            {
                var s = m_client.DeleteSync(addr);
                r = Convert.ToBoolean(s);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when delete " + addr, ex);
            }
            if (!r)
            {
                throw new InvalidUserOperationException("operation failed!");
            }
        }

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

        public Repository(MyHttpClient client)
        {
            m_client = client;
        }
        public void Dispose()
        {
        }

        private MyHttpClient m_client;

        public object Get(Type type, object id)
        {
            throw new NotSupportedException("not suppored in web Repository!");
        }

        public System.Collections.IList List(Type persistantClass)
        {
            throw new NotSupportedException("not suppored in web Repository!");
        }

        private string m_serviceAddress;
        private string GetServiceAddress(Type type)
        {
            if (string.IsNullOrEmpty(m_serviceAddress))
            {
                m_serviceAddress = string.Format("{0}/{1}/Generated/DSS_{2}.svc/", SystemConfiguration.Server, SystemConfiguration.ApplicationName, type.Name);
            }
            return m_serviceAddress;
        }
        public IList<T> List<T>(string queryString = null, Dictionary<string, object> parameters = null)
            where T : class, new()
        {
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
            string addr = GetServiceAddress(typeof(T)) + query.ToString();
            try
            {
                var s = m_client.GetSync(addr);
                if (string.IsNullOrEmpty(s))
                    return null;

                System.Collections.IEnumerable data = Newtonsoft.Json.JsonConvert.DeserializeObject(s) as System.Collections.IEnumerable;
                var list = new List<T>();
                
                foreach (var i in data)
                {
                    var item = Utils.TypeHelper.ConvertTypeFromWSToReal<T>(i);
                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when get " + addr, ex);
            }    
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSupportTransaction
        {
            get { return false; }
        }
    }
}
