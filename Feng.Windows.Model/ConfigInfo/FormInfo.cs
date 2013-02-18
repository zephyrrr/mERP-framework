using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 特殊窗体信息
    /// </summary>
    [Class(0, Name = "Feng.FormInfo", Table = "AD_Form", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class FormInfo : BaseADEntity
    {
        /// <summary>
        /// 显示标题
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 图像文件名。具体图像在Resource项目下。
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string ImageName
        {
            get;
            set;
        }

        /// <summary>
        /// 特殊窗体的窗体类名称。类必须继承自<see cref="T:System.Windows.Forms.Form"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 创建特殊窗体时的参数，如有多个参数，以","分割
        /// 只能以string作为参数
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string Params
        {
            get;
            set;
        }

        /// <summary>
        /// 执行权限。格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Access
        {
            get;
            set;
        }
    }
}
