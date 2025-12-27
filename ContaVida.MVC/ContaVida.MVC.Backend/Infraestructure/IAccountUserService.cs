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
        Task<GoogleUserInfo> VerifyGoogleToken(string idToken);
        Task<LoginTokenDataModel> ExternalVendorLoginAndRetrieveToken(string username);
        Task SendPasswordResetEmail(string email);
        Task<bool> ChangePasswordWithRequestLink(Guid requestID, string newPassword);
        Task<bool> ValidateRecoveryRequestID(Guid requestID);
        Task ChangePassword(string currentPassword, string newPassword);
        Task<LoginTokenDataModel> LoginAndRetrieveTokenForImpersonate(Guid userID);
        public bool GetMaintenancePageFlag();
        public Task SetMaintenacePage(bool showMaintacePage);
        public Task SetMaintenancePageWithKey(MaintenanceKeyInputModel input);
        Task<bool> GetMaintenancePageFromDB();
    }
}
