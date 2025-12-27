using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaVida.MVC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [LoggedUserDataFilter]

    public class RelapsesController : ControllerBase
    {
        private readonly IRelapseService _relapseService;
        public RelapsesController(IRelapseService relapseService)
        {
            _relapseService = relapseService;
        }
        // GET api/<RelapsesController>/5
        [HttpGet]
        [Route("getEventCounterRelapses")]
        public async Task<IActionResult> Get(Guid eventCounterId)
        {
            var relapses = await _relapseService.GetEventRelapses(eventCounterId);
            return Ok(relapses);
        }

        [HttpGet]
        [Route("getRelapseReasons")]
        public IActionResult GetRelapseReasons()
        {
            var relapseReasons = _relapseService.GetRelapseReasons();
            return Ok(relapseReasons);
        }

    }
}
