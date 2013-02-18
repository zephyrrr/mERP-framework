using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Feng.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Enum to List
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IList<EnumNameValue> EnumToList(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType should be enum, not " + enumType);
            }
            IList<EnumNameValue> list = new List<EnumNameValue>();
            foreach (object i in GetValues(enumType))
            {
                list.Add(new EnumNameValue(i, Convert.ToInt32(i), GetEnumDescription((Enum)i), enumType));
            }

            return list;
        }

        /// <summary>
        /// 得到Enum对应的Description字符串
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum en)
        {
            if (en == null)
            {
                throw new ArgumentNullException("en");
            }

            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute),
                                                                false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetValues<T>()
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<T> values = new List<T>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add((T)value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

    }
}
