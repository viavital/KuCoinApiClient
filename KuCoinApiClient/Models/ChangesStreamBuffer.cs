using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuCoinApiClient.Models
{
    
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
    public class ChangesStreamBuffer
    {
      public  DataOfChangesSteamBuffer data { get; set; }
    }
}
