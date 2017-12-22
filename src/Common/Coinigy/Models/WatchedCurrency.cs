using Newtonsoft.Json;
using System;

namespace CoinGram.Common.Coinigy.Models
{
    public class WatchedCurrency
    {
        [JsonProperty("btc_volume")]
        public decimal BitcoinVolume { get; set; }

        [JsonProperty("current_volume")]
        public decimal Volume { get; set; }

        [JsonProperty("exch_code")]
        public string ExchangeCode { get; set; }

        [JsonProperty("exch_name")]
        public string ExchangeName { get; set; }

        [JsonProperty("exchmkt_id")]
        public int ExchangeId { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("fiat_market")]
        public bool FiatMarket { get; set; }

        [JsonProperty("high_trade")]
        public decimal HighTrade { get; set; }

        [JsonProperty("last_price")]
        public decimal LastPrice { get; set; }

        [JsonProperty("low_trade")]
        public decimal LowTrade { get; set; }

        [JsonProperty("mkt_name")]
        public string MarketName { get; set; }

        [JsonProperty("prev_price")]
        public decimal PreviousPrice { get; set; }

        [JsonProperty("primary_currency_name")]
        public string PrimaryCurrencyName { get; set; }

        [JsonProperty("secondary_currency_name")]
        public string SecondaryCurrencyName { get; set; }

        [JsonProperty("server_time")]
        public DateTime ServerTime { get; set; }
    }
}
