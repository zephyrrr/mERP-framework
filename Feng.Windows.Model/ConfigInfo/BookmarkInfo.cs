using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 书签
    /// </summary>
    public class BookmarkInfo
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 地址。可以ServiceProvider.GetService&lt;IApplication&gt;().ExecuteAction()执行
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 子书签
        /// </summary>
        public IList<BookmarkInfo> Childs
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool IsFolder
        {
            get;
            set;
        }
    }
}
