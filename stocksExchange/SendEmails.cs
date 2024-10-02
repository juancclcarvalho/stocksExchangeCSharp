using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace stocksExchange
{
    class SMTPServer
    {
        private string server;
        private int port;
        private string senderEmail;
        private string password;
        private string receiverEmail;

        public SMTPServer(string server, int port, string senderEmail, string password, string receiverEmail)
        {
            this.server = server;
            this.port = port;
            this.senderEmail = senderEmail;
            this.password = password;
            this.receiverEmail = receiverEmail;
        }

        public void SendEmail(string action, string stockSymbol, float stockPrice)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("StocksApp", this.senderEmail));
            email.To.Add(new MailboxAddress("Investor", this.receiverEmail));

            email.Subject = "Update on your stocks";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"The price of the stock {stockSymbol} is {stockPrice}. You should consider {action} it."
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(this.server, this.port);

                smtp.Authenticate(this.senderEmail, this.password);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}