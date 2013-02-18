using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Xceed.Grid;
using Feng;

namespace Feng.Grid
{
    public class MyGroupManagerRow : Xceed.Grid.GroupManagerRow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyGroupManagerRow()
            : base()
        {
        }

        /// <summary>
        /// Constructor(u should override it)
        /// </summary>
        /// <param name="template"></param>
        public MyGroupManagerRow(MyGroupManagerRow template)
            : base(template)
        {
        }

        /// <summary>
        /// CreateInstance(u should override it)
        /// </summary>
        /// <returns></returns>
        protected override Xceed.Grid.Row CreateInstance()
        {
            return new MyGroupManagerRow(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string DefaultTitleFormat
        {
            get
            {
                // groupTitle 默认是groupKey的显示值（key=100000，title=管理员）
                return "%ColumnTitle% : %GroupTitle% - 共%DataRowCount%项";
            }
        }
    }


    /// <summary>
    /// 特殊GroupManageRow
    /// 必须Override Constructor，CreateInstance
    /// </summary>
    public class MyGroupManagerRowCustom : Xceed.Grid.GroupManagerRow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyGroupManagerRowCustom()
            : base()
        {
        }

        /// <summary>
        /// Constructor(u should override it)
        /// </summary>
        /// <param name="template"></param>
        public MyGroupManagerRowCustom(MyGroupManagerRowCustom template)
            : base(template)
        {
        }

        /// <summary>
        /// CreateInstance(u should override it)
        /// </summary>
        /// <returns></returns>
        protected override Xceed.Grid.Row CreateInstance()
        {
            return new MyGroupManagerRowCustom(this);
        }

        /// <summary>
        /// GetVariableText
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        protected override string GetVariableText(string variableName)
        {
            return base.GetVariableText(variableName);
        }

        /// <summary>
        /// DefaultBackColor
        /// </summary>
        protected override Color DefaultBackColor
        {
            get
            {
                if (this.ParentGroup != null)
                {
                    return this.ParentGroup.BackColor;
                }
                else
                {
                    return base.DefaultBackColor;
                }
            }
        }

        /// <summary>
        /// Gets the borders of the <see cref="ColumnManagerRow"/>.
        /// </summary><value>
        /// A <see cref="Borders"/> structure representing the borders of the ColumnManagerRow.
        /// Unless overridden, all borders are equal to 0 (no borders).
        /// </value>
        public override Borders Borders
        {
            get { return new Borders(0, 0, 0, 2); }
        }

        /// <summary>
        /// PaintForeground
        /// </summary>
        /// <param name="e"></param>
        protected override void PaintForeground(GridPaintEventArgs e)
        {
            base.PaintForeground(e);

            int intGroupUIState = 0;

            if (!this.ParentGroup.Collapsed)
            {
                intGroupUIState |= Xceed.UI.GroupUIState.Expanded;
            }

            Xceed.UI.GroupUIState groupUIState = new Xceed.UI.GroupUIState(intGroupUIState);

            Color color = this.BackColor;
            foreach (Xceed.Grid.DataRow row in this.ParentGroup.GetSortedDataRows(true))
            {
                if (!string.IsNullOrEmpty(row.ErrorDescription))
                {
                    color = Color.Red;
                    break;
                }
            }
            using (Brush brush = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, 15, 36));
            }

            this.Theme.PaintGroup(e.Graphics, new Rectangle(2, e.ClientRectangle.Bottom - 15, 12, 12), groupUIState, 1);
        }

        /// <summary>
        /// PaintBorders
        /// </summary>
        /// <param name="e"></param>
        protected override void PaintBorders(GridPaintEventArgs e)
        {
            GroupBase currentGroup = this.ParentGroup;

            if (currentGroup == null)
            {
                base.PaintBorders(e);
                return;
            }

            GridControl gridControl = this.GridControl;

            if (gridControl == null)
            {
                return;
            }

            Rectangle paintRectangle = e.DisplayRectangle;
            Borders borders = this.Borders;

            int left = e.ClientRectangle.Left;
            //int right = paintRectangle.Right + borders.Right - 1;
            //int top = paintRectangle.Top - borders.Top;
            int bottom = paintRectangle.Bottom + borders.Bottom - borders.Bottom;

            // Horizontal (bottom) gridline.
            if (borders.Bottom > 0)
            {
                //GroupMargin groupMargin = currentGroup.SideMargin;
                int bottomBorderWidth = e.BottomBorderWidth - (left - paintRectangle.Left);

                if (bottomBorderWidth > 0)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(165, 164, 189)))
                    {
                        e.Graphics.FillRectangle(brush, left, bottom, bottomBorderWidth, borders.Bottom);
                    }
                }
            }
        }
    }
}