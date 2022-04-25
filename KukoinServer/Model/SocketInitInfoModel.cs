namespace KuCoinApiClient.Model
{
    public class SocketInitInfoModel
    {
        public string token { get; set; }
        public SocketInitInfoServerModel[] instanceServers { get; set; }

    }

    public class SocketInitInfoServerModel
    {
        public string endpoint { get; set; }
        public string protocol { get; set; }
        public bool encrypt { get; set; }
        public int pingInterval { get; set; } 
        public int pingTimeout { get; set; } 
    }
}
