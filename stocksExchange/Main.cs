using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace stocksExchange
{
    public class SMTPServerConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public string ReceiverEmail { get; set; }
    }
    public class StocksApp
    {
        public static void Main(string[] args)
        {
            string apiToken = Environment.GetEnvironmentVariable("STOCK_API_TOKEN", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(apiToken))
                throw new ArgumentNullException("No API token found");

            string smtpConfigPath = Environment.GetEnvironmentVariable("SMTP_CONFIG_PATH", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(smtpConfigPath))
                throw new ArgumentNullException("No SMTP config path found");

            string strSMPTConfig = File.ReadAllText(smtpConfigPath);
            SMTPServerConfig smtpConfig = JsonSerializer.Deserialize<SMTPServerConfig>(strSMPTConfig);

            SMTPServer smtpServer = new SMTPServer(smtpConfig);


            string stockSymbol = args[0];
            float sellPrice = float.Parse(args[1]);
            float buyPrice = float.Parse(args[2]);

            float previousStockPrice = 0.0f;

            while (true)
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

                // TODO: look for other ways to run the funciton in time intervals
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
