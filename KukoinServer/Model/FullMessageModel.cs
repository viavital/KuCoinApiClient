namespace KuCoinApiClient.Model
{
    public class FullMessageModel
    {
        public FullMessageModelData data { get; set; }
        public string subject { get; set; }
        public string topic { get; set; }
        public string type { get; set; }
    }

    public class FullMessageModelData
    {
        public long sequenceStart { get; set; }
        public long sequenceEnd { get; set; }
        public string symbol { get; set; }
        public FullMessageModelDataChanges changes { get; set; }
    }

    public class FullMessageModelDataChanges
    {
        public string[][] asks { get; set; }
        public string[][] bids { get; set; }
    }
}
