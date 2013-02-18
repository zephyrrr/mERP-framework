using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Edit = 0
    }

    /// <summary>
    /// 任务设置所需信息，用于<see cref="T:Feng.Windows.Forms.TaskPane"/>。
    /// 任务为起始窗体中的信息，如“您今天还有2票备案未提交”
    /// </summary>
    [Class(0, Name = "Feng.TaskInfo", Table = "AD_Task", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class TaskInfo : BaseADEntity
    {
        /// <summary>
        /// 组名，目前程序中用到的是"全部"
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GroupName
        {
            get;
            set;
        }


        /// <summary>
        /// 显示文本
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 查找管理器名称。具体设置见<see cref="WindowTabInfo.SearchManagerClassName"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string SearchManagerClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 查找管理器<see cref="SearchManagerClassName"/>初始化参数。多个参数用','分隔
        /// </summary>
        [Property(Length = 2000, NotNull = false)]
        public virtual string SearchManagerClassParams
        {
            get;
            set;
        }

        /// <summary>
        /// 查找条件，表达式格式见<see cref="Feng.SearchExpression"/>
        /// </summary>
        [Property(Length = 200, NotNull = true)]
        public virtual string SearchExpression
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
        /// 是否显示，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 父级名称。目前支持2级任务
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string ParentName
        {
            get;
            set;
        }

        
        /// <summary>
        /// 任务类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual TaskType TaskType
        {
            get;
            set;
        }

        /// <summary>
        /// 处理该任务时的动作。目前支持普通窗体<see cref="WindowInfo"/>和特殊窗体<see cref="FormInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Task_Action", NotNull = true)]
        public virtual ActionInfo Action
        {
            get;
            set;
        }

        ///// <summary>
        ///// 处理该任务时的动作。目前支持普通窗体<see cref="WindowInfo"/>和特殊窗体<see cref="FormInfo"/>
        ///// </summary>
        //[Property(Column = "ActionId", NotNull = true)]
        //public virtual long ActionId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 任务重要级别（目前不可用）
        /// </summary>
        [Property(NotNull = true)]
        public virtual int Level
        {
            get;
            set;
        }
    }
}
