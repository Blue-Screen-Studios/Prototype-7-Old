using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Assembly.IBX.Web
{
    public class IBXWebRequest
    {
        private HttpClient client;

        public IBXWebRequest()
        {
            client = new HttpClient();
        }

        public async Task<string> Get(string url, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string url, Dictionary<string, string> headers = null, Dictionary<string, string> formData = null)
        {
            var content = new FormUrlEncodedContent(formData ?? new Dictionary<string, string>());

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Put(string url, Dictionary<string, string> headers = null, Dictionary<string, string> formData = null)
        {
            var content = new FormUrlEncodedContent(formData ?? new Dictionary<string, string>());

            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = content
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}