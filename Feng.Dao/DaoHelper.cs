using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class DaoHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="action"></param>
        public static void DoInRepository(IRepositoryDao dao, Action<IRepository> action)
        {
            using (IRepository rep = dao.GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();
                    action(rep);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void DoInRepository(Action<IRepository> action)
        {
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();
                    action(rep);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
