using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{    
    /// <summary>
    /// 选择窗体数据定义，用于<see cref="T:Feng.Forms.ArchiveSelectForm"/>
    /// </summary>
    [Class(0, Name = "Feng.WindowSelectInfo", Table = "AD_Window_Select", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class WindowSelectInfo : BaseADEntity
    {
        /// <summary>
        /// 在Radio上显示的文本
        /// </summary>
        [Property(Length = 50, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 名称，用作读入相关信息
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可见，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Visible
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
        ///此选项要打开的<see cref="WindowInfo"/>，目前支持<see cref="WindowType.Query"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_WindowSelect_Window")]
        public virtual WindowInfo Window
        {
            get;
            set;
        }

        ///// <summary>
        ///// 此选项要打开的<see cref="WindowInfo"/>Id，目前支持<see cref="WindowType.Query"/>
        ///// </summary>
        //[Property(Column = "WindowId", NotNull = false)]
        //public virtual long? WindowId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 此选项要打开的<see cref="FormInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_WindowSelect_Form")]
        public virtual FormInfo Form
        {
            get;
            set;
        }

        ///// <summary>
        ///// 此选项要打开的<see cref="FormInfo"/>Id
        ///// </summary>
        //[Property(Column = "FormId", NotNull = false)]
        //public virtual long? FormId
        //{
        //    get;
        //    set;
        //}
    }
}
