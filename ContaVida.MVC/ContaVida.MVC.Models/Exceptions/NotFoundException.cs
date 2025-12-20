using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
