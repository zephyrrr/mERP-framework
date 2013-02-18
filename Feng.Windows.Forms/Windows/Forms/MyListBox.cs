using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyListBox
    /// </summary>
    public class MyListBox : ListBox, IBindingDataValueControl
    {
        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyListBox()
        {
            base.SelectionMode = SelectionMode.One;

            base.Size = new System.Drawing.Size(120, 42);
        }

        /// <summary>
        /// SelectionMode = SelectionMode.One
        /// </summary>
        [DefaultValue(SelectionMode.One)]
        public new SelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        #endregion

        #region "IBindingDataControl"

        /// <summary>
        /// SetDataBinding
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public virtual void SetDataBinding(object dataSource, string dataMember)
        {
            base.BeginUpdate();
            base.DataSource = dataSource;
            base.EndUpdate();
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
        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// ListBox对应的SelectedValue
        /// 如SelectedValue类型和ValueMember对应类型不同，则设置为null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue
        {
            get
            {
                if (base.SelectedIndex == -1)
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
                if (value == null || this.Items.Count == 0)
                {
                    base.SelectedIndex = -1;
                }
                else
                {
                    //object subItem = base.Items[0].CreateType().InvokeMember(this.ValueMember, System.Reflection.BindingFlags.GetProperty, null, base.Items[0], new object[] { });
                    //Type type = subItem.CreateType();
                    //if (type != value.CreateType())
                    //    throw new ArgumentException("MyListBox's SelectedDataValue's type is invalid");


                    //foreach (object item in base.Items)
                    //{
                    //    subItem = item.CreateType().InvokeMember(this.ValueMember, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance, null, item, new object[] { });
                    //    if (subItem == value)
                    //    {
                    //        base.SelectedItem = item;
                    //        break;
                    //    }
                    //}
                    try
                    {
                        base.SelectedValue = value;
                    }
                    catch
                    {
                        throw new ArgumentException("MyListBox's SelectedDataValue's type is invalid");
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// ReadOnly = !Enable
        /// </summary>
        [Category("Data")]
        [Description("是否可读")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return !base.Enabled; }
            set
            {
                if (base.Enabled != !value)
                {
                    base.Enabled = !value;

                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion
    }
}