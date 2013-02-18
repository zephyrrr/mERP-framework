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
    public interface IArchiveDetailFormAuto : IArchiveDetailForm
    {
        void ReplaceFlowLayoutPanel(Control control);

        void RemoveControls();

        void AddControls(Control[] controls);

        event EventHandler DataControlsCreated;
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveDetailFormAuto : ArchiveDetailFormWithDetailGrids, IArchiveDetailFormAuto
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                DisposeDataControls(m_dataControlsBuffer);
            }

            base.Dispose(disposing);
        }

        internal void CreateDataControls()
        {
            InternalCreateDataControls(base.DisplayManager, base.ControlManager, this.GridName, this, this.toolTip1);

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

        internal static void DisposeDataControls(IDataControl[] dataControlsBuffer)
        {
            if (dataControlsBuffer != null)
            {
                foreach (IDataControl dc in dataControlsBuffer)
                {
                    LabeledDataControl ldc = dc as LabeledDataControl;
                    if (ldc != null)
                    {
                        INameValueMappingBindingControl bc = ldc.Control as INameValueMappingBindingControl;
                        if (bc != null && !string.IsNullOrEmpty(bc.NameValueMappingName)
                            && !bc.NameValueMappingName.StartsWith("EnumType"))
                        {
                            ldc.MappingEdited -= new EventHandler(ldc_MappingEdited);
                            ldc.MappingReload -= new EventHandler(ldc_MappingReload);
                        }
                    }
                }

                dataControlsBuffer = null;
            }
        }
        internal static void GetSubControls(Dictionary<string, IDataControl> dict, Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                IDataControl dc = child as IDataControl;
                if (dc != null)
                {
                    dict[dc.Name] = dc;
                }
                GetSubControls(dict, child);
            }
        }

        internal static void InternalCreateDataControls(IDisplayManager dm, IControlManager cm, string gridName, IArchiveDetailFormAuto parent, ToolTip toolTip)
        {
            // 创建数据控件
            IList<GridColumnInfo> infos = ADInfoBll.Instance.GetGridColumnInfos(gridName);
            if (infos != null)
            {
                Dictionary<string, IDataControl> nowControls = new Dictionary<string, IDataControl>();
                GetSubControls(nowControls, parent as Control);

                List<Control> addControls = new List<Control>();
                if (infos.Count > 0)
                {
                    foreach (GridColumnInfo info in infos)
                    {
                        IDataControl dc;
                        if (!nowControls.ContainsKey(info.GridColumnName))
                        {
                            if (string.IsNullOrEmpty(info.DataControlType))
                                continue;

                            // SetDataControls in GetDataControl();
                            dc = Feng.Windows.Utils.ControlFactory.GetDataControl(info, dm.Name);
                            if (dc != null)
                            {
                                addControls.Add(dc as Control);
                            }
                        }
                        else
                        {
                            dc = nowControls[info.GridColumnName];
                            dc.Available = true;

                            Feng.Windows.Utils.ControlFactory.SetDataControls(dc as Control, info, dm.Name);
                        }

                        // not process Editable and Insertable in here, should be in DataManger.SetState()
                        //dc.SetState(m_cm.State);

                        dm.DataControls.Add(dc);
                        dc.AvailableChanged -= new EventHandler(dc_AvailableChanged);
                        dc.AvailableChanged += new EventHandler(dc_AvailableChanged);

                        LabeledDataControl ldc = dc as LabeledDataControl;
                        if (ldc != null)
                        {
                            if (toolTip != null)
                            {
                                toolTip.SetToolTip(ldc.Label, string.IsNullOrEmpty(info.Help) ? info.Caption : info.Help);
                            }

                            INameValueMappingBindingControl bc = ldc.Control as INameValueMappingBindingControl;
                            if (bc != null && !string.IsNullOrEmpty(bc.NameValueMappingName)
                                && !bc.NameValueMappingName.StartsWith("EnumType"))
                            {
                                ldc.MappingEdited -= new EventHandler(ldc_MappingEdited);
                                ldc.MappingReload -= new EventHandler(ldc_MappingReload);
                                ldc.MappingEdited += new EventHandler(ldc_MappingEdited);
                                ldc.MappingReload += new EventHandler(ldc_MappingReload);
                            }
                        }
                    }

                    parent.AddControls(addControls.ToArray());
                }
                else
                {
                    // 没有GridColumnInfo
                    foreach (IDataControl dc in nowControls.Values)
                    {
                        dc.AvailableChanged -= new EventHandler(dc_AvailableChanged);
                        dc.AvailableChanged += new EventHandler(dc_AvailableChanged);

                        //AddControl(dc as Control);
                        dc.Available = true;
                        dm.DataControls.Add(dc);
                    }
                }
            }
        }

        private static void dc_AvailableChanged(object sender, EventArgs e)
        {
            IDataControl dc = sender as IDataControl;
            (dc as Control).Visible = dc.Available;
        }

        private static void ldc_MappingReload(object sender, EventArgs e)
        {
            LabeledDataControl ldc = sender as LabeledDataControl;
            IBindingDataValueControl bc = ldc.Control as IBindingDataValueControl;
            bc.ReloadData();
        }

        private static void ldc_MappingEdited(object sender, EventArgs e)
        {
            LabeledDataControl ldc = sender as LabeledDataControl;
            INameValueMappingBindingControl bc = ldc.Control as INameValueMappingBindingControl;
            if (!string.IsNullOrEmpty(bc.NameValueMappingName)
                    && !bc.NameValueMappingName.StartsWith("EnumType"))
            {
                NameValueMappingInfo info = ADInfoBll.Instance.GetNameValueMappingInfo(bc.NameValueMappingName);
                if (info != null && info.EditAction != null)
                {
                    ServiceProvider.GetService<IApplication>().ExecuteAction(info.EditAction.Name);
                }
                else
                {
                    ServiceProvider.GetService<IMessageBox>().ShowWarning("无对应编辑窗口，请在配置信息中配置！");
                }
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        protected ArchiveDetailFormAuto()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormAuto(IWindowControlManager cm, string controlGroupName)
            : base(cm, controlGroupName)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormAuto(IDisplayManager dm, string controlGroupName)
            : base(dm, controlGroupName)
        {
            InitializeComponent();
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
            this.Controls.Remove(this.flowLayoutPanel1);
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