using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class MySummaryCell : SummaryCell
    {
        /// <summary>
        /// 
        /// </summary>
        public MySummaryCell()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public MySummaryCell(MySummaryCell template)
            : base(template)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        protected internal MySummaryCell(Column parentColumn)
            : base(parentColumn)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Cell CreateInstance()
        {
            return new MySummaryCell(this);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        protected override string GetVariableText(string variableName)
        {
            string s = base.GetVariableText(variableName);
            if (!string.IsNullOrEmpty(s) && s != "Unspecified field")
            {
                return s;
            }

            string functionName, fieldName, format, expression;
            MySummaryRow parentRow = base.ParentRow as MySummaryRow;
            int level = (parentRow == null) ? -1 : parentRow.RunningStatGroupLevel;

            MySummaryRow.ExtractParameters(variableName, out functionName, out fieldName, out format, ref level, out expression);
            return MySummaryRow.GetVariableFunctionResult(parentRow.ParentGroup.GetSortedDataRows(true), functionName, fieldName, format, level, expression);
        }
    }
}