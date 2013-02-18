using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Net;
using System.Collections.Specialized;
using Feng.Utils;

namespace Feng.Server.Service
{
    //[ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single)]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataSearchService<T, S> : IDataSearchRestService<T>, IDataSearchSoapService<T>
        where T: class, IEntity, new()
        where S: class, IEntity, new()
    {
        protected virtual int OnGetItemCount()
        {
            if (!Utils.AuthHelper.CheckAuthentication())
                return 0;

            string address = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.LocalPath;
            ISearchManager sm = new Feng.NH.SearchManager<S>();// ServiceHelper.GetSearchManagerFromAddress(address);
            if (sm == null)
                return 0;

            ISearchExpression exp = null;
            NameValueCollection nvc = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
            if (!string.IsNullOrEmpty(nvc["exp"]))
            {
                exp = SearchExpression.Parse(System.Web.HttpUtility.UrlDecode(nvc["exp"]));
            }
            return sm.GetCount(exp);
        }
        
        
        protected virtual List<T> OnGetItems()
        {
            if (!Utils.AuthHelper.CheckAuthentication())
                return null;

            string address = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.LocalPath;
            ISearchManager sm = new Feng.NH.SearchManager<S>();//ServiceHelper.GetSearchManagerFromAddress(address);
            if (sm == null)
                return null;
            NameValueCollection nvc = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
            var smParams = Utils.WebHelper.GetSearchManagerParameters(sm, nvc);
            IEnumerable list = sm.GetData(smParams.Item1, smParams.Item2);

            List<T> items = new List<T>();
            foreach (var i in list)
            {
                var item = Feng.Net.Utils.TypeHelper.ConvertTypeFromRealToWS<T>(i);
                items.Add(item);
            }

            //WindowTabInfo tabInfo = ADInfoBll.Instance.GetWindowTabInfo(ServiceHelper.GetWindowTabNameFromAddress(address));
            //using (GridDataConvert dp = new GridDataConvert())
            //{
            //    IList<Dictionary<string, object>> r = dp.Process(list, tabInfo.GridName);

            //    foreach (Dictionary<string, object> i in r)
            //    {
            //        T item = Feng.Utils.ReflectionHelper.CreateInstanceFromType(typeof(T)) as T;
            //        foreach (KeyValuePair<string, object> kvp in i)
            //        {
            //            EntityScript.SetPropertyValue(item, kvp.Key, kvp.Value);
            //        }
            //        items.Add(item);
            //    }
            //}

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
        #region JSON and XML interfaces for exposing the resource over HTTP.

        #region HTTP methods using XML format
        ItemList<T> IDataSearchRestService<T>.GetItemsInXml()
        {
#if DEBUG
            Console.WriteLine("DataSearchService<T> GetItemsInXml");
#endif
            return GetItems();
        }
        int IDataSearchRestService<T>.GetItemCountInXml()
        {
#if DEBUG
            Console.WriteLine("DataSearchService<T> GetItemCountInXml");
#endif
            return GetItemCount();
        }
        #endregion

        #region HTTP methods using JSON format

        ItemList<T> IDataSearchRestService<T>.GetItemsInJson()
        {
#if DEBUG
            Console.WriteLine("DataSearchService<T> GetItemsInJson");
#endif
            return GetItems();
        }
        int IDataSearchRestService<T>.GetItemCountInJson()
        {
#if DEBUG
            Console.WriteLine("DataSearchService<T> GetItemCountInJson");
#endif
            return GetItemCount();
        }
        #endregion

        #endregion

        ///// <summary>
        ///// Gets the item with the specified id. A null return value will result in a response status code of NotFound (404), unless the method explicitly sets the status code to a different error
        ///// </summary>
        ///// <param name="id">identifier for the item</param>
        ///// <returns>item corresponding to the given id. Null if the item does not exist</returns>
        //protected override T OnGetItem(string id)
        //{
        //    throw new NotSupportedException();
        //}

        ///// <summary>
        ///// Adds the item to the enumeration. A null return value will result in a response status code of InternalServerError (500), unless the method explicitly sets the status code to a different error
        ///// </summary>
        ///// <param name="initialValue">The item to add</param>
        ///// <param name="id">The id of the item to add</param>
        ///// <returns>The item if adding it was successful. Returns null if adding the item failed</returns>
        //protected override T OnAddItem(T initialValue, out string id)
        //{
        //    throw new NotSupportedException();
        //}

        ///// <summary>
        ///// Updates the item with the id specified. Returns null if the item does not exist or if the item could not be updated
        ///// If the item item does not already exist, throw a NotFound WebProtocolException.
        ///// Unless the method explicitly sets the status code to a different error, a null return value will result in a response status code of NotFound (404) if the item does not exist; 
        ///// if the item exists already, a null return value will result in a response status code of InternalServerError (500)
        ///// </summary>
        ///// <param name="id">The id of the item to update</param>
        ///// <param name="newValue">The new value for the item</param>
        ///// <returns>The updated item.</returns>
        //protected override T OnUpdateItem(string id, T newValue)
        //{
        //    throw new NotSupportedException();
        //}

        ///// <summary>
        ///// Delete the item with the specified id, if it exists. Return false if the item does not exist.
        ///// A return value of false will result in a response status code of NotFound (404) unless the method explicitly sets the status code to a different error.
        ///// </summary>
        ///// <param name="id">Item id to delete.</param>
        ///// <returns>True if item was successfully removed, otherwise false.</returns>
        //protected override bool OnDeleteItem(string id)
        //{
        //    throw new NotSupportedException();
        //}


        public T Get(string id)
        {
            var newId = ConvertId(id);
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<S>())
            {
                var i = rep.Get<S>(newId);
                var item = Feng.Net.Utils.TypeHelper.ConvertTypeFromRealToWS<T>(i);
                return item;
            }
        }
        private object ConvertId(string id)
        {
            var m = ServiceProvider.GetService<IEntityMetadataGenerator>().GenerateEntityMetadata(typeof(S));
            var idType = typeof(S).GetProperty(m.IdName).PropertyType;
            var newId = Feng.Utils.ConvertHelper.ChangeType(id, idType);
            return newId;
        }
    }
}
