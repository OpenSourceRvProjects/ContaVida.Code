using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Security.Infraestructure
{
    public interface IDecryptCore
    {
        Task<bool> ValidatePassword(string hashedPassword, string salt, string rawPassword);
    }
}
