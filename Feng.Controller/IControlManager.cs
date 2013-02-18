using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Feng.Collections;

namespace Feng
{
    /// <summary>
    /// 数据操作管理器接口，操作数据（增加，删除，修改）。
    /// 用于操作DataTable类型的数据。（Not Supported Now）
    /// 目前支持用IList(T)数据的IControlManager(T)。
    /// </summary>
    public interface IControlManager : IDisposable, IStateControl //, ICheckControl
    {
        /// <summary>
        /// 名字
        /// </summary>
        string Name
        {
            get;
            set;
        }

        #region "Inner Controls"

        /// <summary>
        /// DisplayManager
        /// </summary>
        IDisplayManager DisplayManager { get; }

        /// <summary>
        /// 状态控件集合
        /// </summary>
        IStateControlCollection StateControls { get; }

        /// <summary>
        /// 处理<see cref="ControlCheckException" />的接口
        /// </summary>
        IControlCheckExceptionProcess ControlCheckExceptionProcess { get; }

        /// <summary>
        /// 检查控件集合
        /// </summary>
        ICheckControlCollection CheckControls { get; }
        /// <summary>
        /// 检查控件值
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        /// <exception cref="ControlCheckException">控件值不符合标准</exception>
        bool CheckControlValue(IDataControl dc);

        #endregion

        #region "Operations"
        /// <summary>
        /// Data Access Layer
        /// </summary>
        IBaseDao Dao
        {
            get;
            set;
        }

        /// <summary>
        /// 是否正在操作
        /// </summary>
        bool InOperating
        {
            get;
        }

        /// <summary>
        /// 在列表末尾添加新的实体类。
        /// </summary>
        object AddNew();

        /// <summary>
        /// 编辑当前位置实体类。
        /// </summary>
        void EditCurrent();

        /// <summary>
        /// 删除当前位置实体类
        /// </summary>
        /// <returns>是否成功删除</returns>
        bool DeleteCurrent();

        /// <summary>
        /// 结束编辑或添加
        /// </summary>
        /// <returns></returns>
        bool EndEdit();

        /// <summary>
        /// 结束编辑或添加
        /// </summary>
        /// <param name="commit">是否保存到数据库</param>
        bool EndEdit(bool commit);

        /// <summary>
        /// 取消当前编辑或添加状态
        /// </summary>
        void CancelEdit();

        /// <summary>
        /// 保存当前数据
        /// </summary>
        bool SaveCurrent();

        /// <summary>
        /// 引发<see cref="ListChanged"/> 事件
        /// </summary>
        /// <param name="e"></param>
        void OnListChanged(ListChangedEventArgs e);

       /// <summary>
        /// 处理位于Position位置的Entity改变后的事情（保存，更新界面）
        /// </summary>
        /// <returns></returns>
        bool ProcessEntityChanged(int position);

        ///// <summary>
        ///// 实体类属性改变后发生
        ///// </summary>
        //event EventHandler<EntityChangedEventArgs> EntityChanged;

        /// <summary>
        /// 在数据列表有变化时发生
        /// </summary>
        event ListChangedEventHandler ListChanged;

        /// <summary>
        /// BeginningEdit event
        /// </summary>
        event EventHandler BeginningEdit;

        /// <summary>
        /// EditBegun event
        /// </summary>
        event EventHandler EditBegun;

        /// <summary>
        /// EndingEdit event
        /// </summary>
        event EventHandler EndingEdit;

        /// <summary>
        /// EditEnded event
        /// </summary>
        event EventHandler EditEnded;

        /// <summary>
        /// EditCanceled event
        /// </summary>
        event EventHandler EditCanceled;

        /// <summary>
        /// CancellingEdit
        /// </summary>
        event CancelEventHandler CancellingEdit;
        #endregion

        #region "State"

        /// <summary>
        /// 当前状态
        /// </summary>
        StateType State { get; set; }

        /// <summary>
        /// StateChanged
        /// </summary>
        event EventHandler StateChanged; 
        #endregion

        #region "Permission"

        /// <summary>
        /// 是否允许删除
        /// </summary>
        bool AllowDelete { get; set; }

        /// <summary>
        /// 是否允许添加
        /// </summary>
        bool AllowInsert { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        bool AllowEdit { get; set; }

        #endregion

        #region "Changed"
        /// <summary>
        /// 数据是否改变
        /// </summary>
        bool Changed { get; }

        /// <summary>
        /// 尝试CancelEdit（如果Cancel则返回true，否则返回false）
        /// </summary>
        /// <returns></returns>
        bool TryCancelEdit();

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        object Clone();

        #endregion
    }
}