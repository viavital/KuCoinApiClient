using KuCoinApiClient.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

            app.Use(async (context, next) =>
            {
                string MessageFromSocket = "null";
                var tcs = new TaskCompletionSource();
                GetTokenMessageService getTokenMessageServ = new GetTokenMessageService();
                var token = await getTokenMessageServ.GetToken();
                var socket = new WebSocket("wss://ws-api.kucoin.com/endpoint?token=" + token);
                socket.MessageReceived += (s, e) =>
                {
                    MessageFromSocket = e.Message.ToString();
                    logger.LogInformation(MessageFromSocket);
                };
                socket.Opened += (s, e) => { socket.Send("{\"id\":1545910660740, \"type\":\"subscribe\",\"topic\": \"/spotMarket/level2Depth5:BTC-USDT\", \"response\": true }"); };
                socket.Open();
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
