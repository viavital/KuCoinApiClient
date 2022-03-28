
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
    public async Task<IActionResult> Market()
        { 
            var message = "Look on Console!!!";   
            ViewBag.MarketData = message;            
            return View();
        }
    }        
 }

