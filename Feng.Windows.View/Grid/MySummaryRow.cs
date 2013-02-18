using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class MySummaryRow : SummaryRow
    {
        /// <summary>
        /// 
        /// </summary>
        public MySummaryRow()
            : base()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public MySummaryRow(MySummaryRow template)
            : base(template)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        /// <returns></returns>
        protected override Cell CreateCell(Column parentColumn)
        {
            return new MySummaryCell(parentColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Row CreateInstance()
        {
            return new MySummaryRow(this);
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

            string functionName, fieldName, format, where;
            int level = this.RunningStatGroupLevel;
            ExtractParameters(variableName, out functionName, out fieldName, out format, ref level, out where);

            return GetVariableFunctionResult(this.ParentGroup.GetSortedDataRows(true), functionName, fieldName, format, level, where);
        }

        /// <summary>
        /// 根据StatTile获得汇总结果
        /// </summary>
        /// <param name="rowList"></param>
        /// <param name="titleFormat"></param>
        /// <returns></returns>
        public static string GetSummaryResult(Xceed.Grid.Collections.ReadOnlyDataRowList rowList, string titleFormat)
        {
            string functionName, fieldName, format, expression;
            int level = -1;

            ExtractParameters(titleFormat, out functionName, out fieldName, out format, ref level, out expression);
            return GetVariableFunctionResult(rowList, functionName, fieldName, format, level, expression);
        }

        private static Regex s_regex;
        /// <summary>
        /// 分割参数
        /// 格式 % ["]stat_fieldname["] [ format=["]format_specifier["] ] [ level=running_stat_group_level ] [ where=["]where_specifier["] ]%
        /// 例如 "here is %COUNT:简称 format=abc level=0 expression="$iif(%收付标志%=收,金额,-金额)$"%";
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="functionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="format"></param>
        /// <param name="level"></param>
        /// <param name="expression"></param>
        public static void ExtractParameters(string variableName, out string functionName, out string fieldName,
                                               out string format, ref int level, out string expression)
        {
            // % ["]stat_fieldname["] [ format=["]format_specifier["] ] [ level=running_stat_group_level ] [ expression=["]expression_specifier["] ]%
            functionName = string.Empty;
            fieldName = string.Empty;
            format = string.Empty;
            expression = string.Empty;
            if (s_regex == null)
            {
                s_regex =
                    new Regex(
                        "^\\s*" + 
                        "(?<FctName>\\w+)\\s*:*\\s*" + 
                        "((?<FieldName>(\\w|\\.)+)|(\\\"(?<FieldName>.+?)\\\"))*" +
                        "(\\s+level\\s*=\\s*(?<Level>(\\d+|-\\d+)))*" +
                        "(\\s+format\\s*=\\s*((\\\"(?<Format>.+?)\\\")|(?<Format>\\w+)))*" + 
                        "(\\s+level\\s*=\\s*(?<Level>(\\d+|-\\d+)))*" +
                        "(\\s+expression\\s*=\\s*((\\\"(?<Expression>.+?)\\\")|(?<Expression>\\w+)))*" + 
                        "\\s*$",
                        RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled |
                        RegexOptions.IgnoreCase);
            }
            Match match = s_regex.Match(variableName);
            if (match.Success)
            {
                functionName = match.Result("${FctName}").ToUpper(System.Globalization.CultureInfo.InvariantCulture);
                fieldName = match.Result("${FieldName}");
                format = match.Result("${Format}");
                expression = match.Result("${Expression}");
                string s = match.Result("${Level}");
                if (s.Length > 0)
                {
                    level = int.Parse(s);
                }

                if (format == "null")
                {
                    format = string.Empty;
                }
                if (expression == "null")
                {
                    expression = string.Empty;
                }
            }
        }

        /// <summary>
        /// GetVariableFunctionResult
        /// </summary>
        /// <param name="rowList"></param>
        /// <param name="functionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="format"></param>
        /// <param name="level"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetVariableFunctionResult(Xceed.Grid.Collections.ReadOnlyDataRowList rowList, string functionName, string fieldName,
                                               string format, int level, string expression)
        {
            // VariableText中不能带%（默认是作为VariableText的分隔符）
            if (!string.IsNullOrEmpty(expression))
            {
                expression = expression.Replace('#', '%');
            }

            switch (functionName.ToUpper())
            {
                case "COUNT":
                    int cnt = 0;
                    foreach (Xceed.Grid.DataRow row in rowList)
                    {
                        if (string.IsNullOrEmpty(expression))
                        {
                            cnt++;
                        }
                        else
                        {
                            object s = EntityScript.CalculateExpression(expression, row.Tag);
                            int? r = Feng.Utils.ConvertHelper.ToInt(s);
                            if (r.HasValue)
                            {
                                cnt += r.Value;
                            }
                        }
                    }
                    return cnt.ToString();
                case "SUM":
                    decimal sum = 0;
                    foreach (Xceed.Grid.DataRow row in rowList)
                    {
                        if (string.IsNullOrEmpty(expression))
                        {
                            sum += Convert.ToDecimal(row.Cells[fieldName].Value);
                        }
                        else
                        {
                            object s = EntityScript.CalculateExpression(expression, row.Tag);
                            decimal? r = Feng.Utils.ConvertHelper.ToDecimal(s);
                            if (r.HasValue)
                            {
                                sum += r.Value;
                            }
                        }
                    }
                    return string.IsNullOrEmpty(format) ? sum.ToString("N2") : sum.ToString(format);
                case "ALLTRUE":
                    {
                        bool all = true;
                        foreach (Xceed.Grid.DataRow row in rowList)
                        {
                            if (string.IsNullOrEmpty(expression))
                            {
                            }
                            else
                            {
                                object s = EntityScript.CalculateExpression(expression, row.Tag);
                                bool? r = Feng.Utils.ConvertHelper.ToBoolean(s);
                                if (!r.HasValue || !r.Value)
                                {
                                    all = false;
                                    break;
                                }
                            }
                        }
                        return all ? "全" : "否";
                    }

            }
            return string.Empty;
        }
    }
}