namespace KuCoinApiClient.Models
{
    public class OrderBookDto
    {
        public long sequence { get; }
        public CoinItemOrderModel[] bids { get; }
        public CoinItemOrderModel[] asks { get; }

        public OrderBookDto(string sequence, string[][] bidsModel, string[][] asksModel)
        {
            this.sequence = long.TryParse(sequence, out var seq) ? seq : 0;

            bids = new CoinItemOrderModel[bidsModel.Length];
            for (var i = 0; i < bidsModel.Length; i++)
            {
                bids[i] = new CoinItemOrderModel(bidsModel[i]);
            }

            asks = new CoinItemOrderModel[asksModel.Length];
            for (var i = 0; i < asksModel.Length; i++)
            {
                asks[i] = new CoinItemOrderModel(asksModel[i]);
            }
        }

        public OrderBookDto(long sequence, CoinItemOrderModel[] bids, CoinItemOrderModel[] asks)
        {
            this.sequence = sequence;
            this.bids = bids;
            this.asks = asks;
        }
    }
}
