using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Net;
using System.Collections.Specialized;
using Feng.Windows.Utils;

namespace Feng.Server.Service
{
    //[ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataSearchViewService : IDataSearchViewRestService
    {
        // http://localhost:8080/查询统计_人员单位/?exp=编号  = 100000
        // http://localhost:8080/查询统计_人员单位/count?exp=编号  = 100000
        // http://localhost:8080/查询统计_人员单位/?format=json&exp= 编号  = 100000
        // http://localhost:8080/查询统计_人员单位/count?format=json&exp= 编号  = 100000
        internal static int InternalGetItemCount(string winTabName, NameValueCollection nvc)
        {
            if (!Utils.AuthHelper.CheckAuthentication())
                return 0;

            ISearchManager sm = Feng.Server.Utils.ServiceHelper.GetSearchManagerFromWindowTab(winTabName);
            if (sm == null)
                return 0;

            ISearchExpression exp = null;
            if (!string.IsNullOrEmpty(nvc["exp"]))
            {
                exp = SearchExpression.Parse(System.Web.HttpUtility.UrlDecode(nvc["exp"]));
            }
            return sm.GetCount(exp);
        }
        internal static IList<Dictionary<string, object>> InternalGetItems(string winTabName, NameValueCollection nvc)
        {
            if (!Utils.AuthHelper.CheckAuthentication())
                return null;

            ISearchManager sm = Feng.Server.Utils.ServiceHelper.GetSearchManagerFromWindowTab(winTabName);
            var smParams = Utils.WebHelper.GetSearchManagerParameters(sm, nvc);
            var se = smParams.Item1;
            var so = smParams.Item2;
            sm.FillSearchAdditionals(ref se, ref so);
            IEnumerable list = sm.GetData(se, so);

            WindowTabInfo tabInfo = ADInfoBll.Instance.GetWindowTabInfo(winTabName);
            bool includeDetail = false;
            if (!string.IsNullOrEmpty(nvc["detail"]))
            {
                includeDetail = Feng.Utils.ConvertHelper.ToBoolean(nvc["detail"]).Value;
            }
            using (GridDataConvert dp = new GridDataConvert(!includeDetail))
            {
                IList<Dictionary<string, object>> r = dp.Process(list, tabInfo.GridName);

                return r;
            }
        }

        protected virtual int OnGetItemCount(string winTabName)
        {
            return InternalGetItemCount(winTabName, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);
        }

        // http://localhost:20557/Search/Test/SearchManager.svc/%E5%91%98%E5%B7%A5/?exp=Id%20=%20100000
        protected virtual List<AjaxDictionary<string, object>> OnGetItems(string winTabName)
        {
            var r = InternalGetItems(winTabName, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);
            if (r == null)
                return null;

            var ret = new List<AjaxDictionary<string, object>>();
            foreach (Dictionary<string, object> j in r)
            {
                var l = new AjaxDictionary<string, object>();
                foreach (var kvp in j)
                {
                    l.Add(kvp.Key, kvp.Value);
                }
                ret.Add(l);
            }

            return ret;
        }

        private List<AjaxDictionary<string, object>> GetItems(string winTab)
        {
            try
            {
                return OnGetItems(winTab);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                throw;
            }
            return null;
        }

        public int GetItemCount(string winTab)
        {
            try
            {
                return OnGetItemCount(winTab);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                throw;
            }
            return 0;
        }

        public const string XmlItemsTemplate = "/{winTab}";
        public const string XmlItemCountTemplate = "/{winTab}/count/";
        public const string JsonItemsTemplate = "/{winTab}?format=json";
        public const string JsonItemCountTemplate = "/{winTab}/count?format=json";
        
        #region JSON and XML interfaces for exposing the resource over HTTP.

        List<AjaxDictionary<string, object>> IDataSearchViewRestService.GetItemsInXml(string winTab)
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService GetItemsInXml");
#endif
            return GetItems(winTab);
        }
        int IDataSearchViewRestService.GetItemCountInXml(string winTab)
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService GetItemCountInXml");
#endif
            return GetItemCount(winTab);
        }

        List<AjaxDictionary<string, object>> IDataSearchViewRestService.GetItemsInJson(string winTab)
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService GetItemsInJson");
#endif
            return GetItems(winTab);
        }
        int IDataSearchViewRestService.GetItemCountInJson(string winTab)
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService GetItemCountInJson");
#endif
            return GetItemCount(winTab);
        }
        #endregion
    }
}
