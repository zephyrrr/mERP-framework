using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Feng.Windows.Utils;
using Feng.Data;
using Feng.Windows.Forms;
using Feng.Grid;
using Feng.Grid.Viewers;
using Feng.Grid.Editors;

namespace Feng.Windows.Utils
{
	/// <summary>
	/// 为各种支持数据绑定的控件读入数据
	/// </summary>
	public sealed class GridDataLoad
	{
		#region "Constructor"
		/// <summary>
		/// Consturtor
		/// </summary>
        private GridDataLoad()
		{
		}
		#endregion

		#region "GridViewer"
		/// <summary>
		/// 获得DataGrid中ComboBox的显示方式
		/// </summary>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyComboBoxViewer GetGridComboViewer(string nvName, string filter)
		{
            return GetGridComboViewer(null, nvName, filter);
		}

        /// <summary>
        /// 获得DataGrid中ComboBox的显示方式
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static MyComboBoxViewer GetGridComboViewer(string dsName, string nvName, string filter)
        {
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(dsName, nvName, filter);
            return new MyComboBoxViewer(nvName);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的显示方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MyComboBoxViewer GetGridComboViewer(Type type)
        {
            // 当以Entity方式时，不需要Viewer。当以DataTable方式时，默认是不用enum
            return GetGridComboViewer(type, true);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的显示方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <returns></returns>
        public static MyComboBoxViewer GetGridComboViewer(Type type, bool notUseEnum)
        {
            string typeNvName = NameValueMappingCollection.Instance.Add(type, notUseEnum);
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(null, typeNvName, string.Empty);

            return new MyComboBoxViewer(newNvName);

            //// 如果我们用DataSet+Name的方式，会有DataView的ListChanged事件，不能dispose
            //return new MyComboBoxViewer
            //    (NameValueMappingCollection.Instance.DataTable(newName, filter), string.Empty, nv.ValueMember, "%" + nv.DisplayMember + "%");
        }

		/// <summary>
		/// 获得DataGrid中MultiCombo的显示方式
		/// </summary>
		/// <param name="nvName"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyOptionPickerViewer GetGridMultiComboViewer(string nvName, string filter)
		{
            return GetGridMultiComboViewer(null, nvName, filter);
		}

        /// <summary>
        /// 获得DataGrid中MultiCombo的显示方式
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="nvName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static MyOptionPickerViewer GetGridMultiComboViewer(string dsName, string nvName, string filter)
        {
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(dsName, nvName, filter);
            return new MyOptionPickerViewer(newNvName);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的显示方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MyOptionPickerViewer GetGridMultiComboViewer(Type type)
        {
            return GetGridMultiComboViewer(type, true);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的显示方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <returns></returns>
        public static MyOptionPickerViewer GetGridMultiComboViewer(Type type, bool notUseEnum)
        {
            string typeNvName = NameValueMappingCollection.Instance.Add(type, notUseEnum);
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(null, typeNvName, string.Empty);

            return new MyOptionPickerViewer(newNvName);
        }
#endregion

        #region "GridEditor"
        /// <summary>
		///  获得DataGrid中ComboBox的编辑方式
		/// </summary>
		/// <param name="nvName"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyComboBoxEditor GetGridComboEditor(string nvName, string filter)
		{
            return GetGridComboEditor(null, nvName, filter); 
		}

		/// <summary>
		/// 获得DataGrid中ComboBox的编辑方式
		/// </summary>
        /// <param name="dsName"></param>
		/// <param name="nvName"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static MyComboBoxEditor GetGridComboEditor(string dsName, string nvName, string filter)
		{
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(dsName, nvName, filter);

            MyComboBoxEditor editor = new MyComboBoxEditor(newNvName);
            ControlDataLoad.InitDataControl((editor.TemplateControl as INameValueMappingBindingControl), dsName, nvName, nvName, filter);
			return editor;
		}

        /// <summary>
        /// 获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MyComboBoxEditor GetGridComboEditor(Type type)
        {
            // 默认模式值是Enum
            return GetGridComboEditor(type, false);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <returns></returns>
        public static MyComboBoxEditor GetGridComboEditor(Type type, bool notUseEnum)
        {
            return GetGridComboEditor(type, notUseEnum, string.Empty);
        }

		/// <summary>
		/// 获得DataGrid中ComboBox的编辑方式
		/// </summary>
		/// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyComboBoxEditor GetGridComboEditor(Type type, bool notUseEnum, string filter)
		{
            string typeNvName = NameValueMappingCollection.Instance.Add(type, notUseEnum);
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(null, typeNvName, string.Empty);

            MyComboBoxEditor editor = new MyComboBoxEditor(newNvName);
            ControlDataLoad.InitDataControl(editor.TemplateControl as INameValueMappingBindingControl, type, notUseEnum, filter);

			return editor;
		}


		/// <summary>
		///  获得DataGrid中ComboBox的编辑方式
		/// </summary>
		/// <param name="nvName"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyFreeComboBoxEditor GetGridFreeComboEditor(string nvName, string filter)
		{
            return GetGridFreeComboEditor(null, nvName, filter);
		}

		/// <summary>
		/// 获得DataGrid中ComboBox的编辑方式
		/// </summary>
        /// <param name="dsName"></param>
		/// <param name="nvName"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static MyFreeComboBoxEditor GetGridFreeComboEditor(string dsName, string nvName, string filter)
		{
            MyFreeComboBoxEditor editor = new MyFreeComboBoxEditor();
            ControlDataLoad.InitDataControl(editor.TemplateControl as INameValueMappingBindingControl, dsName, nvName, nvName, filter);

			return editor;
		}

        /// <summary>
        ///  获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MyFreeComboBoxEditor GetGridFreeComboEditor(Type type)
        {
            return GetGridFreeComboEditor(type, false);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <returns></returns>
        public static MyFreeComboBoxEditor GetGridFreeComboEditor(Type type, bool notUseEnum)
        {
            return GetGridFreeComboEditor(type, notUseEnum, string.Empty);
        }

		/// <summary>
		/// 获得DataGrid中ComboBox的编辑方式
		/// </summary>
		/// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyFreeComboBoxEditor GetGridFreeComboEditor(Type type, bool notUseEnum, string filter)
		{
            string typeNvName = NameValueMappingCollection.Instance.Add(type, notUseEnum);
			MyFreeComboBoxEditor editor = new MyFreeComboBoxEditor();
            ControlDataLoad.InitDataControl(editor.TemplateControl as INameValueMappingBindingControl, type, notUseEnum, filter);

			return editor;
		}

        /// <summary>
        ///  获得DataGrid中MultiCombo的编辑方式
		/// </summary>
		/// <param name="nvName"></param>
        /// <param name="filter"></param>
		/// <returns></returns>
        public static MyOptionPickerEditor GetGridMultiComboEditor(string nvName, string filter)
		{
            return GetGridMultiComboEditor(null, nvName, filter);
		}

		/// <summary>
        /// 获得DataGrid中MultiCombo的编辑方式
		/// </summary>
        /// <param name="dsName"></param>
		/// <param name="nvName"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
        public static MyOptionPickerEditor GetGridMultiComboEditor(string dsName, string nvName, string filter)
		{
            string newNvName = NameValueMappingCollection.Instance.GetDataSourceName(dsName, nvName, filter);
            MyOptionPickerEditor editor = new MyOptionPickerEditor(newNvName);
            ControlDataLoad.InitDataControl(editor.TemplateControl as INameValueMappingBindingControl, dsName, nvName, nvName, filter);

			return editor;
		}

        /// <summary>
        /// 获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MyOptionPickerEditor GetGridMultiComboEditor(Type type)
        {
            return GetGridMultiComboEditor(type, false);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <returns></returns>
        public static MyOptionPickerEditor GetGridMultiComboEditor(Type type, bool notUseEnum)
        {
            return GetGridMultiComboEditor(type, notUseEnum, string.Empty);
        }

        /// <summary>
        /// 获得DataGrid中ComboBox的编辑方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="notUseEnum"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static MyOptionPickerEditor GetGridMultiComboEditor(Type type, bool notUseEnum, string filter)
        {
            string typeNvName = NameValueMappingCollection.Instance.Add(type, notUseEnum);
            MyOptionPickerEditor editor = new MyOptionPickerEditor(typeNvName);
            ControlDataLoad.InitDataControl(editor.TemplateControl as INameValueMappingBindingControl, type, notUseEnum, filter);

            return editor;
        }
		#endregion


	}
}
