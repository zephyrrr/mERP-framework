using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// Process类型
    /// 针对一些常用的Process进行归纳而来
    /// </summary>
    public enum WindowMenuType
    {
        /// <summary>
        /// 不合理
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 1,
        /// <summary>
        /// 撤销提交
        /// </summary>
        Unsubmit = 2,
        /// <summary>
        /// 多条提交（多条，表格当前页）
        /// </summary>
        SubmitMulti = 11,
        /// <summary>
        /// 选择内容（例如对账单中选择费用）
        /// </summary>
        Select = 3,
        /// <summary>
        /// 输入数据（例如自动凭证对账单中输入凭证金额）
        /// 未实现
        /// </summary>
        Input = 4,
        /// <summary>
        /// 作废
        /// </summary>
        Cancellate = 5,
        /// <summary>
        /// 报表（单条）
        /// (多线程)
        /// </summary>
        ReportSingle = 6,
        /// <summary>
        /// 报表（多条，表格当前页）
        /// (多线程)
        /// </summary>
        ReportMulti = 8, 
        /// <summary>
        /// 执行数据库过程或sql语句
        /// (多线程)
        /// </summary>
        DatabaseCommand = 7,
        /// <summary>
        /// 多条数据库命令（需要参数）。
        /// 执行多次单条命令
        ///  (多线程)
        /// </summary>
        DatabaseCommandMulti = 12,
        /// <summary>
        /// 单数据库命令（多个参数，用,分隔）。
        ///  (多线程)
        /// </summary>
        DatabaseCommandMultiParam = 13,
        /// <summary>
        /// 执行给定的Process(按照ProcessInfo)
        /// </summary>
        Process = 9,
        /// <summary>
        /// 执行Dao中的特定方法
        /// </summary>
        DaoProcess = 10,
        /// <summary>
        /// Detail编辑窗体修改或者增加Master
        /// </summary>
        ManyToOneWindow = 14,
        /// <summary>
        /// 微软报表（单条）
        /// </summary>
        MsReportSingle = 16,
        /// <summary>
        /// 微软报表（多条，表格当前页）
        /// </summary>
        MsReportMulti = 18, 
        /// <summary>
        /// 分隔条
        /// </summary>
        Separator = 99,
        /// <summary>
        /// 增加
        /// </summary>
        Add = 81,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 82,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 83,
        /// <summary>
        /// 确认3种操作
        /// </summary>
        Confirm = 84,
        /// <summary>
        /// 取消3种操作
        /// </summary>
        Cancel = 85,
        /// <summary>
        /// 执行Action
        /// </summary>
        Action = 70
    }

    /// <summary>
    /// 窗口<see cref="WindowInfo"/>自身的工具栏数据定义。工具栏可位于主窗体或者明细窗体。
    /// </summary>
    [Class(0, Name = "Feng.WindowMenuInfo", Table = "AD_Window_Menu", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class WindowMenuInfo : BaseADEntity
    {
        /// <summary>
        /// 工具栏标题
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 图像名
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string ImageName
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
        /// 工具栏按钮类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual WindowMenuType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 点击此工具栏按钮时，要执行的过程的参数。
        /// 不同WindowMenuType参数含义不同。
        /// </summary>
        [Property(NotNull = false, Length = 50)]
        public virtual string ExecuteParam
        {
            get;
            set;
        }

        /// <summary>
        /// 此按钮属于的窗体<see cref="WindowInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_WindowMenu_Window")]
        public virtual WindowInfo Window
        {
            get;
            set;
        }

        ///// <summary>
        ///// 此按钮属于的窗体<see cref="WindowInfo"/>Id
        ///// </summary>
        //[Property(Column = "WindowId", NotNull = true)]
        //public virtual long WindowId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 是否在主窗体
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool InMasterWindow
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有效，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(NotNull = true, Length = 200)]
        public virtual string Enable
        {
            get;
            set;
        }

        /// <summary>
        /// 按钮有效时，<see cref="T:Feng.IControlManager"/>的状态
        /// </summary>
        [Property(NotNull = true, Length = 100)]
        public virtual string EnableState
        {
            get;
            set;
        }

        /// <summary>
        /// 按钮是否可见，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(NotNull = true, Length = 200)]
        public virtual string Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 如果需要对原有按钮进行更改，此处填写原有按钮Name
        /// 当=Null时添加新按钮
        /// </summary>
        [Property(NotNull = false, Length = 50)]
        public virtual string OriginalName
        {
            get;
            set;
        }
    }
}
