using System;
using System.Collections.Generic;
using System.Text;
using Feng.Search;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class LogicalFilter : IFilter
    {
        private LogicalOperator m_operator;
        private IFilter m_filter1, m_filter2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        public LogicalFilter(LogicalOperator op, IFilter filter1, IFilter filter2)
        {
            m_operator = op;
            m_filter1 = filter1;
            m_filter2 = filter2;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogicalOperator Operator
        {
            get { return m_operator; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IFilter FilterLeft
        {
            get { return m_filter1; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IFilter FilterRight
        {
            get { return m_filter2; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Evaluate(object value)
        {
            switch (m_operator)
            {
                case LogicalOperator.And:
                    return m_filter1.Evaluate(value) && m_filter2.Evaluate(value);
                case LogicalOperator.Or:
                    return m_filter1.Evaluate(value) || m_filter2.Evaluate(value);
                default:
                    System.Diagnostics.Debug.Assert(false, "Invalid op in LogicalFilter");
                    return false;
            }
        }
    }
}