using MimeKit;

namespace _DATN____SaleQQ.Common.Config
{
    public class EmailRequest
    {
        public List<MailboxAddress> To { get;set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public EmailRequest(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(p => new MailboxAddress("email", p)));
            Subject = subject;
            Content = content;
        }
    }
}
