using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 菜单信息，用于生成程序“功能”模块下的各个子菜单，支持子菜单。
    /// </summary>
    [Class(0, Name = "Feng.MenuInfo", Table = "AD_Menu", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class MenuInfo : BaseADEntity
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
        /// 此菜单所关联的动作，即点击菜单后会产生的动作，见<see cref="ActionInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Menu_Action")]
        public virtual ActionInfo Action
        {
            get;
            set;
        }

        ///// <summary>
        ///// 此菜单所关联的动作，即点击菜单后会产生的动作，见<see cref="ActionInfo"/>
        ///// </summary>
        //[Property(Column = "ActionId", NotNull = false)]
        //public virtual long? ActionId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 如果动作类型为窗体，决定是否以模式对话框显示。
        /// 默认为非模式对话框，为多窗体结构下的子窗体
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool AsDialog
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
        /// 父级菜单，用于确定菜单的父子关系
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Menu_Parent")]
        public virtual MenuInfo ParentMenu
        {
            get;
            set;
        }

        ///// <summary>
        ///// 父级菜单Id，用于确定菜单的父子关系
        ///// </summary>
        //[Property(Column = "ParentMenuId", NotNull = false)]
        //public virtual long? ParentMenuId
        //{
        //    get;
        //    set;
        //}

        //private IList<MenuInfo> m_childs = new List<MenuInfo>();
        ///// <summary>
        ///// 子级菜单，用于缓存程序自动生成。
        ///// 为了提高效率，菜单为一次性读入，然后程序根据<see cref="ParentMenu"/>来自动生成父子关系，
        ///// 而不是通过数据库的外键和NHibernate自动读取子菜单。
        ///// </summary>
        //public virtual IList<MenuInfo> Childs
        //{
        //    get { return m_childs; }
        //}

        /// <summary>
        /// 子级菜单(通过数据库读取，为LazyLoad的Proxy)
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true, OrderBy = "SeqNo")]
        [Key(1, Column = "ParentMenu")]
        [Index(2, Column = "SeqNo", Type = "int")]
        [OneToMany(3, ClassType = typeof(MenuInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<MenuInfo> ChildMenus
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可见。格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string Visible 
        { 
            get; 
            set;
        }

        // Todo: 如何用MultiComboBox来修改
        ///// <summary>
        ///// 快捷键
        ///// </summary>
        //[Property(NotNull = false)]
        //public virtual System.Windows.Forms.Keys? Shortcut
        //{
        //    get;
        //    set; 
        //}
    }
}