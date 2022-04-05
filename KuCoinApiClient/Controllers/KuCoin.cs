
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
using KuCoinApiClient.Models;
using System.Collections.Concurrent;
using System.Timers;

namespace KuCoinApiClient.Controllers
{
    public class KuCoin : Controller
    {
        private readonly ILogger<KuCoin> _logger;
        DataForOrderBook dataForOrderBook = new DataForOrderBook();
        BestBidAsk bestBidAsk = new BestBidAsk();
        ConcurrentQueue<ChangesStreamBuffer> changesStreamBuffers = new ConcurrentQueue<ChangesStreamBuffer>();
        GetOrderbooktService getOrderbook = new GetOrderbooktService("BTC-USDT");
        public KuCoin(ILogger<KuCoin> logger)
        {
            _logger = logger;

        }
        
        public async Task<IActionResult> Market()
        {
            string MessageFromSocket = "null";
            GetTokenMessageService getTokenMessageServ = new GetTokenMessageService();
            var tokenForConnection = await getTokenMessageServ.GetToken();
            var socket = new WebSocket("wss://ws-api.kucoin.com/endpoint?token=" + tokenForConnection);

            socket.MessageReceived +=  (s, e) =>
            {
                MessageFromSocket = e.Message.ToString();
                var chStrBuffer = JsonConvert.DeserializeObject<ChangesStreamBuffer>(MessageFromSocket);
                if (chStrBuffer != null && chStrBuffer.data != null)
                {
                    changesStreamBuffers.Enqueue(chStrBuffer); 
                }
            };
            socket.Opened += (s, e) => { _logger.LogInformation("socket established"); socket.Send("{\"id\":1545910660740, \"type\":\"subscribe\",\"topic\": \"/market/level2:BTC-USDT\", \"response\": true }"); };
            socket.Closed += (s, e) => { _logger.LogInformation("Socket closed"); };
            socket.Open();
           
            dataForOrderBook = await getOrderbook.GetOrderbook();
            dataForOrderBook.BestAskBidChanchedEvent += () =>
            {
                bestBidAsk.GetBestBidAsk(dataForOrderBook);
                _logger.LogInformation(bestBidAsk.ToString() + " from changes");
            };

            bestBidAsk.GetBestBidAsk(dataForOrderBook);
            _logger.LogInformation(bestBidAsk.ToString() + " first request");
           
            Task.Run(() =>
            {
                while (true)
                {
                    var hasoOne = changesStreamBuffers.TryDequeue(out var buffer);
                    if (hasoOne)
                    {
                        dataForOrderBook.AcceptChanges(buffer);
                    };
                }
            });
            // make a snapsot of OrderBook every 5sec

            System.Timers.Timer timer = new System.Timers.Timer(5000);

            timer.Elapsed += async (s, e) =>
            {
                _logger.LogInformation("new time cycle...");
                dataForOrderBook = await getOrderbook.GetOrderbook();
                bestBidAsk.GetBestBidAsk(dataForOrderBook);
                _logger.LogInformation(bestBidAsk.ToString() + "  from snapshot");
            };
            timer.Start();


            ViewBag.MarketData = bestBidAsk.ToString();
            return View();
        }

       
    }        
 }

