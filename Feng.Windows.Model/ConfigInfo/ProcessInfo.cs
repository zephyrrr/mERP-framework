using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 程序类型
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// 不合理（=0）
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// C#生成的方法，在dll中（=1）
        /// </summary>
        CSharp = 1,
        /// <summary>
        /// Python文件（=2）
        /// </summary>
        PythonFile = 2,
        /// <summary>
        /// Python表达式（=4）
        /// </summary>
        PythonStatement = 3,
        /// <summary>
        /// Python（不判断类型）（=5）
        /// </summary>
        Python = 4
    }

    /// <summary>
    /// 过程信息，用于定义代码中定义的方法
    /// </summary>
    [Class(0, Name = "Feng.ProcessInfo", Table = "AD_Process", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ProcessInfo : BaseADEntity
    {
        /// <summary>
        /// 方法类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual ProcessType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 过程方法所在类名称
        /// </summary>
        [Property(Length = 4000, NotNull = true)]
        public virtual string ExecuteParam
        {
            get;
            set;
        }

        ///// <summary>
        ///// 过程方法名
        ///// </summary>
        //[Property(Length = 100, NotNull = false)]
        //public virtual string MethodName
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 参数。如有多个参数，以","分割。
        ///// </summary>
        //[Property(Length = 255, NotNull = false)]
        //public virtual string Params
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 执行权限，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Access
        {
            get;
            set;
        }

        /// <summary>
        /// 父级ProcessInfo。
        /// 执行此Process必须先执行父Process
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Process_Parent")]
        public virtual ProcessInfo Parent
        {
            get;
            set;
        }
    }
}
