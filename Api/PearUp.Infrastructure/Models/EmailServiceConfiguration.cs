namespace PearUp.Infrastructure
{
    public class EmailServiceConfiguration
    {
        public EmailServiceConfiguration()
        {
            SMTPPort = Constants.EmailConstants.SMTPPort;
            SMTPHost = Constants.EmailConstants.SMTPHost;
            MailSubject = Constants.EmailConstants.MailSubject;
        }
        public string FromPassword { get; set; }
        public string MailFrom { get; set; }
        public string MailSubject { get; set; }
        public string MailTo { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
    }
}
