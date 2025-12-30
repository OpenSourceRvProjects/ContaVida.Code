using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Models.Account;
using ContaVida.MVC.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ContaVida.MVC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private IAccountUserService _accountService;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountUserService accountUserService, IWebHostEnvironment hostingEnv, IConfiguration configuration)
        {
            _accountService = accountUserService;
            _hostingEnv = hostingEnv;
            _configuration = configuration;

        }

        [HttpGet]
        [Route("getSystemStatus")]
        public async Task<IActionResult> GetSystemStatus()
        {
            try
            {
                var response = await _accountService.GetSystemStatus();
                response.Environment = _hostingEnv.EnvironmentName;
                System.IO.File.AppendAllText("fileLog.txt", DateTime.Now.ToString() + Environment.NewLine);

                return Ok(response);

            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("exceptionsLog.txt", DateTime.Now.ToString() + "|" + ex.Message + "." + ex.InnerException?.Message + Environment.NewLine);
                return StatusCode(500, "Error getting health");
            }
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> Post([FromBody] RegisterModel newRegister)
        {
            try
            {
                var result = await _accountService.RegisterUserAccount(newRegister);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel loginModel)
        {
            var token = await _accountService.LoginAndRetrieveToken(loginModel.UserName, loginModel.Password);
            return Ok(token);
        }

        [HttpGet]
        [Route("getGoogleClientID")]
        public IActionResult GetGoogleClientID()
        {
            var clientId = _configuration["security:googleClientID"];
            return Ok(new { googleClientID = clientId });
        }

        [HttpPost]
        [Route("registerGoogleAuth")]
        public async Task<IActionResult> regisgerWithGoogle(GoogleAuthRequest request)
        {
            var googleUser = await _accountService.VerifyGoogleToken(request.IdToken);
            if (googleUser == null)
                return Unauthorized("Invalid Google token");

            string fullName = googleUser.DisplayName ?? googleUser.Name ?? "";
            string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string firstName = nameParts.Length > 0 ? nameParts[0] : "";
            string lastName1 = nameParts.Length > 1 ? nameParts[1] : "";

            var response = await _accountService.RegisterUserAccount(new RegisterModel()
            {
                Email = googleUser.Email,
                Name = firstName,
                LastName1 = lastName1,
                LastName2 = "",
                UserName = googleUser.Email,
            });

            return (Ok(response));
        }

        [HttpPost]
        [Route("loginGoogleAuth")]
        public async Task<IActionResult> loginWithGoogle(GoogleAuthRequest request)
        {
            var googleUser = await _accountService.VerifyGoogleToken(request.IdToken);
            if (googleUser == null)
                return Unauthorized("Invalid Google token");

            var response = await _accountService.ExternalVendorLoginAndRetrieveToken(googleUser.Email);
            return (Ok(response));
        }


        [HttpGet]
        [Route("validateRecoveryRequestID")]
        public async Task<ActionResult> ResetPassword(Guid requestID)
        {
            var isValidID = await _accountService.ValidateRecoveryRequestID(requestID);
            return Ok(isValidID);
        }


        [HttpGet]
        [Route("resetPassword")]
        public async Task<ActionResult> ResetPassword(string email)
        {
            try
            {
                await _accountService.SendPasswordResetEmail(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("changePasswordWithURL")]
        public async Task<ActionResult> ChangePasswordURL(Guid id, string password)
        {
            var result = await _accountService.ChangePasswordWithRequestLink(id, password);
            return Ok(result);
        }

        [HttpPost]
        [Route("changePassword")]
        [LoggedUserDataFilter]
        [ExceptionManager]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            await _accountService.ChangePassword(changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            return Ok();
        }

        [HttpGet]
        [Route("impersonate")]
        [LoggedUserDataFilter]
        public async Task<IActionResult> Impersonate(Guid userID)
        {
            var token = await _accountService.LoginAndRetrieveTokenForImpersonate(userID);
            return Ok(token);
        }


        [HttpGet]
        [Route("maintenancePage")]
        [AllowAnonymous]
        public async Task<IActionResult> MaintenancePage()
        {
            var flag = _accountService.GetMaintenancePageFlag();
            var textFlag = false;
            var dbFlag = false;
            try
            {
                var newTextFlag = System.IO.File.ReadAllLines("maitenancePageValue.txt");
                textFlag = bool.Parse(newTextFlag[0]);
                dbFlag = await _accountService.GetMaintenancePageFromDB();

            }
            catch (Exception ex)
            {
                textFlag = false;
            }

            return Ok(new
            {
                showMaintenancePage = flag || textFlag || dbFlag
            });
        }
    }
}
