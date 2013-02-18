using System;
using System.Collections;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityList
    {
        /// <summary>
        /// 数据源中实体类列表
        /// </summary>
        IList Items
        {
            get;
        }

        /// <summary>
        /// 当前实体类列表数量
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 当前实体类
        /// </summary>
        object CurrentItem
        {
            get;
        }
    }
}
