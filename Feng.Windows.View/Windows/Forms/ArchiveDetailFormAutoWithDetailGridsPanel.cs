using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveDetailFormAutoWithDetailGridsPanel : ArchiveDetailFormWithDetailGrids, IArchiveDetailFormAuto
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                ArchiveDetailFormAuto.DisposeDataControls(m_dataControlsBuffer);
            }

            base.Dispose(disposing);
        }

        internal void CreateDataControls()
        {
            ArchiveDetailFormAuto.InternalCreateDataControls(base.DisplayManager, base.ControlManager, this.GridName, this, this.toolTip1);

            var dm = this.DisplayManager;
            m_dataControlsBuffer = new IDataControl[dm.DataControls.Count];
            dm.DataControls.CopyTo(m_dataControlsBuffer, 0);

            if (DataControlsCreated != null)
            {
                DataControlsCreated(this, System.EventArgs.Empty);
            }
        }
        public override void UpdateContent()
        {
            if (m_dataControlsBuffer == null)
            {
                CreateDataControls();
            }
            else
            {
                base.DisplayManager.DataControls.AddRange(m_dataControlsBuffer);
            }

            base.UpdateContent();
        }

        private IDataControl[] m_dataControlsBuffer;
        /// <summary>
        /// DataControl 创建完毕事件
        /// DataControl是在Form Visible的时候创建，然后Invisible时从DisplayManager.DataControls 中移除。
        /// 但下次不再重复创建，而是从缓存中重新加入到DisplayManager.DataControls。
        /// 第一次时，DataControl是在Form.Show后，UpdateContent时创建。
        /// 由于不知道什么时候DataControls可用，因此创建此事件
        /// </summary>
        public event EventHandler DataControlsCreated;

        /// <summary>
        /// 
        /// </summary>
        protected ArchiveDetailFormAutoWithDetailGridsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormAutoWithDetailGridsPanel(IDisplayManager dm, string controlGroupName)
            : base(dm, controlGroupName)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormAutoWithDetailGridsPanel(IWindowControlManager cm, string controlGroupName)
            : base(cm, controlGroupName)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            base.Form_Load(sender, e);

            if (this.splitContainer1.LoadLayout())
            {
                this.splitContainer1.SplitterDistance = 160;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            splitContainer1.SaveLayout();

            base.Form_Closing(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        public virtual void AddControls(Control[] controls)
        {
            if (!m_isReplaced)
            {
                this.flowLayoutPanel1.Controls.AddRange(controls);
            }
        }

        /// <summary>
        /// Invislbie全部明细控件
        /// </summary>
        public virtual void RemoveControls()
        {
            this.splitContainer1.Panel1.Controls.Remove(this.flowLayoutPanel1);
            this.splitContainer1.Panel1MinSize = 0;
            this.splitContainer1.Panel1Collapsed = true;
        }

        private bool m_isReplaced = false;
        public void ReplaceFlowLayoutPanel(Control control)
        {
            m_isReplaced = true;

            this.SuspendLayout();

            this.Controls.Remove(this.flowLayoutPanel1);

            control.Dock = System.Windows.Forms.DockStyle.Fill;
            control.Location = new System.Drawing.Point(0, 0);
            control.TabIndex = 2;
            this.Controls.Add(control);
            control.Visible = true;
            control.BringToFront();

            this.ResumeLayout(false);
        }
    }
}
