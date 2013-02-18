using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Feng.Windows.Forms
{
    public interface IForm : IWin32Window
    {
        /// <summary>
        /// ToolStrips
        /// </summary>
        ToolStrip ToolStrip { get; }

        void MergeToolStrip(ToolStrip toolStrip);

        void RevertMergeToolStrip(ToolStrip toolStrip);

        /// <summary>
        /// MenuStrip
        /// </summary>
        MenuStrip MenuStrip { get; }

        void MergeMenu(MenuStrip menuStrip);

        void RevertMergeMenu(MenuStrip menuStrip);

        void Show();

        string Text { get; set; }
    }

    /// <summary>
    /// MyForm
    /// </summary>
    public partial class MyForm : Form, IForm
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                this.ControlAdded -= new ControlEventHandler(MyForm_ControlAdded);

                this.Load -= new EventHandler(Form_Load);
                this.FormClosing -= new FormClosingEventHandler(Form_Closing);

                //while (m_menuStrips.Count > 0)
                //{
                //    RevertMergeMenu(m_menuStrips[m_menuStrips.Count - 1]);
                //}
                //while (m_toolStrips.Count > 0)
                //{
                //    RevertMergeToolStrip(m_toolStrips[m_toolStrips.Count - 1]);
                //}
                m_assoMenu2ToolStrip.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public MyForm()
        {
            InitializeComponent();

            this.ImeMode = ImeMode.OnHalf;
            this.ControlAdded += new ControlEventHandler(MyForm_ControlAdded);

            this.Load += new EventHandler(Form_Load);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
            this.FormClosed += new FormClosedEventHandler(Form_FormClosed);
        }

        public Control FindControlByName(string name)
        {
            return FindControlByName(name, this);
        }

        private Control FindControlByName(string name, Control parent)
        {
            if (parent.Name == name)
                return parent;
            foreach(Control c in parent.Controls)
            {
                var r = FindControlByName(name, c);
                if (r != null)
                    return r;
            }
            return null;
        }
        /// <summary>
        /// Form_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Form_Load(object sender, EventArgs e)
        {
            LoadLayout();
        }

        /// <summary>
        /// Form_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Form_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SaveLayout();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        /// <summary>
        /// Form_FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private static void EnumerateControls(Control parent, Action<ILayoutControl> action)
        {
            ILayoutControl lc = parent as ILayoutControl;
            if (lc != null)
            {
                action(lc);
            }

            foreach (Control c in parent.Controls)
            {
                EnumerateControls(c, action);
            }
        }

        private void MyForm_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.ImeMode = ImeMode.Inherit;
        }

        private IList<ToolStrip> m_toolStrips = new List<ToolStrip>();
        private IList<MenuStrip> m_menuStrips = new List<MenuStrip>();

        /// <summary>
        /// 最终的工具栏
        /// </summary>
        public ToolStrip ToolStrip
        {
            get 
            {
                if (m_toolStrips.Count > 0)
                {
                    return m_toolStrips[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 最终的菜单栏
        /// </summary>
        public MenuStrip MenuStrip
        {
            get 
            {
                if (m_menuStrips.Count > 0)
                {
                    return m_menuStrips[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// ReverseMergeMenu
        /// </summary>
        /// <param name="menuStrip"></param>
        public void RevertMergeMenu(MenuStrip menuStrip)
        {
            if (this.MenuStrip != null && menuStrip != this.MenuStrip)
            {
                ToolStripManager.RevertMerge(this.MenuStrip, menuStrip);
                menuStrip.Visible = true;
            }
            m_menuStrips.Remove(menuStrip);
        }

        /// <summary>
        /// MergeToolStrip
        /// </summary>
        /// <param name="toolStrip"></param>
        public void RevertMergeToolStrip(ToolStrip toolStrip)
        {
            if (this.ToolStrip != null && toolStrip != this.ToolStrip)
            {
                ToolStripManager.RevertMerge(this.ToolStrip, toolStrip);
                toolStrip.Visible = true;
            }
            m_toolStrips.Remove(toolStrip);
        }

        /// <summary>
        /// MergeMenu
        /// MenuStrip[0] 是作为子窗体的主菜单，合并到Mdi窗体中
        /// </summary>
        /// <param name="menuStrip"></param>
        public void MergeMenu(MenuStrip menuStrip)
        {
            m_menuStrips.Add(menuStrip);
            if (menuStrip != this.MenuStrip)
            {
                ToolStripManager.Merge(menuStrip, this.MenuStrip);
                menuStrip.Visible = false;
            }
        }

        /// <summary>
        /// MergeToolStrip
        /// </summary>
        /// <param name="toolStrip"></param>
        public void MergeToolStrip(ToolStrip toolStrip)
        {
            m_toolStrips.Add(toolStrip);
            if (toolStrip != this.ToolStrip)
            {
                ToolStripManager.Merge(toolStrip, this.ToolStrip);
                toolStrip.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected AssociateMenuToToolStrip m_assoMenu2ToolStrip = new AssociateMenuToToolStrip();
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void AssociateMenuToToolStrip()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            AssociateMenuToToolStrip();
        }

        public bool SaveLayout()
        {
            EnumerateControls(this, (lc) =>
            {
                try
                {
                    lc.SaveLayout();
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                }
            });
            return true;
        }

        public bool LoadLayout()
        {
            EnumerateControls(this, (lc) =>
            {
                lc.LoadLayout();
            });
            return true;
        }
    }
}