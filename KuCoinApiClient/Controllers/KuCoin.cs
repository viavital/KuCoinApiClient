
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
using WebSocketSharp;

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

        public async Task <IActionResult> Market()
        {
            string MarketMessage = "null";
            async Task GetMarketDataBySocket()
            {
                using (WebSocket socket = new WebSocket("wss://ws-api.kucoin.com/endpoint?token="+ GetToken()))
                {              
                    socket.OnMessage += (s, e) => { MarketMessage = e.Data.ToString(); };
                    socket.Connect();
                }
            }
            await GetMarketDataBySocket();

            ViewBag.MarketData = MarketMessage;

            return View();
        }
    }        
    }

