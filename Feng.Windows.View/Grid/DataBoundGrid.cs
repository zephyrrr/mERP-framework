using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using Xceed.Grid;
using Xceed.Validation;

namespace Feng.Grid
{
    /// <summary>
    /// 数据绑定的Grid
    /// </summary>
    public partial class DataBoundGrid : MyGrid, IBoundGrid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_boundGridHelper != null)
                {
                    m_boundGridHelper.Dispose();
                    m_boundGridHelper = null;
                }
                if (this.DisplayManager != null)
                {
                    this.DisplayManager.Dispose();
                    this.DisplayManager = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataBoundGrid()
            : base()
        {
        }

        private BoundGridHelper m_boundGridHelper;
        /// <summary>
        /// BoundGridHelper
        /// </summary>
        public BoundGridHelper BoundGridHelper
        {
            get 
            {
                if (m_boundGridHelper == null)
                {
                    m_boundGridHelper = new BoundGridHelper(this);
                }
                return m_boundGridHelper; 
            }
        }

        /// <summary>
        /// IDisplayManager
        /// </summary>
        [DefaultValue(null)]
        public IDisplayManager DisplayManager
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CreateGrid()
        {
            this.CreateBoundGrid();

            this.BoundGridHelper.Initialize();
        }

        /// <summary>
        /// 重新读入是否显示等属性
        /// </summary>
        public virtual void LoadDefaultLayout()
        {
            this.LoadGridDefaultLayout();
        }

        ///// <summary>
        ///// 正在读入数据
        ///// </summary>
        //public bool IsDataLoading
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否在手工设置Position
        ///// </summary>
        //public bool InPositionSetting
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public override void SetDataBinding(object dataSource, string dataMember)
        {
            try
            {
                this.DisplayManager.BeginBatchOperation();

                base.SetDataBinding(dataSource, dataMember);
                this.SetGridRowCellProperties();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                this.DisplayManager.EndBatchOperation();
                this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);
            }
        }
    }
}