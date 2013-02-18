using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Master Entity，用于NHibernateMasterGenerateDetailDao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IMasterGenerateDetailEntity<T, S> : IMasterEntity<T, S>
        where T : class, IMasterGenerateDetailEntity<T, S>
        where S : class, IDetailGenerateDetailEntity<T, S>
    {
        /// <summary>
        /// 生成子记录
        /// </summary>
        IList<S> GenerateDetails();
    }
}
