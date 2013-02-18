using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace Feng.Server.Web
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://www.feng.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GeneralSearch : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            if (string.IsNullOrEmpty(context.Request.Path))
            {
                context.Response.Write("You must supply a name!");
                return;
            }

            try
            {
                string queryAddress = context.Request.PathInfo.Substring(1) + context.Request.Url.Query;    // remove PathInfo first "/"

                string webServiceHost = "http://" + context.Request.Url.Host + ":8080/";
                string s = SearchManagerXslt.GenerateHtml(queryAddress, webServiceHost);
                context.Response.Write(s);
            }
            catch (System.ServiceModel.Web.WebFaultException ex)
            {
                context.Response.StatusCode = (int)ex.StatusCode;
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
