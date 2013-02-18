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
    public class DataSearchViewService<T> : IDataSearchViewRestService<T>, IDataSearchViewSoapService<T>
        where T : class, IEntity, new()
    {
        protected string m_winTabName;
        protected virtual int OnGetItemCount()
        {
            if (string.IsNullOrEmpty(m_winTabName))
            {
                m_winTabName = Feng.Server.Utils.ServiceHelper.GetWindowTabNameFromAddress(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.LocalPath);
            }
            return DataSearchViewService.InternalGetItemCount(m_winTabName, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);
        }

        /// <returns>An enumeration of the (id, item) pairs. Returns null if no items are present</returns>
        protected virtual List<T> OnGetItems()
        {
            if (string.IsNullOrEmpty(m_winTabName))
            {
                m_winTabName = Feng.Server.Utils.ServiceHelper.GetWindowTabNameFromAddress(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.LocalPath);
            }
            var r = DataSearchViewService.InternalGetItems(m_winTabName, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);
            if (r == null)
                return null;

            List<T> items = new List<T>();
            foreach (Dictionary<string, object> i in r)
            {
                T item = Feng.Utils.ReflectionHelper.CreateInstanceFromType(typeof(T)) as T;
                foreach (KeyValuePair<string, object> kvp in i)
                {
                    EntityScript.SetPropertyValue(item, kvp.Key, kvp.Value);
                }
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ItemList<T> GetItems()
        {
            try
            {
                var r = OnGetItems();
                if (r != null)
                {
                    return new ItemList<T>(r);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                throw;
            }
            return null;
        }

        public int GetItemCount()
        {
            try
            {
                return OnGetItemCount();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                throw;
            }
            return 0;
        }

        // URI template definitions for how clients can access the items using XML or JSON. Modify if needed
        // The URI template to get all the items in XML format or add an item in XML format. The URL is of the form http://<url-for-svc-file>/
        public const string XmlItemsTemplate = "/";
        public const string XmlItemCountTemplate = "/count";

        // The URI template to manipulate a particular item in XML format. The URL is of the form http://<url-for-svc-file>/item1
        //public const string XmlItemTemplate = "{id}";
        // The URI template to get all the items in JSON format or add an item in JSON format. The URL is of the form http://<url-for-svc-file>/?format=json
        public const string JsonItemsTemplate = "/?format=json";
        public const string JsonItemCountTemplate = "/count?format=json";
        // The URI template to manipulate a particular item in JSON format. The URL is of the form http://<url-for-svc-file>/item1?format=json
        //public const string JsonItemTemplate = "{id}?format=json";

        #region JSON and XML interfaces for exposing the resource over HTTP.

        #region HTTP methods using XML format
        ItemList<T> IDataSearchViewRestService<T>.GetItemsInXml()
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService<T> GetItemsInXml");
#endif
            return GetItems();
        }
        int IDataSearchViewRestService<T>.GetItemCountInXml()
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService<T> GetItemCountInXml");
#endif
            return GetItemCount();
        }
        #endregion

        #region HTTP methods using JSON format

        ItemList<T> IDataSearchViewRestService<T>.GetItemsInJson()
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService<T> GetItemsInJson");
#endif
            return GetItems();
        }
        int IDataSearchViewRestService<T>.GetItemCountInJson()
        {
#if DEBUG
            Console.WriteLine("DataSearchViewService<T> GetItemCountInJson");
#endif
            return GetItemCount();
        }
        #endregion

        #endregion
    }
}
