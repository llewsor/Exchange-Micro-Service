using Moq;
using System.Net;
using Xunit;
using ExchangeService.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests
{
    public class RateProviderServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly ILogger<RateProviderService> _mockLogger;

        public RateProviderServiceTests()
        {
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = NullLogger<RateProviderService>.Instance;
        }

        private class DelegatingHandlerStub : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"rates\":{\"EUR\":1.2}}")
                };

                return await Task.FromResult(response);
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Fact]
        public async Task GetRateAsync_ReturnsExpectedRate()
        {
            // Arrange
            var cacheKey = "USD-EUR";
            var expectedRate = 1.2m;
            var cacheEntry = Mock.Of<ICacheEntry>();

            _mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntry);
            _mockMemoryCache.Setup(m => m.TryGetValue(cacheKey, out expectedRate)).Returns(true);

            var httpClient = new HttpClient(new DelegatingHandlerStub());
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new RateProviderService(_mockMemoryCache.Object, _mockHttpClientFactory.Object, _mockLogger);

            // Act
            var rate = await service.GetRateAsync("USD", "EUR");

            // Assert
            Assert.Equals(expectedRate, rate);
        }
    }
}
