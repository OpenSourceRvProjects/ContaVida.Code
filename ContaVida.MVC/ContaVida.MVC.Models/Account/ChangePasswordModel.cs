using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Account
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
