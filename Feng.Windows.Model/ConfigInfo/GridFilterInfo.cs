using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 用于表格筛选按钮<see cref="T:Feng.Grid.GridFilterToolStripItem"/>的信息
    /// </summary>
    [Class(0, Name = "Feng.GridFilterInfo", Table = "AD_Grid_Filter", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridFilterInfo : BaseADEntity
    {
        /// <summary>
        /// 显示标题
        /// </summary>
        [Property(Length = 50, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 表格名，用于读入相应表格的筛选数据
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// Grid中筛选Row的方式。
        /// 格式如下：
        /// Grid1名称:表达式1;Grid2名称:表达式2;...
        /// 表达式以$..$引用此行的Row信息（即为Column名称），其余为一般表达式。
        /// 例如 grdTicket:$出具保函标志$="是";  grdFeeSr:$费用项代码$="出保函收费"; grdFeeDd:$费用项代码$=""; grdFeeCb:$费用项代码$="保函出具费"
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string RowExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Grid中筛选Column的方式。
        /// 见<see cref="RowExpression"/>。表达式目前只支持$PropertyName$，即为此列名称。
        /// 例如：grdTicket:$PropertyName$="报关单号" OR $PropertyName$="出具保函标志" OR $PropertyName$="保函期限" OR $PropertyName$="实际核销时间"
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string ColumnExpression
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
    }
}