using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuCoinApiClient.Models
{
    //public struct Changes
    //{
    //    public float price { get; set; }
    //    public float size { get; set; }
    //    public ulong sequence { get; set; }
    //}
    public class ChangesOfBidsAsks
    {
        public float[][] asks { get; set; }
        public float[][] bids { get; set; }
    }
    public class DataOfChangesSteamBuffer
    {
        public ulong sequenceStart { get; set; }
        public ChangesOfBidsAsks changes { get; set; }
        
    }
    public class ChangesSteamBuffer
    {
      public  DataOfChangesSteamBuffer data { get; set; }
    }
}
