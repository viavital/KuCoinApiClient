using KuCoinApiClient.Model;
using KuCoinApiClient.Utils;

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
            const int MAX_SIZE = 10000;
            _bids = new FixedSizeQueue<CoinItemMessageModel>(MAX_SIZE);
            _asks = new FixedSizeQueue<CoinItemMessageModel>(MAX_SIZE);
        }

        public void AddToChache(FullMessageModelDataChanges changes)
        {
            var asks = changes.asks;
            foreach(var ask in asks)
            {
                _asks.Enqueue(new CoinItemMessageModel(ask));
            }

            var bids = changes.bids;
            foreach(var bid in bids)
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
