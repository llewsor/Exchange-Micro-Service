using ExchangeService.Entity;
using ExchangeService.Repositories.Interfaces;

namespace ExchangeService.Repositories
{
    public class RepositoryWrapper(ExchangeDbContext context) : IRepositoryWrapper
    {
        private ExchangeDbContext _context = context;
        private CurrencyExchangeTradeRepository _tradeRepository;

        public CurrencyExchangeTradeRepository Trade {
            get
            {
                if (_tradeRepository == null)
                    _tradeRepository = new CurrencyExchangeTradeRepository(_context);

                return _tradeRepository;
            } 
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
