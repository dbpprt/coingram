using CoinGram.Common.Coinigy;
using CoinGram.Handlers;
using CoinGram.Handlers.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CoinGram
{
    class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly CoinigyApiClient _coinigyApiClient;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IMediator _mediator;

        public Application(ILogger<Application> logger, CoinigyApiClient coinigyApiClient, ITelegramBotClient telegramBotClient, IMediator mediator)
        {
            _logger = logger;
            _coinigyApiClient = coinigyApiClient;
            _telegramBotClient = telegramBotClient;
            _mediator = mediator;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("initializing application...");

            _logger.LogInformation("testing coinigy api connection...");
            try
            {
                var accounts = await _coinigyApiClient.GetExchangeAccountsAsync();

                foreach (var account in accounts)
                {
                    _logger.LogInformation($"found coinigy exchange account {account.Name} with id {account.Id}");
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "unable to connect to coinigy, please verify your settings!");
                throw;
            }

            _logger.LogInformation("testing telegram connection...");
            try
            {
                var me = await _telegramBotClient.GetMeAsync();
                _logger.LogInformation($"telegram connection successful. hello from {me.Username}!");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "unable to connect to telegram, please verify your settings!");
                throw;
            }
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("entering main loop...");

            while (!cancellationToken.IsCancellationRequested)
            {
                switch (DateTime.Now.Second)
                {
                    case 0:
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        _mediator.Publish(new StartBalanceSynchronizationNotification());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        break;
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
