using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Models.Account;
using ContaVida.MVC.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaVida.MVC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        // GET: api/<AdminController>
        private IAccountUserService _accountService;
        public AdminController(IAccountUserService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [LoggedUserDataFilter]
        [Route("getAllUsers")]
        public async Task<IActionResult> Get()
        {
            var users = await _accountService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [LoggedUserDataFilter]
        [Route("setMaintenancePage")]
        public async Task<IActionResult> setMaintenancePage(bool showMaintenancePage)
        {
            try
            {
                await _accountService.SetMaintenacePage(showMaintenancePage);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMsg = ex.Message });
            }
        }


        [HttpPost]
        [Route("turnMaintenancePageWithKey")]
        [AllowAnonymous]
        public async Task<IActionResult> TurnMaintenancePageWithKey([FromBody] MaintenanceKeyInputModel input)
        {
            try
            {
                await _accountService.SetMaintenancePageWithKey(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMsg = ex.Message });
            }
        }

    }
}
