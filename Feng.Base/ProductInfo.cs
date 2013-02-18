using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductInfo : Singleton<ProductInfo>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public Version CurrentVersion
        {
            get;
            set;
        }
    }
}
