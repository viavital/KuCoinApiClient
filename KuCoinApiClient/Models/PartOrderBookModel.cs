using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuCoinApiClient.Models
{
    
    public class DataForOrderBook
    {
        public event Action BestAskBidChanchedEvent;
        public object  time { get; set; }
        public ulong sequence { get; set; }
        public float[][] bids { get; set; }
        public float[][] asks { get; set; }

        internal void AcceptChanges(ChangesStreamBuffer changesStreamBuffer)
        {
            if (changesStreamBuffer.data.sequenceStart <= this.sequence)
            {
                return;
            }
            if (changesStreamBuffer.data.changes.asks.Length > 0 && changesStreamBuffer.data.changes.asks[0][0] != 0)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (this.asks[i][0] == changesStreamBuffer.data.changes.asks[0][0])
                    {
                        this.asks[i][1] = changesStreamBuffer.data.changes.asks[0][1];
                        
                    }
                }
                if (this.asks[0][0] == changesStreamBuffer.data.changes.asks[0][0])
                {
                    BestAskBidChanchedEvent?.Invoke();
                }
            }
            if (changesStreamBuffer.data.changes.bids.Length > 0 && changesStreamBuffer.data.changes.bids[0][0] != 0)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (this.bids[i][0] == changesStreamBuffer.data.changes.bids[0][0])
                    {
                        this.bids[i][1] = changesStreamBuffer.data.changes.bids[0][1];
                    }
                }
                if (this.bids[0][0] == changesStreamBuffer.data.changes.bids[0][0])
                {
                    BestAskBidChanchedEvent?.Invoke();
                }
            }
        }
    }
    public class PartOrderBookModel
    {
        public string code { get; set; }
        public DataForOrderBook data { get; set; }
    }

    public class BestBidAsk
    {
      public void GetBestBidAsk(DataForOrderBook dataForOrderBook)
        {
            BestBidPrice = dataForOrderBook.bids[0][0];
            BestBidSize = dataForOrderBook.bids[0][1];
            BestAskPrice = dataForOrderBook.asks[0][0];
            BestAskSize = dataForOrderBook.asks[0][1];
        }
        public float BestBidPrice { get; set; }
        public float BestBidSize { get; set; }
        public float BestAskPrice { get; set; }
        public float BestAskSize { get; set; }

        public override string ToString()
        {
            return "BestBid price is - "+ BestBidPrice + " size is - " + BestBidSize + "\r\n Best Ask price is - " + BestAskPrice + " size is - "+ BestAskSize;
        }
    }
}
