using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Feng.Collections;

namespace Feng
{
	/// <summary>
    /// 显示控制器接口
	/// </summary>
	public interface IDisplayManager : IDisposable, IEntityList
	{
        /// <summary>
        /// 名字
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 数据控件集合
        /// </summary>
        IDataControlCollection DataControls
        {
            get;
        }

		/// <summary>
        /// 绑定空间集合
		/// </summary>
        IBindingControlCollection BindingControls
		{
			get;
		}

        /// <summary>
        /// 查询管理器
        /// </summary>
        ISearchManager SearchManager
        {
            get;
        }

        ///// <summary>
        ///// 数据源
        ///// </summary>
        //object DataSource
        //{
        //    get;
        //}

        ///// <summary>
        ///// 数据成员
        ///// </summary>
        //string DataMember
        //{
        //    get;
        //}

		/// <summary>
		/// 和数据源绑定
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="dataMember"></param>
		void SetDataBinding(object dataSource, string dataMember);

        /// <summary>
        /// 增加一个数据项
        /// </summary>
        /// <param name="dataItem"></param>
        void AddDataItem(object dataItem);

        /// <summary>
        /// 当前位置
        /// </summary>
        int Position
        {
            get;
            set;
        }

        /// <summary>
        /// 引发PositionChanged<see cref="PositionChanged"/>事件
        /// </summary>
        void OnPositionChanged(System.EventArgs e);

        /// <summary>
        /// <see cref="Position"/>改变前发生
        /// </summary>
        event CancelEventHandler PositionChanging;

        /// <summary>
        /// <see cref="Position"/>改变后发生
        /// </summary>
        event EventHandler PositionChanged;

        /// <summary>
        /// 显示当前位置实体类数据
        /// </summary>
        void DisplayCurrent();

        

        ///// <summary>
        ///// 根据序号获得实体类
        ///// </summary>
        ///// <param name="idx"></param>
        ///// <returns></returns>
        //object GetItem(int idx);

        /// <summary>
        /// 实体类信息
        /// </summary>
        IEntityMetadata EntityInfo
        {
            get;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        object Clone();

        /// <summary>
        /// SelectedDataValueChanged event
        /// </summary>
        event EventHandler<SelectedDataValueChangedEventArgs> SelectedDataValueChanged;

        /// <summary>
        /// 引发<see cref="SelectedDataValueChanged"/> 事件
        /// </summary>
        /// <param name="e"></param>
        void OnSelectedDataValueChanged(SelectedDataValueChangedEventArgs e);

        /// <summary>
        /// 开始批量操作
        /// </summary>
        void BeginBatchOperation();

        /// <summary>
        /// 结束批量操作
        /// </summary>
        void EndBatchOperation();

        /// <summary>
        /// 是否正在批量操作
        /// </summary>
        /// <returns></returns>
        bool InBatchOperation
        {
            get;
        }
	}
}
