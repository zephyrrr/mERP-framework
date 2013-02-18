using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;

namespace Feng.Grid
{
    /// <summary>
    /// ArchiveGridExtention
    /// </summary>
    public static class ArchiveGridExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="cm"></param>
        public static void SetControlManager(this IArchiveGrid grid, IControlManager cm)
        {
            SetControlManager(grid, cm, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="cm"></param>
        /// <param name="gridName"></param>
        public static void SetControlManager(this IArchiveGrid grid, IControlManager cm, string gridName)
        {
            grid.ControlManager = cm;

            grid.GridName = gridName;
            grid.SetGridPermissions();

            grid.SetDisplayManager(cm.DisplayManager, gridName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static bool CheckControlValue(this IArchiveGrid grid)
        {
            bool ret = true;
            System.ComponentModel.CancelEventArgs e = new System.ComponentModel.CancelEventArgs();
            if (grid.ArchiveGridHelper.InsertionRow != null && grid.ArchiveGridHelper.InsertionRow.IsBeingEdited)
            {
                ret &= grid.ArchiveGridHelper.DoValidateRow(grid.ArchiveGridHelper.InsertionRow, e);
                if (!ret)
                {
                    return false;
                }
            }
            foreach (Xceed.Grid.DataRow row in grid.DataRows)
            {
                ret &= grid.ArchiveGridHelper.DoValidateRow(row, e);
                if (!ret)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 创建Group
        /// </summary>
        internal static void CreateArchiveEvents(this IArchiveGrid grid)
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
                            Xceed.Grid.InsertionRow insertRow = grid.GetInsertionRow();
                            if (insertRow != null)
                            {
                                insertRow.Cells[info.GridColumnName].DoubleClick += new EventHandler(delegate(object sender, EventArgs e)
                                {
                                    EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(info2.DoubleClick), sender, e);
                                });
                            }
                        }
                        break;
                    //default:
                    //    throw new InvalidOperationException("Invalide gridcolumnType of " + info.GridColumnType + " in " + info.Name);
                }
            }
        }

        /// <summary>
        /// 把Grid ColumnManagerRow中非空的Column设置为红色
        /// </summary>
        /// <param name="grid"></param>
        public static void SetColumnManagerRowNotNull(this IGrid grid)
        {
            if (string.IsNullOrEmpty(Const.LabelNotNullPreFix))
                return;

            var columnManagerRow = grid.GetColumnManageRow();
            if (columnManagerRow != null)
            {
                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                {
                    switch (info.GridColumnType)
                    {
                        case GridColumnType.Normal:
                            if (Authority.AuthorizeByRule(info.NotNull))
                            {
                                //columnManagerRow.Cells[info.GridColumnName].ForeColor = System.Drawing.Color.Red;
                                if (!grid.Columns[info.GridColumnName].Title.StartsWith(Const.LabelNotNullPreFix))
                                {
                                    grid.Columns[info.GridColumnName].Title = Const.LabelNotNullPreFix + grid.Columns[info.GridColumnName].Title;
                                }
                            }
                            else
                            {
                                if (grid.Columns[info.GridColumnName].Title.StartsWith(Const.LabelNotNullPreFix))
                                {
                                    grid.Columns[info.GridColumnName].Title = grid.Columns[info.GridColumnName].Title.Substring(Const.LabelNotNullPreFix.Length);
                                }
                            }
                            break;
                    }
                }
            }
        }

        internal static void SetGridPermissions(this IArchiveGrid grid)
        {
            GridInfo info = ADInfoBll.Instance.GetGridInfo(grid.GridName);
            if (!string.IsNullOrEmpty(info.AllowInnerInsert))
            {
                grid.AllowInnerInsert = Authority.AuthorizeByRule(info.AllowInnerInsert);
            }
            if (!string.IsNullOrEmpty(info.AllowInnerEdit))
            {
                grid.AllowInnerEdit = Authority.AuthorizeByRule(info.AllowInnerEdit);
            }
            if (!string.IsNullOrEmpty(info.AllowInnerDelete))
            {
                grid.AllowInnerDelete = Authority.AuthorizeByRule(info.AllowInnerDelete);
            }

            if (!string.IsNullOrEmpty(info.AllowInsert))
            {
                grid.ControlManager.AllowInsert = Authority.AuthorizeByRule(info.AllowInsert);
            }
            if (!string.IsNullOrEmpty(info.AllowEdit))
            {
                grid.ControlManager.AllowEdit = Authority.AuthorizeByRule(info.AllowEdit);
            }
            if (!string.IsNullOrEmpty(info.AllowDelete))
            {
                grid.ControlManager.AllowDelete = Authority.AuthorizeByRule(info.AllowDelete);
            }
        }

        internal static void SetGridRowCellColors(this IBoundGrid grid, Xceed.Grid.DataRow row)
        {
            GridRowInfo rowInfo = ADInfoBll.Instance.GetGridRowInfo(grid.GridName);
            object entity = row.Tag;

            if (!string.IsNullOrEmpty(rowInfo.BackColor))
            {
                string s = (string)EntityScript.CalculateExpression(rowInfo.BackColor, entity);
                if (!string.IsNullOrEmpty(s))
                {
                    row.BackColor = System.Drawing.Color.FromName(s);
                }
                else
                {
                    row.ResetBackColor();
                }
            }

            if (!string.IsNullOrEmpty(rowInfo.ForeColor))
            {
                string s = (string)EntityScript.CalculateExpression(rowInfo.ForeColor, entity);
                if (!string.IsNullOrEmpty(s))
                {
                    row.ForeColor = System.Drawing.Color.FromName(s);
                }
                else
                {
                    row.ResetForeColor();
                }
            }

            foreach (GridCellInfo cellInfo in ADInfoBll.Instance.GetGridCellInfos(grid.GridName))
            {
                Xceed.Grid.Cell cell = row.Cells[cellInfo.GridColumName];
                if (cell == null)
                    continue;

                if (!string.IsNullOrEmpty(cellInfo.BackColor))
                {
                    string s = (string)EntityScript.CalculateExpression(cellInfo.BackColor, entity);
                    if (!string.IsNullOrEmpty(s))
                    {
                        cell.BackColor = System.Drawing.Color.FromName(s);
                    }
                    else
                    {
                        cell.ResetBackColor();
                    }
                }

                if (!string.IsNullOrEmpty(rowInfo.ForeColor))
                {
                    string s = (string)EntityScript.CalculateExpression(cellInfo.ForeColor, entity);
                    if (!string.IsNullOrEmpty(s))
                    {
                        cell.ForeColor = System.Drawing.Color.FromName(s);
                    }
                    else
                    {
                        cell.ResetForeColor();
                    }
                }
            }
        }

        internal static void SetGridRowVisible(this IBoundGrid grid, Xceed.Grid.DataRow row)
        {
            GridRowInfo info = ADInfoBll.Instance.GetGridRowInfo(grid.GridName);
            object entity = row.Tag;

            bool visible = Permission.AuthorizeByRule(info.Visible, entity);
            row.Visible = visible;
            if (!row.Visible)
            {
                grid.DataRows.Remove(row);
                return;
            }
        }
        internal static void SetGridRowCellPermissions(this IBoundGrid grid, Xceed.Grid.DataRow row)
        {
            GridRowInfo info = ADInfoBll.Instance.GetGridRowInfo(grid.GridName);
            object entity = row.Tag;

            if (grid.ReadOnly)
                return;

            // Only set when readOnly(如果ReadOnly=false，则不设置，继承Parent's ReadOnly）
            bool readOnly = Feng.Permission.AuthorizeByRule(info.ReadOnly, entity);
            if (readOnly)
            {
                row.ReadOnly = readOnly;
            }
            if (row.ReadOnly)
                return;

            foreach (GridCellInfo cellInfo in ADInfoBll.Instance.GetGridCellInfos(grid.GridName))
            {
                Xceed.Grid.Cell cell = row.Cells[cellInfo.GridColumName];
                if (cell == null)
                    continue;

                if (cell.ParentGrid.ReadOnly)
                {
                    continue;
                }
                if (cell.ParentColumn.ReadOnly)
                {
                    continue;
                }
                if (cell.ParentRow.ReadOnly)
                {
                    continue;
                }

                readOnly = Permission.AuthorizeByRule(cellInfo.ReadOnly, entity);
                if (readOnly)
                {
                    cell.ReadOnly = readOnly;
                }
            }
        }

        internal static void SetGridRowCellProperties(this IBoundGrid grid)
        {
            foreach (Xceed.Grid.DataRow row in grid.DataRows)
            {
                SetGridRowCellPermissions(grid, row);
                SetGridRowCellColors(grid, row);
            }
        }
    }
}
