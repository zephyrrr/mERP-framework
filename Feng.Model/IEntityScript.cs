using System;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityScript : IScript
    {
        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        object CalculateExpression(string expression, object entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fullPropertyName"></param>
        /// <returns></returns>
        object GetPropertyValue(object entity, string fullPropertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fullPropertyName"></param>
        /// <param name="setValue"></param>
        void SetPropertyValue(object entity, string fullPropertyName, object setValue);
    }
}
