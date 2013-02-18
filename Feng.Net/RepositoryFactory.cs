using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Feng.Net
{
    /// <summary>
    /// 创建Repository的Factory
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        /// <summary>
        /// 根据配置名生成Repository
        /// </summary>
        /// <param name="repCfgName"></param>
        /// <returns></returns>
        public virtual IRepository GenerateRepository(string repCfgName = null)
        {
            return new Repository(new MyHttpClient());
        }


        public IRepository GetCurrentRepository()
        {
            throw new NotImplementedException();
        }
    }
}
