using CoinGram.Common.Coinigy;
using CoinGram.Handlers.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoinGram.Handlers
{
    public class BalanceSynchronizationHandler : INotificationHandler<StartBalanceSynchronizationNotification>
    {
        private readonly ILogger<BalanceSynchronizationHandler> _logger;
        private readonly CoinigyApiClient _coinigyApiClient;
        private readonly IMediator _mediator;

        public BalanceSynchronizationHandler(ILogger<BalanceSynchronizationHandler> logger, CoinigyApiClient coinigyApiClient, IMediator mediator) {
            _logger = logger;
            _coinigyApiClient = coinigyApiClient;
            _mediator = mediator;
        }

        public async Task Handle(StartBalanceSynchronizationNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("start synchronizing balances...");
            
            try
            {
                var balances = await _coinigyApiClient.GetBalancesAsync();

                if (balances == null)
                {
                    throw new ArgumentNullException(nameof(balances));
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _mediator.Publish(new BalancesRefreshedNotification
                {
                    Balances = balances
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                _logger.LogInformation("balances successfully synchronized!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "unable to refresh balances");
            }
        }
    }
}
