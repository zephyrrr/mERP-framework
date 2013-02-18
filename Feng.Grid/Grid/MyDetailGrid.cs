using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Data;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// MyDetailGrid
    /// </summary>
    public class MyDetailGrid : DetailGrid, IGrid, IDisposable
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
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DataRows.Clear();

                foreach (Xceed.Grid.Column column in this.Columns)
                {
                    if (column.CellEditorManager != null)
                    {
                        if (column.CellEditorManager.TemplateControl != null)
                        {
                            column.CellEditorManager.TemplateControl.Dispose();
                        }
                        column.CellEditorManager.Dispose();
                        column.CellEditorManager = null;
                    }
                    if (column.CellViewerManager != null)
                    {
                        if (column.CellViewerManager.Control != null)
                        {
                            column.CellViewerManager.Control.Dispose();
                        }
                        column.CellViewerManager.Dispose();
                        column.CellViewerManager = null;
                    }
                }

                this.ColumnAdded -= new ColumnAddedEventHandler(MyDetailGrid_ColumnAdded);

                foreach (MyDetailGrid detailGrid in this.DetailGridTemplates)
                {
                    detailGrid.Dispose();
                }
            }
        }

        private GridHelper m_gridHelper;
        /// <summary>
        /// GridHelper
        /// </summary>
        public GridHelper GridHelper
        {
            get
            {
                if (m_gridHelper == null)
                {
                    m_gridHelper = new GridHelper(this);
                }
                return m_gridHelper;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyDetailGrid()
            : base()
        {
            this.HeaderRows.Add(new ColumnManagerRow());

            this.ColumnAdded += new ColumnAddedEventHandler(MyDetailGrid_ColumnAdded);
        }

        void MyDetailGrid_ColumnAdded(object sender, ColumnAddedEventArgs e)
        {
            this.ApplyDefaultSettings();
            this.ApplyDefaultEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="synchronizationRoot"></param>
        protected MyDetailGrid(DetailGrid template, DetailGrid synchronizationRoot)
            : base(template, synchronizationRoot)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synchronizationRoot"></param>
        /// <returns></returns>
        protected override DetailGrid CreateInstance(DetailGrid synchronizationRoot)
        {
            return new MyDetailGrid(this, synchronizationRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.Form FindForm()
        {
            if (this.GridControl == null)
                return null;
            else
                return this.GridControl.FindForm();
        }

        /// <summary>
        /// FixedHeaderRows
        /// </summary>
        public Xceed.Grid.Collections.RowList FixedHeaderRows
        {
            get { return this.HeaderRows; }
        }

        /// <summary>
        /// FixedFooterRows
        /// </summary>
        public Xceed.Grid.Collections.RowList FixedFooterRows
        {
            get { return this.FooterRows; }
        }

        /// <summary>
        /// CurrentRow
        /// </summary>
        public Xceed.Grid.Row CurrentRow
        {
            get
            {
                Row row = this.GridControl.CurrentRow;
                if (row.ParentGrid == this)
                {
                    return row;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.ParentGrid == this)
                {
                    MyGrid.SetCurrentRow(this.GridControl, value);
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return 0; }
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public new virtual void SetDataBinding(object dataSource, string dataMember)
        {
            base.SetDataBinding(dataSource, dataMember);
        }
    }
}