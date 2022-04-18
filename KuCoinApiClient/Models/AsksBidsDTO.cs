namespace KuCoinApiClient.Models
{
    public class AsksBidsDTO
    {
        public CoinItemMessageModel[] asks { get; }
        public CoinItemMessageModel[] bids { get; }

        public AsksBidsDTO(CoinItemMessageModel[] asks, CoinItemMessageModel[] bids)
        {
            this.asks = asks;
            this.bids = bids;
        }
    }
}
