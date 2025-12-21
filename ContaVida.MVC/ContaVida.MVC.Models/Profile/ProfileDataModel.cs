using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Profile
{
    public class ProfileDataModel
    {
        public string Name { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool AllowAccess { get; set; }
    }
}
