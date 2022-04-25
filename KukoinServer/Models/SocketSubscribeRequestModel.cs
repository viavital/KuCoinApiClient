namespace KuCoinApiClient.Models
{
    public class SocketSubscribeRequestModel : SimpleMessageModel
    {
        public string topic { get; set; }
        public bool privateChannel { get; set; }
        public bool response { get; set; }
    }
}
