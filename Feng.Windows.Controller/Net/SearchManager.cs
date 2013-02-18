using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using Feng.Collections;
using Feng.Utils;

namespace Feng.Net
{
    /// <summary>
    /// 用于WebService的查找管理器
    /// </summary>
    public class SearchManager : AbstractSearchManager
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="webServiceTypeName"></param>
        /// <param name="serviceAddress"></param>
        public SearchManager(string webServiceTypeName = null, string serviceAddress = null)
            : base()
        {
            if (string.IsNullOrEmpty(webServiceTypeName))
            {
                //throw new ArgumentNullException("webServiceTypeName");
                webServiceTypeName = "Feng.Net.WebRestClient, Feng.Windows.Controller";
            }
            if (webServiceTypeName.ToUpper() == "REST")
                webServiceTypeName = "Feng.Net.WebRestClient, Feng.Windows.Controller";
            else if (webServiceTypeName.ToUpper() == "SOAP")
                webServiceTypeName = "Feng.Net.WebSoapClient, Feng.Windows.Controller";

            if (string.IsNullOrEmpty(serviceAddress))
            {
                //throw new ArgumentNullException("serviceAddress");
                serviceAddress = "Search";
            }
            if (!serviceAddress.StartsWith("http://"))
            {
                serviceAddress = string.Format("{0}/Generated/{1}.svc", SystemConfiguration.Server + "/" + SystemConfiguration.ApplicationName, serviceAddress);
            }

            m_client = ReflectionHelper.CreateInstanceFromType(ReflectionHelper.GetTypeFromName(webServiceTypeName), serviceAddress) as IWebServiceClient;
            if (m_client == null)
            {
                throw new ArgumentNullException(string.Format("webServiceTypeName of {0} with address {1} created failed!", webServiceTypeName, serviceAddress));
            }
            //m_webServiceTypeName = webServiceTypeName;
            //m_serviceAddress = serviceAddress;
        }
        //private string m_webServiceTypeName, m_serviceAddress;
        protected IWebServiceClient m_client;

        /// <summary>
        /// 读入数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            base.Result = GetData(searchExpression, searchOrders);
            base.Count = GetCount(searchExpression);
        }

        /// <summary>
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            string exp = SearchExpression.ToString(searchExpression);
            string order = SearchOrder.ToString(searchOrders);
            var list = m_client.GetData(exp, order, this.FirstResult, this.MaxResult);
            var ret = new List<IDictionary<string, object>>();
            foreach (var i in list)
            {
                ret.Add(Feng.Net.Utils.TypeHelper.ConvertTypeFromWSToDictionary(i));
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public override int GetCount(ISearchExpression searchExpression)
        {
            string exp = SearchExpression.ToString(searchExpression);
            return m_client.GetDataCount(exp);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManager sm = new SearchManager();
            sm.m_client = this.m_client;
            return sm;
        }
    }
}