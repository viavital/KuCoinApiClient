using KuCoinApiClient.Model;
using Newtonsoft.Json;
using System.Text;

namespace KuCoinApiClient.Services
{
    public class HttpService
    {
        private readonly ILogger _logger;
        public HttpService(ILogger<HttpService> logger)
        {
            _logger = logger;
        }
        public async Task<T> DoPost<T>(string url, Object body) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                var result = await response.Content.ReadAsStringAsync();
                var resultParsed = JsonConvert.DeserializeObject<HttpResponseModel<T>>(result);
                return resultParsed.data;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }

        public async Task<T> DoGet<T>(string url) where T : class
        {
            try
            {
                using var client = new HttpClient();
                var result = await client.GetStringAsync(url);
                var resultParsed = JsonConvert.DeserializeObject<HttpResponseModel<T>>(result);
                return resultParsed.data;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("\nException Caught Message :{0}", e.Message!);                
                return null;
            }
        }
    }
}
