using System.Text.Json;

namespace stocksExchange.Tests
{
    public class MonitorStockServiceTests
    {
        [Fact]
        public void MonitorStockAndSuggestAction_ReturnsTrue_WhenThereAreNoEmailErrors()
        {
            // Arrange
            string stockSymbol = "AAPL";
            float sellPrice = 150.0f;
            float buyPrice = 100.0f;
            float previousStockPrice = 0.0f;
            string apiToken = Environment.GetEnvironmentVariable("STOCK_API_TOKEN", EnvironmentVariableTarget.User);

            string smtpConfigPath = Environment.GetEnvironmentVariable("SMTP_CONFIG_PATH", EnvironmentVariableTarget.User);

            string strSMPTConfig = File.ReadAllText(smtpConfigPath);
            SMTPServerConfig smtpConfig = JsonSerializer.Deserialize<SMTPServerConfig>(strSMPTConfig);

            SMTPServer smtpServer = new SMTPServer(smtpConfig);

            // Act
            bool result = MonitorStockService.MonitorStockAndSuggestAction(
                stockSymbol, sellPrice, buyPrice, ref previousStockPrice, apiToken, smtpServer
            );

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MonitorStockAndSuggestAction_ReturnsFalse_WhenCannotSendEmail()
        {
            // Arrange
            string stockSymbol = "AAPL";
            float sellPrice = 150.0f;
            float buyPrice = 100.0f;
            float previousStockPrice = 0.0f;
            string apiToken = Environment.GetEnvironmentVariable("STOCK_API_TOKEN", EnvironmentVariableTarget.User);

            string smtpConfigPath = Environment.GetEnvironmentVariable("SMTP_CONFIG_PATH", EnvironmentVariableTarget.User);

            string strSMPTConfig = File.ReadAllText(smtpConfigPath);
            SMTPServerConfig smtpConfig = JsonSerializer.Deserialize<SMTPServerConfig>(strSMPTConfig);

            smtpConfig.SenderEmail = "";
            SMTPServer smtpServer = new SMTPServer(smtpConfig);

            // Act
            bool result = MonitorStockService.MonitorStockAndSuggestAction(
                stockSymbol, sellPrice, buyPrice, ref previousStockPrice, apiToken, smtpServer
            );

            // Assert
            Assert.False(result);
        }
    }
}