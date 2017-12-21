using CoinGram.Common.Coinigy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoinGram.Common.Coinigy
{
    class CoinigyApiClient
    {
        private readonly string _serverBaseUrl;
        private readonly string _userAgent;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public CoinigyApiClient(string apiKey, string apiSecret, string serverBaseUrl = "https://api.coinigy.com/api/v1/",
            string userAgent =
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36")
        {
            _userAgent = userAgent;
            _serverBaseUrl = serverBaseUrl;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        private async Task<T> HttpPostRequestAsync<T>(string url, List<KeyValuePair<string, string>> data)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            client.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            client.DefaultRequestHeaders.Add("X-API-SECRET", _apiSecret);

            var content = new FormUrlEncodedContent(data);

            var response = await client.PostAsync(_serverBaseUrl + url, content);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())
                : throw new Exception("Api request failed with reason " + response.ReasonPhrase);
        }

        public async Task<IEnumerable<Balance>> GetBalancesAsync(bool showNullBalances = false, string authenticationIds = "")
        {
            return (await HttpPostRequestAsync<BalancesResponse>("balances", new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("show_nils", Convert.ToInt32(showNullBalances).ToString()),
                new KeyValuePair<string, string>("auth_ids", authenticationIds)
            })).Balances;
        }

        public async Task<IEnumerable<ExchangeAccount>> GetExchangeAccountsAsync()
        {
            return (await HttpPostRequestAsync<AccountsResponse>("accounts", new List<KeyValuePair<string, string>>())).ExchangeAccounts;
        }
    }
}
