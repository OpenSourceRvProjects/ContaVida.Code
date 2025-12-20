using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message) : base(message)
        {
        }
    }
}
