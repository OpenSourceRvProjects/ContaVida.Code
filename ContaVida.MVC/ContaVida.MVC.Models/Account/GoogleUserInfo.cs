using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Account
{
    public class GoogleAuthRequest
    {
        public string IdToken { get; set; }
    }
    public class GoogleUserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string DisplayName { get; set; }
    }
}
