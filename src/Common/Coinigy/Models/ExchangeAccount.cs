using Newtonsoft.Json;
using System;

namespace CoinGram.Common.Coinigy.Models
{
    public class ExchangeAccount
    {
        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("auth_active")]
        public bool AuthActive { get; set; }

        [JsonProperty("auth_id")]
        public int AuthId { get; set; }

        [JsonProperty("auth_key")]
        public string AuthKey { get; set; }

        [JsonProperty("auth_nickname")]
        public string AuthNickname { get; set; }

        [JsonProperty("auth_optional1")]
        public string AuthOptional { get; set; }

        [JsonProperty("auth_secret")]
        public string AuthSecret { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("auth_trade")]
        public bool AuthTradingEnabled { get; set; }

        [JsonProperty("auth_updated")]
        public DateTime AuthUpdated { get; set; }

        [JsonProperty("exch_id")]
        public int Id { get; set; }

        [JsonProperty("exch_name")]
        public string Name { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("exch_trade_enabled")]
        public bool TradingEnabled { get; set; }
    }
}
