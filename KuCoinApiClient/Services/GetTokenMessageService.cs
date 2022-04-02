using KuCoinApiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KuCoinApiClient.Services
{
    public class DataForGetToken
    {
       public string token { get; set; }
       object instanceServers { get; set; }

    }

    public class TokenMessageForSerilizaton
    {
        public string code { get; set; }
        public DataForGetToken data { get; set; }
    }
    public class GetTokenMessageService
    {
        public async Task<string> GetToken()
        {
            HttpClient client = new HttpClient();
            var content = new System.Net.Http.StringContent("");
            HttpResponseMessage response = await client.PostAsync("https://api.kucoin.com/api/v1/bullet-public", content); // кидає запит
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync(); // виводить результат запиту на екран
            var TokenMessClass = JsonConvert.DeserializeObject<TokenMessageForSerilizaton>(responseBody);
            return TokenMessClass.data.token;
        }
       
    }
}
