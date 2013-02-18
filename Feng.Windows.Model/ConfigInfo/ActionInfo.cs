using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 动作类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 无效。( = 0)
        /// </summary>
        None = 0,
        /// <summary>
        /// 通用窗体，程序自动根据<see cref="WindowInfo"/>生成，可做CRUD操作。( = 1)
        /// </summary>
        Window = 1,
        /// <summary>
        /// 过程，见<see cref="ProcessInfo"/>。( = 2)
        /// </summary>
        Process = 2,
        /// <summary>
        /// 特殊窗体，见<see cref="FormInfo"/>。( = 3)
        /// </summary>
        Form = 3,
        /// <summary>
        /// 网页。( = 4)
        /// </summary>
        Url = 4,
        /// <summary>
        /// 任务（当前不可用）。( = 6)
        /// </summary>
        Task = 6,
        /// <summary>
        /// 工作流（当前不可用）。( = 7)
        /// </summary>
        Workflow = 7
    }

    /// <summary>
    /// 关于动作的配置信息，用于<see cref="MenuInfo"/>
    /// </summary>
    [Class(0, Name = "Feng.ActionInfo", Table = "AD_Action", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ActionInfo : BaseADEntity
    {
        /// <summary>
        /// 动作类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual ActionType ActionType
        {
            get;
            set;
        }

        /// <summary>
        /// 执行权限，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string Access
        {
            get;
            set;
        }

        /// <summary>
        /// 如果ActionType==Window，要打开的<see cref="WindowInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Menu_Window")]
        public virtual WindowInfo Window
        {
            get;
            set;
        }

        ///// <summary>
        ///// 如果ActionType==Window，要打开的<see cref="WindowInfo"/>Name。
        ///// </summary>
        //[Property(Column = "Window", NotNull = false, Length = 50)]
        //public virtual string WindowName
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 如果ActionType==Process，要打开的<see cref="ProcessInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Menu_Process")]
        public virtual ProcessInfo Process
        {
            get;
            set;
        }

        ///// <summary>
        ///// 如果ActionType==Process，要打开的<see cref="ProcessInfo"/>Id
        ///// </summary>
        //[Property(Column = "ProcessId", NotNull = false)]
        //public virtual string ProcessName
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 如果ActionType==Form，要打开的<see cref="FormInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Menu_Form")]
        public virtual FormInfo Form
        {
            get;
            set;
        }

        ///// <summary>
        ///// 如果ActionType==Form，要打开的<see cref="FormInfo"/>Id
        ///// </summary>
        //[Property(Column = "FormId",NotNull = false)]
        //public virtual long? FormId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 如果ActionType==Web，要打开的网址
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string WebAddress
        {
            get;
            set;
        }
    }
}
