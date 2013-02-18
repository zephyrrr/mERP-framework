using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;


namespace Feng.Utils
{
    /// <summary>
    /// 类型转换器
    /// </summary>
    public static class ConvertHelper
    {
        ///// <summary>
        ///// 从Argb到Color
        ///// </summary>
        ///// <param name="argb"></param>
        ///// <returns></returns>
        //public static Color ToColor(int argb)
        //{
        //    return Color.FromArgb(argb);
        //}

        ///// <summary>
        ///// 从Color到Argb Int
        ///// </summary>
        ///// <param name="color"></param>
        ///// <returns></returns>
        //public static int FromColor(Color color)
        //{
        //    return color.ToArgb();
        //}

        /// <summary>
        /// 转换为String。如果是System.DBNull.Value，也会返回null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            try
            {
                string s = value.ToString();
                if (string.IsNullOrEmpty(s))
                {
                    return null;
                }
                else
                {
                    return s;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// Any to bool(if exception, return false)
        /// "true", 1, -1 -> true
        /// "false", 0 -> false
        /// Exceptions -> null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? ToBoolean(object value)
        {
            try
            {
                if (value == null || value == System.DBNull.Value)
                {
                    return null;
                }
                return Convert.ToBoolean(value.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// Any to Decimal(if exception, return null0)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// ToDouble
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double? ToDouble(object value)
        {
            try
            {
                return Convert.ToDouble(value.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// Any to Long(if exception, return null)
        /// 12.34m -> 0(Exception)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? ToLong(object value)
        {
            try
            {
                return Convert.ToInt64(value.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// Any to Int(if exception, return null)
        /// 12.34m -> 0(Exception)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? ToInt(object value)
        {
            try
            {
                return Convert.ToInt32(value.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// Any to ToDateTime(if exception, return null)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object value)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return null;
                return Convert.ToDateTime(value.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public static void Copy<T>(T src, T dest)
        {
            PropertyInfo[] pi = typeof (T).GetProperties();
            foreach (PropertyInfo p in pi)
            {
                object o = typeof (T).InvokeMember(p.Name, BindingFlags.GetProperty, null, src, null, null, null, null);
                typeof (T).InvokeMember(p.Name, BindingFlags.SetProperty, null, dest, new object[] {o}, null, null, null);
            }
        }

        /// <summary>
        /// 改变List中类型(T应该为S类型)
        /// </summary>
        /// <typeparam name="TSrcType"></typeparam>
        /// <typeparam name="TDestType"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static IList<TDestType> ChangeListType<TSrcType, TDestType>(IList<TSrcType> src)
            where TDestType : class
            where TSrcType : class
        {
            IList<TDestType> dest = new List<TDestType>();
            if (src != null)
            {
                foreach (TSrcType t in src)
                {
                    TDestType j = t as TDestType;
                    if (j != null)
                    {
                        dest.Add(j);
                    }
                    else
                    {
                        throw new ArgumentException("T should be S type!");
                    }
                }
            }
            return dest;
        }

        /// <summary>
        /// Int -> Enum
        /// </summary>
        /// <param name="src"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static object TryIntToEnum(object src, Type destType)
        {
            if (src.GetType().IsEnum)
            {
                return src;
            }
            if (!destType.IsEnum)
            {
                return src;
            }

            if (src.GetType() == Enum.GetUnderlyingType(destType) && destType.IsEnum)
            {
                return Enum.Parse(destType, src.ToString(), true);
            }

            throw new NotSupportedException("Invalid enum value");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IList<T> ChangeListType<T>(System.Collections.IList value)
        {
            if (value == null)
            {
                return null;
            }
            IList<T> ret = new List<T>();
            for (int i = 0; i < value.Count; ++i)
            {
                ret.Add((T)ChangeType(value[i], typeof(T)));
            }
            return ret;
        }

        /// <summary>
        /// ChangeType
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static void ChangeType(System.Collections.IList value, Type conversionType)
        {
            if (value == null)
            {
                return;
            }
            for (int i = 0; i < value.Count; ++i)
            {
                value[i] = ChangeType(value[i], conversionType);
            }
        }

        /// <summary>
        /// ChangeType（如果为null或者空，返回null）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            try
            {
                if (value == null)
                    return null;
                if (string.IsNullOrEmpty(value.ToString()))
                    return null;

                if (conversionType.IsAssignableFrom(value.GetType()))
                {
                    return value;
                }
                if (conversionType.IsEnum)
                {
                    return Enum.Parse(conversionType, value.ToString(), true);
                }
                if (conversionType == typeof(Guid))
                {
                    return new Guid(value.ToString());
                }
                if (conversionType == typeof(bool))
                {
                    if (value.ToString() == "0")
                        return false;
                    else if (value.ToString() == "1")
                        return true;
                }
                return Convert.ChangeType(value, conversionType, null);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// MergeList
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static IList<T> MergeList<T>(IList<T> t1, IList<T> t2)
        {
            IList<T> ret = new List<T>();
            foreach (T i in t1)
            {
                ret.Add(i);
            }
            foreach (T i in t2)
            {
                ret.Add(i);
            }
            return ret;
        }

        ///// <summary>
        ///// 得到Set里的第一个元素
        ///// </summary>
        ///// <param name="set"></param>
        ///// <returns></returns>
        //public static T GetFirstElementInSet<T>(Iesi.Collections.Generic.ISet<T> set)
        //{
        //    if (set.Count == 0)
        //    {
        //        return default(T);
        //    }
        //    IEnumerator<T> enumerator = set.GetEnumerator();
        //    enumerator.MoveNext();
        //    return enumerator.Current;
        //}

        /// <summary>
        /// ListToArray
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] ListToArray<T>(IList<T> list)
        {
            T[] arr = new T[list.Count];
            list.CopyTo(arr, 0);
            return arr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static S[] ListToArray<T, S>(IList<T> list, Func<T, S> func)
        {
            S[] arr = new S[list.Count];
            for (int i = 0; i < list.Count; ++i)
                arr[i] = func(list[i]);
            return arr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static string StringArrayToString(string[] ss)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in ss)
            {
                sb.Append(s);
                sb.Append(',');
            }
            return sb.ToString();
        }
    }
}