namespace KuCoinApiClient.Models
{
    public class HttpResponseModel<T> where T : class
    {
        public string code { get; set; }
        public T data { get; set; }
    }
}
