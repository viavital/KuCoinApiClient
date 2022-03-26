using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuCoinApiClient.Services
{
    public class Data
    {
       public string token { get; set; }
       object instanceServers { get; set; }

    }
    public class GetTokenMessage
    {
        public string code { get; set; }
        public Data data { get; set; }
    }
}
