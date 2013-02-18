using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Server.Utils
{
    public static class WebHelper
    {
        public static Tuple<ISearchExpression, IList<ISearchOrder>> GetSearchManagerParameters(ISearchManager sm, System.Collections.Specialized.NameValueCollection nvc)
        {
            sm.FirstResult = 0;
            sm.MaxResult = SearchManagerDefaultValue.MaxResult;

            ISearchExpression exp = null;
            IList<ISearchOrder> orders = null;
            if (!string.IsNullOrEmpty(nvc["exp"]))
            {
                exp = SearchExpression.Parse(System.Web.HttpUtility.UrlDecode(nvc["exp"]));
            }
            else if (!string.IsNullOrEmpty(nvc["filter"]))
            {
                exp = SearchExpression.Parse(System.Web.HttpUtility.UrlDecode(nvc["filter"]));
            }
            if (!string.IsNullOrEmpty(nvc["order"]))
            {
                orders = SearchOrder.Parse(System.Web.HttpUtility.UrlDecode(nvc["order"]));
            }
            else if (!string.IsNullOrEmpty(nvc["orderby"]))
            {
                orders = SearchOrder.Parse(System.Web.HttpUtility.UrlDecode(nvc["orderby"]));
            }
            if (!string.IsNullOrEmpty(nvc["first"]))
            {
                sm.FirstResult = Feng.Utils.ConvertHelper.ToInt(nvc["first"]).Value;
            }
            else if (!string.IsNullOrEmpty(nvc["skip"]))
            {
                sm.FirstResult = Feng.Utils.ConvertHelper.ToInt(nvc["skip"]).Value;
            }
            if (!string.IsNullOrEmpty(nvc["count"]))
            {
                sm.MaxResult = Feng.Utils.ConvertHelper.ToInt(nvc["count"]).Value;
            }
            else if (!string.IsNullOrEmpty(nvc["top"]))
            {
                sm.MaxResult = Feng.Utils.ConvertHelper.ToInt(nvc["top"]).Value;
            }
            return new Tuple<ISearchExpression, IList<ISearchOrder>>(exp, orders);
        }
    }
}
