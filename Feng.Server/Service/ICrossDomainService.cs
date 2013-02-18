using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.IO;

namespace Feng.Server.Service
{
    ///
    /// Allows the service to be available cross domain,
    /// specifically for Silverlight.
    ///
    [ServiceContract]
    public interface ICrossDomainService
    {
         //uncomment this if you want to return a clientaccesspolicy.xml
         //if you do this then I think crossdomain won't work!
        [OperationContract]
        [WebGet(UriTemplate = "/ClientAccessPolicy.xml")]
        //Message ProvideClientAccessPolicyFile();
        Stream ProvideClientAccessPolicyFile();

        ///
        /// Retrieve the text from the CrossDomain.xml file (or from the method)
        /// as a message
        ///
        [OperationContract]
        [WebGet(UriTemplate = "/crossdomain.xml")]
        Stream ProvideCrossDomainFile();
    }
}
