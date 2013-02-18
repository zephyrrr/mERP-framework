using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 使用Repository的类
    /// </summary>
    public interface IRepositoryConsumer
    {
        /// <summary>
        /// Repository配置名。可为空，空时采用默认名。
        /// </summary>
        string RepositoryCfgName
        {
            get;
            set;
        }
    }
}
