using CoinGram.Common.Coinigy.Models;
using MediatR;
using System.Collections.Generic;

namespace CoinGram.Handlers.Models
{
    public class BalancesRefreshedNotification : INotification
    {
        public IEnumerable<Balance> Balances { get; set; }
    }
}
