using Newtonsoft.Json;

namespace CoinGram.Common.Coinigy.Models
{
    class Balance
    {
        [JsonProperty("balance_amount_avail")]
        public decimal AmountAvailable { get; set; }

        [JsonProperty("balance_amount_held")]
        public decimal AmountHeld { get; set; }

        [JsonProperty("balance_amount_total")]
        public decimal AmountTotal { get; set; }

        [JsonProperty("balance_curr_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("btc_balance")]
        public string BitcoinAmount { get; set; }

        [JsonProperty("last_price")]
        public decimal LastPrice { get; set; }
    }
}
