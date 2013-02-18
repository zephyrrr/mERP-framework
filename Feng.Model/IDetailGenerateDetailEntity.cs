using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Detail Entity，用于NHibernateMasterGenerateDetailDao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IDetailGenerateDetailEntity<T, S> : IDetailEntity<T, S>
        where T : class, IMasterGenerateDetailEntity<T, S>
        where S : class, IDetailGenerateDetailEntity<T, S>
    {
        /// <summary>
        /// 与新纪录比较，如果相符，则拷贝相应信息（从newEntity到this），返回true
        /// </summary>
        bool CopyIfMatch(S newEntity);
    }
}
