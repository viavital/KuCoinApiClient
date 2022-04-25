<<<<<<< HEAD:KuCoinApiClient/MessagesStorage.cs
﻿using KuCoinApiClient.Models;
using KuCoinApiClient.Utils;
using System.Threading.Tasks;
=======
﻿using KuCoinApiClient.Model;
using KuCoinApiClient.Utils;
>>>>>>> MaxSizeBuffer:KukoinServer/MessagesStorage.cs

namespace KuCoinApiClient
{
    public class MessagesStorage
    {
        //todo maybe better to use one collection, and add type (bid, ask) to model ???
        private readonly FixedSizeQueue<CoinItemMessageModel> _bids;
        private readonly FixedSizeQueue<CoinItemMessageModel> _asks;

        private readonly TaskCompletionSource<bool> _waitMinimumData = new TaskCompletionSource<bool>();

        public MessagesStorage()
        {
<<<<<<< HEAD:KuCoinApiClient/MessagesStorage.cs
            const int MAX_SIZE = 10000;
=======
            const int MAX_SIZE = 1000;
>>>>>>> MaxSizeBuffer:KukoinServer/MessagesStorage.cs
            _bids = new FixedSizeQueue<CoinItemMessageModel>(MAX_SIZE);
            _asks = new FixedSizeQueue<CoinItemMessageModel>(MAX_SIZE);
        }

        public void AddToChache(FullMessageModelDataChanges changes)
        {
            var asks = changes.asks;
<<<<<<< HEAD:KuCoinApiClient/MessagesStorage.cs
            foreach (var ask in asks)
=======
            foreach(var ask in asks)
>>>>>>> MaxSizeBuffer:KukoinServer/MessagesStorage.cs
            {
                _asks.Enqueue(new CoinItemMessageModel(ask));
            }

            var bids = changes.bids;
<<<<<<< HEAD:KuCoinApiClient/MessagesStorage.cs
            foreach (var bid in bids)
=======
            foreach(var bid in bids)
>>>>>>> MaxSizeBuffer:KukoinServer/MessagesStorage.cs
            {
                _bids.Enqueue(new CoinItemMessageModel(bid));
            }

            const int MIN_DATA_AMOUNT = 10;  //minimum data set to 10
            if (_asks.Count >= MIN_DATA_AMOUNT && _bids.Count >= MIN_DATA_AMOUNT && !_waitMinimumData.Task.IsCompleted)
            {
                _waitMinimumData.TrySetResult(true);
            }
        }

        public AsksBidsDTO GetMessages()
        {
            var dto = new AsksBidsDTO(_asks.ToArray(), _bids.ToArray());
            return dto;
        }

        public Task<bool> WaitData()
        {
            return _waitMinimumData.Task;
        }
    }
}
