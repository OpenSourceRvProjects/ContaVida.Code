using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Email
{
    public class MessageModel
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public MessageModel(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(subject, x)));
            Subject = subject;
            Content = content;
        }
    }
}
