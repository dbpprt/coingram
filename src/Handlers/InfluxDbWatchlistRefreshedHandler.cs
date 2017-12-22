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
    public class InfluxDbWatchlistRefreshedHandler : INotificationHandler<WatchlistRefreshedNotification>
    {
        private readonly ILogger<InfluxDbWatchlistRefreshedHandler> _logger;
        private readonly LineProtocolClient _lineProtocolClient;

        public InfluxDbWatchlistRefreshedHandler(ILogger<InfluxDbWatchlistRefreshedHandler> logger, LineProtocolClient lineProtocolClient)
        {
            _logger = logger;
            _lineProtocolClient = lineProtocolClient;
        }

        public async Task Handle(WatchlistRefreshedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("start writing watchlist to influxdb");

            try
            {
                var payload = new LineProtocolPayload();

                foreach (var currency in notification.Currencies)
                {
                    var point = new LineProtocolPoint(
                        "watchlist",
                        new Dictionary<string, object>
                        {
                            { "btc_volume", currency.BitcoinVolume },
                            //{ "high_trade", currency.HighTrade },
                            { "last_price", currency.LastPrice },
                            //{ "low_trade", currency.LowTrade },
                            //{ "previous_price", currency.PreviousPrice },
                            { "volume", currency.Volume },
                            { "server_time", currency.ServerTime.Ticks },
                            { "market", currency.MarketName },
                        },
                        new Dictionary<string, string>
                        {
                            //{ "primary_currency", currency.PrimaryCurrencyName },
                            //{ "secondary_currency", currency.SecondaryCurrencyName },
                            { "market_name", currency.MarketName },
                            { "exchange_code", currency.ExchangeCode }
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
