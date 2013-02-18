using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 作废Dao
    /// </summary>
    public interface ICancellateDao : IRepositoryDao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        void Cancellate(IRepository rep, object entity);
    }
}
