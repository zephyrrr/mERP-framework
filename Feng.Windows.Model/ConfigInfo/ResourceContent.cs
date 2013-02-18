using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 资源内容类型
    /// </summary>
    public enum ResourceContentType
    {
        /// <summary>
        /// 本地文件
        /// </summary>
        File = 1,
        /// <summary>
        /// 字符串
        /// </summary>
        String = 2,
        /// <summary>
        /// 二进制文件
        /// </summary>
        Binary = 3
    }

    /// <summary>
    /// 资源内容
    /// </summary>
    public class ResourceContent
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceContentType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 具体内容
        /// </summary>
        public object Content
        {
            get;
            set;
        }
    }
}
