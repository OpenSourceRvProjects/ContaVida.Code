using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Account
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
    }
}
