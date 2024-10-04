﻿namespace stocksExchange
{
    class MonitorStockService
    {
        public static void MonitorStockAndSuggestAction(
            string stockSymbol, float sellPrice, float buyPrice, ref float previousStockPrice, string apiToken, SMTPServer smtpServer
            )
        {
            Task<string> stockData = StockApiClient.GetStockDataAsync(stockSymbol, apiToken);
            float stockPrice = StockApiClient.ParseStockData(stockData.Result);

            if (stockPrice >= sellPrice && previousStockPrice != stockPrice)
            {
                Console.WriteLine($"The price of the stock {stockSymbol} is above {sellPrice}. Sending email to sell it.");
                smtpServer.SendEmail("selling", stockSymbol, stockPrice);
            }

            else if (stockPrice <= buyPrice && previousStockPrice != stockPrice)
            {
                Console.WriteLine($"The price of the stock {stockSymbol} is below {buyPrice}. Sending email to buy it.");
                smtpServer.SendEmail("buying", stockSymbol, stockPrice);
            }

            previousStockPrice = stockPrice;
        }
    }
}
