using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Search
{
    /// <summary>
    /// Base class for <see cref="ISearchExpression"/> implementations.
	/// </summary>
    public class AbstractExpression : ISearchExpression
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static AbstractExpression operator &(AbstractExpression lhs, AbstractExpression rhs)
        {
            return new LogicalExpression(lhs, rhs, LogicalOperator.And);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static AbstractExpression operator |(AbstractExpression lhs, AbstractExpression rhs)
        {
            return new LogicalExpression(lhs, rhs, LogicalOperator.Or);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crit"></param>
        /// <returns></returns>
        public static AbstractExpression operator !(AbstractExpression crit)
        {
            return new LogicalExpression(crit, null, LogicalOperator.Not);
        }

        /// <summary>
        /// See here for details:
        /// http://steve.emxsoftware.com/NET/Overloading+the++and++operators
        /// </summary>
        public static bool operator false(AbstractExpression criteria)
        {
            return false;
        }

        /// <summary>
        /// See here for details:
        /// http://steve.emxsoftware.com/NET/Overloading+the++and++operators
        /// </summary>
        public static bool operator true(AbstractExpression criteria)
        {
            return false;
        }
    }
}
