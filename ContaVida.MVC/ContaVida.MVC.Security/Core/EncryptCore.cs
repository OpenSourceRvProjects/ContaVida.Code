using ContaVida.MVC.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContaVida.MVC.Security.Core
{
    public class EncryptCore : Infraestructure.IEncryptCore
    {
        //NOTE:  FOR THIS ADD Microsoft.Extensions.Configuration FROM NUGET
        private IConfiguration _configuration;

        public EncryptCore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<EncryptResultModel> RunEncrypt(string rawPassword)
        {

            try
            {
                var secretKey = _configuration["security:passwordPrivateKey"];
                var saltValue = Guid.NewGuid().ToString() + "_" + DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString();
                var saltBuffer = Encoding.UTF8.GetBytes(saltValue);
                byte[] clearBytes = Encoding.Unicode.GetBytes(rawPassword);


                string passwordHash = String.Empty;

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, saltBuffer, 1000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            await cs.WriteAsync(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        passwordHash = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return new EncryptResultModel() { EncodeddPassword = passwordHash, Salt = saltValue };
            }
            catch (Exception ex)
            {
                return new EncryptResultModel() { EncodeddPassword = "", ErrorMessage = ex.Message, IsError = true };
            }

        }
    }
}
