using ContaVida.MVC.Security.Infraestructure;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ContaVida.MVC.Security.Core
{
    public class DecryptCore : IDecryptCore
    {

        private IConfiguration _configuration;

        public DecryptCore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> ValidatePassword(string hashedPassword, string salt, string rawPassword)
        {
            var decryptedPassword = await Decrypt(hashedPassword, salt);
            return decryptedPassword == rawPassword;
        }

        private async Task<string> Decrypt(string hashedPassword, string salt)
        {
            try
            {
                //string secretKey = UtilService.GetAppSettingsConfiguration("security", "passwordPrivateKey");
                string secretKey = _configuration["security:passwordPrivateKey"];
                var saltBuffer = Encoding.UTF8.GetBytes(salt);
                hashedPassword = hashedPassword.Replace(" ", "+");
                string password = string.Empty;
                byte[] cipherBytes = Convert.FromBase64String(hashedPassword);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, saltBuffer, 1000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            await cs.WriteAsync(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        password = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return password;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
