using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Server.Utils
{
    public class WebServiceInstance : Singleton<WebServiceInstance>
    {
        public Dictionary<string, string> m_tags = new Dictionary<string, string>();
        public void AddWebService(string address, string tag)
        {
            m_tags[address] = tag;
        }

        public string GetWebServiceTag(string address)
        {
            if (m_tags.ContainsKey(address))
            {
                return m_tags[address];
            }
            else
            {
                return null;
            }
        }
    }
}
