using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Forms;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MyGridSetupForm : PositionPersistForm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        public MyGridSetupForm(IBoundGrid grid)
        {
            InitializeComponent();

            m_masterGrid = grid;
        }

        private IBoundGrid m_masterGrid;

        private void MyGridSetupForm_Load(object sender, EventArgs e)
        {
            LoadGridInfos();
        }

        private void LoadGridInfos()
        {
            ArchiveSetupForm.LoadGridInfos(grdGridColumns, m_masterGrid);
        }

        private void SaveGridInfos()
        {
            ArchiveSetupForm.SaveGridInfos(grdGridColumns, m_masterGrid);
        }

        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            ArchiveSetupForm.ResetGridInfos(grdGridColumns, m_masterGrid);

            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveGridInfos();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            
            ArchiveSetupForm.MoveRow(grdGridColumns, true);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            ArchiveSetupForm.MoveRow(grdGridColumns, false);
        }
    }
}