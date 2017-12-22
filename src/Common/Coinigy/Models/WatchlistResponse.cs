using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoinGram.Common.Coinigy.Models
{
    public class WatchlistResponse
    {
        [JsonProperty("data")]
        public List<WatchedCurrency> Currencies { get; set; }

        [JsonProperty("notifications")]
        public List<object> Notifications { get; set; }
    }
}
