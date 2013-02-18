using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng.Search
{
    /// <summary>
    /// 比较条件
    /// </summary>
    public enum SimpleOperator
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
        /// 等于属性
        /// </summary>
        [Description("等于属性")]
        EqProperty,
        /// <summary>
        /// 不等于属性
        /// </summary>
        [Description("不等于属性")]
        NotEqProperty,
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
        /// 包含集合
        /// </summary>
        [Description("包含集合")]
        InG,
        /// <summary>
        /// 为空
        /// </summary>
        [Description("为空")]
        IsNull,
        /// <summary>
        /// 不为空
        /// </summary>
        [Description("不为空")]
        IsNotNull,
        /// <summary>
        /// 查询集合中的值在数据库字段中出现（不同于包含集合，包含集合为查询集合中的值=数据库字段）
        /// 数据库字段一般为以“，”分割的值列表
        /// </summary>
        [Description("集合包含集合")]
        GInG,
        /// <summary>
        /// 任意(只是让NHibernate产生Join连接）
        /// </summary>
        [Description("任意")]
        Any,
        /// <summary>
        /// Sql. 注意语法是 "SqlExpr" ISSQL, "SqlExpr"为PropertyName。在Hibernate中用以NHibernate.Criterion.Expression.Sql，Data中直接附加
        /// </summary>
        [Description("SQL语句")]
        Sql,

    }

    /// <summary>
    /// SimpleExpression
    /// </summary>
    public class SimpleExpression : AbstractExpression
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //private SimpleExpression()
        //{
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="op">查询条件类型</param>
        /// <param name="fullPropertyName">属性名</param>
        /// <param name="values">数据</param>
        public SimpleExpression(string fullPropertyName, object values, SimpleOperator op)
        {
            m_op = op;
            m_fullPropertyName = fullPropertyName;
            m_values = values;

            if (op == SimpleOperator.InG
                || op == SimpleOperator.GInG)
            {
                System.Diagnostics.Debug.Assert(values is IList, "data in collection op should be IList");
            }
        }

        private SimpleOperator m_op;
        /// <summary>
        /// 查询条件类型
        /// </summary>
        public SimpleOperator Operator 
        {
            get { return m_op; }
            set { m_op = value; }
        }

        private string m_fullPropertyName;
        /// <summary>
        /// 属性名
        /// </summary>
        public string FullPropertyName 
        {
            get { return m_fullPropertyName; }
            set { m_fullPropertyName = value; }
        }


        private object m_values;
        /// <summary>
        /// 数据（单个数据或者集合数据(ArrayList)）
        /// </summary>
        //[XmlArray("ComparisonDatas")]
        //[XmlArrayItem("ComparisonSubDatasItemString", typeof(string))]
        //[XmlArrayItem("ComparisonSubDatasItemInt", typeof(int))]
        public object Values 
        {
            get { return m_values; }
            set { m_values = value; }
        }

        /// <summary>
        /// 查询条件的内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string columnName = this.FullPropertyName;
            //int idx = columnName.LastIndexOf(':');
            //if (idx != -1)
            //{
            //    columnName = columnName.Substring(idx+1);
            //}
            sb.Append("\"" + columnName + "\"");
            sb.Append(" ");
            sb.Append(GetSimpleOperatorName(this.Operator));
            sb.Append(" ");

            if (this.Values != null)
            {
                if (this.Values is IList)
                {
                    IList array = this.Values as IList;
                    sb.Append("\"[");
                    for (int i = 0; i < array.Count; ++i)
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(array[i].ToString());
                    }
                    sb.Append("]\"");
                }
                else
                {
                    //if (this.Values.GetType() == typeof(DateTime))
                    //{
                    //    sb.Append(((DateTime)this.Values).ToShortDateString());
                    //}
                    //else
                    {
                        sb.Append("\"" + this.Values.ToString() + "\"");
                    }
                }
            }
            else
            {
                if (this.Operator == SimpleOperator.Eq
                    || this.Operator == SimpleOperator.EqProperty
                    || this.Operator == SimpleOperator.GInG
                    || this.Operator == SimpleOperator.InG
                    || this.Operator == SimpleOperator.Like
                    || this.Operator == SimpleOperator.NotEq
                    || this.Operator == SimpleOperator.NotEqProperty)
                {
                    sb.Append("\"\"");
                }
            }
            return sb.ToString();
        }

        private static string GetSimpleOperatorName(SimpleOperator op)
        {
            switch (op)
            {
                case SimpleOperator.Like:
                    return "Like";
                case SimpleOperator.InG:
                    return "In";
                case SimpleOperator.NotEq:
                    return "<>";
                case SimpleOperator.IsNotNull:
                    return "ISNOTNULL";
                case SimpleOperator.Gt:
                    return ">";
                case SimpleOperator.Ge:
                    return ">=";
                case SimpleOperator.Eq:
                    return "=";
                case SimpleOperator.EqProperty:
                    return "=P";
                case SimpleOperator.GInG:
                    return "InG";
                case SimpleOperator.IsNull:
                    return "ISNULL";
                case SimpleOperator.Lt:
                    return "<";
                case SimpleOperator.Le:
                    return "<=";
                case SimpleOperator.Sql:
                    return "IsSql";
                default:
                    throw new ArgumentException("Invalide op of " + op);
            }
        }
    }
}
