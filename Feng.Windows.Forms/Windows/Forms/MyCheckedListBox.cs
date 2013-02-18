using System;
using System.ComponentModel;
using System.Text;
using System.Collections;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 一组Checkbox
    /// </summary>
    public class MyCheckedListBox : System.Windows.Forms.CheckedListBox, IBindingDataValueControl,
                                    IMultiDataValueControl
    {
        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyCheckedListBox()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CheckOnClick = true;

            this.Size = new System.Drawing.Size(120, 42);
        }

        /// <summary>
        /// Default BorderStyle
        /// </summary>
        [DefaultValue(System.Windows.Forms.BorderStyle.None)]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// Default CheckOnClick
        /// </summary>
        [DefaultValue(true)]
        public new bool CheckOnClick
        {
            get { return base.CheckOnClick; }
            set { base.CheckOnClick = value; }
        }

        #endregion

        #region "IBindingDataControl"

        /// <summary>
        /// SetDataBinding
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public void SetDataBinding(object dataSource, string dataMember)
        {
            base.BeginUpdate();
            base.DataSource = dataSource;
            base.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewerName"></param>
        /// <param name="editorName"></param>
        /// <param name="editorFilter"></param>
        public void SetDataBinding(string nvcName, string viewerName, string editorName, string editorFilter)
        {
            throw new NotSupportedException("CheckListBox is not Supported now");
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void ReloadData()
        {
            if (!this.ReadOnly)
            {
                throw new NotSupportedException("CheckListBox is not Supported now");
            }
        }
        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// See <see cref="IMultiDataValueControl.SelectedDataValues"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedDataValues
        {
            get
            {
                ArrayList array = new ArrayList();
                for (int i = 0; i < this.Items.Count; ++i)
                {
                    // oebjct subItem = item.CreateType().InvokeMember(this.ValueMember, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance, null, item, new object[] { });
                    if (base.GetItemChecked(i))
                    {
                        object subItem = base.FilterItemOnProperty(this.Items[i], base.ValueMember);
                        array.Add(subItem);
                    }
                }
                return array;
            }
            set
            {
                if (value == null || value.Count == 0 || base.Items.Count == 0)
                {
                    for (int i = 0; i < this.Items.Count; ++i)
                    {
                        base.SetItemChecked(i, false);
                    }
                }
                else
                {
                    try
                    {
                        for (int i = 0; i < this.Items.Count; ++i)
                        {
                            object subItem = base.FilterItemOnProperty(this.Items[i], base.ValueMember);

                            if (value.IndexOf(subItem.ToString()) != -1)
                            {
                                base.SetItemChecked(i, true);
                            }
                            else
                            {
                                base.SetItemChecked(i, false);
                            }
                        }
                    }
                    catch
                    {
                        throw new ArgumentException("MyCheckedListBox's SelectedDataValues must be object[]");
                    }
                }
            }
        }

        /// <summary>
        /// See <see cref="IDataValueControl.SelectedDataValue"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                IList array = SelectedDataValues;
                foreach (object item in array)
                {
                    sb.Append(item.ToString());
                    sb.Append(',');
                }
                return sb.Length == 0 ? null : sb.ToString();
            }
            set
            {
                if (value == null || base.Items.Count == 0)
                {
                    for (int i = 0; i < this.Items.Count; ++i)
                    {
                        base.SetItemChecked(i, false);
                    }
                }
                else
                {
                    try
                    {
                        string[] ss = ((string) value).Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        SelectedDataValues = new ArrayList(ss);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyCheckedListBox's SelectedDataValue must be string", ex);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// See<see cref="IReadOnlyControl.ReadOnly"/>
        /// </summary>
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