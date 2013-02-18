using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Feng.Net
{
    /// <summary>
    /// Http方法
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// Get
        /// </summary>
        Get = 1,
        /// <summary>
        /// Post
        /// </summary>
        Post = 2,
        /// <summary>
        /// Put
        /// </summary>
        Put = 3,
        /// <summary>
        /// Delete
        /// </summary>
        Delete = 4
    }

    /// <summary>
    /// Web读取基类（可保存Cookie）。
    /// 可以Get、Post方式读取。
    /// 可读取文本和二进制数据。
    /// </summary>
    public class WebProxy
    {
        /// <summary>
        /// 任意类型序列化成byte[](xml)
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Serialize(object src)
        {
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(ms, src);
            byte[] byteArray = ms.ToArray();

            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        /// 任意类型从byte[](xml)反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object Deserialize(string str)
        {
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(str));
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return serializer.Deserialize(ms);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveCookie()
        {
            if (string.IsNullOrEmpty(m_name))
            {
                throw new InvalidOperationException("If you want to save cookie, please supply a name!");
            }
            if (m_cookieContainer.Count > 0)
            {
                string cookieFileName = "WebProxy_" + m_name + ".cookie";
                using (StreamWriter sw = new StreamWriter(cookieFileName, false))
                {
                    foreach (Uri uri in m_uris)
                    {
                        var cookies = m_cookieContainer.GetCookies(uri);
                        foreach (Cookie c in cookies)
                        {
                            string s = Serialize(c);
                            sw.WriteLine(s);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ClearCookie
        /// </summary>
        public void ClearCookie()
        {
            this.m_cookieContainer = new CookieContainer();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public WebProxy(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                m_name = name;
                string cookieFileName = "WebProxy_" + m_name + ".cookie";
                if (System.IO.File.Exists(cookieFileName))
                {
                    using (StreamReader sr = new StreamReader(cookieFileName))
                    {
                        while (true)
                        {
                            string s = sr.ReadLine();
                            if (string.IsNullOrEmpty(s))
                                break;
                            try
                            {
                                Cookie c = Deserialize(s) as Cookie;
                                if (c != null)
                                {
                                    m_cookieContainer.Add(c);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }

        private string m_name;
        private Encoding m_encoding = Encoding.GetEncoding("gb2312");
        private CookieContainer m_cookieContainer = new CookieContainer();

        /// <summary>
        /// 历次读取所返回的Cookie。再下次读取时会自动发送
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return m_cookieContainer; }
        }

        /// <summary>
        /// 编码方式
        /// </summary>
        public Encoding Encoding
        {
            get { return m_encoding; }
            set { m_encoding = value; }
        }

        private ICredentials m_credentials;
        /// <summary>
        /// Credentials
        /// </summary>
        public ICredentials Credentials
        {
            get { return m_credentials; }
            set { m_credentials = value; }
        }

        private string m_contentType;
        /// <summary>
        /// 
        /// </summary>
        public string ContentType
        {
            get { return m_contentType; }
            set { m_contentType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Accept
        {
            get;
            set;
        }

        public string Proxy
        {
            get;
            set;
        }

        private void SetWebRequestDefaults(HttpWebRequest request)
        {
            if (m_credentials != null)
            {
                request.Credentials = m_credentials;
            }
            request.CookieContainer = m_cookieContainer;
            if (!string.IsNullOrEmpty(this.Accept))
            {
                request.Accept = this.Accept;
            }
            if (!string.IsNullOrEmpty(this.Proxy))
            {
                request.Proxy = new System.Net.WebProxy(this.Proxy);
            }
            request.KeepAlive = false;
        }
        /// <summary>
        /// Put Data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string PutToString(string url, byte[] data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                SetWebRequestDefaults(request);

                request.Method = "PUT";
                request.ContentType = string.IsNullOrEmpty(m_contentType) ? "application/octet-stream" : m_contentType;
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return GetString(response);
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                string s = GetString(response);
                throw new WebException(s, ex);
            }
        }

        /// <summary>
        /// 根据Url以Get方式直接读取文本(html文件等)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetToString(string url)
        {
            HttpWebResponse myResponse = CreateGetResponse(url);
            return GetString(myResponse);
        }

        /// <summary>
        /// 根据Url以Get方式直接读取二进制文件(图像、文件等)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public byte[] GetToBytes(string url)
        {
            HttpWebResponse myResponse = CreateGetResponse(url);
            return GetBytes(myResponse);
        }

        /// <summary>
        /// 根据Url以Post方式直接读取文本(html文件等)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string PostToString(string url, string postData)
        {
            HttpWebResponse myResponse = CreatePostResponse(url, postData);
            return GetString(myResponse);
        }

        /// <summary>
        /// 根据Url以Post方式直接读取二进制文件(图像、文件等)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public byte[] PostToBytes(string url, string postData)
        {
            HttpWebResponse myResponse = CreatePostResponse(url, postData);
            return GetBytes(myResponse);
        }

        /// <summary>
        /// 根据WebResponse读取文本
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string GetString(WebResponse response)
        {
            Stream stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream, m_encoding);
            string rs = sr.ReadToEnd();

            sr.Close();
            stream.Close();
            response.Close();

            return rs;
        }

        /// <summary>
        /// 根据WebResponse读取二进制数据
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private byte[] GetBytes(WebResponse response)
        {
            Stream stream = response.GetResponseStream();

            BinaryReader sr = new BinaryReader(stream, m_encoding);
            List<byte> ret = new List<byte>();
            int len = 10240;
            while (true)
            {
                byte[] bytes = sr.ReadBytes(len);
                ret.AddRange(bytes);
                if (bytes.Length != len)
                    break;
            }

            sr.Close();
            stream.Close();
            response.Close();

            return ret.ToArray(); ;
        }

        private List<Uri> m_uris = new List<Uri>();

        /// <summary>
        /// 根据Url以Get方式创建HttpWebResponse
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpWebResponse CreateGetResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            SetWebRequestDefaults(request);
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.Cookies.Count > 0)
                {
                    foreach (Cookie cookie in response.Cookies)
                    {
                        m_cookieContainer.Add(cookie);
                    }
                    m_uris.Add(new Uri(url));
                }
                return response;
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                string s = GetString(response);
                throw new WebException(s, ex);
            }
        }

        /// <summary>
        /// 根据Url以Post方式创建HttpWebResponse
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private HttpWebResponse CreatePostResponse(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            SetWebRequestDefaults(request);
            request.ContentType = string.IsNullOrEmpty(m_contentType) ? "application/x-www-form-urlencoded" : m_contentType;
            request.Method = "POST";

            Stream stream = request.GetRequestStream();
            byte[] byteArray = m_encoding.GetBytes(postData);
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.Cookies.Count > 0)
                {
                    foreach (Cookie cookie in response.Cookies)
                    {
                        m_cookieContainer.Add(cookie);
                    }
                    m_uris.Add(new Uri(url));
                }
                return response;
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                string s = GetString(response);
                throw new WebException(s, ex);
            }
        }

        private static Regex m_regexHtmlTag = new Regex(@"<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// 移除网页中处在的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpaces(string str)
        {
            string s = m_regexHtmlTag.Replace(str, "");
            return s.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", "").Trim();
        }

    }
}
