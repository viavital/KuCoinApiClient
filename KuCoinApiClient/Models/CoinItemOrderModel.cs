namespace KuCoinApiClient.Models
{
    public class CoinItemOrderModel
    {
        public double price { get; set; }
        public double size { get; }

        public CoinItemOrderModel(string[] item)
        {
            price = double.TryParse(item[0], out var pr) ? pr : 0;
            size = double.TryParse(item[1], out var sz) ? sz : 0;
        }
    }
}
