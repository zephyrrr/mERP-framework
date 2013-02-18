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
    public interface IDataSearchRestService<T>
        where T : class, IEntity, new()
    {
        [WebGet(UriTemplate = DataSearchViewService<T>.XmlItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        ItemList<T> GetItemsInXml();

        [WebGet(UriTemplate = DataSearchViewService<T>.XmlItemCountTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        int GetItemCountInXml();

        [WebGet(UriTemplate = DataSearchViewService<T>.JsonItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ItemList<T> GetItemsInJson();

        [WebGet(UriTemplate = DataSearchViewService<T>.JsonItemCountTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int GetItemCountInJson();
    }

    [ServiceContract]
    public interface IDataSearchSoapService<T>
        where T : class, IEntity, new()
    {
        [WebGet()]
        [OperationContract]
        ItemList<T> GetItems();

        [WebGet()]
        [OperationContract]
        int GetItemCount();
    }

    //[CollectionDataContract(Name = "ItemList", Namespace = "")]
    //public class ItemList<T> : List<T>
    //    where T : class, IEntity, new()
    //{
    //    public ItemList()
    //        : base()
    //    {
    //    }

    //    public ItemList(IEnumerable<T> items)
    //        : base(items)
    //    {
    //    }
    //}
}
