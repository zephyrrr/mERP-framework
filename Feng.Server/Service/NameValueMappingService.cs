using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Net;
using System.Collections.Specialized;
using Feng;
using Feng.Utils;

namespace Feng.Server.Service
{
    [DataContract()]
    public class NameValueList
    {
        [DataMember()]
        public string Name
        {
            get;
            set;
        }

        [DataMember()]
        public Dictionary<string, string> NameValues
        {
            get;
            set;
        }
    }

    [ServiceContract()]
    public interface INameValueMappingService
    {
        [WebGet(UriTemplate = NameValueMappingService.XmlItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        NameValueList GetNameValueInXml(string name);

        [WebGet(UriTemplate = NameValueMappingService.JsonItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        NameValueList GetNameValueInJson(string name);
    }

    //[ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single)]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NameValueMappingService : INameValueMappingService
    {
        public const string XmlItemsTemplate = "/{name}/";
        public const string JsonItemsTemplate = "/{name}/?format=json";

        protected virtual NameValueList OnGetItems(string name)
        {
            NameValueMapping nv = NameValueMappingCollection.Instance[name];
            if (nv == null)
                return null;

            var dv = NameValueMappingCollection.Instance.GetDataSource(name);
            NameValueList ret = new NameValueList { Name = name, NameValues = new Dictionary<string, string>() };
            foreach (System.Data.DataRowView dvr in dv)
            {
                ret.NameValues[dvr[nv.ValueMember].ToString()] = dvr[nv.DisplayMember].ToString();
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public NameValueList GetItems(string name)
        {
            try
            {
                return OnGetItems(name);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                throw;
            }
            return null;
        }

        // URI template definitions for how clients can access the items using XML or JSON. Modify if needed
        // The URI template to get all the items in XML format or add an item in XML format. The URL is of the form http://<url-for-svc-file>/
        #region JSON and XML interfaces for exposing the resource over HTTP.

        #region HTTP methods using XML format
        public NameValueList GetNameValueInXml(string name)
        {
#if DEBUG
            Console.WriteLine("NameValueMappingService GetNameValueInXml");
#endif
            return GetItems(name);
        }
        #endregion

        #region HTTP methods using JSON format

        public NameValueList GetNameValueInJson(string name)
        {
#if DEBUG
            Console.WriteLine("NameValueMappingService GetNameValueInJson");
#endif
            return GetItems(name);
        }
        #endregion


        #endregion
    }
}
