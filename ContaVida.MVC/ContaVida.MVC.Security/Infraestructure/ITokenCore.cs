using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Security.Infraestructure
{
    public interface ITokenCore
    {
        string RunTokenGeneration(List<KeyValuePair<string, string>> tokenInfo, Guid userID);
    }
}
