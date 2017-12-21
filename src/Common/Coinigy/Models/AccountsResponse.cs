﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoinGram.Common.Coinigy.Models
{
    class AccountsResponse
    {
        [JsonProperty("data")]
        public List<ExchangeAccount> ExchangeAccounts;

        [JsonProperty("notifications")]
        public List<object> Notifications;
    }
}