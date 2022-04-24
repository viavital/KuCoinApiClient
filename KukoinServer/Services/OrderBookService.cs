using KuCoinApiClient.Config;
using KuCoinApiClient.Model;

namespace KuCoinApiClient.Services
{
    public class OrderBookService
    {
        private readonly HttpService _httpService;
        private readonly ILogger _logger;

        public OrderBookService(HttpService httpService, ILogger<OrderBookService> logger)
        {
            _httpService = httpService;
        }

        internal async Task<OrderBookDto> GetOrderBook(string pairId)
        {
            var response = await _httpService.DoGet<OrderBookResponseModel>(KucoinSettings.BaseUrl + "/api/v1/market/orderbook/level2_20?symbol=" + pairId);
            if (response == null)
            {
                _logger.LogError("order book response error");
                return null; 
            }
            return new OrderBookDto(response.sequence, response.bids, response.asks);
        }
    }
}
