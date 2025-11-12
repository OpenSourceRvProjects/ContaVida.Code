using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Account
{
    public class StatusPageResponseModel
    {
        public bool ConnectionDB { get; set; }
        public DateTime ServerDate { get; set; }
        public string Environment { get; set; }
    }
}
