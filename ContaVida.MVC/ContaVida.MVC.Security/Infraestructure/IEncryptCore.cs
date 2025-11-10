using ContaVida.MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Security.Infraestructure
{
    public interface IEncryptCore
    {
        Task<EncryptResultModel> RunEncrypt(string rawPassword);
    }
}
