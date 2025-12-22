using ContaVida.MVC.Models.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace ContaVida.MVC.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigurationModel _emailConfig;
        public EmailSender(EmailConfigurationModel emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(MessageModel message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MessageModel message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ContaVida App", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format(message.Content) };


            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    //log an error message or throw an exception or both.
                    throw ex;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
