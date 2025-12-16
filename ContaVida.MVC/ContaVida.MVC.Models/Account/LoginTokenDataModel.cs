using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Account
{
    public class LoginTokenDataModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsSysAdmin { get; set; }
    }
}
