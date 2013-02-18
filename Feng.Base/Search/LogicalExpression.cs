using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Search
{
    /// <summary>
    /// 逻辑条件
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// 而且
        /// </summary>
        And,
        /// <summary>
        /// 或者
        /// </summary>
        Or,
        /// <summary>
        /// 取反
        /// </summary>
        Not
    }

    /// <summary>
    /// An <see cref="ISearchExpression"/> that combines two <see cref="ISearchExpression"/>s 
    /// with a operator (either "<c>and</c>" or "<c>or</c>") between them.
    /// </summary>
    public class LogicalExpression : AbstractExpression
    {
        ///// <summary>
        ///// Private Constructor(for xml serialize)
        ///// </summary>
        //private LogicalExpression()
        //{
        //}

        private ISearchExpression _lhs;
        private ISearchExpression _rhs;
        private LogicalOperator _op;

        /// <summary>
        /// Initialize a new instance of the <see cref="LogicalExpression" /> class that
        /// combines two other <see cref="ISearchExpression"/>s.
        /// </summary>
        /// <param name="lhs">The <see cref="ISearchExpression"/> to use in the Left Hand Side.</param>
        /// <param name="rhs">The <see cref="ISearchExpression"/> to use in the Right Hand Side.</param>
        /// <param name="op"></param>
        public LogicalExpression(ISearchExpression lhs, ISearchExpression rhs, LogicalOperator op)
        {
            _lhs = lhs;
            _rhs = rhs;
            _op = op;
        }

        /// <summary>
        /// Gets the <see cref="ISearchExpression"/> that will be on the Left Hand Side of the Op.
        /// </summary>
        public ISearchExpression LeftHandSide
        {
            get { return _lhs; }
            //set { _lhs = value; }
        }

        /// <summary>
        /// Gets the <see cref="ISearchExpression" /> that will be on the Right Hand Side of the Op.
        /// </summary>
        public ISearchExpression RightHandSide
        {
            get { return _rhs; }
            //set { _rhs = value; }
        }

        /// <summary>
        /// LogicOperator
        /// </summary>
        public LogicalOperator LogicOperator
        {
            get { return _op; }
            //set { _op = value; }
        }

        /// <summary>
        /// 查询条件的内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_op == LogicalOperator.Not)
            {
                sb.Append("NOT ");
                sb.Append("(" + _lhs.ToString() + ")");
            }
            else
            {
                if (_lhs is LogicalExpression)
                {
                    sb.Append("(" + _lhs.ToString() + ")");
                }
                else
                {
                    sb.Append(_lhs.ToString());
                }
                sb.Append(LogicOperator == LogicalOperator.And ? " AND " : " OR ");
                if (_rhs is LogicalExpression)
                {
                    sb.Append("(" + _rhs.ToString() + ")");
                }
                else
                {
                    sb.Append(_rhs.ToString());
                }
            }
            return sb.ToString();
        }
    }
}
