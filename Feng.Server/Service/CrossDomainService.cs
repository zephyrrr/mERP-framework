using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using System.Text;

namespace Feng.Server.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class CrossDomainService : ICrossDomainService
    {
        public Stream ProvideClientAccessPolicyFile()
        {
#if DEBUG
            Console.WriteLine("ProvideClientAccessPolicyFile");
#endif
            return GetFile("clientaccesspolicy.xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Stream ProvideCrossDomainFile()
        {
#if DEBUG
            Console.WriteLine("ProvideCrossDomainFile");
#endif
            return GetFile("crossdomain.xml");
        }

        private Stream GetFile(string fileName)
        {
            string result = null;

            if (!System.IO.File.Exists(fileName))
            {
                result = string.Format("File {0} is not existed!", fileName);
            }
            else
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    result = sr.ReadToEnd();
                }
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }
    }
}
