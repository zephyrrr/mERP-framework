using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Feng
{
    /// <summary>
    /// 包含实体类表达式的权限定义。
    /// 基本权限见<see cref="Authority"/>。
    /// 另外，增加实体类表达式定义，规则如下：
    /// 表达式用#..#包含，内部变量用$..$包含，
    /// 例如: 在票备案中，#$报关组编号$=300001# OR I:200001
    /// </summary>
    public static class Permission
    {
        //private static Regex s_regexExpression = new Regex(@"\$(.*?)\$", RegexOptions.Compiled);
        //private static Regex s_regexEntityParamter = new Regex(@"\#(.*?)\#", RegexOptions.Compiled);

        /// <summary>
        /// 判断用户是否满足一定的规则(包含实体类表达式).
        /// Tips：如果和字符串比较，需要用双引号""。
        /// int,double等则不需要加字符串
        /// 
        /// </summary>
        /// <param name="ruleExpression">表达式（可包含实体类表达式）</param>
        /// <param name="entity">对应的实体类</param>
        /// <returns></returns>
        public static bool AuthorizeByRule(string ruleExpression, object entity)
        {
            if (string.IsNullOrEmpty(ruleExpression))
            {
                return false;
            }

            string entityKey = EntityHelper.ReplaceEntity(ruleExpression, entity);
            string key = "Permission.AuthorizeByRule:" + ruleExpression + ";entityKey:" + entityKey;
            return Cache.TryGetCache<bool>(key, new Func<bool>(delegate()
                {
                    try
                    {
                        ruleExpression = EntityHelper.ReplaceExpression(ruleExpression, entity);

                        ruleExpression = ruleExpression.Replace("true", " I:* ").Replace("True", " I:* ").Replace("TRUE", " I:* ");
                        ruleExpression = ruleExpression.Replace("false", " I:Nobody ").Replace("False", " I:Nobody ").Replace("FALSE", " I:Nobody ");
                        return Authority.AuthorizeByRule(ruleExpression);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(string.Format("expression of {0} format is invalid!", ruleExpression), ex);
                    }
                }));
        }
    }
}