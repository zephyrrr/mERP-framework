using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// 比较条件
    /// </summary>
    [Serializable]
    public enum ComparisonType
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Eq,
        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NotEq,
        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        Gt,
        /// <summary>
        /// 大于或等于
        /// </summary>
        [Description("大于等于")]
        Ge,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        Lt,
        /// <summary>
        /// 小于或等于
        /// </summary>
        [Description("小于等于")]
        Le,
        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        Like,
        /// <summary>
        /// 不包含
        /// </summary>
        [Description("不包含")]
        NotLike,
    }

    /// <summary>
    /// 
    /// </summary>
    public class ComparisonFilter : IFilter
    {
        private ComparisonType m_comparionType;
        private IComparable m_value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public ComparisonFilter(ComparisonType type, object value)
        {
            IComparable r1 = value as IComparable;
            if (value != null && r1 == null)
            {
                throw new ArgumentException("value is not IComparable", "value");
            }

            m_comparionType = type;
            m_value = r1;
        }

        /// <summary>
        /// 
        /// </summary>
        public ComparisonType ComparisonType
        {
            get { return m_comparionType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get { return m_value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Evaluate(object value)
        {
            if (m_value == null)
            {
                switch (m_comparionType)
                {
                    case ComparisonType.Eq:
                        return (value == null || value == DBNull.Value);
                    case ComparisonType.NotEq:
                        return !(value == null || value == DBNull.Value);
                    case ComparisonType.Lt:
                        return false;
                    case ComparisonType.Le:
                        return false;
                    case ComparisonType.Gt:
                        return false;
                    case ComparisonType.Ge:
                        return false;
                    case ComparisonType.Like:
                        return false;
                    case ComparisonType.NotLike:
                        return false;
                    default:
                        throw new NotSupportedException("Invalid m_comparionType");
                }
            }

            int r = m_value.CompareTo(value);
            switch (m_comparionType)
            {
                case ComparisonType.Eq:
                    return (r == 0);
                case ComparisonType.NotEq:
                    return (r != 0);
                case ComparisonType.Lt:
                    return (r > 0);
                case ComparisonType.Le:
                    return (r >= 0);
                case ComparisonType.Gt:
                    return (r < 0);
                case ComparisonType.Ge:
                    return (r <= 0);
                case ComparisonType.Like:
                    return (value == null ? false : value.ToString().Contains(m_value.ToString()));
                case ComparisonType.NotLike:
                    return !(value == null ? false : value.ToString().Contains(m_value.ToString()));
                default:
                    throw new NotSupportedException("Invalid m_comparionType");
            }
        }
    }
}