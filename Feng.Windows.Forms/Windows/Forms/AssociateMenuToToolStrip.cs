using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class AssociateMenuToToolStrip : IDisposable
    {
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ToolStripMenuItem menu in m_buf1.Keys)
                {
                    menu.Click -= new EventHandler(menu_Click);
                    menu.DropDownItems.Clear();
                }

                foreach (ToolStripItem toolStrip in m_buf2.Keys)
                {
                    toolStrip.EnabledChanged -= new EventHandler(toolStrip_EnabledChanged);
                    toolStrip.VisibleChanged -= new EventHandler(toolStrip_VisibleChanged);
                }
                foreach (ToolStripItem toolStrip in m_toolStripBuffer.Keys)
                {
                    toolStrip.EnabledChanged -= new EventHandler(toolStrip_EnabledChanged);
                    toolStrip.VisibleChanged -= new EventHandler(toolStrip_VisibleChanged);
                }

                m_buf1.Clear();
                m_buf2.Clear();
                m_toolStripBuffer.Clear();
                m_toolStripSameState.Clear();
            }
        }

        private Dictionary<ToolStripMenuItem, ToolStripItem> m_buf1 = new Dictionary<ToolStripMenuItem, ToolStripItem>();
        private Dictionary<ToolStripItem, ToolStripMenuItem> m_buf2 = new Dictionary<ToolStripItem, ToolStripMenuItem>();

        private Dictionary<ToolStripItem, IList<ToolStripItem>> m_toolStripBuffer = new Dictionary<ToolStripItem, IList<ToolStripItem>>();
        private Dictionary<ToolStripItem, bool> m_toolStripSameState = new Dictionary<ToolStripItem, bool>();

        /// <summary>
        /// 从toolStripSrc状态复制到toolStripDest
        /// </summary>
        /// <param name="toolStripSrc"></param>
        /// <param name="toolStripDest"></param>
        public void Associate(ToolStripItem toolStripSrc, ToolStripItem toolStripDest)
        {
            Associate(toolStripSrc, toolStripDest, true);
        }

        /// <summary>
        /// 从toolStripSrc状态复制到toolStripDest
        /// </summary>
        /// <param name="toolStripSrc"></param>
        /// <param name="toolStripDest"></param>
        public void Associate(ToolStripItem toolStripSrc, ToolStripItem toolStripDest, bool sameState)
        {
            if (!m_toolStripBuffer.ContainsKey(toolStripSrc))
            {
                m_toolStripBuffer[toolStripSrc] = new List<ToolStripItem>();
                toolStripSrc.EnabledChanged += new EventHandler(toolStripSrc_EnabledChanged);
                toolStripSrc.VisibleChanged += new EventHandler(toolStripSrc_VisibleChanged);
            }

            m_toolStripBuffer[toolStripSrc].Add(toolStripDest);
            m_toolStripSameState[toolStripDest] = sameState;

            toolStripDest.Enabled = sameState ? toolStripSrc.Enabled : !toolStripSrc.Enabled;
        }

        void toolStripSrc_VisibleChanged(object sender, EventArgs e)
        {
            ToolStripItem toolStrip = sender as ToolStripItem;
            foreach (ToolStripItem i in m_toolStripBuffer[toolStrip])
            {
                i.Visible = m_toolStripSameState[i] ? toolStrip.Visible : !toolStrip.Visible;
            }
        }

        void toolStripSrc_EnabledChanged(object sender, EventArgs e)
        {
            ToolStripItem toolStrip = sender as ToolStripItem;
            foreach (ToolStripItem i in m_toolStripBuffer[toolStrip])
            {
                i.Enabled = m_toolStripSameState[i] ? toolStrip.Enabled : !toolStrip.Enabled;
            }
        }

        /// <summary>
        /// 从ToolStripItem状态数据复制到MenuItem
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="toolStrip"></param>
        public void Associate(ToolStripMenuItem menu, ToolStripItem toolStrip)
        {
            m_buf1[menu] = toolStrip;
            m_buf2[toolStrip] = menu;

            if (string.IsNullOrEmpty(menu.ToolTipText))
            {
                menu.ToolTipText = toolStrip.ToolTipText;
            }
            if (menu.Image == null)
            {
                menu.Image = toolStrip.Image;
            }
            if (string.IsNullOrEmpty(menu.Text))
            {
                menu.Text = toolStrip.Text;
            }
            menu.Enabled = toolStrip.Enabled;
            toolStrip.EnabledChanged += new EventHandler(toolStrip_EnabledChanged);
            toolStrip.VisibleChanged += new EventHandler(toolStrip_VisibleChanged);

            ToolStripDropDownItem dropDownItem = toolStrip as ToolStripDropDownItem;

            if (dropDownItem == null)
            {
                menu.Click += new EventHandler(menu_Click);
            }
            else
            {
                foreach (ToolStripItem subTsb in dropDownItem.DropDownItems)
                {
                    ToolStripMenuItem subMenu = new ToolStripMenuItem();
                    subMenu.Name = subTsb.Name + "toolStripMenuItem";
                    subMenu.Size = new System.Drawing.Size(172, 22);

                    menu.DropDownItems.Add(subMenu);

                    Associate(subMenu, subTsb);
                }
            }
        }

        void toolStrip_VisibleChanged(object sender, EventArgs e)
        {
            ToolStripItem toolStrip = sender as ToolStripItem;
            m_buf2[toolStrip].Visible = toolStrip.Visible;
        }

        void menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            m_buf1[menu].PerformClick();
        }

        void toolStrip_EnabledChanged(object sender, EventArgs e)
        {
            ToolStripItem toolStrip = sender as ToolStripItem;
            m_buf2[toolStrip].Enabled = toolStrip.Enabled;
        }
    }
}
