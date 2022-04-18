namespace KuCoinApiClient.Models
{
    public class CoinItemMessageModel : CoinItemOrderModel
    {
        public long sequence { get; }

        public CoinItemMessageModel(string[] item) : base(item)
        {
            sequence = long.TryParse(item[2], out var seq) ? seq : 0;
        }
    }
}
