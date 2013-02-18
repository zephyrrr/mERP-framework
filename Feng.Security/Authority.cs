using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security;

namespace Feng
{
    /// <summary>
    /// 基本权限
    /// 规则如下：
    /// 动词：AND, OR, NOT
    /// 特殊单词： I（用户），R（组），?（匿名），*（所有人）
    /// 可用括号限定优先级。
    /// 例如，I:ABC OR R:XYZ 代表 用户ABC 或者 是XYZ组里的人有权限
    /// </summary>
    public static class Authority
    {
        static Authority()
        {
            m_isAdministrators = Authority.AuthorizeByRule("R: " + SystemConfiguration.AdministratorsRoleName);
            m_isDevelopers = Authority.AuthorizeByRule("R: " + SystemConfiguration.DeveloperRoleName);
        }
        private static bool m_isAdministrators, m_isDevelopers;
        public static bool IsAdministrators()
        {
            return m_isAdministrators;
        }
        public static bool IsDeveloper()
        {
            return m_isDevelopers;
        }

        /// <summary>
        /// 判断用户是否满足一定的规则(按照实际规则）
        /// </summary>
        /// <param name="ruleExpression">规则</param>
        /// <returns></returns>
        public static bool AuthorizeByRule(string ruleExpression, System.Security.Principal.IPrincipal principal = null)
        {
            if (string.IsNullOrEmpty(ruleExpression))
            {
                return false;
            }
            if (principal == null)
            {
                principal = System.Threading.Thread.CurrentPrincipal;
            }
            //if (m_isDevelopers)
            //    return true;
            string key = "Authority.AuthorizeByRule:" + ruleExpression;
            return Cache.TryGetCache<bool>(key, new Func<bool>(delegate()
                {
                    if (ruleExpression.ToUpper() == "TRUE")
                        return true;
                    if (ruleExpression.ToUpper() == "FALSE")
                        return false;
                    ruleExpression = ruleExpression.Replace("or", "OR").Replace("and", "AND").Replace("not", "NOT");
                    Parser parser = new Parser();
                    BooleanExpression booleanExpression = parser.Parse(ruleExpression);
                    if (booleanExpression == null)
                    {
                        throw new InvalidOperationException("Invalid rule format " + ruleExpression);
                    }

                    bool result = booleanExpression.Evaluate(principal);
                    return result;
                }));
        }

        /// <summary>
        /// 判断用户是否满足一定的规则(按照.config文件里定义的规则名称)
        /// </summary>
        /// <param name="rulenName">规则名称</param>
        /// <returns></returns>
        public static bool AuthorizeByRuleName(string rulenName, System.Security.Principal.IPrincipal principal = null)
        {
            if (principal == null)
            {
                principal = System.Threading.Thread.CurrentPrincipal;
            }
            string key = "Authority.AuthorizeByRuleName:" + rulenName;
            return Cache.TryGetCache<bool>(key, new Func<bool>(delegate()
                {
                    try
                    {
                        return AuthorizationFactory.GetAuthorizationProvider().Authorize(principal, rulenName);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            return AuthorizationFactory.GetAuthorizationProvider().Authorize(principal, "Others");
                        }
                        catch (Exception ex)
                        {
                            ExceptionProcess.ProcessWithResume(ex);
                            return false;
                        }
                    }
                }));
        }
    }
}