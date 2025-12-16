using ContaVida.MVC.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Infraestructure
{
    public interface IAccountUserService
    {
        Task<StatusPageResponseModel> GetSystemStatus();
        Task<RegisterResultModel> RegisterUserAccount(RegisterModel newRegister);
        Task<LoginTokenDataModel> LoginAndRetrieveToken(string username, string password);
    }
}
