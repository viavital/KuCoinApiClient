using KuCoinApiClient.Models;
using KuCoinApiClient.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocket4Net;

namespace KuCoinApiClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            DataForOrderBook dataForOrderBook = new DataForOrderBook();
            List<ChangesSteamBuffer> changesSteamBuffers = new List<ChangesSteamBuffer>();

            app.Use(async (context, next) =>
            {                                
                string MessageFromSocket = "null";
                GetTokenMessageService getTokenMessageServ = new GetTokenMessageService();
                var token = await getTokenMessageServ.GetToken();
                var socket = new WebSocket("wss://ws-api.kucoin.com/endpoint?token=" + token);
                socket.MessageReceived += (s, e) =>
                {
                    MessageFromSocket = e.Message.ToString();
                    logger.LogInformation(MessageFromSocket);
                    changesSteamBuffers.Add(JsonConvert.DeserializeObject<ChangesSteamBuffer>(MessageFromSocket));                    
                };
                socket.Opened += (s, e) => { socket.Send("{\"id\":1545910660740, \"type\":\"subscribe\",\"topic\": \"/market/level2:BTC-USDT\", \"response\": true }"); };
                socket.Open();
                //доч≥куЇмось доки отримаЇмо перш≥ 2 пов≥домленн€
                await Task.Run(()=> {
                    while (changesSteamBuffers.Count < 2)
                    {
                    }
                } );
                // переходимо до наступного м≥длвера
                await next.Invoke();
            });

            app.Use(async (context, next) =>
            {                   
                DataForOrderBook dataForOrderBook = new DataForOrderBook(); 
                GetOrderbooktService getOrderbook = new GetOrderbooktService("BTC-USDT");
                dataForOrderBook = await getOrderbook.GetOrderbook();    // робимо снепшот д≥ючого стакана ц≥н            
                changesSteamBuffers.RemoveAll(u => u.data == null); // видал€Їмо перш≥ 2 серв≥сн≥ пов≥домленн€ €к≥ неможливо привести до ChangesSteamBuffer
                changesSteamBuffers.RemoveAll(u => u.data.sequenceStart <= dataForOrderBook.sequence); // видал€Їмо вс≥ пов≥домленн€ стар≥ш≥ в≥д нашого снепшоту
                Task.Run(() =>
                {
                    while (true)  // безперервно перемаслаЇмо в паралельному потоц≥ кожне отримане пов≥домленн€ (прим≥н€Їмо зм≥ни до снепшоту)
                    {
                        if (changesSteamBuffers != null && changesSteamBuffers.Count > 0)
                        {
                            dataForOrderBook.AcceptChanges(changesSteamBuffers[0]); // чомусь викидуЇ Ќал–еференс≈ксепшн 
                            changesSteamBuffers.RemoveAt(0);
                        }
                    }
                });
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=KuCoin}/{action=Market}/{id?}");
            });
        }
    }
}
