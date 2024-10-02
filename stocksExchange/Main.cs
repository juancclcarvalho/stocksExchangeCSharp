using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stocksExchange
{
    public class StocksApp
    {
        public static void Main(string[] args)
        {
            string server = "";
            int port = 587;
            string senderEmail = "";
            string password = "";
            string receiverEmail = "";
            SMTPServer smtpServer = new SMTPServer(server, port, senderEmail, password, receiverEmail);

            float previousStockPrice = 0.0f;
            // TODO: read these values from the command line
            string stockSymbol = "PETR4";
            float sellPrice = 22.67f;
            float buyPrice = 22.59f;


            while (true)
            {
                Task<string> stockData = ConsumeAPI.GetStockDataAsync(stockSymbol);
                float stockPrice = ConsumeAPI.ParseStockData(stockData.Result);

                if (stockPrice >= sellPrice && previousStockPrice != sellPrice)
                    smtpServer.SendEmail("selling", stockSymbol, stockPrice);

                else if (stockPrice <= buyPrice && previousStockPrice != sellPrice)
                    smtpServer.SendEmail("buying", stockSymbol, stockPrice);

                previousStockPrice = stockPrice;

                // TODO: look for other ways to run the funciton in time intervals
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
