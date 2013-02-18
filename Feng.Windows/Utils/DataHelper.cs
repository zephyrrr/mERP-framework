using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Feng.Windows.Utils
{
    public static class DataHelper
    {
        /// <summary>
        /// 从List转换到DataTable（ColumnName用Type的Property）
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static System.Data.DataTable ListToDataTable(System.Collections.IEnumerable list, Type type)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            System.Data.DataTable dt = new System.Data.DataTable();

            PropertyInfo[] pi = type.GetProperties();
            foreach (PropertyInfo pInfo in pi)
            {
                Type propertyType = pInfo.PropertyType;
                if (pInfo.PropertyType.IsGenericType && pInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = pInfo.PropertyType.GetGenericArguments()[0];
                }
                dt.Columns.Add(new System.Data.DataColumn(pInfo.Name, propertyType));
            }

            foreach (object obj in list)
            {
                System.Data.DataRow row = dt.NewRow();
                foreach (PropertyInfo p in pi)
                {
                    object o = null;
                    try
                    {
                        o = p.GetValue(obj, null);
                    }
                    catch (Exception)
                    {
                    }

                    row[p.Name] = o == null ? System.DBNull.Value : o;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>
        /// 从List转换到DataTable（ColumnName用Type的Property）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static System.Data.DataTable ListToDataTable<T>(System.Collections.Generic.IEnumerable<T> list)
        {
            return ListToDataTable(list, typeof(T));
        }
    }
}
