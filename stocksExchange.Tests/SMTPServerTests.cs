using System.Text.Json;

namespace stocksExchange.Tests
{
    public class SMTPServerTests
    {
        // setup the configuration for the SMTP server
        private static SMTPServerConfig GetSMTPServerConfig()
        {
            string smtpConfigPath = Environment.GetEnvironmentVariable("SMTP_CONFIG_PATH", EnvironmentVariableTarget.User);
            string strSMPTConfig = File.ReadAllText(smtpConfigPath);
            return JsonSerializer.Deserialize<SMTPServerConfig>(strSMPTConfig);
        }

        [Fact]
        public void SendEmail_SendEmailWithCorrectContent()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.True(res);
        }

        [Fact]
        public void SendEmail_ReturnsFalse_WhenEmailIsEmpty()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();
            smtpConfig.SenderEmail = "";

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void SendEmail_ReturnsFalse_WhenEmailIsInvalid()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();
            smtpConfig.SenderEmail = "invalid_email@";

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void SendEmail_ReturnsFalse_WhenServerIsEmpty()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();
            smtpConfig.Server = "";

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void SendEmail_ReturnsFalse_WhenServerIsInvalid()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();
            smtpConfig.Server = "smtp.invalid.com";

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void SendEmail_ReturnsFalse_WhenPortIsInvalid()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();
            smtpConfig.Port = 1;

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void SendEmail_ReturnsFalse_WhenAuthenticationFails()
        {
            // Arrange
            var smtpConfig = GetSMTPServerConfig();
            smtpConfig.Password = "";

            var smtpServer = new SMTPServer(smtpConfig);
            var message = "This is a test message.";

            // Act
            var res = smtpServer.SendEmail(message);

            // Assert
            Assert.False(res);
        }
    }
}