using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 事件处理类型
    /// </summary>
    public enum EventProcessType
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// SelectedDataValueChanged(for DataControl and for Grid's Cell) ( = 1)
        /// 此时参数是 e.DataControlName : EventProcessInfo's Name 的列表，以';'分隔
        /// </summary>
        SelectedDataValueChanged = 1, 
        /// <summary>
        /// 重新读入NameValueMapping中的数据（动态） ( = 101)
        /// </summary>
        ReloadNv = 101,
        /// <summary>
        /// 一般过程 ( = 99)
        /// </summary>
        Process = 99
    }

    /// <summary>
    /// 事件处理配置信息
    /// </summary>
    [Class(0, Name = "Feng.EventProcessInfo", Table = "AD_EventProcess", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class EventProcessInfo : BaseADEntity
    {
        /// <summary>
        /// EventName
        /// (几个不同的Event可以有同一个EventName，用于出发一系列事件，例如 对应于AD_Window_Tab中的SelectedDataValuesChanged
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string EventName
        {
            get;
            set;
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual EventProcessType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 要执行的过程的参数。
        /// 如果是SelectedDataValueChanged, e.DataControlName : EventProcessInfo's Name 的列表，以';'分隔，根据SelectedDataValue改变的Control再执行响应event
        /// 如果ReloadNv，
        ///     如果是从Control's SelectedDataValueChanged 过来，参数是要改变DataSource的IDataControl名称, 用:分割
        ///     如果是从Cell's SelectedDataValueChanged过来，参数是要改变Datasource的column名称, 用:分割        
        ///     如果是从其他地方过来，参数是NameValueMapping名称，用:分割
        ///     nv的参数需以@开头，如果有Navigator，用_连接，例如 票_箱_箱号
        /// 如果是Process，则是Process名称
        /// </summary>
        [Property(NotNull = false, Length = 100)]
        public virtual string ExecuteParam
        {
            get;
            set;
        }
    }
}
