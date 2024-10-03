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
                string apiUrl = $"https://brapi.dev/api/quote/{stockSymbol}?interval=1m&token={apiToken}";
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

        public static float ParseStockData(string stockData)
        {
            JsonDocument jsonStockData = JsonDocument.Parse(stockData);
            return jsonStockData.RootElement.GetProperty("results")[0].GetProperty("regularMarketPrice").GetSingle();
        }
    }
}
        