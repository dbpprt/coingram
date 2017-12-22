using CoinGram.Common.Coinigy;
using CoinGram.Handlers.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoinGram.Handlers
{
    public class WatchlistSynchronizationHandler : INotificationHandler<StartWatchlistSynchronizationNotification>
    {
        private readonly ILogger<WatchlistSynchronizationHandler> _logger;
        private readonly CoinigyApiClient _coinigyApiClient;
        private readonly IMediator _mediator;

        public WatchlistSynchronizationHandler(ILogger<WatchlistSynchronizationHandler> logger, CoinigyApiClient coinigyApiClient, IMediator mediator) {
            _logger = logger;
            _coinigyApiClient = coinigyApiClient;
            _mediator = mediator;
        }

        public async Task Handle(StartWatchlistSynchronizationNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("start synchronizing watchlist...");
            
            try
            {
                var currencies = await _coinigyApiClient.GetWatchlistAsync();

                if (currencies == null)
                {
                    throw new ArgumentNullException(nameof(currencies));
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _mediator.Publish(new WatchlistRefreshedNotification
                {
                    Currencies = currencies
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                _logger.LogInformation("watchlist successfully synchronized!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "unable to refresh watchlist");
            }
        }
    }
}
