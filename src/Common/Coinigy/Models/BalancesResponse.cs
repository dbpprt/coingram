using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoinGram.Common.Coinigy.Models
{
    public class BalancesResponse
    {
        [JsonProperty("data")]
        public List<Balance> Balances { get; set; }

        [JsonProperty("notifications")]
        public List<object> Notifications { get; set; }
    }
}
