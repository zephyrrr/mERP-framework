using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    /// <summary>
    /// 可编辑的Grid
    /// </summary>
    public interface IArchiveGrid : IBoundGrid, ICheckControl
    {
        /// <summary>
        /// 是否允许内部插入
        /// </summary>
        bool AllowInnerInsert { get; set; }

        /// <summary>
        /// 是否允许内部编辑
        /// </summary>
        bool AllowInnerEdit { get; set; }

        /// <summary>
        /// 是否允许内部删除
        /// </summary>
        bool AllowInnerDelete { get; set; }

        /// <summary>
        /// 操作管理器
        /// </summary>
        IControlManager ControlManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.InsertionRow InsertionRow { get; }

        /// <summary>
        /// ArchiveGridHelper
        /// </summary>
        ArchiveGridHelper ArchiveGridHelper { get; }

        /// <summary>
        /// 是否通过InsertRow添加
        /// </summary>
        bool AddThrowInsertRow { get; set; }

        /// <summary>
        /// 用户自定义Validate
        /// </summary>
        event System.ComponentModel.CancelEventHandler ValidateRow;

        /// <summary>
        /// 是否是主从模式里的从Grid（如果是，要设置状态）
        /// 不同于DetailGrid，是主从模式里的子Grid
        /// </summary>
        bool IsInDetailMode
        {
            get;
            set;
        }
    }
}