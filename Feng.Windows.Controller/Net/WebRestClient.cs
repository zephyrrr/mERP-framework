using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Feng.Net
{
    /// <summary>
    /// 
    /// </summary>
    public enum WebRestDatType
    {
        /// <summary>
        /// 
        /// </summary>
        Xml = 1,
        /// <summary>
        /// 
        /// </summary>
        Json = 2,
    }

    /// <summary>
    /// Rest Client for Search
    /// </summary>
    [CLSCompliant(false)]
    public class WebRestClient : IWebServiceClient
    {
        private Feng.Windows.Net.MyAuthHttpClient m_httpClient = new Feng.Windows.Net.MyAuthHttpClient();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceAddress"></param>
        public WebRestClient(string serviceAddress)
        {
            m_serviceAddress = serviceAddress;
            if (!m_serviceAddress.EndsWith("/"))
                m_serviceAddress += "/";
        }

        private string m_serviceAddress;
        private WebRestDatType m_webRestDataFormat = WebRestDatType.Json;
        public WebRestDatType RestDataFormat
        {
            get { return m_webRestDataFormat; }
            set { m_webRestDataFormat = value; }
        }
        
        /// <summary>
        /// 读入数据
        /// </summary>
        /// <returns>符合条件的数据。结果格式为IList[Dictionary[string, object]]</returns>
        public System.Collections.IEnumerable GetData(string searchExpression = null, string searchOrder = null, int? firstResult = null, int? maxResult = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append("?");
            if (m_webRestDataFormat == WebRestDatType.Json)
            {
                query.Append("format=json&");
            }
            if (!string.IsNullOrEmpty(searchExpression))
            {
                query.Append(string.Format("exp={0}&", searchExpression));
            }
            if (!string.IsNullOrEmpty(searchOrder))
            {
                query.Append(string.Format("order={0}&", searchOrder));
            }
            if (firstResult.HasValue)
            {
                query.Append(string.Format("first={0}&", firstResult));
            }
            if (maxResult.HasValue)
            {
                query.Append(string.Format("count={0}&", maxResult));
            }

            string addr = m_serviceAddress + query.ToString();
            try
            {
                var s = m_httpClient.GetSync(addr);
                if (string.IsNullOrEmpty(s))
                    return null;

                string jsonText = null;
                if (m_webRestDataFormat == WebRestDatType.Json)
                {
                    jsonText = s;
                }
                else if (m_webRestDataFormat == WebRestDatType.Xml)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(s);
                    jsonText = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                    //// XsdDataContractImporter to generate Type from xsd
                    //// Todo: Xsd Validate and Xsd DataType
                    //IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
                    //if (xmlDoc.HasChildNodes)
                    //{
                    //    foreach (XmlNode node in xmlDoc.ChildNodes)
                    //    {
                    //        IDictionary<string, object> dict = new Dictionary<string, object>();
                    //        foreach (XmlNode subNode in node.ChildNodes)
                    //        {
                    //            dict[subNode.Name] = subNode.Value;
                    //            // Todo: Change Type
                    //        }
                    //        ret.Add(dict);
                    //    }
                    //}
                }
                else
                {
                    throw new NotSupportedException("Invalid WebRestDatFormat!");
                }

                object data = Newtonsoft.Json.JsonConvert.DeserializeObject(s);

                var list = data as System.Collections.IEnumerable;
                if (list == null)
                {
                    throw new NotSupportedException("Returned Jons string should be IEnumerable!");
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when access " + addr, ex);
            }
        }

        /// <summary>
        /// 读入数据条数
        /// </summary>
        /// <param name="searchExpression">查找条件</param>
        /// <returns>符合条件的数据条数</returns>
        public int GetDataCount(string searchExpression = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append("/count?");
            if (m_webRestDataFormat == WebRestDatType.Json)
            {
                query.Append("format=json&");
            }
            if (!string.IsNullOrEmpty(searchExpression))
            {
                query.Append(string.Format("exp={0}&", searchExpression));
            }
            string addr = m_serviceAddress + query.ToString();
            try
            {
                var s = m_httpClient.GetSync(addr);
                if (string.IsNullOrEmpty(s))
                    return 0;

                if (m_webRestDataFormat == WebRestDatType.Json)
                {
                    int? ret = Feng.Utils.ConvertHelper.ToInt(Newtonsoft.Json.JsonConvert.DeserializeObject(s));
                    if (ret.HasValue)
                        return ret.Value;
                    else
                        return 0;
                }
                else if (m_webRestDataFormat == WebRestDatType.Xml)
                {
                    // Todo
                    return -1;
                }
                else
                {
                    throw new NotSupportedException("Invalid WebRestDatType!");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when access " + addr, ex);
            }
        }
    }
}
