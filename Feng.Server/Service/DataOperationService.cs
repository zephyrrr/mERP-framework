using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Server.Service
{
    public class DataOperationService<T, S> : DataSearchService<T, S>, IDataOperationRestService<T>
        where T : class, IEntity, new()
        where S : class, IEntity, new()
    {
        public bool Insert(T entity)
        {
            var e = Feng.Net.Utils.TypeHelper.ConvertTypeFromWSToReal<S>(entity);
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<S>())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.Save(e);
                    rep.CommitTransaction();
                    return true;
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithResume(ex);
                    return false;
                }
            }
        }

        public bool Update(T entity)
        {
            var e = Feng.Net.Utils.TypeHelper.ConvertTypeFromWSToReal<S>(entity);

            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<S>())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.Update(e);
                    rep.CommitTransaction();
                    return true;
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithResume(ex);
                    return false;
                }
            }
        }

        public bool Delete(T entity)
        {
            var e = Feng.Net.Utils.TypeHelper.ConvertTypeFromWSToReal<S>(entity);

            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<S>())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.Delete(e);
                    rep.CommitTransaction();
                    return true;
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithResume(ex);
                    return false;
                }
            }
        }
    }
}
