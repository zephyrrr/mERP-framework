using System;
using System.Collections.Generic;
using System.Text;
using Feng.Utils;

namespace Feng.Data
{
    /// <summary>
    /// 主键产生器
    /// </summary>
    public static class PrimaryMaxIdGenerator
    {
        /// <summary>
        /// 得到DateTime对应的YYMM字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetIdYearMonth(DateTime dt)
        {
            return dt.ToString("yyMM");
        }

        /// <summary>
        /// 得到今天对应的YYMM字符串
        /// </summary>
        /// <returns></returns>
        public static string GetIdYearMonth()
        {
            return GetIdYearMonth(System.DateTime.Today);
        }


        /// <summary>
        /// 得到当前的表中可用的ID（Int形式）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <returns></returns>
        public static long GetMaxId(string tableName, string primaryKeyName)
        {
            return ConvertHelper.ToLong(DbHelper.Instance.ExecuteScalar("SELECT " + GetIsNullString("MAX(" + primaryKeyName + ")", "0") + " + 1 FROM " + tableName)).Value;
        }

        /// <summary>
        /// 得到当前的表中可用的ID（字符串形式）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="primaryKeyLen"></param>
        /// <returns></returns>
        public static string GetMaxId(string tableName, string primaryKeyName, int primaryKeyLen)
        {
            return GetMaxId(tableName, primaryKeyName, primaryKeyLen, null);
        }

        /// <summary>
        /// 得到当前的表中可用的ID（字符串形式）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="preName"></param>
        /// <param name="primaryKeyLen"></param>
        /// <returns></returns>
        public static string GetMaxId(string tableName, string primaryKeyName, int primaryKeyLen, string preName)
        {
            return GetMaxId(tableName, primaryKeyName, primaryKeyLen, preName, 0);
        }

        private static string GetIsNullString(string exp, string value)
        {
            return "CASE WHEN " + exp + " IS NULL THEN " + value + " ELSE " + exp + " END";
        }

        /// <summary>
        /// GetMaxIdFromPrevId
        /// </summary>
        /// <param name="prePrimaryKeyId"></param>
        /// <param name="preName"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static string GetMaxIdFromPrevId(string prePrimaryKeyId, string preName, int delta)
        {
            string s1 = prePrimaryKeyId.Substring(preName.Length, prePrimaryKeyId.Length - preName.Length);
            int pre = Feng.Utils.ConvertHelper.ToInt(s1).Value;
            return preName + (pre + delta).ToString("D" + s1.Length);
        }

        /// <summary>
        /// 获得当前表中的主键的最大值（不包括前缀）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static long GetMaxInt(string tableName, string primaryKeyName, string pattern)
        {
            return GetMaxInt(tableName, primaryKeyName, pattern, null);
        }

        /// <summary>
        /// 获得当前表中的主键的最大值（不包括前缀）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="pattern">主键值模式，递增的数字用?表示,%代表任意数字，例如ABCD?-%</param>
        /// <param name="whereLike"></param>
        /// <returns></returns>
        public static long GetMaxInt(string tableName, string primaryKeyName, string pattern, string whereLike)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (string.IsNullOrEmpty(primaryKeyName))
            {
                throw new ArgumentNullException("primaryKeyName");
            }

            object strID;

            if (!string.IsNullOrEmpty(pattern))
            {
                int left = -1;
                int right = -1;
                for (int i = 0; i < pattern.Length; ++i)
                {
                    if (left == -1)
                    {
                        if (pattern[i] == '?')
                        {
                            left = i;
                        }
                    }
                    else
                    {
                        if (pattern[i] != '?')
                        {
                            right = i;
                            break;
                        }
                    }
                }
                if (left == -1)
                {
                    throw new ArgumentException("Pattern format is invalid! pattern should contain at least one ?", "pattern");
                }

                string newPattern;
                if (string.IsNullOrEmpty(whereLike))
                {
                    newPattern = pattern.Substring(0, left) + "%";
                }
                else
                {
                    newPattern = whereLike;
                }

                string sql;
                if (right == -1)
                {
                    string nullString = string.Format("MAX(CONVERT(INT, SUBSTRING({0}, {1}, LEN({0}) - {1} + 1)))", primaryKeyName, left + 1);
                    nullString = GetIsNullString(nullString, "0");
                    sql = string.Format("SELECT {0} FROM {1} WHERE {2} LIKE '{3}'", nullString, tableName, primaryKeyName, newPattern);
                }
                else
                {
                    string endSign = pattern.Substring(right).Replace("%", "").Replace("?", "");

                    string nullString = string.Format("MAX(CONVERT(INT, SUBSTRING({0}, {1}, CASE CHARINDEX('{2}', {0}) WHEN 0 THEN LEN({0}) + 1 ELSE CHARINDEX('{2}', {0}) END - {1}) ))", primaryKeyName, left + 1, endSign);
                    nullString = GetIsNullString(nullString, "0");
                    sql = string.Format("SELECT {0} FROM {1} WHERE {2} LIKE '{3}'", nullString, tableName, primaryKeyName, newPattern);
                }

                strID = DbHelper.Instance.ExecuteScalar(sql);
            }
            else
            {
                strID = DbHelper.Instance.ExecuteScalar("SELECT " +
                    GetIsNullString("MAX(CONVERT(INT, " + primaryKeyName + "))", "0") +
                                    " FROM " + tableName);
            }

