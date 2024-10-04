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

        public SMTPServer(SMTPServerConfig smtpConfig)
        {
            this.server = smtpConfig.Server;
            this.port = smtpConfig.Port;
            this.senderEmail = smtpConfig.SenderEmail;
            this.password = smtpConfig.Password;
            this.receiverEmail = smtpConfig.ReceiverEmail;
        }

        public void SendEmail(string message)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("StocksApp", this.senderEmail));
            email.To.Add(new MailboxAddress("Investor", this.receiverEmail));

            email.Subject = "Update on your stocks";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
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