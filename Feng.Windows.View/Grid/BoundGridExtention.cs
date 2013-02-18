using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Xceed.Grid;
using Feng.Windows.Utils;
using Feng.Grid.Columns;

namespace Feng.Grid
{
    /// <summary>
    /// 数据绑定Grid扩展
    /// </summary>
    public static class BoundGridExtention
    {
        /// <summary>
        /// 设置显示控制器
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dm"></param>
        public static void SetDisplayManager(this IBoundGrid grid, IDisplayManager dm)
        {
            SetDisplayManager(grid, dm, null);
        }

        /// <summary>
        /// 设置显示控制器
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dm"></param>
        /// <param name="gridName"></param>
        public static void SetDisplayManager(this IBoundGrid grid, IDisplayManager dm, string gridName)
        {
            if (dm == null)
            {
                throw new ArgumentNullException("dm");
            }
            grid.DisplayManager = dm;
            grid.GridName = gridName;
            grid.SetState(StateType.View);

            grid.DisplayManager.BindingControls.Add(grid);
            grid.CreateGrid();
            ManagerFactory.CreateSearchManagerEagerFetchs(dm.SearchManager, gridName);

            grid.AfterLoadData();
        }

        /// <summary>
        /// 创建绑定Grid
        /// </summary>
        /// <param name="grid"></param>
        internal static void CreateBoundGrid(this IBoundGrid grid)
        {
            grid.DisplayManager.SetDataBinding(grid.DisplayManager.SearchManager.GetSchema(), string.Empty);

            if (ADInfoBll.Instance.GetGridColumnInfos(grid.GridName).Count > 0)
            {
                try
                {
                    grid.BeginInit();

                    foreach (Xceed.Grid.Column column in grid.Columns)
                    {
                        column.Visible = false;
                    }

                    foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                    {
                        switch (info.GridColumnType)
                        {
                            case GridColumnType.NoColumn:
                            case GridColumnType.StatColumn:
                            case GridColumnType.WarningColumn:
                            case GridColumnType.CheckColumn:
                                break;
                            case GridColumnType.Normal:
                                Xceed.Grid.Column column = grid.Columns[info.GridColumnName];
                                if (column == null)
                                {
                                    throw new ArgumentException("Invalid GridColumnInfo of " + info.GridColumnName);
                                }

                                column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
                                if (!column.Visible)
                                {
                                    // only for column custom visible.
                                    // 当重置配置的时候，不会使他显示出来
                                    column.MaxWidth = 0;
                                }

                                column.VisibleIndex = info.SeqNo;
                                column.Title = (string.IsNullOrEmpty(info.Caption) ? info.PropertyName : info.Caption);

                                if (!string.IsNullOrEmpty(info.BackColor))
                                {
                                    column.BackColor = System.Drawing.Color.FromName(info.BackColor);
                                }
                                if (!string.IsNullOrEmpty(info.ForeColor))
                                {
                                    column.ForeColor = System.Drawing.Color.FromName(info.ForeColor);
                                }
                                if (!string.IsNullOrEmpty(info.FontName) && info.FontSize.HasValue)
                                {
                                    column.Font = new System.Drawing.Font(info.FontName, info.FontSize.Value);
                                }

                                column.Tag = info;

                                GridFactory.CreateCellViewerManager(column, info, grid.DisplayManager);

                                bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
                                if (readOnly)
                                {
                                    column.ReadOnly = readOnly;
                                }
                                else
                                {
                                    GridFactory.CreateCellEditorManager(column, info, grid.DisplayManager);
                                }
                                break;
                            default:
                                throw new NotSupportedException("Invalide gridcolumnType of " + info.GridColumnType + " in " + info.Name);
                        }
                    }

                    grid.CreateSumRow();
                    grid.CreateGroups();
                    grid.CreateEvents();
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
                finally
                {
                    grid.EndInit();
                }
            }

            grid.BoundGridHelper.CreateColumnManageRowEvent();
            grid.SetColumnManagerRowHorizontalAlignment();
        }

        /// <summary>
        /// 创建Group
        /// </summary>
        internal static void CreateEvents(this IBoundGrid grid)
        {
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
            {
                switch (info.GridColumnType)
                {
                    case GridColumnType.NoColumn:
                        break;
                    default:
                        if (info.DoubleClick != null)
                        {
                            GridColumnInfo info2 = info;
                            grid.DataRowTemplate.Cells[info.GridColumnName].DoubleClick += new EventHandler(delegate(object sender, EventArgs e)
                            {
                                EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info2.DoubleClick), sender, e);
                            });
                        }
                        break;
                    //default:
                    //    throw new InvalidOperationException("Invalide gridcolumnType of " + info.GridColumnType + " in " + info.Name);
                }
            }
        }

