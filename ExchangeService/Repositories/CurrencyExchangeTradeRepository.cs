using ExchangeService.Entity;
using ExchangeService.Entity.Models;

namespace ExchangeService.Repositories
{
    public class CurrencyExchangeTradeRepository : BaseRepository<CurrencyExchangeTrade>
    {
        public CurrencyExchangeTradeRepository(ExchangeDbContext context) : base(context) { }
    }
}
