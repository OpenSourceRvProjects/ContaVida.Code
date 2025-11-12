using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.Models.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Services
{
    public class AccountUserService : IAccountUserService
    {
        private ContaVidaDbContext _dbContext;
        public AccountUserService(ContaVidaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StatusPageResponseModel> GetSystemStatus()
        {
            var result = new StatusPageResponseModel
            {
                ConnectionDB = await _dbContext.Users.CountAsync() >= 0,
                ServerDate = DateTime.Now,
            };

            return result;
        }
    }
}
