
using Microsoft.AspNetCore.Mvc;
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
       
        //private async Task<string> GetMarketData()
        //{            
        //    HttpClient client = new HttpClient();
        //    var content = new System.Net.Http.StringContent("");           
        //    HttpResponseMessage response = await client.PostAsync("https://api.kucoin.com/api/v1/bullet-public", content); // кидає запит
        //    response.EnsureSuccessStatusCode();
        //    string responseBody = await response.Content.ReadAsStringAsync(); // виводить результат запиту на екран
        //    return responseBody;
        // }
        

        //private void Socket_OnOpen(object sender, EventArgs e)
        //{
        //    MarketPin = e.data;
        //}
       
        public async Task <IActionResult> Market()
        {
            string MarketMessage = "null";
            async Task GetMarketDataBySocket()
            {                
                using ( WebSocket socket = new WebSocket("wss://ws-api.kucoin.com/endpoint?token=2neAiuYvAU61ZDXANAGAsiL4-iAExhsBXZxftpOeh_55i3Ysy2q2LEsEWU64mdzUOPusi34M_wGoSf7iNyEWJ1D4WZWJkGLNVr0g_mKH9HycnGyzdXGX_9iYB9J6i9GjsxUuhPw3BlrzazF6ghq4L4yQJK_UCJ7uj2wIdHSXKEM=.Vl24Izb0L16hBG1zYIO_Ug=="))
                {
                    // socket.OnOpen += Socket_OnOpen;
                    socket.OnMessage += (s, e) => { MarketMessage = e.Data.ToString(); };
                    socket.Connect();                   
                }               
            }
            await GetMarketDataBySocket();

            ViewBag.MarketData =  MarketMessage;

            return View();
        }
    }        
    }

