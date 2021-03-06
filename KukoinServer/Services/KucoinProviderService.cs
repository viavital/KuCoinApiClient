using KuCoinApiClient.Model;

namespace KuCoinApiClient.Services
{
    public class KucoinProviderService
    {
        private readonly KucoinMessagingService _messagingService;
        private readonly OrderBookService _orderBookService;
        private readonly MessagesStorage _messagesStorage;
        private readonly ILogger _logger;

        public KucoinProviderService(KucoinMessagingService messagingService, OrderBookService orderBookService, MessagesStorage messagesStorage, ILogger<KucoinProviderService> logger)
        {
            _messagingService = messagingService;
            _orderBookService = orderBookService;
            _messagesStorage = messagesStorage;
            _logger = logger;
        }

        internal async Task<StatusDTO> GetInfo(string pairId)
        {
            if (!_messagingService.isConnectedAndReady(pairId))
            {
                var isSucess = await _messagingService.ConnectToSocket(pairId);
                
                if (!isSucess)
                {
                    _logger.LogError("error can not connect - (_messagingService.ConnectToSocket)");
                    return null; 
                }
            }

            var hasMinData = await _messagesStorage.WaitData();
            if (!hasMinData)
            {
                _logger.LogError("error lack of data (_messagesStorage.WaitData)");
                return null; 
            }

            var messages = _messagesStorage.GetMessages();
            if (messages == null)
            {
                _logger.LogError("error messages not found (_messagesStorage.GetMessages)");
                return null; 
            }

            var orderBook = await _orderBookService.GetOrderBook(pairId);
            if (orderBook == null)
            {
                _logger.LogError("error order book not found (_orderBookService.GetOrderBook)");
                return null; 
            }

            var filteredMessage = DropOldSequencesAndZeroPrices(messages, orderBook.sequence);
            var orderBookUpdated = FilterEmptyFromOrderBookAndUpdatePrices(orderBook, filteredMessage);

            var statusDTO = CreateResultDTO(orderBook, pairId);
            return statusDTO;
        }

        private AsksBidsDTO DropOldSequencesAndZeroPrices(AsksBidsDTO messages, long sequence)
        {
            var actualAsks = messages.asks.Where(a => a.sequence >= sequence && a.price > 0).ToArray();
            var actualBids = messages.bids.Where(b => b.sequence >= sequence && b.price > 0).ToArray();
            return new AsksBidsDTO(actualAsks, actualBids);
        }

        private OrderBookDto FilterEmptyFromOrderBookAndUpdatePrices(OrderBookDto orderBook, AsksBidsDTO filteredMessage)
        {
            _logger.LogInformation($"Begin updating OrderBook: Primary OrderBookResult - BestAsk - {orderBook.asks[0].price} size -{orderBook.asks[0].size} ; BestBid - {orderBook.bids[0].price} size -{orderBook.bids[1].size}");
            var noAmountAsksPrices = filteredMessage.asks.Where(a => a.size == 0).Select(a => a.price);
            var noAmountBidsPrices = filteredMessage.bids.Where(b => b.size == 0).Select(b => b.price);

            var orderAsksNotEmpty = orderBook.asks.Where(a => noAmountAsksPrices.All(no => no != a.price));
            var orderBidsNotEmpty = orderBook.bids.Where(b => noAmountBidsPrices.All(no => no != b.price));

            foreach(var orderAsk in orderAsksNotEmpty)
            {
                var updatedPriceAsk = filteredMessage.asks.FirstOrDefault(a => a.price == orderAsk.price);
                if (updatedPriceAsk != null)
                {
                    orderAsk.size = updatedPriceAsk.size;
                }
            }

            foreach(var orderBid in orderBidsNotEmpty)
            {
                var updatedPriceBid = filteredMessage.bids.FirstOrDefault(a => a.price == orderBid.price);
                if (updatedPriceBid != null)
                {
                    orderBid.size = updatedPriceBid.size;
                }
            }

            return new OrderBookDto(orderBook.sequence, orderBidsNotEmpty.ToArray(), orderAsksNotEmpty.ToArray());
        }

        private StatusDTO CreateResultDTO(OrderBookDto orderBookWithUpdatedPrizes, string pairId)
        {            
            var minAskOrdered = orderBookWithUpdatedPrizes.asks.OrderBy(a => a.price);
            var maxBidOrdered = orderBookWithUpdatedPrizes.bids.OrderByDescending(b => b.price);

            var minAsk = minAskOrdered.FirstOrDefault();
            var maxBid = maxBidOrdered.FirstOrDefault();

            var minAskPrice = minAsk?.price ?? 0;
            var minAskAmount = minAsk?.size ?? 0;

            var maxBidPrice = maxBid?.price ?? 0;
            var maxBidAmount = maxBid?.size ?? 0;
            _logger.LogInformation($"End of updating OrderBook:Updated OrderBookResult - BestAsk - {minAskPrice} - size {minAskAmount}; BestBid - {maxBidPrice} size {maxBidAmount}");
            return new StatusDTO(minAskPrice, minAskAmount, maxBidPrice, maxBidAmount, pairId);
        }
    }
}
