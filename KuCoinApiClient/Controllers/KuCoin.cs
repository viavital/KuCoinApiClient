
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

namespace KuCoinApiClient.Controllers
{
    public class KuCoin : Controller
    {
    public async Task<IActionResult> Market()
        {
            //PartOrderBookModel MarketOrderbook = new PartOrderBookModel();
            //GetOrderbooktService getOrderbook = new GetOrderbooktService("BTC-USDT");
            //MarketOrderbook = await getOrderbook.GetOrderbook();
            //BestBidAsk bestBidAsk = new BestBidAsk(MarketOrderbook);

            //var message = "Best Bid price - " + bestBidAsk.BestBidPrice + "Best Bid size - " + bestBidAsk.BestBidSize +
            //              "Best Ask price - " + bestBidAsk.BestAskPrice + "Best Bid price - " + bestBidAsk.BestAskSize;

            //ViewBag.MarketData = message;            
            return View();
        }
    }        
 }

