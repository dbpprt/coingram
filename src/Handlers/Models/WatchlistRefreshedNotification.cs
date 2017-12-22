using CoinGram.Common.Coinigy.Models;
using MediatR;
using System.Collections.Generic;

namespace CoinGram.Handlers.Models
{
    public class WatchlistRefreshedNotification : INotification
    {
        public IEnumerable<WatchedCurrency> Currencies { get; set; }
    }
}
