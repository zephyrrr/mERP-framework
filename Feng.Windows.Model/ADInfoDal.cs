using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    public class ADInfoDal
    {
        private IRepository GenerateStatelessRepository<T>()
        {
            return ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository();
        }

        private IRepository GenerateStateRepository<T>()
        {
            return ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository();
        }

        internal T GetInfo<T>(string idName)
            where T : class, new()
        {
            var list = GetInfos<T>(string.Format("ID = '{0}'", idName));
            if (list.Count > 0)
                return list[0];
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        internal virtual IList<T> GetInfos<T>(string queryString = null, Dictionary<string, object> parameters = null)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = string.Format("from {0} where IsActive = true", typeof(T).FullName);
            }
            else
            {
                if (!queryString.StartsWith("from"))
                {
                    queryString = string.Format("from {0} where {1} and IsActive = true", typeof(T).FullName, queryString);
                }
            }
            if (!queryString.Contains("order by"))
            {
                Type type = typeof(T);
                if (type.GetProperty("SeqNo") != null)
                {
                    queryString += " order by SeqNo";
                }
            }
            using (var rep = GenerateStatelessRepository<T>())
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                parameters["MaxResults"] = -1;
                var ret = rep.List<T>(queryString, parameters);
                return ret;
            }
        }

        internal virtual void GetUserConfigurationData(UserConfigurationInfo userInfo)
        {
            return;
        }
        internal virtual void SaveOrUpdateUserConfigurationInfo(UserConfigurationInfo userInfo)
        {
            return;
        }
    }
}
