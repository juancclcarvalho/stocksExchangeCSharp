using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer;

namespace stocksExchange
{
    public class StocksApp
    {
        private static Timer _timer;
        private static string _stockSymbol;
        private static float _sellPrice;
        private static float _buyPrice;
        private static float _previousStockPrice = 0.0f;
        private static string _apiToken;
        private static SMTPServer _smtpServer;

        public static void Main(string[] args)
        {
            _apiToken = Environment.GetEnvironmentVariable("STOCK_API_TOKEN", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(_apiToken))
                throw new ArgumentNullException("No API token found");

            string smtpConfigPath = Environment.GetEnvironmentVariable("SMTP_CONFIG_PATH", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(smtpConfigPath))
                throw new ArgumentNullException("No SMTP config path found");

            string strSMPTConfig = File.ReadAllText(smtpConfigPath);
            SMTPServerConfig smtpConfig = JsonSerializer.Deserialize<SMTPServerConfig>(strSMPTConfig);

            _smtpServer = new SMTPServer(smtpConfig);

            _stockSymbol = args[0];
            _sellPrice = float.Parse(args[1]);
            _buyPrice = float.Parse(args[2]);

            MonitorStockService.MonitorStockAndSuggestAction(_stockSymbol, _sellPrice, _buyPrice, ref _previousStockPrice, _apiToken, _smtpServer);

            int queryIntervalMinutes = 30;
            _timer = new Timer(queryIntervalMinutes * 60 * 1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            Console.WriteLine("Press [Enter] to exit the program.");
            Console.ReadLine();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            MonitorStockService.MonitorStockAndSuggestAction(_stockSymbol, _sellPrice, _buyPrice, ref _previousStockPrice, _apiToken, _smtpServer);
        }
    }
}
