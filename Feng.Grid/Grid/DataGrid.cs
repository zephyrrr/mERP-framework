using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng.Grid
{
    /// <summary>
    /// 数据Grid，不用于列表显示，用于输入数据
    /// </summary>
    public class DataGrid : MyGrid, IBindingDataValueControl
    {
        private bool m_readOnlyWhenView;

        /// <summary>
        /// 设置是否在View状态ReadOnly
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnlyWhenView
        {
            get { return m_readOnlyWhenView; }
            set { m_readOnlyWhenView = value; }
        }

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public override void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state, m_readOnlyWhenView);
        }

        /// <summary>
        /// DisplayMember(meanless)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DisplayMember
        {
            get { return null; }
            set { throw new InvalidOperationException("Grid can't set DisplayMember"); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nvcName"></param>
        /// <param name="viewerName"></param>
        /// <param name="editorName"></param>
        /// <param name="editorFilter"></param>
        public void SetDataBinding(string nvcName, string viewerName, string editorName, string editorFilter)
        {
            throw new NotSupportedException("ListBox is not Supported now");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string EditorMappingName
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string EditorFilter
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void ReloadData()
        {
            if (!this.ReadOnly)
            {
                throw new NotSupportedException("ListBox is not Supported now");
            }
        }

        /// <summary>
        /// Grid ValueMember对应的SelectedValue
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.ValueMember) || base.Columns[base.ValueMember] == null)
                {
                    return null;
                }

                if (base.SelectedValue == null)
                {
                    return null;
                }
                else
                {
                    return base.SelectedValue;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(base.ValueMember) || base.Columns[base.ValueMember] == null)
                {
                    return;
                }

                if (value == null)
                {
                    base.SelectedRows.Clear();
                }
                else
                {
                    base.SelectedValue = value;
                }
            }
        }
    }
}