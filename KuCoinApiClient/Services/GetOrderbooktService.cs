using KuCoinApiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KuCoinApiClient.Services
{
    public class GetOrderbooktService
    {
        
        private string Symbols; // like "BTC-USDT"
        Mutex mutexObj = new();
        public GetOrderbooktService(string symbols)
        {
            Symbols = symbols;
        }
        public async Task<DataForOrderBook> GetOrderbook()
        {
            HttpClient client = new HttpClient();               
            HttpResponseMessage response = await client.GetAsync("https://api.kucoin.com/api/v1/market/orderbook/level2_20?symbol=" + Symbols); // drop request
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync(); 
            var orderbook =  JsonConvert.DeserializeObject<PartOrderBookModel>(responseBody);
            
            return orderbook.data;
        }

        
    }
}