        /// <summary>
        /// 创建Group
        /// </summary>
        internal static void CreateGroups(this IBoundGrid grid)
        {
            IList<GridGroupInfo> groupInfos = ADInfoBll.Instance.GetGridGroupInfos(grid.GridName);

            foreach (GridGroupInfo info in groupInfos)
            {
                Group group = grid.AddGroup(info.GroupBy, info.TitleFormat);
                group.Collapsed = info.Collapsed;
            }
        }

        /// <summary>
        /// 创建Summary行
        /// </summary>
        internal static void CreateSumRow(this IBoundGrid grid)
        {
            bool needSum = false;

            if (!string.IsNullOrEmpty(ADInfoBll.Instance.GetGridInfo(grid.GridName).StatTitle))
            {
                needSum = true;
            }
            if (!needSum)
            {
                foreach (Column column in grid.Columns)
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
                SummaryRow sumRow = grid.AddSummaryRowToFixedFooter();

                sumRow.TextFormat = ADInfoBll.Instance.GetGridInfo(grid.GridName).StatTitle;

                foreach (Column column in grid.Columns)
                {
                    GridColumnInfo info = column.Tag as GridColumnInfo;
                    if (info == null || !Authority.AuthorizeByRule(info.ColumnVisible))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(info.StatFunction))
                    {
                        MySummaryCell cell = ((MySummaryCell)sumRow.Cells[info.GridColumnName]);
                        cell.StatFunction = BoundGridExtention.GetStatFunction(info.StatFunction);
                        cell.RunningStatGroupLevel = 0;
                        //cell.ResultDataType = typeof(string);

                        // 只有在StatFunction有效的时候，StatTitle才有效。就是一个常量型的文字
                        if (!string.IsNullOrEmpty(info.StatTitle))
                        {
                            //MySummaryCell cell = ((MySummaryCell)sumRow.Cells[info.GridColumnName]);
                            //cell.RunningStatGroupLevel = 0;
                            //cell.ResultDataType = typeof(string);
                            cell.TitleFormat = info.StatTitle;
                            cell.TitlePosition = Xceed.Grid.TitlePosition.PreferablyRight;
                        }
                    }
                   
                }
            }
        }

        /// <summary>
        ///  根据名称得到统计函数 
        /// </summary>
        /// <param name="statFunction"></param>
        /// <returns></returns>
        internal static Xceed.Grid.StatFunction GetStatFunction(string statFunction)
        {
            switch (statFunction.ToUpper())
            {
                case "SUM":
                    return Xceed.Grid.StatFunction.Sum;
                case "COUNT":
                    return Xceed.Grid.StatFunction.Count;
                case "MAXIMUM":
                    return Xceed.Grid.StatFunction.Maximum;
                case "MINIUM":
                    return Xceed.Grid.StatFunction.Minimum;
                case "AVERAGE":
                    return Xceed.Grid.StatFunction.Average;
                default:
                    throw new NotSupportedException("Invalid statFunction");
            }
        }

        /// <summary>
        /// 重新读入数据
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="reload">是否读入上一查询条件的数据。如false，则重新载入查询条件并读入数据</param>
        public static void ReloadData(this IBoundGrid grid, bool reload)
        {
            int pos = grid.DisplayManager.Position;
            grid.DisplayManager.BeginBatchOperation();
            if (reload)
            {
                grid.DisplayManager.SearchManager.ReloadData();
            }
            else
            {
                grid.DisplayManager.SearchManager.LoadDataAccordSearchControls();
            }

            bool ret = grid.DisplayManager.SearchManager.WaitLoadData();
            grid.DisplayManager.EndBatchOperation();

            if (!ret)
                return;

            if (pos != -1 && pos < grid.DataRows.Count)
            {
                if (grid.DisplayManager.Position != pos)
                {
                    grid.DisplayManager.Position = pos;
                }
                else
                {
                    grid.DisplayManager.OnPositionChanged(System.EventArgs.Empty);
                }

                MyGrid.SyncSelectedRowToCurrentRow(grid.GridControl);

                //if (grid.Visible && grid.DataRows[pos].Visible)
                //{
                //    grid.DataRows[pos].BringIntoView();

                //    //grid.GridControl.SelectedRows.Clear();
                //    //grid.GridControl.SelectedRows.Add(grid.DataRows[pos]);
                //    //MyGrid.SetCurrentRow(grid as Xceed.Grid.GridControl, grid.DataRows[pos]);
                //}
            }
            //else
            //{
            //    MessageForm.ShowWarning("无数据可刷新，请先搜索。");
            //}
        }
        /// <summary>
        /// 重新读入数据，并且保持当前条位置
        /// </summary>
        public static void ReloadData(this IBoundGrid grid)
        {
            ReloadData(grid, true);
        }

        /// <summary>
        /// 重新读入绑定Grid默认Layout
        /// </summary>
        /// <param name="grid"></param>
        public static void LoadGridDefaultLayout(this IGrid grid)
        {
            if (ADInfoBll.Instance.GetGridColumnInfos(grid.GridName).Count > 0)
            {
                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                {
                    Xceed.Grid.Column column = grid.Columns[info.GridColumnName];
                    if (column != null)
                    {
                        column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
                        column.VisibleIndex = info.SeqNo;

                        column.Fixed = info.ColumnFixed.HasValue ? info.ColumnFixed.Value : false;
                    }
                }
            }
            else
            {
                foreach (Xceed.Grid.Column column in grid.Columns)
                {
                    column.Visible = true;
                    column.Fixed = false;
                }
            }
        }
    }

    /// <summary>
    /// 非绑定Grid扩展
    /// </summary>
    public static class UnBoundGridExtention
    {
        /// <summary>
        /// 刷新Row
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        public static void ResetRowData(this IBoundGrid grid, Xceed.Grid.DataRow row)
        {
            SetDataRowsIListData(grid, grid.DisplayManager.Items[row.Index], row);
        }

        /// <summary>
        /// 根据Entity的值设置row's cell值
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="entity"></param>
        /// <param name="row"></param>
        public static void SetDataRowsIListData(this IBoundGrid grid, object entity, Xceed.Grid.DataRow row)
        {
            // 如果同一个实体类，只是里面内容变了，重新设置不会设置成功
            row.Tag = null;
            if (entity == null)
                return;

            row.Tag = entity;

            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
            {
                try
                {
                    switch (info.GridColumnType)
                    {
                        case GridColumnType.Normal:
                            if (row.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                            {
                                row.Cells[info.GridColumnName].Value = EntityScript.GetPropertyValue(entity, info.Navigator, info.PropertyName);
                            }
                            break;
                        case GridColumnType.WarningColumn:
                            row.Cells[info.GridColumnName].Value = (row.Cells[info.GridColumnName].ParentColumn as WarningColumn).Calculate(entity);
                            break;
                        case GridColumnType.StatColumn:
                            {
                                if (row.DetailGrids.Count == 0)
                                {
                                    return;
                                    //throw new ArgumentException("stateColumn should has detailgrids.");
                                }

                                row.Cells[info.GridColumnName].Value = Feng.Utils.ConvertHelper.ChangeType(
                                    MySummaryRow.GetSummaryResult(row.DetailGrids[0].GetSortedDataRows(true), info.PropertyName), row.Cells[info.GridColumnName].ParentColumn.DataType);
                            }
                            break;
                        case GridColumnType.ExpressionColumn:
                            {
                                //var info2 = info;
                                //var row2 = row;
                                //Feng.Async.AsyncHelper.Start(() =>
                                //{
                                //    if (info2.PropertyName.Contains("%"))
                                //    {
                                //        return EntityScript.CalculateExpression(info2.PropertyName, entity);
                                //    }
                                //    else
                                //    {
                                //        return Feng.Utils.ProcessInfoHelper.TryExecutePython(info2.PropertyName,
                                //            new Dictionary<string, object>() { { "entity", entity }, { "row", row2 } });
                                //    }
                                //}, (result) =>
                                //{
                                //    row2.Cells[info2.GridColumnName].Value = result;
                                //});
                                if (info.PropertyName.Contains("%"))
                                {
                                    row.Cells[info.GridColumnName].Value = EntityScript.CalculateExpression(info.PropertyName, entity);
                                }
                                else
                                {
                                    row.Cells[info.GridColumnName].Value = ProcessInfoHelper.TryExecutePython(info.PropertyName,
                                        new Dictionary<string, object>() { { "entity", entity }, { "row", row } });
                                }
                            }
                            break;
                        case GridColumnType.ImageColumn:
                            {
                                var i = Feng.Windows.ImageResource.Get(info.PropertyName);
                                row.Cells[info.GridColumnName].Value = i == null ? null : i.Reference;
                            }
                            break;
                        case GridColumnType.CheckColumn:
                            {
                                row.Cells[info.GridColumnName].Value = false;
                            }
                            break;
                        case GridColumnType.NoColumn:
                        case GridColumnType.SplitColumn:
                            break;
                        case GridColumnType.UnboundColumn:
                            {
                                try
                                {
                                    row.Cells[info.GridColumnName].Value = Feng.Utils.ConvertHelper.ChangeType(info.PropertyName, Feng.Utils.ReflectionHelper.GetTypeFromName(info.TypeName));
                                }
                                catch (Exception)
                                {
                                }
                            }
                            break;
                        case GridColumnType.IndexColumn:
                            {
                                row.Cells[info.GridColumnName].Value = row.Index + 1;
                            }
                            break;
                        default:
                            throw new NotSupportedException("invalid GridColumnType of " + info.GridColumnType);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(new ArgumentException(info.PropertyName + " info is invalid!", ex));
                }
            }
        }

        internal static void SetColumnProperties(Xceed.Grid.Column column, GridColumnInfo info, IGrid grid)
        {
            column.Visible = Authority.AuthorizeByRule(info.ColumnVisible);
            if (!column.Visible)
            {
                // only for column custom visible
                column.MaxWidth = 0;
            }

            column.VisibleIndex = info.SeqNo;
            column.Title = (string.IsNullOrEmpty(info.Caption) ? info.PropertyName : info.Caption);

            column.Tag = info;
            bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
            if (readOnly)
            {
                column.ReadOnly = readOnly;
            }

            if (!string.IsNullOrEmpty(info.BackColor))
            {
                column.BackColor = System.Drawing.Color.FromName(info.BackColor);
            }
            if (!string.IsNullOrEmpty(info.ForeColor))
            {
                column.ForeColor = System.Drawing.Color.FromName(info.ForeColor);
            }
            if (!string.IsNullOrEmpty(info.FontName) && info.FontSize.HasValue)
            {
                column.Font = new System.Drawing.Font(info.FontName, info.FontSize.Value);
            }
            //if (info.ColumnWidth.HasValue)
            //{
            //    column.Width = info.ColumnWidth.Value * grid.Width / 1024;
            //}
            if (info.ColumnMaxWidth.HasValue)
            {
                column.MaxWidth = info.ColumnMaxWidth.Value;
            }
            if (info.ColumnFixed.HasValue)
            {
                column.Fixed = info.ColumnFixed.Value;
            }

            if (!string.IsNullOrEmpty(info.SortDirection))
            {
                if (info.SortDirection.ToUpper() == "ASC")
                {
                    column.SortDirection = SortDirection.Ascending;
                }
                else if (info.SortDirection.ToUpper() == "DESC")
                {
                    column.SortDirection = SortDirection.Descending;
                }
            }
        }

        internal static void CreateUnBoundGrid(this IBoundGrid grid)
        {
            try
            {
                grid.BeginInit();

                grid.Columns.Clear();
                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                {
                    // 有些列是要设置值但不可见的，例如Id
                    //if (!Authority.AuthorizeByRule(info.ColumnVisible))
                    //    continue;

                    switch (info.GridColumnType)
                    {
                        case GridColumnType.NoColumn:
                            break;
                        case GridColumnType.CheckColumn:
                            {
                                CheckColumn column = grid.AddCheckColumn(info.GridColumnName);
                                SetColumnProperties(column, info, grid);
                            }
                            break;
                        case GridColumnType.Normal:
                            {
                                Xceed.Grid.Column column;
                                if (grid.Columns[info.GridColumnName] != null)
                                {
                                    //throw new ArgumentException("there have already exist column " + info.GridColumnName);
                                    continue;
                                }
                                else
                                {
                                    column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                                }

                                SetColumnProperties(column, info, grid);

                                GridFactory.CreateCellViewerManager(column, info, grid.DisplayManager);
                                bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
                                if (readOnly)
                                {
                                    column.ReadOnly = readOnly;
                                }
                                else
                                {
                                    GridFactory.CreateCellEditorManager(column, info, grid.DisplayManager);
                                }

                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.WarningColumn:
                            {
                                Columns.WarningColumn column = new Columns.WarningColumn(info.GridColumnName, info.PropertyName);
                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.StatColumn:
                            {
                                Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                                SetColumnProperties(column, info, grid);
                                GridFactory.CreateCellViewerManager(column, info, grid.DisplayManager);
                                column.ReadOnly = true;
                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.ExpressionColumn:
                            {
                                Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                                SetColumnProperties(column, info, grid);
                                GridFactory.CreateCellViewerManager(column, info, grid.DisplayManager);
                                bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
                                if (readOnly)
                                {
                                    column.ReadOnly = readOnly;
                                }
                                else
                                {
                                    GridFactory.CreateCellEditorManager(column, info, grid.DisplayManager);
                                }
                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.ImageColumn:
                            {
                                Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, typeof(System.Drawing.Image));
                                SetColumnProperties(column, info, grid);
                                column.ReadOnly = true;
                                column.MaxWidth = 72;
                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.SplitColumn:
                            {
                                Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, typeof(string));
                                SetColumnProperties(column, info, grid);
                                column.ReadOnly = true;
                                column.BackColor = System.Drawing.Color.LightGray;
                                column.Title = " ";
                                column.MaxWidth = 5;
                                column.Width = 5;
                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.UnboundColumn:
                            {
                                Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                                SetColumnProperties(column, info, grid);

                                GridFactory.CreateCellViewerManager(column, info, grid.DisplayManager);
                                bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
                                if (readOnly)
                                {
                                    column.ReadOnly = readOnly;
                                }
                                else
                                {
                                    GridFactory.CreateCellEditorManager(column, info, grid.DisplayManager);
                                }

                                grid.Columns.Add(column);
                            }
                            break;
                        case GridColumnType.IndexColumn:
                            {
                                Xceed.Grid.Column column = new Xceed.Grid.Column(info.GridColumnName, typeof(int));
                                SetColumnProperties(column, info, grid);
                                column.ReadOnly = true;

                                grid.Columns.Add(column);
                            }
                            break;
                        default:
                            throw new NotSupportedException("Invalide gridcolumnType of " + info.GridColumnType + " in " + info.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                grid.EndInit();
            }

            grid.CreateSumRow();
            grid.CreateGroups();
            grid.CreateEvents();

            grid.BoundGridHelper.CreateColumnManageRowEvent();
            grid.SetColumnManagerRowHorizontalAlignment();
            grid.CreateMultiColumnHeaderColumnManagerRow();
        }

        public static void CreateMultiColumnHeaderColumnManagerRow(this IGrid grid)
        {
            var gridColumnInfos = ADInfoBll.Instance.GetGridColumnInfos(grid.GridName);
            if (gridColumnInfos.Count == 0)
                return;

            List<int> counts = new List<int>();
            List<string> captions = new List<string>();
            string lastCaption = gridColumnInfos[0].GroupCaption;
            int count = 1;
            for(int i=1; i<gridColumnInfos.Count + 1; ++i)
            {
                if (i < gridColumnInfos.Count && 
                    (lastCaption == gridColumnInfos[i].GroupCaption
                    || (string.IsNullOrEmpty(lastCaption) && string.IsNullOrEmpty(gridColumnInfos[i].GroupCaption))))
                {
                    count++;
                }
                else
                {
                    counts.Add(count);
                    captions.Add(lastCaption);
                    if (i < gridColumnInfos.Count)
                    {
                        lastCaption = gridColumnInfos[i].GroupCaption;
                        count = 1;
                    }
                }
            }
            if (counts.Count == 1 && string.IsNullOrEmpty(captions[0]))
                return;

            grid.FixedHeaderRows.Insert(0, new MultiColumnHeaderColumnManagerRow(counts.ToArray(), captions.ToArray()));
        }

        /// <summary>
        /// 把Grid ColumnManagerRow中非空的Column设置为红色
        /// </summary>
        /// <param name="grid"></param>
        public static void SetColumnManagerRowHorizontalAlignment(this IGrid grid)
        {
            var columnManagerRow = grid.GetColumnManageRow();
            if (columnManagerRow != null)
            {
                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                {
                    if (!string.IsNullOrEmpty(info.HorizontalAlignment))
                    {
                        if (info.HorizontalAlignment == "Center")
                        {
                            columnManagerRow.Cells[info.GridColumnName].HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Center;
                            grid.DataRowTemplate.Cells[info.GridColumnName].HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Center;
                        }
                        else if (info.HorizontalAlignment == "Left")
                        {
                            columnManagerRow.Cells[info.GridColumnName].HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left;
                            grid.DataRowTemplate.Cells[info.GridColumnName].HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left;
                        }
                        else if (info.HorizontalAlignment == "Right")
                        {
                            columnManagerRow.Cells[info.GridColumnName].HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Right;
                            grid.DataRowTemplate.Cells[info.GridColumnName].HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Right;
                        }
                    }
                }
            }
        }

        internal static void SetUnBoundGridDataBinding(this IBoundGrid grid)
        {
            try
            {
                grid.BeginInit();

                grid.DataRows.Clear();

                GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(grid.GridName);
                if (!Authority.AuthorizeByRule(gridInfo.Visible))
                {
                    return;
                }
                if (!string.IsNullOrEmpty(gridInfo.ReadOnly))
                {
                    grid.ReadOnly = Authority.AuthorizeByRule(gridInfo.ReadOnly);
                }

                GridRowInfo gridRowInfo = ADInfoBll.Instance.GetGridRowInfo(grid.GridName);

                foreach (object entity in grid.DisplayManager.Items)
                {
                    if (!Permission.AuthorizeByRule(gridRowInfo.Visible, entity))
                    {
                        continue;
                    }
                    Xceed.Grid.DataRow row = grid.DataRows.AddNew();
                    grid.SetDataRowsIListData(entity, row);
                    row.EndEdit();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                grid.EndInit();
            }

            grid.AfterLoadData();
        }

        internal static void AddDateItemToUnBoundGrid(this IBoundGrid grid, object dataItem)
        {
            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(grid.GridName);
            if (!Authority.AuthorizeByRule(gridInfo.Visible))
            {
                return;
            }

            GridRowInfo gridRowInfo = ADInfoBll.Instance.GetGridRowInfo(grid.GridName);

            if (!Permission.AuthorizeByRule(gridRowInfo.Visible, dataItem))
            {
                return;
            }
            Xceed.Grid.DataRow row = grid.DataRows.AddNew();
            grid.SetDataRowsIListData(dataItem, row);
            row.EndEdit();
        }
    }

    /// <summary>
    /// Grid关于筛选行的扩展
    /// </summary>
    public static class BoundGridRowsExtention
    {
        /// <summary>
        /// 获取查找行的可见性
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static bool GetSearchRowVisible(this IBoundGrid grid)
        {
            if (grid.BoundGridHelper.SearchRow != null)
            {
                return grid.BoundGridHelper.SearchRow.Visible;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置查找行的可见性
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="visible"></param>
        public static void SetSearchRowVisible(this IBoundGrid grid, bool visible)
        {
            grid.BoundGridHelper.SearchRowVisible = visible;
        }

        //static ISearchExpression GetSearchExpressionFromFilters(IList<Feng.Grid.Filter.IFilter> filters)
        //{
        //    ISearchExpression ret = null;
        //    foreach (var i in filters)
        //    {
        //        if (i is Feng.Grid.Filter.LogicalFilter)
        //        {
        //            return new SearchExpression.
        //        }
        //    }
        //}

        //static ISearchExpression GetSearchExpressionFromFilters(Feng.Grid.Filter.IFilter filter)
        //{
        //    Feng.Grid.Filter.LogicalFilter logicalFilter = filter as Feng.Grid.Filter.LogicalFilter;
        //    if (logicalFilter != null)
        //    {
        //        if (logicalFilter.Operator == Feng.Search.LogicalOperator.And)
        //            return SearchExpression.And(GetSearchExpressionFromFilters(logicalFilter.FilterLeft), GetSearchExpressionFromFilters(logicalFilter.FilterRight));
        //        else if (logicalFilter.Operator == Feng.Search.LogicalOperator.Or)
        //            return SearchExpression.Or(GetSearchExpressionFromFilters(logicalFilter.FilterLeft), GetSearchExpressionFromFilters(logicalFilter.FilterRight));
        //        else if (logicalFilter.Operator == Feng.Search.LogicalOperator.Not)
        //            return SearchExpression.Not(GetSearchExpressionFromFilters(logicalFilter.FilterLeft));
        //    }
        //    else
        //    {
        //        Feng.Grid.Filter.ComparisonFilter comparisonFilter = filter as Feng.Grid.Filter.ComparisonFilter;
        //        switch (comparisonFilter.ComparisonType)
        //        {
        //            case Filter.ComparisonType.Eq:
        //                return SearchExpression.Eq(
        //        }
        //    }
        //}
    }
}