            return ConvertHelper.ToLong(strID).Value;
        }

        /// <summary>
        /// 获得当前表中的主键的最大值（不包括前缀）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="primaryKeyLen"></param>
        /// <param name="preName"></param>
        /// <returns></returns>
        public static long GetMaxInt(string tableName, string primaryKeyName, int primaryKeyLen, string preName)
        {
            return GetMaxInt(tableName, primaryKeyName, primaryKeyLen, preName, null);
        }

        /// <summary>
        /// 获得当前表中的主键的最大值（不包括前缀）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="primaryKeyLen"></param>
        /// <param name="preName"></param>
        /// <param name="specialWhere">生成序号的时候的自定义Where</param>
        /// <returns></returns>
        public static long GetMaxInt(string tableName, string primaryKeyName, int primaryKeyLen, string preName, string specialWhere)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (string.IsNullOrEmpty(primaryKeyName))
            {
                throw new ArgumentNullException("primaryKeyName");
            }

            object strID;

            if (!string.IsNullOrEmpty(preName))
            {
                if (string.IsNullOrEmpty(specialWhere))
                {
                    strID = DbHelper.Instance.ExecuteScalar("SELECT " +
                        GetIsNullString("MAX(CONVERT(INT, SUBSTRING(" + primaryKeyName + ", " + (preName.Length + 1).ToString() + ", " + (primaryKeyLen - preName.Length).ToString() + ")))", "0") +
                                         " FROM " + tableName + " WHERE " + primaryKeyName + " LIKE '" + preName + "%'");
                }
                else
                {
                     strID = DbHelper.Instance.ExecuteScalar("SELECT " +
                        GetIsNullString("MAX(CONVERT(INT, SUBSTRING(" + primaryKeyName + ", " + (preName.Length + 1).ToString() + ", " + (primaryKeyLen - preName.Length).ToString() + ")))", "0") +
                                         " FROM " + tableName + " WHERE " + specialWhere);
                }
            }
            else
            {
                strID = DbHelper.Instance.ExecuteScalar("SELECT " + 
                    GetIsNullString("MAX(CONVERT(INT, " + primaryKeyName + "))", "0") + 
                                    " FROM " + tableName);
            }

            return ConvertHelper.ToLong(strID).Value;
        }

        /// <summary>
        /// 得到当前的表中可用的ID，主要用于产生带前缀的顺序号（字符串形式）
        /// preClass==Others: 有分类号，分类号相同取最大
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="preName">数据库表分类字段。null: 无前分类号，直接取最大</param>
        /// <param name="primaryKeyLen">主键长度</param>
        /// <param name="delta">在最大值上加的增量（0为增加1）</param>
        /// <returns>可用的ID</returns>
        public static string GetMaxId(string tableName, string primaryKeyName, int primaryKeyLen, string preName, int delta)
        {
            long nowMaxId = GetMaxInt(tableName, primaryKeyName, primaryKeyLen, preName);
            return GetMaxId(nowMaxId, primaryKeyLen, preName, delta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nowMaxInt"></param>
        /// <param name="primaryKeyLen"></param>
        /// <param name="preName"></param>
        /// <returns></returns>
        public static string GetMaxId(long nowMaxInt, int primaryKeyLen, string preName, int delta)
        {
            long nextId = nowMaxInt + delta + 1;
            string strId = nextId.ToString();
            if (strId.Length != primaryKeyLen)
            {
                strId = strId.PadLeft(primaryKeyLen - (preName == null ? 0 : preName.Length), '0');
            }

            return preName + strId;
        }
    }
}