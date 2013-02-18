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
    public interface IDataOperationRestService<T>
        where T : class, IEntity, new()
    {
        [WebGet(UriTemplate = "/{id}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        T Get(string id);

        [WebInvoke(Method = "PUT", UriTemplate = "/", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool Insert(T entity);

        [WebInvoke(Method = "POST", UriTemplate = "/", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool Update(T entity);

        [WebInvoke(Method = "DELETE", UriTemplate = "/", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool Delete(T entity);
    }
}
