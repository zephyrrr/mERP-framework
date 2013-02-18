using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    public class BoundGridGroupManagerRow : MyGroupManagerRow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BoundGridGroupManagerRow()
            : base()
        {
        }

        /// <summary>
        /// Constructor(u should override it)
        /// </summary>
        /// <param name="template"></param>
        public BoundGridGroupManagerRow(BoundGridGroupManagerRow template)
            : base(template)
        {
        }

        /// <summary>
        /// CreateInstance(u should override it)
        /// </summary>
        /// <returns></returns>
        protected override Xceed.Grid.Row CreateInstance()
        {
            return new BoundGridGroupManagerRow(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string DefaultTitleFormat
        {
            get
            {
                return GetTitleFormat();
            }
        }

        private string GetTitleFormat()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("%ColumnTitle% : %GroupTitle% - 共%DataRowCount%项. ");

            foreach (Xceed.Grid.Column col in this.ParentGrid.Columns)
            {
                GridColumnInfo info = col.Tag as GridColumnInfo;
                if (info != null)
                {
                    if (!string.IsNullOrEmpty(info.StatFunction))
                    {
                        sb.Append(info.Caption + ":");
                        sb.Append("%" + info.StatFunction + ":" + info.GridColumnName + "%");

                        sb.Append(",");
                    }
                    else if (!string.IsNullOrEmpty(info.StatTitle))
                    {
                        sb.Append(info.Caption + ":");
                        sb.Append(info.StatTitle);

                        sb.Append(",");
                    }
                }
            }

            string s = sb.ToString();
            if (sb[sb.Length - 2] == ',')
            {
                sb[sb.Length - 2] = '.';
            }
            return sb.ToString();
        }
    }
}
