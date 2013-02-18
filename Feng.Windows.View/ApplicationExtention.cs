using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationExtention
    {
        private static string Encrypt(string s)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s));
        }
        private static string Decrypt(string s)
        {
            byte[] b = Convert.FromBase64String(s);
            return System.Text.Encoding.UTF8.GetString(b, 0, b.Length);
        }

        private const string s_addressHeader = "http://";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="actionName"></param>
        public static string GetNavigatorAddress(string actionName, IDisplayManager dm = null)
        {
            SearchHistoryInfo his = null;
            if (dm != null)
            {
                his = dm.SearchManager.GetHistory(0);
            }
            
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}/action/{1}/?", SystemConfiguration.ApplicationName, actionName));
            if (his != null && his.IsCurrentSession)
            {
                if (!string.IsNullOrEmpty(his.Expression))
                {
                    sb.Append(string.Format("exp={0}&", his.Expression));
                }
                if (!string.IsNullOrEmpty(his.Order))
                {
                    sb.Append(string.Format("order={0}&", his.Order));
                }
                if (dm.SearchManager.FirstResult != 0)
                {
                    sb.Append(string.Format("first={0}&", dm.SearchManager.FirstResult));
                }
                sb.Append(string.Format("count={0}&", dm.SearchManager.MaxResult));
            }

            return s_addressHeader + Encrypt(sb.ToString());
        }

        /// <summary>
        /// 按照自定义规则按照地址导航程序到某个界面
        /// http://cd/{action}/exp={exp}&order={order}&pos={pos}
        /// http://cd/action/查询统计_人员单位/?exp=编号 = 100000&order=编号&pos=1
        /// </summary>
        /// <param name="app"></param>
        /// <param name="address"></param>
        public static void NavigateTo(this IApplication app, string address)
        {
            if (string.IsNullOrEmpty(address) || !address.StartsWith(s_addressHeader))
                return;

            string content = address.Substring(s_addressHeader.Length);
            if (!content.Contains("action/"))
            {
                content = Decrypt(content);
                address = s_addressHeader + content;
            }

            UriTemplate template = new UriTemplate("action/{action}/?exp={exp}&order={order}&pos={pos}");
            Uri baseAddress = new Uri(s_addressHeader + SystemConfiguration.ApplicationName);
            Uri fullUri = new Uri(address);


            // Match a URI to a template
            UriTemplateMatch results = template.Match(baseAddress, fullUri);
            if (results != null && results.BaseUri == baseAddress)
            {
                try
                {
                    IDisplayManagerContainer dmC = app.ExecuteAction(results.BoundVariables["action"]) as IDisplayManagerContainer;
                    if (dmC == null)
                        return;
                    if (dmC.DisplayManager != null && dmC.DisplayManager.SearchManager != null)
                    {
                        var t = results.BoundVariables["first"];
                        if (t != null)
                        {
                            int? first = Feng.Utils.ConvertHelper.ToInt(t);
                            if (first.HasValue)
                            {
                                dmC.DisplayManager.SearchManager.FirstResult = first.Value;
                            }
                        }
                        t = results.BoundVariables["count"];
                        if (t != null)
                        {
                            int? count = Feng.Utils.ConvertHelper.ToInt(t);
                            if (count.HasValue)
                            {
                                dmC.DisplayManager.SearchManager.MaxResult = count.Value;
                            }
                        }
                        t = results.BoundVariables["exp"];
                        if (t != null)
                        {
                            var exp = SearchExpression.Parse(t);
                            var order = SearchOrder.Parse(results.BoundVariables["order"]);

                            if (exp != null)
                            {
                                dmC.DisplayManager.SearchManager.LoadData(exp, order);
                                dmC.DisplayManager.Position = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }
    }
}
