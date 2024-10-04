namespace stocksExchange
{
    class MonitorStockService
    {
        public static void MonitorStockAndSuggestAction(
            string stockSymbol, float sellPrice, float buyPrice, ref float previousStockPrice, string apiToken, SMTPServer smtpServer
            )
        {
            Task<string> rawStockData = StockApiClient.GetStockDataAsync(stockSymbol, apiToken);
            FilteredStockData filteredStockData = StockApiClient.FilterStockData(rawStockData.Result);

            float stockPrice = filteredStockData.StockPrice;
            string currency = filteredStockData.Currency;


            if (stockPrice >= sellPrice && previousStockPrice != stockPrice)
            {
                string message = $"The price of {stockSymbol} is {stockPrice} {currency}. " +
                $"This is at your set selling price of {sellPrice} {currency}. We advise selling it.";

                if (stockPrice > sellPrice)
                    message = message.Replace("at", "above");

                Console.WriteLine(message);
                smtpServer.SendEmail(message);
            }

            else if (stockPrice <= buyPrice && previousStockPrice != stockPrice)
            {
                string message = $"The price of {stockSymbol} is {stockPrice} {currency}. " +
                $"This is at your set buying price of {buyPrice} {currency}. We advise buying it.";

                if (stockPrice < buyPrice)
                    message = message.Replace("at", "below");

                Console.WriteLine(message);
                smtpServer.SendEmail(message);
            }

            previousStockPrice = stockPrice;
        }
    }
}
