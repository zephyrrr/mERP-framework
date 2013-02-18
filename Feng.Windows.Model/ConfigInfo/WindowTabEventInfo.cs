using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// WindowTab中的事件针对的主体分类
    /// </summary>
    public enum WindowTabEventManagerType
    {
        /// <summary>
        /// 针对IControlManager（=1）
        /// </summary>
        ControlManager = 1,
        /// <summary>
        /// 针对IDisplayManager（=2）
        /// </summary>
        DisplayManager = 2,
        /// <summary>
        /// 针对ISearchManager（=3）
        /// </summary>
        SearchManager = 3,
        /// <summary>
        /// 针对IBaseDao（=4）
        /// </summary>
        BusinessLayer = 4
    }

    /// <summary>
    /// WindowTab中的事件信息
    /// </summary>
    [Class(0, Name = "Feng.WindowTabEventInfo", Table = "AD_Window_Tab_Event", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class WindowTabEventInfo : BaseADEntity
    {
        /// <summary>
        /// 属于的WindowTab名称
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Window_Tab_Event_Parent")]
        public virtual WindowTabInfo ParentWindowTab
        {
            get;
            set;
        }

        /// <summary>
        /// 针对主体分类
        /// </summary>
        [Property(NotNull = true)]
        public virtual WindowTabEventManagerType ManagerType
        {
            get;
            set;
        }

        /// <summary>
        /// 事件名称。目前支持
        ///     IControlManager：BeginningEdit，EditBegun，EndingEdit，EditEnded，EditCanceled，StateChanged，CancellingEdit
        ///     IDisplayManager：PositionChanging，PositionChanged，SelectedDataValueChanged
        ///     ISearchManager：DataLoaded，DataLoading
        ///     IBaseDao：None
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string EventName
        {
            get;
            set;
        }

        /// <summary>
        /// 事件对应Process名称
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string EventProcessName
        {
            get;
            set;
        }
    }
}
