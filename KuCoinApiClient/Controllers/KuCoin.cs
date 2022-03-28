
using KuCoinApiClient.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;
using Microsoft.Extensions.Logging;


namespace KuCoinApiClient.Controllers
{
    public class KuCoin : Controller
    {

        private async Task<string> GetToken()
        {
            HttpClient client = new HttpClient();
            var content = new System.Net.Http.StringContent("");
            HttpResponseMessage response = await client.PostAsync("https://api.kucoin.com/api/v1/bullet-public", content); // кидає запит
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync(); // виводить результат запиту на екран
            var TokenMessClass = JsonConvert.DeserializeObject<GetTokenMessage>(responseBody);
            return TokenMessClass.data.token;
        }

        private async Task <string> GetMarketDataBySocket()
        {
            string WelcomeMessage = "null";
            var tcs = new TaskCompletionSource();
            string token = await GetToken();
            using var socket = new WebSocket ("wss://ws-api.kucoin.com/endpoint?token=" + token);
            socket.MessageReceived += (s, e) =>
           {
               WelcomeMessage = e.Message.ToString();
               tcs.TrySetResult();
           };
            socket.Open();
            await tcs.Task;
            return WelcomeMessage;
           // ("{\"id\":1545910660740, \"type\":\"subscribe\",\"topic\": \"/spotMarket/level2Depth5:BTC-USDT\", \"response\": true }")), WebSocketMessageType.Text, false, System.Threading.CancellationToken.None);
                    
        }
    public async Task<IActionResult> Market()
        { 
            var message =  await GetMarketDataBySocket();   
            ViewBag.MarketData = message;            
            return View();
        }
    }        
}

