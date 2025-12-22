using ContaVida.MVC.Models.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(MessageModel message);
    }
}
