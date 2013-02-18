using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net.Http;

namespace Feng.Server.Web
{
    public class SearchManagerXslt
    {
        public static string GenerateHtml(string queryAddress, string webServiceAddress)
        {
            int idx = queryAddress.IndexOf("?");
            string windowTabName;
            if (idx == -1)
            {
                windowTabName = queryAddress;
            }
            else
            {
                windowTabName = queryAddress.Substring(0, idx);
            }

            string xsl = GenerateXsl(windowTabName);
            if (string.IsNullOrEmpty(xsl))
            {
                return null;
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(xsl);
            MemoryStream streamXslt = new MemoryStream(byteArray);
            XmlReader xsltReader = XmlReader.Create(streamXslt);
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltReader);
            xsltReader.Close();
            streamXslt.Close();

            var httpClient = new Feng.Windows.Net.MyAuthHttpClient();
            var s = httpClient.GetSync(webServiceAddress + queryAddress);
            XPathDocument xmlDoc = new XPathDocument(new MemoryStream(Encoding.UTF8.GetBytes(s)));

            StringWriter sw = new StringWriter();
            xslt.Transform(xmlDoc, null, sw);

            sw.Close();
            return sw.ToString();

            //ctx.Response.BufferOutput = true;
            //ctx.Response.Write(sw.ToString());

            //// There is known problem using Response.End(), 
            //// described in Q312629 in the inital release of .Net
            //// ctx.Response.End() should be used if not for the bug
            //ctx.ApplicationInstance.CompleteRequest();
        }

        public static string GenerateXsl(string windowTabName)
        {
            WindowTabInfo tabInfo = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            if (tabInfo == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version=""1.0""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
<xsl:output method=""html"" encoding=""UTF-8""/>
<xsl:template match=""/"">
<html>");
            sb.Append("<head><title>" + tabInfo.Name + "</title>");
            sb.Append(@"</head>
<body>
<table width=""100%"" border=""1"">
  <THEAD>
	<TR>");
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(tabInfo.GridName))
            {
                if (info.GridColumnType != GridColumnType.Normal)
                    continue;
                sb.Append("<TD><B>" + info.Caption + "</B></TD>");
            }
            sb.Append(@"</TR>
  </THEAD>");
            sb.Append("<TBODY>" +
    "<xsl:for-each select=\"" + "ItemList" + "/" + tabInfo.GridName + "\">" +
    "<TR>");
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(tabInfo.GridName))
            {
                if (info.GridColumnType != GridColumnType.Normal)
                    continue;
                sb.Append("<TD><xsl:value-of select=\"" + info.GridColumnName + "\" /></TD>");
            }
            sb.Append(@"</TR>
	</xsl:for-each>
  </TBODY>
</table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>");
            return sb.ToString();
        }
    }
}
