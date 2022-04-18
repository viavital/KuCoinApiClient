
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
        private readonly KucoinProviderService kucoinProvider;

        public KuCoin(KucoinProviderService kucoinProvider)
        {
            this.kucoinProvider = kucoinProvider;
        }
<<<<<<< Updated upstream

       //[HttpGet("default")]
        public Task<ActionResult<StatusDTO>> GetKucoinDefault()
        {
            return GetKucoin("BTC-USDT");
        }

            socket.MessageReceived +=  (s, e) =>
            {
                MessageFromSocket = e.Message.ToString();
                var chStrBuffer = JsonConvert.DeserializeObject<ChangesStreamBuffer>(MessageFromSocket);
                if (chStrBuffer != null && chStrBuffer.data != null)
                Task.Run(() =>
                {
                   dataForOrderBook.AcceptChanges(chStrBuffer);
                });
            };
            
            socket.Opened += (s, e) => { _logger.LogInformation("socket established"); socket.Send("{\"id\":1545910660740, \"type\":\"subscribe\",\"topic\": \"/market/level2:BTC-USDT\", \"response\": true }"); };
            socket.Closed += (s, e) => { _logger.LogInformation("Socket closed"); };
            socket.Open();
           
            dataForOrderBook = await getOrderbook.GetOrderbook();
            dataForOrderBook.BestAskBidChanchedEvent += () =>
            {
                return NotFound($"Pair={pairId} can not be received!");
            }
=======
>>>>>>> Stashed changes

       //[HttpGet("default")]
        public Task<ActionResult<StatusDTO>> GetKucoinDefault()
        {
            return GetKucoin("BTC-USDT");
        }

        [HttpGet("{pairId}")]
        public async Task<ActionResult<StatusDTO>> GetKucoin(string pairId)
        {
            var result = await kucoinProvider.GetInfo(pairId);

            if (result == null)
            {
                return NotFound($"Pair={pairId} can not be received!");
            }

            return Json(result);
        }

       
    }        
 }

