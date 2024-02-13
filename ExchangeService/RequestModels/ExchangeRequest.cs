namespace ExchangeService.RequestModels
{
    public class ExchangeRequest
    {
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Amount { get; set; } 
        public string? ClientIp { get; set; }
    }
}
