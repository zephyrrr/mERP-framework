using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Forms;

namespace Feng.Grid.Print
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MyGridExportSaveFileDialog : FileDialogControlBase
    {
        /// <summary>
        /// 
        /// </summary>
        public MyGridExportSaveFileDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPrepareMSDialog()
        {
            base.FileDlgInitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            base.FileDlgFilter = "所有 Excel 文件|*.xls;*.xlsx;*.xml|Excel 97-2003 文件|*.xls|Excel 2007 文件|*.xlsx|XML 文件|*.xml";
            this.FileDlgCaption = "保存";
            this.FileDlgOkCaption = "保存(&S)";
            base.FileDlgType = FileDialogType.SaveFileDlg;
            base.FileDlgCheckFileExists = false;
            base.OnPrepareMSDialog();
        }

        /// <summary>
        /// 导出内容是否包含DetailGrid
        /// </summary>
        public MyCheckBox CkbIncludeDetailGrids
        {
            get { return ckbIncludeDetailGrid; }
        }

        private void ckbIncludeDetailGrid_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
