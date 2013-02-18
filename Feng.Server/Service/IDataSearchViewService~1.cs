using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Net;
using System.Collections.Specialized;

namespace Feng.Server.Service
{
    /// <summary>
    /// Rest Service Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServiceContract]
    public interface IDataSearchViewRestService<T>
        where T : class, IEntity, new()
    {
        #region XML format APIs
        //[WebHelp(Comment = "Returns the items in the collection in XML format, along with URI links to each item.")]
        [WebGet(UriTemplate = DataSearchViewService<T>.XmlItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        ItemList<T> GetItemsInXml();

        //[WebHelp(Comment = "Returns the item count in XML format")]
        [WebGet(UriTemplate = DataSearchViewService<T>.XmlItemCountTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        int GetItemCountInXml();
        #endregion

        #region JSON format APIs
        //[WebHelp(Comment = "Returns the items in the collection in JSON format, along with URI links to each item.")]
        [WebGet(UriTemplate = DataSearchViewService<T>.JsonItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ItemList<T> GetItemsInJson();

        //[WebHelp(Comment = "Returns the item count in JSON format")]
        [WebGet(UriTemplate = DataSearchViewService<T>.JsonItemCountTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int GetItemCountInJson();
        #endregion
    }

    [ServiceContract]
    public interface IDataSearchViewSoapService<T>
        where T : class, IEntity, new()
    {
        [WebGet()]
        [OperationContract]
        ItemList<T> GetItems();

        [WebGet()]
        [OperationContract]
        int GetItemCount();
    }

    [CollectionDataContract(Name = "ItemList", Namespace = "")]
    public class ItemList<T> : List<T>
        where T : class, IEntity, new()
    {
        public ItemList()
            : base()
        {
        }

        public ItemList(IEnumerable<T> items)
            : base(items)
        {
        }
    }
}
