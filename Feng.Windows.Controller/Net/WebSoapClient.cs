using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Net;

namespace Feng.Net
{
    /// <summary>
    /// 用于SoapWebServices的查找管理器的对应WebServiceClient
    /// </summary>
    public class WebSoapClient : SoapHttpClientProtocol, IWebServiceClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceAddress"></param>
        public WebSoapClient(string serviceAddress)
        {
            base.CookieContainer = new CookieContainer();
            base.Credentials = CredentialCache.DefaultCredentials;
            base.PreAuthenticate = true; //Optional

            base.Url = serviceAddress;
        }

        private const string GetDataMethod = "GetData";
        private const string GetDataCountMethod = "GetDataCount";
        /// <summary>
        /// 读入数据
        /// </summary>
        /// <returns>符合条件的数据（根据具体查找方式，可能为IList或者DataView</returns>
        public System.Collections.IEnumerable GetData(string searchExpression = null, string searchOrder = null, int? firstResult = null, int? maxResult = null)
        {
            object[] results = this.Invoke(GetDataMethod, new object[] { searchExpression, searchOrder, firstResult, maxResult });
            if (results != null && results.Length > 0)
            {
                return (results[0] as System.Collections.IList);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 读入数据条数
        /// </summary>
        /// <param name="searchExpression">查找条件</param>
        /// <returns>符合条件的数据条数</returns>
        public int GetDataCount(string searchExpression = null)
        {
            object[] results = this.Invoke(GetDataCountMethod, new object[] { searchExpression });
            if (results != null && results.Length > 0)
            {
                return ((int)(results[0]));
            }
            else
            {
                return -1;
            }
        }
    }
}