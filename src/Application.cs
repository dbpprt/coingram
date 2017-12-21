using CoinGram.Common.Coinigy;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoinGram
{
    class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly CoinigyApiClient _coinigyApiClient;

        public Application(ILogger<Application> logger, CoinigyApiClient coinigyApiClient)
        {
            _logger = logger;
            _coinigyApiClient = coinigyApiClient;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("initializing application...");

            _logger.LogInformation("testing coinigy api connection...");
            var balances = await _coinigyApiClient.GetBalancesAsync();

        }
    }
}
