namespace ExchangeService.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        CurrencyExchangeTradeRepository Trade { get; }
        Task Save();
    }
}