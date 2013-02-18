using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using Feng.Windows.Forms;

namespace Feng.Windows.Collections
{
    /// <summary>
    /// 数据控件集合
    /// </summary>
    public class DataControlCollection : Feng.Collections.DataControlCollection
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public DataControlCollection()
        {
        }

        /// <summary>
        /// 插入控件。
        /// 处理Enter事件，并根据实体类信息设置文本框最大长度
        /// </summary>
        /// <param name="item"></param>
        public override void Add(IDataControl item)
        {
            if (this.Contains(item))
                return;

            base.Add(item);

            IWindowControl windowControl = item as IWindowControl;

            if (windowControl != null && windowControl.Control != null)
            {
                // 确定Index
                if (m_bKeyPressEvent)
                {
                    windowControl.Control.KeyPress += new System.Windows.Forms.KeyPressEventHandler(InputBox_KeyPress);
                    //if (item.Index != -1)
                    //{
                    //    windowControl.Control.TabIndex = item.Index;
                    //}
                }
            }

            //// 未指定Index，最大加1
            //if (item.Index == -1 || m_items.ContainsKey(item.Control.TabIndex)))
            //{
            //    if (dc.Control != null && dc.Control.TabStop && !m_items.ContainsKey(dc.Control.TabIndex))
            //    {
            //        dc.Index = dc.Control.TabIndex;
            //    }
            //    else
            //    {
            //        int delta = 1;
            //        if (dc.Control == null)
            //        {
            //            delta = 100;
            //        }

            //        if (m_items.Count > 0)
            //        {
            //            dc.Index = m_items.Values[m_items.Count - 1].Index + delta;
            //        }
            //        else
            //        {
            //            dc.Index = 0 + delta;
            //        }
            //    }
            //}

            // 根据Entity分析
            if (base.ParentManager != null && base.ParentManager.EntityInfo != null)
            {
                if (!string.IsNullOrEmpty(item.Name)
                    && base.ParentManager.EntityInfo.IdName != item.Name)
                {
                    if (string.IsNullOrEmpty(item.Navigator) && !string.IsNullOrEmpty(item.PropertyName))
                    {
                        var attr = base.ParentManager.EntityInfo.GetPropertMetadata(item.PropertyName);
                        if (attr != null)
                        {
                            TextBox txt = windowControl.Control as TextBox;
                            if (txt != null)
                            {
                                if (attr.Length != -1)
                                {
                                    txt.MaxLength = attr.Length;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region "KeyPress"

        private bool m_bKeyPressEvent = true;

        /// <summary>
        /// 是否触发KeyPressEvent
        /// </summary>
        public bool KeyPressEvent
        {
            get { return m_bKeyPressEvent; }
            set { m_bKeyPressEvent = value; }
        }

        /// <summary>
        /// 手工处理Enter KeyPress事件
        /// </summary>
        public event System.Windows.Forms.KeyPressEventHandler InputBoxKeyPress;

        private void InputBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (InputBoxKeyPress != null)
            {
                InputBoxKeyPress(sender, e);
            }
            else
            {
                if (e.KeyChar == (char) System.Windows.Forms.Keys.Enter)
                {
                    Control c = sender as Control;
                    if (c == null)
                    {
                        return;
                    }

                    IDataControl control = c.Parent as IDataControl;

                    int now = base.Count - 1;
                    for (int i = 0; i < base.Count; ++i)
                    {
                        if (control == base[i] as IDataControl)
                        {
                            now = i;
                            break;
                        }
                    }

                    do
                    {
                        now++;
                        if (now == base.Count)
                        {
                            System.Windows.Forms.SendKeys.Send("{TAB}");
                            break;
                        }

                        c = base[now] as System.Windows.Forms.Control;

                        if (c != null && c.TabStop)
                        {
                            c.Focus();
                        }
                    } while (c == null || !c.ContainsFocus || base[now].ReadOnly);

                    //control.FindForm().SelectNextControl(control, true, true, true, true);

                    e.Handled = true;
                }
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// 设置焦点到第一个可插入的数据控件
        /// </summary>
        public override void FocusFirstInsertableControl()
        {
            FocusFirstControl(true);
        }

        /// <summary>
        /// 设置焦点到第一个可编辑的数据控件
        /// </summary>
        public override void FocusFirstEditableControl()
        {
            FocusFirstControl(false);
        }

        private void FocusFirstControl(bool forAddControl)
        {
            Control c;
            for (int i = 0; i < base.Count; ++i)
            {
                c = base[i] as System.Windows.Forms.Control;
                if (c != null && c.TabStop && !base[i].ReadOnly)
                    //&& ((forAddControl && base[i].Insertable) || (!forAddControl && base[i].Editable)))
                {
                    IWindowControl wc = c as IWindowControl;
                    if (wc != null)
                    {
                        if (wc.Control.CanSelect)
                        {
                            wc.Control.Select();
                            return;
                        }
                    }
                    else
                    {
                        if (c.CanSelect)
                        {
                            c.Select();
                            return;
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// group control with give group id
        ///// </summary>
        ///// <param name="group">group id</param>
        ///// <returns>all control with the group id</returns>
        //public DataControlCollection GetControlGroup(int group)
        //{
        //    DataControlCollection dcc = new DataControlCollection();
        //    IDataControl dc;
        //    for (int i = 0; i < m_items.Count; ++i)
        //    {
        //        dc = m_items.Values[i];
        //        if ( dc.Group == group)
        //        {
        //            dcc.Add(dc);
        //        }
        //    }
        //    return dcc;
        //}

        #endregion
    }
}