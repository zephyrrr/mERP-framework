using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Feng;
using Feng.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GridFilterToolStripItem : ToolStripDropDownButton
    {
        /// <summary>
        /// 
        /// </summary>
        public GridFilterToolStripItem()
        {
            InitializeComponent();
        }

        private MyGrid[] m_grids;

        private ADInfoBll m_bll = ADInfoBll.Instance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grids"></param>
        /// <param name="gridFilterName"></param>
        public void SetFilters(MyGrid[] grids, string gridFilterName)
        {
            m_grids = grids;

            IList<GridFilterInfo> filters = m_bll.GetGridFilterInfos(gridFilterName);
            foreach (GridFilterInfo filter in filters)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = filter.Name + "toolStripMenuItem";
                item.Size = new System.Drawing.Size(172, 22);
                item.Text = filter.Name;
                item.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
                item.Tag = filter;

                this.DropDownItems.Add(item);
            }
        }

        private Regex m_regex = new Regex(@"\$(.*?)\$", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ToolStripItemClicked;

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            GridFilterInfo filter = item.Tag as GridFilterInfo;

            FilterIt(filter);

            if (ToolStripItemClicked != null)
            {
                ToolStripItemClicked(sender, e);
            }
        }

        internal void FilterIt(GridFilterInfo filter)
        {
            try
            {
                // first for rowExpression
                foreach (MyGrid grid in m_grids)
                {
                    foreach (Xceed.Grid.DataRow row in grid.DataRows)
                    {
                        row.Visible = true;
                    }
                }

                if (!string.IsNullOrEmpty(filter.RowExpression))
                {
                    string[] expressions = filter.RowExpression.Split(new char[] {';'},
                                                                      StringSplitOptions.RemoveEmptyEntries);
                    foreach (string expression in expressions)
                    {
                        int idx = expression.IndexOf(':');
                        if (idx == -1)
                        {
                            throw new ArgumentException("Invalid expression format");
                        }
                        string gridName = expression.Substring(0, idx).Trim();
                        string realExpression = expression.Substring(idx + 1).Trim();

                        MyGrid grid = FindGridByName(gridName);
                        if (grid == null)
                        {
                            throw new ArgumentException("Invalid Grid Name");
                        }

                        EvaluationEngine.Parser.Token token = new EvaluationEngine.Parser.Token(realExpression);
                        MatchCollection mc = m_regex.Matches(realExpression);

                        foreach (Xceed.Grid.DataRow row in grid.DataRows)
                        {
                            Dictionary<string, string> variables = new Dictionary<string, string>();

                            foreach (Match m in mc)
                            {
                                variables[m.Groups[1].Value] = m.Groups[0].Value;
                            }

                            foreach (KeyValuePair<string, string> kvp in variables)
                            {
                                if (row.Cells[kvp.Key] == null)
                                {
                                    throw new ArgumentException("Invalid Grid Cell's Name");
                                }
                                else
                                {
                                    if (row.Cells[kvp.Key].ParentColumn.DataType == typeof (string))
                                    {
                                        token.Variables[kvp.Value].VariableValue =
                                            row.Cells[kvp.Key].GetDisplayText() == null
                                                ? string.Empty
                                                : row.Cells[kvp.Key].GetDisplayText();
                                    }
                                    else
                                    {
                                        token.Variables[kvp.Value].VariableValue = row.Cells[kvp.Key].Value == null
                                                                                       ? string.Empty
                                                                                       : row.Cells[kvp.Key].Value.
                                                                                             ToString();
                                    }
                                }
                            }

                            EvaluationEngine.Evaluate.Evaluator eval = new EvaluationEngine.Evaluate.Evaluator(token);

                            string ErrorMsg = "";
                            string result = "";
                            if (eval.Evaluate(out result, out ErrorMsg) == false)
                            {
                                throw new InvalidOperationException("Error evaluating the tokens: " + ErrorMsg + System.Environment.NewLine
                                    + expression);      
                            }

                            row.Visible = (result == "true");
                        }
                    }
                }


                // Then for columnExpression
                foreach (MyGrid grid in m_grids)
                {
                    foreach (Xceed.Grid.Column column in grid.Columns)
                    {
                        column.Fixed = false;
                    }
                }
                if (!string.IsNullOrEmpty(filter.ColumnExpression))
                {
                    string[] expressions = filter.ColumnExpression.Split(new char[] {';'},
                                                                         StringSplitOptions.RemoveEmptyEntries);
                    foreach (string expression in expressions)
                    {
                        int idx = expression.IndexOf(':');
                        if (idx == -1)
                        {
                            throw new ArgumentException("Invalid expression format");
                        }
                        string gridName = expression.Substring(0, idx).Trim();
                        string realExpression = expression.Substring(idx + 1).Trim();

                        MyGrid grid = FindGridByName(gridName);
                        grid.BeginInit();

                        if (grid == null)
                        {
                            throw new ArgumentException("Invalid Grid Name");
                        }

                        EvaluationEngine.Parser.Token token = new EvaluationEngine.Parser.Token(realExpression);
                        MatchCollection mc = m_regex.Matches(realExpression);

                        foreach (Xceed.Grid.Column column in grid.Columns)
                        {
                            Dictionary<string, string> variables = new Dictionary<string, string>();

                            foreach (Match m in mc)
                            {
                                if (m.Groups[0].Value != "$ColumnName$")
                                {
                                    throw new NotSupportedException("ColumnExpression Format is invalid !");
                                }
                                variables[m.Groups[1].Value] = m.Groups[0].Value;
                            }

                            foreach (KeyValuePair<string, string> kvp in variables)
                            {
                                token.Variables[kvp.Value].VariableValue = column.FieldName;
                            }

                            EvaluationEngine.Evaluate.Evaluator eval = new EvaluationEngine.Evaluate.Evaluator(token);

                            string ErrorMsg = "";
                            string result = "";
                            if (eval.Evaluate(out result, out ErrorMsg) == false)
                            {
                                throw new InvalidOperationException("Error evaluating the tokens: " + ErrorMsg + System.Environment.NewLine
                                    + expression);
                            }

                            column.Fixed = (result == "true");
                        }
                        grid.EndInit();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }

        private MyGrid FindGridByName(string gridName)
        {
            foreach (MyGrid grid in m_grids)
            {
                if (grid.Name == gridName)
                {
                    return grid;
                }
            }
            return null;
        }
    }
}