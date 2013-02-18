using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Xceed.Grid;
using Xceed.Grid.Reporting;
using Feng.Windows.Forms;
using Feng.Utils;

namespace Feng.Grid
{
    /// <summary>
    /// 关于Grid样式的帮助类
    /// </summary>
    public static class GridStyleSheetExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetGridStyles()
        {
            List<string> list = new List<string>();
            PropertyInfo[] properties = typeof(StyleSheet).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.FullName == "Xceed.Grid.StyleSheet")
                {
                    list.Add(property.Name);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 读入保存的样式
        /// </summary>
        public static void LoadStyleSheet(this IGrid grid, AMS.Profile.IProfile profile)
        {
            string styleSheetName = profile.GetValue("Grid.StyleSheet." + grid.GridName, "Name", "");
            if (string.IsNullOrEmpty(styleSheetName))
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="styleSheetName"></param>
        public static void ApplyStyleSheet(this IGrid grid, string styleSheetName)
        {
            if (string.IsNullOrEmpty(styleSheetName))
            {
                grid.ApplyStyleSheet(null);
            }
            else if (styleSheetName == "Custom")
            {
                grid.ApplyStyleSheet(CustomStyleSheet);
            }
            else
            {
                Xceed.Grid.StyleSheet styleSheet = typeof(Xceed.Grid.StyleSheet).InvokeMember(styleSheetName,
                    System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                    null, null, null, null, null, null) as Xceed.Grid.StyleSheet;
                grid.ApplyStyleSheet(styleSheet);
            }
        }

        /// <summary>
        /// 应用样式
        /// </summary>
        /// <param name="combo">WinComboBox</param>
        /// <param name="styleSheet">样式</param>
        public static void ApplyStyleSheet(this Xceed.Editors.WinComboBox combo, StyleSheet styleSheet)
        {
            GridControl gridControl = combo.DropDownControl;
            BorderStyle oldBorderStyle = gridControl.BorderStyle;

            gridControl.ApplyStyleSheet(styleSheet);

            gridControl.InactiveSelectionBackColor = gridControl.SelectionBackColor;
            gridControl.InactiveSelectionForeColor = gridControl.SelectionForeColor;
            gridControl.BorderStyle = oldBorderStyle;
        }

        private static StyleSheet s_customStyleSheet;
        /// <summary>
        /// 自定义样式
        /// </summary>
        internal static StyleSheet CustomStyleSheet
        {
            get
            {
                if (s_customStyleSheet == null)
                {
                    CreateCustomStyleSheet();
                }
                return s_customStyleSheet;
            }
        }
        private static void CreateCustomStyleSheet()
        {
            s_customStyleSheet = new StyleSheet();
            s_customStyleSheet.Grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            s_customStyleSheet.Grid.BackColor = Color.White;
            s_customStyleSheet.Grid.ForeColor = Color.Black;
            s_customStyleSheet.Grid.Font = new Font("Verdana", 8.25f);
            s_customStyleSheet.Grid.SelectionBackColor = Color.FromArgb(87, 104, 169);
            s_customStyleSheet.Grid.SelectionForeColor = Color.White;
            s_customStyleSheet.Grid.InactiveSelectionBackColor = Color.FromArgb(176, 183, 214);
            s_customStyleSheet.Grid.InactiveSelectionForeColor = Color.Black;
            s_customStyleSheet.Grid.GridLineColor = Color.LightGray;

            s_customStyleSheet.FixedFooter.ForeColor = Color.FromArgb(23, 40, 111);
            s_customStyleSheet.FixedFooter.BackColor = Color.FromArgb(246, 246, 251);
            s_customStyleSheet.FixedFooter.Font = new Font(s_customStyleSheet.Grid.Font, FontStyle.Bold);
            s_customStyleSheet.FixedHeader.ForeColor = s_customStyleSheet.FixedFooter.ForeColor;
            s_customStyleSheet.FixedHeader.BackColor = s_customStyleSheet.FixedFooter.BackColor;
            s_customStyleSheet.FixedHeader.Font = s_customStyleSheet.FixedFooter.Font;

            s_customStyleSheet.GridFooter.BackColor = Color.FromArgb(246, 246, 251);
            s_customStyleSheet.GridHeader.BackColor = s_customStyleSheet.GridFooter.BackColor;

            s_customStyleSheet.ColumnManagerRow.BackColor = Color.FromArgb(232, 233, 239);
            s_customStyleSheet.ColumnManagerRow.ForeColor = Color.FromArgb(136, 53, 1);
            s_customStyleSheet.ColumnManagerRow.Font = new Font(s_customStyleSheet.Grid.Font, FontStyle.Bold);
            s_customStyleSheet.RowSelectorPane.BackColor = s_customStyleSheet.ColumnManagerRow.BackColor;

            s_customStyleSheet.GroupByRow.BackColor = Color.FromArgb(197, 197, 201);
            s_customStyleSheet.GroupByRow.CellBackColor = s_customStyleSheet.ColumnManagerRow.BackColor;
            s_customStyleSheet.GroupByRow.CellForeColor = s_customStyleSheet.ColumnManagerRow.ForeColor;
            s_customStyleSheet.GroupByRow.CellFont = s_customStyleSheet.ColumnManagerRow.Font;

            s_customStyleSheet.GroupMargin.BackColor = Color.FromArgb(236, 236, 241);
            s_customStyleSheet.GroupManagerRow.BackColor = s_customStyleSheet.GroupMargin.BackColor;

            s_customStyleSheet.DataRows.Add(new VisualGridElementStyle());
            s_customStyleSheet.DataRows[0].ForeColor = Color.FromArgb(23, 40, 111);
            s_customStyleSheet.DataRows[0].BackColor = Color.FromArgb(254, 235, 200);
            s_customStyleSheet.DataRows.Add(new VisualGridElementStyle());
            s_customStyleSheet.DataRows[1].ForeColor = Color.FromArgb(29, 50, 139);
        }
    }

    /// <summary>
    /// Grid关于筛选行的扩展
    /// </summary>
    public static class GridRowsExtention
    {
        /// <summary>
        /// 获取筛选行的可见性
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static bool GetFilterRowVisible(this IGrid grid)
        {
            if (grid.GridHelper.FilterRow != null)
            {
                return grid.GridHelper.FilterRow.Visible;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置筛选行的可见性
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="visible"></param>
        public static void SetFilterRowVisible(this IGrid grid, bool visible)
        {
            grid.GridHelper.FilterRowVisible = visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="visible"></param>
        public static void SetTextFilterRowVisible(this IGrid grid, bool visible)
        {
            grid.GridHelper.TextFilterRowVisible = visible;
        }
    }

    /// <summary>
    /// 关于Grid Column的一些帮助方法
    /// </summary>
    public static class GridColumnExtention
    {
        /// <summary>
        /// Invisible一组Column,和DataTableSchema.GetInvisibleColumns()对应
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnNames"></param>
        /// <param name="visible"></param>
        public static void VisibleColumns(this ISimpleGrid grid, string[] columnNames, bool visible)
        {
            for (int i = 0; i < columnNames.Length; ++i)
            {
                grid.Columns[columnNames[i]].Visible = visible;
            }
        }

        /// <summary>
        /// Visible all column
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="visible"></param>
        public static void VisibleColumns(this ISimpleGrid grid, bool visible)
        {
            for (int i = 0; i < grid.Columns.Count; ++i)
            {
                grid.Columns[i].Visible = visible;
            }
        }

        /// <summary>
        /// Grid's Column排序
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnNames"></param>
        public static void VisibleIndexColumns(this ISimpleGrid grid, string[] columnNames)
        {
            for (int i = 0; i < columnNames.Length; ++i)
            {
                if (grid.Columns[columnNames[i]] == null)
                {
                    throw new ArgumentException("columnName " + columnNames[i] + " is not exist!", "columnNames");
                }

                grid.Columns[columnNames[i]].Visible = true;
                grid.Columns[columnNames[i]].VisibleIndex = i;
            }
        }

        /// <summary>
        /// ReadOnly selected columns
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnNames"></param>
        /// <param name="readOnly"></param>
        public static void ReadOnlyColumns(this ISimpleGrid grid, string[] columnNames, bool readOnly)
        {
            for (int i = 0; i < columnNames.Length; ++i)
            {
                if (grid.Columns[columnNames[i]] == null)
                {
                    throw new ArgumentException("columnName " + columnNames[i] + " is not exist!", "columnNames");
                }
                grid.Columns[columnNames[i]].ReadOnly = readOnly;
            }
        }

        /// <summary>
        /// ReadOnly all columns
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="readOnly"></param>
        public static void ReadOnlyColumns(this ISimpleGrid grid, bool readOnly)
        {
            for (int i = 0; i < grid.Columns.Count; ++i)
            {
                grid.Columns[i].ReadOnly = readOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static Columns.CheckColumn AddCheckColumn(this IGrid grid)
        {
            var column = AddCheckColumn(grid, Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName);
            column.VisibleIndex = 0;
            column.Fixed = true;

            return column;
        }

        /// <summary>
        /// 添加选择（CheckBox）栏
        /// </summary>
        /// <param name="grid"></param>
        public static Columns.CheckColumn AddCheckColumn(this IGrid grid, string columnName)
        {
            Columns.CheckColumn column = new Columns.CheckColumn(grid, columnName);
            grid.Columns.Insert(0, column);
            
            column.Width = 35;
            column.ReadOnly = false;
            grid.DataRowTemplate.Cells[columnName].ReadOnly = false;
            column.CheckColumnHelper.Initialize();
            return column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column"></param>
        public static void AddCheckColumn(this IGrid grid, Xceed.Grid.Column column)
        {
            Feng.Grid.Columns.CheckColumnHelper helper = new Columns.CheckColumnHelper(column);
            helper.Initialize();
        }

        ///// <summary>
        ///// 改变DataRowTemplate为ButtonDataRowTemplate, 为AddButtonColumn做准备
        ///// </summary>
        //public ButtonDataRow ChangeToButtonDataRowTemplate()
        //{
        //    if (!(this.DataRowTemplate is ButtonDataRow))
        //    {
        //        ButtonDataRow buttonDataRow = new ButtonDataRow();
        //        this.DataRowTemplate = buttonDataRow;
        //    }
        //    return this.DataRowTemplate as ButtonDataRow;
        //}

        /// <summary>
        /// 添加按钮（Button）栏
        /// 之前需Call ChangeToButtonDataRowTemplate()
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static Columns.ButtonColumn AddButtonColumn(this IGrid grid, string caption)
        {
            Columns.ButtonColumn column = new Columns.ButtonColumn(caption);
            grid.Columns.Add(column);
            return column;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class GridLayoutExtention
    {
        public static string GetGridDefaultDataDirectory(this IGrid grid)
        {
            string dirPath = SystemDirectory.UserDataDirectory + (grid.FindForm().Name + "\\") + (grid.GridName + "\\");
            return dirPath;
        }

        public static string GetGridGlobalDataDirectory(this IGrid grid)
        {
            string dirPath = SystemDirectory.GlobalDataDirectory + (grid.FindForm().Name + "\\") + (grid.GridName + "\\");
            return dirPath;
        }

        //private static string m_layoutDefaultFileName = "system.xmlg.default";
        /// <summary>
        /// 
        /// </summary>
        public static bool LoadLayout(this IProfileLayoutControl grid)
        {
            return LayoutControlExtention.LoadLayout(grid);
        }

        public static bool SaveLayout(this IProfileLayoutControl grid)
        {
            return LayoutControlExtention.SaveLayout(grid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="minLevel"></param>
        /// <returns></returns>
        public static bool LoadLayout(this IGrid grid, int minLevel)
        {
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(GetGridDefaultDataDirectory(grid) + MyGrid.m_layoutDefaultFileName);
            return LoadLayout(grid, profile, minLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool LoadLayout(this IGrid grid, string fileName)
        {
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(fileName);
            return LoadLayout(grid, profile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static bool LoadLayout(this IGrid grid, AMS.Profile.IProfile profile)
        {
            grid.LoadStyleSheet(profile);

            return LoadLayout(grid, profile, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="profile"></param>
        /// <param name="minLevel"></param>
        /// <returns></returns>
        public static bool LoadLayout(this IGrid grid, AMS.Profile.IProfile profile, int minLevel)
        {
            // 在未初始化前，不读入
            if (grid.Columns.Count == 0)
            {
                return false;
            }

            bool ret = true;
            try
            {
                grid.BeginInit();

                if (0 >= minLevel)
                {
                    Form form = grid.FindForm();
                    ret = grid.LoadLayout("MyGrid." + (form != null ? form.Name : "") + "." + grid.GridName + ".Layout", profile);
                }

                LoadLayoutDetailGrid(grid, 1, grid.DataRows, profile, minLevel);

            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            finally
            {
                grid.EndInit();
            }

            return ret;
        }


        private static bool LoadLayoutDetailGrid(this IGrid grid, int level, Xceed.Grid.Collections.DataRowList rowList, AMS.Profile.IProfile profile, int minLevel)
        {
            bool ret = true;
            if (rowList.Count > 0)
            {
                foreach (MyDetailGrid detailGrid in rowList[0].DetailGrids)
                {
                    if (level >= minLevel)
                    {
                        Form form = grid.FindForm();
                        ret &= detailGrid.LoadLayout("MyGrid." + (form != null ? form.Name : "") + "." + grid.GridName + "." + level.ToString() + "." + detailGrid.GridName + ".Layout", profile);
                    }

                    LoadLayoutDetailGrid(grid, level + 1, detailGrid.DataRows, profile, minLevel);
                }
            }
            return ret;
        }

        /// <summary>
        /// 读入保存的Column布局
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        private static bool LoadLayout(this IGrid grid, string sectionName, AMS.Profile.IProfile profile)
        {
            string s = profile.GetValue(sectionName, "Column", "");
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            string[] columns = s.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string columnName in columns)
            {
                string[] ss = columnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length != 5)
                {
                    continue;
                }
                if (grid.Columns[ss[0]] == null)
                {
                    continue;
                }

                Xceed.Grid.Column column = grid.Columns[ss[0]];
                // 默认是-1，设置成0是gridcolumnInfo 设置成Invisible
                if (column != null && column.MaxWidth != 0) 
                {
                    column.Visible = Convert.ToBoolean(ss[1]);
                    column.VisibleIndex = Convert.ToInt32(ss[2]);
                    column.Width = Convert.ToInt32(ss[3]);
                    column.Fixed = Convert.ToBoolean(ss[4]);
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fileName"></param>
        public static bool SaveLayout(this IGrid grid, string fileName)
        {
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(fileName);
            return SaveLayout(grid, profile);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool SaveLayout(this IGrid grid, AMS.Profile.IProfile profile)
        {
            Form form = grid.FindForm();
            bool ret = SaveLayout(grid, "MyGrid." + (form != null ? form.Name : "") + "." + grid.GridName + ".Layout", profile);

            ret &= SaveLayoutDetailGrid(grid, 1, grid.DataRows, profile);
            return ret;
        }

        private static bool SaveLayoutDetailGrid(this IGrid grid, int level, Xceed.Grid.Collections.DataRowList rowList, AMS.Profile.IProfile profile)
        {
            bool ret = true;
            if (rowList.Count > 0)
            {
                foreach (MyDetailGrid detailGrid in rowList[0].DetailGrids)
                {
                    Form form = grid.FindForm();
                    ret &= detailGrid.SaveLayout("MyGrid." + (form != null ? form.Name : "") + "." + grid.GridName + "." + level.ToString() + "." + detailGrid.GridName + ".Layout", profile);

                    ret &= SaveLayoutDetailGrid(grid, level + 1, detailGrid.DataRows, profile);
                }
            }
            return ret;
        }

        /// <summary>
        /// 保存Column布局
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sectionName"></param>
        private static bool SaveLayout(this IGrid grid, string sectionName, AMS.Profile.IProfile profile)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < grid.Columns.Count; ++i)
            {
                sb.Append(grid.Columns[i].FieldName);
                sb.Append(",");
                sb.Append(grid.Columns[i].Visible);
                sb.Append(",");
                sb.Append(grid.Columns[i].VisibleIndex);
                sb.Append(",");
                sb.Append(grid.Columns[i].Width);
                sb.Append(",");
                sb.Append(grid.Columns[i].Fixed);
                sb.Append(System.Environment.NewLine);
            }
            profile.SetValue(sectionName, "Column", sb.ToString());
            return true;
        }

        private const int m_maxColumnWidth = 400;
        private const int m_minColumnWidth = 10;

        /// <summary>
        /// 根据内容自动设置column width（包括DetailGrid）
        /// </summary>
        public static int AutoAdjustColumnWidth(this ISimpleGrid grid)
        {
            int ret = GridUtils.ReCalculateColumnWidth(grid.Columns, m_maxColumnWidth, m_minColumnWidth, grid.Width);

            AutoAdjustColumnWidthDetailGrid(grid);

            return ret;
        }

        private static void AutoAdjustColumnWidthDetailGrid(this ISimpleGrid grid)
        {
            if (grid.DataRows.Count > 0)
            {
                foreach (MyDetailGrid detailGrid in grid.DataRows[0].DetailGrids)
                {
                    GridUtils.ReCalculateColumnWidth(detailGrid.Columns, m_maxColumnWidth, m_minColumnWidth, grid.Width);

                    AutoAdjustColumnWidthDetailGrid(detailGrid);
                }
            }
        }

        /// <summary>
        /// 根据内容自动设置column width（不包括DetailGrid）
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static int AutoAdjustColumnWidth(Xceed.Grid.Collections.ColumnList columns, int gridWidth)
        {
            int ret = GridUtils.ReCalculateColumnWidth(columns, m_maxColumnWidth, m_minColumnWidth, gridWidth);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        private static void LoadDefaultLayout(this IGrid grid)
        {
            grid.LoadGroupLayout();

            if (!grid.GridHelper.m_firstLoad)
            {
                ILayoutControl layout = grid as ILayoutControl;
                if (layout != null)
                {
                    if (!layout.LoadLayout())
                    {
                        // maybe too slow
                        grid.AutoAdjustColumnWidth();
                    }
                    else
                    {
                        grid.GridHelper.m_firstLoad = true;
                    }
                }
            }
        }
        /// <summary>
        /// 读入数据后，设置Column宽度，设置条数显示，填充filter
        /// </summary>
        public static void AfterLoadData(this IGrid grid)
        {
            // not for detailGrid
            if (grid.FindForm() == null)
                return;

            // 如果有Filter则Apply
            // 先填充Filter
            if (grid.GridHelper.FilterRow != null)
            {
                grid.GridHelper.FilterRow.FillFilters();

                // 不再清空，而是重新筛选
                // this.m_filterRow.ClearFilters();
                grid.GridHelper.FilterRow.ApplyFilters();
            }

            LoadDefaultLayout(grid);

            if (grid.DataRows.Count > 0 && grid is GridControl)
            {
                //(grid as GridControl).MoveCurrentRow(VerticalDirection.Top, GridSection.Body);

                //// when DataRow[0] is invisible, there is an exception
                MyGrid.SetCurrentRow(grid as GridControl, grid.DataRows[0]);

                MyGrid.SyncSelectedRowToCurrentRow(grid as GridControl);
            }

            //this.ShowTitleRow();
        }
    }

    /// <summary>
    /// Grid关于Group的扩展
    /// </summary>
    public static class GridGroupExtention
    {
        /// <summary>
        /// 清除group
        /// </summary>
        public static void ClearGroups(this IGrid grid)
        {
            grid.GroupTemplates.Clear();
        }

        /// <summary>
        /// 加入Group
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="group"></param>
        /// <param name="groupRow"></param>
        /// <param name="groupBy"></param>
        /// <param name="groupTitleFormat"></param>
        /// <param name="groupCollapsed"></param>
        /// <returns></returns>
        private static Group AddGroup(this IGrid grid, Group group, GroupManagerRow groupRow, string groupBy, string groupTitleFormat,
                               bool groupCollapsed)
        {
            groupRow.CanBeCurrent = false;
            groupRow.CanBeSelected = false;
            groupRow.TitleFormat = groupTitleFormat;
            group.HeaderRows.Add(groupRow);
            group.GroupBy = groupBy;
            group.Collapsed = groupCollapsed;
            grid.GroupTemplates.Add(group);

            // 每次改变Group后都要Update
            grid.UpdateGrouping();

            if (group.Level == 0)
            {
                group.CollapsedChanged += new EventHandler(grid.GridHelper.group_CollapsedChanged);
            }

            return group;
        }

        /// <summary>
        /// 加入Group
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="groupRow"></param>
        /// <param name="groupBy"></param>
        /// <param name="groupTitleFormat"></param>
        /// <param name="groupCollapsed"></param>
        /// <returns></returns>
        public static Group AddGroup(this IGrid grid, GroupManagerRow groupRow, string groupBy, string groupTitleFormat, bool groupCollapsed)
        {
            return AddGroup(grid, new Group(), groupRow, groupBy, groupTitleFormat, groupCollapsed);
        }

        /// <summary>
        /// 加入Group
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="groupRow"></param>
        /// <param name="groupBy"></param>
        /// <param name="groupTitleFormat"></param>
        /// <returns></returns>
        public static Group AddGroup(this IGrid grid, GroupManagerRow groupRow, string groupBy, string groupTitleFormat)
        {
            return AddGroup(grid, groupRow, groupBy, groupTitleFormat, false);
        }

        /// <summary>
        /// 加入Group
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="groupBy"></param>
        /// <param name="groupTitleFormat"></param>
        /// <returns></returns>
        public static Group AddGroup(this IGrid grid, string groupBy, string groupTitleFormat)
        {
            return AddGroup(grid, new MyGroupManagerRow(), groupBy, groupTitleFormat);
        }

        /// <summary>
        /// 读入Group Collapse状态
        /// </summary>
        /// <param name="grid"></param>
        public static void LoadGroupLayout(this IGrid grid)
        {
            //foreach (Group group in grid.Groups)
            //{
            //    string str = GridHelper.GenerateGroupSavedKey(group);
            //    if (GridHelper.GroupCollapseState != null
            //        && GridHelper.GroupCollapseState.ContainsKey(str))
            //    {
            //        group.Collapsed = GridHelper.GroupCollapseState[str];
            //        break;
            //    }
            //}
        }
    }
        

    /// <summary>
    /// 关于DataRow的Extention
    /// </summary>
    public static class GridAddDataRowExtention
    {
        /// <summary>
        /// 加入AddTextRow
        /// </summary>
        /// <returns></returns>
        public static TextRow AddTextRowToFixedFooter(this IGrid grid)
        {
            TextRow row = new TextRow();
            row.CanBeCurrent = false;
            row.CanBeSelected = false;
            grid.FixedFooterRows.Add(row);
            return row;
        }

        /// <summary>
        /// 加入ValueRow
        /// </summary>
        /// <returns></returns>
        public static ValueRow AddValueRowToFixedFooter(this IGrid grid)
        {
            ValueRow valueRow = new ValueRow();
            valueRow.CanBeCurrent = false;
            valueRow.CanBeSelected = false;
            valueRow.BackColor = Color.LightBlue;
            grid.FixedFooterRows.Add(valueRow);
            return valueRow;
        }

        /// <summary>
        /// 加入DetailGrid
        /// </summary>
        /// <returns></returns>
        public static MyDetailGrid AddDetailGrid(this IGrid grid, MyDetailGrid detailGrid)
        {
            detailGrid.Collapsed = true;
            grid.DetailGridTemplates.Add(detailGrid);
            grid.UpdateDetailGrids();
            return detailGrid;
        }


        /// <summary>
        /// 删除DetailGrid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="detail"></param>
        public static void RemoveDetailGrid(this IGrid grid, MyDetailGrid detail)
        {
            grid.DetailGridTemplates.Remove(detail);
            grid.UpdateDetailGrids();
        }



        /// <summary>
        /// 把DataTable的值加入到Grid中
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dt"></param>
        /// <param name="setValues"></param>
        public static void AddDataTableRows(this IGrid grid, System.Data.DataTable dt, SetOtherRowValues setValues)
        {
            if (dt == null)
            {
                return;
            }

            Xceed.Grid.DataRow row;
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                row = AddDataRow(grid, dt.Rows[i]);

                if (setValues != null)
                {
                    setValues(row);
                }
            }
        }

        /// <summary>
        /// 把DataTable的值加入到Grid中
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dt"></param>
        public static void AddDataTableRows(this IGrid grid, System.Data.DataTable dt)
        {
            AddDataTableRows(grid, dt, null);
        }

        /// <summary>
        /// 加入某一行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dataRow"></param>
        public static Xceed.Grid.DataRow AddDataRow(this IGrid grid, System.Data.DataRow dataRow)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException("dataRow");
            }
            Xceed.Grid.DataRow row = grid.DataRows.AddNew();
            foreach (Xceed.Grid.Column column in grid.Columns)
            {
                string columnName = column.FieldName;
                if (dataRow.Table.Columns.Contains(columnName))
                {
                    if (grid.Columns[columnName].DataType.FullName == "System.String")
                    {
                        row.Cells[columnName].Value = dataRow[columnName].ToString();
                    }
                    else
                    {
                        row.Cells[columnName].Value = dataRow[columnName] == System.DBNull.Value
                                                          ? null
                                                          : dataRow[columnName];
                    }
                }
            }
            row.EndEdit();

            return row;
        }


        /// <summary>
        /// AddDataTableRow时设置其他值
        /// </summary>
        /// <param name="row"></param>
        public delegate void SetOtherRowValues(Xceed.Grid.DataRow row);

        /// <summary>
        /// 加入DataTable到DetailGrid中
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dt"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="setValues"></param>
        public static void AddDetailDataTableRows(this IGrid grid, System.Data.DataTable dt, string primaryKeyName, SetOtherRowValues setValues)
        {
            System.Collections.Generic.Dictionary<string, Xceed.Grid.DataRow> masterRows =
                new System.Collections.Generic.Dictionary<string, Xceed.Grid.DataRow>();
            foreach (Xceed.Grid.DataRow row in grid.DataRows)
            {
                if (!masterRows.ContainsKey(row.Cells[primaryKeyName].Value.ToString()))
                {
                    masterRows[row.Cells[primaryKeyName].Value.ToString()] = row;
                }
            }

            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                Xceed.Grid.DataRow row;
                if (!masterRows.ContainsKey(dt.Rows[i][primaryKeyName].ToString()))
                {
                    row = grid.AddDataRow(dt.Rows[i]);
                    masterRows[dt.Rows[i][primaryKeyName].ToString()] = row;
                }

                MyDetailGrid detailGrid =
                    masterRows[dt.Rows[i][primaryKeyName].ToString()].DetailGrids[0] as MyDetailGrid;
                row = detailGrid.AddDataRow(dt.Rows[i]);

                if (setValues != null)
                {
                    setValues(row);
                }
            }
        }

        /// <summary>
        /// 加入DataTable到DetailGrid中
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dt"></param>
        /// <param name="primaryKeyName"></param>
        public static void AddDetailDataTableRows(this IGrid grid, System.Data.DataTable dt, string primaryKeyName)
        {
            AddDetailDataTableRows(grid, dt, primaryKeyName, null);
        }

    }

    /// <summary>
    /// Grid默认的一些属性事件
    /// </summary>
    public static class GridEventsExtention
    {
        /// <summary>
        /// RemoveDefaultEvents
        /// </summary>
        internal static void RemoveDefaultEvents(this IGrid grid)
        {
            //grid.DataRowTemplate.RowSelector.DoubleClick -= new EventHandler(grid.GridHelper.RowSelector_DoubleClick);

            foreach (Xceed.Grid.DataCell cell in grid.DataRowTemplate.Cells)
            {
                //cell.DoubleClick -= new System.EventHandler(grid.GridHelper.DataRow_DoubleClick);
                cell.MouseEnter -= new System.EventHandler(grid.GridHelper.Cell_MouseEnter);
                cell.MouseLeave -= new System.EventHandler(grid.GridHelper.Cell_MouseLeave);
            }

            // 右键
            grid.DataRowTemplate.RowSelector.MouseDown -= new MouseEventHandler(grid.GridHelper.RowSelector_MouseDown);
            foreach (Xceed.Grid.DataCell cell in grid.DataRowTemplate.Cells)
            {
                cell.MouseDown -= new System.Windows.Forms.MouseEventHandler(grid.GridHelper.cell_MouseDown);
            }

            foreach (Xceed.Grid.DataCell cell in grid.DataRowTemplate.Cells)
            {
                if (cell.ParentColumn.DataType == typeof(DateTime) || cell.ParentColumn.DataType == typeof(DateTime?))
                {
                    cell.DoubleClick -= new EventHandler(DateTimeCell_DoubleClick);
                }
            }

            foreach (Xceed.Grid.Group group in grid.Groups)
            {
                group.CollapsedChanged -= new EventHandler(grid.GridHelper.group_CollapsedChanged);
            }

        }

        /// <summary>
        /// ApplyDefaultEvents
        /// </summary>
        internal static void ApplyDefaultEvents(this IGrid grid)
        {
            //grid.DataRowTemplate.RowSelector.DoubleClick -= new EventHandler(grid.GridHelper.RowSelector_DoubleClick);
            //grid.DataRowTemplate.RowSelector.DoubleClick += new EventHandler(grid.GridHelper.RowSelector_DoubleClick);

            foreach (Xceed.Grid.DataCell cell in grid.DataRowTemplate.Cells)
            {
                //    // 如果不先删除，会重复添加
                //    cell.DoubleClick -= new System.EventHandler(grid.GridHelper.DataRow_DoubleClick);
                //    cell.DoubleClick += new System.EventHandler(grid.GridHelper.DataRow_DoubleClick);

                cell.MouseEnter -= new System.EventHandler(grid.GridHelper.Cell_MouseEnter);
                cell.MouseEnter += new System.EventHandler(grid.GridHelper.Cell_MouseEnter);
                cell.MouseLeave += new System.EventHandler(grid.GridHelper.Cell_MouseLeave);
                cell.MouseLeave -= new System.EventHandler(grid.GridHelper.Cell_MouseLeave);
            }

            // 右键
            grid.DataRowTemplate.RowSelector.MouseDown -= new MouseEventHandler(grid.GridHelper.RowSelector_MouseDown);
            grid.DataRowTemplate.RowSelector.MouseDown += new MouseEventHandler(grid.GridHelper.RowSelector_MouseDown);
            foreach (Xceed.Grid.DataCell cell in grid.DataRowTemplate.Cells)
            {
                cell.MouseDown -= new System.Windows.Forms.MouseEventHandler(grid.GridHelper.cell_MouseDown);
                cell.MouseDown += new System.Windows.Forms.MouseEventHandler(grid.GridHelper.cell_MouseDown);
            }

            foreach (Xceed.Grid.DataCell cell in grid.DataRowTemplate.Cells)
            {
                if (cell.ParentColumn.DataType == typeof(DateTime) || cell.ParentColumn.DataType == typeof(DateTime?))
                {
                    cell.DoubleClick -= new EventHandler(DateTimeCell_DoubleClick);
                    cell.DoubleClick += new EventHandler(DateTimeCell_DoubleClick);
                }
            }
        }

        private static void DateTimeCell_DoubleClick(object sender, EventArgs e)
        {
            Xceed.Grid.DataCell cell = sender as Xceed.Grid.DataCell;
            if (!cell.IsBeingEdited)
            {
                return;
            }

            cell.LeaveEdit(true);

            if (cell.CellEditorManager.GetType() == typeof(Editors.DateTimeEditor))
            {
                cell.Value = System.DateTime.Now;
            }
            else
            {
                cell.Value = System.DateTime.Today;
            }
        }


        /// <summary>
        /// 设置Grid的基本属性,
        /// </summary>
        public static void ApplyDefaultSettings(this IGrid grid)
        {
            grid.DataRowTemplate.BackColor = Color.FromArgb(0, 255, 255, 255);
        }
    }

    /// <summary>
    /// 关于Grid ManageRow的一些扩展
    /// </summary>
    public static class GridManageRowExtention
    {
        /// <summary>
        /// 得到GroupByRow
        /// </summary>
        /// <returns></returns>
        public static MyGroupByRow GetGroupByRow(this IGrid grid)
        {
            foreach (Row row in grid.FixedHeaderRows)
            {
                MyGroupByRow r = row as MyGroupByRow;
                if (r != null)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到ColumnManageRow
        /// </summary>
        /// <returns></returns>
        public static ColumnManagerRow GetColumnManageRow(this IGrid grid)
        {
            foreach (Row row in grid.FixedHeaderRows)
            {
                ColumnManagerRow r = row as ColumnManagerRow;
                if (r != null)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到InsertionRow
        /// </summary>
        /// <returns></returns>
        public static InsertionRow GetInsertionRow(this IGrid grid)
        {
            foreach (Row row in grid.FixedFooterRows)
            {
                InsertionRow r = row as InsertionRow;
                if (row != null)
                {
                    return r;
                }
            }
            foreach (Row row in grid.FixedHeaderRows)
            {
                InsertionRow r = row as InsertionRow;
                if (row != null)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到SummaryRow
        /// </summary>
        /// <returns></returns>
        public static SummaryRow GetSummaryRow(this IGrid grid)
        {
            foreach (Row row in grid.FixedFooterRows)
            {
                SummaryRow r = row as SummaryRow;
                if (r != null)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到ValueRow
        /// </summary>
        /// <returns></returns>
        public static ValueRow GetValueRow(this IGrid grid)
        {
            foreach (Row row in grid.FixedFooterRows)
            {
                ValueRow r = row as ValueRow;
                if (r != null)
                {
                    return r;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Grid一些帮助工具
    /// </summary>
    public class GridUtils
    {
        /// <summary>
        /// 保存内部ReportStyleSheet为xml文件
        /// </summary>
        public static void SaveInnerReportStyleSheet()
        {
            SaveInnerReportStyleSheet(SystemDirectory.DataDirectory);
        }

        /// <summary>
        /// 保存内部ReportStyleSheet为xml文件
        /// </summary>
        private static void SaveInnerReportStyleSheet(string folderPath)
        {
            IOHelper.TryCreateDirectory(folderPath);

            ReportStyleSheet.Contemporary.Save(folderPath + "Contemporary.xml", true);
            ReportStyleSheet.CorporateAerated.Save(folderPath + "CorporateAerated.xml", true);
            ReportStyleSheet.CorporateCompact.Save(folderPath + "CorporateCompact.xml", true);
            ReportStyleSheet.CorporateConfidential.Save(folderPath + "CorporateConfidential.xml", true);
            ReportStyleSheet.CorporateLined.Save(folderPath + "CorporateLined.xml", true);
            ReportStyleSheet.SoftGray.Save(folderPath + "SoftGray.xml", true);
            ReportStyleSheet.SteelBlue.Save(folderPath + "SteelBlue.xml", true);
            ReportStyleSheet.Typewriter.Save(folderPath + "Typewriter.xml", true);
        }

        /// <summary>
        /// 根据内容自动设置column width
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="maxColumnWidth"></param>
        /// <param name="minColumnWidth"></param>
        /// <param name="gridWidth"></param>
        internal static int ReCalculateColumnWidth(Xceed.Grid.Collections.ColumnList columns, int maxColumnWidth, int minColumnWidth, int gridWidth)
        {
            if (columns.Count < 1)
            {
                return 0;
            }

            // auto fit column width
            int[] columnWidths = new int[columns.Count];
            int allWidth = 0;
            int visibleColumns = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Visible && !columns[i].Fixed)
                {
                    try
                    {
                        columnWidths[i] = columns[i].GetFittedWidth();
                        columnWidths[i] = Math.Min(maxColumnWidth, columnWidths[i]);
                        columnWidths[i] = Math.Max(minColumnWidth, columnWidths[i]);

                        allWidth += columnWidths[i];
                    }
                    // GetFittedWidth() maybe throw exception
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                    }
                    
                    visibleColumns++;
                }

                if (columns[i].Fixed)
                {
                    allWidth += columns[i].Width;
                }
            }

            // 填满整个宽度
            const int delta = 100;
            if (allWidth < gridWidth - delta)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i].Visible && !columns[i].Fixed)
                    {
                        columnWidths[i] = columnWidths[i] + (gridWidth - delta - allWidth) / visibleColumns;
                    }
                }

                allWidth = gridWidth - delta;
            }

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Visible && !columns[i].Fixed)
                {
                    if (columns[i].MaxWidth > 0)
                    {
                        columnWidths[i] = Math.Min(columns[i].MaxWidth, columnWidths[i]);
                    }
                    if (columns[i].MinWidth > 0)
                    {
                        columnWidths[i] = Math.Max(columns[i].MinWidth, columnWidths[i]);
                    }

                    columns[i].Width = columnWidths[i];
                }
            }

            return allWidth;
        }
    }
}
