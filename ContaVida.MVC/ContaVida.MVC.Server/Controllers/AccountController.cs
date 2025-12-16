using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContaVida.MVC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private IAccountUserService _accountService;
        private readonly IWebHostEnvironment _hostingEnv;
        public AccountController(IAccountUserService accountUserService, IWebHostEnvironment hostingEnv)
        {
            _accountService = accountUserService;
            _hostingEnv = hostingEnv;
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
    }
}
