using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Feng.Net
{
    public class MyHttpClient
    {
        public MyHttpClient()
        {
        }

        protected virtual HttpClientHandler CreateHandle()
        {
            return new HttpClientHandler();
        }
        private HttpClient CreateClient()
        {
            HttpClientHandler handler = CreateHandle();

            HttpClient client = new HttpClient(handler);
            client.MaxResponseContentBufferSize = 5242880;
            return client;
        }
        private string GetResult(Task<HttpResponseMessage> task)
        {
            var t = task.ContinueWith((requestTask) =>
            {
                System.Net.Http.HttpResponseMessage response = requestTask.Result;
                
                var t2 = response.Content.ReadAsStringAsync().ContinueWith((readTask) =>
                {
                    string s = readTask.Result;
                    return s;
                });
                t2.Wait();
                string r = t2.Result;
                //response.EnsureSuccessStatusCode();
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException(string.Format("Invalid Rest Service with StatusCode: {0}, Msg: {1}",
                        response.StatusCode, r));
                }
                return r;
            });
            t.Wait();
            return t.Result;
        }
        public string GetSync(string addr)
        {
            var client = CreateClient();
            var t = GetResult(client.GetAsync(addr));
            return t;
        }

        public string PostSync(string addr, string postJson)
        {
            var client = CreateClient();
            System.Net.Http.HttpContent httpContent = new System.Net.Http.StringContent(postJson, Encoding.UTF8, "application/json");
            var t = GetResult(client.PostAsync(addr, httpContent));
            return t;
        }
        public string DeleteSync(string addr)
        {
            var client = CreateClient();
            var t = GetResult(client.DeleteAsync(addr));
            return t;
        }
        public string PutSync(string addr, string postJson)
        {
            var client = CreateClient();
            System.Net.Http.HttpContent httpContent = new System.Net.Http.StringContent(postJson, Encoding.UTF8, "application/json");
            var t = GetResult(client.PutAsync(addr, httpContent));
            return t;
        }
    }
}
