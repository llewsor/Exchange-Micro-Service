using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ExchangeService.Services
{
    public interface IRateProviderService
    {
        Task<decimal> GetRateAsync(string baseCurrency, string targetCurrency);
    }

    public class RateProviderService : IRateProviderService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private const int RateValidityDurationMinutes = 30;
        private readonly ILogger<RateProviderService> _logger;

        public RateProviderService(IMemoryCache cache, IHttpClientFactory httpClientFactory, ILogger<RateProviderService> logger)
        {
            _cache = cache;
            _httpClient = httpClientFactory.CreateClient("ExchangeRatesAPI");
            _logger = logger;
        }

        public async Task<decimal> GetRateAsync(string baseCurrency, string targetCurrency)
        {
            var cacheKey = $"{baseCurrency}-{targetCurrency}";
            if (!_cache.TryGetValue(cacheKey, out decimal rate))
            {
                // Fetch from API and cache
                rate = await FetchRateFromApi(baseCurrency, targetCurrency);
                _cache.Set(cacheKey, rate, TimeSpan.FromMinutes(RateValidityDurationMinutes));
            }

            return rate;
        }

        private class ExchangeRateResponse
        {
            public Dictionary<string, decimal> Rates { get; set; }
        }

        private async Task<decimal> FetchRateFromApi(string baseCurrency, string targetCurrency)
        {
            _logger.LogInformation($"FetchRateFromApi, baseCurrency {baseCurrency}, targetCurrency {targetCurrency}");

            try
            {
                var response = await _httpClient.GetAsync(new Uri($"{_httpClient.BaseAddress}&base={baseCurrency}&symbols={targetCurrency}"));

                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    ExchangeRateResponse exchangeRates = await JsonSerializer.DeserializeAsync<ExchangeRateResponse>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // This makes the deserializer more forgiving with the casing of the properties
                    });

                    return exchangeRates.Rates[$"{targetCurrency}"];
                }
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request exceptions here (e.g., logging)
                throw new Exception("Error fetching exchange rates", e);
            }
            catch (JsonException e)
            {
                // Handle JSON serialization/deserialization errors
                throw new Exception("Error deserializing exchange rates", e);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retriving exchange rates from {baseCurrency} to {targetCurrency}", ex);
            }
        }
    }

}
