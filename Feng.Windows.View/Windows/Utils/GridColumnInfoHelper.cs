using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class GridColumnInfoHelper
    {
        public static Type CreateType(GridColumnInfo info)
        {
            return Feng.Utils.ReflectionHelper.GetTypeFromName(info.TypeName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="asView"></param>
        /// <returns></returns>
        public static string GetNameValueMappingName(GridColumnInfo info, bool asView)
        {
            string initParam = asView ? info.CellViewerManagerParam : info.CellEditorManagerParam;
            if (string.IsNullOrEmpty(initParam))
                return null;

            //string filter = asView ? string.Empty : info.CellEditorManagerParamFilter;
            string nvName;
            if (initParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal))
            {
                Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                //ControlDataLoad.InitDataControl(control, type, false, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                nvName = NameValueMappingCollection.Instance.Add(type, false);
            }
            else if (initParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal))
            {
                Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                //ControlDataLoad.InitDataControl(control, type, true, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                nvName = NameValueMappingCollection.Instance.Add(type, true);
            }
            else
            {
                nvName = initParam;
                //ControlDataLoad.InitDataControl(control, info.CellViewerManagerParam,
                //    info.CellEditorManagerParam, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
            }
            return nvName;
        }

        /// <summary>
        /// 
        /// </summary>
        public const string StartWithBigEnum = "Enum";    // 值用Enum
        /// <summary>
        /// 
        /// </summary>
        public const string StartWithLittleEnum = "enum";    // 值用Int,string等序号

        private static string GetEnumInitParam(string param)
        {
            System.Diagnostics.Debug.Assert(param.StartsWith(StartWithBigEnum) || param.StartsWith(StartWithLittleEnum), "Enum Param should start with enum");
            if (param.Length <= StartWithLittleEnum.Length + 1)
                return null;
            return param.Substring(StartWithLittleEnum.Length + 1).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="initParam"></param>
        /// <returns></returns>
        public static Type CreateTypeFromEnumInitParam(Type columnType, string initParam)
        {
            string s = GetEnumInitParam(initParam);
            if (string.IsNullOrEmpty(s))
            {
                return columnType;
            }
            else
            {
                return Feng.Utils.ReflectionHelper.GetTypeFromName(s);
            }
        }
    }
}
