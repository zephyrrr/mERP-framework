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
    [ServiceContract]
    public interface IDataSearchViewRestService
    {
        #region XML format APIs
        //[WebHelp(Comment = "Returns the items in the collection in XML format, along with URI links to each item.")]
        [WebGet(UriTemplate = DataSearchViewService.XmlItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        List<AjaxDictionary<string, object>> GetItemsInXml(string winTab);

        //[WebHelp(Comment = "Returns the item count in XML format")]
        [WebGet(UriTemplate = DataSearchViewService.XmlItemCountTemplate, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        int GetItemCountInXml(string winTab);
        #endregion

        #region JSON format APIs
        //[WebHelp(Comment = "Returns the items in the collection in JSON format, along with URI links to each item.")]
        [WebGet(UriTemplate = DataSearchViewService.JsonItemsTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<AjaxDictionary<string, object>> GetItemsInJson(string winTab);
        //[WebHelp(Comment = "Returns the item count in JSON format")]
        [WebGet(UriTemplate = DataSearchViewService.JsonItemCountTemplate, BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int GetItemCountInJson(string winTab);
        #endregion
    }
}
