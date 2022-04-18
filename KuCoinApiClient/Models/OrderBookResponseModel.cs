namespace KuCoinApiClient.Models
{
    public class OrderBookResponseModel
    {
        public string sequence { get; set; }
        public long time { get; set; }
        public string[][] bids { get; set; }
        public string[][] asks { get; set; }
    }
}
