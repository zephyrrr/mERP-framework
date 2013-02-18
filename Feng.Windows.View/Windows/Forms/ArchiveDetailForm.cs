using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xceed.Validation;
using Feng.Grid;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 详细编辑窗体
    /// </summary>
    public partial class ArchiveDetailForm : MyChildForm, IArchiveDetailForm, IControlManagerContainer, IDisplayManagerContainer, ILayoutControl, IProfileLayoutControl, IGridNamesContainer //PositionPersistForm
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
                if (m_cm != null)
                {
                    m_cm.EditCanceled -= new EventHandler(cm_EditCanceled);
                }
                if (m_dm != null)
                {
                    m_dm.Dispose();
                }

                base.RevertMergeToolStrip(this.toolStrip1);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ArchiveDetailForm()
        {
            InitializeComponent();

            // 作为主窗体的下方明细窗体
            this.menuStrip1.Visible = false;

            base.MergeToolStrip(this.toolStrip1);

            this.tsbSaveAndGrid.Visible = false;
            this.tsmSaveAndGrid.Visible = false;

            this.tsbViewHideMasterWindow.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExpand.png").Reference;
            this.tsbAddNew.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNew.png").Reference;
            this.tsbEdit.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconEdit.png").Reference;
            this.tsbDelete.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
            this.tsbSetup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSetup.png").Reference;
            this.tsbSaveAndGrid.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSaveAndReturn.png").Reference;
            this.tsbSaveAndNew.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSaveAndNew.png").Reference;
            this.tsbSave.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSave.png").Reference;
            this.tsbCancelAndReturn.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconCancel.png").Reference;
            this.tsbClose.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconClose.png").Reference;
            this.tsbFirst.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFirst.png").Reference;
            this.tsbPrevious.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrevious.png").Reference;
            this.tsbNext.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNext.png").Reference;
            this.tsbLast.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconLast.png").Reference;

            this.tsmDelete.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
            this.tsmEdit.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconEdit.png").Reference;
            this.tsmAddNew.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNew.png").Reference;
            this.tsmSaveAndGrid.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSaveAndReturn.png").Reference;
            this.tsmSaveAndNew.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSaveAndNew.png").Reference;
            this.tsmSave.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSave.png").Reference;
            this.tsmCancelAndReturn.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconCancel.png").Reference;
            this.tsmClose.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconClose.png").Reference;
            this.tsmDelete.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
            this.tsmPresetLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPreset.png").Reference;
            this.tsmLoadLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconOpen.png").Reference;
            this.tsmSaveLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSave.png").Reference;
            this.tsmResetLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReset.png").Reference;

            tsmList.Visible = false;
        }

        //private ArchiveSeeForm m_parentArchiveSeeForm;
        ///// <summary>
        ///// Parent ArchiveSeeForm
        ///// </summary>
        //public ArchiveSeeForm ParentArchiveForm
        //{
        //    get { return m_parentArchiveSeeForm; }
        //    set { m_parentArchiveSeeForm = value; }
        //}

        /// <summary>
        /// ToolStripSaveButton
        /// </summary>
        protected ToolStripButton ToolStripSaveButton
        {
            get { return tsbSave; }
        }

        private string m_controlGroupName;

        /// <summary>
        /// GridName
        /// 虽然这里没有Grid，但一些配置以GridName为准。同读入数据控件时的ControlGroupName
        /// </summary>
        public string GridName
        {
            get { return m_controlGroupName; }
        }

        public string[] GridNames
        {
            get { return new string[] { m_controlGroupName }; }
        }

        /// <summary>
        /// 控制管理器
        /// </summary>
        public IControlManager ControlManager
        {
            get { return m_cm; }
        }

        /// <summary>
        /// 查看管理器
        /// </summary>
        public IDisplayManager DisplayManager
        {
            get { return m_dm; }
        }

        /// <summary>
        /// 如果作为MdiChild显示，需要设置的一些属性
        /// </summary>
        internal void SetAsMdiChild()
        {
            // 作为子窗体
            base.MergeMenu(this.menuStrip1);

            this.tsbViewHideMasterWindow.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public new System.Windows.Forms.DialogResult ShowDialog(IWin32Window owner)
        {
            this.tsmSaveAndNew.Visible = true;
            return base.ShowDialog(owner);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveDetailForm(IDisplayManager dm, string controlGroupName)
            : this()
        {
            SetDisplayManager(dm, controlGroupName);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveDetailForm(IWindowControlManager cm, string controlGroupName)
            : this()
        {
            SetControlMananger(cm, controlGroupName);
        }

        internal void SetControlMananger(IWindowControlManager cm, string controlGroupName)
        {
            if (m_cm != null)
            {
                m_cm.DisplayManager.PositionChanging += new CancelEventHandler(DisplayManager_PositionChanging);
                m_cm.ListChanged += new ListChangedEventHandler(ControlManager_ListChanged);
            }
            SetDisplayManager(cm.DisplayManager, controlGroupName);
            this.m_cm = cm;
            m_cm.EditCanceled += new EventHandler(cm_EditCanceled);
            m_cm.DisplayManager.PositionChanging += new CancelEventHandler(DisplayManager_PositionChanging);
            m_cm.ListChanged += new ListChangedEventHandler(ControlManager_ListChanged);

            if (m_cm != null)
            {
                m_cm.AllowInsert = Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(m_controlGroupName).AllowInsert);
                m_cm.AllowEdit = Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(m_controlGroupName).AllowEdit);
                m_cm.AllowDelete = Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(m_controlGroupName).AllowDelete);
            }
        }

        internal void SetDisplayManager(IDisplayManager dm, string controlGroupName)
        {
            if (m_dm != null)
            {
                m_dm.PositionChanged -= new EventHandler(DisplayManager_PositionChanged);
                m_dm.SearchManager.DataLoaded -= new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);
            }
            this.m_dm = dm;
            m_controlGroupName = controlGroupName;
            m_dm.PositionChanged += new EventHandler(DisplayManager_PositionChanged);
            m_dm.SearchManager.DataLoaded += new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);
        }

        protected override void Form_Load(object sender, System.EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            base.Form_Load(sender, e);

            if (this.ControlManager != null)
            {
                // have set in constructor
                //this.ControlManager.AllowInsert = Authority.AuthorizeByRule(GridSettingInfoCollection.Instance[m_controlGroupName].GridInfos[0].AllowInsert);
                //this.ControlManager.AllowEdit = Authority.AuthorizeByRule(GridSettingInfoCollection.Instance[m_controlGroupName].GridInfos[0].AllowEdit);
                //this.ControlManager.AllowDelete = Authority.AuthorizeByRule(GridSettingInfoCollection.Instance[m_controlGroupName].GridInfos[0].AllowDelete);

                if (!this.ControlManager.AllowInsert
                    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(m_controlGroupName).AllowOperationInsert))
                {
                    this.tsbAddNew.Visible = false;
                    this.tsmAddNew.Visible = false;

                    this.tsbSaveAndNew.Visible = false;
                    this.tsmSaveAndNew.Visible = false;
                }
                if (!this.ControlManager.AllowEdit
                    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(m_controlGroupName).AllowOperationEdit))
                {
                    this.tsbEdit.Visible = false;
                    this.tsmEdit.Visible = false;
                }
                if (!this.ControlManager.AllowDelete
                    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(m_controlGroupName).AllowOperationDelete))
                {
                    this.tsbDelete.Visible = false;
                    this.tsmDelete.Visible = false;
                }

                // 不能根据tsmAdd和tsmEdit.Visible。 有些不能直接编辑，但通过代码用按钮可以编辑，保存也要显示
                if (!this.ControlManager.AllowInsert && !this.ControlManager.AllowEdit)
                {
                    this.tsbSave.Visible = false;
                    this.tsmSave.Visible = false;
                    this.tsbCancelAndReturn.Visible = false;
                    this.tsbCancelAndReturn.Visible = false;
                }
            }
            else
            {
                this.tsbAddNew.Visible = false;
                this.tsmAddNew.Visible = false;
                this.tsbSaveAndNew.Visible = false;
                this.tsmSaveAndNew.Visible = false;
                this.tsbEdit.Visible = false;
                this.tsmEdit.Visible = false;
                this.tsbDelete.Visible = false;
                this.tsmDelete.Visible = false;
                this.tsbSave.Visible = false;
                this.tsmSave.Visible = false;
                this.tsbCancelAndReturn.Visible = false;
                this.tsbCancelAndReturn.Visible = false;
            }

            if (!this.tsmAddNew.Visible && !this.tsmEdit.Visible && !this.tsmDelete.Visible)
            {
                this.tssViewHideMasterWindow.Visible = false;
                this.tssmOpeation.Visible = false;
            }
        }

        void cm_EditCanceled(object sender, EventArgs e)
        {

        }

        private void ResetStatusButton()
        {
            tsbFirst.Enabled = true;
            tsbLast.Enabled = true;
            tsbNext.Enabled = true;
            tsbPrevious.Enabled = true;
            tsbSave.Enabled = true;
            tsbSaveAndGrid.Enabled = true;
            tsbSaveAndNew.Enabled = true;
            tsbAddNew.Enabled = true;
            tsbClose.Enabled = true;
            tsbDelete.Enabled = true;
            tsbEdit.Enabled = true;
        }

        public static void ResetStatusDataControl(IWindowControlManager cm)
        {
            if (cm == null)
                return;

            ResetStatusDataControl(cm.DisplayManager);
            foreach (IDataControl dc in cm.DisplayManager.DataControls)
            {
                cm.RemoveValidation(dc.Name);
            }
            cm.ControlCheckExceptionProcess.ClearAllError();
        }
        public static void ResetStatusDataControl(IDisplayManager dm)
        {
            if (dm == null)
                return;

            foreach (IDataControl dc in dm.DataControls)
            {
                dc.NotNull = false;
                dc.ReadOnly = true;
            }
        }

        ///// <summary>
        ///// 关闭是是隐藏还是正常关闭
        ///// </summary>
        //public bool InvisibleOnClosing
        //{
        //    get;
        //    set;
        //}

        //private StateType m_state;

        void ControlManager_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.DisplayManager.InBatchOperation)
            {
                return;
            }

            try
            {
                this.DisplayManager.BeginBatchOperation();
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        break;
                    case ListChangedType.ItemChanged:
                        if (this.Visible)
                        {
                            UpdateContent();
                        }
                        break;
                    case ListChangedType.ItemDeleted:
                        break;
                    default:
                        throw new NotSupportedException("not supported listChangedType");
                }
            }
            finally
            {
                this.DisplayManager.EndBatchOperation();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void UpdateContent()
        {
            if (!this.Visible)
                return;

            IsClosed = false;
            m_activeClose = false;

            if (m_cm != null)
            {
                //m_state = m_cm.State;
                ResetStatusDataControl(m_cm);
            }
            else
            {
                //m_state = StateType.View;
                ResetStatusDataControl(m_dm);
            }

            ResetStatusButton();

            LoadLayout();

            UpdateStatusButton();

            if (m_cm != null)
            {
                UpdateStatusDataControl(m_cm, m_controlGroupName);
            }
            //// Add List Menu
            //this.tsmList.DropDownItems.Clear();
            //for (int i = 0; i < m_dm.Count; ++i)
            //{
            //    ToolStripMenuItem menuItem = new ToolStripMenuItem(m_dm.GetItem(i).ToString());
            //    menuItem.Click += new EventHandler(menuItem_Click);
            //    menuItem.Tag = i;
            //    this.tsmList.DropDownItems.Add(menuItem);

            //    if (i == m_dm.Position)
            //    {
            //        menuItem.Checked = true;
            //    }
            //}

            this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);

            if (m_cm != null)
            {
                if (m_cm.State == StateType.Add)
                {
                    m_cm.DisplayManager.DataControls.FocusFirstInsertableControl();

                    SetDataControlDefaultValues(m_cm);
                }
                else if (m_cm.State == StateType.Edit)
                {
                    m_cm.DisplayManager.DataControls.FocusFirstEditableControl();
                }
            }
        }

        

        public static void SetDataControlDefaultValues(IControlManager cm)
        {
            try
            {
                foreach (IDataControl dc in cm.DisplayManager.DataControls)
                {
                    GridColumnInfo info = dc.Tag as GridColumnInfo;
                    if (info != null)
                    {
                        if (!string.IsNullOrEmpty(info.DataControlDefaultValue))
                        {
                            object defaultValue = ControlFactory.GetControlDefaultValueByUser(info.DataControlDefaultValue);
                            if (defaultValue != null)
                            {
                                dc.SelectedDataValue = Feng.Utils.ConvertHelper.ChangeType(defaultValue, GridColumnInfoHelper.CreateType(info));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }

        void SearchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            if (IsClosed)
                return;

            m_dm.DisplayCurrent();

            UpdatePositionStatus();
        }

        void DisplayManager_PositionChanging(object sender, CancelEventArgs e)
        {
            e.Cancel = !TryCancelEdit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DisplayManager_PositionChanged(object sender, EventArgs e)
        {
            if (IsClosed)
                return;

            m_dm.DisplayCurrent();

            UpdatePositionStatus();
        }

        private void UpdatePositionStatus()
        {
            tslPosition.Text = (m_dm.Position + 1).ToString() + "/" + m_dm.Count.ToString();

            if (m_cm != null)
            {
                // Add状态也同样可以改变Position
                //if (m_cm.State != StateType.Add) 
                {
                    tsbFirst.Enabled = (m_dm.Position > 0);
                    tsbPrevious.Enabled = (m_dm.Position > 0);
                    tsbNext.Enabled = (m_dm.Position < m_dm.Count - 1);
                    tsbLast.Enabled = (m_dm.Position < m_dm.Count - 1);
                }

                // 设置控件和按钮状态
                UpdateStatusButton();
                //UpdateStatusDataControl(m_cm, m_controlGroupName);
            }
        }

        void menuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            int idx = (int)menuItem.Tag;
            m_dm.Position = idx;

            foreach (ToolStripMenuItem item in tsmList.DropDownItems)
            {
                item.Checked = false;
            }
            menuItem.Checked = true;
        }

        private void UpdateStatusButton()
        {
            if (m_cm != null)
            {
                tsbSave.Enabled = (m_cm.State == StateType.Add || m_cm.State == StateType.Edit);
                tsbSaveAndGrid.Enabled = (m_cm.State == StateType.Add || m_cm.State == StateType.Edit);
                tsbSaveAndNew.Enabled = (m_cm.State == StateType.Add);
                tsbCancelAndReturn.Enabled = (m_cm.State == StateType.Add || m_cm.State == StateType.Edit);

                tsbAddNew.Enabled = (m_cm.State == StateType.View || m_cm.State == StateType.None);
                tsbEdit.Enabled = (m_cm.State == StateType.View || m_cm.State == StateType.None);
                tsbDelete.Enabled = (m_cm.State == StateType.View || m_cm.State == StateType.None);
            }
        }

        public static void UpdateStatusDataControl(IWindowControlManager cm, string controlGroupName)
        {
            if (cm != null)
            {
                cm.UpdateControlsState();

                SetDataControlReadOnly(cm, controlGroupName);

                ClearNotnullIcon();
                AddValidations(cm, controlGroupName);
            }
        }

        public static void SetDataControlReadOnly(IWindowControlManager cm, string controlGroupName)
        {
            if (cm == null || cm.DisplayManager.CurrentItem == null)
            {
                return;
            }

            // column notNull
            if (cm.State == StateType.Add || cm.State == StateType.Edit)
            {
                foreach (IDataControl dc in cm.DisplayManager.DataControls)
                {
                    if (dc.ReadOnly)
                    {
                        continue;
                    }

                    GridColumnInfo columnInfo = dc.Tag as GridColumnInfo;
                    if (columnInfo != null)
                    {
                        dc.ReadOnly = Authority.AuthorizeByRule(columnInfo.ReadOnly);
                    }
                }
            }

            // row and cell notNull
            if (cm.State == StateType.Edit)
            {
                GridRowInfo rowInfo = ADInfoBll.Instance.GetGridRowInfo(controlGroupName);

                bool readOnly = false;
                readOnly = Permission.AuthorizeByRule(rowInfo.ReadOnly, cm.DisplayManager.CurrentItem);
                if (readOnly)
                {
                    // 全部Readonly，则整个设置View，包括DetailGrid
                    // Todo: SetState调用太多次
                    foreach (IDataControl dc in cm.DisplayManager.DataControls)
                    {
                        dc.ReadOnly = true;
                    }

                    // DetailGrid is in StateControls
                    foreach (IStateControl sc in cm.StateControls)
                    {
                        sc.SetState(StateType.View);
                    }

                    SetDetailGridsAllowInsertVisible(cm, false);
                }
                else
                {
                    // DataControl Readonly
                    IList<GridCellInfo> cellInfos = ADInfoBll.Instance.GetGridCellInfos(controlGroupName);
                    foreach (GridCellInfo cellInfo in cellInfos)
                    {
                        IDataControl dc = cm.DisplayManager.DataControls[cellInfo.GridColumName];
                        if (dc != null && !dc.ReadOnly)
                        {
                            dc.ReadOnly = Permission.AuthorizeByRule(cellInfo.ReadOnly, cm.DisplayManager.CurrentItem);
                        }
                    }

                    SetDetailGridsReadOnly(cm, rowInfo);

                    SetDetailGridsAllowInsert(cm, rowInfo);
                }
            }
            else if (cm.State == StateType.Add)
            {
                GridRowInfo rowInfo = ADInfoBll.Instance.GetGridRowInfo(controlGroupName);

                SetDetailGridsReadOnly(cm, rowInfo);

                SetDetailGridsAllowInsert(cm, rowInfo);
            }
            else
            {
                SetDetailGridsAllowInsertVisible(cm, false);
            }
        }

        private static void SetDetailGridsReadOnly(IWindowControlManager cm, GridRowInfo rowInfo)
        {
            // DetailGrid
            if (!string.IsNullOrEmpty(rowInfo.DetailGridReadOnly))
            {
                object[] ss = ParamCreatorHelper.TryGetParams(rowInfo.DetailGridReadOnly);

                foreach (object s in ss)
                {
                    //string[] sss = s.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    object[] sss = ParamCreatorHelper.TryGetParams((string)s);
                    // 第一个是GridName，第二个是权限
                    if (sss.Length == 2)
                    {
                        foreach (IStateControl sc in cm.StateControls)
                        {
                            ArchiveUnboundGrid detailGrid = sc as ArchiveUnboundGrid;
                            if (detailGrid != null && detailGrid.GridName == (string)sss[0])
                            {
                                detailGrid.ReadOnly = Permission.AuthorizeByRule((string)sss[1], cm.DisplayManager.CurrentItem);

                                // 不用重新设置，RowCell的ReadOnly在读取数据后就设置了。
                                // 在SetDataBinding时，凡是ReadOnly=true的都设置好。
                                // 不需要，无DataRow，不用设置。 Column.ReadOnly已经设置好了
                                //if (!detailGrid.ReadOnly)
                                //{
                                //    // 设置detailGrid的Row，Cell属性
                                //    ArchiveUnboundGrid.SetGridRowCellPermissions(detailGrid);
                                //}
                            }
                        }
                    }
                    else if (sss.Length == 1)
                    {
                        foreach (IStateControl sc in cm.StateControls)
                        {
                            ArchiveUnboundGrid detailGrid = sc as ArchiveUnboundGrid;
                            if (detailGrid != null)
                            {
                                detailGrid.ReadOnly = Permission.AuthorizeByRule((string)sss[0], cm.DisplayManager.CurrentItem);
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("DetailGridReadOnly must have 2 or 1 param!");
                    }
                }
            }
        }

        private static void SetDetailGridsAllowInsert(IWindowControlManager cm, GridRowInfo rowInfo)
        {
            // DetailGrid AllowInsert
            if (!string.IsNullOrEmpty(rowInfo.DetailGridAllowInsert))
            {
                object[] ss = ParamCreatorHelper.TryGetParams(rowInfo.DetailGridAllowInsert);

                foreach (object s in ss)
                {
                    //string[] sss = s.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    object[] sss = ParamCreatorHelper.TryGetParams((string)s);
                    // 第一个是GridName，第二个是权限
                    if (sss.Length == 2)
                    {
                        foreach (IStateControl sc in cm.StateControls)
                        {
                            ArchiveUnboundGrid detailGrid = sc as ArchiveUnboundGrid;
                            if (detailGrid != null && detailGrid.GridName == (string)sss[0])
                            {
                                if (detailGrid.InsertionRow != null)
                                {
                                    detailGrid.AllowInnerInsert = Permission.AuthorizeByRule((string)sss[1], cm.DisplayManager.CurrentItem);
                                    detailGrid.InsertionRow.Visible = detailGrid.AllowInnerInsert;
                                }
                            }
                        }
                    }
                    else if (sss.Length == 1)
                    {
                        foreach (IStateControl sc in cm.StateControls)
                        {
                            ArchiveUnboundGrid detailGrid = sc as ArchiveUnboundGrid;
                            if (detailGrid != null)
                            {
                                if (detailGrid.InsertionRow != null)
                                {
                                    detailGrid.AllowInnerInsert = Permission.AuthorizeByRule((string)sss[0], cm.DisplayManager.CurrentItem);
                                    detailGrid.InsertionRow.Visible = detailGrid.AllowInnerInsert;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("DetailGridAllowInsert must have 2 or 1 param!");
                    }
                }
            }
            else
            {
                SetDetailGridsAllowInsertVisible(cm, true);
            }
        }

        private static void SetDetailGridsAllowInsertVisible(IWindowControlManager cm, bool visible)
        {
            foreach (IStateControl sc in cm.StateControls)
            {
                ArchiveUnboundGrid detailGrid = sc as ArchiveUnboundGrid;
                if (detailGrid != null)
                {
                    if (detailGrid.InsertionRow != null)
                    {
                        detailGrid.InsertionRow.Visible = visible;
                    }
                }
            }
        }

        private static System.Windows.Forms.ErrorProvider m_notNullIconProvider = null;
        private static void ShowNotnullIcon(IWindowControl wc)
        {
            if (wc == null)
                return;

            //if (m_notNullIconProvider == null)
            //{
            //    m_notNullIconProvider = new System.Windows.Forms.ErrorProvider();
            //    m_notNullIconProvider.Icon = Feng.PdnResources.GetIconFromImage(Feng.Windows.ImageResource.Get("Icons.asterisk_red.png").Reference);
            //    m_notNullIconProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            //}
            //m_notNullIconProvider.SetIconAlignment(wc.Control, ErrorIconAlignment.MiddleRight);
            //m_notNullIconProvider.SetIconPadding(wc.Control, 0);
            //m_notNullIconProvider.SetError(wc.Control, "必须填写");
        }
        private static void ClearNotnullIcon()
        {
            if (m_notNullIconProvider != null)
            {
                m_notNullIconProvider.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="gridName"></param>
        public static void AddValidations(IWindowControlManager cm, string gridName)
        {
            if (cm == null)
            {
                return;
            }

            // column notNull
            if (cm.State == StateType.Add || cm.State == StateType.Edit)
            {
                foreach (IDataControl dc in cm.DisplayManager.DataControls)
                {
                    if (dc.ReadOnly)
                    {
                        continue;
                    }

                    GridColumnInfo columnInfo = dc.Tag as GridColumnInfo;
                    if (columnInfo != null)
                    {
                        if (!dc.ReadOnly)
                        {
                            dc.NotNull = Authority.AuthorizeByRule(columnInfo.NotNull);

                            ValidationCriterion cri = ArchiveGridHelper.GetValidationCriterion(columnInfo, cm);
                            if (cri != null)
                            {
                                cm.SetValidation(dc.Name, cri);
                            }

                            if (dc.NotNull)
                            {
                                ShowNotnullIcon(dc as IWindowControl);
                            }
                        }
                    }
                }
            }

            // row and cell notNull
            if (cm.State == StateType.Edit)
            {
                if (cm.DisplayManager.CurrentItem == null)
                    return;

                IList<GridCellInfo> cellInfos = ADInfoBll.Instance.GetGridCellInfos(gridName);

                foreach (GridCellInfo cellInfo in cellInfos)
                {
                    IDataControl dc = cm.DisplayManager.DataControls[cellInfo.GridColumName];
                    if (dc != null && !dc.ReadOnly)
                    {
                        if (!dc.ReadOnly)
                        {
                            dc.NotNull = Permission.AuthorizeByRule(cellInfo.NotNull, cm.DisplayManager.CurrentItem);

                            GridColumnInfo columnInfo = (dc as Control).Tag as GridColumnInfo;

                            ValidationCriterion cri = ArchiveGridHelper.GetValidationCriterion(columnInfo, cm);
                            if (cri != null)
                            {
                                cm.SetValidation(dc.Name, cri);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 是否能变动Position（现有记录有改动的情况下要询问）
        /// </summary>
        /// <returns></returns>
        private bool TryCancelEdit()
        {
            if (m_cm == null)
            {
                return true;
            }

            return m_cm.TryCancelEdit();
        }

        private IWindowControlManager m_cm;
        private IDisplayManager m_dm;

        private void tsbSaveAndGrid_Click(object sender, EventArgs e)
        {
            if (DoSave())
            {
                m_cm.EndEdit(true);
                if (m_cm.State == StateType.View)
                {
                    m_activeClose = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 保存。 有时候需自定义保存，此时重载此方法
        /// </summary>
        /// <returns></returns>
        public virtual bool DoSave()
        {
            // 验证控件有效值（有些控件需移出焦点后对其他控件产生影响，如果这里不Validate，会先执行Save）
            this.ValidateChildren();

            return DoSaveS(m_cm);
        }

        public static bool DoSaveS(IControlManager cm)
        {
            if (cm.SaveCurrent())
            {
                return cm.EndEdit(true);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool DoCancel()
        {
            return DoDefaultCancel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool DoCancelS(IControlManager cm)
        {
            return ArchiveOperationForm.TryCancelEdit(cm);
        }
        private bool DoDefaultCancel()
        {
            if (DoCancelS(this.ControlManager))
            {
                return true;
            }
            return false;
        }

        private void tsbSaveAndNew_Click(object sender, EventArgs e)
        {
            if (DoSave())
            {
                if (m_cm.State == StateType.View)
                {
                    m_cm.AddNew();

                    m_dm.DisplayCurrent();

                    UpdateStatusButton();
                    UpdateStatusDataControl(m_cm, m_controlGroupName);

                    m_dm.DataControls.FocusFirstInsertableControl();
                    SetDataControlDefaultValues(m_cm);
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (DoSave())
            {
                if (m_cm.State == StateType.View)
                {
                    UpdateStatusButton();
                    UpdateStatusDataControl(m_cm, m_controlGroupName);

                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public virtual bool DoCancel()
        //{
        //    return TryCancelEdit();
        //}

        private void tsbCancelAndReturn_Click(object sender, EventArgs e)
        {
            bool ret = false;
            ret = DoCancel();
            UpdateContent();

            if (ret)
            {
                UpdateStatusButton();
                UpdateStatusDataControl(m_cm, m_controlGroupName);

                // 因为不是在弹出窗口，不用关闭窗体
                //m_activeClose = true;
                this.DialogResult = DialogResult.Cancel;
                //this.Close();
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void Close(bool force)
        {
            if (force)
            {
                m_activeClose = true;
                this.Close();
            }
            else
            {
                if (TryCancelEdit())
                {
                    //UpdateStatus();

                    //ArchiveSeeForm masterForm = this.ParentArchiveForm;
                    //masterForm.ToolStrip.Visible = true;

                    m_activeClose = true;
                    //this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }
        private void tsbFirst_Click(object sender, EventArgs e)
        {
            if (m_dm.Position > 0)
            {
                if (TryCancelEdit())
                {
                    m_dm.Position = 0;

                    if (m_cm != null)
                    {
                        UpdateStatusButton();
                        UpdateStatusDataControl(m_cm, m_controlGroupName);
                    }
                }
            }
        }

        private void tsbPrevious_Click(object sender, EventArgs e)
        {
            if (m_dm.Position > 0)
            {
                if (TryCancelEdit())
                {
                    m_dm.Position--;

                    if (m_cm != null)
                    {
                        UpdateStatusButton();
                        UpdateStatusDataControl(m_cm, m_controlGroupName);
                    }
                }
            }
        }

        private void tsbNext_Click(object sender, EventArgs e)
        {
            if (m_dm.Position < m_dm.Count - 1)
            {
                if (TryCancelEdit())
                {
                    m_dm.Position++;

                    if (m_cm != null)
                    {
                        UpdateStatusButton();
                        UpdateStatusDataControl(m_cm, m_controlGroupName);
                    }
                }
            }
        }

        private void tsbLast_Click(object sender, EventArgs e)
        {
            if (m_dm.Position != m_dm.Count - 1)
            {
                if (TryCancelEdit())
                {
                    m_dm.Position = m_dm.Count - 1;

                    if (m_cm != null)
                    {
                        UpdateStatusButton();
                        UpdateStatusDataControl(m_cm, m_controlGroupName);
                    }
                }
            }
        }

        private bool m_activeClose;

        /// <summary>
        /// 是否已关闭（包括手工关闭但Invisible）
        /// </summary>
        protected bool IsClosed
        {
            get;
            set;
        }

        /// <summary>
        /// Form_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (m_activeClose)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = !TryCancelEdit();
                }

                if (!e.Cancel)
                {
                    IsClosed = true;

                    //if (InvisibleOnClosing)
                    {
                        e.Cancel = true;
                        if (m_dm != null)
                        {
                            m_dm.DataControls.Clear();
                        }
                        this.Hide();
                    }
                    //else
                    //{
                    //}
                }
            }

            base.Form_Closing(sender, e);
        }

        protected override void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_dm.SearchManager.DataLoaded -= new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);
            m_dm.PositionChanged -= new EventHandler(DisplayManager_PositionChanged);
            m_dm.PositionChanging -= new CancelEventHandler(DisplayManager_PositionChanging);
            if (m_cm != null)
            {
                m_cm.ListChanged -= new ListChangedEventHandler(ControlManager_ListChanged);
            }

            base.Form_FormClosed(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AssociateMenuToToolStrip()
        {
            m_assoMenu2ToolStrip.Associate(tsmSaveAndGrid, tsbSaveAndGrid);
            m_assoMenu2ToolStrip.Associate(tsmSaveAndNew, tsbSaveAndNew);
            m_assoMenu2ToolStrip.Associate(tsmSave, tsbSave);
            m_assoMenu2ToolStrip.Associate(tsmCancelAndReturn, tsbCancelAndReturn);
            m_assoMenu2ToolStrip.Associate(tsmClose, tsbClose);

            m_assoMenu2ToolStrip.Associate(tsmFirst, tsbFirst);
            m_assoMenu2ToolStrip.Associate(tsmPrevious, tsbPrevious);
            m_assoMenu2ToolStrip.Associate(tsmNext, tsbNext);
            m_assoMenu2ToolStrip.Associate(tsmLast, tsbLast);

            m_assoMenu2ToolStrip.Associate(tsbSave, tsbAddNew, false);
            m_assoMenu2ToolStrip.Associate(tsbSave, tsbEdit, false);
            m_assoMenu2ToolStrip.Associate(tsbSave, tsbDelete, false);

            m_assoMenu2ToolStrip.Associate(tsmAddNew, tsbAddNew);
            m_assoMenu2ToolStrip.Associate(tsmDelete, tsbDelete);
            m_assoMenu2ToolStrip.Associate(tsmEdit, tsbEdit);

            base.AssociateMenuToToolStrip();
        }

        private void tsmSearch_Click(object sender, EventArgs e)
        {
            ISearchExpression se = null;

            foreach (IDataControl dc in m_dm.DataControls)
            {
                if (dc.SelectedDataValue == null)
                    continue;
                if (se == null)
                {
                    se = SearchExpression.Eq(dc.Name, dc.SelectedDataValue);
                }
                else
                {
                    se = SearchExpression.And(se, SearchExpression.Eq(dc.Name, dc.SelectedDataValue));
                }
            }
            if (se == null)
            {
                ServiceProvider.GetService<IMessageBox>().ShowWarning("请至少指定一项查找信息！");
                return;
            }

            m_dm.SearchManager.LoadData(se, null);
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            if (this.ParentForm is ArchiveOperationForm)
            {
                (this.ParentForm as ArchiveOperationForm).DoAdd();
            }
            else
            {
                ArchiveOperationForm.DoAddS(this.ControlManager);
                UpdateContent();
            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (this.ParentForm is ArchiveOperationForm)
            {
                (this.ParentForm as ArchiveOperationForm).DoEdit();
            }
            else
            {
                ArchiveOperationForm.DoEditS(this.ControlManager, m_controlGroupName);
                UpdateContent();
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (this.ParentForm is ArchiveOperationForm)
            {
                (this.ParentForm as ArchiveOperationForm).DoDelete();
            }
            else
            {
                ArchiveOperationForm.DoDeleteS(this.ControlManager, m_controlGroupName);
            }
        }

        //public void SetWindowState(WindowMasterDetailState? windowState, bool? showMenu)
        //{
        //    if (windowState.HasValue)
        //    {
        //        if ((windowState & WindowMasterDetailState.EnableViewMaster) != WindowMasterDetailState.EnableViewMaster)
        //        {
        //            tsbViewHideMasterWindow_Click(tsbViewHideMasterWindow, System.EventArgs.Empty);
        //            tsbViewHideMasterWindow.Visible = false;
        //        }
        //    }
        //    if (showMenu.HasValue && !showMenu.Value)
        //    {
        //        menuStrip1.Visible = false;
        //        toolStrip1.Visible = false;
        //    }
        //}

        private void tsbViewHideMasterWindow_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                IArchiveMasterForm masterForm = this.ParentForm as IArchiveMasterForm;
                masterForm.SelfVisible = !tsbViewHideMasterWindow.Checked;
            }
        }

        void tsbSetup_DropDownOpening(object sender, System.EventArgs e)
        {
            if (tsmPresetLayout.DropDownItems.Count == 0)
            {
                string[] folders = new string[] { DefaultDataDirectory };

                foreach (string folder in folders)
                {
                    if (!System.IO.Directory.Exists(folder))
                    {
                        continue;
                    }
                    foreach (string fileName in System.IO.Directory.GetFiles(folder, "*.xmlc"))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = System.IO.Path.GetFileName(fileName).Replace(".xmlc", "");
                        item.Tag = fileName;
                        item.Click += new EventHandler(tsmPresetSubitem_Click);
                        tsmPresetLayout.DropDownItems.Add(item);
                    }
                }

                if (tsmPresetLayout.DropDownItems.Count == 0)
                {
                    tsmPresetLayout.Visible = false;
                }
            }
        }
        private void tsmPresetSubitem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string fileName = item.Tag.ToString();

            AMS.Profile.IProfile profile = new AMS.Profile.Xml(fileName);
            this.LoadLayout(profile);
        }

        private void tsmLoadLayout_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = this.DefaultDataDirectory;
            openFileDialog1.Filter = "配置文件 (*.xmlc)|*.xmlc";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AMS.Profile.IProfile profile = new AMS.Profile.Xml(openFileDialog1.FileName);
                LoadLayout(profile);
            }
            openFileDialog1.Dispose();
        }

        private void tsmSaveLayout_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = this.DefaultDataDirectory;
            saveFileDialog1.Filter = "配置文件 (*.xmlc)|*.xmlc";
            //saveFileDialog1.Title = "保存";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AMS.Profile.IProfile profile = new AMS.Profile.Xml(saveFileDialog1.FileName);
                SaveLayout(profile);
            }
            saveFileDialog1.Dispose();
        }

        private void tsmResetLayout_Click(object sender, EventArgs e)
        {
            foreach (IDataControl dc in m_dm.DataControls)
            {
                GridColumnInfo info = dc.Tag as GridColumnInfo;

                if (info == null || (!string.IsNullOrEmpty(info.DataControlType)
                    && Authority.AuthorizeByRule(info.DataControlVisible)))
                {
                    dc.Available = true;
                }
                else
                {
                    dc.Available = false;
                }
            }
        }

        /// <summary>
        /// DefaultDataDirectory
        /// </summary>
        public virtual string DefaultDataDirectory
        {
            get
            {
                string dirPath = SystemDirectory.UserDataDirectory + (this.Name + "\\") + (this.m_controlGroupName + "\\");
                return dirPath;
            }
        }


        public string LayoutFilePath
        {
            get
            {
                return (this.Name + "\\") + (this.m_controlGroupName + "\\") + m_layoutDefaultFileName;
            }
        }

        private static string m_layoutDefaultFileName = "system.xmlc.default";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ILayoutControl.LoadLayout()
        {
            return LayoutControlExtention.LoadLayout(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ILayoutControl.SaveLayout()
        {
            return LayoutControlExtention.SaveLayout(this);
        }

        /// <summary>
        /// LoadLayout
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool LoadLayout(AMS.Profile.IProfile profile)
        {
            string sectionName = "ArchiveDetailForm." + "." + this.Name + "." + m_controlGroupName + ".Layout";

            string s = profile.GetValue(sectionName, "DataControls", "");
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            string[] dcs = s.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string dc in dcs)
            {
                string[] ss = dc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length != 2)
                {
                    continue;
                }
                if (m_dm.DataControls[ss[0]] == null)
                {
                    continue;
                }

                GridColumnInfo info = m_dm.DataControls[ss[0]].Tag as GridColumnInfo;
                if (m_dm.DataControls[ss[0]] != null)
                {
                    if (info == null || (!string.IsNullOrEmpty(info.DataControlType)
                        && Authority.AuthorizeByRule(info.DataControlVisible)))
                    {
                        m_dm.DataControls[ss[0]].Available = Convert.ToBoolean(ss[1]);
                    }
                    else
                    {
                        m_dm.DataControls[ss[0]].Available = false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// SaveLayout
        /// </summary>
        /// <param name="profile"></param>
        public bool SaveLayout(AMS.Profile.IProfile profile)
        {
            string sectionName = "ArchiveDetailForm." + "." + this.Name + "." + m_controlGroupName + ".Layout";

            if (m_dm != null && m_dm.DataControls.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < m_dm.DataControls.Count; ++i)
                {
                    sb.Append(m_dm.DataControls[i].Name);
                    sb.Append(",");
                    sb.Append(m_dm.DataControls[i].Available);
                    sb.Append(System.Environment.NewLine);
                }
                profile.SetValue(sectionName, "DataControls", sb.ToString());
            }
            return true;
        }
    }
}