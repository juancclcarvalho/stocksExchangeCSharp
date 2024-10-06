namespace stocksExchange
{
    public class MonitorStockService
    {
        public static bool MonitorStockAndSuggestAction(
            string stockSymbol, float sellPrice, float buyPrice, ref float previousStockPrice, string apiToken, SMTPServer smtpServer
            )
        {
            Task<string> rawStockData = StockApiClient.GetStockDataAsync(stockSymbol, apiToken);
            FilteredStockData filteredStockData = StockApiClient.FilterStockData(rawStockData.Result);

            float stockPrice = filteredStockData.StockPrice;
            string currency = filteredStockData.Currency;

            // No reason to send an email. Return true so main can keep running the monitor
            if ((stockPrice < sellPrice && stockPrice > buyPrice) || stockPrice == previousStockPrice)
                return true;

            string message = $"The price of {stockSymbol} is {stockPrice} {currency}. ";

            if (stockPrice >= sellPrice)
            {
                message += $"This is at your set selling price of {sellPrice} {currency}. We advise selling it.";
                if (stockPrice > sellPrice)
                    message = message.Replace("at", "above");
            }

            else if (stockPrice <= buyPrice)
            {
                message += $"This is at your set buying price of {buyPrice} {currency}. We advise buying it.";
                if (stockPrice < buyPrice)
                    message = message.Replace("at", "below");
            }

            Console.WriteLine(message);
            bool emailSent = smtpServer.SendEmail(message);

            previousStockPrice = stockPrice;

            return emailSent;
        }
    }
}
