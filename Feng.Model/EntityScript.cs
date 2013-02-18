using System;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityScript
    {
        ///// <summary>
        ///// 执行statement
        ///// </summary>
        ///// <param name="statement"></param>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public static object ExecuteStatement(string statement, object entity)
        //{

        //}

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object CalculateExpression(string expression, object entity)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }
            string entityKey = EntityHelper.ReplaceEntity(expression, entity);
            string key = "EntityHelper.CalculateExpression:" + expression + ";entityKey:" + entityKey;

            return Cache.TryGetCache<object>(key, new Func<object>(delegate()
            {
                return ServiceProvider.GetService<IEntityScript>().CalculateExpression(expression, entity);
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object CalculateExpression(string expression, System.Collections.Generic.Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }
            return ServiceProvider.GetService<IEntityScript>().CalculateExpression(expression, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fullPropertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object entity, string fullPropertyName)
        {
            return ServiceProvider.GetService<IEntityScript>().GetPropertyValue(entity, fullPropertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fullPropertyName"></param>
        /// <param name="setValue"></param>
        public static void SetPropertyValue(object entity, string fullPropertyName, object setValue)
        {
            ServiceProvider.GetService<IEntityScript>().SetPropertyValue(entity, fullPropertyName, setValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="navigator"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object entity, string navigator, string propertyName)
        {
            return GetPropertyValue(entity, string.IsNullOrEmpty(navigator) ? propertyName : navigator + "." + propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="navigator"></param>
        /// <param name="propertyName"></param>
        /// <param name="setValue"></param>
        public static void SetPropertyValue(object entity, string navigator, string propertyName, object setValue)
        {
            SetPropertyValue(entity, string.IsNullOrEmpty(navigator) ? propertyName : navigator + "." + propertyName, setValue);
        }
    }
}
