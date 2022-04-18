using KuCoinApiClient.Config;
using KuCoinApiClient.Models;
using System.Threading.Tasks;

namespace KuCoinApiClient.Services
{
    public class OrderBookService
    {
        private readonly HttpService _httpService;

        public OrderBookService(HttpService httpService)
        {
            _httpService = httpService;
        }

        internal async Task<OrderBookDto> GetOrderBook(string pairId)
        {
            var response = await _httpService.DoGet<OrderBookResponseModel>(KucoinSettings.BaseUrl + "/api/v1/market/orderbook/level2_20?symbol=" + pairId);
            if (response == null)
            {
                return null; //log order book response error
            }
            return new OrderBookDto(response.sequence, response.bids, response.asks);
        }
    }
}
