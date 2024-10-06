using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Sockets;

namespace stocksExchange
{
    public class SMTPServer
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

        public bool SendEmail(string message)
        {
            var email = new MimeMessage();

            MailboxAddress from;
            MailboxAddress to;
            try
            {
                from = new MailboxAddress("StocksApp", this.senderEmail);
                to = new MailboxAddress("Investor", this.receiverEmail);
            }
            catch (ParseException)
            {
                Console.WriteLine("Sender or receiver email address is not valid");
                return false;
            }
            catch (SmtpCommandException)
            {
                Console.WriteLine("No email address passed from the config");
                return false;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not load email addresses");
                return false;
            }

            email.From.Add(from);
            email.To.Add(to);

            email.Subject = "Update on your stocks";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(this.server, this.port);

                    smtp.Authenticate(this.senderEmail, this.password);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Server address is empty");
                return false;
            }
            catch (SocketException)
            {
                Console.WriteLine("Could not connect to the server");
                return false;
            }
            catch (AuthenticationException)
            {
                Console.WriteLine("Could not authenticate with the server");
                return false;
            }

            return true;
        }
    }
}