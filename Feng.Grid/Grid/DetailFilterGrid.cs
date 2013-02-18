//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;
//using Xceed.Grid;

//namespace Feng.Grid
//{
//    /// <summary>
//    /// FilterGrid with DetailGrid
//    /// </summary>
//    public partial class DetailFilterGrid : FilterGrid
//    {
//        private MyDetailGrid m_detailGrid;

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public DetailFilterGrid() 
//            : base()
//        {
//            m_detailGrid = new MyDetailGrid();
//            m_detailGrid.Columns.Add(new Xceed.Grid.Column(FilterGrid.SelectCaption, typeof (bool)));
//            this.AddDetailGrid(m_detailGrid);
//        }

//        /// <summary>
//        /// EndEdit
//        /// </summary>
//        public override void EndInit()
//        {
//            base.EndInit();
//            m_detailGrid.DataRowTemplate.Cells[SelectCaption].ValueChanged +=
//                new EventHandler(DetailFilterGrid_SelectionValueChanged);
//        }

//        /// <summary>
//        /// DetailGrid
//        /// </summary>
//        public DetailGrid DetailGrid
//        {
//            get { return m_detailGrid; }
//        }

//        private bool m_bMasterSelect;
//        private bool m_bSlaveSelect;

//        /// <summary>
//        /// 选定DetailGrid的所有
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        protected override void FilterGrid_SelectionValueChanged(object sender, EventArgs e)
//        {
//            if (m_bSlaveSelect)
//            {
//                return;
//            }

//            Cell cell = (Cell) sender;
//            //if (((Xceed.Grid.DataRow)cell.ParentRow).DetailGrids.Count == 0)
//            //    return;
//            DetailGrid detailGrid = ((Xceed.Grid.DataRow) cell.ParentRow).DetailGrids[0];
//            if (cell.Value != null)
//            {
//                m_bMasterSelect = true;

//                foreach (Xceed.Grid.DataRow row in detailGrid.DataRows)
//                {
//                    row.Cells[SelectCaption].Value = cell.Value;
//                }

//                m_bMasterSelect = false;
//            }
//        }

//        /// <summary>
//        /// 反应到MasterGrid的CheckBox上
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void DetailFilterGrid_SelectionValueChanged(object sender, EventArgs e)
//        {
//            if (m_bMasterSelect)
//            {
//                return;
//            }

//            Cell cell = (Cell) sender;
//            DetailGrid detailGrid = cell.ParentGrid;

//            bool selectAll = true;
//            bool selectNone = true;

//            foreach (Xceed.Grid.DataRow row in detailGrid.DataRows)
//            {
//                if ((bool) row.Cells[SelectCaption].Value)
//                {
//                    selectNone = false;
//                }
//                else
//                {
//                    selectAll = false;
//                }
//            }

//            m_bSlaveSelect = true;
//            Xceed.Grid.DataRow rowParent = detailGrid.ParentDataRow;
//            if (selectAll)
//            {
//                if (rowParent.Cells[SelectCaption].Value == null ||
//                    Convert.ToBoolean(rowParent.Cells[SelectCaption].Value) != true)
//                {
//                    rowParent.Cells[SelectCaption].Value = true;
//                }
//            }
//            else if (selectNone)
//            {
//                if (rowParent.Cells[SelectCaption].Value == null ||
//                    Convert.ToBoolean(rowParent.Cells[SelectCaption].Value) != false)
//                {
//                    rowParent.Cells[SelectCaption].Value = false;
//                }
//            }
//            else
//            {
//                if (rowParent.Cells[SelectCaption].Value != null)
//                {
//                    rowParent.Cells[SelectCaption].Value = null;
//                }
//            }

//            m_bSlaveSelect = false;
//        }
//    }
//}