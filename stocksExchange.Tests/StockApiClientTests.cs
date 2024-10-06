using System.Net;
using System.Text.Json;

namespace stocksExchange.Tests
{
    public class StockApiClientTests
    {
        [Fact]
        public async Task GetStockDataAsync_ReturnsStockData()
        {
            // Arrange
            string stockSymbol = "VALE3";
            string apiToken = Environment.GetEnvironmentVariable("STOCK_API_TOKEN", EnvironmentVariableTarget.User);

            string expectedResponse = "{\"results\":[{\"currency\":\"BRL\",\"longName\":\"Vale S.A.\"}]}";
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.Content = new StringContent(expectedResponse);
            HttpClient httpClient = new HttpClient(new TestHttpMessageHandler(responseMessage));

            // Act
            string actualResponse = await StockApiClient.GetStockDataAsync(stockSymbol, apiToken);

            // Assert
            using (JsonDocument expectedJson = JsonDocument.Parse(expectedResponse))
            using (JsonDocument actualJson = JsonDocument.Parse(actualResponse))
            {
                JsonElement expectedRoot = expectedJson.RootElement;
                JsonElement actualRoot = actualJson.RootElement;

                Assert.Equal(expectedRoot.GetProperty("results")[0].GetProperty("currency").GetString(), actualRoot.GetProperty("results")[0].GetProperty("currency").GetString());
                Assert.Equal(expectedRoot.GetProperty("results")[0].GetProperty("longName").GetString(), actualRoot.GetProperty("results")[0].GetProperty("longName").GetString());
            }
        }

        [Fact]
        public async Task GetStockDataAsync_ReturnsNull_WhenTokenIsInvalid()
        {
            // Arrange
            string stockSymbol = "VALE3";
            string apiToken = "Invalid Token";

            // Act
            string actualResponse = await StockApiClient.GetStockDataAsync(stockSymbol, apiToken);

            // Assert
            Assert.Null(actualResponse);

        }

        [Fact]
        public async Task GetStockDataAsync_ReturnsNull_WhenStockSymbolIsInvalid()
        {
            // Arrange
            string stockSymbol = "G3X";
            string apiToken = Environment.GetEnvironmentVariable("STOCK_API_TOKEN", EnvironmentVariableTarget.User);

            // Act
            string actualResponse = await StockApiClient.GetStockDataAsync(stockSymbol, apiToken);

            // Assert
            Assert.Null(actualResponse);

        }

        [Fact]
        public void FilterStockData_ReturnsFilteredStockData()
        {
            // Arrange
            string rawStockData = "{\"results\":[{\"currency\":\"BRL\",\"longName\":\"Vale S.A.\",\"symbol\":\"VALE3\",\"regularMarketPrice\":150.25}]}";
            FilteredStockData expectedStockData = new FilteredStockData
            {
                Currency = "BRL",
                LongName = "Vale S.A.",
                Symbol = "VALE3",
                StockPrice = 150.25f
            };

            // Act
            FilteredStockData actualStockData = StockApiClient.FilterStockData(rawStockData);

            // Assert
            Assert.Equal(expectedStockData.Currency, actualStockData.Currency);
            Assert.Equal(expectedStockData.LongName, actualStockData.LongName);
            Assert.Equal(expectedStockData.Symbol, actualStockData.Symbol);
            Assert.Equal(expectedStockData.StockPrice, actualStockData.StockPrice);
        }

        [Fact]
        public void FilterStockData_ReturnsNull_WhenRawStockDataIsNull()
        {
            // Arrange
            string rawStockData = null;
            var expectedStockData = StockApiClient.FilterStockData(rawStockData);

            // Act and Assert
            Assert.Null(expectedStockData);
        }

        [Fact]
        public void FilterStockData_ReturnsNull_WhenCannotFilterStockData()
        {
            // Arrange
            string rawStockData = "{\"results\":[]}";
            var expectedStockData = StockApiClient.FilterStockData(rawStockData);

            // Act and Assert
            Assert.Null(expectedStockData);
        }

        private class TestHttpMessageHandler : HttpMessageHandler
        {
            private HttpResponseMessage _responseMessage;

            public TestHttpMessageHandler(HttpResponseMessage responseMessage)
            {
                _responseMessage = responseMessage;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                return Task.FromResult(_responseMessage);
            }
        }
    }
}
