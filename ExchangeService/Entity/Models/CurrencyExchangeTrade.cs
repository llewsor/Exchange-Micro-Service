using System.ComponentModel.DataAnnotations;

namespace ExchangeService.Entity.Models
{
    public class CurrencyExchangeTrade
    {
        public CurrencyExchangeTrade(string clientId, string baseCurrency, decimal baseAmount, string targetCurrency, decimal rate, decimal targetAmount, DateTime tradeTime)
        {
            ClientId = clientId;
            BaseCurrency = baseCurrency;
            BaseAmount = baseAmount;
            TargetCurrency = targetCurrency;
            Rate = rate;
            TargetAmount = targetAmount;
            TradeTime = tradeTime;
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ClientId { get; set; }
        public string BaseCurrency { get; set; }
        public decimal BaseAmount { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TradeTime { get; set; }
    }
}
