using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Feng.Grid;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public interface IArhiveOperationMasterForm : IArchiveMasterForm, IControlManagerContainer
    {
        bool DoAdd();
        bool DoEdit();
        bool DoDelete();
    }

    /// <summary>
    /// Grid编辑窗体
    /// </summary>
    public partial class ArchiveOperationForm : ArchiveSeeForm, IArhiveOperationMasterForm, IControlManagerContainer, IDisplayManagerContainer
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
                if (this.ControlManager != null)
                {
                    this.ControlManager.StateChanged -= new EventHandler(ControlManager_StateChanged);
                    this.ControlManager.Dispose();
                }

                base.RevertMergeMenu(this.menuStrip1);
                base.RevertMergeToolStrip(this.toolStrip1);
            }
            
            base.Dispose(disposing);
        }

        private ArchiveOperationForm()
            : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveOperationForm(MyGrid masterGrid)
            : base(masterGrid)
        {
            InitializeComponent();

            base.MergeMenu(this.menuStrip1);
            base.MergeToolStrip(this.toolStrip1);

            this.tsbAddNew.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNew.png").Reference;
            this.tsbEdit.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconEdit.png").Reference;
            this.tsbDelete.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
            this.tsmAddNew.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNew.png").Reference;
            this.tsmEdit.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconEdit.png").Reference;
            this.tsmDelete.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
        }

        /// <summary>
        /// 控制管理器
        /// </summary>
        public virtual IControlManager ControlManager
        {
            get
            {
                IArchiveGrid grid = base.MasterGrid as IArchiveGrid;
                return grid == null ? null : grid.ControlManager;
            }
        }

        /// <summary>
        /// 显示管理器
        /// </summary>
        public override IDisplayManager DisplayManager
        {
            get { return ControlManager == null ? null : ControlManager.DisplayManager; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnControlManagerChanged()
        {
            base.OnDisplayManagerChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, System.EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            base.Form_Load(sender, e);

            if (!this.ControlManager.AllowInsert
                || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.MasterGrid.GridName).AllowOperationInsert))
            {
                this.tsbAddNew.Visible = false;
                this.tsmAddNew.Visible = false;
            }
            if (!this.ControlManager.AllowEdit
                || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.MasterGrid.GridName).AllowOperationEdit))
            {
                this.tsbEdit.Visible = false;
                this.tsmEdit.Visible = false;
            }
            if (!this.ControlManager.AllowDelete
                    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.MasterGrid.GridName).AllowOperationDelete))
            {
                this.tsbDelete.Visible = false;
                this.tsmDelete.Visible = false;
            }
            if (!Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.MasterGrid.GridName).AllowExcelOperation))
            {
                this.tsmImportFromExcel.Visible = false;
            }

            if (this.ArchiveDetailForm == null || !(this.ArchiveDetailForm is IControlManagerContainer))
            {
                this.tsbAddNew.Visible = false;
                this.tsbEdit.Visible = false;

                this.tsmAddNew.Visible = false;
                this.tsmEdit.Visible = false;
            }
            else
            {
                //// 按钮按照DetailForm属性来设置
                //IControlManager cm = this.ArchiveDetailForm.ControlManager;

                //if (!cm.AllowInsert
                //    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.ArchiveDetailForm.GridName).AllowOperationInsert))
                //{
                //    this.tsbAddNew.Visible = false;
                //    this.tsmAddNew.Visible = false;
                //}
                //if (!cm.AllowEdit
                //    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.ArchiveDetailForm.GridName).AllowOperationEdit))
                //{
                //    this.tsbEdit.Visible = false;
                //    this.tsmEdit.Visible = false;
                //}
                //if (!cm.AllowDelete
                //    || !Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.ArchiveDetailForm.GridName).AllowOperationDelete))
                //{
                //    this.tsbDelete.Visible = false;
                //    this.tsmDelete.Visible = false;
                //}

                //if (!Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(this.ArchiveDetailForm.GridName).AllowExcelOperation))
                //{
                //    this.tsmImportFromExcel.Visible = false;
                //}
            }

            if (!this.tsmAddNew.Visible && !this.tsmEdit.Visible && !this.tsmDelete.Visible)
            {
                this.tssOperation.Visible = false;
                this.toolStripSeparator2.Visible = false;
            }

            this.ControlManager.StateChanged += new EventHandler(ControlManager_StateChanged);
        }

        void ControlManager_StateChanged(object sender, EventArgs e)
        {
            switch (this.ControlManager.State)
            {
                case StateType.Add:
                case StateType.Delete:
                case StateType.Edit:
                    tsbAddNew.Enabled = tsbEdit.Enabled = tsbDelete.Enabled = false;
                    //tsmAddNew.Enabled = tsmEdit.Enabled = tsmDelete.Enabled = false;
                    break;
                case StateType.None:
                    tsbAddNew.Enabled = true;
                    tsbEdit.Enabled = tsbDelete.Enabled = false;
                    break;
                case StateType.View:
                    tsbAddNew.Enabled = tsbEdit.Enabled = tsbDelete.Enabled = true;
                    break;
                default:
                    throw new InvalidOperationException("Invalid ControlManager State!");
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //protected override void OnActiveGridChanged()
        //{
        //    //if (MasterGrid != null)
        //    //{
        //    //    MasterGrid.RowSelectorDoubleClick -= new EventHandler(ActiveGrid_RowSelectorDoubleClick);
        //    //    MasterGrid.RowSelectorDoubleClick += new EventHandler(ActiveGrid_RowSelectorDoubleClick);

        //    //    IArchiveGrid archiveGrid = MasterGrid as IArchiveGrid;
        //    //    if (archiveGrid != null && archiveGrid.InsertionRow != null)
        //    //    {
        //    //        archiveGrid.InsertionRow.RowSelector.DoubleClick -= new EventHandler(RowSelector_DoubleClick);
        //    //        archiveGrid.InsertionRow.RowSelector.DoubleClick += new EventHandler(RowSelector_DoubleClick);
        //    //    }
        //    //}

        //    base.OnActiveGridChanged();
        //}

        //private void RowSelector_DoubleClick(object sender, EventArgs e)
        //{
        //    tsbAdd_Click(tsbAddNew, System.EventArgs.Empty);
        //}

        //private void ActiveGrid_RowSelectorDoubleClick(object sender, EventArgs e)
        //{
        //    tsbEdit_Click(tsbEdit, System.EventArgs.Empty);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool DoAdd()
        {
            return DoDefaultAdd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool DoEdit()
        {
            return DoDefaultEdit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool DoDelete()
        {
            return DoDefaultDelete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool DoDefaultAdd()
        {
            if (this.ArchiveDetailForm == null)
            {
                MessageForm.ShowError("您只能通过表格添加行来添加数据！");
                return false;
            }

            IControlManagerContainer cmc = this.ArchiveDetailForm as IControlManagerContainer;
            if (cmc == null)
            {
                MessageForm.ShowError("您不能通过此按钮来添加数据！");
                return false;
            }
            if (DoAddS(cmc.ControlManager))
            {
                ShowArchiveDetailForm();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DoAddS(IControlManager cm)
        {
            cm.AddNew();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            DoDefaultAdd();
        }

        public bool DoDefaultEdit()
        {
            if (this.ArchiveDetailForm == null)
            {
                MessageForm.ShowError("您只能通过表格来修改数据！");
                return false;
            }

            IControlManagerContainer cmc = this.ArchiveDetailForm as IControlManagerContainer;
            if (cmc == null)
            {
                MessageForm.ShowError("您不能通过此按钮来修改数据！");
                return false;
            }
            if (DoEditS(cmc.ControlManager, this.MasterGrid.GridName))
            {
                ShowArchiveDetailForm();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DoEditS(IControlManager cm, string gridName)
        {
            var info = ADInfoBll.Instance.GetGridRowInfo(gridName);
            if (info != null && !string.IsNullOrEmpty(info.AllowEdit)
                && !Permission.AuthorizeByRule(info.AllowEdit, cm.DisplayManager.CurrentItem))
            {
                MessageForm.ShowError("您没有修改此记录的权限！");
                return false;
            }

            cm.EditCurrent();

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            DoDefaultEdit();
        }

        public bool DoDefaultDelete()
        {
            int pos = this.ControlManager.DisplayManager.Position;
            if (this.MasterGrid != null
                && pos < 0 && pos >= this.MasterGrid.DataRows.Count)
            {
                MessageForm.ShowError("当前记录不存在！");
                return false;
            }

            return DoDeleteS(this.ControlManager, this.MasterGrid.GridName);
        }

        public static bool DoDeleteS(IControlManager cm, string gridName)
        {
            var info = ADInfoBll.Instance.GetGridRowInfo(gridName);
            if (info != null && !string.IsNullOrEmpty(info.AllowDelete)
                && !Permission.AuthorizeByRule(info.AllowDelete, cm.DisplayManager.CurrentItem))
            {
                MessageForm.ShowError("您没有删除此记录的权限！");
                return false;
            }

            if (MessageForm.ShowYesNo("当前记录将要被删除，是否继续？", "确认", true))
            {
                return cm.DeleteCurrent();
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            DoDefaultDelete();
        }

        

        

        /// <summary>
        /// 是否能变动Position（现有记录有改动的情况下要询问）
        /// </summary>
        /// <returns></returns>
        internal static bool TryCancelEdit(IControlManager cm)
        {
            if (cm == null)
            {
                return true;
            }

            return cm.TryCancelEdit();
        }

        /// <summary>
        /// 参看记录操作
        /// </summary>
        public override bool DoView()
        {
            //if (this.ControlManager.State != StateType.View
            //    || this.ControlManager.State != StateType.None)
            //{
            //    if (!this.ArchiveDetailForm.TryCancelEdit())
            //        return;
            //}

            return base.DoView();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AssociateMenuToToolStrip()
        {
            m_assoMenu2ToolStrip.Associate(tsmAddNew, tsbAddNew);
            m_assoMenu2ToolStrip.Associate(tsmDelete, tsbDelete);
            m_assoMenu2ToolStrip.Associate(tsmEdit, tsbEdit);

            base.AssociateMenuToToolStrip();
        }

        private void tsmImportFromExcel_Click(object sender, EventArgs e)
        {
            if (this.MasterGrid is IArchiveGrid)
            {
                WindowInfo info = ADInfoBll.Instance.GetWindowInfo(this.Name);
                if (info != null)
                {
                    ArchiveExcelForm form = new GeneratedArchiveExcelForm(info);
                    form.Show();
                }
            }
//            OpenFileDialog openFileDialog1 = new OpenFileDialog();
//            openFileDialog1.RestoreDirectory = true;
//            openFileDialog1.Filter = "所有 Excel 文件|*.xls;*.xlsx;*.xml|Excel 97-2003 文件|*.xls|Excel 2007 文件|*.xlsx|XML 文件|*.xml";
//
//            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
//            {
//                try
//                {
//                    IList<DataTable> list = ExcelXmlHelper.ReadExcelXml(openFileDialog1.FileName);
//                    foreach (DataTable dt in list)
//                    {
//                        foreach (DataRow row in dt.Rows)
//                        {
//                            object entity = this.ControlManager.AddNew();
//                            if (entity != null)
//                            {
//                                foreach (DataColumn column in dt.Columns)
//                                {
//                                    EntityHelper.SetPropertyValue(entity, column.ColumnName, row[column.ColumnName]);
//                                }
//                                this.ControlManager.EndEdit(true);
//                            }
//                        }
//
//                        break;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ExceptionProcess.ProcessWithNotify(ex);
//                }
//            }
        }
    }
}