using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class GridDataConvert : IDisposable
    {
        public GridDataConvert(bool useColumnVisible = true)
        {
            m_useColumnVisible = useColumnVisible;
        }
        private bool m_useColumnVisible = true;
        /// <summary>
        /// Dispose RadioButton and remove it from parent's controls
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                //foreach (Xceed.Grid.Viewers.CellViewerManager viewer in m_viewers.Values)
                //{
                //    viewer.Dispose();
                //}
                m_viewers.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public Dictionary<string, object> Process(object entity, string gridName)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();

            // grid
            if (!Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(gridName).Visible))
            {
                return r;
            }

            // row
            if (!Permission.AuthorizeByRule(ADInfoBll.Instance.GetGridRowInfo(gridName).Visible, entity))
            {
                return r;
            }

            // column
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(gridName))
            {
                object v = null;
                if ((m_useColumnVisible && !Authority.AuthorizeByRule(info.ColumnVisible))
                    || (!m_useColumnVisible && !Authority.AuthorizeByRule(info.DataControlVisible)))
                {
                    continue;
                }
                else
                {
                    if (info.GridColumnType != GridColumnType.Normal
                        && info.GridColumnType != GridColumnType.ExpressionColumn)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(info.PropertyName))
                    {
                        v = null;
                    }
                    else if (info.GridColumnType == GridColumnType.Normal)
                    {
                        v = EntityScript.GetPropertyValue(entity, info.Navigator, info.PropertyName);
                    }
                    else if (info.GridColumnType == GridColumnType.ExpressionColumn)
                    {
                        if (info.PropertyName.Contains("%"))
                        {
                            v = EntityScript.CalculateExpression(info.PropertyName, entity);
                        }
                        else
                        {
                            v = ProcessInfoHelper.TryExecutePython(info.PropertyName,
                                new Dictionary<string, object>() { { "entity", entity } });
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (v == System.DBNull.Value)
                {
                    v = null;
                }
                if (v == null)
                {
                    r[info.GridColumnName] = null;
                    continue;
                }

                string vs = null;
                if (GridColumnInfoHelper.CreateType(info).IsEnum)
                {
                    vs = v.ToString();
                }

                switch (info.CellViewerManager)
                {
                    case "Combo":
                    case "MultiCombo":
                        vs = GetViewerText(v, info);
                        break;
                    case "Object":
                        vs = EntityHelper.ReplaceEntity(info.CellViewerManagerParam, v, null);
                        break;
                    case "Numeric":
                    case "Integer":
                    case "Long":
                    case "Currency":
                        vs = Convert.ToDouble(v).ToString(GetFormatString(info));
                        break;
                    case "Date":
                    case "DateTime":
                        vs = Convert.ToDateTime(v).ToString(GetFormatString(info));
                        break;
                    default:
                        vs = v.ToString();
                        break;
                }

                r[info.GridColumnName] = vs;

                //// cell
                //foreach (GridCellInfo info in GridSettingInfoCollection.Instance[gridName].GridCellInfos)
                //{
                //}
            }

            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public IList<Dictionary<string, object>> Process(IEnumerable list, string gridName)
        {
            IList<Dictionary<string, object>> ret = new List<Dictionary<string, object>>();

            // grid
            if (!Authority.AuthorizeByRule(ADInfoBll.Instance.GetGridInfo(gridName).Visible))
            {
                return ret;
            }

            foreach (object entity in list)
            {
                Dictionary<string, object> r = Process(entity, gridName);

                ret.Add(r);
            }

            return ret;
        }

        //private Dictionary<GridColumnInfo, Xceed.Grid.Viewers.CellViewerManager> m_viewers = new Dictionary<GridColumnInfo, Xceed.Grid.Viewers.CellViewerManager>();
        private Dictionary<GridColumnInfo, string> m_viewers = new Dictionary<GridColumnInfo, string>();
        private string GetViewerText(object value, GridColumnInfo info)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return null;

            if (!m_viewers.ContainsKey(info))
            {
                string nvName = GridColumnInfoHelper.GetNameValueMappingName(info, true);
                m_viewers[info] = nvName;
            }

            return Feng.Utils.NameValueControlHelper.GetMultiString(m_viewers[info], value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetFormatString(GridColumnInfo info)
        {
            if (string.IsNullOrEmpty(info.CellViewerManager))
                return null;
            if (string.IsNullOrEmpty(info.CellViewerManagerParam))
                return null;

            if (info.CellViewerManager == "DateTime")
                return info.CellViewerManagerParam;
            if (info.CellViewerManager == "Numeric")
                return info.CellViewerManagerParam.Replace("+", "N");
            return null;
        }
    }
}
