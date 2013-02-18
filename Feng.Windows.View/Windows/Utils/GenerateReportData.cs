using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Feng.Windows.Utils
{
    public class GenerateReportData
    {
        private static void Generate(DataTable dt, object entity, string gridName)
        {
            Dictionary<string, object> dict;
            using (GridDataConvert dp = new GridDataConvert())
            {
                dict = dp.Process(entity, gridName);
            }
            if (dict == null || dict.Count == 0)
                return;

            DataRow newRow = dt.NewRow();

            foreach (DataColumn col in dt.Columns)
            {
                if (dict.ContainsKey(col.ColumnName))
                {
                    newRow[col.ColumnName] = (dict[col.ColumnName] != null ? dict[col.ColumnName] : System.DBNull.Value);
                }
                else
                {
                    object o = null;
                    try
                    {
                        o = EntityScript.GetPropertyValue(entity, col.ColumnName);
                    }
                    catch(Exception)    // 不一定有这个属性，例如大写金额
                    {
                    }
                    newRow[col.ColumnName] = o != null ? o : System.DBNull.Value;
                }
            }

            dt.Rows.Add(newRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        /// <param name="gridName"></param>
        public static void Generate(DataTable dt, IEnumerable list, string gridName)
        {
            foreach (object row in list)
            {
                Generate(dt, row, gridName);
            }
        }
    }
}
