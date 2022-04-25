using KuCoinApiClient.Config;
<<<<<<< HEAD:KuCoinApiClient/Services/OrderBookService.cs
using KuCoinApiClient.Models;
using System.Threading.Tasks;
=======
using KuCoinApiClient.Model;
>>>>>>> MaxSizeBuffer:KukoinServer/Services/OrderBookService.cs

namespace KuCoinApiClient.Services
{
    public class OrderBookService
    {
        private readonly HttpService _httpService;
<<<<<<< HEAD:KuCoinApiClient/Services/OrderBookService.cs

        public OrderBookService(HttpService httpService)
=======
        private readonly ILogger _logger;

        public OrderBookService(HttpService httpService, ILogger<OrderBookService> logger)
>>>>>>> MaxSizeBuffer:KukoinServer/Services/OrderBookService.cs
        {
            _httpService = httpService;
        }

        internal async Task<OrderBookDto> GetOrderBook(string pairId)
        {
            var response = await _httpService.DoGet<OrderBookResponseModel>(KucoinSettings.BaseUrl + "/api/v1/market/orderbook/level2_20?symbol=" + pairId);
            if (response == null)
            {
<<<<<<< HEAD:KuCoinApiClient/Services/OrderBookService.cs
                return null; //log order book response error
=======
                _logger.LogError("order book response error");
                return null; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/OrderBookService.cs
            }
            return new OrderBookDto(response.sequence, response.bids, response.asks);
        }
    }
}
