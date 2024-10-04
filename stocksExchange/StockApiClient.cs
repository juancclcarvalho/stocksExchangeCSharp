using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace stocksExchange
{
    public class StockApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> GetStockDataAsync(string stockSymbol, string apiToken)
        {
            try
            {
                string apiUrl = $"https://brapi.dev/api/quote/{stockSymbol}?token={apiToken}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public static FilteredStockData FilterStockData(string rawStockData)
        {
            ArgumentNullException.ThrowIfNull(rawStockData);

            JsonDocument parsedStockData = JsonDocument.Parse(rawStockData);
            JsonElement root = parsedStockData.RootElement;

            FilteredStockData filteredStockData = new FilteredStockData();
            filteredStockData.Currency = root.GetProperty("results")[0].GetProperty("currency").GetString();
            filteredStockData.LongName = root.GetProperty("results")[0].GetProperty("longName").GetString();
            filteredStockData.Symbol = root.GetProperty("results")[0].GetProperty("symbol").GetString();
            filteredStockData.StockPrice = root.GetProperty("results")[0].GetProperty("regularMarketPrice").GetSingle();

            return filteredStockData;
        }
    }
}
        