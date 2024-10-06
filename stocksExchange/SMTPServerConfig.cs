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
}
