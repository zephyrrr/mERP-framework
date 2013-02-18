using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 数据库型缓存，目前缓存<see cref="T:Feng.NameValueMapping"/>和<see cref="T:Feng.IEntityBuffer"/>
    /// </summary>
    public class DBDataBuffer : Singleton<DBDataBuffer>, IDataBuffer
    {
        private void EvictNHibernateCache()
        {
            var sm = ServiceProvider.GetService<Feng.NH.ISessionFactoryManager>();
            if (sm == null)
                return;
            // NHibernate Buffer
            foreach (NHibernate.ISessionFactory i in sm.SessionFactories.Values)
            {
                i.EvictQueries();
                IDictionary<string, NHibernate.Metadata.IClassMetadata> dict = i.GetAllClassMetadata();
                foreach (KeyValuePair<string, NHibernate.Metadata.IClassMetadata> de in dict)
                {
                    i.EvictEntity(de.Value.EntityName);
                }
                IDictionary<string, NHibernate.Metadata.ICollectionMetadata> dict2 = i.GetAllCollectionMetadata();
                foreach (KeyValuePair<string, NHibernate.Metadata.ICollectionMetadata> de in dict2)
                {
                    i.EvictCollection(de.Value.Role);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            EvictNHibernateCache();
            NameValueMappingCollection.Instance.Clear();
            EntityBufferCollection.Instance.Clear();
        }

        /// <summary>
        /// 重新读入
        /// </summary>
        public void Reload()
        {
            EvictNHibernateCache();
            NameValueMappingCollection.Instance.Reload();
            EntityBufferCollection.Instance.Reload();

            // 如果clear再load，原来的datasource会无效
            //LoadData();
        }

        private List<string> GetParamsName(string whereClause)
        {
            List<string> ret = new List<string>();
            if (string.IsNullOrEmpty(whereClause))
                return ret;

            int idx = -1;
            do
            {
                idx = whereClause.IndexOf('@', idx + 1);
                if (idx == -1)
                    break;
                int idx2 = idx + 1;
                while (!(whereClause[idx2] == ' ' || whereClause[idx2] == ')' || whereClause[idx2] == '+'
                    || whereClause[idx2] == '-' || whereClause[idx2] == '*' || whereClause[idx2] == '/'
                    || whereClause[idx2] == '('))
                {
                    idx2++;
                    if (idx2 == whereClause.Length)
                        break;
                }
                ret.Add(whereClause.Substring(idx, idx2 - idx));
            } while (idx != -1);
            return ret;
        }
        /// <summary>
        /// 读入数据
        /// </summary>
        public void LoadData()
        {
            // Load NameValueMappings
            NameValueMappingCollection.Instance.Clear();
            IList<NameValueMappingInfo> list = ADInfoBll.Instance.GetInfos<NameValueMappingInfo>();
            foreach (NameValueMappingInfo info in list)
            {
                if (info.OtherMembers == null)
                {
                    info.OtherMembers = string.Empty;
                }
                string[] ss = info.OtherMembers.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] members = new string[ss.Length + 2];
                members[0] = info.ValueMember;
                members[1] = info.DisplayMember;
                for (int i = 0; i < ss.Length; ++i)
                {
                    members[2 + i] = ss[i].Trim();
                }
                NameValueMappingCollection.Instance.Add(new NameValueMapping(info.Name, info.TableName,
                   members, info.WhereClause, info.ParentName));
                if (!string.IsNullOrEmpty(info.MemberVisible))
                {
                    ss = info.MemberVisible.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in ss)
                    {
                        string[] s2 = s.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (s2.Length != 2)
                        {
                            throw new ArgumentException("MemberVisible format in NameValueMappingInfo is invalid");
                        }
                        NameValueMappingCollection.Instance[info.Name].MemberVisible[s2[0].Trim()] = Convert.ToBoolean(s2[1], System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                var paramNames = GetParamsName(info.WhereClause);
                foreach (string s in paramNames)
                {
                    NameValueMappingCollection.Instance[info.Name].Params[s.Trim()] = System.DBNull.Value;
                }
            }

            // Load EntityBuffers
            EntityBufferCollection.Instance.Clear();
            IList<EntityBufferInfo> list2 = ADInfoBll.Instance.GetInfos<EntityBufferInfo>();
            foreach (EntityBufferInfo info in list2)
            {
                if (!string.IsNullOrEmpty(info.PersistentClass))
                {
                    Type entityType = Feng.Utils.ReflectionHelper.GetTypeFromName(info.PersistentClass);
                    string typeNeme = "Feng.EntityBuffer`2[[" + info.PersistentClass + "],[" + info.IdTypeName + "]], Feng.Model";
                    Type type = Feng.Utils.ReflectionHelper.GetTypeFromName(typeNeme);
                    IEntityBuffer eb = Feng.Utils.ReflectionHelper.CreateInstanceFromType(type, new object[] { info.Name }) as IEntityBuffer;
                    EntityBufferCollection.Instance.Add(eb);
                }
            }
        }
    }
}
