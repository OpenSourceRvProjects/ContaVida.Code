using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ContaVida.MVC.Security.Infraestructure;

namespace ContaVida.MVC.Backend.Services
{
    public class AccountUserService : IAccountUserService
    {
        private ContaVidaDbContext _dbContext;
        private IHttpContextAccessor _accessor;
        private IEncryptCore _encryptCore;
        private IDecryptCore _decryptCore;
        private ITokenCore _tokenCore;

        public AccountUserService(ContaVidaDbContext dbContext, IHttpContextAccessor accessor, 
            IEncryptCore encryptCore, IDecryptCore decryptCore, ITokenCore tokenCore)
        {
            _dbContext = dbContext;
            _encryptCore = encryptCore;
            _accessor = accessor;
            _decryptCore = decryptCore;
            _tokenCore = tokenCore;
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

        public async Task<RegisterResultModel> RegisterUserAccount(RegisterModel newRegister)
        {
            var anonimizedRequest = new RegisterModel()
            {
                Email = newRegister.Email,
                LastName1 = newRegister.LastName1,
                LastName2 = newRegister.LastName2,
                Name = newRegister.Name,
                Password = "******************"
            };

            var newRequest = new SignUpRequest()
            {
                Id = Guid.NewGuid(),
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                RequestObject = JsonSerializer.Serialize(anonimizedRequest),
                CreationDate = DateTime.Now,
            };

            var result = new RegisterResultModel();
            var user = await _dbContext.Users.FirstOrDefaultAsync(f => f.UserName == newRegister.UserName || f.Email == newRegister.Email);

            if (user != null)
            {
                _dbContext.Add(newRequest);
                _dbContext.SaveChanges();
                throw new Exception("User already exist");
            }

            var encryptResult = await _encryptCore.RunEncrypt(newRegister.Password);
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = newRegister.UserName,
                PasswordHash = encryptResult.EncodeddPassword,
                Email = newRegister.Email,
                Salt = !string.IsNullOrWhiteSpace(encryptResult.Salt) ? encryptResult.Salt : "",
                CreationDate = DateTime.Now,
            };

            var newProfileUser = new PersonalProfile()
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                Name = newRegister.Name,
                LastName1 = newRegister.LastName1,
                LastName2 = newRegister.LastName2,
                UserId = newUser.Id,

            };

            try
            {
                newRequest.UserId = newUser.Id;
                _dbContext.Add(newUser);
                _dbContext.Add(newProfileUser);
                _dbContext.Add(newRequest);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            string token = GenerateToken(newUser, newProfileUser);
            result.UserToken = token;

            return result;
        }

        private string GenerateToken(User newUser, PersonalProfile newProfileUser)
        {
            var tokenInfo = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("userID", newUser.Id.ToString()),
                    new KeyValuePair<string, string>("name", newProfileUser.Name),
                    new KeyValuePair<string, string>("userName", newUser.UserName),
                    new KeyValuePair<string, string>("tokenServerCreationDate", DateTime.Now.ToString()),
                    new KeyValuePair<string, string>("allowSysAdminAccess", newUser.AllowSysAdminAccess.ToString()),
                };
            var token = _tokenCore.RunTokenGeneration(tokenInfo, newUser.Id);
            return token;
        }

    }
}
