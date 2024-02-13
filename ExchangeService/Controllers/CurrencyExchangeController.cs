using ExchangeService.Repositories.Interfaces;
using ExchangeService.RequestModels;
using ExchangeService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly IRateProviderService _rateProvider;
        private readonly IRepositoryWrapper _repo;
        private readonly ILogger<CurrencyExchangeController> _logger;

        public CurrencyExchangeController(IRateProviderService rateProvider, IRepositoryWrapper repo, ILogger<CurrencyExchangeController> logger)
        {
            _rateProvider = rateProvider;
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("exchange")]
        [UppercaseCurrency]
        public async Task<IActionResult> Exchange([FromBody] ExchangeRequest exchangeRequest)
        {
            _logger.LogInformation("Request of currency exchange: {@Request}", exchangeRequest);

            if (string.IsNullOrEmpty(exchangeRequest.BaseCurrency) ||
                string.IsNullOrEmpty(exchangeRequest.TargetCurrency) ||
                string.IsNullOrEmpty(exchangeRequest.ClientIp))
            {
                _logger.LogWarning("Bad Request: {@Request}", exchangeRequest);
                return BadRequest();
            }

            if (_repo.Trade.FindByCondition(x => x.ClientId == exchangeRequest.ClientIp && x.TradeTime > DateTime.Now.AddHours(-1)).Count() > 9) 
            {
                _logger.LogWarning($"Client ip {exchangeRequest.ClientIp}, has reached request limit of 10 calls per hour.");
                return new StatusCodeResult(429);
            }

            try
            {
                decimal rate = await _rateProvider.GetRateAsync(exchangeRequest.BaseCurrency, exchangeRequest.TargetCurrency);
                decimal exchange = exchangeRequest.Amount * rate;

                _repo.Trade.Add(
                    new Entity.Models.CurrencyExchangeTrade(
                        exchangeRequest.ClientIp, exchangeRequest.BaseCurrency, exchangeRequest.Amount, exchangeRequest.TargetCurrency, rate, exchange, DateTime.Now));

                await _repo.Save();

                return Ok(exchange);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while processing currency exchange: {@Request}, {@Exception}", exchangeRequest, exception);
                return NotFound();
            }
        }
    }
}
