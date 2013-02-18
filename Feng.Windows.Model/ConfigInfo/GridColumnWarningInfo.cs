using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 警示信息设置所需信息，用于<see cref="GridColumnInfo.GridColumnType"/>中的<see cref="GridColumnType.WarningColumn"/>
    /// </summary>
    [Class(0, Name = "Feng.GridColumnWarningInfo", Table = "AD_Grid_Column_Warning", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridColumnWarningInfo : BaseADEntity
    {
        /// <summary>
        /// 分组名称，用以根据<see cref="GridColumnInfo.PropertyName"/>读入相应信息
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 显示
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 图像名称
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string ImageName
        {
            get;
            set;
        }

        /// <summary>
        /// 计算条件，表达式格式见Script类。
        /// 返回值必须是bool类型，如是true则显示相应图像。
        /// </summary>
        [Property(Length = 1000, NotNull = true)]
        public virtual string Expression
        {
            get;
            set;
        }

        ///// <summary>
        ///// 背景颜色
        ///// </summary>
        //[Property(Length = 100, NotNull = true)]
        //public virtual string BackColor
        //{
        //    get;
        //    set;
        //}
    }
}
