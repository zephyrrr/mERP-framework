using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 可提交的记录
    /// </summary>
    public interface ISubmittedEntity : IEntity
    {
        /// <summary>
        /// 是否已提交
        /// </summary>
        bool Submitted
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 当Entity中需要2个提交时。。。
    /// </summary>
    public interface ISubmittedEntity2 : ISubmittedEntity
    {
        /// <summary>
        /// 是否已提交
        /// </summary>
        bool Submitted2
        {
            get;
            set;
        }
    }
}
