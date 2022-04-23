namespace KuCoinApiClient.Model
{
    public class StatusDTO
    {
        public double minAskPrice { get; }
        public double askAmount { get; }
        public double maxBidPrice { get;  }
        public double bidAmount { get; }
        public string pair { get; }

        public StatusDTO(double minAsk, double askAmount, double maxBid, double bidAmount, string pair)
        {
            this.minAskPrice = minAsk;
            this.askAmount = askAmount;
            this.maxBidPrice = maxBid;
            this.bidAmount = bidAmount;
            this.pair = pair;
        }
    }
}
