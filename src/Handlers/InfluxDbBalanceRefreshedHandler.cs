using CoinGram.Common.Coinigy;
using CoinGram.Handlers.Models;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoinGram.Handlers
{
    public class InfluxDbBalanceRefreshedHandler : INotificationHandler<BalancesRefreshedNotification>
    {
        private readonly ILogger<InfluxDbBalanceRefreshedHandler> _logger;
        private readonly LineProtocolClient _lineProtocolClient;

        public InfluxDbBalanceRefreshedHandler(ILogger<InfluxDbBalanceRefreshedHandler> logger, LineProtocolClient lineProtocolClient)
        {
            _logger = logger;
            _lineProtocolClient = lineProtocolClient;
        }

        public async Task Handle(BalancesRefreshedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("start writing balances to influxdb");

            try
            {
                var payload = new LineProtocolPayload();

                foreach (var balance in notification.Balances)
                {
                    var point = new LineProtocolPoint(
                        "balances",
                        new Dictionary<string, object>
                        {
                            { "available", balance.AmountAvailable },
                            { "held", balance.AmountHeld },
                            { "total", balance.AmountTotal },
                            { "btc", balance.BitcoinAmount },
                            { "last_price", balance.LastPrice }
                        },
                        new Dictionary<string, string>
                        {
                            { "currency", balance.CurrencyCode }
                        },
                        DateTime.UtcNow
                    );

                    payload.Add(point);
                }

                var influxResult = await _lineProtocolClient.WriteAsync(payload);

                if (!influxResult.Success)
                {
                    throw new Exception("unable to write data to influxdb! error: " + influxResult.ErrorMessage);
                }
                else
                {
                    _logger.LogInformation("data successfully written to influxdb");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "unable to write data to influxdb");
            }
        }
    }
}
