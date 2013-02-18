using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 记录创建和修改用户和时间的实体类的接口
    /// </summary>
    public interface ILogEntity : IEntity
    {
        /// <summary>
        /// 创建者
        /// </summary>
        string CreatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// 修改者
        /// </summary>
        string UpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? Updated
        {
            get;
            set;
        }
    }
}
