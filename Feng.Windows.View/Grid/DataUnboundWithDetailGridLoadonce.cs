using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Xceed.Grid;
using Feng.Grid.Columns;
using Feng.Windows.Utils;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataUnboundWithDetailGridLoadonce : DataUnboundGrid
    {
        /// <summary>
        /// 
        /// </summary>
        public DataUnboundWithDetailGridLoadonce()
        {
        }

        /// <summary>
        /// 重新读入是否显示等属性
        /// </summary>
        public override void LoadDefaultLayout()
        {
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(this.GridName))
            {
                Xceed.Grid.Column column = this.Columns[info.GridColumnName];
                if (column != null)
                {
                    column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
                    column.VisibleIndex = info.SeqNo;
                }
                else
                {
                    foreach (Xceed.Grid.DataRow row in this.DataRows)
                    {
                        if (row.DetailGrids.Count > 0)
                        {
                            LoadDefaultLayout(row.DetailGrids[0], info);
                        }
                    }
                }
            }
        }

        private void LoadDefaultLayout(DetailGrid detailGrid, GridColumnInfo info)
        {
            Xceed.Grid.Column column = detailGrid.Columns[info.GridColumnName];
            if (column != null)
            {
                column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
                column.VisibleIndex = info.SeqNo;
            }
            else
            {
                foreach (Xceed.Grid.DataRow row in detailGrid.DataRows)
                {
                    if (row.DetailGrids.Count > 0)
                    {
                        LoadDefaultLayout(row.DetailGrids[0], info);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string[] m_levelParents;

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, GridColumnInfo> m_infos;

        /// <summary>
        /// 
        /// </summary>
        protected int m_maxLevel;

        /// <summary>
        /// 
        /// </summary>
        protected List<string>[] m_gridColumnNames;

        /// <summary>
        /// 
        /// </summary>
        public override void CreateGrid()
        {
            this.BeginInit();

            m_infos = new Dictionary<string, GridColumnInfo>();
            Dictionary<string, int> levels = new Dictionary<string, int>();
            int cnt = 0;
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(this.GridName))
            {
                if (m_infos.ContainsKey(info.GridColumnName))
                {
                    throw new ArgumentException("there have already exist column " + info.GridColumnName);
                }
                m_infos[info.GridColumnName] = info;
                cnt++;
            }
            m_maxLevel = 0;
            int repeatTime = 0;
            while (levels.Count < cnt)
            {
                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(this.GridName))
                {
                    if (levels.ContainsKey(info.GridColumnName))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(info.ParentPropertyName))
                    {
                        levels[info.GridColumnName] = 0;
                    }
                    else if (levels.ContainsKey(info.ParentPropertyName))
                    {
                        levels[info.GridColumnName] = levels[info.ParentPropertyName] + 1;
                        m_maxLevel = Math.Max(m_maxLevel, levels[info.GridColumnName]);
                    }
                }
                repeatTime++;
                if (repeatTime >= cnt)
                {
                    throw new ArgumentException("there must have some invalide ParentPropertyName!");
                }
            }
            Debug.Assert(m_maxLevel > 0);

            m_levelParents = new string[m_maxLevel];
            foreach (KeyValuePair<string, int> kvp in levels)
            {
                if (kvp.Value == 0)
                {
                    continue;
                }
                m_levelParents[kvp.Value - 1] = m_infos[kvp.Key].ParentPropertyName;
            }

            m_gridColumnNames = new List<string>[m_maxLevel + 1];
            for (int i = 0; i <= m_maxLevel; ++i)
            {
                m_gridColumnNames[i] = new List<string>();
            }
            foreach (KeyValuePair<string, int> kvp in levels)
            {
                if (Array.IndexOf<string>(m_levelParents, kvp.Key) != -1)
                {
                    m_gridColumnNames[kvp.Value].Insert(0, kvp.Key);
                }
                else
                {
                    m_gridColumnNames[kvp.Value].Add(kvp.Key);
                }
            }


            MyDetailGrid[] detailGrid = new MyDetailGrid[m_maxLevel];
            for (int i = 0; i < m_maxLevel; ++i)
            {
                MyDetailGrid detail = new MyDetailGrid();
                detail.Collapsed = true;

                if (i == 0)
                {
                    base.DetailGridTemplates.Add(detail);
                }
                else
                {
                    detailGrid[i - 1].DetailGridTemplates.Add(detail);
                }
                detailGrid[i] = detail;
            }
            this.UpdateDetailGrids();


            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(this.GridName))
            {
                switch (info.GridColumnType)
                {
                    case GridColumnType.NoColumn:
                        break;
                    case GridColumnType.CheckColumn:
                        this.AddCheckColumn();
                        break;
                    case GridColumnType.StatColumn:
                        {
                            Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                            column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
                            if (!column.Visible)
                            {
                                // only for column custom visible
                                column.MaxWidth = 0;
                            }
                            column.Title = string.IsNullOrEmpty(info.Caption) ? info.PropertyName : info.Caption;
                            column.Tag = info;
                            column.ReadOnly = true;
                            if (levels[info.GridColumnName] == 0)
                            {
                                this.Columns.Add(column);
                            }
                            else
                            {
                                detailGrid[levels[info.GridColumnName] - 1].Columns.Add(column);
                            }
                        }
                        break;
                    case GridColumnType.ExpressionColumn:
                    case GridColumnType.ImageColumn:
                    case GridColumnType.SplitColumn:
                        break;
                    case GridColumnType.Normal:
                        {
                            Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                            column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
                            if (!column.Visible)
                            {
                                // only for column custom visible
                                column.MaxWidth = 0;
                            }
                            column.Title = string.IsNullOrEmpty(info.Caption) ? info.PropertyName : info.Caption;
                            column.Tag = info;

                            GridFactory.CreateCellViewerManager(column, info, this.DisplayManager);
                            
                            bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
                            if (readOnly)
                            {
                                column.ReadOnly = readOnly;
                            }
                            else
                            {
                                GridFactory.CreateCellEditorManager(column, info, this.DisplayManager);
                            }

                            if (levels[info.GridColumnName] == 0)
                            {
                                this.Columns.Add(column);
                            }
                            else
                            {
                                detailGrid[levels[info.GridColumnName] - 1].Columns.Add(column);
                            }
                        }
                        break;
                    default:
                        throw new NotSupportedException("Invalid GridColumnType");
                }
            }
            this.UpdateDetailGrids();
            this.EndInit();

            this.CreateSumRow();
            this.CreateGroups();
            this.CreateEvents();

            for (int i = 0; i < m_maxLevel; ++i)
            {
                CreateDetailGridSumRow(detailGrid[i]);
            }

            this.BoundGridHelper.CreateColumnManageRowEvent();
            for (int i = 0; i < m_maxLevel; ++i)
            {
                CreateDetailGridColumnManageRowTips(detailGrid[i]);
            }
        }

        /// <summary>
        /// CreateDetailGridColumnManageRowTips
        /// </summary>
        /// <param name="detailGrid"></param>
        protected void CreateDetailGridColumnManageRowTips(MyDetailGrid detailGrid)
        {
            ColumnManagerRow columnManageRow = detailGrid.GetColumnManageRow();
            if (columnManageRow != null)
            {
                foreach (Column column in detailGrid.Columns)
                {
                    columnManageRow.Cells[column.FieldName].MouseEnter += new EventHandler(this.BoundGridHelper.ColumnManageCell_MouseEnter);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailGrid"></param>
        protected void CreateDetailGridSumRow(MyDetailGrid detailGrid)
        {
            bool needSum = false;
            if (!needSum)
            {
                foreach (Column column in detailGrid.Columns)
                {
                    GridColumnInfo info = column.Tag as GridColumnInfo;
                    if (info == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(info.StatFunction) || !Authority.AuthorizeByRule(info.ColumnVisible))
                    {
                        continue;
                    }

                    needSum = true;
                    break;
                }
            }

            if (needSum)
            {
                SummaryRow sumRow = detailGrid.AddSummaryRowToFixedFooter();
                sumRow.TextFormat = ADInfoBll.Instance.GetGridInfo(detailGrid.GridName).StatTitle;

                foreach (Column column in detailGrid.Columns)
                {
                    GridColumnInfo info = column.Tag as GridColumnInfo;
                    if (info == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(info.StatFunction) || !Authority.AuthorizeByRule(info.ColumnVisible))
                    {
                        continue;
                    }

                    MySummaryCell cell = ((MySummaryCell)sumRow.Cells[info.GridColumnName]);
                    cell.StatFunction = BoundGridExtention.GetStatFunction(info.StatFunction);
                    cell.RunningStatGroupLevel = 0;
                    cell.ResultDataType = typeof(string);

                    if (!string.IsNullOrEmpty(info.StatTitle))
                    {
                        cell.TitleFormat = info.StatTitle;
                        cell.TitlePosition = TitlePosition.PreferablyRight;
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public override void SetDataBinding(object dataSource, string dataMember)
        {
            SetDataRows(dataSource, dataMember, m_maxLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="hereLevel"></param>
        protected void SetDataRows(object dataSource, string dataMember, int hereLevel)
        {
            try
            {
                this.DisplayManager.BeginBatchOperation();

                this.DataRows.Clear();

                System.Collections.Generic.Dictionary<string, Xceed.Grid.DataRow> masterRows =
                    new System.Collections.Generic.Dictionary<string, Xceed.Grid.DataRow>();

                DataTable dt = dataSource as DataTable;
                if (dt == null)
                {
                    IEnumerable list = dataSource as IEnumerable;
                    if (list != null)
                    {
                        foreach (object entity in list)
                        {
                            for (int i = 0; i <= hereLevel; ++i)
                            {
                                Xceed.Grid.DataRow gridRow = null;

                                if (i < hereLevel || i == 0)
                                {
                                    if (!masterRows.ContainsKey(GetListValue(entity, m_levelParents[i]).ToString()))
                                    {
                                        if (i == 0)
                                        {
                                            gridRow = this.DataRows.AddNew();
                                            masterRows[GetListValue(entity, m_levelParents[i]).ToString()] = gridRow;
                                        }
                                        else
                                        {
                                            gridRow = masterRows[GetListValue(entity, m_levelParents[i - 1]).ToString()].DetailGrids[0].DataRows.AddNew();
                                        }
                                    }
                                    else
                                    {
                                        gridRow = masterRows[GetListValue(entity, m_levelParents[i]).ToString()];
                                    }
                                }
                                else
                                {
                                    gridRow = masterRows[GetListValue(entity, m_levelParents[i - 1]).ToString()].DetailGrids[0].DataRows.AddNew();
                                }
                                gridRow.EndEdit();

                                gridRow.Tag = entity;
                                foreach (string columnName in m_gridColumnNames[i])
                                {
                                    GridColumnInfo info = m_infos[columnName];

                                    switch (info.GridColumnType)
                                    {
                                        case GridColumnType.Normal:
                                            if (gridRow.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                                            {
                                                gridRow.Cells[info.GridColumnName].Value = EntityScript.GetPropertyValue(entity, info.Navigator, info.PropertyName);
                                            }
                                            break;
                                        case GridColumnType.WarningColumn:
                                            gridRow.Cells[info.GridColumnName].Value = (gridRow.Cells[info.GridColumnName].ParentColumn as WarningColumn).Calculate(entity);
                                            break;
                                        case GridColumnType.StatColumn:
                                        case GridColumnType.ExpressionColumn:
                                        case GridColumnType.ImageColumn:
                                        case GridColumnType.SplitColumn:
                                        case GridColumnType.CheckColumn:
                                        case GridColumnType.NoColumn:
                                            break;
                                        default:
                                            throw new NotSupportedException("invalid GridColumnType of " + info.GridColumnType);
                                    }
                                    // 
                                    //gridRow.Cells[columnName].Value = GetListValue(entity, columnName);
                                }
                                gridRow.EndEdit();
                            }
                        }
                    }

                    Dictionary<Xceed.Grid.DataRow, bool> m_processdRows = new Dictionary<Xceed.Grid.DataRow, bool>();
                    foreach (object entity in list)
                    {
                        for (int i = 0; i < hereLevel; ++i)
                        {
                            Xceed.Grid.DataRow gridRow = masterRows[GetListValue(entity, m_levelParents[i]).ToString()];
                            if (!m_processdRows.ContainsKey(gridRow))
                            {
                                m_processdRows[gridRow] = true;
                            }
                            else
                            {
                                continue;
                            }
                            foreach (string columnName in m_gridColumnNames[i])
                            {
                                GridColumnInfo info = m_infos[columnName];
                                switch (info.GridColumnType)
                                {
                                    case GridColumnType.StatColumn:
                                        {
                                            if (gridRow.DetailGrids.Count == 0)
                                            {
                                                throw new ArgumentException("stateColumn should has detailgrids.");
                                            }
                                            gridRow.Cells[info.GridColumnName].Value = Feng.Utils.ConvertHelper.ChangeType(
                                                MySummaryRow.GetSummaryResult(gridRow.DetailGrids[0].GetSortedDataRows(true), info.PropertyName), gridRow.Cells[info.GridColumnName].ParentColumn.DataType);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        for (int i = 0; i <= hereLevel; ++i)
                        {
                            Xceed.Grid.DataRow gridRow = null;

                            if (i < hereLevel || i == 0)
                            {
                                if (!masterRows.ContainsKey(dataRow[m_levelParents[i]].ToString()))
                                {
                                    if (i == 0)
                                    {
                                        gridRow = this.DataRows.AddNew();

                                        masterRows[dataRow[m_levelParents[i]].ToString()] = gridRow;
                                    }
                                    else
                                    {
                                        gridRow = masterRows[dataRow[m_levelParents[i - 1]].ToString()].DetailGrids[0].DataRows.AddNew();
                                    }
                                }
                                else
                                {
                                    gridRow = masterRows[dataRow[m_levelParents[i]].ToString()];
                                }
                            }
                            else
                            {
                                gridRow = masterRows[dataRow[m_levelParents[i - 1]].ToString()].DetailGrids[0].DataRows.AddNew();
                            }

                            // 如果在下方EndEdit，DetailGrid可能不会生成，不知原因
                            gridRow.EndEdit();

                            gridRow.Tag = dataRow;
                            foreach (string columnName in m_gridColumnNames[i])
                            {
                                GridColumnInfo info = m_infos[columnName];

                                switch (info.GridColumnType)
                                {
                                    case GridColumnType.Normal:
                                        {
                                            if (!dt.Columns.Contains(columnName))
                                            {
                                                continue;
                                            }
                                            if (gridRow.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                                            {
                                                gridRow.Cells[info.GridColumnName].Value = (dataRow[columnName] == System.DBNull.Value ?
                                                        null : Feng.Utils.ConvertHelper.TryIntToEnum(dataRow[columnName], GridColumnInfoHelper.CreateType(m_infos[columnName])));
                                            }
                                        }
                                        break;
                                    case GridColumnType.WarningColumn:
                                        throw new NotSupportedException("warning column is not supported in datarow.");
                                    case GridColumnType.StatColumn:
                                    case GridColumnType.ExpressionColumn:
                                    case GridColumnType.ImageColumn:
                                    case GridColumnType.SplitColumn:
                                    case GridColumnType.CheckColumn:
                                    case GridColumnType.NoColumn:
                                        break;
                                    default:
                                        throw new NotSupportedException("invalid GridColumnType of " + info.GridColumnType);
                                }
                            }
                            gridRow.EndEdit();
                        }
                    }

                    Dictionary<Xceed.Grid.DataRow, bool> m_processdRows = new Dictionary<Xceed.Grid.DataRow, bool>();
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        for (int i = 0; i < hereLevel; ++i)
                        {
                            Xceed.Grid.DataRow gridRow = masterRows[dataRow[m_levelParents[i]].ToString()];
                            if (!m_processdRows.ContainsKey(gridRow))
                            {
                                m_processdRows[gridRow] = true;
                            }
                            else
                            {
                                continue;
                            }
                            foreach (string columnName in m_gridColumnNames[i])
                            {
                                GridColumnInfo info = m_infos[columnName];
                                switch (info.GridColumnType)
                                {
                                    case GridColumnType.StatColumn:
                                        {
                                            if (gridRow.DetailGrids.Count == 0)
                                            {
                                                throw new ArgumentException("stateColumn should has detailgrids.");
                                            }

                                            gridRow.Cells[info.GridColumnName].Value = Feng.Utils.ConvertHelper.ChangeType(
                                                MySummaryRow.GetSummaryResult(gridRow.DetailGrids[0].GetSortedDataRows(true), info.PropertyName), gridRow.Cells[info.GridColumnName].ParentColumn.DataType);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                this.AfterLoadData();
            }
            finally
            {
                this.DisplayManager.EndBatchOperation();
                this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        protected object GetListValue(object entity, string columnName)
        {
            GridColumnInfo info = m_infos[columnName];
            return EntityScript.GetPropertyValue(entity, info.Navigator, info.PropertyName);
        }
    }
}