using System.Globalization;

namespace KuCoinApiClient.Model
{
    public class CoinItemOrderModel
    {
        public double price { get; set; }
        public double size { get; set; }
        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        public CoinItemOrderModel(string[] item)
        {
            price = double.TryParse(item[0], System.Globalization.NumberStyles.AllowDecimalPoint, formatter, out var pr) ? pr : 0;
            size = double.TryParse(item[1], System.Globalization.NumberStyles.AllowDecimalPoint, formatter, out var sz) ? sz : 0;

        }
    }
}
