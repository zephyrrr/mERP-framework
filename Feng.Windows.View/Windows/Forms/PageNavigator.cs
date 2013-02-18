using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class PageNavigator : BindingNavigator, IProfileLayoutControl
    {
        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                //this.WireUpTextBox(ref this.pageCountItem, null, new KeyEventHandler(this.OnPositionKey), new EventHandler(this.OnPositionLostFocus));
                this.PageCountItem = null;
                this.PositionItem = null;

                this.BindingSource = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public PageNavigator() :
            base()
        {
            this.components = new System.ComponentModel.Container();
            this.设置SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // 设置SToolStripMenuItem
            // 
            this.设置SToolStripMenuItem.Name = "设置SToolStripMenuItem";
            this.设置SToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.设置SToolStripMenuItem.Text = "设置(&S)";
            this.设置SToolStripMenuItem.Click += new System.EventHandler(this.设置SToolStripMenuItem_Click);

            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
                                                  {
                                                      this.设置SToolStripMenuItem
                                                  });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 114);

            this.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AddStandardItems()
        {
            base.AddStandardItems();

            this.AllCountItem = new ToolStripLabel();
            this.AllCountItem.Name = "bindingNavigatorAllCountItem";
            this.allCountItem.Text = "共0条/每页";
            this.AllCountItem.ToolTipText = "总条数";
            this.AllCountItem.AutoToolTip = false;
            this.Items.Add(AllCountItem);

            this.pageCountItem = new ToolStripTextBox();
            

            this.pageCountItem.Name = "bindingNavigatorPageCountItem";
            this.pageCountItem.Text = "50";
            this.pageCountItem.ToolTipText = "每页条数";
            this.pageCountItem.AutoToolTip = false;
            this.Items.Add(pageCountItem);

            this.pageNameItem = new ToolStripLabel();
            this.pageNameItem.Name = "bindingNavigatorPageNameItem";
            this.pageNameItem.Text = "条";
            this.pageNameItem.ToolTipText = "总条数";
            this.pageNameItem.AutoToolTip = false;
            this.Items.Add(pageNameItem);


            this.MoveFirstItem.Text = "移到第一页";
            this.MovePreviousItem.Text = "移到上一页";
            this.MoveNextItem.Text = "移到下一页";
            this.MoveLastItem.Text = "移到最后一页";
            this.CountItem.Text = "总页数";

            this.MoveFirstItem.ToolTipText = "移到第一页";
            this.MovePreviousItem.ToolTipText = "移到上一页";
            this.MoveNextItem.ToolTipText = "移到下一页";
            this.MoveLastItem.ToolTipText = "移到最后一页";
            this.CountItem.ToolTipText = "总页数";
            this.CountItemFormat = "/ {0}页";

            (this.PositionItem as ToolStripTextBox).Size = new System.Drawing.Size(30, 25);
            (this.pageCountItem as ToolStripTextBox).Size = new System.Drawing.Size(40, 25);
        }

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置SToolStripMenuItem;

        private ToolStripItem allCountItem;
        private ToolStripItem pageCountItem;
        private ToolStripItem pageNameItem;

        private void 设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FindToolWindowSetupForm form = new FindToolWindowSetupForm(BindingSource.FingerManager);
            //if (form.ShowDialog(this) == DialogResult.OK)
            //{
            //    BindingSource.FingerManager.ReloadData();

            //    m_profile.SetValue("SearchManager." + this.BindingSource.FingerManager.ManagerName, "MaxResult",
            //                       BindingSource.FingerManager.MaxResult);
            //}
        }

        private void WireUpTextBox(ref ToolStripItem oldTextBox, ToolStripItem newTextBox, KeyEventHandler keyUpHandler, EventHandler lostFocusHandler)
        {
            if (oldTextBox != newTextBox)
            {
                ToolStripControlHost host = oldTextBox as ToolStripControlHost;
                ToolStripControlHost host2 = newTextBox as ToolStripControlHost;
                if (host != null)
                {
                    host.KeyUp -= keyUpHandler;
                    host.LostFocus -= lostFocusHandler;
                }
                if (host2 != null)
                {
                    host2.KeyUp += keyUpHandler;
                    host2.LostFocus += lostFocusHandler;
                }
                oldTextBox = newTextBox;
                this.RefreshItemsInternal();
            }
        }
        private void WireUpLabel(ref ToolStripItem oldLabel, ToolStripItem newLabel)
        {
            if (oldLabel != newLabel)
            {
                oldLabel = newLabel;
                this.RefreshItemsInternal();
            }
        }

        private void RefreshItemsInternal()
        {
            //if (!this.initializing)
            {
                this.OnRefreshItems();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [TypeConverter(typeof (ReferenceConverter))]
        public ToolStripItem AllCountItem
        {
            get
            {
                if ((this.allCountItem != null) && this.allCountItem.IsDisposed)
                {
                    this.allCountItem = null;
                }
                return this.allCountItem;
            }
            set
            {
                this.WireUpLabel(ref this.allCountItem, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TypeConverter(typeof(ReferenceConverter))]
        public ToolStripItem PageNameItem
        {
            get
            {
                if ((this.pageNameItem != null) && this.pageNameItem.IsDisposed)
                {
                    this.pageNameItem = null;
                }
                return this.pageNameItem;
            }
            set
            {
                this.WireUpLabel(ref this.pageNameItem, value);
            }
        }

        [ TypeConverter(typeof(ReferenceConverter))]
        public ToolStripItem PageCountItem
        {
            get
            {
                if ((this.pageCountItem != null) && this.pageCountItem.IsDisposed)
                {
                    this.pageCountItem = null;
                }
                return this.pageCountItem;
            }
            set
            {
                this.WireUpTextBox(ref this.pageCountItem, value, new KeyEventHandler(this.OnPositionKey), new EventHandler(this.OnPositionLostFocus));
            }
        }

        private void OnPositionKey(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode != Keys.Return)
            {
                if (keyCode != Keys.Escape)
                {
                    return;
                }
            }
            else
            {
                this.AcceptNewPageCount();
                return;
            }
            this.CancelNewPosition();
        }

        private void OnPositionLostFocus(object sender, EventArgs e)
        {
            this.AcceptNewPageCount();
        }

        private void AcceptNewPageCount()
        {
            if ((this.pageCountItem != null))
            {
                int pageCount = BindingSource.SearchManager.MaxResult;
                try
                {
                    pageCount = Convert.ToInt32(this.pageCountItem.Text, System.Globalization.CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
                if (pageCount != BindingSource.SearchManager.MaxResult)
                {
                    this.BindingSource.SearchManager.MaxResult = pageCount;

                    SaveLayout();

                    // 不再自动Reload。如果焦点移除去按Search按钮，会导致Search按钮无效。事件先后关系
                    //this.BindingSource.SearchManager.ReloadData();
                }

                this.RefreshItemsInternal();
            }
        }

        private static string m_layoutDefaultFileName = "system.xmls.default";
        public string LayoutFilePath
        {
            get
            {
                if (this.SearchManager != null)
                {
                    return this.SearchManager.Name + "\\" + m_layoutDefaultFileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool LoadLayout()
        {
            return LayoutControlExtention.LoadLayout(this);
        }

        public bool SaveLayout()
        {
            return LayoutControlExtention.SaveLayout(this);
        }

        public bool LoadLayout(AMS.Profile.IProfile profile)
        {
            if (this.SearchManager != null)
            {
                int r = profile.GetValue("SearchManager." + this.SearchManager.Name, "MaxResult", -1);
                if (r != -1)
                {
                    this.SearchManager.MaxResult = r;
                    return true;
                }
            }
            return false;
        }

        public bool SaveLayout(AMS.Profile.IProfile profile)
        {
            if (this.SearchManager != null)
            {
                if (this.SearchManager.MaxResult != SearchManagerDefaultValue.MaxResult)
                {
                    profile.SetValue("SearchManager." + this.BindingSource.SearchManager.Name, "MaxResult", this.BindingSource.SearchManager.MaxResult);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private ISearchManager SearchManager
        {
            get
            {
                if (this.BindingSource != null)
                {
                    return this.BindingSource.SearchManager;
                }
                else
                {
                    return null;
                }
            }
        }
        private void CancelNewPosition()
        {
            this.RefreshItemsInternal();
        }

        /// <summary>
        /// 
        /// </summary>
        public new PageBindingSource BindingSource
        {
            get { return base.BindingSource as PageBindingSource; }
            set
            {
                if (base.BindingSource == value)
                {
                    return;
                }
                if (base.BindingSource != null)
                {
                    base.BindingSource.ListChanged -= new System.ComponentModel.ListChangedEventHandler(BindingSource_ListChanged);
                }
                base.BindingSource = value;

                if (base.BindingSource != null)
                {
                    base.BindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(BindingSource_ListChanged);

                    this.Enabled = this.BindingSource.SearchManager.EnablePage;
                    this.pageCountItem.Enabled = this.Enabled;
                    this.PositionItem.Enabled = this.Enabled;

                    if (this.Enabled)
                    {
                        LoadLayout();

                        this.PageCountItem.Text = this.BindingSource.SearchManager.MaxResult.ToString();
                    }
                    else
                    {
                        this.pageCountItem.Text = string.Empty;
                    }

                    UpdateStatus();
                }
            }
        }

        private void BindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.Reset)
            {
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            this.AllCountItem.Text = "共" + BindingSource.SearchManager.Count + "条/每页";
                                      //+ BindingSource.FingerManager.MaxResult + "条";
        }
    }
}