using _DATN____SaleQQ.Common.Config;
using MimeKit;
using MailKit.Net.Smtp;

namespace _DATN____SaleQQ.Service.Impl
{
    public class EmailServiceImpl : EmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailServiceImpl(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(EmailRequest emailRequest)
        {
            MimeMessage emailMessage = CreateEmailMessage(emailRequest);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailRequest emailRequest)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(emailRequest.To);
            emailMessage.Subject = emailRequest.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailRequest.Content
            };
            return emailMessage;
        }

        private void Send(MimeMessage mimeMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(mimeMessage);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
