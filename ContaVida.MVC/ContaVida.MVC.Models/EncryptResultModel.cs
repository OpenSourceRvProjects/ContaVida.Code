using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models
{
    public class EncryptResultModel
    {
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string EncodeddPassword { get; set; }
        public string Salt { get; set; }
    }
}
