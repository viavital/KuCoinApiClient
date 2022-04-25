<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
﻿using KuCoinApiClient.Models;
using System.Linq;
using System.Threading.Tasks;
=======
﻿using KuCoinApiClient.Model;
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs

namespace KuCoinApiClient.Services
{
    public class KucoinProviderService
    {
        private readonly KucoinMessagingService _messagingService;
        private readonly OrderBookService _orderBookService;
        private readonly MessagesStorage _messagesStorage;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs

        public KucoinProviderService(KucoinMessagingService messagingService, OrderBookService orderBookService, MessagesStorage messagesStorage)
=======
        private readonly ILogger _logger;

        public KucoinProviderService(KucoinMessagingService messagingService, OrderBookService orderBookService, MessagesStorage messagesStorage, ILogger<KucoinProviderService> logger)
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
        {
            _messagingService = messagingService;
            _orderBookService = orderBookService;
            _messagesStorage = messagesStorage;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
=======
            _logger = logger;
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
        }

        internal async Task<StatusDTO> GetInfo(string pairId)
        {
            if (!_messagingService.isConnectedAndReady(pairId))
            {
                var isSucess = await _messagingService.ConnectToSocket(pairId);
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
                if (!isSucess)
                {
                    return null; // log error can not connect
=======
                
                if (!isSucess)
                {
                    _logger.LogError("error can not connect - (_messagingService.ConnectToSocket)");
                    return null; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
                }
            }

            var hasMinData = await _messagesStorage.WaitData();
            if (!hasMinData)
            {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
                return null; //log error lack of data
=======
                _logger.LogError("error lack of data (_messagesStorage.WaitData)");
                return null; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            }

            var messages = _messagesStorage.GetMessages();
            if (messages == null)
            {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
                return null; // log error messages not found
=======
                _logger.LogError("error messages not found (_messagesStorage.GetMessages)");
                return null; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            }

            var orderBook = await _orderBookService.GetOrderBook(pairId);
            if (orderBook == null)
            {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
                return null; // log error order book not found
=======
                _logger.LogError("error order book not found (_orderBookService.GetOrderBook)");
                return null; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            }

            var filteredMessage = DropOldSequencesAndZeroPrices(messages, orderBook.sequence);
            var orderBookUpdated = FilterEmptyFromOrderBookAndUpdatePrices(orderBook, filteredMessage);

<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
            var statusDTO = CreateResultDTO(orderBookUpdated, pairId);
=======
            var statusDTO = CreateResultDTO(orderBook, pairId);
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
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
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
=======
            _logger.LogInformation($"Begin updating OrderBook: Primary OrderBookResult - BestAsk - {orderBook.asks[0].price} size -{orderBook.asks[0].size} ; BestBid - {orderBook.bids[0].price} size -{orderBook.bids[1].size}");
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            var noAmountAsksPrices = filteredMessage.asks.Where(a => a.size == 0).Select(a => a.price);
            var noAmountBidsPrices = filteredMessage.bids.Where(b => b.size == 0).Select(b => b.price);

            var orderAsksNotEmpty = orderBook.asks.Where(a => noAmountAsksPrices.All(no => no != a.price));
            var orderBidsNotEmpty = orderBook.bids.Where(b => noAmountBidsPrices.All(no => no != b.price));

<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
            foreach (var orderAsk in orderAsksNotEmpty)
=======
            foreach(var orderAsk in orderAsksNotEmpty)
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            {
                var updatedPriceAsk = filteredMessage.asks.FirstOrDefault(a => a.price == orderAsk.price);
                if (updatedPriceAsk != null)
                {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
                    orderAsk.price = updatedPriceAsk.price;
                }
            }

            foreach (var orderBid in orderBidsNotEmpty)
=======
                    orderAsk.size = updatedPriceAsk.size;
                }
            }

            foreach(var orderBid in orderBidsNotEmpty)
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            {
                var updatedPriceBid = filteredMessage.bids.FirstOrDefault(a => a.price == orderBid.price);
                if (updatedPriceBid != null)
                {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
                    orderBid.price = updatedPriceBid.price;
=======
                    orderBid.size = updatedPriceBid.size;
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
                }
            }

            return new OrderBookDto(orderBook.sequence, orderBidsNotEmpty.ToArray(), orderAsksNotEmpty.ToArray());
        }

        private StatusDTO CreateResultDTO(OrderBookDto orderBookWithUpdatedPrizes, string pairId)
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
        {
=======
        {            
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            var minAskOrdered = orderBookWithUpdatedPrizes.asks.OrderBy(a => a.price);
            var maxBidOrdered = orderBookWithUpdatedPrizes.bids.OrderByDescending(b => b.price);

            var minAsk = minAskOrdered.FirstOrDefault();
            var maxBid = maxBidOrdered.FirstOrDefault();

            var minAskPrice = minAsk?.price ?? 0;
            var minAskAmount = minAsk?.size ?? 0;

            var maxBidPrice = maxBid?.price ?? 0;
            var maxBidAmount = maxBid?.size ?? 0;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinProviderService.cs
=======
            _logger.LogInformation($"End of updating OrderBook:Updated OrderBookResult - BestAsk - {minAskPrice} - size {minAskAmount}; BestBid - {maxBidPrice} size {maxBidAmount}");
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinProviderService.cs
            return new StatusDTO(minAskPrice, minAskAmount, maxBidPrice, maxBidAmount, pairId);
        }
    }
}
